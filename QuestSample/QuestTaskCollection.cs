﻿using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections;
using System.Collections.Generic;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
using System.Linq;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestTaskCollection : IQuestTaskCollection
    {
        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks")]
#endif
        [SerializeThis("Tasks")]
        #endregion
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
            return Has(item);
        }

        public void CopyTo(IQuestTask[] array, int arrayIndex)
        {
            _tasks.CopyTo(array, arrayIndex);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is QuestTaskCollection collection))
            {
                return false;
            }

            return Equals(collection);
        }

        public bool Equals(IReadOnlyQuestTaskCollection? other)
        {
            if (other is null) return false;

            for (int i = 0; i < Count; i++)
            {
                if (!this.ElementAt(i).Equals(other.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator<IQuestTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        public bool Has(IQuestTask task)
        {
            if (task is null) return false;

            foreach (var task1 in _tasks)
            {
                if (task1.Equals(task)) return true;
            }

            return false;
        }

        public bool Remove(IQuestTask item)
        {
            return _tasks.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return _tasks.GetHashCode();
        }
    }
}