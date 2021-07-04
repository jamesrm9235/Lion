using Lion.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Common.Storage
{
    public interface INamespaceStore
    {
        Task<long> AddNamespaceAsync(Namespace @namespace);

        Task DeleteNamespaceAsync(Namespace @namespace);

        Task<Namespace> GetNamespaceAsync(long id);

        Task<IReadOnlyList<Namespace>> ListNamespacesAsync(long cursor, int limit);

        Task UpdateNamespaceAsync(Namespace @namespace);
    }
}
