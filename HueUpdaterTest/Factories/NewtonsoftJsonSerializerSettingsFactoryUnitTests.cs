﻿using FluentAssertions;
using Xunit;

namespace HueUpdater.Factories
{

    [Trait("TestType", "Unit")]
    public class NewtonsoftJsonSerializerSettingsFactoryUnitTests
    {

        [Fact]
        public void Build_ByDefault_IsNotNull()
        {
            var settings = NewtonsoftJsonSerializerSettingsFactory.Build();
            settings.Should().NotBeNull();
        }

    }

}
