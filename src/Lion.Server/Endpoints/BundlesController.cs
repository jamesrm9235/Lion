using AutoMapper;
using Lion.Abstractions;
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
        private readonly IBundleRepository repository;
        private readonly IMapper mapper;

        public BundlesController(IBundleRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}
