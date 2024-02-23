using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using KarpikQuests.CompletionTypes;
using KarpikQuests.TaskProcessorTypes;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace KarpikQuests.QuestSample
{
    public sealed class TaskBundleCollection : ITaskBundleCollection
    {
        private readonly List<ITaskBundle> _bundles = new List<ITaskBundle>();

        public ICompletionType CompletionType
        {
            get => _completionType;
            private set
            {
#if DEBUG
                if (value is null) throw new ArgumentNullException(nameof(value));
#endif

                _completionType = value;
            }
        }

        public IProcessorType Processor
        {
            get => _processor;
            private set
            {
#if DEBUG
                if (value is null) throw new ArgumentNullException(nameof(value));
#endif

                _processor = value;
                _processor.Setup(_bundles);
            }
        }

        public int Count
        {
            get => _bundles.Count;
        }

        public bool IsReadOnly
        {
            get => false;
        }

        public ITaskBundle this[int index]
        {
            get => _bundles[index];
            set => _bundles[index] = value;
        }

        [SerializeThis("CompletionType")]
        private ICompletionType _completionType;

        [SerializeThis("TaskProcessor")]
        private IProcessorType _processor;

        public TaskBundleCollection() : this(new AND(), new Disorderly())
        {

        }

        public TaskBundleCollection(ICompletionType completionType, IProcessorType processor)
        {
            SetCompletionType(completionType);
            SetProcessorType(processor);
        }

        public void SetCompletionType(ICompletionType completionType)
        {
            CompletionType = completionType;
        }

        public void SetProcessorType(IProcessorType processor)
        {
            Processor = processor;
        }

#region list
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

        public bool Has(ITask task)
        {
            return _bundles.Exists((bundle) =>  bundle.Has(task));
        }

        public bool Has(ITaskBundle bundle)
        {
            for (int i = 0; i < _bundles.Count; i++)
            {
                ITaskBundle? anotherTaskBundle = _bundles[i];
                if (anotherTaskBundle.Equals(bundle))
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
#endregion

        public bool IsCompleted()
        {
            return CompletionType.CheckCompletion(this);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is TaskBundleCollection collection))
            {
                return false;
            }

            return Equals(this, collection);
        }

        public bool Equals(IReadOnlyTaskBundleCollection? x, IReadOnlyTaskBundleCollection? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            for (int i = 0; i < Count; i++)
            {
                if (!x[i].Equals(y[i]))
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

        public int GetHashCode([DisallowNull] IReadOnlyTaskBundleCollection obj)
        {
            return obj.GetHashCode();
        }
    }
}
