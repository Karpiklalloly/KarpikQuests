using KarpikQuests.Interfaces;
using KarpikQuests.Statuses;
using System.Text;
using KarpikQuests.Saving;
using KarpikQuests.Extensions;
using System;
using KarpikQuests.Keys;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class Quest : IQuest
    {
        public event Action<string, string>? KeyChanged;
        public event Action<IQuest>? Started;
        public event Action<IQuest, ITaskBundle>? Updated;
        public event Action<IQuest>? Completed;

        public string Key
        {
            get => _key;
            set
            {
#if DEBUG
                if (!value.IsValid()) throw new ArgumentNullException(nameof(value));
#endif

                string temp = _key;
                _key = value;
                KeyChanged?.Invoke(temp, _key);
            }
        }
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        public bool Inited
        {
            get => _inited;
            private set => _inited = value;
        }

        public IReadOnlyTaskBundleCollection TaskBundles
        {
            get => _bundles;
            private set => _bundles = (ITaskBundleCollection)value;
        }

        public IStatus Status
        {
            get => _status;
            private set => _status = value;
        }

        [SerializeThis("Key")]
        private string _key;

        [SerializeThis("Name")]
        private string _name;

        [SerializeThis("Description")]
        private string _description;

        [SerializeThis("Inited")]
        private bool _inited;

        [SerializeThis("Status")]
        private IStatus _status;

        [SerializeThis("Tasks")]
        private ITaskBundleCollection _bundles;

        private bool _disposedValue;

        public void Init()
        {
            Init(KeyGenerator.GenerateKey(), "Quest", "Description", new TaskBundleCollection());
        }

        public void Init(string key, string name, string description, ITaskBundleCollection bundles)
        {
#if DEBUG
            if (!bundles.IsValid()) throw new ArgumentNullException(nameof(bundles));
#endif

            Key = key;
            Name = name;
            Description = description;
            TaskBundles = bundles;
            Status = new UnStarted();

            Inited = true;

            TaskBundles.Updated += OnBundlesUpdated;
            TaskBundles.Completed += OnBundlesCompleted;
        }

        public void Start()
        {
            Status = new Started();
            TaskBundles.Processor.Setup(TaskBundles);
            Started?.Invoke(this);
        }

        public void AddTask(ITask task)
        {
            if (_bundles.Has(task)) return;

            var bundle = new TaskBundle
            {
                task
            };
            _bundles.Add(bundle);
        }

        public void RemoveTask(ITask task)
        {
            if (!_bundles.Has(task)) return;

            ITaskBundle? b = null;
            foreach (var bundle in _bundles)
            {
                if (!bundle.Has(task)) continue;

                bundle.Remove(task);
                b = bundle;
                break;
            }

            if (b?.QuestTasks.Count == 0)
            {
                _bundles.Remove(b);
            }
        }

        public void AddBundle(ITaskBundle bundle)
        {
            if (_bundles.Has(bundle)) return;
            _bundles.Add(bundle);
        }

        public void RemoveBundle(ITaskBundle bundle)
        {
            if (!_bundles.Has(bundle)) return;
            _bundles.Remove(bundle);
        }

        public void Reset()
        {
            Status = new UnStarted();
            foreach (var bundle in TaskBundles)
            {
                bundle.ResetAll();
            }
        }

        public void Clear()
        {
            foreach (var bundle in _bundles)
            {
                bundle.ClearTasks();
                bundle.Clear();
            }
            _bundles.Clear();

            Started = null;
            Updated = null;
            Completed = null;
        }

        public object Clone()
        {
            Quest quest = new Quest
            {
                Key = Key,
                Name = Name,
                Description = Description,
                _bundles = (ITaskBundleCollection)_bundles.Clone(),
                Status = Status,
                Started = (Action<IQuest>?)Started?.Clone(),
                Updated = (Action<IQuest, ITaskBundle>?)Updated?.Clone(),
                Completed = (Action<IQuest>?)Completed?.Clone()
            };

            return quest;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append($"'{Key}': {Name}:\n" +
                $"{Description}\n" +
                $"Status: {Status}\n" +
                $"\tTasks:\n");
            foreach (var task in TaskBundles)
            {
                str.Append($"\t{task}\n");
            }

            return str.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Quest quest))
            {
                return false;
            }

            return Equals(this, quest);
        }

        public bool Equals(IQuest? x, IQuest? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            return x.Key.Equals(y.Key);
        }

        public int GetHashCode([DisallowNull] IQuest obj)
        {
            return obj.GetHashCode();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _bundles.Clear();

                KeyChanged = null;
                Started = null;
                Updated = null;
                Completed = null;

                _inited = false;

                _disposedValue = true;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            TaskBundles.Updated += OnBundlesUpdated;
            TaskBundles.Completed += OnBundlesCompleted;
        }

        private void OnBundlesUpdated(IReadOnlyTaskBundleCollection bundles, ITaskBundle bundle)
        {
            if (Status is UnStarted)
            {
                Start();
            }

            Updated?.Invoke(this, bundle);
        }

        private void OnBundlesCompleted(IReadOnlyTaskBundleCollection bundles)
        {
            Status = new Completed();

            Completed?.Invoke(this);
        }
    }
}