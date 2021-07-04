using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace Lion.Server.Hypermedia
{
    public abstract class CollectionHypermediaService<T> : HypermediaService<Collection<T>> where T : class
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public CollectionHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        protected abstract int Limit { get; }

        protected abstract string Endpoint { get; }

        protected abstract string GenerateCursor(T representation);

        protected abstract void AddLinksToCollectionObjects(Collection<T> representation);

        protected virtual void AddRouteParameters(HttpContext context, RouteValueDictionary routeValues)
        {
            // no-op
        }

        public sealed override void Process(Collection<T> representation)
        {
            var context = httpContextAccessor.HttpContext;

            var limit = Limit;
            if (context.Request.Query.TryGetValue("limit", out var value) && int.TryParse(value.ToString(), out var n))
            {
                limit = n;
            }

            if (representation.Data.Count == limit + 1)
            {
                var last = representation.Data.Last();
                representation.Data.Remove(last);

                var cursor = GenerateCursor(last);

                var routeValues = new RouteValueDictionary(new { cursor = cursor, limit = limit });

                AddRouteParameters(context, routeValues);

                representation.AddLink(
                    "next",
                    new Link(linkGenerator.GetUriByName(context, Endpoint, routeValues)));
            }

            AddLinksToCollectionObjects(representation);
        }
    }
}
