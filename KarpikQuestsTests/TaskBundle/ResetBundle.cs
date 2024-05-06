using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Tests;

public class ResetBundle
{
    [Test]
    public void WhenCreateBundle_AndReset_ThenBundleIsUnstarted()
    {
        //Action
        TaskBundle bundle = new TaskBundle();

        //Condition
        bundle.Reset();

        //Result
        Assert.That(bundle.IsUnStarted());
    }
}