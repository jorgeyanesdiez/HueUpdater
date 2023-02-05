using System;
using System.Collections.Generic;
using FluentAssertions;
using HueUpdater.Models;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleApplicabilityResolverUnitTests
    {

        [Fact]
        public void Resolve_Null_Throws()
        {
            var sut = new ScheduleApplicabilityResolver();
            Action action = () => sut.Resolve(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        private static readonly Dictionary<string, ScheduleSettings> Schedules = new()
        {
            { "Empty", new ScheduleSettings { Priority = 1, Hours = new TimeRangeSettings { Start = "00:00", Finish = "00:00" } } } ,
            { "Invalid", new ScheduleSettings { Priority = 2, Hours = new TimeRangeSettings { Start = "00:01", Finish = "00:00" } } },
            { "Valid", new ScheduleSettings { Priority = 3, Hours = new TimeRangeSettings { Start = "00:00", Finish = "00:01" } } }
        };


        [Theory]
        [InlineData("00:00", "Empty", false)]
        [InlineData("00:01", "Empty", false)]
        [InlineData("00:00", "Invalid", false)]
        [InlineData("00:01", "Invalid", false)]
        [InlineData("00:00", "Valid", true)]
        [InlineData("00:01", "Valid", false)]
        public void Resolve_IsExpected(string time, string scheduleName, bool isApplicable)
        {
            var sut = new ScheduleApplicabilityResolver();
            var result = sut.Resolve(new ScheduleQuery { Time = TimeSpan.Parse(time), Schedule = Schedules[scheduleName] });
            result.Should().Be(isApplicable);
        }

    }

}
