using Lion.Server.Hypermedia;
using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [HttpGet("{id:long}", Name = nameof(GetBundleAsync))]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Bundle>> GetBundleAsync([FromRoute] GetQuery query)
        {
            var bundle = await repository.GetBundleAsync(query.Id).ConfigureAwait(false);
            if (bundle == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(mapper.Map<Bundle>(bundle));
            }
        }

        public class GetQuery
        {
            public long Id { get; set; }
        }
    }
}
