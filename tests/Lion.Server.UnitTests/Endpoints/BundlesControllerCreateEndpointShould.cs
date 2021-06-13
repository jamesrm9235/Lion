using AutoFixture;
using FluentAssertions;
using Lion.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerCreateEndpointShould : BundlesControllerTestBase
    {
        [Test]
        public async Task Return_Conflict_When_Bundle_Name_Is_Unavailable()
        {
            // Arrange
            repo.Setup(o => o.AddBundleAsync(It.IsAny<Bundle>())).ThrowsAsync(new BundleNameUnavailableException());

            // Act
            var response = await sut.CreateBundleAsync(new Fixture().Create<BundlesController.CreateCommand>());
            var actual = response as ConflictObjectResult;

            // Assert
            actual.Should().NotBeNull();
        }

        [Test]
        public async Task Return_Created_When_Bundle_Is_Created()
        {
            // Arrange
            repo.Setup(o => o.AddBundleAsync(It.IsAny<Bundle>())).ReturnsAsync(1);

            // Act
            var response = await sut.CreateBundleAsync(new Fixture().Create<BundlesController.CreateCommand>());
            var actual = response as CreatedAtActionResult;

            // Assert
            response.Should().NotBeNull();
            repo.Verify(o => o.AddBundleAsync(It.IsAny<Bundle>()), Times.Once);
        }
    }
}
