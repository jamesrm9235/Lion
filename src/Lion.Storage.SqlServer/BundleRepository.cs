using Dapper;
using Lion.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Storage.SqlServer
{
    public sealed class BundleRepository : IBundleRepository
    {
        private readonly string connectionString;

        public BundleRepository(IOptions<LionOptions> options)
        {
            connectionString = options.Value.ConnectionString;
        }

        public async Task<long> AddBundleAsync(Bundle bundle)
        {
            try
            {
                if (bundle == null)
                {
                    throw new ArgumentNullException(nameof(bundle));
                }

                using var connection = new SqlConnection(connectionString);

                await connection.OpenAsync().ConfigureAwait(false);

                using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

                var id = await connection.QueryFirstAsync<int>(
                    sql: @"INSERT INTO [Lion].[Tags] ([Bundle],[Namespace],[Comment])
                           VALUES (@Bundle,@Namespace,@Comment);
                           SELECT CAST (SCOPE_IDENTITY() AS INT);",
                    param: new
                    {
                        Bundle = bundle.Name,
                        Comment = bundle.Comment,
                        Namespace = bundle.Namespace
                    },
                    transaction: transaction).ConfigureAwait(false);

                foreach (var message in bundle.Messages)
                {
                    var messageId = await connection.QueryFirstAsync<int>(
                        sql: @"INSERT INTO [Lion].[Messages] ([Language],[Value]) VALUES (@Language,@Value);
                               SELECT CAST (SCOPE_IDENTITY() AS INT);",
                        param: new { Language = message.Language, Value = message.Value },
                        transaction: transaction).ConfigureAwait(false);
                    await connection.ExecuteAsync(
                        sql: "INSERT INTO [Lion].[MessageTags] ([MessageId],[BundleId]) VALUES (@MessageId,@BundleId);",
                        param: new { MessageId = messageId, BundleId = id },
                        transaction: transaction).ConfigureAwait(false);
                }

                transaction.Commit();

                return id;
            }
            catch (SqlException e) when (e.Number == 2627) // unique constraint violation
            {
                throw new BundleNameUnavailableException(
                    $"Bundle names must be unique within a namespace. There is already a bundle called '{bundle.Name}' in '{bundle.Namespace}' namespace.", e);
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

            var id = await connection.QueryFirstAsync<int>(
                sql: @"INSERT INTO [Lion].[Messages] ([Language],[Value]) VALUES (@Language,@Value);
                        SELECT CAST (SCOPE_IDENTITY() AS INT);",
                param: new { Language = message.Language, Value = message.Value },
                transaction: transaction).ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: "INSERT INTO [Lion].[MessageTags] ([MessageId],[BundleId]) VALUES (@MessageId,@BundleId);",
                param: new { BundleId = message.BundleId, MessageId = id },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();

            return id;
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
                sql: @"DELETE FROM [Lion].[Messages] WHERE [MessageId] IN (SELECT [MessageId] FROM [Lion].[MessageTags] WHERE [BundleId] = @BundleId);
                       DELETE FROM [Lion].[Tags] WHERE [BundleId] = @BundleId;",
                param: new { BundleId = bundle.BundleId },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }

        public async Task<Bundle> GetBundleAsync(long id)
        {
            using var connection = new SqlConnection(connectionString);

            Bundle bundle = null;

            var result = await connection.QueryAsync<Bundle, Message, Bundle>(
                sql: @"SELECT
                           t.[BundleId]
                           ,t.[Namespace]
                           ,t.[Bundle] AS [Name]
                           ,t.[CreatedAt]
                           ,m.[MessageId]
                           ,m.[Language]
                           ,m.[Value]
                       FROM [Lion].[Tags] AS t
                       LEFT JOIN [Lion].[MessageTags] AS mt
                       INNER JOIN [Lion].[Messages] AS m
                       ON mt.[MessageId] = m.[MessageId]
                       ON mt.[BundleId] = t.[BundleId]
                       WHERE
                           t.[BundleId] = @BundleId;",
                map: (currentBundle, currentMessage) =>
                {
                    if (bundle == null)
                    {
                        bundle = currentBundle;
                    }
                    if (currentMessage != null)
                    {
                        currentMessage.BundleId = bundle.BundleId;
                        bundle.Messages.Add(currentMessage);
                    }
                    return bundle;
                },
                param: new { BundleId = id },
                splitOn: "MessageId").ConfigureAwait(false);

            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Bundle>> ListBundlesAsync(long cursor, int limit)
        {
            var query = new StringBuilder(
                @"SELECT
                    t.[BundleId]
                    ,t.[Namespace]
                    ,t.[Bundle] AS [Name]
                    ,t.[CreatedAt]
                    ,m.[Language]
                    ,m.[Value]
                FROM [Lion].[Tags] AS t
                LEFT JOIN [Lion].[MessageTags] AS mt
                INNER JOIN [Lion].[Messages] AS m
                ON mt.[MessageId] = m.[MessageId]
                ON mt.[BundleId] = t.[BundleId]
                WHERE
                    t.[BundleId] IN (SELECT TOP (@Limit) [BundleId] FROM [Lion].[Tags] ");
            if (cursor > 0)
            {
                query.Append("WHERE [BundleId] <= @Cursor ");
            }
            query.Append("ORDER BY [BundleId] DESC);");

            using var connection = new SqlConnection(connectionString);

            var bundles = new Dictionary<long, Bundle>();

            var result = await connection.QueryAsync<Bundle, Message, Bundle>(
                sql: query.ToString(),
                map: (currentBundle, currentMessage) =>
                {
                    if (!bundles.TryGetValue(currentBundle.BundleId, out var bundle))
                    {
                        bundle = currentBundle;
                        bundles.Add(currentBundle.BundleId, currentBundle);
                    }
                    if (currentMessage != null)
                    {
                        bundle.Messages.Add(currentMessage);
                    }
                    return bundle;
                },
                param: new { Cursor = cursor, Limit = limit },
                splitOn: "Language").ConfigureAwait(false);

            return result.Distinct().ToList().AsReadOnly();
        }

        public async Task UpdateMessageAsync(Message message)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            using var transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(
                sql: @"UPDATE [Lion].[Messages]
                       SET [Language] = @Language, [Value] = @Value
                       WHERE [MessageId] = @MessageId;",
                param: new
                {
                    Language = message.Language,
                    Value = message.Value,
                    MessageId = message.MessageId
                },
                transaction: transaction).ConfigureAwait(false);

            transaction.Commit();
        }
    }
}
