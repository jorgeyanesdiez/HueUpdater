using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class AppSettingsUnitTests
    {

        [Fact]
        public void Schedules_ByDefault_IsNotNull()
        {
            var sut = new AppSettings();
            sut.HueUpdater.Should().NotBeNull();
        }

    }

}
