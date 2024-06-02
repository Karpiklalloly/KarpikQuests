using System;
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

        [JsonIgnore] public IProcessorType Processor
        {
            get => _processor;
            private set
            {
                _processor = value;
                _processor.Setup(this);
            }
        }
        [JsonIgnore] public IStatus Status => _status;

        [JsonProperty("Tasks")]
        [SerializeThis("Tasks", IsReference = true)]
        private ITaskCollection _tasks = new TaskCollection();
        [JsonProperty("Processor")]
        [SerializeThis("Processor", IsReference = true)]
        private IProcessorType _processor;
        [JsonProperty("CompletionType")]
        [SerializeThis("CompletionType", IsReference = true)]
        private ICompletionType _completionType;
        [JsonProperty("Status")]
        [SerializeThis("Status", IsReference = true)]
        private IStatus _status;

        public TaskBundle() : this(
            CompletionTypesFactory.Instance.Create(),
            ProcessorFactory.Instance.Create())
        {

        }

        public TaskBundle(ICompletionType completionType, IProcessorType processor)
        {
            CompletionType = completionType;
            Processor = processor;

            _status = new UnStarted();
        }
        
        public void Setup()
        {
            _processor.Setup(this);
        }

        public void ResetTasks()
        {
            foreach (var task in _tasks)
            {
                task.Reset();
                Subscribe(task);
            }
        }

        public void Reset()
        {
            ResetTasks();

            Updated = null;
            Completed = null;
            Failed = null;

            _status = new UnStarted();
        }

        public void ResetFirst()
        {
            if (!_tasks.Any()) return;
            
            var first = _tasks.First();
            first.Reset();
            Subscribe(first);
        }

#region collection

        [JsonIgnore] public int Count => _tasks.Count;
        
        public int IndexOf(ITask item)
        {
            return _tasks.IndexOf(item2 => item2.Equals(item));
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
        
        public bool Remove(ITask item)
        {
            UnSubscribe(item);
            return _tasks.Remove(item);
        }
        
        public bool Has(ITask task)
        {
            if (task is null) return false;
            return _tasks.Has(task);
        }
        
        public bool Has(Id taskKey)
        {
            if (taskKey.IsEmpty()) return false;
            var task = _tasks.First(x => x.Id.Equals(taskKey));
            return _tasks.Has(task);
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
            return _tasks.GetHashCode();
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
            var result = _completionType.Check(this);
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
