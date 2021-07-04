using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Text;

namespace Lion.Server.Hypermedia
{
    public sealed class CompactBundleCollectionHypermediaService : CollectionHypermediaService<CompactBundle>
    {
        public CompactBundleCollectionHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
            : base(httpContextAccessor, linkGenerator)
        {
        }

        protected override int Limit => Endpoints.BundlesController.ListQuery.DefaultLimit;

        protected override string Endpoint => nameof(Endpoints.BundlesController.ListBundlesAsync);

        protected override string GenerateCursor(CompactBundle representation)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{nameof(CompactBundle)}|{representation.Id}"));
        }

        protected override void AddRouteParameters(HttpContext context, RouteValueDictionary routeValues)
        {
            if (context.Request.Query.TryGetValue("namespace", out var @namespace))
            {
                routeValues.Add("namespace", @namespace);
            }
        }

        protected override void AddLinksToCollectionObjects(Collection<CompactBundle> representation)
        {
            // no-op
        }
    }
}
