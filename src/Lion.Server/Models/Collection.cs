using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class Collection<T> : Representation where T : class
    {
        public List<T> Data { get; set; }
    }
}
