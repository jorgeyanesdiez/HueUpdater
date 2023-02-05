using FluentAssertions;
using Xunit;

namespace HueUpdater.Models
{

    [Trait("TestType", "Unit")]
    public class LightStatusUnitTests
    {

        [Fact]
        public void Equals_Null_IsExpected()
        {
            var sut = new LightStatus();
            var result = sut.Equals(null);
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData(null, LightPower.Off)]
        [InlineData(LightColor.Blue, LightPower.Off)]
        [InlineData(LightColor.Green, LightPower.Off)]
        [InlineData(LightColor.Red, LightPower.Off)]
        [InlineData(LightColor.Yellow, LightPower.Off)]
        [InlineData(null, LightPower.On)]
        [InlineData(LightColor.Blue, LightPower.On)]
        [InlineData(LightColor.Green, LightPower.On)]
        [InlineData(LightColor.Red, LightPower.On)]
        [InlineData(LightColor.Yellow, LightPower.On)]
        public void Equals_Self_IsExpected(LightColor? lightColor, LightPower lightPower)
        {
            var sut = new LightStatus { Color = lightColor, Power = lightPower };
            var result = sut.Equals(sut);
            result.Should().BeTrue();
        }


        [Theory]
        [InlineData(null, LightPower.Off)]
        [InlineData(LightColor.Blue, LightPower.Off)]
        [InlineData(LightColor.Green, LightPower.Off)]
        [InlineData(LightColor.Red, LightPower.Off)]
        [InlineData(LightColor.Yellow, LightPower.Off)]
        [InlineData(null, LightPower.On)]
        [InlineData(LightColor.Blue, LightPower.On)]
        [InlineData(LightColor.Green, LightPower.On)]
        [InlineData(LightColor.Red, LightPower.On)]
        [InlineData(LightColor.Yellow, LightPower.On)]
        public void Equals_Same_IsExpected(LightColor? lightColor, LightPower lightPower)
        {
            var sut = new LightStatus { Color = lightColor, Power = lightPower };
            var target = new LightStatus { Color = lightColor, Power = lightPower };
            var result = sut.Equals(target);
            result.Should().BeTrue();
        }


        [Theory]
        [InlineData(null, LightColor.Blue)]
        [InlineData(null, LightColor.Green)]
        [InlineData(null, LightColor.Red)]
        [InlineData(null, LightColor.Yellow)]
        [InlineData(LightColor.Blue, null)]
        [InlineData(LightColor.Blue, LightColor.Green)]
        [InlineData(LightColor.Blue, LightColor.Red)]
        [InlineData(LightColor.Blue, LightColor.Yellow)]
        [InlineData(LightColor.Green, null)]
        [InlineData(LightColor.Green, LightColor.Blue)]
        [InlineData(LightColor.Green, LightColor.Red)]
        [InlineData(LightColor.Green, LightColor.Yellow)]
        [InlineData(LightColor.Red, null)]
        [InlineData(LightColor.Red, LightColor.Blue)]
        [InlineData(LightColor.Red, LightColor.Green)]
        [InlineData(LightColor.Red, LightColor.Yellow)]
        [InlineData(LightColor.Yellow, null)]
        [InlineData(LightColor.Yellow, LightColor.Blue)]
        [InlineData(LightColor.Yellow, LightColor.Green)]
        [InlineData(LightColor.Yellow, LightColor.Red)]
        public void Equals_DifferentLightColor_IsExpected(LightColor? lightColor1, LightColor? lightColor2)
        {
            var sut = new LightStatus { Color = lightColor1, Power = LightPower.On };
            var target = new LightStatus { Color = lightColor2, Power = LightPower.On };
            var result = sut.Equals(target);
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData(null)]
        [InlineData(LightColor.Blue)]
        [InlineData(LightColor.Green)]
        [InlineData(LightColor.Red)]
        [InlineData(LightColor.Yellow)]
        public void Equals_DifferentLightPower_IsExpected(LightColor? lightColor)
        {
            var sut = new LightStatus { Color = lightColor, Power = LightPower.On };
            var target = new LightStatus { Color = lightColor, Power = LightPower.Off };
            var result = sut.Equals(target);
            result.Should().BeFalse();
        }

    }

}
