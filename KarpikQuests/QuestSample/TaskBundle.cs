using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.TaskProcessorTypes;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    public sealed class TaskBundle : ITaskBundle
    {
        public event Action<ITaskBundle>? Updated;
        public event Action<ITaskBundle>? Completed;

        [JsonProperty("Tasks")]
        public IReadOnlyTaskCollection QuestTasks { get; private set; } = new TaskCollection();

        [JsonIgnore]
        public ICompletionType CompletionType
        {
            get => _completionType;
            private set
            {
#if DEBUG
                if (value is null) throw new ArgumentNullException(nameof(value));
#endif

                _completionType = value;
            }
        }

        [JsonIgnore]
        public IProcessorType Processor
        {
            get => _processor;
            private set
            {
#if DEBUG
                if (value is null) throw new ArgumentNullException(nameof(value));
#endif

                _processor = value;
                _processor.Setup(this);
            }
        }

        [JsonProperty("IsCompleted")]
        public bool IsCompleted { get; private set; }

        [JsonProperty("CompletionType")]
        private ICompletionType _completionType;

        [JsonProperty("TaskProcessor")]
        private IProcessorType _processor;

        public TaskBundle() : this(And.Instance, ProcessorTypesPool.Instance.Pull<Orderly>())
        {

        }

        public TaskBundle(ICompletionType completionType, IProcessorType questTaskProcessor)
        {
            SetCompletionType(completionType);
            SetProcessorType(questTaskProcessor);
        }

        public void SetCompletionType(ICompletionType completionType)
        {
            CompletionType = completionType;
        }

        public void SetProcessorType(IProcessorType processor)
        {
            Processor = processor;
        }

        public void ClearTasks()
        {
            foreach (var task in QuestTasks)
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

        public void StartFirst()
        {
            QuestTasks.FirstOrDefault()?.Start();
        }

        public void ResetAll()
        {
            foreach (var task in QuestTasks)
            {
                task.Reset();
                task.Completed += OnTaskCompleted;
            }
        }

        public void ResetFirst()
        {
            if (!QuestTasks.Any()) return;

            QuestTasks[0].Reset();
            QuestTasks[0].Completed += OnTaskCompleted;
        }

#region list

        [JsonIgnore]
        public int Count => QuestTasks.Count;

        [JsonIgnore]
        public bool IsReadOnly => false;

        public void Add(ITask item)
        {
            QuestTasks.Add(item);
            item.Completed += OnTaskCompleted;
        }

        public void Clear()
        {
            QuestTasks.Clear();
        }

        public bool Contains(ITask item)
        {
            return QuestTasks.Has(item);
        }

        public void CopyTo(ITask[] array, int arrayIndex)
        {
            QuestTasks.CopyTo(array, arrayIndex);
        }
        
        public IEnumerator<ITask> GetEnumerator()
        {
            return QuestTasks.GetEnumerator();
        }

        public bool Remove(ITask item)
        {
            item.Completed -= OnTaskCompleted;
            return QuestTasks.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (QuestTasks as IEnumerable).GetEnumerator();
        }

        public bool Has(ITask task)
        {
            return QuestTasks.Has(task);
        }

        public bool Has(string taskKey)
        {
            var task = QuestTasks.First(x => x.Key.Equals(taskKey));
            return QuestTasks.Has(task);
        }
#endregion

        public override bool Equals(object? obj)
        {
            if (!(obj is TaskBundle bundle))
            {
                return false;
            }

            return Equals(this, bundle);
        }

        public bool Equals(ITaskBundle? x, ITaskBundle? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            if (x.QuestTasks.GetType() != y.QuestTasks.GetType())
            {
                return false;
            }

            if (x.QuestTasks.Count != y.Count)
            {
                return false;
            }

            return x.QuestTasks.Equals(y.QuestTasks);
        }

        public int GetHashCode(ITaskBundle obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return QuestTasks.GetHashCode();
        }

        public object Clone()
        {
            TaskBundle clone = new TaskBundle
            {
                QuestTasks = (IReadOnlyTaskCollection)QuestTasks.Clone(),
                CompletionType = CompletionType,
                Processor = Processor
            };

            return clone;
        }

        private void OnTaskCompleted(ITask task)
        {
            Updated?.Invoke(this);

            if (CompletionType.CheckCompletion(this))
            {
                IsCompleted = true;
                Completed?.Invoke(this);

                Updated = null;
                Completed = null;
            }
        }
    }
}
