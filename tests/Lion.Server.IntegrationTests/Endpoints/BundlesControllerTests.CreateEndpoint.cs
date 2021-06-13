using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public partial class BundlesControllerTests
    {
        [Test]
        public async Task CreateEndpoint_Returns409_WhenBundleNameIsNotUnique()
        {
            // Arrange
            var createCommand = new BundlesController.CreateCommand { Name = "greeting", Namespace = "salutations" };

            // Act
            var _ = await client.PostAsJsonAsync("api/bundles", createCommand);
            var conflictResponse = await client.PostAsJsonAsync("api/bundles", createCommand);
            var deserialized = JsonConvert.DeserializeObject<ProblemDetails>(await conflictResponse.Content.ReadAsStringAsync());

            // Assert
            conflictResponse.StatusCode.Should().Be(409);
            deserialized.Title.Should().Be("Bundle name is not available.");
        }

        [TestCase("foobar")]
        [TestCase("foo.bar")]
        [TestCase("foo-bar")]
        [TestCase("foo_bar")]
        public async Task CreateEndpoint_Returns201AndLocationOfResource_WhenBundleIsCreated(string name)
        {
            // Arrange
            var createCommand = new BundlesController.CreateCommand { Name = name, Namespace = "test" };

            // Act
            var response = await client.PostAsJsonAsync("api/bundles", createCommand);

            // Assert
            response.StatusCode.Should().Be(201);
            response.Headers.Location.Should().Be("http://localhost/api/bundles/51");
        }
    }
}
