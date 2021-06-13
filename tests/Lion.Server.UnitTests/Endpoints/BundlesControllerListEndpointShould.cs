using AutoFixture;
using FluentAssertions;
using Lion.Abstractions;
using Lion.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerListEndpointShould : BundlesControllerTestBase
    {
        [Test]
        public async Task Query_Limit_Plus_One()
        {
            // Arrange
            var query = new BundlesController.ListQuery { Limit = 2 };
            repo.Setup(o => o.ListBundlesAsync(It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(new Fixture().CreateMany<Abstractions.Bundle>().ToList().AsReadOnly());

            // Act
            var response = await sut.ListBundlesAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            repo.Verify(o => o.ListBundlesAsync(It.IsAny<long>(), query.Limit + 1), Times.Once);
            actual.Value.Should().BeOfType<Collection<Models.Bundle>>();
        }
    }
}
