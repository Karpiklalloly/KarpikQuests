using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Requirements;
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
            var collection = new List<IRequirement>();

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
        public void WhenOr_AndCheckSingleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckSingleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckSingleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleLocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckMultipleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Or();
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
        public void WhenOr_AndCheckOneCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new Or();
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
            collectionQ[2].ForceUnlock();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Completed));
        }
    
        [Test]
        public void WhenOr_AndCheckFailedLockedUnlocked_ThenFailed()
        {
            //Action
            ICompletionType type = new Or();
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
            collectionQ[1].ForceLock();
            collectionQ[2].ForceUnlock();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    
        [Test]
        public void WhenOr_AndCheckOneFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Or();
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
            collectionQ[1].ForceLock();
            collectionQ[2].ForceUnlock();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    }
}