using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Abstractions
{
    public interface IBundleRepository
    {
        Task<long> AddBundleAsync(Bundle bundle);

        Task<long> AddMessageAsync(Message message);

        Task DeleteBundleAsync(Bundle bundle);

        //Task DeleteMessageAsync(Message message);

        Task<Bundle> GetBundleAsync(long id);

        Task<IReadOnlyList<Bundle>> ListBundlesAsync(long cursor, int limit);

        Task UpdateMessageAsync(Message message);
    }
}
