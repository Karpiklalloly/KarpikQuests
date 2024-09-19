using System;
using System.IO;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Saving
{
    public static class QuestAggregatorSaver
    {
        private static ISerializer<IAggregator> _serializer;

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
            [SerializeThis("Version")]
            public Version Version;

            [SerializeThis("Aggregator")]
            public IAggregator Aggregator;

            [SerializeThis("AggregatorType")]
            public Type AggregatorType;
        }
    }
}
