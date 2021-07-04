using AutoMapper;
using Lion.Common.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Lion.Server.Endpoints
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/bundles")]
    public sealed partial class BundlesController : ControllerBase
    {
        private readonly IBundleStore store;
        private readonly IMapper mapper;

        public BundlesController(IBundleStore store, IMapper mapper)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}
