using Dapper;
using Lion.Common;
using Lion.Common.Entities;
using Lion.Common.Exceptions;
using Lion.Common.Storage;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Storage.SqlServer
{
    public sealed class NamespaceStore : INamespaceStore
    {
        private readonly string connectionString;

        public NamespaceStore(IOptions<LionOptions> options)
        {
            connectionString = options.Value.ConnectionString;
        }

        public async Task<long> AddNamespaceAsync(Namespace @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            try
            {
                using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync().ConfigureAwait(false);

                using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

                var id = await connection.QueryFirstAsync<int>(
                    sql: "INSERT INTO [Lion].[Namespaces] ([Key]) VALUES (@key); SELECT CAST (SCOPE_IDENTITY() AS INT);",
                    param: new { key = @namespace.Key },
                    transaction: transaction).ConfigureAwait(false);

                transaction.Commit();

                return id;
            }
            catch (SqlException e) when (e.Number == 2627) // unique constraint violation
            {
                throw new NameUnavailableException($"The key, '{@namespace.Key}', is already in use by another namespace. Namespace keys must be globally unique.", e);
            }
        }

        public async Task DeleteNamespaceAsync(Namespace @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: "DELETE FROM [Lion].[Namespaces] WHERE [Id] = @id;",
                param: new { id = @namespace.Id },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }

        public async Task<Namespace> GetNamespaceAsync(long id)
        {
            using var connection = new SqlConnection(connectionString);

            var reader = await connection.QueryMultipleAsync(
                    sql: @"SELECT [Id], [Key] FROM [Lion].[Namespaces] WHERE [Id] = @id;
                        SELECT DISTINCT [Language]
                        FROM [Lion].[Messages] AS m
                        JOIN [Lion].[Bundles] AS b
                        ON m.[BundleId] = b.[Id]
                        WHERE b.[NamespaceId] = @id;",
                    param: new { id }).ConfigureAwait(false);

            var namespaceResults = await reader.ReadAsync<Namespace>().ConfigureAwait(false);
            var languagesResults = await reader.ReadAsync<string>().ConfigureAwait(false);

            var @namespace = namespaceResults.FirstOrDefault();
            if (@namespace == null)
            {
                return null;
            }
            else
            {
                @namespace.Languages = languagesResults.ToList();
            }

            return @namespace;
        }

        public async Task<IReadOnlyList<Namespace>> ListNamespacesAsync(long cursor, int limit)
        {
            using var connection = new SqlConnection(connectionString);
            var query = new StringBuilder("SELECT TOP(@limit) [Id], [Key] FROM [Lion].[Namespaces] ");
            if (cursor > 0)
            {
                query.Append("WHERE [Id] <= @cursor ");
            }
            query.Append("ORDER BY [Id] DESC;");

            var result = await connection.QueryAsync<Namespace>(query.ToString(), new { cursor, limit }).ConfigureAwait(false);

            return result.ToList();
        }

        public async Task UpdateNamespaceAsync(Namespace @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }
            try
            {
                using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync().ConfigureAwait(false);

                using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

                await connection.ExecuteAsync(
                    sql: "UPDATE [Lion].[Namespaces] SET [Key] = @key WHERE [Id] = @id;",
                    param: new { id = @namespace.Id, key = @namespace.Key },
                    transaction: transaction).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (SqlException e) when (e.Number == 2627) // unique constraint violation
            {
                throw new NameUnavailableException($"'{@namespace.Key}' is already in use by another namespace. Namespaces must be globally unique.", e);
            }
        }
    }
}
