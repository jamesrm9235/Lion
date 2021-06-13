using FluentAssertions;
using Lion.Server.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Lion.Server.Endpoints
{
    public partial class BundlesControllerTests
    {
        [Test]
        public async Task ListEndpoint_UsesDefaultLimit_WhenNotSpecifiedInQuery()
        {
            var @default = BundlesController.ListQuery.DefaultLimit;

            var response = await client.GetAsync("api/bundles");
            var representation = JsonConvert.DeserializeObject<Collection<Bundle>>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(200);
            representation.Data.Should().HaveCount(@default);
        }

        [Test]
        public async Task ListEndpoint_ReturnsCountEqualToLimit_WhenSpecifiedInQuery()
        {
            var count = 5;

            var response = await client.GetAsync($"api/bundles?limit={count}");
            var representation = JsonConvert.DeserializeObject<Collection<Bundle>>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(200);
            representation.Data.Should().HaveCount(count);
        }

        [Test]
        public async Task ListEndpoint_ReturnsTopResults_WhenCursorIsInvalid()
        {
            // Arrange
            var invalidCursor = "foobar";

            var response = await client.GetAsync($"api/bundles?cursor={invalidCursor}");
            var representation = JsonConvert.DeserializeObject<Collection<Bundle>>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(200);
            representation.Data.Should().BeInDescendingOrder(o => o.Id);
        }

        [Test]
        public async Task ListEndpoint_ReturnsRepresentations_WithLinks()
        {
            var count = 3;

            var response = await client.GetAsync($"api/bundles?limit={count}");
            var representation = JsonConvert.DeserializeObject<Collection<Bundle>>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(200);
            representation.Data.Should().SatisfyRespectively(
                first => first.Links.Should().NotBeEmpty(),
                second => second.Links.Should().NotBeEmpty(),
                third => third.Links.Should().NotBeEmpty());
        }

        [Test]
        public async Task ListEndpoint_GetsAll_WhenNextLinksAreFollowed()
        {
            // 50 items in fake repo
            // 20 are requested per request

            // 1
            var response_1 = await client.GetAsync($"api/bundles?limit=20");
            var representation_1 = JsonConvert.DeserializeObject<Collection<Bundle>>(await response_1.Content.ReadAsStringAsync());

            response_1.StatusCode.Should().Be(200);
            representation_1.Data.Should().HaveCount(20);
            representation_1.Links.Should().SatisfyRespectively(link =>
            {
                link.Rel.Should().Be("next");
                link.Href.Should().MatchRegex("api/bundles\\?limit=20&cursor=[a-zA-Z0-9\\+/=]+$");
            });

            // 2
            var response_2 = await client.GetAsync(representation_1.Links.First(o => o.Rel == "next").Href);
            var representation_2 = JsonConvert.DeserializeObject<Collection<Bundle>>(await response_2.Content.ReadAsStringAsync());

            response_2.StatusCode.Should().Be(200);
            representation_2.Data.Should().HaveCount(20).And.NotBeEquivalentTo(representation_1.Data);
            representation_2.Links.Should().SatisfyRespectively(link =>
            {
                link.Rel.Should().Be("next");
                link.Href.Should()
                .MatchRegex("api/bundles\\?limit=20&cursor=[a-zA-Z0-9\\+/=]+$")
                .And
                .NotBeEquivalentTo(representation_1.Links.First(o => o.Rel == "next").Href);
            });

            // 3
            var response_3 = await client.GetAsync(representation_2.Links.First(o => o.Rel == "next").Href);
            var representation_3 = JsonConvert.DeserializeObject<Collection<Bundle>>(await response_3.Content.ReadAsStringAsync());

            response_3.StatusCode.Should().Be(200);
            representation_3.Data.Should().HaveCount(10).And.NotBeEquivalentTo(representation_2.Data);
            representation_3.Links.Should().HaveCount(0);
        }
    }
}
