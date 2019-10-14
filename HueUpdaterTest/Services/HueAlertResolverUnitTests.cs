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

        private static readonly CIStatus idleStable = new CIStatus { ActivityStatus = CIActivityStatus.Idle, BuildStatus = CIBuildStatus.Stable };
        private static readonly CIStatus idleBroken = new CIStatus { ActivityStatus = CIActivityStatus.Idle, BuildStatus = CIBuildStatus.Broken };
        private static readonly CIStatus buildingStable = new CIStatus { ActivityStatus = CIActivityStatus.Building, BuildStatus = CIBuildStatus.Stable };
        private static readonly CIStatus buildingBroken = new CIStatus { ActivityStatus = CIActivityStatus.Building, BuildStatus = CIBuildStatus.Broken };


        public static IEnumerable<object[]> Resolve_Same_IsExpected_Data => new object[][]
        {
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
            result.Alert.Should().Be(HueAlertFactory.None);
        }


        public static IEnumerable<object[]> Resolve_Different_IsExpected_Data => new object[][]
        {
            new object[] { idleStable, idleBroken },
            new object[] { idleStable, buildingStable },
            new object[] { idleStable, buildingBroken },
            new object[] { idleBroken, buildingStable },
            new object[] { idleBroken, buildingBroken },
            new object[] { buildingStable, buildingBroken }
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
