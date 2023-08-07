using KarpikQuests.Interfaces;
using System;
using System.IO;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

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

        public static IQuestAggregator Load(string path)
        {
#if JSON_NEWTONSOFT
            return LoadJsonNewtonsoft(path);
#endif
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

            var json = JsonConvert.SerializeObject(aggregator, settings);

            File.WriteAllText(path, json);
        }

        private static IQuestAggregator LoadJsonNewtonsoft(string path)
        {
            var json = File.ReadAllText(path);
            var aggregator = JsonConvert.DeserializeObject<KarpikQuests.QuestSample.QuestAggregator>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return aggregator;
        }
#endif
        //TODO: Implement
        private class SaveData
        {
            public Version Version { get; set; }
            public IQuestAggregator Aggregator { get; set; }
        }
    }
}
