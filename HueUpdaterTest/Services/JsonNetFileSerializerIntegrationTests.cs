using System.IO;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Integration")]
    public class JsonNetFileSerializerIntegrationTests
    {

        [Fact]
        public void Deserialize_SerializationExample_IsExpected()
        {
            var filePath = "SerializationExample.input.json";
            var sut = new JsonNetFileSerializer(filePath);
            var expected = new SerializationExample();
            var result = sut.Deserialize<SerializationExample>();
            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void Serialize_SerializationExample_CreatesExpectedFile()
        {
            var filePath = "SerializationExample.output.json";
            File.Delete(filePath);
            File.Exists(filePath).Should().BeFalse();
            var sut = new JsonNetFileSerializer(filePath);
            sut.Serialize(new SerializationExample());
            File.Exists(filePath).Should().BeTrue();
        }


        private class SerializationExample
        {
            public string First { get; set; } = "first";
            public bool Second { get; set; } = true;
        }

    }

}
