namespace Lion.Server.Hypermedia
{
    public interface IHypermediaService
    {
        bool CanProcess(object representation);

        void Process(object representation);
    }
}
