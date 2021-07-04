using AutoFixture;
using FluentAssertions;
using Lion.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public sealed class NamespacesControllerGetEndpointShould : NamespacesControllerFixture
    {
        [Test]
        public async Task Return_Model()
        {
            // Arrange
            var fixture = new Fixture();
            var query = fixture.Create<NamespacesController.GetQuery>();
            var entity = fixture.Create<Namespace>();
            var model = fixture.Create<Models.Namespace>();

            repo.Setup(o => o.GetNamespaceAsync(query.Id)).ReturnsAsync(entity);
            mapper.Setup(o => o.Map<Models.Namespace>(entity)).Returns(model);

            // Act
            var response = await sut.GetNamespaceAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.Value.Should().BeOfType<Models.Namespace>();
        }

        [Test]
        public async Task Return_Not_Found_When_Namespace_Is_Null()
        {
            // Arrange

            // Act
            var response = await sut.GetNamespaceAsync(new Fixture().Create<NamespacesController.GetQuery>());
            var actual = response.Result as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            repo.Verify(o => o.GetNamespaceAsync(It.IsAny<long>()), Times.Once);
        }
    }
}
