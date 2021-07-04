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
        [HttpGet("({id:long},{language})", Name = nameof(GetMessageAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(HypermediaResultFilter))]
        public async Task<ActionResult<Message>> GetMessageAsync([FromRoute] GetMessageQuery query)
        {
            var bundle = await store.GetBundleAsync(query.Id).ConfigureAwait(false);
            var message = bundle.Messages.FirstOrDefault(o => o.Language == query.Language);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(mapper.Map<Message>(message));
            }
        }

        public class GetMessageQuery
        {
            public long Id { get; set; }
            public string Language { get; set; }
        }
    }
}
