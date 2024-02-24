using Newtonsoft.Json;
using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeThisAttribute : Attribute, ISerializeAttribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public Version Version { get; set; }
        public bool IsReference { get; set; }
    }

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
