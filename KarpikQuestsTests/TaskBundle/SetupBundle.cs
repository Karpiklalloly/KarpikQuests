using Karpik.Quests.CompletionTypes;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class SetupBundle
{
    [Test]
    public void WhenCreateBundle_AndDontSetup_ThenFirstQuestCantBeCompleted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Orderly())
        {
            new Task(),
            new Task()
        };
        
        //Condition

        //Result
        Assert.That(!bundle[0].CanBeCompleted);
    }
    
    [Test]
    public void WhenCreateBundleWithOrderly_AndSetup_ThenFirstQuestCanBeCompleted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Orderly())
        {
            new Task(),
            new Task()
        };
        
        //Condition
        bundle.Setup();

        //Result
        Assert.That(bundle[0].CanBeCompleted);
    }
    
    [Test]
    public void WhenCreateBundleWithDisorderly_AndSetup_ThenLastQuestCanBeCompleted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Disorderly())
        {
            new Task(),
            new Task()
        };
        
        //Condition
        bundle.Setup();

        //Result
        Assert.That(bundle[^1].CanBeCompleted);
    }
}