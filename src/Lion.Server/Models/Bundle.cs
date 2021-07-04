using System;
using System.Collections.Generic;

namespace Lion.Server.Models
{
    public sealed class Bundle : Representation
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public Namespace Namespace { get; set; }
        public List<Message> Messages { get; set; }
    }
}
