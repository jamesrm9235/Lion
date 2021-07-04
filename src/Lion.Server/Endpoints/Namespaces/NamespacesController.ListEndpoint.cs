using Lion.Server.Hypermedia;
using Lion.Server.Models;
using Lion.Server.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class NamespacesController
    {
        [HttpGet(Name = nameof(ListNamespacesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Collection<Namespace>>> ListNamespacesAsync([FromQuery] ListQuery query)
        {
            // select 1 more to obtain the next cursor
            var namespaces = await store.ListNamespacesAsync(query.DecodedCursor, query.Limit + 1).ConfigureAwait(false);

            return Ok(new Collection<Namespace>
            {
                Data = namespaces.Select(o => mapper.Map<Namespace>(o)).ToList(),
            });
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
