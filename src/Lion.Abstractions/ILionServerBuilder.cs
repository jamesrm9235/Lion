using Microsoft.Extensions.DependencyInjection;

namespace Lion.Abstractions
{
    public interface ILionServerBuilder
    {
        public IServiceCollection Services { get; set; }
    }
}
