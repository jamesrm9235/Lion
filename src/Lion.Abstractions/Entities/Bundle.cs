using System;
using System.Collections.Generic;

namespace Lion.Abstractions
{
    public class Bundle
    {
        public long BundleId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public DateTime ModifiedAt { get; set; }
    }
}
