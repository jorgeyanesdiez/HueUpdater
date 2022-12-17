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

        private static readonly CIStatus idleStable = new() { ActivityStatus = CIActivityStatus.Idle, BuildStatus = CIBuildStatus.Stable };
        private static readonly CIStatus idleBroken = new() { ActivityStatus = CIActivityStatus.Idle, BuildStatus = CIBuildStatus.Broken };
        private static readonly CIStatus buildingStable = new() { ActivityStatus = CIActivityStatus.Building, BuildStatus = CIBuildStatus.Stable };
        private static readonly CIStatus buildingBroken = new() { ActivityStatus = CIActivityStatus.Building, BuildStatus = CIBuildStatus.Broken };


        public static IEnumerable<object[]> Resolve_Same_IsExpected_Data => new object[][]
        {
            // null and null are assumed different, so not part of this set.
            new object[] { idleStable },
            new object[] { idleBroken },
            new object[] { buildingStable },
            new object[] { buildingBroken }
        };


        [Theory]
        [MemberData(nameof(Resolve_Same_IsExpected_Data))]
        public void Resolve_Same_IsExpected(CIStatus status)
        {
            var sut = new HueAlertResolver();
            var result = sut.Resolve(new CIStatusChangeQuery { Current = status, Previous = status });
            result.Should().BeNull();
        }


        public static IEnumerable<object[]> Resolve_Different_IsExpected_Data => new object[][]
        {
            new object[] { null, null },
            new object[] { null, idleStable },
            new object[] { null, idleBroken },
            new object[] { null, buildingStable },
            new object[] { null, buildingBroken },
            new object[] { idleStable, null },
            new object[] { idleStable, idleBroken },
            new object[] { idleStable, buildingStable },
            new object[] { idleStable, buildingBroken },
            new object[] { idleBroken, null },
            new object[] { idleBroken, idleStable },
            new object[] { idleBroken, buildingStable },
            new object[] { idleBroken, buildingBroken },
            new object[] { buildingStable, null },
            new object[] { buildingStable, idleStable },
            new object[] { buildingStable, idleBroken },
            new object[] { buildingStable, buildingBroken },
            new object[] { buildingBroken, null },
            new object[] { buildingBroken, idleStable },
            new object[] { buildingBroken, idleBroken },
            new object[] { buildingBroken, buildingStable }
        };


        [Theory]
        [MemberData(nameof(Resolve_Different_IsExpected_Data))]
        public void Resolve_Different_IsExpected(CIStatus current, CIStatus previous)
        {
            var sut = new HueAlertResolver();
            var result = sut.Resolve(new CIStatusChangeQuery { Current = current, Previous = previous });
            result.Alert.Should().Be(HueAlertFactory.Blink);
        }

    }

}
