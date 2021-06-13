namespace Lion.Abstractions
{
    public class Message
    {
        public long BundleId { get; set; }
        public string Language { get; set; }
        public long MessageId { get; set; }
        public string Value { get; set; }
    }
}
