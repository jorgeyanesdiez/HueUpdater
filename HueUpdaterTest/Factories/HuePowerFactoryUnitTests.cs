using FluentAssertions;
using Xunit;

namespace HueUpdater.Factories
{

    [Trait("TestType", "Unit")]
    public class HuePowerFactoryUnitTests
    {

        [Fact]
        public void CreateOff_IsExpected()
        {
            var sut = () => HuePowerFactory.CreateOff();
            var result = sut();
            result.Should().NotBeNull();
            result.On.Should().Be(HuePowerFactory.Off);
        }


        [Fact]
        public void CreateOn_IsExpected()
        {
            var sut = () => HuePowerFactory.CreateOn();
            var result = sut();
            result.Should().NotBeNull();
            result.On.Should().Be(HuePowerFactory.On);
        }


        [Fact]
        public void Values_OnOff_IsExpected()
        {
            HuePowerFactory.On.Should().NotBe(HuePowerFactory.Off);
        }

    }

}
