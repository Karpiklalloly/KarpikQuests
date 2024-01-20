using KarpikQuests.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestSample
{
    public class TaskBundleCollection : ITaskBundleCollection
    {
        public ICollection<ITaskBundle> Bundles { get; private set; } = new List<ITaskBundle>();

        public int Count => Bundles.Count;

        public bool IsReadOnly => false;

        public void Add(ITaskBundle item)
        {
            Bundles.Add(item);
        }

        public void Clear()
        {
            Bundles.Clear();
        }

        public object Clone()
        {
            TaskBundleCollection another = new TaskBundleCollection();
            foreach (var bundle in Bundles)
            {
                another.Add((ITaskBundle)bundle.Clone());
            }
            return another;
        }

        public bool Contains(ITaskBundle item)
        {
            return Has(item);
        }

        public bool Has(IQuestTask task)
        {
            foreach (var bundle in Bundles)
            {
                if (bundle.Has(task))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(ITaskBundle[] array, int arrayIndex)
        {
            Bundles.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ITaskBundle> GetEnumerator()
        {
            return Bundles.GetEnumerator();
        }

        public bool Remove(ITaskBundle item)
        {
            return Bundles.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Bundles.GetEnumerator();
        }

        public bool Has(ITaskBundle item)
        {
            foreach (var bundle in Bundles)
            {
                if (bundle.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
