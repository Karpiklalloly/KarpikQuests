using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class QuestTaskCompleter<T> : IQuestTaskCompleter<T>
    where T : IEquatable<T>
    {
        private Dictionary<IQuestTask, T[]> _requiredValues = new Dictionary<IQuestTask, T[]>();

        public void Subscribe(IQuestTask task, params T[] requiredValues)
        {
            Unsubscribe(task);
            _requiredValues.Add(task, requiredValues);
        }

        public bool Unsubscribe(IQuestTask task)
        {
            if (!_requiredValues.ContainsKey(task))
            {
                return false;
            }

            _requiredValues.Remove(task);
            return true;
        }

        public void Update(T value, params T[] values)
        {
            var data = new T[values.Length + 1];
            data[0] = value;
            values.CopyTo(data, 1);

            foreach (var item in _requiredValues)
            {
                if (item.Value.Length != data.Length)
                {
                    continue;
                }

                bool breaked = false;
                for (int i = 0; i < data.Length; i++)
                {
                    if (!data[i].Equals(item.Value[i]))
                    {
                        breaked = true;
                        break;
                    }
                }
                if (!breaked)
                {
                    item.Key.TryToComplete();
                }
            }
        }
    }
}