﻿using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Task : ITask
    {
        public event Action<ITask> Started;
        public event Action<ITask> Completed;
        public event Action<ITask> Failed;

        [JsonIgnore] public Id Id => _id;
        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public string Description => _description;
        [JsonIgnore] public ITask.TaskStatus Status => _status;
        [JsonIgnore] public bool CanBeCompleted => _canBeCompleted;
        [JsonIgnore] public bool Inited => _inited;

        [JsonProperty("Key")]
        private readonly Id _id;
        [JsonProperty("Name")]
        private string _name;
        [JsonProperty("Description")]
        private string _description;
        [JsonProperty("Status")]
        private ITask.TaskStatus _status;
        [JsonProperty("CanBeCompleted")]
        private bool _canBeCompleted;
        [JsonProperty("Inited")]
        private bool _inited;

        public Task() : this(Id.NewId())
        {
            
        }

        public Task(Id id)
        {
            _id = id;
            _name = string.Empty;
            _description = string.Empty;
        }

        public void Init()
        {
            Init("Task", "Description");
        }

        public void Init(string name, string description = "")
        {
            _name = string.IsNullOrWhiteSpace(name) ? "Task" : name;
            _description = description is null ? "Description" : description;
            
            Setup();

            _inited = true;
        }

        public void Setup()
        {
            _canBeCompleted = false;
            _status = ITask.TaskStatus.UnStarted;
        }

        public void Start()
        {
            _canBeCompleted = true;
            _status = ITask.TaskStatus.Started;
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
            if (!CanBeCompleted) return false;

            ForceComplete();

            return true;
        }

        public void ForceComplete()
        {
            _canBeCompleted = false;
            _status = ITask.TaskStatus.Completed;
            Completed?.Invoke(this);
        }

        public bool TryFail()
        {
            if (Status == ITask.TaskStatus.Completed || Status == ITask.TaskStatus.Failed) return false;
            
            ForceFail();

            return true;
        }

        public void ForceFail()
        {
            _canBeCompleted = false;
            _status = ITask.TaskStatus.Failed;
            Failed?.Invoke(this);
        }
        
        public override bool Equals(object obj)
        {
            return obj is Task task && Equals(this, task);
        }
        
        public bool Equals(ITask other)
        {
            return !(other is null) && _id.Equals(other.Id);
        }
        
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{_id} {Name} ({Status})";
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            
        }
    }
}