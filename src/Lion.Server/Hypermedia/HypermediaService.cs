using Lion.Server.Models;

namespace Lion.Server.Hypermedia
{
    public abstract class HypermediaService<T> : IHypermediaService where T : Representation
    {
        public bool CanProcess(object reprsentation) => reprsentation is T;

        public void Process(object representation) => Process(representation as T);

        public abstract void Process(T representation);
    }
}
