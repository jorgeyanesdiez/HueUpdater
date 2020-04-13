using System;
using FluentAssertions;
using HueUpdater.Models;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleApplicabilityResolverUnitTests
    {

        private ScheduleSettings Schedules { get; set; }


        public ScheduleApplicabilityResolverUnitTests()
        {
            Schedules = CreateSchedules();
        }


        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleApplicabilityResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_Null_Throws()
        {
            var sut = new ScheduleApplicabilityResolver(new ScheduleSettings());
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("anything", "00:00")]
        public void Resolve_EmptySchedules_IsExpected(string scheduleName, string time)
        {
            var sut = new ScheduleApplicabilityResolver(new ScheduleSettings());
            TimeSpan.TryParse(time, out var timeSpan);
            var result = sut.Resolve(new ScheduleQuery { ScheduleName = scheduleName, Time = timeSpan });
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData("Unexpected", "00:00")]
        [InlineData("Empty", "00:00")]
        [InlineData("Empty", "00:01")]
        [InlineData("Invalid", "00:00")]
        [InlineData("Invalid", "00:01")]
        [InlineData("Valid", "00:01")]
        public void Resolve_ValidSchedules_OutOfRange_IsExpected(string scheduleName, string time)
        {
            var sut = new ScheduleApplicabilityResolver(Schedules);
            var result = sut.Resolve(new ScheduleQuery { ScheduleName = scheduleName, Time = TimeSpan.Parse(time) });
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData("Valid", "00:00")]
        public void Resolve_ValidSchedules_InRange_IsExpected(string scheduleName, string time)
        {
            var sut = new ScheduleApplicabilityResolver(Schedules);
            var result = sut.Resolve(new ScheduleQuery { ScheduleName = scheduleName, Time = TimeSpan.Parse(time) });
            result.Should().BeTrue();
        }


        private ScheduleSettings CreateSchedules()
        {
            var schedules = new ScheduleSettings
            {
                { "Empty", new TimeRangeSettings { Start = "00:00", Finish = "00:00" } },
                { "Invalid", new TimeRangeSettings { Start = "00:01", Finish = "00:00" } },
                { "Valid", new TimeRangeSettings { Start = "00:00", Finish = "00:01" } }
            };
            return schedules;
        }

    }

}
