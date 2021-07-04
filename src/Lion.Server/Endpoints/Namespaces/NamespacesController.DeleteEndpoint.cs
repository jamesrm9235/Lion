using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class NamespacesController
    {
        [HttpDelete("{id:long}", Name = nameof(DeleteNamespaceAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteNamespaceAsync([FromRoute] DeleteCommand command)
        {
            var @namespace = await store.GetNamespaceAsync(command.Id).ConfigureAwait(false);
            if (@namespace == null)
            {
                return NotFound();
            }
            else
            {
                await store.DeleteNamespaceAsync(@namespace).ConfigureAwait(false);
                return NoContent();
            }
        }

        public class DeleteCommand
        {
            public long Id { get; set; }
        }
    }
}
