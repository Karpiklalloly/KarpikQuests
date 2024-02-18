using KarpikQuests.Interfaces;
using KarpikQuests.Statuses;
using System.Runtime.Serialization;
using System.Text;
using KarpikQuests.Saving;
using KarpikQuests.Extensions;
using System;
using System.Linq;
using KarpikQuests.Keys;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class Quest : IQuest
    {
        public string Key
        {
            get => _key;
            set
            {
                if (!value.IsValid()) throw new ArgumentNullException(nameof(value));

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

        [SerializeThis("Tasks", Order = 6)]
        private ITaskBundleCollection _bundles;
        private bool disposedValue;

        public event Action<string, string>? KeyChanged;
        public event Action<IQuest>? Started;
        public event Action<IQuest, ITaskBundle>? Updated;
        public event Action<IQuest>? Completed;

        public IReadOnlyTaskBundleCollection TaskBundles
        {
            get => _bundles;
        }

        [SerializeThis("Status", Order = 5)]
        public IStatus Status { get; private set; } = new UnStarted();

        [SerializeThis("Key", Order = 1)]
        private string _key;
        [SerializeThis("Name", Order = 2)]
        private string _name;
        [SerializeThis("Description", Order = 3)]
        private string _description;
        [SerializeThis("Inited", Order = 4)]
        private bool _inited;

        public void Init()
        {
            Init(KeyGenerator.GenerateKey(""), "Quest", "Description", new TaskBundleCollection());
        }

        public void Init(string key, string name, string description, ITaskBundleCollection bundles)
        {
            if (!bundles.IsValid()) throw new ArgumentNullException(nameof(bundles));

            Key = key;
            Name = name;
            Description = description;
            _bundles = bundles;

            Inited = true;
        }

        public void AddTask(IQuestTask task)
        {
            if (_bundles.Has(task)) return;

            var bundle = new TaskBundle
            {
                task
            };
            _bundles.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        public void RemoveTask(IQuestTask task)
        {
            if (!_bundles.Has(task)) return;

            ITaskBundle b = _bundles.First();
            foreach (var bundle in _bundles)
            {
                if (bundle.Has(task))
                {
                    bundle.Remove(task);
                    b = bundle;
                    break;
                }
            }

            if (b?.QuestTasks.Count == 0)
            {
                _bundles.Remove(b);
            }
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

        public void Reset()
        {
            Status = new UnStarted();
            foreach (var bundle in TaskBundles)
            {
                bundle.ResetAll();
            }
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

        public bool Equals(IQuest? other)
        {
            if (other is null) return false;

            return Key.Equals(other.Key);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void AddBundle(ITaskBundle bundle)
        {
            if (_bundles.Has(bundle)) return;
            _bundles.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        public void RemoveBundle(ITaskBundle bundle)
        {
            if (!_bundles.Has(bundle)) return;
            _bundles.Remove(bundle);
            bundle.Completed -= OnBundleComplete;
        }

        public void Start()
        {
            Status = new Started();
            TaskBundles.Processor.Setup(TaskBundles);
            Started?.Invoke(this);
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

        public bool CheckCompletion()
        {
            return TaskBundles.CheckCompletion();
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disposing();
                }

                FreeResources();
                disposedValue = true;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var bundle in TaskBundles)
            {
                bundle.Completed += OnBundleComplete;
            }
        }

        private void Disposing()
        {
            _bundles.Clear();

            Started = null;
            Updated = null;
            Completed = null;
        }

        private void FreeResources()
        {

        }

        private void OnBundleComplete(ITaskBundle bundle)
        {
            if (this.IsNotStarted())
            {
                Start();
            }

            Updated?.Invoke(this, bundle);

            if (CheckCompletion())
            {
                Status = new Completed();
                Completed?.Invoke(this);

                Updated = null;
                Completed = null;
            }
        }
    }
}