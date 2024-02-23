using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.Saving;
using System;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class Task : ITask
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

        public ITask.TaskStatus Status
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

        public event Action<ITask>? Completed;

        [SerializeThis("Key")]
        private string _key;

        [SerializeThis("Name")]
        private string _name;

        [SerializeThis("Description")]
        private string _description;

        [SerializeThis("Status")]
        private ITask.TaskStatus _status;

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

            _status = ITask.TaskStatus.UnCompleted;

            _inited = true;
        }

        public void Reset(bool canBeCompleted = false)
        {
            CanBeCompleted = canBeCompleted;
            Status = ITask.TaskStatus.UnCompleted;
            Completed = null;
        }

        public object Clone()
        {
            Task task = new Task
            {
                Key = Key,
                Name = Name,
                Status = Status,
                CanBeCompleted = CanBeCompleted,
                Completed = (Action<ITask>?)Completed?.Clone()
            };

            return task;
        }

        public bool Equals(ITask? other)
        {
            if (other is null) return false;
            if (Key is null) return false;
            return Key.Equals(other.Key);
        }

        public bool TryToComplete()
        {
            if (!CanBeCompleted) return false;

            Status = ITask.TaskStatus.Completed;
            CanBeCompleted = false;
            Completed?.Invoke(this);

            return true;
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