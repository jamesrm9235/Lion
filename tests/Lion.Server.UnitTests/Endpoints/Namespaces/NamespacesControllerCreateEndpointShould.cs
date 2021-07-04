using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class NamespacesControllerCreateEndpointShould : NamespacesControllerFixture
    {
        [Test]
        public void Require_Namespace_Key()
        {
            var prop = typeof(NamespacesController.CreateCommand)
                .GetProperty(nameof(NamespacesController.CreateCommand.Key));

            prop.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Test]
        public async Task Return_Conflict_When_Namespace_Key_Is_Unavailable()
        {
            // Arrange
            repo.Setup(o => o.AddNamespaceAsync(It.IsAny<Namespace>())).ThrowsAsync(new NameUnavailableException());

            // Act
            var response = await sut.CreateNamespaceAsync(new Fixture().Create<NamespacesController.CreateCommand>());
            var actual = response as ConflictObjectResult;

            // Assert
            actual.Should().NotBeNull();
        }

        [Test]
        public async Task Return_Created_When_Namespace_Is_Created()
        {
            // Arrange
            repo.Setup(o => o.AddNamespaceAsync(It.IsAny<Namespace>())).ReturnsAsync(1);

            // Act
            var response = await sut.CreateNamespaceAsync(new Fixture().Create<NamespacesController.CreateCommand>());
            var actual = response as CreatedAtRouteResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.AddNamespaceAsync(It.IsAny<Namespace>()), Times.Once);
        }
    }
}
