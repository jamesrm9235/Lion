using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerUpsertEndpointShould : BundlesControllerFixture
    {
        [Test]
        public async Task Create_Message_When_One_Does_Not_Exist()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpsertCommand>();
            repo.Setup(o => o.GetBundleAsync(command.Id)).ReturnsAsync(new Common.Entities.Bundle { Id = command.Id });

            // Act
            var response = await sut.UpsertMessageAsync(command);
            var actual = response as CreatedAtRouteResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.AddMessageAsync(It.IsAny<Message>()), Times.Once);
        }

        [Test]
        public async Task Update_Existing_Message()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<BundlesController.UpsertCommand>();
            var message = new Common.Entities.Message { Language = command.Language };
            var bundle = new Common.Entities.Bundle
            {
                Id = command.Id,
                Messages = new List<Message> { message }
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
