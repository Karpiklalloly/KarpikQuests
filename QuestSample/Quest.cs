using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using KarpikQuests.QuestStatuses;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using KarpikQuests.QuestCompletionTypes;
using KarpikQuests.QuestTaskProcessorTypes;
using KarpikQuests.Saving;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class Quest : QuestBase
    {
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Key", Order = 1)]
#endif
        [SerializeThis("Key", 1)]
        public override string Key { get; protected set; }

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name", Order = 2)]
#endif
        [SerializeThis("Name", 2)]
        public override string Name { get; protected set; }

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description", Order = 3)]
#endif
        [SerializeThis("Description", 3)]
        public override string Description { get; protected set; }

#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks", Order = 5)]
#endif
        [SerializeThis("Tasks", 3)]
        private IQuestTaskCollection _tasks = new QuestTaskCollection();

        public override event Action<IQuest> Started;
        public override event Action<IQuest, IQuestTask> Updated;
        public override event Action<IQuest> Completed;

#if JSON_NEWTONSOFT
        [Newtonsoft.Json.JsonIgnore]
#endif
        [DoNotSerializeThis]
        public override IEnumerable<IQuestTask> Tasks => _tasks;

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status", Order = 4)]
#endif
        [SerializeThis("Status", 4)]
        public override IQuestStatus Status { get; protected set; } = new UnStartedQuest();

        public override IQuestCompletionType CompletionType { get; protected set; } = new QuestCompletionAND();
        public override IQuestTaskProcessorType QuestTaskProcessor { get; protected set; } = new QuestTaskProcessorDisorderly();

        protected override void Init(string key, string name, string description)
        {
            Key = key;
            Name = name;
            Description = description;
        }

        protected override void SetKey(string key)
        {
            Key = key;
        }

        protected override void AddTask(IQuestTask task)
        {
            if (_tasks.Contains(task)) return;
            _tasks.Add(task);
        }

        protected override void RemoveTask(IQuestTask task)
        {
            if (_tasks.Contains(task))
            {
                _tasks.Remove(task);
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append($"'{Key}': {Name}:\n" +
                $"{Description}\n" +
                $"Status: {Status}\n" +
                $"\tTasks:\n");
            foreach (var task in Tasks)
            {
                str.Append($"\t{task}\n");
            }

            return str.ToString();
        }

        protected override void OnTaskComplete(IQuestTask task)
        {
            if (Status is UnStartedQuest)
            {
                Start();
            }

            Updated?.Invoke(this, task);
            QuestTaskProcessor.OnTaskCompleted(Tasks, task);

            if (CompletionType.CheckCompletion(Tasks))
            {
                Status = new CompletedQuest();
                Completed?.Invoke(this);

                Updated = null;
                Completed = null;
            }
        }

        protected override void Start()
        {
            Status = new StartedQuest();
            QuestTaskProcessor.Setup(Tasks);
            Started?.Invoke(this);
            Started = null;
        }

        public override void Reset()
        {
            Status = new UnStartedQuest();
            foreach (var task in Tasks)
            {
                task.Reset();
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            var interf = this as IQuest;
            foreach (var task in Tasks)
            {
                task.Completed += interf.OnTaskComplete;
            }
        }

        protected override void Disposing()
        {
            _tasks.Clear();

            Started = null;
            Updated = null;
            Completed = null;
        }

        protected override void FreeResources()
        {

        }

        public override object Clone()
        {
            Quest quest = new Quest
            {
                Key = Key,
                Name = Name,
                Description = Description,
                _tasks = (IQuestTaskCollection)_tasks.Clone(),
                Status = Status,
            };

            quest.Started = (Action<IQuest>)Started?.Clone();
            quest.Updated = (Action<IQuest, IQuestTask>)Updated?.Clone();
            quest.Completed = (Action<IQuest>)Completed?.Clone();

            return quest;
        }

        protected override void SetCompletionType(IQuestCompletionType completionType)
        {
           CompletionType = completionType;
        }

        protected override void SetTaskProcessorType(IQuestTaskProcessorType processor)
        {
            QuestTaskProcessor = processor;
        }
    }
}