using NewKarpikQuests.Interfaces;
using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Enumerators
{
    public class QuestCollectionEnumerator : Enumerator<Quest>
    {
        public QuestCollectionEnumerator(IQuestCollection collection) : base(collection)
        {
        }
    }
}