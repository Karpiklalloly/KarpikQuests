using System;
using System.IO;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.Saving
{
    public static class QuestAggregatorSaver
    {
        private static ISerializer<IAggregator> _serializer = new JsonResolver<IAggregator>();

        public static ISerializer<IAggregator> Serializer
        {
            get
            {
                return _serializer;
            }
            set
            {
                _serializer = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public static void Save(IAggregator aggregator, string path, bool readable = false)
        {
            var str = Serializer.Serialize(aggregator);
            File.WriteAllText(path, str);
        }

        public static IAggregator Load(string path)
        {
            var data = File.ReadAllText(path);
            return Serializer.Deserialize(data);
        }

        [Serializable]
        public sealed class SaveData
        {
            [JsonProperty("Version")]
            [SerializeThis("Version")]
            public Version Version { get; set; }
            [JsonProperty("Aggregator")]
            [SerializeThis("Aggregator")]
            public IAggregator Aggregator { get; set; }
            [JsonProperty("AggregatorType")]
            [SerializeThis("AggregatorType")]
            public Type AggregatorType { get; set; }
        }
    }
}
