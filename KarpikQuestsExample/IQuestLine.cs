using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Example
{
    internal interface IQuestLine
    {
        public IAggregator Aggregator { get; }

        public void DeInit();
        public void Init();
        public void Start();
    }
}