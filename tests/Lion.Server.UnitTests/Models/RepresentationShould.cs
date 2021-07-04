using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Models
{
    [TestFixture]
    public class RepresentationShould
    {
        [Test]
        public void Replace_Link_When_Same_Key_Is_Added()
        {
            // Arrange
            var fixture = new Fixture();
            var key = fixture.Create<string>();
            var link_1 = fixture.Create<Link>();
            var link_2 = fixture.Create<Link>();
            var sut = new Mock<Representation>().Object;

            // Act
            sut.AddLink(key, link_1);
            sut.AddLink(key, link_2);

            // Assert
            sut.Links.Should().ContainKey(key).And.ContainValue(link_2).And.NotContainValue(link_1);
        }
    }
}
