using NewKarpikQuests;
using NewKarpikQuests.Interfaces;
using NewKarpikQuests.Sample;

namespace KarpikQuestsTests.BuilderTests
{
    internal class QuestBuilderTests
    {
        private IGraph _graph;

        [SetUp]
        public void Setup()
    {
        _graph = new Graph();
    }
    
        [Test]
        public void WhenBuilderCreatesQuest_AndAddAggregatorOnCreate_ThenThereIsQuest()
    {
        QuestBuilder
            .Start("", "")
            .SetGraph(_graph)
            .Build();

        Assert.That(_graph.Quests, Is.Not.Empty);
    }

        [TearDown]
        public void TearDown()
    {
        _graph.Dispose();
    }
    }
}
