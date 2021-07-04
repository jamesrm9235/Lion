using Lion.Common.Entities;
using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [Consumes("application/json")]
        [HttpPost(Name = nameof(CreateBundleAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateBundleAsync([FromBody] CreateCommand command)
        {
            try
            {
                var bundle = new Bundle
                {
                    Key = command.Key,
                    Messages = command.Messages.Select(o => new Message { Language = o.Language, Value = o.Value }).ToList(),
                    Namespace = new Namespace { Id = command.NamespaceId }
                };

                var id = await store.AddBundleAsync(bundle).ConfigureAwait(false);

                return CreatedAtRoute(nameof(BundlesController.GetBundleAsync), new { Id = id }, null);
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

        public class CreateCommand
        {
            [Required]
            [RegularExpression("^[A-Za-z0-9_.-]+$")]
            public string Key { get; set; }

            [Range(typeof(long), "1", "9223372036854775807", ErrorMessage = "Namespace ID must be an integer greater than zero.")]
            public long NamespaceId { get; set; }

            public List<Message> Messages { get; set; } = new List<Message>();

            public class Message
            {
                [Required]
                [RegularExpression("^[A-Za-z0-9_.-]+$")]
                public string Language { get; set; }

                public string Value { get; set; }
            }
        }
    }
}
