using AutoFixture;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public sealed class NamespaceCollectionHypermediaServiceShould : CollectionHypermediaServiceFixture
    {
        [Test]
        public void Add_Links_To_Each_Namespace()
        {
            //Arrange
            var sut = fixture.Create<NamespaceCollectionHypermediaService>();
            var namespaces = fixture.CreateMany<Namespace>().ToList();
            var collection = new Collection<Namespace> { Data = new List<Namespace>(namespaces) };

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
