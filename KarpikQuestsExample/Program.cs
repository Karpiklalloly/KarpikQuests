namespace Karpik.Quests.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IQuestLine questLine = new BasicVariativeQuest();
            questLine.Init();
            questLine.Start();
            questLine.DeInit();
        }
    }
}