using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.Saving
{
    public class JsonResolver<T> : ISerializer<T> where T : class
    {
        public T Deserialize(string data)
        {
            return JsonConvert.DeserializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,

            }) as T;
        }

        public string Serialize(T aggregator)
        {
            return JsonConvert.SerializeObject(aggregator, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}