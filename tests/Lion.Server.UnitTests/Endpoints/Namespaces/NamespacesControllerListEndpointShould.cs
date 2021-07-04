using AutoFixture;
using FluentAssertions;
using Lion.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    [TestFixture]
    public sealed class NamespacesControllerListEndpointShould : NamespacesControllerFixture
    {
        [Test]
        public async Task Query_Limit_Plus_One()
        {
            // Arrange
            var query = new NamespacesController.ListQuery { Limit = 2 };
            repo.Setup(o => o.ListNamespacesAsync(It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(new Fixture().CreateMany<Common.Entities.Namespace>().ToList().AsReadOnly());

            // Act
            var response = await sut.ListNamespacesAsync(query);
            var actual = response.Result as OkObjectResult;

            // Assert
            repo.Verify(o => o.ListNamespacesAsync(It.IsAny<long>(), query.Limit + 1), Times.Once);
            actual.Value.Should().BeOfType<Collection<Models.Namespace>>();
        }
    }
}
