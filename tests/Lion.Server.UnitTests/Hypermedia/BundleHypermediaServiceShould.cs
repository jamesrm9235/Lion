using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class BundleHypermediaServiceShould
    {
        [Test]
        public void Add_Links_To_Bundle()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<BundleHypermediaService>();
            var bundle = new Bundle();

            // Act
            sut.Process(bundle);

            // Assert
            bundle.Links.Should().HaveCount(2);
        }
    }
}
