using Lion.Abstractions;
using Lion.Server;
using Lion.Server.Hypermedia;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LionServiceCollectionExtensions
    {
        public static ILionServerBuilder AddLionServer(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(LionServiceCollectionExtensions));

            services.AddScoped<BundleCollectionHypermediaService>();
            services.AddScoped<IHypermediaService, BundleCollectionHypermediaService>(provider => provider.GetRequiredService<BundleCollectionHypermediaService>());

            services.AddScoped<BundleHypermediaService>();
            services.AddScoped<IHypermediaService, BundleHypermediaService>(provider => provider.GetRequiredService<BundleHypermediaService>());

            services.AddScoped<HypermediaResultFilter>();

            return new LionServerBuilder(services);
        }
    }
}
