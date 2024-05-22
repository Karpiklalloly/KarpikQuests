namespace Karpik.Quests.Interfaces
{
    public interface ISingleton<out T>
    {
        public static T Instance { get; }
    }
}