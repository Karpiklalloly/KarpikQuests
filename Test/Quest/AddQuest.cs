using Karpik.Quests;

namespace KarpikQuestsTests.QuestTests
{
    public class AddQuest
    {
        [Test]
        public void WhenCreateQuest_AndAddSubQuest_ThenQuestHasThisSubQuest()
    {
        //Action
        var quest = new Quest();
        var subQuest = new Quest();

        //Condition
        quest.Add(subQuest);

        //Result
        Assert.That(quest.Has(subQuest.Id));
    }

        [Test]
        public void WhenCreateQuest_AndAddSubQuestWithSubQuest_ThenQuestHasThisSubSubQuest()
    {
        //Action
        var quest = new Quest();
        var subQuest = new Quest();
        var subSubQuest = new Quest();
        subQuest.Add(subSubQuest);

        //Condition
        quest.Add(subQuest);

        //Result
        Assert.That(quest.Has(subSubQuest.Id));
    }
    }
}