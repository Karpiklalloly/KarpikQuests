using KarpikQuests.Interfaces;
using KarpikQuests.QuestStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class Quest : IQuest
    {
        private readonly IQuestTaskCollection _tasks = new QuestTaskCollection();

        public string Key { get; private set; }

        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public event Action<IQuest> Started;
        public event Action<IQuest, IQuestTask> Updated;
        public event Action<IQuest> Completed;

        public IEnumerable<IQuestTask> Tasks => _tasks;

        public IQuestStatus Status { get; private set; } = new UnStartedQuest();

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
            _tasks.Add(task);
        }

        public bool Equals(IQuest other)
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
            str.Append($"{Key}: {Name}:\n" +
                $"{Description}\n" +
                $"\tTasks:\n");
            foreach (var task in Tasks)
            {
                str.Append($"\t{task}\n");
            }

            return str.ToString();
        }

        void IQuest.OnTaskComplete(IQuestTask task)
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

        void IQuest.Start()
        {
            Status = new StartedQuest();
            foreach (var task in _tasks)
            {
                task.ForceBeCompleted();
            }
        }
    }
}