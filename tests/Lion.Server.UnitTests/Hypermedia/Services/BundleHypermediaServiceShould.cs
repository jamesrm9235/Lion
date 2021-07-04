using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class BundleHypermediaServiceShould
    {
        private BundleHypermediaService sut;

        [SetUp]
        public void BeforeEachTest()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            sut = fixture.Create<BundleHypermediaService>();
        }

        [Test]
        public void Add_Links_To_Bundle()
        {
            // Arrange
            var bundle = new Bundle { Messages = new List<Message>(), Namespace = new Namespace() };

            // Act
            sut.Process(bundle);

            // Assert
            bundle.Links.Should().HaveCount(2)
                .And.ContainKey("self")
                .And.ContainKey("message");
            bundle.Namespace.Links.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);
        }

        [Test]
        public void Add_Links_To_Embedded_Messages()
        {
            // Arrange
            var bundle = new Bundle { Messages = new List<Message> { new Message() }, Namespace = new Namespace() };

            // Act
            sut.Process(bundle);

            // Assert
            bundle.Messages.Should().SatisfyRespectively(
                first =>
                {
                    first.Links.Should().HaveCountGreaterOrEqualTo(1);
                });
        }
    }
}
