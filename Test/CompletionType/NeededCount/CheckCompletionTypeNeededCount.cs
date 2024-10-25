using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;

namespace Test.CompletionType
{
    public class CheckCompletionTypeNeededCount
    {
        [Test]
        public void WhenNeededCountNot0_AndCheckEmpty_ThenUnlocked()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Unlocked));
    }
    
        [Test]
        public void WhenNeededCount0_AndCheckEmpty_ThenCompleted()
    {
        //Action
        ICompletionType type = new NeededCount(0);
        var collection = new QuestCollection();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleLocked_ThenLocked()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceLock();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Locked));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleUnlocked_ThenUnlocked()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceUnlock();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Unlocked));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceComplete();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleFailed_ThenFailed()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection
        {
            new()
        };
        collection[0].ForceFail();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Failed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleLocked_ThenLocked()
    {
        //Action
        ICompletionType type = new NeededCount(1);
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
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Locked));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleUnlocked_ThenUnlocked()
    {
        //Action
        ICompletionType type = new NeededCount(1);
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
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Unlocked));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new NeededCount(1);
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
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleFailed_ThenFailed()
    {
        //Action
        ICompletionType type = new NeededCount(1);
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
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Failed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckEnoughCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new NeededCount(1);
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        
        collection[0].ForceLock();
        collection[1].ForceComplete();
        collection[2].ForceUnlock();
        
        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenNeededCount_AndCheckEnoughFailed_ThenFailed()
    {
        //Action
        ICompletionType type = new NeededCount(2);
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        
        collection[0].ForceFail();
        collection[1].ForceComplete();
        collection[2].ForceFail();
        
        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Failed));
    }
    }
}