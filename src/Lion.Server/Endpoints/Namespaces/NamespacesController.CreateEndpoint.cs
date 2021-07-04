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
        [HttpPost(Name = nameof(CreateNamespaceAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateNamespaceAsync([FromBody] CreateCommand command)
        {
            try
            {
                var @namespace = new Common.Entities.Namespace
                {
                    Key = command.Key
                };
                var id = await store.AddNamespaceAsync(@namespace).ConfigureAwait(false);
                return CreatedAtRoute(nameof(GetNamespaceAsync), new { id }, null);
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

        public class CreateCommand
        {
            [Required]
            [RegularExpression("^[A-Za-z0-9_.-]+$")]
            public string Key { get; set; }
        }
    }
}
