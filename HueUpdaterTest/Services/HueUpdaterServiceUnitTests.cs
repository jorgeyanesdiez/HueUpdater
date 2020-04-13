using System;
using System.Dynamic;
using System.Threading.Tasks;
using FluentAssertions;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;
using HueUpdater.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueUpdaterServiceUnitTests
    {

        [Fact]
        public void Constructor_NullAppLifetime_Throws()
        {
            Action action = () => new HueUpdaterService(
                null,
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullLogger_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                null,
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullActivityStatusResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                null,
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullBuildStatusResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                null,
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullHueAlertResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                null,
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullHueColorResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                null,
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullHueInvoker_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                null,
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullSerializer_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                null,
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullScheduleApplicabilityResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                null,
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullScheduleResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                null,
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullActivityStatusAggregators_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                null,
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullBuildStatusAggregators_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                null
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_EmptyActivityStatusAggregators_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new IActivityStatusAggregator<Task<CIActivityStatus>>[0],
                new[] { Mock.Of<IBuildStatusAggregator<Task<CIBuildStatus>>>() }
            );
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void Constructor_EmptyBuildStatusAggregators_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                Mock.Of<IResolver<CIActivityStatus[], CIActivityStatus>>(),
                Mock.Of<IResolver<CIBuildStatus[], CIBuildStatus>>(),
                Mock.Of<IResolver<CIStatusChangeQuery, HueAlert>>(),
                Mock.Of<IResolver<CIStatus, HueColor>>(),
                Mock.Of<IHueInvoker>(),
                Mock.Of<ISerializer>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(),
                new[] { Mock.Of<IActivityStatusAggregator<Task<CIActivityStatus>>>() },
                new IBuildStatusAggregator<Task<CIBuildStatus>>[0]
            );
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }


        [Fact]
        public async Task UpdateHueEndpointAsync_CalledExpected()
        {
            var appLifetimeMock = Mock.Of<IHostApplicationLifetime>();
            var loggerMock = Mock.Of<ILogger<HueUpdaterService>>();
            var activityStatusResolverMock = new Mock<IResolver<CIActivityStatus[], CIActivityStatus>>();
            var buildStatusResolverMock = new Mock<IResolver<CIBuildStatus[], CIBuildStatus>>();
            var hueAlertResolverMock = new Mock<IResolver<CIStatusChangeQuery, HueAlert>>();
            var hueColorResolverMock = new Mock<IResolver<CIStatus, HueColor>>();
            var hueInvokerMock = new Mock<IHueInvoker>();
            var serializerMock = new Mock<ISerializer>();
            var scheduleApplicabilityResolverMock = new Mock<IResolver<ScheduleQuery, bool>>();
            var scheduleResolverMock = new Mock<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>();
            var activityStatusAggregatorMock = new Mock<IActivityStatusAggregator<Task<CIActivityStatus>>>();
            var buildStatusAggregatorMock = new Mock<IBuildStatusAggregator<Task<CIBuildStatus>>>();

            activityStatusResolverMock.Setup(m => m.Resolve(It.IsAny<CIActivityStatus[]>())).Returns(new CIActivityStatus()).Verifiable();
            buildStatusResolverMock.Setup(m => m.Resolve(It.IsAny<CIBuildStatus[]>())).Returns(new CIBuildStatus()).Verifiable();
            hueAlertResolverMock.Setup(m => m.Resolve(It.IsAny<CIStatusChangeQuery>())).Returns(new HueAlert()).Verifiable();
            hueColorResolverMock.Setup(m => m.Resolve(It.IsAny<CIStatus>())).Returns(new HueColor()).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HueColor>())).ReturnsAsync(new ExpandoObject()).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HueAlert>())).ReturnsAsync(new ExpandoObject()).Verifiable();
            serializerMock.Setup(m => m.Deserialize<CIStatus>()).Returns(new CIStatus()).Verifiable();
            serializerMock.Setup(m => m.Serialize(It.IsAny<CIStatus>())).Verifiable();
            scheduleApplicabilityResolverMock.Setup(m => m.Resolve(It.IsAny<ScheduleQuery>())).Returns(true).Verifiable();
            scheduleResolverMock.Setup(m => m.Resolve(It.IsAny<DateTime>())).Returns(("", new TimeRangeSettings())).Verifiable();
            activityStatusAggregatorMock.Setup(m => m.GetActivityStatus()).ReturnsAsync(new CIActivityStatus()).Verifiable();
            buildStatusAggregatorMock.Setup(m => m.GetBuildStatus()).ReturnsAsync(new CIBuildStatus()).Verifiable();

            var sut = new HueUpdaterService(
                appLifetimeMock,
                loggerMock,
                activityStatusResolverMock.Object,
                buildStatusResolverMock.Object,
                hueAlertResolverMock.Object,
                hueColorResolverMock.Object,
                hueInvokerMock.Object,
                serializerMock.Object,
                scheduleApplicabilityResolverMock.Object,
                scheduleResolverMock.Object,
                new[] { activityStatusAggregatorMock.Object },
                new[] { buildStatusAggregatorMock.Object }
            );

            await sut.UpdateHueEndpointAsync();

            activityStatusResolverMock.Verify(m => m.Resolve(It.IsAny<CIActivityStatus[]>()), Times.Once);
            buildStatusResolverMock.Verify(m => m.Resolve(It.IsAny<CIBuildStatus[]>()), Times.Once);
            hueAlertResolverMock.Verify(m => m.Resolve(It.IsAny<CIStatusChangeQuery>()), Times.Once);
            hueColorResolverMock.Verify(m => m.Resolve(It.IsAny<CIStatus>()), Times.Once);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HueColor>()), Times.Once);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HueAlert>()), Times.Once);
            serializerMock.Verify(m => m.Deserialize<CIStatus>(), Times.Once);
            serializerMock.Verify(m => m.Serialize(It.IsAny<CIStatus>()), Times.Once);
            scheduleApplicabilityResolverMock.Verify(m => m.Resolve(It.IsAny<ScheduleQuery>()), Times.Once);
            scheduleResolverMock.Verify(m => m.Resolve(It.IsAny<DateTime>()), Times.Once);
            activityStatusAggregatorMock.Verify(m => m.GetActivityStatus(), Times.Once);
            buildStatusAggregatorMock.Verify(m => m.GetBuildStatus(), Times.Once);
        }

    }

}
