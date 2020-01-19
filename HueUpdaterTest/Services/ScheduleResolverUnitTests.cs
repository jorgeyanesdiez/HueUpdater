using System;
using System.Collections.Generic;
using FluentAssertions;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleResolverUnitTests
    {

        private CalendarSettings InvalidCalendar { get; }
        private CalendarSettings Calendar { get; }


        public ScheduleResolverUnitTests()
        {
            InvalidCalendar = CreateInvalidCalendar();
            Calendar = CreateCalendar();
        }


        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleResolver(InvalidCalendar);
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveDayOverridenSchedule_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleResolver(InvalidCalendar);
            var result = sut.ResolveDayOverridenSchedule(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveDefaultSchedule_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleResolver(InvalidCalendar);
            var result = sut.ResolveDefaultSchedule(DateTime.Today);
            result.Should().BeNull();
        }


        [Theory]
        [InlineData("2020-01-01", "FirstDay")]
        [InlineData("2020-01-06", "SecondWeek")]
        [InlineData("2020-01-07", "SecondWeek")]
        [InlineData("2020-01-08", "SecondWeek")]
        [InlineData("2020-01-09", "SecondWeek")]
        [InlineData("2020-01-10", "SecondWeek")]
        [InlineData("2020-01-11", "Weekends")]
        [InlineData("2020-01-12", "Weekends")]
        [InlineData("2020-02-15", "AllFebruary")]
        [InlineData("2020-02-16", "AllFebruary")]
        [InlineData("2020-03-01", "Weekends")]
        public void Resolve_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleResolver(Calendar);
            var result = sut.Resolve(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("2020-01-01", null)]
        [InlineData("2020-01-06", null)]
        [InlineData("2020-01-07", null)]
        [InlineData("2020-01-08", null)]
        [InlineData("2020-01-09", null)]
        [InlineData("2020-01-10", null)]
        [InlineData("2020-01-11", "Weekends")]
        [InlineData("2020-01-12", "Weekends")]
        [InlineData("2020-02-15", "Weekends")]
        [InlineData("2020-02-16", "Weekends")]
        [InlineData("2020-03-01", "Weekends")]
        public void ResolveDayOverridenSchedule_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleResolver(Calendar);
            var result = sut.ResolveDayOverridenSchedule(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("2020-01-01", "FirstDay")]
        [InlineData("2020-01-06", "SecondWeek")]
        [InlineData("2020-01-07", "SecondWeek")]
        [InlineData("2020-01-08", "SecondWeek")]
        [InlineData("2020-01-09", "SecondWeek")]
        [InlineData("2020-01-10", "SecondWeek")]
        [InlineData("2020-01-11", "SecondWeek")]
        [InlineData("2020-01-12", "SecondWeek")]
        [InlineData("2020-02-15", "AllFebruary")]
        [InlineData("2020-02-16", "AllFebruary")]
        [InlineData("2020-03-01", null)]
        public void ResolveDefaultSchedule_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleResolver(Calendar);
            var result = sut.ResolveDefaultSchedule(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        private CalendarSettings CreateInvalidCalendar()
        {
            var invalidCalendar = new CalendarSettings()
            {
                Defaults = new Dictionary<string, DateRangeSettings[]>
                {
                    { "", new[] { new DateRangeSettings { Start = "", Finish = "" } } }
                },
                DayOverrides = new Dictionary<string, string[]>
                {
                    { "", new[] { "" } }
                }
            };

            return invalidCalendar;
        }


        private CalendarSettings CreateCalendar()
        {
            var calendar = new CalendarSettings()
            {
                Defaults = new Dictionary<string, DateRangeSettings[]>
                {
                    { "FirstDay", new[] { new DateRangeSettings { Start = "2020-01-01", Finish = "2020-01-01" } } },
                    { "SecondWeek", new[] { new DateRangeSettings { Start = "2020-01-06", Finish = "2020-01-12" } } },
                    { "AllFebruary", new[] { new DateRangeSettings { Start = "2020-02-01", Finish = "2020-02-29" } } }
                },
                DayOverrides = new Dictionary<string, string[]>
                {
                    { "Weekends", new[] { "Saturday", "Sunday" } }
                },
                DayOverridesExclusions = new List<string>() { "AllFebruary" }
            };
            return calendar;
        }

    }

}
