using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class Namespace : Representation
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public List<string> Languages { get; set; }
    }
}
