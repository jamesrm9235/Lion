using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Lion.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class BundlesControllerCreateEndpointShould : BundlesControllerFixture
    {
        [Test]
        public void Require_Bundle_Key()
        {
            var prop = typeof(BundlesController.CreateCommand)
                .GetProperty(nameof(BundlesController.CreateCommand.Key));

            prop.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Invalidate_Namespace_Id_When_Less_Than_Or_Equal_To_Zero(int id)
        {
            // Arrange
            var sut = new BundlesController.CreateCommand() { NamespaceId = id };
            var context = new ValidationContext(sut) { MemberName = nameof(BundlesController.CreateCommand.NamespaceId) };
            var validations = new List<ValidationResult>();

            // Act
            var actual = Validator.TryValidateProperty(sut.NamespaceId, context, validations);

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public void Require_Message_Language()
        {
            var prop = typeof(BundlesController.CreateCommand.Message)
                .GetProperty(nameof(BundlesController.CreateCommand.Message.Language));

            prop.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Test]
        public async Task Return_Conflict_When_Bundle_Key_Is_Unavailable()
        {
            // Arrange
            repo.Setup(o => o.AddBundleAsync(It.IsAny<Bundle>())).ThrowsAsync(new NameUnavailableException());

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
            var actual = response as CreatedAtRouteResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.AddBundleAsync(It.IsAny<Bundle>()), Times.Once);
        }
    }
}
