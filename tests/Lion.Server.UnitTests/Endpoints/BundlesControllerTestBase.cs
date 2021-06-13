using AutoMapper;
using Lion.Abstractions;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public class BundlesControllerTestBase
    {
        protected Mock<IMapper> mapper;
        protected Mock<IBundleRepository> repo;
        protected BundlesController sut;

        [SetUp]
        public void BeforeEachTest()
        {
            repo = new Mock<IBundleRepository>();
            mapper = new Mock<IMapper>();
            sut = new BundlesController(repo.Object, mapper.Object);
        }
    }
}
