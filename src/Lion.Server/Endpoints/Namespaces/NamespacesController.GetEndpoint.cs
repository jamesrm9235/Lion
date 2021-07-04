using Lion.Server.Hypermedia;
using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class NamespacesController
    {
        [HttpGet("{id:long}", Name = nameof(GetNamespaceAsync))]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Namespace>> GetNamespaceAsync([FromRoute] GetQuery query)
        {
            var @namespace = await store.GetNamespaceAsync(query.Id).ConfigureAwait(false);
            if (@namespace == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(mapper.Map<Namespace>(@namespace));
            }
        }

        public class GetQuery
        {
            public long Id { get; set; }
        }
    }
}
