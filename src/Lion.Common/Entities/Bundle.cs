using System.Collections.Generic;

namespace Lion.Common.Entities
{
    public class Bundle
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public Namespace Namespace { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
