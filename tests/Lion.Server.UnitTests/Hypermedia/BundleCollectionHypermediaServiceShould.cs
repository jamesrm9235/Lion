using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class BundleCollectionHypermediaServiceShould
    {
        [Test]
        public void Add_Next_Page_Link_When_Collection_Has_One_More_Bundle_Than_Limit()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var query = new Mock<IQueryCollection>();
            var request = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            var accessor = new Mock<IHttpContextAccessor>();

            var limit = new StringValues("2");
            query.Setup(o => o.TryGetValue("limit", out limit)).Returns(true);
            request.SetupGet(o => o.Query).Returns(query.Object);
            httpContext.SetupGet(o => o.Request).Returns(request.Object);
            accessor.SetupGet(o => o.HttpContext).Returns(httpContext.Object);
            fixture.Inject(accessor.Object);

            var sut = fixture.Create<BundleCollectionHypermediaService>();

            var first = new Bundle { Id = 1 };
            var second = new Bundle {Id = 2 };
            var third = new Bundle {Id = 3 };
            var collection = new Collection<Bundle> { Data = new List<Bundle> { first, second, third } };

            // Act
            sut.Process(collection);

            // Assert
            collection.Links.Should().HaveCount(1);
            collection.Data.Should().NotContain(third, because: "service should remove it");
        }

        [Test]
        public void Add_Links_To_Each_Bundle()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var sut = fixture.Create<BundleCollectionHypermediaService>();

            var first = new Bundle { Id = 1 };
            var second = new Bundle {Id = 2 };
            var third = new Bundle {Id = 3 };
            var collection = new Collection<Bundle> { Data = new List<Bundle> { first, second, third } };

            // Act
            sut.Process(collection);

            // Assert
            collection.Data.Should().SatisfyRespectively(
                first =>
                {
                    first.Links.Should().NotBeEmpty();
                },
                second => 
                {
                    second.Links.Should().NotBeEmpty();
                },
                third => 
                {
                    third.Links.Should().NotBeEmpty();
                });
        }
    }
}
