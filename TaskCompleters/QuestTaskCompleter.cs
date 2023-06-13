using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.TaskCompleters
{
    public class QuestTaskCompleter<T> : IQuestTaskCompleter<T>
    where T : IEquatable<T>
    {
        public IQuestTaskCollection Tasks { get; private set; } = new QuestTaskCollection();

        public List<DataObserver<T>> Datas { get; private set; } = new List<DataObserver<T>>();

        public List<T> RequiredValues { get; private set; } = new List<T>();

        public void Subscribe(IQuestTask task, ref T observableData, T requiredValue)
        {
            Tasks.Add(task);
            Datas.Add(new DataObserver<T>(ref observableData));
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