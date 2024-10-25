using NewKarpikQuests;
using NewKarpikQuests.Extensions;
using NewKarpikQuests.Sample;

namespace KarpikQuestsTests.GraphTests
{
    public class AddGraph
    {
        [Test]
        public void WhenCreateGraph_AndAddQuests_ThenGraphHasThem()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();

        //Condition
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(graph.Has(quest1));
            Assert.That(graph.Has(quest2));
        });
    }
    
        [Test]
        public void WhenCreateGraph_AndAddQuests_ThenGraphHasThemId()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();

        //Condition
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(graph.Has(quest1.Id));
            Assert.That(graph.Has(quest2.Id));
        });
    }
    }
}