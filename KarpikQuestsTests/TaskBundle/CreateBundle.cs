using Karpik.Quests.CompletionTypes;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class CreateBundle
{
    [Test]
    public void WhenCreateBundle_ThenBundleIsEmpty()
    {
        var bundle = new TaskBundle();
        
        Assert.That(!bundle.Any());
    }
    
    [Test]
    public void WhenCreateBundle_AndAddTasks_ThenBundleHasAllOfThem([Values(1, 10, 100)]int count)
    {
        var bundle = new TaskBundle();
        
        for (int i = 0; i < count; i++)
        {
            var task = new Task();
            task.Init("task", $"desc{i}");
            bundle.Add(task);
        }

        Assert.That(bundle, Has.Count.EqualTo(count));
    }
    
    [Test]
    public void WhenCreateBundle_AndSetAddType_ThenBundleHasAndType()
    {
        var bundle = new TaskBundle(new And(), new Disorderly());

        Assert.That(bundle.CompletionType is And);
    }
    
    [Test]
    public void WhenCreateBundle_AndSetOrType_ThenBundleHasOrType()
    {
        var bundle = new TaskBundle(new Or(), new Disorderly());

        Assert.That(bundle.CompletionType is Or);
    }
    
    [Test]
    public void WhenCreateBundle_AndSetOrderlyType_ThenBundleHasOrderlyType()
    {
        var bundle = new TaskBundle(null, new Orderly());

        Assert.That(bundle.ProcessorType is Orderly);
    }
    
    [Test]
    public void WhenCreateBundle_AndSetDisorderlyType_ThenBundleHasDisorderlyType()
    {
        var bundle = new TaskBundle(null, new Disorderly());

        Assert.That(bundle.ProcessorType is Disorderly);
    }
}