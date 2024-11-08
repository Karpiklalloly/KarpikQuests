using Karpik.Quests;
using Karpik.Quests.Requirements;

namespace Test.CompletionType
{
    public class CheckCompletionTypeXor
    {
        [Test]
        public void WhenXor_AndCheckEmpty_ThenCompleted()
        {
            //Action
            ICompletionType type = new Xor();
            var collection = new List<IRequirement>();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Completed));
        }
    
        [Test]
        public void WhenXor_AndCheckSingleLocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckSingleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckSingleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckSingleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckMultipleLocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckMultipleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckMultipleCompleted_ThenFailed()
        {
            //Action
            ICompletionType type = new Xor();
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
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    
        [Test]
        public void WhenXor_AndCheckMultipleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckOneCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckFailedLockedUnlocked_ThenFailed()
        {
            //Action
            ICompletionType type = new Xor();
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
        public void WhenXor_AndCheckOneFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new Xor();
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