using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Tests.Quest;

public class CreateQuest
{
    [Test]
    public void WhenCreateQuest_AndSetCorrectId_ThenQuestHasThisId()
    {
        //Action
        Id id = Id.NewId();

        //Condition
        var quest = new QuestSample.Quest(id);

        //Result
        Assert.That(quest.Id, Is.EqualTo(id));
    }


}