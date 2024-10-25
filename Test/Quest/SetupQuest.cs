using System.Reflection;
using NewKarpikQuests;
using NewKarpikQuests.Extensions;
using NewKarpikQuests.Sample;
using Test;

namespace KarpikQuestsTests.QuestTests
{
    public class SetupQuest
    {
        [Test]
        public void WhenSetup_AndQuestIsJustCreated_ThenStatusIsLocked()
    {
        //Action
        var quest = new Quest();
        
        //Condition
        quest.Setup();

        //Result
        Assert.That(quest.IsLocked());
    }
    
        [Test]
        public void WhenSetup_AndQuestHasSubQuests_ThenAllStatusesAreLocked()
    {
        //Action
        var quest = new Quest();
        var subQuest1 = new Quest();
        var subQuest2 = new Quest();
        var subSubQuest1 = new Quest();
        var subSubQuest2 = new Quest();
        subQuest1.Add(subSubQuest1);
        subQuest2.Add(subSubQuest2);
        quest.Add(subQuest1);
        quest.Add(subQuest2);
        
        //Condition
        quest.Setup();

        //Result
        Assert.Multiple(() =>
        {
            Assert.That(quest.IsLocked());
            Assert.That(quest.Quests().All(x => x.IsLocked()));
            Assert.That(quest.Quests().SelectMany(x => x.Quests()).All(x => x.IsLocked()));
        });
    }
    }
}