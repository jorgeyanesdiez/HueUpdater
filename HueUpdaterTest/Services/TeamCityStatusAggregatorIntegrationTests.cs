using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Integration")]
    public class TeamCityStatusAggregatorIntegrationTests
    {

        public static readonly string url = "http://localhost";


        [Fact]
        public async Task GetActivityStatus_NotRunning_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Builds.NotRunning.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetActivityStatus();
            result.Should().Be(CIActivityStatus.Idle);
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetActivityStatus_Running_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Builds.Running.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetActivityStatus();
            result.Should().Be(CIActivityStatus.Building);
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetBuildStatus_Stable_IsExpected()
        {
            var response1 = File.ReadAllText("TeamCity.Builds.json");
            var response2 = File.ReadAllText("TeamCity.Build.Success.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response1, 200);
            httpTest.RespondWith(response2, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetBuildStatus();
            result.Should().Be(CIBuildStatus.Stable);
            httpTest.ShouldHaveCalled(url).Times(2);
        }


        [Fact]
        public async Task GetBuildStatus_Broken_IsExpected()
        {
            var response1 = File.ReadAllText("TeamCity.Builds.json");
            var response2 = File.ReadAllText("TeamCity.Build.Failure.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response1, 200);
            httpTest.RespondWith(response2, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetBuildStatus();
            result.Should().Be(CIBuildStatus.Broken);
            httpTest.ShouldHaveCalled(url).Times(2);
        }


        [Fact]
        public async Task GetRunningBuildCountAsync_NotRunning_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Builds.NotRunning.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetRunningBuildCountAsync();
            result.Should().Be(0);
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetRunningBuildCountAsync_Running_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Builds.Running.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetRunningBuildCountAsync();
            result.Should().BeGreaterThan(0);
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetBuildHrefsAsync_ValidResponse_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Builds.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var hrefs = await sut.GetBuildHrefsAsync();
            var result = hrefs.Single();
            result.Should().Be("/app/rest/buildTypes/id:bt1");
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetBuildStatusesAsync_Null_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetBuildStatusesAsync(null);
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task GetBuildStatusesAsync_Empty_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = await sut.GetBuildStatusesAsync(new string[0]);
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task GetBuildStatusesAsync_Success_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Build.Success.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var hrefs = await sut.GetBuildStatusesAsync(url);
            var result = hrefs.Single();
            result.Should().Be("SUCCESS");
            httpTest.ShouldHaveCalled(url);
        }


        [Fact]
        public async Task GetBuildStatusesAsync_Failure_IsExpected()
        {
            var response = File.ReadAllText("TeamCity.Build.Failure.json");
            using var httpTest = new HttpTest();
            httpTest.RespondWith(response, 200);
            var sut = new TeamCityStatusAggregator(url);
            var hrefs = await sut.GetBuildStatusesAsync(url);
            var result = hrefs.Single();
            result.Should().Be("FAILURE");
            httpTest.ShouldHaveCalled(url);
        }

    }

}
