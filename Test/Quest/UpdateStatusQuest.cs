using Karpik.Quests;

namespace KarpikQuestsTests.QuestTests
{
    public class UpdateStatusQuest
    {
        [Test]
        public void WhenUpdateStatus_AndNoSubQuests_ThenStatusIsNotChanged(
            [Values(
                Status.Completed,
                Status.Locked,
                Status.Failed,
                Status.Unlocked)] Status status)
    {
        //Action
        var quest = new Quest();

        switch (quest.Status)
        {
            case Status.Locked:
                quest.ForceLock();
                break;
            case Status.Unlocked:
                quest.ForceUnlock();
                break;
            case Status.Completed:
                quest.ForceComplete();
                break;
            case Status.Failed:
                quest.ForceFail();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var was = quest.Status;
        
        //Condition
        quest.UpdateStatus();

        //Result
        Assert.That(quest.Status, Is.EqualTo(was));
    }
    
        [Test]
        public void WhenUpdateStatus_AndAllSubQuestsAreCompleted_ThenStatusIsCompleted()
    {
        //Action
        var quest = new Quest();
        var subQuest1 = new Quest();
        var subQuest2 = new Quest();
        quest.Add(subQuest1);
        quest.Add(subQuest2);
        
        subQuest1.ForceComplete();
        subQuest2.ForceComplete();
        
        //Condition
        quest.UpdateStatus();

        //Result
        Assert.That(quest.Status, Is.EqualTo(Status.Completed));
    }
    
        [Test]
        public void WhenUpdateStatus_AndAllSubQuestsAreLocked_ThenStatusIsLocked()
    {
        //Action
        var quest = new Quest();
        var subQuest1 = new Quest();
        var subQuest2 = new Quest();
        quest.Add(subQuest1);
        quest.Add(subQuest2);
        
        subQuest1.ForceLock();
        subQuest2.ForceLock();
        
        //Condition
        quest.UpdateStatus();

        //Result
        Assert.That(quest.Status, Is.EqualTo(Status.Locked));
    }
    
        [Test]
        public void WhenUpdateStatus_AndAllSubQuestsAreUnlocked_ThenStatusIsUnlocked()
    {
        //Action
        var quest = new Quest();
        var subQuest1 = new Quest();
        var subQuest2 = new Quest();
        quest.Add(subQuest1);
        quest.Add(subQuest2);
        
        subQuest1.ForceUnlock();
        subQuest2.ForceUnlock();
        
        //Condition
        quest.UpdateStatus();

        //Result
        Assert.That(quest.Status, Is.EqualTo(Status.Unlocked));
    }
    
        [Test]
        public void WhenUpdateStatus_AndAllSubQuestsAreFailed_ThenStatusIsFailed()
    {
        //Action
        var quest = new Quest();
        var subQuest1 = new Quest();
        var subQuest2 = new Quest();
        quest.Add(subQuest1);
        quest.Add(subQuest2);
        
        subQuest1.ForceFail();
        subQuest2.ForceFail();
        
        //Condition
        quest.UpdateStatus();

        //Result
        Assert.That(quest.Status, Is.EqualTo(Status.Failed));
    }
    }
}