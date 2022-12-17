using System;
using FluentAssertions;
using HueUpdater.Factories;
using HueUpdater.Models;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueColorResolverUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new HueColorResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Null_Throws()
        {
            var hueColorFactoryMock = new HueColorFactory(new AppearancePresetSettings());
            var sut = new HueColorResolver(hueColorFactoryMock);
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Any_IsNotNull()
        {
            var hueColorFactoryMock = new HueColorFactory(new AppearancePresetSettings());
            var sut = new HueColorResolver(hueColorFactoryMock);
            var result = sut.Resolve(new CIStatus());
            result.Should().NotBeNull();
        }

    }

}
