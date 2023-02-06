using System;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueInvokerUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new HueInvoker(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }

    }

}
