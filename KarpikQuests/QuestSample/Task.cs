﻿using Newtonsoft.Json;
using System;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Task : ITask
    {
        public event Action<ITask>? Started;
        public event Action<ITask>? Completed;
        public event Action<ITask>? Failed;

        [JsonIgnore] public Id Id => _id;
        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public string Description => _description;
        [JsonIgnore] public ITask.TaskStatus Status => _status;
        [JsonIgnore] public bool CanBeCompleted => _canBeCompleted;
        [JsonIgnore] public bool Inited => _inited;

        [JsonProperty("Key")]
        private Id _id;
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
        }

        public void Init()
        {
            Init("Task", "Description");
        }

        public void Init(string name, string description = "")
        {
            _name = name;
            _description = description;

            _status = ITask.TaskStatus.UnStarted;

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
            _status = ITask.TaskStatus.UnStarted;
            Started?.Invoke(this);
        }

        public void Reset()
        {
            _canBeCompleted = false;
            _status = ITask.TaskStatus.UnStarted;
            Completed = null;
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

        public object Clone()
        {
            return new Task
            {
                _id = Id,
                _name = Name,
                _status = Status,
                _canBeCompleted = CanBeCompleted,
                Completed = (Action<ITask>?)Completed?.Clone(),
                Failed = (Action<ITask>?)Failed?.Clone()
            };
        }

        public override bool Equals(object? obj)
        {
            return obj is Task task && Equals(this, task);
        }
        
        public bool Equals(ITask? other)
        {
            return !(other is null) && _id.Equals(other.Id);
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id} {Name} ({Status})";
        }
    }
}