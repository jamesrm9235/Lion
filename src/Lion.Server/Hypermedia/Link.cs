namespace Lion.Server.Hypermedia
{
    public class Link
    {
        public Link(string href, string rel, string method, string mediaType = "application/json")
        {
            Href = href;
            Rel = rel;
            Method = method;
            MediaType = mediaType;
        }

        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
        public string MediaType { get; set; }
    }
}
