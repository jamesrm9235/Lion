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
    public sealed class NamespacesControllerDeleteEndpointShould : NamespacesControllerFixture
    {
        [Test]
        public async Task Return_Not_Found_When_Namespace_Is_Null()
        {
            // Arrange

            // Act
            var response = await sut.DeleteNamespaceAsync(new Fixture().Create<NamespacesController.DeleteCommand>());
            var actual = response as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(It.IsAny<long>()), Times.Once);
            repo.Verify(o => o.DeleteNamespaceAsync(It.IsAny<Namespace>()), Times.Never);
        }

        [Test]
        public async Task Delete_Namespace_And_Return_No_Content()
        {
            // Arrange
            var fixture = new Fixture();
            var command = fixture.Create<NamespacesController.DeleteCommand>();
            var namespaceToDelete = fixture.Create<Namespace>();

            repo.Setup(o => o.GetNamespaceAsync(command.Id)).ReturnsAsync(namespaceToDelete);
            repo.Setup(o => o.DeleteNamespaceAsync(namespaceToDelete));

            // Act
            var response = await sut.DeleteNamespaceAsync(command);
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(command.Id), Times.Once);
            repo.Verify(o => o.DeleteNamespaceAsync(namespaceToDelete), Times.Once);
        }
    }
}
