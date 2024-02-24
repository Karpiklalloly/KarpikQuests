using KarpikQuests.Interfaces;
using Newtonsoft.Json;

namespace KarpikQuests.Saving
{
    class JsonResolver : ISerializer<IQuestAggregator>
    {
        public IQuestAggregator? Deserialize(string data)
        {
            return JsonConvert.DeserializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,

            }) as IQuestAggregator;
        }

        public string Serialize(IQuestAggregator aggregator)
        {
            return JsonConvert.SerializeObject(aggregator, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
