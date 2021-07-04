using System.Collections.Generic;

namespace Lion.Common.Entities
{
    public class Namespace
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
    }
}
