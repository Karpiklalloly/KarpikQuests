namespace KarpikQuests.Interfaces;

public interface IQuestTaskCompleter<T>
    where T : IEquatable<T>
{
    public IQuestTaskCollection Tasks { get; }
    public List<DataObserver<T>> Datas { get; }
    public List<T> RequieredValues { get; }

    public void Subscribe(IQuestTask task, ref T observableData, T requiredValue);

    public void Update();
}
