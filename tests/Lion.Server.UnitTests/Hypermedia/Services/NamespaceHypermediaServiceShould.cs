using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class NamespaceHypermediaServiceShould
    {
        [Test]
        public void Add_Links_To_Namespace()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<NamespaceHypermediaService>();
            var @namespace = new Namespace();

            // Act
            sut.Process(@namespace);

            // Assert
            @namespace.Links.Should().HaveCount(2)
                .And.ContainKey("self")
                .And.ContainKey("bundles");
        }
    }
}
