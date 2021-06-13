using Lion.Abstractions;
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
                    Comment = command.Comment,
                    Name = command.Name,
                    Namespace = command.Namespace,
                    Messages = command.Messages.Select(o => new Message { Language = o.Language, Value = o.Value }).ToList()
                };

                var id = await repository.AddBundleAsync(bundle).ConfigureAwait(false);

                return CreatedAtRoute(nameof(BundlesController.GetBundleAsync), new { Id = id }, null);
            }
            catch (BundleNameUnavailableException e)
            {
                return Conflict(new ProblemDetails
                {
                    Detail = e.Message,
                    Status = StatusCodes.Status409Conflict,
                    Title = "Bundle name is not available.",
                });
            }
        }

        public class CreateCommand
        {
            [Required]
            [RegularExpression("^[A-Za-z0-9_.-]+$")]
            public string Name { get; set; }

            [Required]
            [RegularExpression("^[A-Za-z0-9_.-]+$")]
            public string Namespace { get; set; }

            public string Comment { get; set; }

            public List<Message> Messages { get; set; } = new List<Message>();

            public class Message
            {
                [Required]
                [RegularExpression("^[A-Za-z0-9_.-]+$")]
                public string Language { get; set; }

                [Required]
                public string Value { get; set; }
            }
        }
    }
}
