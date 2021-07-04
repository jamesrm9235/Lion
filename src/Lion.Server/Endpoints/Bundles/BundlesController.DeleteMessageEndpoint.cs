using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [HttpDelete("({id:long},{language})", Name = nameof(DeleteMessageAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMessageAsync([FromRoute] DeleteMessageCommand command)
        {
            var bundle = await store.GetBundleAsync(command.Id).ConfigureAwait(false);
            var message = bundle.Messages.FirstOrDefault(o => o.Language == command.Language);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                await store.DeleteMessageAsync(message).ConfigureAwait(false);
                return NoContent();
            }
        }

        public class DeleteMessageCommand
        {
            public long Id { get; set; }
            public string Language { get; set; }
        }
    }
}
