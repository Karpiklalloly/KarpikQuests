using System.Reflection;
using NewKarpikQuests;
using NewKarpikQuests.Sample;

namespace Test
{
    public static class Utils
    {
        public static QuestCollection Quests(this Quest quest)
    {
        return (QuestCollection)quest
            .GetType()
            .GetField("_subQuests", BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(quest);
    }
    }
}