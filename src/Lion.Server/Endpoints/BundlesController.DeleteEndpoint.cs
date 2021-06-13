using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [HttpDelete("{id:long}", Name = nameof(DeleteBundleAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBundleAsync([FromRoute] DeleteCommand command)
        {
            var bundle = await repository.GetBundleAsync(command.Id).ConfigureAwait(false);
            if (bundle == null)
            {
                return NotFound();
            }
            else
            {
                await repository.DeleteBundleAsync(bundle).ConfigureAwait(false);
                return NoContent();
            }
        }

        public class DeleteCommand
        {
            public long Id { get; set; }
        }
    }
}
