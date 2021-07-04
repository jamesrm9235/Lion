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
    [Route("api/namespaces")]
    public sealed partial class NamespacesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly INamespaceStore store;

        public NamespacesController(INamespaceStore store, IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }
    }
}
