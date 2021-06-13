using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerUpsertEndpointShould : BundlesControllerTestBase
    {
        [Test]
        public async Task Create_Message_When_One_Does_Not_Exist()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpsertCommand>();
            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(new Abstractions.Bundle { BundleId = command.Id });

            // Act
            var response = await sut.UpsertMessageAsync(command);
            var actual = response as CreatedAtRouteResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.AddMessageAsync(It.IsAny<Abstractions.Message>()), Times.Once);
        }

        [Test]
        public async Task Update_Existing_Message()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpsertCommand>();
            var message = new Abstractions.Message { Language = command.Language };
            var bundle = new Abstractions.Bundle
            {
                BundleId = command.Id,
                Messages = new List<Abstractions.Message> { message }
            };
            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(bundle);

            // Act
            var response = await sut.UpsertMessageAsync(command);
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.UpdateMessageAsync(message), Times.Once);
        }
    }
}
