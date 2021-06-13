using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class Collection<T> : Representation where T : Representation
    {
        public override string Type => "collection";
        public List<T> Data { get; set; }
    }
}
