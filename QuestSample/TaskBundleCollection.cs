using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using KarpikQuests.CompletionTypes;
using KarpikQuests.TaskProcessorTypes;
using System.Linq;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    public class TaskBundleCollection : ITaskBundleCollection
    {
        private List<ITaskBundle> _bundles = new List<ITaskBundle>();

        public ICompletionType CompletionType
        {
            get => _completionType;
            private set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));

                _completionType = value;
            }
        }

        public IProcessorType Processor
        {
            get => _processor;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));

                _processor = value;
                _processor.Setup(_bundles);
            }
        }

        public int Count => _bundles.Count;

        public bool IsReadOnly => false;

        public ITaskBundle this[int index]
        {
            get => _bundles[index];
            set => _bundles[index] = value;
        }

        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CompletionType", Order = 6)]
#endif
        [SerializeThis("CompletionType", Order = 6)]
        #endregion
        private ICompletionType _completionType;

        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("TaskProcessor", Order = 6)]
#endif
        [SerializeThis("TaskProcessor", Order = 6)]
        #endregion
        private IProcessorType _processor;

        public TaskBundleCollection() : this(new AND(), new Disorderly())
        {

        }

        public TaskBundleCollection(ICompletionType? completionType, IProcessorType? processor)
        {
            CompletionType = completionType ?? new AND();
            Processor = processor ?? new Disorderly();
        }

        public void Add(ITaskBundle item)
        {
            if (Has(item)) return;
            _bundles.Add(item);
        }

        public void Clear()
        {
            _bundles.Clear();
        }

        public object Clone()
        {
            TaskBundleCollection another = new TaskBundleCollection();
            foreach (var bundle in _bundles)
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
            foreach (var bundle in _bundles)
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
            _bundles.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ITaskBundle> GetEnumerator()
        {
            return _bundles.GetEnumerator();
        }

        public bool Remove(ITaskBundle item)
        {
            if (!Has(item)) return false;
            var index = IndexOf(item);
            if (index < 0) return false;
            _bundles.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bundles.GetEnumerator();
        }

        public bool Has(ITaskBundle item)
        {
            foreach (var bundle in _bundles)
            {
                if (bundle.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckCompletion()
        {
            return CompletionType.CheckCompletion(this);
        }

        public int IndexOf(ITaskBundle item)
        {
            return _bundles.FindIndex(x => x.Equals(item));
        }

        public void Insert(int index, ITaskBundle item)
        {
            _bundles.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _bundles.RemoveAt(index);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is TaskBundleCollection collection))
            {
                return false;
            }

            return Equals(collection);
        }

        public bool Equals(IReadOnlyTaskBundleCollection? other)
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

        public override int GetHashCode()
        {
            return _bundles.GetHashCode();
        }
    }
}
