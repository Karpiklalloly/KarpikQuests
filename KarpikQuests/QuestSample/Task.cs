using System;
using System.Runtime.Serialization;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Karpik.Quests.Saving;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Task : ITask
    {
        public event Action<ITask> Started;
        public event Action<ITask> Completed;
        public event Action<ITask> Failed;
        
        [Property]
        public Id Id
        {
            get => _id;
            private set => _id = value;
        }

        [Property]
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        
        [Property]
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        
        [Property]
        public ITask.TaskStatus Status
        {
            get => _status;
            private set => _status = value;
        }
        
        [Property]
        public bool CanBeCompleted
        {
            get => _canBeCompleted;
            private set => _canBeCompleted = value;
        }
        
        [Property]
        public bool Inited
        {
            get => _inited;
            private set => _inited = value;
        }

        [SerializeThis("Key")]
        private Id _id;
        
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


        public Task() : this(Id.NewId())
        {
            
        }

        public Task(Id id)
        {
            Id = id;
            Name = string.Empty;
            Description = string.Empty;
        }

        public void Init()
        {
            Init("Task", "Description");
        }

        public void Init(string name, string description = "")
        {
            Name = !name.IsValid() ? "Task" : name;
            Description = description ?? "Description";
            
            Setup();

            Inited = true;
        }

        public void Setup()
        {
            CanBeCompleted = false;
            Status = ITask.TaskStatus.UnStarted;
        }

        public void Start()
        {
            CanBeCompleted = true;
            Status = ITask.TaskStatus.Started;
            Started?.Invoke(this);
        }

        public void Reset()
        {
            Setup();
            Started = null;
            Completed = null;
            Failed = null;
        }

        public bool TryComplete()
        {
            if (!CanBeCompleted || Status == ITask.TaskStatus.Completed || Status == ITask.TaskStatus.Failed) return false;

            ForceComplete();

            return true;
        }

        public void ForceComplete()
        {
            CanBeCompleted = false;
            Status = ITask.TaskStatus.Completed;
            Completed?.Invoke(this);
        }

        public bool TryFail()
        {
            if (!CanBeCompleted || Status == ITask.TaskStatus.Completed || Status == ITask.TaskStatus.Failed) return false;
            
            ForceFail();

            return true;
        }

        public void ForceFail()
        {
            CanBeCompleted = false;
            Status = ITask.TaskStatus.Failed;
            Failed?.Invoke(this);
        }
        
        public override bool Equals(object obj)
        {
            return obj is Task task && Equals(this, task);
        }
        
        public bool Equals(ITask other)
        {
            return !(other is null) && Id.Equals(other.Id);
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id} {Name} ({Status})";
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            
        }
    }
}