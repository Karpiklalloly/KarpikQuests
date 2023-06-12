using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;

namespace KarpikQuests.TaskCompleters;

public class StringQuestTaskCompleter : IQuestTaskCompleter<string>
{
    public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

    public List<DataObserver<string>> Datas { get; private set; } = new();

    public List<string> RequieredValues { get; private set; } = new();

    public void Subscribe(IQuestTask task, ref string observableData, string requiredValue)
    {
        Tasks.Add(task);
        Datas.Add(new DataObserver<string>(ref observableData));
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
