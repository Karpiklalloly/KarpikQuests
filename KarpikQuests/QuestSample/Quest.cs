using System;
using System.Text;
using Karpik.Quests.Extensions;
using System.Runtime.Serialization;
using Karpik.Quests.Factories;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Karpik.Quests.Saving;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Quest : IQuest
    {
        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;
        public event Action<IQuest> Failed;
        
        [Property]
        public Id Id
        {
            get => _id;
            private set => _id = value;
        }
        
        [Property]
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        
        [Property]
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        
        [Property]
        public bool Inited
        {
            get => _inited;
            private set => _inited = value;
        }
        
        [Property]
        public IReadOnlyTaskBundleCollection TaskBundles
        {
            get => _taskBundles;
            private set => _taskBundles = (ITaskBundleCollection)value;
        }
        
        [Property]
        public ICompletionType CompletionType
        {
            get => _completionType;
            private set => _completionType = value;
        }
        
        [Property]
        public IProcessorType Processor
        {
            get => _processor;
            private set => _processor = value;
        }
        
        [Property]
        public IStatus Status
        {
            get => _status;
            private set => _status = value;
        }
        
        [SerializeThis("Id")]
        private Id _id;
        [SerializeThis("Name")]
        private string _name;
        [SerializeThis("Description")]
        private string _description;
        [SerializeThis("Inited")]
        private bool _inited;
        [SerializeThis("Bundles", IsReference = true)]
        private ITaskBundleCollection _taskBundles;
        [SerializeThis("CompletionType", IsReference = true)]
        private ICompletionType _completionType;
        [SerializeThis("Processor", IsReference = true)]
        private IProcessorType _processor;
        [SerializeThis("Status", IsReference = true)]
        private IStatus _status;
        private bool _disposedValue;

        public Quest() : this(Id.NewId())
        {
            
        }

        public Quest(Id id)
        {
            Id = id;
        }

        public void Init()
        {
            Init("Quest", "Description",
                TaskBundleCollectionFactory.Instance.Create(),
                CompletionTypesFactory.Instance.Create(),
                ProcessorFactory.Instance.Create());
        }

        public void Init(string name, string description, ITaskBundleCollection bundles,
            ICompletionType completionType, IProcessorType processorType)
        {
            Name = name;
            Description = description;
            TaskBundles = bundles;
            Status = new UnStarted();
            CompletionType = completionType;
            Processor = processorType;

            Inited = true;
            
            Subscribe(TaskBundles);
        }

        public void Add(ITask task)
        {
            if (TaskBundles.Has(task)) return;

            var bundle = new TaskBundle();
            bundle.Add(task);
            Add(bundle);
        }
        
        public void Add(ITaskBundle bundle)
        {
            if (TaskBundles.Has(bundle)) return;
            TaskBundles.Add(bundle);
            Subscribe(bundle);
        }

        public void Remove(ITask task)
        {
            if (!TaskBundles.Has(task)) return;

            ITaskBundle b = null;
            foreach (var bundle in TaskBundles)
            {
                if (!bundle.Has(task)) continue;

                bundle.Remove(task);
                b = bundle;
                break;
            }

            if (b?.Count != 0) return;
            Remove(b);
        }

        public void Remove(ITaskBundle bundle)
        {
            if (!TaskBundles.Has(bundle)) return;
            TaskBundles.Remove(bundle);
            UnSubscribe(bundle);
        }

        public void SetStatus(IStatus status)
        {
            Status = status ?? throw new NullReferenceException();
            switch (Status)
            {
                case Statuses.UnStarted:
                    break;
                case Statuses.Started:
                    Started?.Invoke(this);
                    break;
                case Statuses.Completed:
                    Completed?.Invoke(this);
                    break;
                case Statuses.Failed:
                    Failed?.Invoke(this);
                    break;
            }
            
            Updated?.Invoke(this, null);
        }

        public void Reset()
        {
            foreach (var bundle in TaskBundles)
            {
                bundle.Reset();
                Subscribe(bundle);
            }

            Started = null;
            Updated = null;
            Completed = null;
            Failed = null;
            
            SetStatus(new UnStarted());
        }

        public void Clear()
        {
            foreach (var bundle in TaskBundles)
            {
                bundle.Clear();
                bundle.Reset();
            }
            TaskBundles.Clear();

            Started = null;
            Updated = null;
            Completed = null;
            Failed = null;
        }

        public bool Has(ITaskBundle bundle)
        {
            if (bundle is null) return false;
            return TaskBundles.Has(bundle);
        }

        public bool Has(ITask task)
        {
            if (task is null) return false;
            return TaskBundles.Count > 0 && TaskBundles.Has(task);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append($"'{Id}': {Name}:\n" +
                $"{Description}\n" +
                $"Status: {Status}\n" +
                $"\tTasks:\n");
            foreach (var task in TaskBundles)
            {
                str.Append($"\t{task}\n");
            }

            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Quest quest && Equals(quest);
        }

        public bool Equals(IQuest other)
        {
            if (other is null) return false;

            return Id.Equals(other.Id);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Clear();
                
                Inited = false;

                _disposedValue = true;
            }

            Started = null;
            Updated = null;
            Completed = null;
            Failed = null;

            TaskBundles = null;
            Processor = null;
            CompletionType = null;
            Status = null;
            Inited = false;
            Name = null;
            Description = null;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Subscribe(TaskBundles);
        }

        private void Subscribe(IReadOnlyTaskBundleCollection collection)
        {
            foreach (var bundle in collection)
            {
                Subscribe(bundle);
            }
        }
        
        private void UnSubscribe(IReadOnlyTaskBundleCollection collection)
        {
            foreach (var bundle in collection)
            {
                UnSubscribe(bundle);
            }
        }

        private void Subscribe(ITaskBundle bundle)
        {
            bundle.Updated   += OnBundleUpdated;
            bundle.Completed += OnBundleCompleted;
            bundle.Failed    += OnBundleFailed;
        }

        private void UnSubscribe(ITaskBundle bundle)
        {
            bundle.Updated   -= OnBundleUpdated;
            bundle.Completed -= OnBundleCompleted;
            bundle.Failed    -= OnBundleFailed;
        }

        private void OnBundleUpdated(ITaskBundle bundle)
        {
            var result = CompletionType.Check(TaskBundles);
            if (this.IsUnStarted())
            {
                this.Start();
            }
            
            if (result.IsCompleted())
            {
                this.Complete();
            }
            else if (result.IsFailed())
            {
                this.Fail();
            }
            
            Updated?.Invoke(this, bundle);
        }

        private void OnBundleCompleted(ITaskBundle bundle)
        {
            
        }

        private void OnBundleFailed(ITaskBundle bundle)
        {
            
        }
    }
}