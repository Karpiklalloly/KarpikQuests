using Karpik.Quests;

namespace KarpikQuestsTests.QuestTests
{
    public class HasQuest
    {
        [Test]
        public void WhenHas_AndDontHasQuest_ThenFalse()
    {
        //Action
        var quest = new Quest();
        quest.Add(new Quest());

        //Condition
        var result = quest.Has(new Quest());

        //Result
        Assert.That(result, Is.EqualTo(false));
    }
    
        [Test]
        public void WhenHas_AndHasQuest_ThenTrue()
    {
        //Action
        var quest = new Quest();
        var subQuest = new Quest();
        quest.Add(subQuest);

        //Condition
        var result = quest.Has(subQuest);

        //Result
        Assert.That(result, Is.EqualTo(true));
    }
    
        [Test]
        public void WhenHas_AndHasSubQuest_ThenTrue()
    {
        //Action
        var quest = new Quest();
        var subQuest = new Quest();
        var subSubQuest = new Quest();
        subQuest.Add(subSubQuest);
        quest.Add(subQuest);

        //Condition
        var result = quest.Has(subSubQuest);

        //Result
        Assert.That(result, Is.EqualTo(true));
    }
    }
}