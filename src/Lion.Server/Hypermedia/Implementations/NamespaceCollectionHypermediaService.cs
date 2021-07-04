using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Text;

namespace Lion.Server.Hypermedia
{
    public sealed class NamespaceCollectionHypermediaService : CollectionHypermediaService<Namespace>
    {
        private readonly NamespaceHypermediaService namespaceHypermediaService;

        public NamespaceCollectionHypermediaService(
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, NamespaceHypermediaService namespaceHypermediaService)
            : base(httpContextAccessor, linkGenerator)
        {
            this.namespaceHypermediaService = namespaceHypermediaService ?? throw new ArgumentNullException(nameof(namespaceHypermediaService));
        }

        protected override int Limit => Endpoints.NamespacesController.ListQuery.DefaultLimit;
        protected override string Endpoint => nameof(Endpoints.NamespacesController.ListNamespacesAsync);

        protected override void AddLinksToCollectionObjects(Collection<Namespace> representation)
        {
            foreach (var @namespace in representation.Data)
            {
                namespaceHypermediaService.Process(@namespace);
            }
        }

        protected override string GenerateCursor(Namespace representation)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{nameof(Namespace)}|{representation.Id}"));
        }
    }
}
