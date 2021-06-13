using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Server.Hypermedia
{
    public sealed class HypermediaResultFilter : IAsyncResultFilter
    {
        private readonly IEnumerable<IHypermediaService> hypermediaServices;

        public HypermediaResultFilter(IEnumerable<IHypermediaService> hypermediaServices)
        {
            this.hypermediaServices = hypermediaServices;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result)
            {
                var model = result.Value;
                foreach (var service in hypermediaServices)
                {
                    if (service.CanProcess(model))
                    {
                        service.Process(model);
                    }
                }
            }

            await next();
        }
    }
}
