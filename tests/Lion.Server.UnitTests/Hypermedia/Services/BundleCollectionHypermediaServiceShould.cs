using AutoFixture;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public sealed class BundleCollectionHypermediaServiceShould : CollectionHypermediaServiceFixture
    {
        [Test]
        public void Add_Links_To_Each_Bundle()
        {
            //Arrange
            var sut = fixture.Create<BundleCollectionHypermediaService>();
            var bundles = fixture.CreateMany<Bundle>().ToList();
            var collection = new Collection<Bundle> { Data = new List<Bundle>(bundles) };

            //Act
            sut.Process(collection);

            // Assert
            collection.Data.Should().SatisfyRespectively(
                first =>
                {
                    first.Links.Should().NotBeEmpty();
                },
                second =>
                {
                    second.Links.Should().NotBeEmpty();
                },
                third =>
                {
                    third.Links.Should().NotBeEmpty();
                });
        }
    }
}
