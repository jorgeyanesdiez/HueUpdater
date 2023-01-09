using System;
using FluentAssertions;
using HueUpdater.Dtos;
using HueUpdater.Factories;
using HueUpdater.Models;
using Moq;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueColorFactoryUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new HueColorResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Null_Throws()
        {
            var hueColorFactoryMock = Mock.Of<IHueColorFactory>();
            var sut = new HueColorResolver(hueColorFactoryMock);
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Any_IsNotNull()
        {
            var hueColorFactoryMock = new Mock<IHueColorFactory>();
            var mockHueColor = new HueColor();
            hueColorFactoryMock.Setup(m => m.CreateGreen()).Returns(mockHueColor);

            var sut = new HueColorResolver(hueColorFactoryMock.Object);
            var result = sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Idle,
                BuildStatus = CIBuildStatus.Stable
            });
            result.Should().Be(mockHueColor);
        }


        [Fact]
        public void Resolve_IdleStable_CallsExpected()
        {
            var hueColorFactoryMock = new Mock<IHueColorFactory>();
            hueColorFactoryMock.Setup(m => m.CreateGreen()).Verifiable();

            var sut = new HueColorResolver(hueColorFactoryMock.Object);
            sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Idle,
                BuildStatus = CIBuildStatus.Stable
            });

            hueColorFactoryMock.Verify(m => m.CreateGreen(), Times.Once);
        }


        [Fact]
        public void Resolve_BuildingStable_CallsExpected()
        {
            var hueColorFactoryMock = new Mock<IHueColorFactory>();
            hueColorFactoryMock.Setup(m => m.CreateBlue()).Verifiable();

            var sut = new HueColorResolver(hueColorFactoryMock.Object);
            sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Building,
                BuildStatus = CIBuildStatus.Stable
            });

            hueColorFactoryMock.Verify(m => m.CreateBlue(), Times.Once);
        }


        [Fact]
        public void Resolve_IdleBroken_CallsExpected()
        {
            var hueColorFactoryMock = new Mock<IHueColorFactory>();
            hueColorFactoryMock.Setup(m => m.CreateRed()).Verifiable();

            var sut = new HueColorResolver(hueColorFactoryMock.Object);
            sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Idle,
                BuildStatus = CIBuildStatus.Broken
            });

            hueColorFactoryMock.Verify(m => m.CreateRed(), Times.Once);
        }


        [Fact]
        public void Resolve_BuildingBroken_CallsExpected()
        {
            var hueColorFactoryMock = new Mock<IHueColorFactory>();
            hueColorFactoryMock.Setup(m => m.CreateYellow()).Verifiable();

            var sut = new HueColorResolver(hueColorFactoryMock.Object);
            sut.Resolve(new CIStatus()
            {
                ActivityStatus = CIActivityStatus.Building,
                BuildStatus = CIBuildStatus.Broken
            });

            hueColorFactoryMock.Verify(m => m.CreateYellow(), Times.Once);
        }

    }

}
