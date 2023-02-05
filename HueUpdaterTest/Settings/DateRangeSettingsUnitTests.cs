using System;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class DateRangeSettingsUnitTests
    {

        [Fact]
        public void Range_ByDefault_IsEmpty()
        {
            var sut = new DateRangeSettings();
            var difference = sut.FinishDate - sut.StartDate;
            difference.TotalMilliseconds.Should().Be(0);
        }


        [Fact]
        public void Range_KnownSize_IsExpected()
        {
            var date = DateTime.Today;
            var finish = date;
            var start = date.AddDays(-1);

            var dateFmt = "yyyy-MM-dd";
            var sut = new DateRangeSettings
            {
                Start = start.ToString(dateFmt),
                Finish = finish.ToString(dateFmt)
            };

            var difference = sut.FinishDate - sut.StartDate;
            difference.TotalDays.Should().Be(1);
        }

    }

}
