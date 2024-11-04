using Karpik.Quests;

namespace KarpikQuestsTests.QuestTests
{
    public class RemoveQuest
    {
        [Test]
        public void WhenAddSubQuest_AndRemoveSubQuest_ThenQuestDoesNotHaveSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(bundle.Id));
    }
    
        [Test]
        public void WhenAddSubQuest_AndRemoveSubQuest_ThenQuestDoesNotHaveSubQuest2()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        quest.Add(new Quest());
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(bundle.Id));
    }
    
        [Test]
        public void WhenAddSubSubQuest_AndRemoveSubQuest_ThenQuestDoesNotHaveSubSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        var task = new Quest();
        bundle.Add(task);
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(task.Id));
    }
    
        [Test]
        public void WhenRemoveSubQuestNotAdded_ThenQuestDoesNotHaveSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(bundle.Id));
    }
    
        [Test]
        public void WhenRemoveSubSubQuestNotAdded_ThenQuestDoesNotHaveSubSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        var task = new Quest();
        bundle.Add(task);

        //Condition
        quest.Remove(task);

        //Result
        Assert.That(!quest.Has(task.Id));
    }
    
        [Test]
        public void WhenRemoveSubQuestMultipleTimes_ThenQuestDoesNotHaveSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(bundle.Id));
    }
    
        [Test]
        public void WhenRemoveSubSubQuestMultipleTimes_ThenQuestDoesNotHaveSubSubQuest()
    {
        //Action
        var quest = new Quest();
        var bundle = new Quest();
        var task = new Quest();
        bundle.Add(task);
        quest.Add(bundle);

        //Condition
        quest.Remove(task);
        quest.Remove(task);

        //Result
        Assert.That(!quest.Has(task.Id));
    }
    }
}