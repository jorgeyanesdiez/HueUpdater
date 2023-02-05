using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleNameResolverUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleNameResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        private static readonly Dictionary<string, ScheduleSettings> Schedules = new()
        {
            { "ScheduleWithPriority1", new ScheduleSettings { Priority = 1, Hours = new TimeRangeSettings { Start = "00:00", Finish = "00:00" } } } ,
            { "ScheduleWithPriority2", new ScheduleSettings { Priority = 2, Hours = new TimeRangeSettings { Start = "00:01", Finish = "00:00" } } },
            { "ScheduleWithPriority3", new ScheduleSettings { Priority = 3, Hours = new TimeRangeSettings { Start = "00:00", Finish = "00:01" } } }
        };


        [Theory]
        [InlineData("ScheduleWithPriority1", "ScheduleWithPriority3", "ScheduleWithPriority1")]
        [InlineData("ScheduleWithPriority2", "ScheduleWithPriority3", "ScheduleWithPriority2")]
        [InlineData("ScheduleWithPriority3", "ScheduleWithPriority3", "ScheduleWithPriority3")]
        [InlineData("ScheduleWithPriority1", "UndefinedSchedule", "ScheduleWithPriority1")]
        [InlineData("UndefinedSchedule", "UndefinedSchedule", "UndefinedSchedule")]
        public void Resolve_ValidSchedules_IsExpected(string expected, params string[] scheduleNames)
        {
            var sut = new ScheduleNameResolver(Schedules);
            var result = sut.Resolve(scheduleNames);
            result.Should().Be(expected);
        }

    }

}
