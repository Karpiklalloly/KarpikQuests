using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using KarpikQuests.CompletionTypes;
using KarpikQuests.TaskProcessorTypes;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KarpikQuests.QuestSample
{
    public sealed class TaskBundleCollection : ITaskBundleCollection
    {
        public event Action<IReadOnlyTaskBundleCollection, ITaskBundle>? Updated;
        public event Action<IReadOnlyTaskBundleCollection>? Completed;

        [property: JsonIgnore]
        [DoNotSerializeThis]
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

        [property: JsonIgnore]
        [DoNotSerializeThis]
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

        [SerializeThis(Name = "CompletionType")]
        [JsonProperty("CompletionType")]
        private ICompletionType _completionType;

        [SerializeThis(Name = "TaskProcessor")]
        [JsonProperty("TaskProcessor")]
        private IProcessorType _processor;

        [SerializeThis(Name = "Bundles")]
        [JsonProperty("Bundles")]
        private readonly List<ITaskBundle> _bundles = new List<ITaskBundle>();

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
        [property: JsonIgnore]
        [DoNotSerializeThis]
        public int Count
        {
            get => _bundles.Count;
        }

        [property: JsonIgnore]
        [DoNotSerializeThis]
        public bool IsReadOnly
        {
            get => false;
        }

        public ITaskBundle this[int index]
        {
            get => _bundles[index];
            set => _bundles[index] = value;
        }

        public void Add(ITaskBundle item)
        {
            if (Has(item)) return;
            
            _bundles.Add(item);
            item.Updated += OnBundleUpdated;
            item.Completed += OnBundleComplete;
        }

        public void Clear()
        {
            foreach (var bundle in _bundles)
            {
                bundle.Completed -= OnBundleComplete;
                bundle.Updated -= OnBundleUpdated;
            }

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
            item.Completed -= OnBundleComplete;
            item.Updated -= OnBundleUpdated;
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

        public void Setup()
        {
            Processor.Setup(this);
        }

        public void StartFirst()
        {
            if (!_bundles.Any()) return;

            _bundles[0].StartFirst();
        }

        public void ResetAll()
        {
            foreach (var bundle in _bundles)
            {
                bundle.ResetAll();
            }
        }

        public void ResetFirst()
        {
            if (!_bundles.Any()) return;

            _bundles[0].ResetAll();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext constext)
        {
            foreach (var bundle in _bundles)
            {
                bundle.Updated += OnBundleUpdated;
                bundle.Completed += OnBundleComplete;
            }
        }

        private void OnBundleUpdated(ITaskBundle bundle)
        {
            Updated?.Invoke(this, bundle);
        }

        private void OnBundleComplete(ITaskBundle bundle)
        {
            Completed?.Invoke(this);
        }
    }
}
