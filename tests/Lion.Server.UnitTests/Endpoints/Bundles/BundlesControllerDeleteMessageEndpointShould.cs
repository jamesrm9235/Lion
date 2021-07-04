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
    public sealed class BundlesControllerDeleteMessageEndpointShould : BundlesControllerFixture
    {
        [Test]
        public async Task Return_Not_Found_When_Message_Is_Null()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.DeleteMessageCommand>();
            var bundle = fixture.Create<Bundle>();

            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundle);

            // Act
            var response = await sut.DeleteMessageAsync(command);
            var actual = response as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(It.IsAny<long>()), Times.Once);
            repo.Verify(o => o.DeleteMessageAsync(It.IsAny<Message>()), Times.Never);
        }

        [Test]
        public async Task Return_No_Content_When_Message_Is_Deleted()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.DeleteMessageCommand>();
            var bundle = fixture.Create<Bundle>();
            var messageToDelete = new Message { Language = command.Language };
            bundle.Messages.Add(messageToDelete);

            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundle);
            repo.Setup(o => o.DeleteMessageAsync(messageToDelete));

            // Act
            var response = await sut.DeleteMessageAsync(command);
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(command.Id), Times.Once);
            repo.Verify(o => o.DeleteMessageAsync(messageToDelete), Times.Once);
        }
    }
}
