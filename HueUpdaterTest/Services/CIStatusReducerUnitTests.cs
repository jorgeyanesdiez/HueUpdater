using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using Moq;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class CIStatusReducerUnitTests
    {

        [Fact]
        public void Constructor_NullActivityStatusReducer_Throws()
        {
            Action action = () => new CIStatusReducer(null, Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>());
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullBuildStatusReducer_Throws()
        {
            Action action = () => new CIStatusReducer(Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(), null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_CallsExpected()
        {
            var activityStatus = CIActivityStatus.Building;
            var activityStatusReducerMock = new Mock<IResolver<CIActivityStatus[], CIActivityStatus>>();
            activityStatusReducerMock.Setup(m => m.Resolve(It.IsAny<CIActivityStatus[]>()))
                .Returns(activityStatus)
                .Verifiable();

            var buildStatus = CIBuildStatus.Broken;
            var buildStatusReducerMock = new Mock<IResolver<CIBuildStatus[], CIBuildStatus>>();
            buildStatusReducerMock.Setup(m => m.Resolve(It.IsAny<CIBuildStatus[]>()))
                .Returns(buildStatus)
                .Verifiable();

            var sut = new CIStatusReducer(activityStatusReducerMock.Object, buildStatusReducerMock.Object);
            var result = sut.Resolve(Array.Empty<CIStatus>());

            activityStatusReducerMock.Verify(m => m.Resolve(It.IsAny<CIActivityStatus[]>()), Times.Once);
            buildStatusReducerMock.Verify(m => m.Resolve(It.IsAny<CIBuildStatus[]>()), Times.Once);
            result.Should().NotBeNull();
            result.ActivityStatus.Should().Be(activityStatus);
            result.BuildStatus.Should().Be(buildStatus);
        }

    }

}
