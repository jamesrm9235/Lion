using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Lion.Server.Hypermedia
{
    public sealed class BundleHypermediaService : HypermediaService<Bundle>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public BundleHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public override void Process(Bundle representation)
        {
            var context = httpContextAccessor.HttpContext;

            representation.AddLink(new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.BundlesController.GetBundleAsync), new { Id = representation.Id }, scheme: "https"),
                "self",
                "GET"));

            representation.AddLink(new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.BundlesController.DeleteBundleAsync), new { Id = representation.Id }, scheme: "https"),
                "delete",
                "DELETE"));
        }
    }
}
