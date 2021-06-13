using Lion.Server.Hypermedia;
using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [HttpGet(Name = nameof(ListBundlesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Collection<Bundle>>> ListBundlesAsync([FromQuery] ListQuery query)
        {
            // select 1 more to obtain the next cursor
            var bundles = await repository.ListBundlesAsync(query.DecodedCursor, query.Limit + 1).ConfigureAwait(false);

            var representation = new Collection<Bundle>
            {
                Data = bundles.Select(o => mapper.Map<Bundle>(o)).ToList(),
            };

            return Ok(representation);
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
        }
    }
}
