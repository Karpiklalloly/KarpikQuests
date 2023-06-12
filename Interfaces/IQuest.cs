namespace KarpikQuests.Interfaces;

public interface IQuest: IEquatable<IQuest>
{
    public string Key { get; }

    public string Name { get; }
    public string Description { get; }

    public IEnumerable<IQuestTask> Tasks { get; }

    public IQuestStatus Status { get; }

    public event Action<IQuest> Started;
    public event Action<IQuest, IQuestTask> Updated;
    public event Action<IQuest> Completed;

    protected internal void Init(string key, string name, string description);
    protected internal void Start();
    protected internal void SetKey(string key);
    protected internal void AddTask(IQuestTask task);
    protected internal void OnTaskComplete(IQuestTask task);
}
