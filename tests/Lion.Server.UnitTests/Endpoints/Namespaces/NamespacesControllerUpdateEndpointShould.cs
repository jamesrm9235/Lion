using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class NamespacesControllerUpdateEndpointShould : NamespacesControllerFixture
    {
        [Test]
        public void Require_Namespace_Key()
        {
            var prop = typeof(NamespacesController.UpdateCommand.Body).GetProperties().First(o => o.Name == nameof(NamespacesController.UpdateCommand.Data.Key));

            prop.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [TestCase("foobar")]
        [TestCase("foo.bar")]
        [TestCase("foo-bar")]
        [TestCase("foo_bar")]
        public void Require_Specific_Key_Format(string key)
        {
            // Arrange
            var sut = new NamespacesController.CreateCommand() { Key = key };
            var context = new ValidationContext(sut);
            var validations = new List<ValidationResult>();

            // Act
            var actual = Validator.TryValidateObject(sut, context, validations, validateAllProperties: true);

            // Assert
            actual.Should().BeTrue();
        }

        [Test]
        public async Task Return_Conflict_When_Namespace_Key_Is_Unavailable()
        {
            // Arrange
            var fixture = new Fixture();
            var @namespace = fixture.Create<Namespace>();
            repo.Setup(o => o.GetNamespaceAsync(It.IsAny<long>())).ReturnsAsync(@namespace);
            repo.Setup(o => o.UpdateNamespaceAsync(@namespace)).ThrowsAsync(new NameUnavailableException());

            // Act
            var response = await sut.UpdateNamespaceAsync(fixture.Create<NamespacesController.UpdateCommand>());
            var actual = response as ConflictObjectResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(It.IsAny<long>()), Times.Once);
            repo.Verify(o => o.UpdateNamespaceAsync(@namespace), Times.Once);
        }

        [Test]
        public async Task Return_Not_Found_When_Namespace_Is_Null()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var response = await sut.UpdateNamespaceAsync(fixture.Create<NamespacesController.UpdateCommand>());
            var actual = response as NotFoundObjectResult;

            // Assert
            response.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Return_No_Content_When_Namespace_Is_Updated()
        {
            // Arrange
            var fixture = new Fixture();
            var @namespace = fixture.Create<Namespace>();
            repo.Setup(o => o.GetNamespaceAsync(It.IsAny<long>())).ReturnsAsync(@namespace);
            repo.Setup(o => o.UpdateNamespaceAsync(@namespace));

            // Act
            var response = await sut.UpdateNamespaceAsync(fixture.Create<NamespacesController.UpdateCommand>());
            var actual = response as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(It.IsAny<long>()), Times.Once);
            repo.Verify(o => o.UpdateNamespaceAsync(@namespace), Times.Once);
        }
    }
}
