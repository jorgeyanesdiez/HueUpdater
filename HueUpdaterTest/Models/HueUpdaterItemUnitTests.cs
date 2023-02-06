using System;
using FluentAssertions;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using Moq;
using Xunit;

namespace HueUpdater.Models
{

    [Trait("TestType", "Unit")]
    public class HueUpdaterItemUnitTests
    {

        [Fact]
        public void Constructor_NullInvoker_Throws()
        {
            Action action = () => new HueUpdaterItem(null, Mock.Of<IResolver<LightColor, HueColor>>());
            action.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void Constructor_NullFactory_Throws()
        {
            Action action = () => new HueUpdaterItem(Mock.Of<IHueInvoker>(), null);
            action.Should().Throw<ArgumentNullException>();
        }

    }

}
