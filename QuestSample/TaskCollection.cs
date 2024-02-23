using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class TaskCollection : ITaskCollection
    {
        [SerializeThis("Tasks")]
        private readonly List<ITask> _tasks = new List<ITask>();

        public int Count => _tasks.Count;

        public bool IsReadOnly => false;

#region list

        public ITask this[int index]
        {
            get => _tasks[index];
            set => _tasks[index] = value;
        }

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

        public IEnumerator<ITask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        public bool Has(ITask task)
        {
            if (task is null) return false;

            foreach (var task1 in _tasks)
            {
                if (task1.Equals(task)) return true;
            }

            return false;
        }

        public bool Remove(ITask item)
        {
            if (!Has(item)) return false;
            var index = IndexOf(item);
            if (index < 0) return false;
            _tasks.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return _tasks.GetHashCode();
        }

        public int IndexOf(ITask item)
        {
            return _tasks.FindIndex(x => x.Equals(item));
        }

        public void Insert(int index, ITask item)
        {
            _tasks.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _tasks.RemoveAt(index);
        }
#endregion

        public object Clone()
        {
            TaskCollection clone = new TaskCollection();
            foreach (ITask item in _tasks)
            {
                clone.Add((ITask)item.Clone());
            }

            return clone;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is TaskCollection collection)) return false;

            return Equals(collection);
        }

        public bool Equals(IReadOnlyTaskCollection? other)
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
    }
}