using Karpik.Quests;
using Karpik.Quests.Processors;

namespace Test.Processor
{
    public class SetupProcessor
    {
        [Test]
        public void WhenDisorderly_AndSetupEmpty_ThenAssertTrue()
    {
        //Action
        IProcessorType processor = new Disorderly();
        var collection = new QuestCollection();
        
        //Condition
        processor.Setup(collection);

        //Result
        Assert.That(true);
    }
    
        [Test]
        public void WhenDisorderly_AndSetupSingle_ThenItIsUnlocked()
    {
        //Action
        IProcessorType processor = new Disorderly();
        var collection = new QuestCollection
        {
            new()
        };

        //Condition
        processor.Setup(collection);

        //Result
        Assert.That(collection[0].IsUnlocked());
    }
    
        [Test]
        public void WhenDisorderly_AndSetupMultiple_ThenTheyAreUnlocked()
    {
        //Action
        IProcessorType processor = new Disorderly();
        var collection = new QuestCollection
        {
            new(),
            new()
        };

        //Condition
        processor.Setup(collection);

        //Result
        Assert.That(collection.All(x => x.IsUnlocked()));
    }
    
        [Test]
        public void WhenOrderly_AndSetupEmpty_ThenAssertTrue()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection();
        
        //Condition
        processor.Setup(collection);

        //Result
        Assert.That(true);
    }
    
        [Test]
        public void WhenOrderly_AndSetupSingle_ThenItIsUnlocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new()
        };

        //Condition
        processor.Setup(collection);

        //Result
        Assert.That(collection[0].IsUnlocked());
    }
    
        [Test]
        public void WhenOrderly_AndSetupMultiple_ThenOnlyFirstIsUnlocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new()
        };

        //Condition
        processor.Setup(collection);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(collection[0].IsUnlocked());
            Assert.That(collection[1].IsLocked());
        });
    }
    }
}