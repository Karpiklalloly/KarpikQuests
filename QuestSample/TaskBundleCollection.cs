using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using KarpikQuests.CompletionTypes;
using KarpikQuests.TaskProcessorTypes;

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
        public ICollection<ITaskBundle> Bundles { get; private set; } = new List<ITaskBundle>();

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
                _processor.Setup(Bundles);
            }
        }

        public int Count => Bundles.Count;

        public bool IsReadOnly => false;

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

        public bool CheckCompletion()
        {
            return CompletionType.CheckCompletion(this);
        }
    }
}
