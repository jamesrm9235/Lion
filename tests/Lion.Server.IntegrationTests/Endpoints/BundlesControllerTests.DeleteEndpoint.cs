using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public partial class BundlesControllerTests
    {
        [Test]
        public async Task DeleteEndpoint_Returns404_WhenResourceIsNotFound()
        {
            var response = await client.DeleteAsync("api/bundles/0");

            response.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task DeleteEndpoint_Returns204_WhenResourceIsDeleted()
        {
            var response = await client.DeleteAsync("api/bundles/1");

            response.StatusCode.Should().Be(204);
        }
    }
}
