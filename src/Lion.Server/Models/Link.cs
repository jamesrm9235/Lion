using System.Text.Json.Serialization;
using System.Web;

namespace Lion.Server.Models
{
    public class Link
    {
        public Link(string href)
        {
            Href = href;
        }

        public Link(string href, bool templated)
        {
            Href = templated ? HttpUtility.UrlDecode(href) : href;
            Templated = templated;
        }

        [JsonInclude]
        public string Href { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Templated { get; set; }
    }
}
