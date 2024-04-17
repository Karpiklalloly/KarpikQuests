using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Enumerators
{
    public class GraphCollectionEnumerator : Enumerator<IGraph>
    {
        public GraphCollectionEnumerator(IGraphCollection collection) : base(collection)
        {
        }
    }
}