using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class HueUpdaterSettingsUnitTests
    {

        [Fact]
        public void AppearancePresets_ByDefault_IsNotNull()
        {
            var sut = new HueUpdaterSettings();
            sut.AppearancePresets.Should().NotBeNull();
        }


        [Fact]
        public void StatusUrls_ByDefault_IsNotNull()
        {
            var sut = new HueUpdaterSettings();
            sut.StatusUrls.Should().NotBeNull();
        }


        [Fact]
        public void HueLights_ByDefault_IsNotNull()
        {
            var sut = new HueUpdaterSettings();
            sut.HueLights.Should().NotBeNull();
        }


        [Fact]
        public void Persistence_ByDefault_IsNotNull()
        {
            var sut = new HueUpdaterSettings();
            sut.Persistence.Should().NotBeNull();
        }


        [Fact]
        public void WorkPlan_ByDefault_IsNotNull()
        {
            var sut = new HueUpdaterSettings();
            sut.WorkPlan.Should().NotBeNull();
        }

    }

}
