using KarpikQuests.Interfaces;

namespace KarpikQuests.QuestSample;

public class QuestTask : IQuestTask
{
    public string Name { get; private set; }

    public IQuestTask.TaskStatus Status { get; private set; } = IQuestTask.TaskStatus.UnCompleted;

    bool IQuestTask.CanBeCompleted { get; set; }

    public event Action<IQuestTask> Completed;

    public void Init(string name)
    {
        Name = name;
    }

    void IQuestTask.Complete()
    {
        if (!(this as IQuestTask).CanBeCompleted)
        {
            return;
        }

        Status = IQuestTask.TaskStatus.Completed;
        Completed?.Invoke(this);
    }

    public override string ToString()
    {
        return $"{Name} -> {Status}";
    }

    void IQuestTask.ForceBeCompleted()
    {
        (this as IQuestTask).CanBeCompleted = true;
    }
}
