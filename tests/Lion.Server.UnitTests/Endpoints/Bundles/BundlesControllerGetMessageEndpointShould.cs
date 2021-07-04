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
    public sealed class BundlesControllerGetMessageEndpointShould : BundlesControllerFixture
    {
        [Test]
        public async Task Return_Model()
        {
            // Arrange
            var fixture = new Fixture();
            var query = fixture.Create<BundlesController.GetMessageQuery>();
            var bundle = fixture.Create<Bundle>();
            var message = new Message { Language = query.Language };
            bundle.Messages.Add(message);
            var model = fixture.Create<Models.Message>();

            repo.Setup(o => o.GetBundleAsync(query.Id)).ReturnsAsync(bundle);
            mapper.Setup(o => o.Map<Models.Message>(message)).Returns(model);

            // Act
            var response = await sut.GetMessageAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.Value.Should().BeOfType<Models.Message>();
            repo.Verify(o => o.GetBundleAsync(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Return_Not_Found_When_Message_Is_Null()
        {
            // Arrange
            var fixture = new Fixture();
            var query = fixture.Create<BundlesController.GetMessageQuery>();
            var bundle = fixture.Create<Bundle>();

            repo.Setup(o => o.GetBundleAsync(query.Id)).ReturnsAsync(bundle);

            // Act
            var response = await sut.GetMessageAsync(query);
            var actual = response.Result as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetBundleAsync(It.IsAny<long>()), Times.Once);
        }
    }
}
