using Lion.Common;
using Lion.Server;
using Lion.Server.Hypermedia;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LionServiceCollectionExtensions
    {
        public static ILionServerBuilder AddLionServer(this IServiceCollection services)
        {
            services.AddRequiredPlatformServices();

            services.AddAutoMapper(typeof(LionServiceCollectionExtensions));

            services.AddHypermediaServices();

            return new LionServerBuilder(services);
        }

        private static IServiceCollection AddRequiredPlatformServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            return services;
        }

        private static IServiceCollection AddHypermediaServices(this IServiceCollection services)
        {
            services.AddScoped<BundleCollectionHypermediaService>();
            services.AddScoped<IHypermediaService, BundleCollectionHypermediaService>(provider => provider.GetRequiredService<BundleCollectionHypermediaService>());

            services.AddScoped<BundleHypermediaService>();
            services.AddScoped<IHypermediaService, BundleHypermediaService>(provider => provider.GetRequiredService<BundleHypermediaService>());

            services.AddScoped<CompactBundleCollectionHypermediaService>();
            services.AddScoped<IHypermediaService, CompactBundleCollectionHypermediaService>(provider => provider.GetRequiredService<CompactBundleCollectionHypermediaService>());

            services.AddScoped<MessageHypermediaService>();
            services.AddScoped<IHypermediaService, MessageHypermediaService>(provider => provider.GetRequiredService<MessageHypermediaService>());

            services.AddScoped<NamespaceCollectionHypermediaService>();
            services.AddScoped<IHypermediaService, NamespaceCollectionHypermediaService>(provider => provider.GetRequiredService<NamespaceCollectionHypermediaService>());

            services.AddScoped<NamespaceHypermediaService>();
            services.AddScoped<IHypermediaService, NamespaceHypermediaService>(provider => provider.GetRequiredService<NamespaceHypermediaService>());

            services.AddScoped<HypermediaResultFilter>();

            return services;
        }
    }
}
