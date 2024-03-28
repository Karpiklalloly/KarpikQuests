using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;
using Karpik.Quests.TaskProcessorTypes;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    public sealed class TaskBundle : ITaskBundle
    {
        public event Action<ITaskBundle>? Updated;
        public event Action<ITaskBundle>? Completed;
        public event Action<ITaskBundle>? Failed;

        [JsonIgnore] public IReadOnlyTaskCollection Tasks => _tasks;
        [JsonIgnore] public ICompletionType CompletionType => _completionType;
        [JsonIgnore] public IProcessorType Processor => _processor;
        [JsonIgnore] public IStatus Status => _status;

        [JsonProperty("Tasks")]
        private IReadOnlyTaskCollection _tasks = new TaskCollection();
        [JsonProperty("TaskProcessor")]
        private IProcessorType _processor;
        [JsonProperty("CompletionType")]
        private ICompletionType _completionType;
        [JsonProperty("Status")]
        private IStatus _status;

        public TaskBundle() : this(CompletionTypesPool.Instance.Pull<And>(), ProcessorTypesPool.Instance.Pull<Orderly>())
        {

        }

        public TaskBundle(ICompletionType completionType, IProcessorType questTaskProcessor)
        {
            SetCompletionType(completionType);
            SetProcessorType(questTaskProcessor);

            _status = new UnStarted();
        }

        public void SetCompletionType(ICompletionType completionType)
        {
#if DEBUG
            if (completionType is null) throw new ArgumentNullException(nameof(completionType));
#endif

            _completionType = completionType;
        }

        public void SetProcessorType(IProcessorType processor)
        {
#if DEBUG
            if (processor is null) throw new ArgumentNullException(nameof(processor));
#endif

            _processor = processor;
            _processor.Setup(this);
        }

        public void ClearTasks()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
            }
            Updated = null;
            Completed = null;
        }

        public void Setup()
        {
            Processor.Setup(this);
        }

        public void Reset()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
                Subscribe(task);
            }

            _status = new UnStarted();
        }

        public void ResetFirst()
        {
            if (!Tasks.Any()) return;

            Tasks.First().Reset();
            Subscribe(Tasks.First());
        }

#region collection

        [JsonIgnore]
        public int Count => Tasks.Count;

        [JsonIgnore]
        public bool IsReadOnly => false;

        public void Add(ITask item)
        {
            Tasks.Add(item);
            if (Tasks.Has(item))
            {
                Subscribe(item);
            }
        }

        public void Clear()
        {
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
            return Tasks.Has(task);
        }
        
        public bool Has(Id taskKey)
        {
            var task = Tasks.First(x => x.Id.Equals(taskKey));
            return Tasks.Has(task);
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Tasks as IEnumerable).GetEnumerator();
        }
        
#endregion

        public object Clone()
        {
            return new TaskBundle
            {
                _tasks = (IReadOnlyTaskCollection)Tasks.Clone(),
                _completionType = CompletionType,
                _processor = Processor,
                Updated = (Action<ITaskBundle>?)Updated?.Clone(),
                Completed = (Action<ITaskBundle>?)Completed?.Clone(),
                Failed = (Action<ITaskBundle>?)Failed?.Clone() 
            };
        }

        public bool Equals(ITaskBundle? other)
        {
            return !(other is null) && _tasks.Equals(other.Tasks);
        }

        public override bool Equals(object? obj)
        {
            return obj is TaskBundle bundle && Equals(bundle);
        }
        
        public override int GetHashCode()
        {
            return Tasks.GetHashCode();
        }

        private void Subscribe(ITask task)
        {
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
