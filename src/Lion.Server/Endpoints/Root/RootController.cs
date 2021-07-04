using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lion.Server.Endpoints
{
    [ApiController]
    [Produces("application/json")]
    [Route("api")]
    public sealed class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Root> GetRoot()
        {
            var root = new Root();

            root.AddLink("self", new Link(Url.Link(nameof(GetRoot), null)));
            root.AddLink("namespaces", new Link(Url.Link(nameof(NamespacesController.ListNamespacesAsync), null)));
            root.AddLink("bundles", new Link(Url.Link(nameof(BundlesController.ListBundlesAsync), null)));

            return Ok(root);
        }
    }
}
