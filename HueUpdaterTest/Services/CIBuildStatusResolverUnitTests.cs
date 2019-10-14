using FluentAssertions;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class CIBuildStatusResolverUnitTests
    {

        [Fact]
        public void Resolve_Null_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(null);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void Resolve_Empty_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve();
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void Resolve_Stable_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(CIBuildStatus.Stable);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void Resolve_StableMultiple_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(CIBuildStatus.Stable, CIBuildStatus.Stable);
            result.Should().Be(CIBuildStatus.Stable);
        }


        [Fact]
        public void Resolve_Broken_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(CIBuildStatus.Broken);
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Fact]
        public void Resolve_BrokenMultiple_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(CIBuildStatus.Broken, CIBuildStatus.Broken);
            result.Should().Be(CIBuildStatus.Broken);
        }


        [Fact]
        public void Resolve_Mixed_IsExpected()
        {
            var sut = new CIBuildStatusResolver();
            var result = sut.Resolve(CIBuildStatus.Stable, CIBuildStatus.Broken);
            result.Should().Be(CIBuildStatus.Broken);
        }

    }

}
