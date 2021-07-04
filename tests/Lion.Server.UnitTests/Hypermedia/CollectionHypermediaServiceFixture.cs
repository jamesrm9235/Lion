using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class CollectionHypermediaServiceFixture
    {
        private Mock<IQueryCollection> query;

        protected IFixture fixture;

        [SetUp]
        public void BeforeEachTest()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());

            query = new Mock<IQueryCollection>();
            var request = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            var accessor = new Mock<IHttpContextAccessor>();

            request.SetupGet(o => o.Query).Returns(query.Object);
            httpContext.SetupGet(o => o.Request).Returns(request.Object);
            accessor.SetupGet(o => o.HttpContext).Returns(httpContext.Object);
            fixture.Inject(accessor.Object);
        }

        protected void AddQuery(string queryKey, string queryValue)
        {
            var value = new StringValues(queryValue);
            query.Setup(o => o.TryGetValue(queryKey, out value)).Returns(true);
        }
    }
}
