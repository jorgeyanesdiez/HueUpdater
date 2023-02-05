using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HueUpdater.Settings;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleNameByCalendarResolverUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleNameByCalendarResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_EmptyCalendar_IsExpected()
        {
            var calendar = new Dictionary<string, DateRangeSettings[]>
            {{
                nameof(Resolve_EmptyCalendar_IsExpected),
                Array.Empty<DateRangeSettings>()
            }};

            var sut = new ScheduleNameByCalendarResolver(calendar);
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        private static readonly Dictionary<string, DateRangeSettings[]> Calendar = new()
        {
            { "FirstDay", new[] { new DateRangeSettings { Start = "2020-01-01", Finish = "2020-01-01" } } },
            { "SecondWeek",  new[] { new DateRangeSettings { Start = "2020-01-06", Finish = "2020-01-12" } } },
            { "AllFebruary", new[] { new DateRangeSettings { Start = "2020-02-01", Finish = "2020-02-29" } } }
        };


        [Theory]
        [InlineData("2020-01-01", "FirstDay")]
        [InlineData("2020-01-06", "SecondWeek")]
        [InlineData("2020-01-07", "SecondWeek")]
        [InlineData("2020-01-12", "SecondWeek")]
        [InlineData("2020-02-16", "AllFebruary")]
        [InlineData("2111-11-11", null)]
        public void Resolve_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleNameByCalendarResolver(Calendar);
            var result = sut.Resolve(DateTime.Parse(date));
            result.Should().Be(expected);
        }

    }

}
