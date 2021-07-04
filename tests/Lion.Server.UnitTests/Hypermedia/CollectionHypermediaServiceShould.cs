using AutoFixture;
using FluentAssertions;
using Lion.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public sealed class CollectionHypermediaServiceShould : CollectionHypermediaServiceFixture
    {
        [Test]
        public void Add_Next_Page_Link_When_Collection_Size_Is_Greater_Than_Limit_By_One()
        {
            // Arrange
            var sut = fixture.Create<FakeCollectionHypermediaService>();
            AddQuery("limit", "2");
            var objects = fixture.CreateMany<FakeRepresentation>().ToList(); // collection size is 3
            var collection = new Collection<FakeRepresentation> { Data = new List<FakeRepresentation>(objects) };

            // Act
            sut.Process(collection);

            // Assert
            collection.Links.Should().HaveCount(1).And.ContainKey("next");
            collection.Data.Should().NotContain(objects.Last());
        }

        private class FakeRepresentation : Representation
        {
        }

        private class FakeCollectionHypermediaService : CollectionHypermediaService<FakeRepresentation>
        {
            public FakeCollectionHypermediaService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
                : base(httpContextAccessor, linkGenerator)
            {
            }

            protected override int Limit { get => 10; }
            protected override string Endpoint { get => "Fake"; }

            protected override void AddLinksToCollectionObjects(Collection<FakeRepresentation> representation)
            {
                // no-op
            }

            protected override string GenerateCursor(FakeRepresentation representation)
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
