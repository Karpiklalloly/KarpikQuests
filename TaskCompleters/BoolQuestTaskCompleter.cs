using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.TaskCompleters
{
    [System.Serializable]
    public class BoolQuestTaskCompleter : IQuestTaskCompleter<bool>
    {
        public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

        public List<DataObserver<bool>> Datas { get; private set; } = new List<DataObserver<bool>>();

        public List<bool> RequiredValues { get; private set; } = new List<bool>();

        public void Subscribe(IQuestTask task, ref bool observableData, bool requiredValue)
        {
            Tasks.Add(task);
            Datas.Add(new DataObserver<bool>(ref observableData));
            RequiredValues.Add(requiredValue);
        }

        public void Update()
        {
            for (int i = 0; i < RequiredValues.Count; i++)
            {
                if (Datas[i] == null)
                {
                    continue;
                }

                if (Datas[i].Value.Equals(RequiredValues[i]))
                {
                    Tasks.ElementAt(i)?.Complete();
                }
            }
        }
    }
}