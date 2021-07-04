using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class CompactBundle : Dictionary<string, Dictionary<string, string>>
    {
        public CompactBundle(long id, string @namespace, string bundle, Dictionary<string, string> messages)
        {
            Id = id;
            this[$"{@namespace}.{bundle}"] = messages;
        }

        public long Id { get; set; }
    }
}
