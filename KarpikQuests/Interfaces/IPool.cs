namespace Karpik.Quests.Interfaces
{
    public interface IPool<T>
    {
        public T PullDefault();
        public TGet Pull<TGet>() where TGet : T, new();
        public void Push(T instance);
    }
}