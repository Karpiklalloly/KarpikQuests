using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class TaskCollection : ITaskCollection
    {
        [JsonProperty("Tasks")]
        private readonly List<ITask> _tasks = new List<ITask>();

        #region collection

        [JsonIgnore] public int Count => _tasks.Count;

        [JsonIgnore] public bool IsReadOnly => false;

        public void Add(ITask item)
        {
            if (Has(item)) return;
            _tasks.Add(item);
        }

        public void Clear()
        {
            _tasks.Clear();
        }

        public bool Contains(ITask item)
        {
            return Has(item);
        }

        public void CopyTo(ITask[] array, int arrayIndex)
        {
            _tasks.CopyTo(array, arrayIndex);
        }
        
        public bool Has(ITask? task)
        {
            return !(task is null) && _tasks.Contains(task);
        }

        public bool Remove(ITask item)
        {
            if (!Has(item)) return false;
            var index = _tasks.FindIndex(x => x.Equals(item));
            if (index < 0) return false;
            _tasks.RemoveAt(index);
            return true;
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }
#endregion

        public object Clone()
        {
            var clone = new TaskCollection();
            
            foreach (var item in _tasks)
            {
                clone.Add((ITask)item.Clone());
            }

            return clone;
        }

        public override bool Equals(object? obj)
        {
            return obj is TaskCollection collection && Equals(collection);
        }

        public bool Equals(IReadOnlyTaskCollection? other)
        {
            if (other is null) return false;
            if (Count != other.Count) return false;
            
            for (var i = 0; i < Count; i++)
            {
                if (!this.ElementAt(i).Equals(other.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
        
        public override int GetHashCode()
        {
            return _tasks.GetHashCode();
        }
    }
}