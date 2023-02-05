using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class WorkPlanSettingsUnitTests
    {

        [Fact]
        public void Schedules_ByDefault_IsNotNull()
        {
            var sut = new WorkPlanSettings();
            sut.Schedules.Should().NotBeNull();
        }


        [Fact]
        public void Calendar_ByDefault_IsNotNull()
        {
            var sut = new WorkPlanSettings();
            sut.Calendar.Should().NotBeNull();
        }


        [Fact]
        public void Overrides_ByDefault_IsNotNull()
        {
            var sut = new WorkPlanSettings();
            sut.Overrides.Should().NotBeNull();
        }

    }

}
