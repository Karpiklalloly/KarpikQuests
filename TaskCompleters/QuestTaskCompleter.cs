using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;

namespace KarpikQuests.TaskCompleters;

public class QuestTaskCompleter<T> : IQuestTaskCompleter<T>
    where T : IEquatable<T>
{
    public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

    public List<DataObserver<T>> Datas { get; private set; } = new();

    public List<T> RequieredValues { get; private set; } = new();

    public void Subscribe(IQuestTask task, ref T observableData, T requiredValue)
    {
        Tasks.Add(task);
        Datas.Add(new DataObserver<T>(ref observableData));
        RequieredValues.Add(requiredValue);
    }

    public void Update()
    {
        for (int i = 0;  i < RequieredValues.Count; i++)
        {
            if (Datas[i] == null)
            {
                continue;
            }

            if (Datas[i].Value.Equals(RequieredValues[i]))
            {
                Tasks.ElementAt(i)?.Complete();
            }
        }
    }
}
