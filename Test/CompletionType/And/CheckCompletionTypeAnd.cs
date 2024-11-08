using Karpik.Quests;
using Karpik.Quests.Requirements;

namespace Test.CompletionType
{
    public class CheckCompletionTypeAnd
    {
        [Test]
        public void WhenAnd_AndCheckEmpty_ThenCompleted()
        {
            //Action
            ICompletionType type = new And();
            var collection = new List<IRequirement>();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Completed));
        }
    
        [Test]
        public void WhenAnd_AndCheckSingleLocked_ThenLocked()
        {
            //Action
            ICompletionType type = new And();
            var quest = new Quest();
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(quest)
            };
            quest.ForceLock();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenAnd_AndCheckSingleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new And();
            var quest = new Quest();
            var collection = new List<IRequirement>
            {
                new QuestHasStatus(quest)
            };
            quest.ForceUnlock();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Locked));
        }
    
        [Test]
        public void WhenAnd_AndCheckSingleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new And();
            var quest = new Quest();
            var collection = new List<IRequirement>()
            {
                new QuestHasStatus(quest)
            };
            quest.ForceComplete();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Completed));
        }
    
        [Test]
        public void WhenAnd_AndCheckSingleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new And();
            var quest = new Quest();
            var collection = new List<IRequirement>()
            {
                new QuestHasStatus(quest)
            };
            quest.ForceFail();

            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    
        [Test]
        public void WhenAnd_AndCheckMultipleLocked_ThenLocked()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
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
        public void WhenAnd_AndCheckMultipleUnlocked_ThenLocked()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
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
        public void WhenAnd_AndCheckMultipleCompleted_ThenCompleted()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
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
        public void WhenAnd_AndCheckMultipleFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
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
        public void WhenAnd_AndCheckOneCompleted_ThenUnlocked()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
            };
            
            collectionQ[0].ForceLock();
            collectionQ[1].ForceComplete();
            collectionQ[2].ForceUnlock();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Unlocked));
        }
    
        [Test]
        public void WhenAnd_AndCheckOneFailed_ThenFailed()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
            };
            
            collectionQ[0].ForceFail();
            collectionQ[1].ForceComplete();
            collectionQ[2].ForceUnlock();
            
            //Condition
            var result = type.Check(collection);

            //Result
            Assert.That(result, Is.EqualTo(Status.Failed));
        }
    
        [Test]
        public void WhenAnd_AndCheckFailedLockedUnlocked_ThenFailed()
        {
            //Action
            ICompletionType type = new And();
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
                new QuestHasStatus(collectionQ[2])
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