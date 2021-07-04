using AutoMapper;
using Lion.Common.Storage;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public class BundlesControllerFixture
    {
        protected Mock<IMapper> mapper;
        protected Mock<IBundleStore> repo;
        protected BundlesController sut;

        [SetUp]
        public void BeforeEachTest()
        {
            repo = new Mock<IBundleStore>();
            mapper = new Mock<IMapper>();
            sut = new BundlesController(repo.Object, mapper.Object);
        }
    }
}
