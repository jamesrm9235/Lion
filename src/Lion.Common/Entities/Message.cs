using System;

namespace Lion.Common.Entities
{
    public class Message
    {
        public long BundleId { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
