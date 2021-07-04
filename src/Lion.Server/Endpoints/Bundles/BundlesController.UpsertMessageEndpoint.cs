using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed partial class BundlesController
    {
        [Consumes("application/json")]
        [HttpPut("({id:long},{language})", Name = nameof(UpsertMessageAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpsertMessageAsync([FromRoute] UpsertCommand command)
        {
            var bundle = await store.GetBundleAsync(command.Id).ConfigureAwait(false);
            var message = bundle.Messages.FirstOrDefault(o => o.Language == command.Language);
            if (message == null)
            {
                var _ = await store.AddMessageAsync(
                    new Common.Entities.Message
                    {
                        BundleId = command.Id,
                        Language = command.Language,
                        Value = command.Message.Value
                    }).ConfigureAwait(false);
                return CreatedAtRoute(nameof(GetBundleAsync), new { Id = command.Id, Language = command.Language }, null);
            }
            else
            {
                message.Value = command.Message.Value;
                await store.UpdateMessageAsync(message).ConfigureAwait(false);
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
