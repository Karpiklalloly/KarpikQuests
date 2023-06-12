using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;

namespace KarpikQuests.TaskCompleters;

public class BoolQuestTaskCompleter : IQuestTaskCompleter<bool>
{
    public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

    public List<DataObserver<bool>> Datas { get; private set; } = new();

    public List<bool> RequieredValues { get; private set; } = new();

    public void Subscribe(IQuestTask task, ref bool observableData, bool requiredValue)
    {
        Tasks.Add(task);
        Datas.Add(new DataObserver<bool>(ref observableData));
        RequieredValues.Add(requiredValue);
    }

    public void Update()
    {
        for (int i = 0; i < RequieredValues.Count; i++)
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
