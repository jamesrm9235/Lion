using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Text;

namespace Lion.Server.Hypermedia
{
    public sealed class BundleCollectionHypermediaService : HypermediaService<Collection<Bundle>>
    {
        private readonly BundleHypermediaService bundleHypermediaService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public BundleCollectionHypermediaService(
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, BundleHypermediaService bundleHypermediaService)
        {
            this.bundleHypermediaService = bundleHypermediaService ?? throw new ArgumentNullException(nameof(bundleHypermediaService));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public override void Process(Collection<Bundle> representation)
        {
            var context = httpContextAccessor.HttpContext;

            var limit = Endpoints.BundlesController.ListQuery.DefaultLimit;

            if (context.Request.Query.TryGetValue("limit", out var value) && int.TryParse(value.ToString(), out var n))
            {
                limit = n;
            }

            if (representation.Data.Count == limit + 1)
            {
                var last = representation.Data.Last();
                representation.Data.Remove(last);

                var cursor = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{nameof(Bundle)}|{last.Id}"));

                representation.AddLink(new Link(
                    linkGenerator.GetUriByName(
                        context,
                        nameof(Endpoints.BundlesController.ListBundlesAsync),
                        new { limit = limit, cursor = cursor },
                        scheme: "https"),
                    "next",
                    "GET"));
            }

            foreach (var bundle in representation.Data)
            {
                bundleHypermediaService.Process(bundle);
            }
        }
    }
}
