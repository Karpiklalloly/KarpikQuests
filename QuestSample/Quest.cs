using KarpikQuests.Interfaces;
using KarpikQuests.Statuses;
using System.Runtime.Serialization;
using System.Text;
using KarpikQuests.CompletionTypes;
using KarpikQuests.TaskProcessorTypes;
using KarpikQuests.Saving;
using KarpikQuests.Extensions;
using System;
using System.Linq;
using KarpikQuests.Misc;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;

#endif

#if UNITY
using UnityEngine;
#endif

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

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name", Order = 2)]
#endif
        [SerializeThis("Name", Order = 2)]
        #endregion
        public string Name { get; private set; }

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description", Order = 3)]
#endif
        [SerializeThis("Description", Order = 3)]
        #endregion
        public string Description { get; private set; }

        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks", Order = 5)]
#endif
        [SerializeThis("Tasks", Order = 5)]
        #endregion
        private ITaskBundleCollection _bundles;
        private bool disposedValue;

        public event Action<string, string>? KeyChanged;
        public event Action<IQuest>? Started;
        public event Action<IQuest, ITaskBundle>? Updated;
        public event Action<IQuest>? Completed;

        #region noserialize
#if JSON_NEWTONSOFT
        [JsonIgnore]
#endif
        [DoNotSerializeThis]
        #endregion
        public IReadOnlyTaskBundleCollection TaskBundles => _bundles;

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status", Order = 4)]
#endif
        [SerializeThis("Status", Order = 4)]
        #endregion
        public IStatus Status { get; private set; } = new UnStarted();

        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Key", Order = 1)]
#endif
        [SerializeThis("Key", Order = 1)]
        #endregion
        private string _key;

        public void Init(string key, string name, string description, IProcessorType? processor, ICompletionType? completionType)
        {
            if (!key.IsValid()) throw new ArgumentNullException(nameof(key));

            if (!name.IsValid()) throw new ArgumentNullException(nameof(name));

            if (!description.IsValid()) throw new ArgumentNullException(nameof(description));

            Key = key;
            Name = name;
            Description = description;
            _bundles = new TaskBundleCollection(completionType, processor);
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
            if (other is null)
            {
                return false;
            }

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