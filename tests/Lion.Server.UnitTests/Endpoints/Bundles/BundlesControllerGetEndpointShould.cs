using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerGetEndpointShould : BundlesControllerFixture
    {
        [Test]
        public async Task Return_Model()
        {
            // Arrange
            var fixture = new Fixture();
            var query = fixture.Create<BundlesController.GetQuery>();
            var entity = fixture.Create<Bundle>();
            var model = fixture.Create<Models.Bundle>();

            repo.Setup(o => o.GetBundleAsync(query.Id)).ReturnsAsync(entity);
            mapper.Setup(o => o.Map<Models.Bundle>(entity)).Returns(model);

            // Act
            var response = await sut.GetBundleAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.Value.Should().BeOfType<Models.Bundle>();
        }

        [Test]
        public async Task Return_Compact_Bundle_When_Requested()
        {
            // Arrange
            var query = new BundlesController.GetQuery { Id = 1, MediaType = "application/vnd.lion.compact+json" };
            var fixture =new Fixture();
            var entity = fixture.Create<Bundle>();
            var model = fixture.Create<Models.CompactBundle>();

            repo.Setup(o => o.GetBundleAsync(query.Id)).ReturnsAsync(entity);
            mapper.Setup(o => o.Map<Models.CompactBundle>(entity)).Returns(model);

            // Act
            var response = await sut.GetBundleAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.Value.Should().BeOfType<Models.CompactBundle>();
        }

        [Test]
        public async Task Return_Not_Found_When_Bundle_Is_Null()
        {
            // Arrange

            // Act
            var response = await sut.GetBundleAsync(new Fixture().Create<BundlesController.GetQuery>());
            var actual = response.Result as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(It.IsAny<long>()), Times.Once);
        }
    }
}
