using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
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
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullLogger_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                null,
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullCIStatusUrls_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                null,
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_EmptyCIStatusUrls_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string>(),
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void Constructor_NullCIStatusReducer_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                null,
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullHueAlertResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                null,
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullHueUpdaterItems_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                null,
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_EmptyHueUpdaterItems_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                Enumerable.Empty<HueUpdaterItem>(),
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void Constructor_NullLightColorResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                null,
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullSerializer_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                null,
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullScheduleApplicabilityResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                null,
                Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>()
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullScheduleResolver_Throws()
        {
            Action action = () => new HueUpdaterService(
                Mock.Of<IHostApplicationLifetime>(),
                Mock.Of<ILogger<HueUpdaterService>>(),
                new List<string> { "" },
                Mock.Of<IResolver<CIStatus[], CIStatus>>(),
                Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>(),
                new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) },
                Mock.Of<IResolver<CIStatus, LightColor>>(),
                Mock.Of<ISerializer<LightStatus>>(),
                Mock.Of<IResolver<ScheduleQuery, bool>>(),
                null
            );
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public async Task StartAsync_CallsExpected()
        {
            var appLifetimeMock = new Mock<IHostApplicationLifetime>();
            var loggerMock = Mock.Of<ILogger<HueUpdaterService>>();
            var ciStatusUrls = new List<string>() { "http://huebridge.example/" };
            var ciStatusReducerMock = Mock.Of<IResolver<CIStatus[], CIStatus>>();
            var hueAlertResolverMock = Mock.Of<IResolver<LightStatusChangeQuery, HueAlert>>();
            var hueUpdaterItems = new List<HueUpdaterItem>() { new HueUpdaterItem(Mock.Of<IHueInvoker>(), Mock.Of<IResolver<LightColor, HueColor>>()) };
            var lightColorResolverMock = Mock.Of<IResolver<CIStatus, LightColor>>();
            var serializerMock = Mock.Of<ISerializer<LightStatus>>();
            var scheduleApplicabilityResolverMock = Mock.Of<IResolver<ScheduleQuery, bool>>();
            var scheduleResolverMock = Mock.Of<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>();

            appLifetimeMock.Setup(m => m.StopApplication()).Verifiable();

            var sut = new HueUpdaterService(
                appLifetimeMock.Object,
                loggerMock,
                ciStatusUrls,
                ciStatusReducerMock,
                hueAlertResolverMock,
                hueUpdaterItems,
                lightColorResolverMock,
                serializerMock,
                scheduleApplicabilityResolverMock,
                scheduleResolverMock
            );

            Func<Task> action = async () => await sut.StartAsync(new CancellationToken());
            await action.Should().ThrowAsync<Exception>();
            appLifetimeMock.Verify(m => m.StopApplication(), Times.Once);
        }


        [Fact]
        public async Task ProcessItemsAsync_ScheduleApplicable_CallsExpected()
        {
            var appLifetimeMock = Mock.Of<IHostApplicationLifetime>();
            var loggerMock = Mock.Of<ILogger<HueUpdaterService>>();
            var ciStatusUrls = new List<string>() { "http://huebridge.example/" };
            var ciStatusReducerMock = new Mock<IResolver<CIStatus[], CIStatus>>();
            var hueAlertResolverMock = new Mock<IResolver<LightStatusChangeQuery, HueAlert>>();
            var hueInvokerMock = new Mock<IHueInvoker>();
            var hueColorFactoryMock = new Mock<IResolver<LightColor, HueColor>>();
            var hueUpdaterItems = new List<HueUpdaterItem>() { new HueUpdaterItem(hueInvokerMock.Object, hueColorFactoryMock.Object) };
            var lightColorResolverMock = new Mock<IResolver<CIStatus, LightColor>>();
            var serializerMock = new Mock<ISerializer<LightStatus>>();
            var scheduleApplicabilityResolverMock = new Mock<IResolver<ScheduleQuery, bool>>();
            var scheduleResolverMock = new Mock<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>();

            var sut = new HueUpdaterService(
                appLifetimeMock,
                loggerMock,
                ciStatusUrls,
                ciStatusReducerMock.Object,
                hueAlertResolverMock.Object,
                hueUpdaterItems,
                lightColorResolverMock.Object,
                serializerMock.Object,
                scheduleApplicabilityResolverMock.Object,
                scheduleResolverMock.Object
            );

            ciStatusReducerMock.Setup(m => m.Resolve(It.IsAny<CIStatus[]>())).Returns(new CIStatus()).Verifiable();
            hueAlertResolverMock.Setup(m => m.Resolve(It.IsAny<LightStatusChangeQuery>())).Returns(new HueAlert()).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HueColor>())).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HuePower>())).Verifiable();
            hueColorFactoryMock.Setup(m => m.Resolve(It.IsAny<LightColor>())).Verifiable();
            lightColorResolverMock.Setup(m => m.Resolve(It.IsAny<CIStatus>())).Returns(new LightColor()).Verifiable();
            serializerMock.Setup(m => m.Deserialize()).Returns(new LightStatus()).Verifiable();
            serializerMock.Setup(m => m.Serialize(It.IsAny<LightStatus>())).Verifiable();
            scheduleApplicabilityResolverMock.Setup(m => m.Resolve(It.IsAny<ScheduleQuery>())).Returns(true).Verifiable();
            scheduleResolverMock.Setup(m => m.Resolve(It.IsAny<DateTime>())).Returns(("", new ScheduleSettings())).Verifiable();

            using var httpTest = new HttpTest(); // Causes Flurl calls to be faked.
            await sut.ProcessItemsAsync();

            ciStatusReducerMock.Verify(m => m.Resolve(It.IsAny<CIStatus[]>()), Times.Once);
            hueAlertResolverMock.Verify(m => m.Resolve(It.IsAny<LightStatusChangeQuery>()), Times.Once);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HueColor>()), Times.Once);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HuePower>()), Times.Never);
            hueColorFactoryMock.Verify(m => m.Resolve(It.IsAny<LightColor>()), Times.Once);
            lightColorResolverMock.Verify(m => m.Resolve(It.IsAny<CIStatus>()), Times.Once);
            serializerMock.Verify(m => m.Deserialize(), Times.Once);
            serializerMock.Verify(m => m.Serialize(It.IsAny<LightStatus>()), Times.Once);
            scheduleApplicabilityResolverMock.Verify(m => m.Resolve(It.IsAny<ScheduleQuery>()), Times.Once);
            scheduleResolverMock.Verify(m => m.Resolve(It.IsAny<DateTime>()), Times.Once);
        }


        [Fact]
        public async Task ProcessItemsAsync_ScheduleNotApplicable_CallsExpected()
        {
            var appLifetimeMock = Mock.Of<IHostApplicationLifetime>();
            var loggerMock = Mock.Of<ILogger<HueUpdaterService>>();
            var ciStatusUrls = new List<string>() { "http://huebridge.example/" };
            var ciStatusReducerMock = new Mock<IResolver<CIStatus[], CIStatus>>();
            var hueAlertResolverMock = new Mock<IResolver<LightStatusChangeQuery, HueAlert>>();
            var hueInvokerMock = new Mock<IHueInvoker>();
            var hueColorFactoryMock = new Mock<IResolver<LightColor, HueColor>>();
            var hueUpdaterItems = new List<HueUpdaterItem>() { new HueUpdaterItem(hueInvokerMock.Object, hueColorFactoryMock.Object) };
            var lightColorResolverMock = new Mock<IResolver<CIStatus, LightColor>>();
            var serializerMock = new Mock<ISerializer<LightStatus>>();
            var scheduleApplicabilityResolverMock = new Mock<IResolver<ScheduleQuery, bool>>();
            var scheduleResolverMock = new Mock<IResolver<DateTime, (string Name, ScheduleSettings Schedule)>>();

            var sut = new HueUpdaterService(
                appLifetimeMock,
                loggerMock,
                ciStatusUrls,
                ciStatusReducerMock.Object,
                hueAlertResolverMock.Object,
                hueUpdaterItems,
                lightColorResolverMock.Object,
                serializerMock.Object,
                scheduleApplicabilityResolverMock.Object,
                scheduleResolverMock.Object
            );

            ciStatusReducerMock.Setup(m => m.Resolve(It.IsAny<CIStatus[]>())).Returns(new CIStatus()).Verifiable();
            hueAlertResolverMock.Setup(m => m.Resolve(It.IsAny<LightStatusChangeQuery>())).Returns(new HueAlert()).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HueColor>())).Verifiable();
            hueInvokerMock.Setup(m => m.PutAsync(It.IsAny<HuePower>())).Verifiable();
            hueColorFactoryMock.Setup(m => m.Resolve(It.IsAny<LightColor>())).Verifiable();
            lightColorResolverMock.Setup(m => m.Resolve(It.IsAny<CIStatus>())).Returns(new LightColor()).Verifiable();
            serializerMock.Setup(m => m.Deserialize()).Returns(new LightStatus()).Verifiable();
            serializerMock.Setup(m => m.Serialize(It.IsAny<LightStatus>())).Verifiable();
            scheduleApplicabilityResolverMock.Setup(m => m.Resolve(It.IsAny<ScheduleQuery>())).Returns(false).Verifiable();
            scheduleResolverMock.Setup(m => m.Resolve(It.IsAny<DateTime>())).Returns(("", new ScheduleSettings())).Verifiable();

            await sut.ProcessItemsAsync();

            ciStatusReducerMock.Verify(m => m.Resolve(It.IsAny<CIStatus[]>()), Times.Never);
            hueAlertResolverMock.Verify(m => m.Resolve(It.IsAny<LightStatusChangeQuery>()), Times.Once);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HueColor>()), Times.Never);
            hueInvokerMock.Verify(m => m.PutAsync(It.IsAny<HuePower>()), Times.Once);
            hueColorFactoryMock.Verify(m => m.Resolve(It.IsAny<LightColor>()), Times.Never);
            lightColorResolverMock.Verify(m => m.Resolve(It.IsAny<CIStatus>()), Times.Never);
            serializerMock.Verify(m => m.Deserialize(), Times.Once);
            serializerMock.Verify(m => m.Serialize(It.IsAny<LightStatus>()), Times.Once);
            scheduleApplicabilityResolverMock.Verify(m => m.Resolve(It.IsAny<ScheduleQuery>()), Times.Once);
            scheduleResolverMock.Verify(m => m.Resolve(It.IsAny<DateTime>()), Times.Once);
        }

    }

}
