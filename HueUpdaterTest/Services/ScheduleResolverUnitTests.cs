using System;
using FluentAssertions;
using HueUpdater.Abstractions;
using HueUpdater.Settings;
using Moq;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class ScheduleResolverUnitTests
    {

        [Fact]
        public void Constructor_NullService_Throws()
        {
            Action action = () => new ScheduleResolver(null, new ScheduleSettings());
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullSchedules_Throws()
        {
            Action action = () => new ScheduleResolver(Mock.Of<IResolver<DateTime, string>>(), null);
            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_EmptySchedules_Throws()
        {
            Action action = () => new ScheduleResolver(Mock.Of<IResolver<DateTime, string>>(), new ScheduleSettings());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }


        [Theory]
        [InlineData(null)]
        [InlineData("Explicit")]
        public void Resolve_Schedules_IsExpected(string mockResult)
        {
            var mock = new Mock<IResolver<DateTime, string>>();
            mock.Setup(m => m.Resolve(It.IsAny<DateTime>()))
                .Returns(mockResult)
                .Verifiable();

            var schedules = new ScheduleSettings
            {
                { "Default", new TimeRangeSettings() },
                { "Explicit", new TimeRangeSettings() }
            };
            var sut = new ScheduleResolver(mock.Object, schedules);
            var actual = sut.Resolve(DateTime.Today);

            mock.Verify();
            if (mockResult == null) { actual.Name.Should().NotBeNull().And.NotBe(mockResult); }
            else { actual.Name.Should().Be(mockResult); }
        }

    }

}
