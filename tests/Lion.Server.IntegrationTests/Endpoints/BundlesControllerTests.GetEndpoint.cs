using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public partial class BundlesControllerTests
    {
        [Test]
        public async Task GetEndpoint_Returns200AndRepresentation_WhenResourceIsFound()
        {
            var response = await client.GetAsync("api/bundles/1");
            var representation = JsonConvert.DeserializeObject<Models.Bundle>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(200);
            representation.Links.Should().SatisfyRespectively(
                first =>
                {
                    first.Href.Should().MatchRegex("api/bundles/1");
                    first.Rel.Should().Be("self");
                    first.Method.Should().Be("GET");
                    first.MediaType.Should().Be("application/json");
                },
                second =>
                {
                    second.Href.Should().MatchRegex("api/bundles/1");
                    second.Rel.Should().Be("delete");
                    second.Method.Should().Be("DELETE");
                    second.MediaType.Should().Be("application/json");
                });
        }

        [Test]
        public async Task GetEndpoint_Returns404_WhenResourceIsNotFound()
        {
            var response = await client.GetAsync("api/bundles/0");

            response.StatusCode.Should().Be(404);
        }
    }
}
