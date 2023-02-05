using System;
using System.IO;
using HueUpdater.Abstractions;
using Newtonsoft.Json;

namespace HueUpdater.Services
{

    /// <summary>
    /// File serializer using Json.Net
    /// </summary>
    public class JsonNetFileSerializer<TObject>
        : ISerializer<TObject>
          where TObject : new()
    {

        /// <summary>
        /// The path for the serialization operations of this instance.
        /// </summary>
        private string FilePath { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="filePath">The value for the <see cref="FilePath"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a required dependency is not valid.</exception>
        public JsonNetFileSerializer(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentOutOfRangeException(nameof(filePath)); }
        }


        /// <inheritdoc/>
        public TObject Deserialize()
        {
            TObject result;
            try
            {
                using var reader = new StreamReader(FilePath);
                var contents = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<TObject>(contents);
            }
            catch (Exception e) when (e is FileNotFoundException || e is JsonReaderException)
            {
                result = default;
            }
            return result;
        }


        /// <inheritdoc/>
        public void Serialize(TObject input)
        {
            var contents = JsonConvert.SerializeObject(input);
            using var writer = new StreamWriter(FilePath);
            writer.Write(contents);
        }

    }

}
