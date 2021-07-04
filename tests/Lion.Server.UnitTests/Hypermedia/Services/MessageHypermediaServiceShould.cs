using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Lion.Server.Models;
using NUnit.Framework;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class MessageHypermediaServiceShould
    {
        [Test]
        public void Add_Links_To_Message()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<MessageHypermediaService>();
            var message = new Message();

            // Act
            sut.Process(message);

            // Assert
            message.Links.Should().HaveCount(1).And.ContainKey("self");
        }
    }
}
