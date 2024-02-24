using KarpikQuests.Interfaces;

namespace KarpikQuestsExample
{
    internal interface IQuestLine
    {
        public IQuestAggregator Aggregator { get; }

        public void DeInit();
        public void Init();
        public void Start();
    }
}