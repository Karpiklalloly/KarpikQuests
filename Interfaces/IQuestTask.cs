using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KarpikQuestsTest")]
namespace KarpikQuests.Interfaces;

public interface IQuestTask
{
    public string Name { get; }
    public TaskStatus Status { get; }
    protected internal bool CanBeCompleted { get; set; }

    public void Init(string name);

    public event Action<IQuestTask> Completed;

    public enum TaskStatus
    {
        Completed,
        UnCompleted
    }

    protected internal void Complete();

    protected internal void ForceBeCompleted();
}
