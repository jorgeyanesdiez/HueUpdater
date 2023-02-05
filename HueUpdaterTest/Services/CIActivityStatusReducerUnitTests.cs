using FluentAssertions;
using HueUpdater.Dtos;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class CIActivityStatusReducerUnitTests
    {

        [Fact]
        public void Resolve_Null_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(null);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Fact]
        public void Resolve_Empty_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve();
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Fact]
        public void Resolve_Idle_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(CIActivityStatus.Idle);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Fact]
        public void Resolve_IdleMultiple_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(CIActivityStatus.Idle, CIActivityStatus.Idle);
            result.Should().Be(CIActivityStatus.Idle);
        }


        [Fact]
        public void Resolve_Building_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(CIActivityStatus.Building);
            result.Should().Be(CIActivityStatus.Building);
        }


        [Fact]
        public void Resolve_BuildingMultiple_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(CIActivityStatus.Building, CIActivityStatus.Building);
            result.Should().Be(CIActivityStatus.Building);
        }


        [Fact]
        public void Resolve_Mixed_IsExpected()
        {
            var sut = new CIActivityStatusReducer();
            var result = sut.Resolve(CIActivityStatus.Idle, CIActivityStatus.Building);
            result.Should().Be(CIActivityStatus.Building);
        }

    }

}
