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
        private readonly MessageHypermediaService messageHypermediaService;
        private readonly NamespaceHypermediaService namespaceHypermediaService;

        public BundleHypermediaService(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            MessageHypermediaService messageHypermediaService,
            NamespaceHypermediaService namespaceHypermediaService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
            this.messageHypermediaService = messageHypermediaService ?? throw new ArgumentNullException(nameof(messageHypermediaService));
            this.namespaceHypermediaService = namespaceHypermediaService ?? throw new ArgumentNullException(nameof(namespaceHypermediaService));
        }

        public override void Process(Bundle representation)
        {
            var context = httpContextAccessor.HttpContext;

            representation.AddLink("self", new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.BundlesController.GetBundleAsync), new { Id = representation.Id })));

            representation.AddLink("message", new Link(
                linkGenerator.GetUriByName(
                    context, nameof(Endpoints.BundlesController.GetMessageAsync), new { Id = representation.Id, Language = "{language}" }),
                templated: true));

            namespaceHypermediaService.Process(representation.Namespace);

            foreach (var message in representation.Messages)
            {
                messageHypermediaService.Process(message);
            }
        }
    }
}
