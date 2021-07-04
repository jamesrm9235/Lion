using System;
using System.Text.Json.Serialization;

namespace Lion.Server.Models
{
    public class Message : Representation
    {
        [JsonIgnore]
        public int BundleId { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
