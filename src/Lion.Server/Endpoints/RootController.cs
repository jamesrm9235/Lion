using Lion.Server.Hypermedia;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lion.Server.Endpoints
{
    [ApiController]
    [Route("api")]
    public sealed class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetEntryLinks))]
        public ActionResult<List<Link>> GetEntryLinks()
        {
            var links = new List<Link>();

            links.Add(new Link(Url.Link(nameof(GetEntryLinks), null), "self", "GET"));
            links.Add(new Link(Url.Link(nameof(BundlesController.ListBundlesAsync), null), "bundles", "GET"));
            links.Add(new Link(Url.Link(nameof(BundlesController.CreateBundleAsync), null), "create-bundle", "POST"));

            return Ok(links);
        }
    }
}
