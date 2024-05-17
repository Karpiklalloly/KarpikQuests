using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Extensions;
using Karpik.Quests.Factories;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;
using Karpik.Quests.Statuses;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
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
        [SerializeThis("Tasks")]
        private ITaskCollection _tasks = new TaskCollection();
        [JsonProperty("TaskProcessor")]
        [SerializeThis("TaskProcessor")]
        private IProcessorType _processorType;
        [JsonProperty("CompletionType")]
        [SerializeThis("CompletionType")]
        private ICompletionType _completionType;
        [JsonProperty("Status")]
        [SerializeThis("Status")]
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
            foreach (var task in _tasks)
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
            if (!_tasks.Any()) return;

            _tasks.First().Reset();
            Subscribe(_tasks.First());
        }

#region collection

        [JsonIgnore] public int Count => _tasks.Count;
        
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
            
            _tasks.Add(item);
            Subscribe(item);
        }

        public void Clear()
        {
            foreach (var task in _tasks)
            {
                task.Reset();
            }
            _tasks.Clear();
        }

        public bool Contains(ITask item)
        {
            return Tasks.Has(item);
        }

        public void CopyTo(ITask[] array, int arrayIndex)
        {
            _tasks.CopyTo(array, arrayIndex);
        }
        
        public bool Remove(ITask item)
        {
            UnSubscribe(item);
            return _tasks.Remove(item);
        }
        
        public bool Has(ITask task)
        {
            if (task is null) return false;
            return Tasks.Has(task);
        }
        
        public bool Has(Id taskKey)
        {
            if (taskKey.IsEmpty()) return false;
            var task = _tasks.First(x => x.Id.Equals(taskKey));
            return Tasks.Has(task);
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
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
