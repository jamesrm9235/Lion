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
    public sealed class BundleStore : IBundleStore
    {
        private readonly string connectionString;

        public BundleStore(IOptions<LionOptions> options)
        {
            connectionString = options.Value.ConnectionString;
        }

        public async Task<long> AddBundleAsync(Bundle bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }

            try
            {
                using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync().ConfigureAwait(false);

                using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

                var id = await connection.QueryFirstAsync<int>(
                    sql: @"INSERT INTO [Lion].[Bundles] ([Key],[NamespaceId])
                           VALUES (@key,@namespaceId);
                           SELECT CAST (SCOPE_IDENTITY() AS INT);",
                    param: new
                    {
                        key = bundle.Key,
                        namespaceId = bundle.Namespace.Id
                    },
                    transaction: transaction).ConfigureAwait(false);

                foreach (var message in bundle.Messages)
                {
                    await connection.ExecuteAsync(
                        sql: @"INSERT INTO [Lion].[Messages] ([BundleId],[Language],[Value],[Comment])
                            VALUES (@bundleId,@language,@value,@comment);",
                        param: new
                        {
                            bundleId = id,
                            language = message.Language,
                            value = message.Value,
                            comment = message.Comment
                        },
                        transaction: transaction).ConfigureAwait(false);
                }

                transaction.Commit();

                return id;
            }
            catch (SqlException e) when (e.Number == 2627) // unique constraint violation
            {
                throw new NameUnavailableException($"The key, {bundle.Key}, is already in use by another bundle. Bundle keys must be unique within a namespace.", e);
            }
        }

        public async Task<long> AddMessageAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: @"INSERT INTO [Lion].[Messages] ([BundleId],[Language],[Value],[Comment])
                    VALUES (@bundleId,@language,@value,@comment);",
                param: new { bundleId = message.BundleId, language = message.Language, value = message.Value, comment = message.Comment },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();

            return message.BundleId;
        }

        public async Task DeleteBundleAsync(Bundle bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection .BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: @"DELETE FROM [Lion].[Bundles] WHERE [Id] = @id;",
                param: new { id = bundle.Id },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }

        public async Task DeleteMessageAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection .BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: @"DELETE FROM [Lion].[Messages] WHERE [BundleId] = @bundleId AND [Language] = @language;",
                param: new { bundleId = message.BundleId, language = message.Language },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }

        public async Task<Bundle> GetBundleAsync(long id)
        {
            using var connection = new SqlConnection(connectionString);

            Bundle bundle = null;

            var result = await connection.QueryAsync<Namespace, Bundle, Message, Bundle>(
                sql: @"SELECT
                    n.[Id]
                    ,n.[Key]
                    ,b.[Id]
                    ,b.[Key]
                    ,m.[Language]
                    ,m.[Value]
                    ,m.[Comment]
                    ,m.[CreatedAt]
                    ,m.[ModifiedAt]
                FROM [Lion].[Bundles] AS b
                JOIN [Lion].[Namespaces] AS n
                ON n.[Id] = b.[NamespaceId]
                LEFT JOIN [Lion].[Messages] AS m
                ON b.[Id] = m.[BundleId]
                WHERE
                    b.[Id] = @id;",
                map: (currentNamespace, currentBundle, currentMessage) =>
                {
                    if (bundle == null)
                    {
                        bundle = currentBundle;
                    }

                    if (bundle.Namespace == null)
                    {
                        bundle.Namespace = new Namespace { Id = currentNamespace.Id, Key = currentNamespace.Key };
                    } 

                    if (currentMessage != null)
                    {
                        currentMessage.BundleId = bundle.Id;
                        bundle.Messages.Add(currentMessage);
                    }
                    return bundle;
                },
                param: new { id = id },
                splitOn: "Id, Language").ConfigureAwait(false);

            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Bundle>> ListBundlesAsync(long cursor, int limit, string @namespace = "")
        {
            var query = new StringBuilder(
                @"SELECT
                    n1.[Id]
                    ,n1.[Key]
                    ,b1.[Id]
                    ,b1.[Key]
                    ,m1.[Language]
                    ,m1.[Value]
                    ,m1.[Comment]
                    ,m1.[CreatedAt]
                    ,m1.[ModifiedAt]
                FROM [Lion].[Bundles] AS b1
                JOIN [Lion].[Namespaces] AS n1
                ON n1.[Id] = b1.[NamespaceId]
                LEFT JOIN [Lion].[Messages] AS m1
                ON b1.[Id] = m1.[BundleId]
                WHERE
                    b1.[Id] IN (SELECT TOP (@limit) b2.[Id] FROM [Lion].[Bundles] AS b2 ");

            if (!string.IsNullOrEmpty(@namespace))
            {
                query.Append(
                    @"JOIN [Lion].[Namespaces] AS n2
                    ON b2.[NamespaceId] = n2.[Id]
                    WHERE n2.[Key] = @namespace ");
                if (cursor > 0)
                {
                    query.Append("AND b2.[Id] <= @cursor ");
                } 
            }
            else if (cursor > 0)
            {
                query.Append("WHERE b2.[Id] <= @cursor ");
            }

            query.Append("ORDER BY b2.[Id] DESC);");

            using var connection = new SqlConnection(connectionString);

            var bundles = new Dictionary<long, Bundle>();

            var result = await connection.QueryAsync<Namespace, Bundle, Message, Bundle>(
                sql: query.ToString(),
                map: (currentNamespace, currentBundle, currentMessage) =>
                {
                    if (!bundles.TryGetValue(currentBundle.Id, out var bundle))
                    {
                        bundle = currentBundle;
                        bundles.Add(currentBundle.Id, currentBundle);
                    }

                    if (bundle.Namespace == null)
                    {
                        bundle.Namespace = new Namespace { Id = currentNamespace.Id, Key = currentNamespace.Key };
                    } 

                    if (currentMessage != null)
                    {
                        currentMessage.BundleId = bundle.Id;
                        bundle.Messages.Add(currentMessage);
                    }

                    return bundle;
                },
                param: new { cursor = cursor, limit = limit, @namespace = @namespace },
                splitOn: "Id, Language").ConfigureAwait(false);

            return result.Distinct().ToList().AsReadOnly();
        }

        public async Task UpdateBundleAsync(Bundle bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }
            try
            {
                using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync().ConfigureAwait(false);

                using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

                await connection.ExecuteAsync(
                    sql: "UPDATE [Lion].[Bundles] SET [Key] = @key WHERE [Id] = @id;",
                    param: new { id = bundle.Id, key = bundle.Key },
                    transaction: transaction).ConfigureAwait(false);

                transaction.Commit();
            }
            catch (SqlException e) when (e.Number == 2627) // unique constraint violation
            {
                throw new NameUnavailableException($"'{bundle.Key}' is already in use by another bundle. Bundle keys must be unique within a namespace.", e);
            }
        }

        public async Task UpdateMessageAsync(Message message)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: @"UPDATE [Lion].[Messages]
                       SET [Value] = @Value, [ModifiedAt] = @ModifiedAt
                       WHERE [BundleId] = @bundleId AND [Language] = @language;",
                param: new
                {
                    bundleId = message.BundleId,
                    language = message.Language,
                    modifiedAt = DateTime.UtcNow,
                    value = message.Value,
                },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }
    }
}
