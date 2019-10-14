using System;
using System.IO;
using HueUpdater.Abstractions;
using Newtonsoft.Json;

namespace HueUpdater.Services
{

    /// <summary>
    /// File serializer using Json.Net
    /// </summary>
    public class JsonNetFileSerializer
        : ISerializer
    {

        /// <summary>
        /// The path for the serialization operations of this instance.
        /// </summary>
        private string FilePath { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="filePath">The value for the <see cref="FilePath"/> property.</param>
        public JsonNetFileSerializer(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentOutOfRangeException(nameof(filePath)); }
        }


        /// <inheritdoc/>
        public TOutput Deserialize<TOutput>() where TOutput : new()
        {
            TOutput result;
            try
            {
                using (var reader = new StreamReader(FilePath))
                {
                    var contents = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<TOutput>(contents);
                }
            }
            catch(Exception e) when (e is FileNotFoundException || e is JsonReaderException)
            {
                result = default;
            }
            return result;
        }


        /// <inheritdoc/>
        public void Serialize<TInput>(TInput input) where TInput : new()
        {
            var contents = JsonConvert.SerializeObject(input);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(contents);
            }
        }

    }

}
