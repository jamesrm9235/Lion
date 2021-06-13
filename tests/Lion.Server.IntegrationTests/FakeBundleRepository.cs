using Lion.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server
{
    internal sealed class FakeBundleRepository : IBundleRepository
    {
        private readonly List<Bundle> bundles;

        public FakeBundleRepository()
        {
            bundles = new List<Bundle>();
        }

        public Task<long> AddBundleAsync(Bundle bundle)
        {
            var bundleWithName = bundles.Find(o => o.Name == bundle.Name);
            if (bundleWithName != null)
            {
                throw new BundleNameUnavailableException();
            }

            long nextId = 1;
            if (bundles.Count > 0)
            {
                nextId = bundles.Max(o => o.BundleId) + 1;
            }

            bundle.BundleId = nextId;
            bundles.Add(bundle);

            return Task.FromResult(nextId);
        }

        public Task<long> AddMessageAsync(Message message)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteBundleAsync(Bundle bundle)
        {
            bundles.Remove(bundle);
            return Task.CompletedTask;
        }

        public Task<Bundle> GetBundleAsync(long id)
        {
            return Task.FromResult(bundles.FirstOrDefault(o => o.BundleId == id));
        }

        public Task<IReadOnlyList<Bundle>> ListBundlesAsync(long cursor, int limit)
        {
            IEnumerable<Bundle> results;
            if (cursor > 0)
            {
                results = bundles
                .OrderByDescending(o => o.BundleId)
                .Where(o => o.BundleId <= cursor)
                .Take(limit);
            }
            else
            {
                results = bundles
                .OrderByDescending(o => o.BundleId)
                .Take(limit);
            }
            return Task.FromResult((IReadOnlyList<Bundle>)results.ToList().AsReadOnly());
        }

        public Task UpdateMessageAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var bundle = bundles.Find(o => o.BundleId == message.MessageId);
            if (bundle != null)
            {
                bundle.Messages.Remove(message);
                bundle.Messages.Add(message);
            }

            return Task.CompletedTask;
        }

        public void Reset(int seed)
        {
            bundles.Clear();
            for (int i = 1; i <= seed; i++)
            {
                bundles.Add(new Bundle { BundleId = i });
            }
        }
    }
}
