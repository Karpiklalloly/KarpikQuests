using KarpikQuests.Interfaces;
using KarpikQuests.QuestStatuses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
        [JsonProperty("Key", Order = 1)]
        public override string Key { get; protected set; }

#if UNITY
        [field: SerializeField]
#endif
        [JsonProperty("Name", Order = 2)]
        public override string Name { get; protected set; }

#if UNITY
        [field: SerializeField]
#endif
        [JsonProperty("Description", Order = 3)]
        public override string Description { get; protected set; }

#if UNITY
        [SerializeField]
#endif
        [JsonProperty("Tasks", Order = 5)]
        private readonly IQuestTaskCollection _tasks = new QuestTaskCollection();

        public override event Action<IQuest> Started;
        public override event Action<IQuest, IQuestTask> Updated;
        public override event Action<IQuest> Completed;

        [JsonIgnore]
        public override IEnumerable<IQuestTask> Tasks => _tasks;

#if UNITY
        [field: SerializeField]
#endif
        [JsonProperty("Status", Order = 4)]
        public override IQuestStatus Status { get; protected set; } = new UnStartedQuest();

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
            _tasks.Add(task);
        }

        public override bool Equals(IQuest other)
        {
            if (other == null)
            {
                return false;
            }
            return Key.Equals(other.Key);
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
                (this as IQuest).Start();
            }

            Updated?.Invoke(this, task);

            bool allTasksCompleted = Tasks.Select(x => x.Status == IQuestTask.TaskStatus.Completed).Contains(true);
            if (allTasksCompleted)
            {
                Status = new CompletedQuest();
                Completed?.Invoke(this);
            }
        }

        protected override void Start()
        {
            Status = new StartedQuest();
            foreach (var task in _tasks)
            {
                task.ForceCanBeCompleted();
            }
            Started?.Invoke(this);
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
    }
}