using System;
using System.Collections.Generic;
using FluentAssertions;
using HueUpdater.Abstractions;
using HueUpdater.Settings;
using Moq;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleResolverUnitTests
    {

        [Fact]
        public void Constructor_NullScheduleNameCandidateResolvers_Throws()
        {
            Action action = () => new ScheduleResolver(null, Mock.Of<IResolver<string[], string>>(), new Dictionary<string, ScheduleSettings>());
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullScheduleNameResolver_Throws()
        {
            Action action = () => new ScheduleResolver(Mock.Of<IEnumerable<IResolver<DateTime, string>>>(), null, new Dictionary<string, ScheduleSettings>());
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullSchedules_Throws()
        {
            Action action = () => new ScheduleResolver(Mock.Of<IEnumerable<IResolver<DateTime, string>>>(), Mock.Of<IResolver<string[], string>>(), null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_CallsExpected()
        {
            var sampleScheduleName = "sample_schedule";
            var sampleSchedule = new Dictionary<string, ScheduleSettings>() { { sampleScheduleName, new ScheduleSettings() } };

            var scheduleNameCandidateResolverMock1 = new Mock<IResolver<DateTime, string>>();
            scheduleNameCandidateResolverMock1.Setup(m => m.Resolve(It.IsAny<DateTime>()))
                .Verifiable();

            var scheduleNameCandidateResolverMock2 = new Mock<IResolver<DateTime, string>>();
            scheduleNameCandidateResolverMock2.Setup(m => m.Resolve(It.IsAny<DateTime>()))
                .Verifiable();

            var scheduleNameCandidateResolvers = new[]
            {
                scheduleNameCandidateResolverMock1.Object,
                scheduleNameCandidateResolverMock2.Object
            };

            var scheduleNameResolverMock = new Mock<IResolver<string[], string>>();
            scheduleNameResolverMock.Setup(m => m.Resolve(It.IsAny<string[]>()))
                .Returns(sampleScheduleName)
                .Verifiable();

            var sut = new ScheduleResolver(
                scheduleNameCandidateResolvers,
                scheduleNameResolverMock.Object,
                sampleSchedule
            );

            var (ScheduleName, Schedule) = sut.Resolve(DateTime.Today);

            scheduleNameCandidateResolverMock1.Verify(m => m.Resolve(It.IsAny<DateTime>()), Times.Once);
            scheduleNameCandidateResolverMock2.Verify(m => m.Resolve(It.IsAny<DateTime>()), Times.Once);
            scheduleNameResolverMock.Verify(m => m.Resolve(It.IsAny<string[]>()), Times.Once);
            ScheduleName.Should().Be(sampleScheduleName);
            Schedule.Should().NotBeNull();
        }

    }

}
