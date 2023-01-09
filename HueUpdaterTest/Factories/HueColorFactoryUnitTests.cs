using System;
using FluentAssertions;
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


        [Fact]
        public void CreateBlue_ReturnsExpected()
        {
            int testHue = 0xC0FFEE;
            var appearancePresetSettings = new AppearancePresetSettings() { BlueHue = testHue };
            var sut = new HueColorFactory(appearancePresetSettings);
            var result = sut.CreateBlue();
            result.Should().NotBeNull();
            result.Hue.Should().Be(testHue);
        }


        [Fact]
        public void CreateGreen_ReturnsExpected()
        {
            int testHue = 0xC0FFEE;
            var appearancePresetSettings = new AppearancePresetSettings() { GreenHue = testHue };
            var sut = new HueColorFactory(appearancePresetSettings);
            var result = sut.CreateGreen();
            result.Should().NotBeNull();
            result.Hue.Should().Be(testHue);
        }


        [Fact]
        public void CreateRed_ReturnsExpected()
        {
            int testHue = 0xC0FFEE;
            var appearancePresetSettings = new AppearancePresetSettings() { RedHue = testHue };
            var sut = new HueColorFactory(appearancePresetSettings);
            var result = sut.CreateRed();
            result.Should().NotBeNull();
            result.Hue.Should().Be(testHue);
        }


        [Fact]
        public void CreateYellow_ReturnsExpected()
        {
            int testHue = 0xC0FFEE;
            var appearancePresetSettings = new AppearancePresetSettings() { YellowHue = testHue };
            var sut = new HueColorFactory(appearancePresetSettings);
            var result = sut.CreateYellow();
            result.Should().NotBeNull();
            result.Hue.Should().Be(testHue);
        }

    }

}
