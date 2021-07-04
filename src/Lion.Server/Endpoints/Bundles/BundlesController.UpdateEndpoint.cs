using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [Consumes("application/json")]
        [HttpPut("{id:long}", Name = nameof(UpdateBundleAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateBundleAsync([FromRoute] UpdateBundleCommand command)
        {
            try
            {
                var bundle = await store.GetBundleAsync(command.Id).ConfigureAwait(false);
                if (bundle == null)
                {
                    return NotFound();
                }
                else
                {
                    bundle.Key = command.Data.Key;
                    await store.UpdateBundleAsync(bundle).ConfigureAwait(false);
                    return NoContent();
                }
            }
            catch (NameUnavailableException e)
            {
                return Conflict(new ProblemDetails
                {
                    Detail = e.Message,
                    Status = StatusCodes.Status409Conflict,
                    Title = "Bundle key is not available.",
                });
            }
        }

        public class UpdateBundleCommand
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
