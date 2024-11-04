using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;

namespace Karpik.Quests.Enumerators
{
    public class QuestCollectionEnumerator : Enumerator<Quest>
    {
        public QuestCollectionEnumerator(IQuestCollection collection) : base(collection)
        {
        }
    }
}