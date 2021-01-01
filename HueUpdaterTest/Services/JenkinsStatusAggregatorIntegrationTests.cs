using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl;
using Flurl.Http.Testing;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Integration")]
    public class JenkinsStatusAggregatorIntegrationTests
    {

        public static readonly string url = "http://localhost";


        [Theory]
        [InlineData("BlueYellowGrey")]
        public async Task GetActivityStatus_IdleJobs_IsExpected(string partialName)
        {
            var response = File.ReadAllText($"Jenkins.{partialName}.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url);
            var result = await sut.GetActivityStatus();
            result.Should().Be(CIActivityStatus.Idle);
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }


        [Theory]
        [InlineData("BlueAnimeYellowGrey")]
        [InlineData("BlueBlueAnime")]
        public async Task GetActivityStatus_BuildingJobs_IsExpected(string partialName)
        {
            var response = File.ReadAllText($"Jenkins.{partialName}.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url);
            var result = await sut.GetActivityStatus();
            result.Should().Be(CIActivityStatus.Building);
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }


        [Theory]
        [InlineData("BlueBlueAnime")]
        public async Task GetBuildStatus_StableJobs_IsExpected(string partialName)
        {
            var response = File.ReadAllText($"Jenkins.{partialName}.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url);
            var result = await sut.GetBuildStatus();
            result.Should().Be(CIBuildStatus.Stable);
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }


        [Theory]
        [InlineData("BlueRed")]
        [InlineData("BlueYellowGrey")]
        public async Task GetBuildStatus_BrokenJobs_IsExpected(string partialName)
        {
            var response = File.ReadAllText($"Jenkins.{partialName}.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url);
            var result = await sut.GetBuildStatus();
            result.Should().Be(CIBuildStatus.Broken);
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }


        [Fact]
        public async Task GetJobColorsAsync_BlueYellowGrey_IsExpected()
        {
            var response = File.ReadAllText("Jenkins.BlueYellowGrey.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url);
            var result = await sut.GetJobColorsAsync();
            result.Should().Contain("blue", "yellow");
            result.Should().NotContain("grey");
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }


        [Fact]
        public async Task GetJobColorsAsync_Filtered_IsExpected()
        {
            var response = File.ReadAllText("Jenkins.BlueYellowGrey.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new JenkinsStatusAggregator(url, "JOB1");
            var result = await sut.GetJobColorsAsync();
            result.Should().Contain("blue");
            result.Should().HaveCount(1);
            httpTest.ShouldHaveCalled(Url.Combine(url, "*"));
        }

    }

}
