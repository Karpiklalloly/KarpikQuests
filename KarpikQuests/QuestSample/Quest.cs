using System;
using System.Text;
using Karpik.Quests.Extensions;
using System.Runtime.Serialization;
using Karpik.Quests.Factories;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Karpik.Quests.Statuses;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Quest : IQuest
    {
        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;
        public event Action<IQuest> Failed;

        [JsonIgnore] public Id Id => _id;
        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public string Description => _description;
        [JsonIgnore] public bool Inited => _inited;
        [JsonIgnore] public IReadOnlyTaskBundleCollection TaskBundles => _bundles;
        [JsonIgnore] public ICompletionType CompletionType => _completionType;
        [JsonIgnore] public IProcessorType Processor => _processor;
        [JsonIgnore] public IStatus Status => _status;
        
        [JsonProperty("Id")]
        private readonly Id _id;
        [JsonProperty("Name")]
        private string _name;
        [JsonProperty("Description")]
        private string _description;
        [JsonProperty("Inited")]
        private bool _inited;

        [JsonProperty("CompletionType")]
        private ICompletionType _completionType;
        [JsonProperty("Processor")]
        private IProcessorType _processor;
        [JsonProperty("Status")]
        private IStatus _status;
        [JsonProperty("Tasks")]
        private ITaskBundleCollection _bundles;

        private bool _disposedValue;

        public Quest() : this(Id.NewId())
        {
            
        }

        public Quest(Id id)
        {
            _id = id;
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
            _name = name;
            _description = description;
            _bundles = bundles;
            _status = new UnStarted();
            _completionType = completionType;
            _processor = processorType;

            _inited = true;
            
            Subscribe(_bundles);
        }

        public void Add(ITask task)
        {
            if (_bundles.Has(task)) return;

            var bundle = new TaskBundle
            {
                task
            };
            Add(bundle);
            Add(bundle, task);
        }

        public void Add(ITaskBundle bundle, ITask task)
        {
            if (_bundles.Has(task)) return;
            bundle.Add(task);
        }

        public void Remove(ITask task)
        {
            if (!_bundles.Has(task)) return;

            ITaskBundle b = null;
            foreach (var bundle in _bundles)
            {
                if (!bundle.Has(task)) continue;

                bundle.Remove(task);
                b = bundle;
                break;
            }

            if (b?.Tasks.Count == 0)
            {
                _bundles.Remove(b);
            }
        }

        public void Add(ITaskBundle bundle)
        {
            if (_bundles.Has(bundle)) return;
            _bundles.Add(bundle);
            Subscribe(bundle);
        }

        public void Remove(ITaskBundle bundle)
        {
            if (!_bundles.Has(bundle)) return;
            _bundles.Remove(bundle);
            UnSubscribe(bundle);
        }

        public void SetStatus(IStatus status, Action<IQuest, IStatus> moreStatusesSetting)
        {
            _status = status ?? throw new NullReferenceException();
            switch (_status)
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
                default:
                    moreStatusesSetting?.Invoke(this, status);
                    break;
            }
            
            Updated?.Invoke(this, null);
        }

        public void Reset()
        {
            foreach (var bundle in _bundles)
            {
                bundle.Reset();
                Subscribe(bundle);
            }

            Started = null;
            Updated = null;
            Completed = null;
            Failed = null;
            
            SetStatus(new UnStarted(), null);
        }

        public void Clear()
        {
            foreach (var bundle in _bundles)
            {
                bundle.Clear();
                bundle.Reset();
            }
            _bundles.Clear();

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
            return _id.GetHashCode();
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
                
                _inited = false;

                _disposedValue = true;
            }

            Started = null;
            Updated = null;
            Completed = null;
            Failed = null;

            _bundles = null;
            _processor = null;
            _completionType = null;
            _status = null;
            _inited = false;
            _name = null;
            _description = null;
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
            Updated?.Invoke(this, bundle);
        }

        private void OnBundleCompleted(ITaskBundle bundle)
        {
            var result = _completionType.Check(TaskBundles);
            if (result.IsCompleted())
            {
                this.Complete();
            }
        }

        private void OnBundleFailed(ITaskBundle bundle)
        {
            var result = _completionType.Check(TaskBundles);
            if (result.IsFailed())
            {
                this.Fail();
            }
        }
    }
}