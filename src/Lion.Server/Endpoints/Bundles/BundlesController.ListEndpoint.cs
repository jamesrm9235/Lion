using Lion.Server.Hypermedia;
using Lion.Server.Models;
using Lion.Server.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [HttpGet(Name = nameof(ListBundlesAsync))]
        [Produces("application/vnd.lion.compact+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Collection<Bundle>>> ListBundlesAsync([FromQuery] ListQuery query)
        {
            // select 1 more to obtain the next cursor
            var bundles = await store.ListBundlesAsync(query.DecodedCursor, query.Limit + 1, query.Namespace).ConfigureAwait(false);

            if (query.MediaType == "application/vnd.lion.compact+json")
            {
                return Ok(new Collection<CompactBundle>
                {
                    Data = bundles.Select(o => mapper.Map<CompactBundle>(o)).ToList()
                });
            }
            else
            {
                return Ok(new Collection<Bundle>
                {
                    Data = bundles.Select(o => mapper.Map<Bundle>(o)).ToList(),
                });
            }
        }

        public sealed class ListQuery : CursorQuery
        {
            private const int MaxLimit = 100;
            private int limit = 10;

            public const int DefaultLimit = 10;

            public override int Limit
            {
                get => limit;
                set => limit = value > MaxLimit ? MaxLimit : value;
            }

            [FromHeader(Name = "Accept")]
            public string MediaType { get; set; }

            public string Namespace { get; set; }
        }
    }
}
