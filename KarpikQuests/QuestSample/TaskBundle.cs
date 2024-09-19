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

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public sealed class TaskBundle : ITaskBundle
    {
        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;
        public event Action<ITaskBundle> Failed;
        
        [Property]
        public IReadOnlyTaskCollection Tasks
        {
            get => _tasks;
            private set => _tasks = (ITaskCollection)value;
        }
        
        [Property]
        public ICompletionType CompletionType
        {
            get => _completionType;
            private set => _completionType = value;
        }
        
        [Property]
        public IProcessorType Processor
        {
            get => _processor;
            private set => _processor = value;
        }
        
        [Property]
        public IStatus Status
        {
            get => _status;
            private set => _status = value;
        }

        [SerializeThis("Tasks", IsReference = true)]
        private ITaskCollection _tasks = new TaskCollection();
        [SerializeThis("CompletionType", IsReference = true)]
        private ICompletionType _completionType;

        [SerializeThis("Processor", IsReference = true)]
        private IProcessorType _processor;

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
            Processor.Setup(this);

            Status = new UnStarted();
        }
        
        public void Setup()
        {
            Processor.Setup(this);
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

            Status = new UnStarted();
        }

        public void ResetFirst()
        {
            if (!_tasks.Any()) return;
            
            var first = _tasks.First();
            first.Reset();
            Subscribe(first);
        }

#region collection

        [DoNotSerializeThis] public int Count => _tasks.Count;
        
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
            task.Started -= OnTaskUpdated;
            task.Completed -= OnTaskUpdated;
            task.Failed    -= OnTaskUpdated;
        }
        
        private void OnTaskUpdated(ITask task)
        {
            var result = CompletionType.Check(this);
            if (Status.IsUnStarted())
            {
                Status = new Started();
            }
            
            if (result.IsCompleted())
            {
                Status = new Completed();
                Completed?.Invoke(this);
            }
            else if (result.IsFailed())
            {
                Status = new Failed();
                Failed?.Invoke(this);
            }
            
            Updated?.Invoke(this);
        }
    }
}
