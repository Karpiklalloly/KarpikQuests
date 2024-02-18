namespace KarpikQuests.Interfaces
{
    public interface IFactory<T>
    {
        public T Create();
    }
}