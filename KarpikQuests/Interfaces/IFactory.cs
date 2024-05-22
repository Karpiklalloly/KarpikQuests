namespace Karpik.Quests.Interfaces
{
    public interface IFactory<out T>
    {
        public T Create();
    }
}