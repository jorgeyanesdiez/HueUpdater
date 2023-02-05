using System;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Settings
{

    [Trait("TestType", "Unit")]
    public class TimeRangeSettingsUnitTests
    {

        [Fact]
        public void Range_ByDefault_IsEmpty()
        {
            var sut = new TimeRangeSettings();
            var difference = sut.FinishTime - sut.StartTime;
            difference.TotalMilliseconds.Should().Be(0);
        }


        [Fact]
        public void Range_KnownSize_IsExpected()
        {
            var time = DateTime.Now;
            var finish = time;
            var start = time.AddHours(-1);

            var timeFmt = "HH:mm:ss";
            var sut = new TimeRangeSettings
            {
                Start = start.ToString(timeFmt),
                Finish = finish.ToString(timeFmt)
            };

            var difference = sut.FinishTime - sut.StartTime;
            difference.TotalMinutes.Should().Be(60);
        }

    }

}
