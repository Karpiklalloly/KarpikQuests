using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Enumerators;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class TaskCollection : ITaskCollection
    {
        [SerializeThis("Tasks", IsReference = true)]
        private List<ITask> _tasks = new List<ITask>();

        #region collection

        [DoNotSerializeThis] public int Count => _tasks.Count;

        [DoNotSerializeThis] public bool IsReadOnly => false;

        public void Add(ITask item)
        {
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
        
        public bool Has(ITask task)
        {
            if (task is null) return false;

            foreach (var task1 in _tasks)
            {
                if (task.Equals(task1)) return true;
            }

            return false;
        }
        
        public bool Has(Id task)
        {
            foreach (var task1 in _tasks)
            {
                if (task.Equals(task1.Id)) return true;
            }

            return false;
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
            return new TaskCollectionEnumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TaskCollectionEnumerator(this);
        }
#endregion

        public override bool Equals(object obj)
        {
            return obj is TaskCollection collection && Equals(collection);
        }

        public bool Equals(IReadOnlyTaskCollection other)
        {
            if (other is null) return false;
            if (Count != other.Count()) return false;
            
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

        public int IndexOf(ITask item)
        {
            return _tasks.IndexOf(item);
        }

        public void Insert(int index, ITask item)
        {
            _tasks.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _tasks.RemoveAt(index);
        }

        public ITask this[int index]
        {
            get => _tasks[index];
            set => _tasks[index] = value;
        }
    }
}