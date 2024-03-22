using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Example
{
    internal interface IQuestLine
    {
        public IQuestAggregator Aggregator { get; }

        public void DeInit();
        public void Init();
        public void Start();
    }
}