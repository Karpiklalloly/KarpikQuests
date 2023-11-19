using KarpikQuests.Interfaces;
using KarpikQuests.QuestStatuses;
using System.Runtime.Serialization;
using System.Text;
using KarpikQuests.QuestCompletionTypes;
using KarpikQuests.QuestTaskProcessorTypes;
using KarpikQuests.Saving;
using KarpikQuests.Extensions;
using System;
using System.Threading.Tasks;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

//TODO: Перенести OnTaskCompleted в бандлы, чтобы квест не взаимодействовал напрямую с тасками
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
        [SerializeThis("Tasks", Order = 3)]
        private ITaskBundleCollection _tasks = new TaskBundleCollection();
        private bool disposedValue;

        public event Action<IQuest>? Started;
        public event Action<IQuest, ITaskBundle>? Updated;
        public event Action<IQuest>? Completed;

#if JSON_NEWTONSOFT
        [JsonIgnore]
#endif
        [DoNotSerializeThis]
        public ITaskBundleCollection TaskBundles => _tasks;

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status", Order = 4)]
#endif
        [SerializeThis("Status", Order = 4)]
        public IQuestStatus Status { get; private set; } = new UnStartedQuest();

        public IQuestCompletionType CompletionType { get; private set; } = new QuestCompletionAND();
        public IQuestTaskProcessorType QuestTaskProcessor { get; private set; } = new QuestTaskProcessorDisorderly();

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
            _tasks.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        void IQuest.RemoveTask(IQuestTask task)
        {
            if (!ContainsTask(task)) return;
            ITaskBundle b = _tasks.First();
            foreach (var bundle in _tasks)
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
                _tasks.Remove(b);
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
            Status = new UnStartedQuest();
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
            _tasks.Clear();

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
                _tasks = (ITaskBundleCollection)_tasks.Clone(),
                Status = Status,
                Started = (Action<IQuest>)Started?.Clone(),
                Updated = (Action<IQuest, ITaskBundle>)Updated?.Clone(),
                Completed = (Action<IQuest>)Completed?.Clone()
            };

            return quest;
        }

        void IQuest.SetCompletionType(IQuestCompletionType completionType)
        {
           CompletionType = completionType;
        }

        void IQuest.SetTaskProcessorType(IQuestTaskProcessorType processor)
        {
            QuestTaskProcessor = processor;
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
            if (_tasks.Contains(bundle)) return;
            _tasks.Add(bundle);
            bundle.Completed += OnBundleComplete;
        }

        void IQuest.RemoveBundle(ITaskBundle bundle)
        {
            if (_tasks.Contains(bundle)) return;
            _tasks.Remove(bundle);
            bundle.Completed -= OnBundleComplete;
        }

        private bool ContainsTask(IQuestTask task)
        {
            foreach (var bundle in _tasks)
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
            Status = new StartedQuest();
            QuestTaskProcessor.Setup(TaskBundles);
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
            QuestTaskProcessor.OnBundleCompleted(TaskBundles, bundle);

            if (CompletionType.CheckCompletion(TaskBundles))
            {
                Status = new CompletedQuest();
                Completed?.Invoke(this);

                Updated = null;
                Completed = null;
            }
        }

        public void Clear()
        {
            foreach (var bundle in _tasks)
            {
                bundle.ClearTasks();
                bundle.Clear();
            }
            _tasks.Clear();

            Started = null;
            Updated = null;
            Completed = null;
        }
    }
}