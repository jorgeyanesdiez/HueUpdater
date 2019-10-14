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
        [InlineData("2019-01-01", "First")]
        [InlineData("2019-01-02", "Second")]
        [InlineData("2019-01-03", "Third")]
        public void Resolve_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleResolver(Calendar);
            var result = sut.Resolve(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("2019-01-01", null)]
        [InlineData("2019-01-02", null)]
        [InlineData("2019-01-03", "Third")]
        public void ResolveDayOverridenSchedule_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleResolver(Calendar);
            var result = sut.ResolveDayOverridenSchedule(DateTime.Parse(date));
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("2019-01-01", "First")]
        [InlineData("2019-01-02", "Second")]
        [InlineData("2019-01-03", null)]
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
                    { "First", new[] { new DateRangeSettings { Start = "2019-01-01", Finish = "2019-01-02" } } },
                    { "Second", new[] { new DateRangeSettings { Start = "2019-01-02", Finish = "2019-01-03" } } },
                },
                DayOverrides = new Dictionary<string, string[]>
                {
                    { "Third", new[] { "Thursday" } }
                }
            };
            return calendar;
        }

    }

}
