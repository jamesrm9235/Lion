using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Lion.Server.Hypermedia
{
    public sealed class NamespaceHypermediaService : HypermediaService<Namespace>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public NamespaceHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public override void Process(Namespace representation)
        {
            var context = httpContextAccessor.HttpContext;

            representation.AddLink("self", new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.NamespacesController.GetNamespaceAsync), new { id = representation.Id })));

            representation.AddLink("bundles", new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.BundlesController.ListBundlesAsync), new { @namespace = representation.Key })));
        }
    }
}
