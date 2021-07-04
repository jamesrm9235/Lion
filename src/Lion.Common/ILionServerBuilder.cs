using Microsoft.Extensions.DependencyInjection;

namespace Lion.Common
{
    public interface ILionServerBuilder
    {
        public IServiceCollection Services { get; set; }
    }
}
