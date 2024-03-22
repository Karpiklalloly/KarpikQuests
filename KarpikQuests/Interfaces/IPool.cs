namespace Karpik.Quests.Interfaces
{
    public interface IPool<in T>
    {
        public TGet Pull<TGet>() where TGet : T;
        public void Push(T instance);
    }
}