using System.IO;
using FluentAssertions;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Integration")]
    public class JsonNetFileSerializerIntegrationTests
    {

        [Fact]
        public void Deserialize_NonExisting_IsExpected()
        {
            var filePath = "NonExistingFile";
            var sut = new JsonNetFileSerializer<SerializationExample>(filePath);
            var result = sut.Deserialize();
            result.Should().BeNull();
        }


        [Fact]
        public void Deserialize_SerializationExample_IsExpected()
        {
            var filePath = "SerializationExample.input.json";
            var sut = new JsonNetFileSerializer<SerializationExample>(filePath);
            var expected = new SerializationExample();
            var result = sut.Deserialize();
            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void Serialize_SerializationExample_CreatesExpected()
        {
            var filePath = "SerializationExample.output.json";
            File.Delete(filePath);
            File.Exists(filePath).Should().BeFalse();
            var sut = new JsonNetFileSerializer<SerializationExample>(filePath);
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
