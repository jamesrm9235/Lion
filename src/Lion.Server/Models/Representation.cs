using Lion.Server.Hypermedia;
using System.Collections.Generic;
using System.Linq;

namespace Lion.Server.Models
{
    public abstract class Representation
    {
        public abstract string Type { get; }

        public List<Link> Links { get; set; } = new List<Link>();

        public void AddLink(Link link)
        {
            var exists = Links.FirstOrDefault(o => o.Rel == link.Rel);
            if (exists != null)
            {
                Links.Remove(exists);
            }
            Links.Add(link);
        }
    }
}
