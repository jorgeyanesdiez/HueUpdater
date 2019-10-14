using System;
using System.Linq;
using FluentAssertions;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class JenkinsStatusAggregatorUnitTests
    {

        public static readonly string url = "http://localhost";


        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new JenkinsStatusAggregator(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Theory]
        [InlineData("invalidUri")]
        public void Constructor_Invalid_Throws(string invalidUri)
        {
            Action action = () => new JenkinsStatusAggregator(invalidUri);
            action.Should().ThrowExactly<UriFormatException>();
        }


        [Fact]
        public void ResolveActivityStatus_Empty_IsExpected()
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveActivityStatus(new string[0]);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Theory]
        [InlineData("red")]
        [InlineData("yellow")]
        [InlineData("blue")]
        [InlineData("grey")]
        [InlineData("disabled")]
        [InlineData("aborted")]
        [InlineData("notbuilt")]
        [InlineData("UNEXPECTED")]
        public void ResolveActivityStatus_RegularColors_IsExpected(string regularColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveActivityStatus(regularColor);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Theory]
        [InlineData("red_anime")]
        [InlineData("yellow_anime")]
        [InlineData("blue_anime")]
        [InlineData("grey_anime")]
        [InlineData("disabled_anime")]
        [InlineData("aborted_anime")]
        [InlineData("notbuilt_anime")]
        [InlineData("UNEXPECTED_anime")]
        public void ResolveActivityStatus_AnimatedColors_IsExpected(string animatedColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveActivityStatus(animatedColor);
            result.Should().Be(CIActivityStatus.Building);
        }


        [Theory]
        [InlineData("red", "red_anime")]
        [InlineData("UNEXPECTED", "UNEXPECTED_anime")]
        public void ResolveActivityStatus_MixedColors_IsExpected(string regularColor, string animatedColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveActivityStatus(regularColor, animatedColor);
            result.Should().Be(CIActivityStatus.Building);
        }


        [Fact]
        public void ResolveBuildStatus_Empty_IsExpected()
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveBuildStatus(new string[0]);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Theory]
        [InlineData("blue")]
        [InlineData("blue_anime")]
        public void ResolveBuildStatus_StableColors_IsExpected(string stableColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveBuildStatus(stableColor);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Theory]
        [InlineData("red")]
        [InlineData("yellow")]
        [InlineData("grey")]
        [InlineData("disabled")]
        [InlineData("aborted")]
        [InlineData("notbuilt")]
        [InlineData("UNEXPECTED")]
        [InlineData("red_anime")]
        [InlineData("yellow_anime")]
        [InlineData("grey_anime")]
        [InlineData("disabled_anime")]
        [InlineData("aborted_anime")]
        [InlineData("notbuilt_anime")]
        [InlineData("UNEXPECTED_anime")]
        public void ResolveBuildStatus_BrokenColors_IsExpected(string brokenColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveBuildStatus(brokenColor);
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Theory]
        [InlineData("blue", "red")]
        [InlineData("blue_anime", "red_anime")]
        [InlineData("UNEXPECTED", "UNEXPECTED_anime")]
        public void ResolveBuildStatus_MixedColors_IsExpected(string regularColor, string animatedColor)
        {
            var sut = new JenkinsStatusAggregator(url);
            var result = sut.ResolveBuildStatus(regularColor, animatedColor);
            result.Should().Be(CIBuildStatus.Broken);
        }

    }

}
