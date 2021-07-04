using Lion.Common;
using Lion.Common.Storage;
using Lion.Storage.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Lion.Server
{
    public static class LionServerBuilderExtensions
    {
        public static ILionServerBuilder AddSqlServer(this ILionServerBuilder builder, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            } 

            builder.Services.Configure<LionOptions>(opt => opt.ConnectionString = connectionString);

            builder.Services.TryAddScoped<IBundleStore, BundleStore>();

            builder.Services.TryAddScoped<INamespaceStore, NamespaceStore>();

            return builder;
        }
    }
}
