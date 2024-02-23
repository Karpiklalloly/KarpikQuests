using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Saving
{
    public static class QuestAggregatorSaver
    {
        public static void Save(IQuestAggregator aggregator, string path, bool readable = false)
        {
#if JSON_NEWTONSOFT
            SaveJsonNewtonsoft(aggregator, path, readable);
#endif
        }

        public static IQuestAggregator? Load(string path)
        {
#if JSON_NEWTONSOFT
            return LoadJsonNewtonsoft(path);
#endif
            return null;
        }

#if JSON_NEWTONSOFT
        private static void SaveJsonNewtonsoft(IQuestAggregator aggregator, string path, bool readable = false)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };

            if (readable)
            {
                settings.Formatting = Formatting.Indented;
            }

            SaveData data = new SaveData()
            {
                Version = Environment.Version,
                Aggregator = aggregator
            };
            var json = JsonConvert.SerializeObject(data, settings);

            File.WriteAllText(path, json);
        }

        private static IQuestAggregator? LoadJsonNewtonsoft(string path)
        {
            var json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<SaveData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (data is null) return null;
            return data.Aggregator;
        }
#endif
        [Serializable]
        private sealed class SaveData
        {
            public Version Version { get; set; }
            public IQuestAggregator Aggregator { get; set; }
        }
    }
}
