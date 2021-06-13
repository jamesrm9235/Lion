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
    public sealed class BundlesControllerDeleteEndpointShould : BundlesControllerTestBase
    {
        [Test]
        public async Task Return_Not_Found_When_Bundle_Is_Null()
        {
            // Arrange

            // Act
            var response = await sut.DeleteBundleAsync(new Fixture().Create<BundlesController.DeleteCommand>());
            var actual = response as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(It.IsAny<long>()), Times.Once);
            repo.Verify(o => o.DeleteBundleAsync(It.IsAny<Bundle>()), Times.Never);
        }

        [Test]
        public async Task Delete_Bundle_And_Return_No_Content()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.DeleteCommand>();
            var bundleToDelete = fixture.Create<Bundle>();

            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundleToDelete);
            repo.Setup(o => o.DeleteBundleAsync(bundleToDelete));

            // Act
            var response = await sut.DeleteBundleAsync(command);
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(command.Id), Times.Once);
            repo.Verify(o => o.DeleteBundleAsync(bundleToDelete), Times.Once);
        }
    }
}
