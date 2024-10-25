using NewKarpikQuests;
using NewKarpikQuests.Extensions;
using NewKarpikQuests.Sample;

namespace KarpikQuestsTests.GraphTests
{
    public class RemoveGraph
    {
        [Test]
        public void WhenAddQuests_AndRemoveThem_ThenGraphDoesNotHaveThem()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);

        //Condition
        graph.TryRemove(quest1);
        graph.TryRemove(quest2);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.Has(quest1));
            Assert.That(!graph.Has(quest2));
        });
    }
    
        [Test]
        public void WhenAddQuests_AndRemoveThemId_ThenGraphDoesNotHaveThem()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);

        //Condition
        graph.TryRemove(quest1.Id);
        graph.TryRemove(quest2.Id);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.Has(quest1));
            Assert.That(!graph.Has(quest2));
        });
    }
    
        [Test]
        public void WhenAddQuests_AndRemoveThem_ThenGraphDoesNotHaveThemId()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);

        //Condition
        graph.TryRemove(quest1);
        graph.TryRemove(quest2);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.Has(quest1.Id));
            Assert.That(!graph.Has(quest2.Id));
        });
    }
    
        [Test]
        public void WhenAddQuests_AndRemoveThemId_ThenGraphDoesNotHaveThemId()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);

        //Condition
        graph.TryRemove(quest1.Id);
        graph.TryRemove(quest2.Id);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.Has(quest1.Id));
            Assert.That(!graph.Has(quest2.Id));
        });
    }
    }
}