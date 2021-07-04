using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Text;

namespace Lion.Server.Hypermedia
{
    public sealed class BundleCollectionHypermediaService : CollectionHypermediaService<Bundle>
    {
        private readonly BundleHypermediaService bundleHypermediaService;

        public BundleCollectionHypermediaService(
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, BundleHypermediaService bundleHypermediaService)
            : base(httpContextAccessor, linkGenerator)
        {
            this.bundleHypermediaService = bundleHypermediaService ?? throw new ArgumentNullException(nameof(bundleHypermediaService));
        }

        protected override int Limit => Endpoints.BundlesController.ListQuery.DefaultLimit;

        protected override string Endpoint => nameof(Endpoints.BundlesController.ListBundlesAsync);

        protected override string GenerateCursor(Bundle representation)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{nameof(Bundle)}|{representation.Id}"));
        }

        protected override void AddLinksToCollectionObjects(Collection<Bundle> representation)
        {
            foreach (var bundle in representation.Data)
            {
                bundleHypermediaService.Process(bundle);
            }
        }

        protected override void AddRouteParameters(HttpContext context, RouteValueDictionary routeValues)
        {
            if (context.Request.Query.TryGetValue("namespace", out var @namespace))
            {
                routeValues.Add("namespace", @namespace);
            }
        }
    }
}
