using System.Collections.Generic;

namespace Lion.Server.Models
{
    public abstract class Representation
    {
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();

        public void AddLink(string rel, Link link)
        {
            if (Links.ContainsKey(rel))
            {
                Links.Remove(rel);
            }
            Links.Add(rel, link);
        }
    }
}
