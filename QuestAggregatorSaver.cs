using KarpikQuests.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KarpikQuests
{
    //TODO: Где-нибудь сделать так, чтобы после загрузки квесты с одинаковыми ключами ссылались на одну ячейку памяти
    public static class QuestAggregatorSaver
    {
        public static void Save(IQuestAggregator aggregator, string path, bool readable = false)
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

        public static IQuestAggregator Load(string path)
        {
            var json = File.ReadAllText(path);
            var aggregator = JsonConvert.DeserializeObject<KarpikQuests.QuestSample.QuestAggregator>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return aggregator;
        }
    }
}
