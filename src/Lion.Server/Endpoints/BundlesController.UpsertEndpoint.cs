using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [Consumes("application/json")]
        [HttpPut("{id:long}/messages/{language}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpsertMessageAsync([FromRoute] UpsertCommand command)
        {
            var bundle = await repository.GetBundleAsync(command.Id).ConfigureAwait(false);
            var message = bundle.Messages.FirstOrDefault(o => o.Language == command.Language);
            if (message == null)
            {
                var _ = await repository.AddMessageAsync(
                    new Abstractions.Message
                    {
                        BundleId = command.Id,
                        Language = command.Language,
                        Value = command.Message.Value
                    }).ConfigureAwait(false);
                return CreatedAtRoute(nameof(GetBundleAsync), new { id = command.Id }, null);
            }
            else
            {
                message.Value = command.Message.Value;
                await repository.UpdateMessageAsync(message).ConfigureAwait(false);
                return NoContent();
            }
        }

        public class UpsertCommand
        {
            [FromRoute(Name = "id")]
            public long Id { get; set; }

            [FromRoute(Name = "language")]
            public string Language { get; set; }

            [FromBody]
            public Body Message { get; set; }

            public class Body
            {
                
                public string Value { get; set; }
            }
        }
    }
}
