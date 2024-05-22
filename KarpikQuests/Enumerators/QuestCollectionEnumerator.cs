using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Enumerators
{
    public class QuestCollectionEnumerator : Enumerator<IQuest>
    {
        public QuestCollectionEnumerator(IQuestCollection collection) : base(collection)
        {
        }
    }
}