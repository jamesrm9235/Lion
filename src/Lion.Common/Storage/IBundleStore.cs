using Lion.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Common.Storage
{
    public interface IBundleStore
    {
        Task<long> AddBundleAsync(Bundle bundle);

        Task<long> AddMessageAsync(Message message);

        Task DeleteBundleAsync(Bundle bundle);

        Task DeleteMessageAsync(Message message);

        Task<Bundle> GetBundleAsync(long id);

        Task<IReadOnlyList<Bundle>> ListBundlesAsync(long cursor, int limit, string @namespace = "");

        Task UpdateBundleAsync(Bundle bundle);

        Task UpdateMessageAsync(Message message);
    }
}
