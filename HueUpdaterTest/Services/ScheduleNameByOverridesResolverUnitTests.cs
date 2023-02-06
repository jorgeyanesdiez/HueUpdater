using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleNameByOverridesResolverUnitTests
    {

        [Fact]
        public void Constructor_Null_Throws()
        {
            Action action = () => new ScheduleNameByOverridesResolver(null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Resolve_EmptyOverrides_IsExpected()
        {
            var sut = new ScheduleNameByOverridesResolver(new Dictionary<string, string>());
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        [Fact]
        public void Resolve_InvalidOverrides_IsExpected()
        {
            var sut = new ScheduleNameByOverridesResolver(new Dictionary<string, string> { { "", "" } });
            var result = sut.Resolve(DateTime.Today);
            result.Should().BeNull();
        }


        private static readonly Dictionary<string, string> Overrides = new()
        {
            { "Saturday", "Weekends" },
            { "Sunday", "Weekends" }
        };


        [Theory]
        [InlineData("2020-01-10", null)]
        [InlineData("2020-01-11", "Weekends")]
        [InlineData("2020-01-12", "Weekends")]
        [InlineData("2020-01-13", null)]
        public void Resolve_GivenData_IsExpected(string date, string expected)
        {
            var sut = new ScheduleNameByOverridesResolver(Overrides);
            var result = sut.Resolve(DateTime.Parse(date));
            result.Should().Be(expected);
        }

    }

}
