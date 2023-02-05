using FluentAssertions;
using Xunit;

namespace HueUpdater.Factories
{

    [Trait("TestType", "Unit")]
    public class HueAlertFactoryUnitTests
    {

        [Fact]
        public void CreateBlink_IsExpected()
        {
            var sut = () => HueAlertFactory.CreateBlink();
            var result = sut();
            result.Should().NotBeNull();
            result.Alert.Should().Be(HueAlertFactory.Blink);
        }

    }

}
