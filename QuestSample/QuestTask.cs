using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.Saving;
using System;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class QuestTask : IQuestTask
    {
        public string Key
        {
            get => _key;
            private set => _key = value;
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

        public IQuestTask.TaskStatus Status
        {
            get => _status;
            private set => _status = value;
        }
        
        public bool CanBeCompleted
        {
            get => _canBeCompleted;
            set => _canBeCompleted = value;
        }

        public bool Inited
        {
            get => _inited;
            set => _inited = value;
        }

        public event Action<IQuestTask>? Completed;

        [SerializeThis("Key")]
        private string _key;

        [SerializeThis("Name")]
        private string _name;

        [SerializeThis("Description")]
        private string _description;

        [SerializeThis("Status")]
        private IQuestTask.TaskStatus _status;

        [SerializeThis("CanBeCompleted")]
        private bool _canBeCompleted;

        [SerializeThis("Inited")]
        private bool _inited;

        public void Init()
        {
            Init(KeyGenerator.GenerateKey(""), "Task", "Description");
        }

        public void Init(string key, string name, string description)
        {
            Key = key;
            Name = name;
            Description = description;

            _status = IQuestTask.TaskStatus.UnCompleted;

            _inited = true;
        }

        public void Reset(bool canBeCompleted = false)
        {
            CanBeCompleted = canBeCompleted;
            Status = IQuestTask.TaskStatus.UnCompleted;
        }

        public object Clone()
        {
            QuestTask task = new QuestTask
            {
                Key = Key,
                Name = Name,
                Status = Status,
                CanBeCompleted = CanBeCompleted,
                Completed = (Action<IQuestTask>?)Completed?.Clone()
            };

            return task;
        }

        public bool Equals(IQuestTask? other)
        {
            if (other is null) return false;
            if (Key is null) return false;
            return Key.Equals(other.Key);
        }

        public bool TryToComplete()
        {
            if (!CanBeCompleted) return false;

            Status = IQuestTask.TaskStatus.Completed;
            CanBeCompleted = false;
            Completed?.Invoke(this);

            return true;
        }

        public void Clear()
        {
            Reset(false);
            Completed = null;
        }

        public override string ToString()
        {
            return $"{Key} {Name} ({Status})";
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}