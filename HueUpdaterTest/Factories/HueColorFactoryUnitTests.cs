using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HueUpdater.Models;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Factories
{

    [Trait("TestType", "Unit")]
    public class HueColorFactoryUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new HueColorFactory(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Theory]
        [InlineData(LightColor.Blue, "BlueHue")]
        [InlineData(LightColor.Green, "GreenHue")]
        [InlineData(LightColor.Red, "RedHue")]
        [InlineData(LightColor.Yellow, "YellowHue")]
        public void Resolve_IsExpected(LightColor lightColor, string huePropName)
        {
            var appearancePresetSettings = new AppearancePresetSettings();
            var presetHueProp = typeof(AppearancePresetSettings).GetProperty(huePropName);
            var huePropValue = new Random(0xC0FFEE).Next();
            presetHueProp.SetValue(appearancePresetSettings, huePropValue);

            var sut = new HueColorFactory(appearancePresetSettings);
            var result = sut.Resolve(lightColor);
            result.Should().NotBeNull();
            result.Hue.Should().Be(huePropValue);
        }

    }

}
