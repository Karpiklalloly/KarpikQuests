using KarpikQuests.Interfaces;
using System.Collections;
using System.Collections.Generic;

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

        public bool Contains(IQuestTask item)
        {
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