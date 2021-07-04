using AutoFixture;
using FluentAssertions;
using Lion.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerListEndpointShould : BundlesControllerFixture
    {
        [Test]
        public async Task Query_Limit_Plus_One()
        {
            // Arrange
            var query = new BundlesController.ListQuery { Limit = 2 };
            repo.Setup(o => o.ListBundlesAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new Fixture().CreateMany<Common.Entities.Bundle>().ToList().AsReadOnly());

            // Act
            var response = await sut.ListBundlesAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            repo.Verify(o => o.ListBundlesAsync(It.IsAny<long>(), query.Limit + 1, It.IsAny<string>()), Times.Once);
            actual.Value.Should().BeOfType<Collection<Models.Bundle>>();
        }

        [Test]
        public async Task Return_Compact_Bundles_When_Requested()
        {
            // Arrange
            var fixture = new Fixture();
            var query = new BundlesController.ListQuery { MediaType = "application/vnd.lion.compact+json" };
            repo.Setup(o => o.ListBundlesAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(fixture.CreateMany<Common.Entities.Bundle>().ToList().AsReadOnly());
            mapper.Setup(o => o.Map<CompactBundle>(It.IsAny<Common.Entities.Bundle>())).Returns(fixture.Create<CompactBundle>());

            // Act
            var response = await sut.ListBundlesAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            repo.Verify(o => o.ListBundlesAsync(It.IsAny<long>(), query.Limit + 1, It.IsAny<string>()), Times.Once);
            actual.Value.Should().BeOfType<Collection<Models.CompactBundle>>();
        }
    }
}
