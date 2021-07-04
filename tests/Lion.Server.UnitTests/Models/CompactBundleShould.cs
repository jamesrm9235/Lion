using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Lion.Server.Models
{
    [TestFixture]
    public class CompactBundleShould
    {
        [Test]
        public void Concatenate_Namespace_And_Bundle()
        {
            // Arrange
            var messages = new Fixture().Create<Dictionary<string, string>>();

            // Act
            var actual = new CompactBundle(1, "namespace", "bundle", messages);

            // Assert
            actual.Should().ContainKey("namespace.bundle");
        }
    }
}
