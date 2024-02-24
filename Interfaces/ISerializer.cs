namespace KarpikQuests.Interfaces
{
    public interface ISerializer<T> where T : class, IQuestAggregator
    {
        public string Serialize(T aggregator);

        public T? Deserialize(string data);
    }
}
