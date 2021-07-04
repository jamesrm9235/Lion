using AutoFixture;
using FluentAssertions;
using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerUpdateEndpointShould : BundlesControllerFixture
    {
        [Test]
        public void Require_Bundle_Key()
        {
            var sut = typeof(BundlesController.UpdateBundleCommand.Body)
                .GetProperty(nameof(BundlesController.UpdateBundleCommand.Body.Key));
            sut.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Test]
        public async Task Return_Not_Found_When_Bundle_Is_Null()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpdateBundleCommand>();

            // Act
            var response = await sut.UpdateBundleAsync(command);
            var actual = response as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.UpdateBundleAsync(It.IsAny<Common.Entities.Bundle>()), Times.Never);
        }

        [Test]
        public async Task Return_No_Content_When_Bundle_Is_Updated()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpdateBundleCommand>();
            var bundle = fixture.Create<Common.Entities.Bundle>();

            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundle);

            // Act
            var response = await sut.UpdateBundleAsync(command);
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.UpdateBundleAsync(bundle), Times.Once);
        }

        [Test]
        public async Task Return_Conflict_When_Bundle_Key_Is_Unavailable()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpdateBundleCommand>();
            var bundle = fixture.Create<Common.Entities.Bundle>();

            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundle);
            repo.Setup(o => o.UpdateBundleAsync(bundle)).ThrowsAsync(new NameUnavailableException());

            // Act
            var response = await sut.UpdateBundleAsync(command);
            var actual = response as ConflictObjectResult;

            // Assert
            actual.Should().NotBeNull();
        }
    }
}
