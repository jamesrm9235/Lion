using Lion.Abstractions;
using Lion.Storage.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lion.Server
{
    public static class LionServerBuilderExtensions
    {
        public static ILionServerBuilder AddSqlServer(this ILionServerBuilder builder, string connectionString)
        {
            builder.Services.Configure<LionOptions>(opt => opt.ConnectionString = connectionString);

            builder.Services.TryAddScoped<IBundleRepository, BundleRepository>();

            return builder;
        }
    }
}
