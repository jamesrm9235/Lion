using Lion.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lion.Server
{
    public sealed class LionServerBuilder : ILionServerBuilder
    {
        public LionServerBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; set; }
    }
}
