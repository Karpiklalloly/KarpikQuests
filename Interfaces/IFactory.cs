namespace KarpikQuests.Interfaces
{
    public interface IFactory<out T>
    {
        public T Create();
    }
}