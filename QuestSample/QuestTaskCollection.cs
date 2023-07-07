using KarpikQuests.Interfaces;
using System.Collections;
using System.Collections.Generic;

#if JSON
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestTaskCollection : IQuestTaskCollection
    {
#if UNITY
[SerializeField]
#endif
#if JSON
        [JsonProperty("Tasks")]
#endif
        private readonly List<IQuestTask> _tasks = new List<IQuestTask>();

        public int Count => _tasks.Count;

        public bool IsReadOnly => false;

        public void Add(IQuestTask item)
        {
            _tasks.Add(item);
        }

        public void Clear()
        {
            _tasks.Clear();
        }

        public object Clone()
        {
            QuestTaskCollection clone = new QuestTaskCollection();
            foreach (IQuestTask item in _tasks)
            {
                clone.Add((IQuestTask)item.Clone());
            }

            return clone;
        }

        public bool Contains(IQuestTask item)
        {
            if (item == null) return false;
            return _tasks.Contains(item);
        }

        public void CopyTo(IQuestTask[] array, int arrayIndex)
        {
            _tasks.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IQuestTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        public bool Remove(IQuestTask item)
        {
            return _tasks.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }
    }
}