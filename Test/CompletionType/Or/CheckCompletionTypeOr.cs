using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;

namespace Test.CompletionType
{
    public class CheckCompletionTypeOr
    {
        [Test]
        public void WhenOr_AndCheckEmpty_ThenCompleted()
    {
        //Action
        ICompletionType type = new Or();
        var collection = new QuestCollection();

        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenOr_AndCheckSingleLocked_ThenLocked()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckSingleUnlocked_ThenUnlocked()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckSingleCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckSingleFailed_ThenFailed()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleLocked_ThenLocked()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleUnlocked_ThenUnlocked()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleFailed_ThenFailed()
    {
        //Action
        ICompletionType type = new Or();
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
        public void WhenOr_AndCheckOneCompleted_ThenCompleted()
    {
        //Action
        ICompletionType type = new Or();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        
        collection[0].ForceFail();
        collection[1].ForceComplete();
        collection[2].ForceUnlock();
        
        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenOr_AndCheckFailedLockedUnlocked_ThenUnlocked()
    {
        //Action
        ICompletionType type = new Or();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        
        collection[0].ForceFail();
        collection[1].ForceLock();
        collection[2].ForceUnlock();
        
        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Unlocked));
    }
    
        [Test]
        public void WhenOr_AndCheckOneFailed_ThenUnlocked()
    {
        //Action
        ICompletionType type = new Or();
        var collection = new QuestCollection
        {
            new(),
            new(),
            new()
        };
        
        collection[0].ForceFail();
        collection[1].ForceLock();
        collection[2].ForceUnlock();
        
        //Condition
        var result = type.Check(collection);

        //Result
        Assert.That(result, Is.EqualTo(Status.Unlocked));
    }
    }
}