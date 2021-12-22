using System;
using FluentAssertions;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleNameResolverUnitTests
    {

        private CalendarSettings MalformedCalendar { get; }
        private CalendarSettings InvalidCalendar { get; }
        private CalendarSettings Calendar { get; }


        public ScheduleNameResolverUnitTests()
        {
            MalformedCalendar = CreateMalformedCalendar();
            InvalidCalendar = CreateInvalidCalendar();
            Calendar = CreateCalendar();
        }


        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleNameResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_MalformedCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(MalformedCalendar);
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveOverriden_MalformedCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(MalformedCalendar);
            var result = sut.ResolveOverriden(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveDefault_MalformedCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(MalformedCalendar);
            var result = sut.ResolveDefault(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void Resolve_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(InvalidCalendar);
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveOverriden_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(InvalidCalendar);
            var result = sut.ResolveOverriden(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void ResolveDefault_InvalidCalendar_IsExpected()
        {
            var sut = new ScheduleNameResolver(InvalidCalendar);
            var result = sut.ResolveDefault(DateTime.Today);
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
            var sut = new ScheduleNameResolver(Calendar);
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
        public void ResolveOverriden_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleNameResolver(Calendar);
            var result = sut.ResolveOverriden(DateTime.Parse(date));
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
        public void ResolveDefault_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleNameResolver(Calendar);
            var result = sut.ResolveDefault(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        private static CalendarSettings CreateMalformedCalendar()
        {
            var malformedCalendar = new CalendarSettings()
            {
                Defaults = null,
                DayOverrides = null,
                DayOverridesExclusions = null
            };

            return malformedCalendar;
        }


        private static CalendarSettings CreateInvalidCalendar()
        {
            var invalidCalendar = new CalendarSettings()
            {
                Defaults = new CalendarDefaultSettings
                {
                    { "", new[] { new DateRangeSettings { Start = "", Finish = "" } } }
                },
                DayOverrides = new CalendarDayOverrideSettings
                {
                    { "", new[] { "" } }
                },
                DayOverridesExclusions = new CalendarDayOverrideExclusionSettings { null }
            };

            return invalidCalendar;
        }


        private static CalendarSettings CreateCalendar()
        {
            var calendar = new CalendarSettings()
            {
                Defaults = new CalendarDefaultSettings
                {
                    { "FirstDay", new[] { new DateRangeSettings { Start = "2020-01-01", Finish = "2020-01-01" } } },
                    { "SecondWeek", new[] { new DateRangeSettings { Start = "2020-01-06", Finish = "2020-01-12" } } },
                    { "AllFebruary", new[] { new DateRangeSettings { Start = "2020-02-01", Finish = "2020-02-29" } } }
                },
                DayOverrides = new CalendarDayOverrideSettings
                {
                    { "Weekends", new[] { "Saturday", "Sunday" } }
                },
                DayOverridesExclusions = new CalendarDayOverrideExclusionSettings { "AllFebruary" }
            };
            return calendar;
        }

    }

}
