using System;
using FluentAssertions;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueColorResolverUnitTests
    {

        [Fact]
        public void Resolve_Null_Throws()
        {
            var sut = new HueColorResolver();
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Any_IsNotNull()
        {
            var sut = new HueColorResolver();
            var result = sut.Resolve(new CIStatus());
            result.Should().NotBeNull();
        }

    }

}
