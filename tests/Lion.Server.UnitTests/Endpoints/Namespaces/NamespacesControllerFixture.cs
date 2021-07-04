using AutoMapper;
using Lion.Common.Storage;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public class NamespacesControllerFixture
    {
        protected Mock<IMapper> mapper;
        protected Mock<INamespaceStore> repo;
        protected NamespacesController sut;

        [SetUp]
        public void BeforeEachTest()
        {
            repo = new Mock<INamespaceStore>();
            mapper = new Mock<IMapper>();
            sut = new NamespacesController(repo.Object, mapper.Object);
        }
    }
}
