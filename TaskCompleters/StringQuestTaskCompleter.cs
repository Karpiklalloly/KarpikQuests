using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace KarpikQuests.TaskCompleters
{
    [System.Serializable]
    public class StringQuestTaskCompleter : IQuestTaskCompleter<string>
    {
        public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

        public List<DataObserver<string>> Datas { get; private set; } = new List<DataObserver<string>>();

        public List<string> RequiredValues { get; private set; } = new List<string>();

        public void Subscribe(IQuestTask task, ref string observableData, string requiredValue)
        {
            Tasks.Add(task);
            Datas.Add(new DataObserver<string>(ref observableData));
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