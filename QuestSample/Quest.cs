using KarpikQuests.Interfaces;
using KarpikQuests.Statuses;
using System.Runtime.Serialization;
using System.Text;
using KarpikQuests.QuestCompletionTypes;
using KarpikQuests.TaskProcessorTypes;
using KarpikQuests.Saving;
using KarpikQuests.Extensions;
using System;
using System.Linq;

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
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Key", Order = 1)]
#endif
        [SerializeThis("Key", Order = 1)]
        public string Key { get; private set; }

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name", Order = 2)]
#endif
        [SerializeThis("Name", Order = 2)]
        public string Name { get; private set; }

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description", Order = 3)]
#endif
        [SerializeThis("Description", Order = 3)]
        public string Description { get; private set; }

#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks", Order = 5)]
#endif
        [SerializeThis("Tasks", Order = 5)]
        private ITaskBundleCollection _bundles = new TaskBundleCollection();
        private bool disposedValue;

        public event Action<IQuest>? Started;
        public event Action<IQuest, ITaskBundle>? Updated;
        public event Action<IQuest>? Completed;

#if JSON_NEWTONSOFT
        [JsonIgnore]
#endif
        [DoNotSerializeThis]
        public IReadOnlyTaskBundleCollection TaskBundles => _bundles;

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status", Order = 4)]
#endif
        [SerializeThis("Status", Order = 4)]
        public IStatus Status { get; private set; } = new UnStarted();

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CompletionType", Order = 6)]
#endif
        [SerializeThis("CompletionType", Order = 6)]
        public ICompletionType CompletionType { get; private set; }
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("TaskProcessor", Order = 6)]
#endif
        [SerializeThis("TaskProcessor", Order = 6)]
        public ITaskProcessorType TaskProcessor { get; private set; }

        public Quest() : this(new CompletionAND(), new TaskProcessorDisorderly())
        {

        }

        public Quest(ICompletionType completionType, ITaskProcessorType questTaskProcessor)
        {
            CompletionType = completionType ?? new CompletionAND();
            TaskProcessor = questTaskProcessor ?? new TaskProcessorDisorderly();
        }

        void IQuest.Init(string key, string name, string description)
        {
            Key = key;
            Name = name;
            Description = description;
        }

        void IQuest.SetKey(string key)
        {
            Key = key;
        }

        void IQuest.AddTask(IQuestTask task)
        {
            if (ContainsTask(task)) return;
            var bundle = new TaskBundle
            {
                task
            };
            _bundles.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        void IQuest.RemoveTask(IQuestTask task)
        {
            if (!ContainsTask(task)) return;
            ITaskBundle b = _bundles.First();
            foreach (var bundle in _bundles)
            {
                if (bundle.Contains(task))
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

        void IQuest.OnBundleComplete(ITaskBundle bundle)
        {
            OnBundleComplete(bundle);
        }

        void IQuest.Reset()
        {
            Status = new UnStarted();
            foreach (var bundle in TaskBundles)
            {
                bundle.ResetAll();
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

        public object Clone()
        {
            Quest quest = new Quest
            {
                Key = Key,
                Name = Name,
                Description = Description,
                _bundles = (ITaskBundleCollection)_bundles.Clone(),
                Status = Status,
                Started = (Action<IQuest>)Started?.Clone(),
                Updated = (Action<IQuest, ITaskBundle>)Updated?.Clone(),
                Completed = (Action<IQuest>)Completed?.Clone()
            };

            return quest;
        }

        void IQuest.SetCompletionType(ICompletionType completionType)
        {
           CompletionType = completionType;
        }

        void IQuest.SetTaskProcessorType(ITaskProcessorType processor)
        {
            TaskProcessor = processor;
        }

        public bool Equals(IQuest other)
        {
            return Key.Equals(other.Key);
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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void IQuest.AddBundle(ITaskBundle bundle)
        {
            if (_bundles.Contains(bundle)) return;
            _bundles.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        void IQuest.RemoveBundle(ITaskBundle bundle)
        {
            if (_bundles.Contains(bundle)) return;
            _bundles.Remove(bundle);
            bundle.Completed -= OnBundleComplete;
        }

        private bool ContainsTask(IQuestTask task)
        {
            foreach (var bundle in _bundles)
            {
                foreach (var curTask in bundle)
                {
                    if (curTask.Equals(task))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Start()
        {
            Status = new Started();
            TaskProcessor.Setup(TaskBundles);
            Started?.Invoke(this);
            Started = null;
        }

        private void OnBundleComplete(ITaskBundle bundle)
        {
            if (this.IsNotStarted())
            {
                Start();
            }

            Updated?.Invoke(this, bundle);
            TaskProcessor.OnBundleCompleted(TaskBundles, bundle);

            if (CompletionType.CheckCompletion(TaskBundles))
            {
                Status = new Completed();
                Completed?.Invoke(this);

                Updated = null;
                Completed = null;
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
    }
}