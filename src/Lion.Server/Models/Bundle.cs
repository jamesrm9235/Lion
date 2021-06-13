using System;
using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class Bundle : Representation
    {
        public override string Type => "bundle";
        public DateTime CreatedAt { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<Message> Messages { get; set; }
    }
}
