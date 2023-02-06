using System;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class JsonNetFileSerializerUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new JsonNetFileSerializer<object>(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Constructor_Invalid_Throws(string input)
        {
            Action action = () => new JsonNetFileSerializer<object>(input);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

    }

}
