using KarpikQuests.Interfaces;
using Task = KarpikQuests.QuestSample.Task;

namespace KarpikQuestsTests;

internal class QuestTaskTests
{
    [Test]
    public void WhenTaskInited_AndSettedKeyNameDescription_ThenTheyAreNotNULL([Values("key", "task", "KEY")] string key,
                                                                              [Values("name", "task", "NAME")] string name,
                                                                              [Values("desc", "task", "DESC")] string desc)
    {
        ITask task = new Task();

        task.Init(key, name, desc);
        Assert.Multiple(() =>
        {
            Assert.That(task.Key, Is.EqualTo(key));
            Assert.That(task.Name, Is.EqualTo(name));
            Assert.That(task.Description, Is.EqualTo(desc));
        });
    }
}
