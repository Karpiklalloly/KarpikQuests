﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Enumerators;
using Karpik.Quests.Extensions;
using Karpik.Quests.Factories;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    public sealed class TaskBundle : ITaskBundle
    {
        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;
        public event Action<ITaskBundle> Failed;

        [JsonIgnore] public IReadOnlyTaskCollection Tasks => _tasks;

        [JsonIgnore] public ICompletionType CompletionType
        {
            get => _completionType;
            private set => _completionType = value;
        }

        [JsonIgnore] public IProcessorType ProcessorType
        {
            get => _processorType;
            private set
            {
                _processorType = value;
                _processorType.Setup(this);
            }
        }
        [JsonIgnore] public IStatus Status => _status;

        [JsonProperty("Tasks")]
        private IReadOnlyTaskCollection _tasks = new TaskCollection();
        [JsonProperty("TaskProcessor")]
        private IProcessorType _processorType;
        [JsonProperty("CompletionType")]
        private ICompletionType _completionType;
        [JsonProperty("Status")]
        private IStatus _status;

        public TaskBundle() : this(
            CompletionTypesFactory.Instance.Create(),
            ProcessorFactory.Instance.Create())
        {

        }

        public TaskBundle(ICompletionType completionType, IProcessorType processorType)
        {
            CompletionType = completionType;
            ProcessorType = processorType;

            _status = new UnStarted();
        }

        public void ClearTasks()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
                Subscribe(task);
            }
        }

        public void Setup()
        {
            ProcessorType.Setup(this);
        }

        public void Reset()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
                Subscribe(task);
            }

            Updated = null;
            Completed = null;
            Failed = null;

            _status = new UnStarted();
        }

        public void ResetFirst()
        {
            if (!Tasks.Any()) return;

            Tasks.First().Reset();
            Subscribe(Tasks.First());
        }

#region collection

        [JsonIgnore] public int Count => Tasks.Count;

        [JsonIgnore] public bool IsReadOnly => false;
        
        public int IndexOf(ITask item)
        {
            return _tasks.IndexOf(item);
        }

        public void Insert(int index, ITask item)
        {
            _tasks.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _tasks.RemoveAt(index);
        }

        public ITask this[int index]
        {
            get => _tasks[index];
            set => _tasks[index] = value;
        }

        public void Add(ITask item)
        {
            if (Has(item)) return;
            
            Tasks.Add(item);
            Subscribe(item);
        }

        public void Clear()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
            }
            Tasks.Clear();
        }

        public bool Contains(ITask item)
        {
            return Tasks.Has(item);
        }

        public void CopyTo(ITask[] array, int arrayIndex)
        {
            Tasks.CopyTo(array, arrayIndex);
        }
        
        public bool Remove(ITask item)
        {
            UnSubscribe(item);
            return Tasks.Remove(item);
        }
        
        public bool Has(ITask task)
        {
            if (task is null) return false;
            return Tasks.Has(task);
        }
        
        public bool Has(Id taskKey)
        {
            if (taskKey.IsEmpty()) return false;
            var task = Tasks.First(x => x.Id.Equals(taskKey));
            return Tasks.Has(task);
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return new TaskBundleEnumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TaskBundleEnumerator(this);
        }
        
#endregion

        public bool Equals(ITaskBundle other)
        {
            return !(other is null) && _tasks.Equals(other.Tasks);
        }

        public override bool Equals(object obj)
        {
            return obj is TaskBundle bundle && Equals(bundle);
        }
        
        public override int GetHashCode()
        {
            return Tasks.GetHashCode();
        }
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var task in _tasks)
            {
                Subscribe(task);
            }
        }

        private void Subscribe(ITask task)
        {
            task.Started += OnTaskUpdated;
            task.Completed += OnTaskUpdated;
            task.Failed    += OnTaskUpdated;
        }
        
        private void UnSubscribe(ITask task)
        {
            task.Completed -= OnTaskUpdated;
            task.Failed    -= OnTaskUpdated;
        }
        
        private void OnTaskUpdated(ITask task)
        {
            var result = CompletionType.Check(this);
            if (_status.IsUnStarted())
            {
                _status = new Started();
            }
            
            if (result.IsCompleted())
            {
                _status = new Completed();
                Completed?.Invoke(this);
            }
            else if (result.IsFailed())
            {
                _status = new Failed();
                Failed?.Invoke(this);
            }
            
            Updated?.Invoke(this);
        }
    }
}
