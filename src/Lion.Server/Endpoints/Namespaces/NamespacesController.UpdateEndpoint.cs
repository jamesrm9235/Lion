using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class NamespacesController
    {
        [Consumes("application/json")]
        [HttpPut("{id:long}", Name = nameof(UpdateNamespaceAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateNamespaceAsync([FromRoute] UpdateCommand command)
        {
            try
            {
                var @namespace = await store.GetNamespaceAsync(command.Id).ConfigureAwait(false);
                if (@namespace == null)
                {
                    return NotFound();
                }
                else
                {
                    @namespace.Key = command.Data.Key;
                    await store.UpdateNamespaceAsync(@namespace).ConfigureAwait(false);
                    return NoContent();
                }
            }
            catch (NameUnavailableException e)
            {
                return Conflict(new ProblemDetails
                {
                    Detail = e.Message,
                    Status = StatusCodes.Status409Conflict,
                    Title = "Namespace key is not available.",
                });
            }
        }

        public class UpdateCommand
        {
            [FromRoute]
            public long Id { get; set; }

            [FromBody]
            public Body Data { get; set; }

            public class Body
            {
                [Required]
                [RegularExpression("^[A-Za-z0-9_.-]+$")]
                public string Key { get; set; }
            }
        }
    }
}
