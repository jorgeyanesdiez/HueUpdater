using System.Collections.Generic;
using FluentAssertions;
using HueUpdater.Factories;
using HueUpdater.Models;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Unit")]
    public class HueAlertResolverUnitTests
    {

        private static readonly LightStatus off = new() { Color = null, Power = LightPower.Off };
        private static readonly LightStatus idleStable = new() { Color = LightColor.Green, Power = LightPower.On };
        private static readonly LightStatus idleBroken = new() { Color = LightColor.Red, Power = LightPower.On };
        private static readonly LightStatus buildingStable = new() { Color = LightColor.Blue, Power = LightPower.On };
        private static readonly LightStatus buildingBroken = new() { Color = LightColor.Yellow, Power = LightPower.On };


        public static IEnumerable<object[]> Resolve_Same_IsExpected_Data => new object[][]
        {
            // null and null are considered different, so not part of this dataset.
            new object[] { off },
            new object[] { idleStable },
            new object[] { idleBroken },
            new object[] { buildingStable },
            new object[] { buildingBroken }
        };


        [Theory]
        [MemberData(nameof(Resolve_Same_IsExpected_Data))]
        public void Resolve_Same_IsExpected(LightStatus status)
        {
            var sut = new HueAlertResolver();
            var result = sut.Resolve(new LightStatusChangeQuery { Current = status, Previous = status });
            result.Should().BeNull();
        }


        public static IEnumerable<object[]> Resolve_Different_IsExpected_Data => new object[][]
        {
            new object[] { null, null },
            new object[] { null, off },
            new object[] { null, idleStable },
            new object[] { null, idleBroken },
            new object[] { null, buildingStable },
            new object[] { null, buildingBroken },
            new object[] { off, null },
            new object[] { off, idleStable },
            new object[] { off, idleBroken },
            new object[] { off, buildingStable },
            new object[] { off, buildingBroken },
            new object[] { idleStable, null },
            new object[] { idleStable, off },
            new object[] { idleStable, idleBroken },
            new object[] { idleStable, buildingStable },
            new object[] { idleStable, buildingBroken },
            new object[] { idleBroken, null },
            new object[] { idleBroken, off },
            new object[] { idleBroken, idleStable },
            new object[] { idleBroken, buildingStable },
            new object[] { idleBroken, buildingBroken },
            new object[] { buildingStable, null },
            new object[] { buildingStable, off },
            new object[] { buildingStable, idleStable },
            new object[] { buildingStable, idleBroken },
            new object[] { buildingStable, buildingBroken },
            new object[] { buildingBroken, null },
            new object[] { buildingBroken, off },
            new object[] { buildingBroken, idleStable },
            new object[] { buildingBroken, idleBroken },
            new object[] { buildingBroken, buildingStable }
        };


        [Theory]
        [MemberData(nameof(Resolve_Different_IsExpected_Data))]
        public void Resolve_Different_IsExpected(LightStatus current, LightStatus previous)
        {
            var sut = new HueAlertResolver();
            var result = sut.Resolve(new LightStatusChangeQuery { Current = current, Previous = previous });
            result.Alert.Should().Be(HueAlertFactory.Blink);
        }

    }

}
