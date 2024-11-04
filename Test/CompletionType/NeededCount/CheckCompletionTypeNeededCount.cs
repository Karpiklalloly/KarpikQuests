using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Requirements;
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
            var collection = new List<IRequirement>();

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
            var collection = new List<IRequirement>();

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
            var collectionQ = new QuestCollection
            {
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
            };
            collectionQ[0].ForceLock();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new NeededCount(1);
            var collectionQ = new QuestCollection
            {
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
            };
            collectionQ[0].ForceUnlock();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenNeededCount_AndCheckSingleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new NeededCount(1);
            var collectionQ = new QuestCollection
            {
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
            };
            collectionQ[0].ForceComplete();

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
            var collectionQ = new QuestCollection
            {
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
            };
            collectionQ[0].ForceFail();

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
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            foreach (var quest in collectionQ)
            {
                quest.ForceLock();    
            }

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new NeededCount(1);
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            foreach (var quest in collectionQ)
            {
                quest.ForceUnlock();    
            }

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenNeededCount_AndCheckMultipleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new NeededCount(1);
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            foreach (var quest in collectionQ)
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
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            foreach (var quest in collectionQ)
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
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            collectionQ[0].ForceLock();
            collectionQ[1].ForceComplete();
            collectionQ[2].ForceUnlock();
            
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
            var collectionQ = new QuestCollection
            {
                new(),
                new(),
                new()
            };
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(collectionQ[0]),
                new QuestHasStatus(collectionQ[1]),
                new QuestHasStatus(collectionQ[2]),
            };
            
            collectionQ[0].ForceFail();
            collectionQ[1].ForceComplete();
            collectionQ[2].ForceFail();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    }
}