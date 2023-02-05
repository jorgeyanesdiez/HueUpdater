using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class ScheduleSettingsUnitTests
    {

        [Fact]
        public void Hours_ByDefault_IsNotNull()
        {
            var sut = new ScheduleSettings();
            sut.Hours.Should().NotBeNull();
        }

    }

}
