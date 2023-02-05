using System;
using FluentAssertions;
using HueUpdater.Dtos;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class LightColorResolverUnitTests
    {

        [Fact]
        public void Resolve_Null_Throws()
        {
            var sut = new LightColorResolver();
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_IdleStable_IsExpected()
        {
            var sut = new LightColorResolver();
            var result = sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Idle,
                BuildStatus = CIBuildStatus.Stable
            });

            result.Should().Be(LightColor.Green);
        }


        [Fact]
        public void Resolve_BuildingStable_IsExpected()
        {
            var sut = new LightColorResolver();
            var result = sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Building,
                BuildStatus = CIBuildStatus.Stable
            });

            result.Should().Be(LightColor.Blue);
        }


        [Fact]
        public void Resolve_IdleBroken_IsExpected()
        {
            var sut = new LightColorResolver();
            var result = sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Idle,
                BuildStatus = CIBuildStatus.Broken
            });

            result.Should().Be(LightColor.Red);
        }


        [Fact]
        public void Resolve_BuildingBroken_IsExpected()
        {
            var sut = new LightColorResolver();
            var result = sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Building,
                BuildStatus = CIBuildStatus.Broken
            });

            result.Should().Be(LightColor.Yellow);
        }

    }

}
