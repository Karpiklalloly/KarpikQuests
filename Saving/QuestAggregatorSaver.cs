using KarpikQuests.Interfaces;
using System;
using System.IO;

namespace KarpikQuests.Saving
{
    public static class QuestAggregatorSaver
    {
        private static ISerializer<IQuestAggregator> _serializer = new JsonResolver();

        public static ISerializer<IQuestAggregator> Serializer
        {
            get
            {
                return _serializer;
            }
            set
            {
                if (value == null)
                {
#if DEBUG
                    throw new ArgumentNullException(nameof(value));
#endif
                }
                _serializer = value;
            }
        }

        public static void Save(IQuestAggregator aggregator, string path, bool readable = false)
        {
            var str = Serializer.Serialize(aggregator);
            File.WriteAllText(path, str);
        }

        public static IQuestAggregator? Load(string path)
        {
            return Serializer.Deserialize(File.ReadAllText(path));
        }

        [Serializable]
        public sealed class SaveData
        {
            [SerializeThis(Name = "Version")]
            public Version Version { get; set; }
            [SerializeThis(Name = "Aggregator")]
            public IQuestAggregator Aggregator { get; set; }
            [SerializeThis(Name = "AggregatorType")]
            public Type AggregatorType { get; set; }
        }
    }
}
