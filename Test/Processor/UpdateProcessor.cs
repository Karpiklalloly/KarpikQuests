using Karpik.Quests;
using Karpik.Quests.Processors;

namespace Test.Processor
{
    public class UpdateProcessor
    {
        [Test]
        public void WhenOrderly_AndUpdateSingleLocked_ThenItIsLocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceLock();

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection[0].IsLocked(), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateSingleUnlocked_ThenItIsUnlocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceUnlock();

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection[0].IsUnlocked(), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateSingleCompleted_ThenItIsCompleted()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceComplete();

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection[0].IsCompleted(), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateSingleFailed_ThenItIsFailed()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceFail();

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection[0].IsFailed(), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateMultipleLocked_ThenTheyAreLocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        foreach (var quest in collection)
        {
            quest.ForceLock();
        }

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection.All(x => x.IsLocked()), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateMultipleUnlocked_ThenTheyAreUnlocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        foreach (var quest in collection)
        {
            quest.ForceUnlock();
        }

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection.All(x => x.IsUnlocked()), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateMultipleCompleted_ThenTheyAreCompleted()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        foreach (var quest in collection)
        {
            quest.ForceComplete();
        }

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection.All(x => x.IsCompleted()), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateMultipleFailed_ThenTheyAreFailed()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        foreach (var quest in collection)
        {
            quest.ForceFail();
        }

        //Condition
        processor.Update(collection);

        //Result
        Assert.That(collection.All(x => x.IsFailed()), Is.True);
    }
    
        [Test]
        public void WhenOrderly_AndUpdateFirstCompleted_ThenSecondIsUnlockedThirdIsLocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        collection[0].ForceComplete();

        //Condition
        processor.Update(collection);

        //Result
        Assert.Multiple(() =>
        {
            Assert.That(collection[0].IsCompleted(), Is.True);
            Assert.That(collection[1].IsUnlocked(), Is.True);
            Assert.That(collection[2].IsLocked(), Is.True);
        });
    }
    
        [Test]
        public void WhenOrderly_AndUpdateFirstIsNotFinishedSecondIsCompleted_ThenNothingIsChanged()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        collection[0].ForceUnlock();
        collection[1].ForceComplete();
        collection[2].ForceLock();

        //Condition
        processor.Update(collection);

        //Result
        Assert.Multiple(() =>
        {
            Assert.That(collection[0].IsFinished(), Is.False);
            Assert.That(collection[1].IsCompleted(), Is.True);
            Assert.That(collection[2].IsLocked(), Is.True);
        });
    }
    
        [Test]
        public void WhenOrderly_AndUpdateFirstIsFinishedSecondIsCompleted_ThenThirdIsUnlocked()
    {
        //Action
        IProcessorType processor = new Orderly();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        collection[0].ForceFail();
        collection[1].ForceComplete();
        collection[2].ForceLock();

        //Condition
        processor.Update(collection);

        //Result
        Assert.Multiple(() =>
        {
            Assert.That(collection[0].IsFinished(), Is.True);
            Assert.That(collection[1].IsCompleted(), Is.True);
            Assert.That(collection[2].IsUnlocked(), Is.True);
        });
    }
    }
}