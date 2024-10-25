using Karpik.Quests;

namespace KarpikQuestsTests.QuestTests
{
    public class ClearQuest
    {
        [Test]
        public void WhenAddSubQuests_AndClearQuest_ThenQuestDoesNotHaveThem()
    {
        //Action
        var quest = new Quest();
        var task1 = new Quest();
        var task2 = new Quest();
        quest.Add(task1);
        quest.Add(task2);

        //Condition
        quest.Clear();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!quest.Has(task1));
            Assert.That(!quest.Has(task2));
        });
    }
    }
}