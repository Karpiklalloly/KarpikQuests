namespace KarpikQuests.Interfaces
{
    public interface IInitable
    {
        public bool Inited {get; }
        public void Init();
    }
}