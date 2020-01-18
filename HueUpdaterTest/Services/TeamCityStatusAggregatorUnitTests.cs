using System;
using FluentAssertions;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class TeamCityStatusAggregatorUnitTests
    {

        public static readonly string url = "http://localhost";


        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new TeamCityStatusAggregator(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Theory]
        [InlineData("invalidUri")]
        public void Constructor_Invalid_Throws(string invalidUri)
        {
            Action action = () => new TeamCityStatusAggregator(invalidUri);
            action.Should().ThrowExactly<UriFormatException>();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ResolveActivityStatus_Idle_IsExpected(int count)
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveActivityStatus(count);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ResolveActivityStatus_Building_IsExpected(int count)
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveActivityStatus(count);
            result.Should().Be(CIActivityStatus.Building);
        }


        [Fact]
        public void ResolveBuildStatus_Empty_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus(new string[0]);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void ResolveBuildStatus_Success_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("SUCCESS");
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void ResolveBuildStatus_Successes_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("SUCCESS", "SUCCESS");
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void ResolveBuildStatus_Failure_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("FAILURE");
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Fact]
        public void ResolveBuildStatus_Failures_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("FAILURE", "FAILURE");
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Fact]
        public void ResolveBuildStatus_Mixed_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("SUCCESS", "FAILURE");
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Fact]
        public void ResolveBuildStatus_Unexpected_IsExpected()
        {
            var sut = new TeamCityStatusAggregator(url);
            var result = sut.ResolveBuildStatus("SUCCESS", "UNEXPECTED");
            result.Should().Be(CIBuildStatus.Broken);
        }

    }

}
