namespace Karpik.Quests.Interfaces
{
    public interface ISerializer<T> where T : class
    {
        public string Serialize(T aggregator);

        public T Deserialize(string data);
    }
}
