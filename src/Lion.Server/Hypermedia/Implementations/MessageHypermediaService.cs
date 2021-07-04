using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Lion.Server.Hypermedia
{
    public class MessageHypermediaService : HypermediaService<Message>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public MessageHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public override void Process(Message representation)
        {
            var context = httpContextAccessor.HttpContext;

            representation.AddLink("self", new Link(linkGenerator.GetUriByName(
                context, nameof(Endpoints.BundlesController.GetMessageAsync), new { Id = representation.BundleId, Language = representation.Language })));
        }
    }
}
