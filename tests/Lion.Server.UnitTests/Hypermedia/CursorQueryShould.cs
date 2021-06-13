using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Lion.Server.Hypermedia
{
    [TestFixture]
    public class CursorQueryShould
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("foobar")]
        [TestCase("MTYwMTkxOTU5NjQxM3xZbGI4VE5EZ1dvZTlla09uWjhoZFpR")]
        public void Return_Zero_When_Encoded_String_Has_Invalid_Format(object arg)
        {
            // Arrange
            var sut = new Mock<CursorQuery>() { CallBase = true }.Object;
            sut.EncodedCursor = (string)arg;

            // Act
            var actual = sut.DecodedCursor;

            // Assert
            actual.Should().Be(0);
        }

        [TestCase("QnVuZGxlfDMw", 30)]
        public void Return_Long_When_Encoded_String_Has_Correct_Format(string arg, long expected)
        {
            // Arrange
            var sut = new Mock<CursorQuery>() { CallBase = true }.Object;
            sut.EncodedCursor = arg;

            // Act
            var actual = sut.DecodedCursor;

            // Assert
            actual.Should().Be(expected);
        }
    }
}
