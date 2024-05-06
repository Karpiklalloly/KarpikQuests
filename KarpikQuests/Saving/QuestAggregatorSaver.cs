using System;
using System.IO;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.Saving
{
    public static class QuestAggregatorSaver
    {
        private static ISerializer<IQuestAggregator> _serializer = new JsonResolver<IQuestAggregator>();

        public static ISerializer<IQuestAggregator> Serializer
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

        public static void Save(IQuestAggregator aggregator, string path, bool readable = false)
        {
            var str = Serializer.Serialize(aggregator);
            File.WriteAllText(path, str);
        }

        public static IQuestAggregator Load(string path)
        {
            var data = File.ReadAllText(path);
            return Serializer.Deserialize(data);
        }

        [Serializable]
        public sealed class SaveData
        {
            [JsonProperty("Version")]
            public Version Version { get; set; }
            [JsonProperty("Aggregator")]
            public IQuestAggregator Aggregator { get; set; }
            [JsonProperty("AggregatorType")]
            public Type AggregatorType { get; set; }
        }
    }
}
