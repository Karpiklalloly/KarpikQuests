﻿using KarpikQuests.Interfaces;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KarpikQuests.TaskProcessorTypes;
using System.Diagnostics.CodeAnalysis;

namespace KarpikQuests.QuestSample
{
    public sealed class TaskBundle : ITaskBundle
    {
        public event Action<ITaskBundle>? Updated;
        public event Action<ITaskBundle>? Completed;

        [SerializeThis("Tasks")]
        public IReadOnlyTaskCollection QuestTasks { get; private set; } = new TaskCollection();

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

        public int Count
        {
            get => QuestTasks.Count;
        }

        public bool IsReadOnly => false;

        public bool IsCompleted { get; private set; }

        [SerializeThis("CompletionType")]
        private ICompletionType _completionType;

        [SerializeThis("TaskProcessor")]
        private IProcessorType _processor;

        public TaskBundle() : this(new AND(), new Orderly())
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
            foreach (ITask task in QuestTasks)
            {
                task.Reset();
            }
            Updated = null;
            Completed = null;
        }

        public void ResetAll(bool canBeCompleted = false)
        {
            Processor.Setup(this);
        }

        public void ResetFirst(bool canBeCompleted = false)
        {
            QuestTasks.FirstOrDefault()?.Reset(canBeCompleted);
        }

#region list
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

        public int GetHashCode([DisallowNull] ITaskBundle obj)
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
                Completed?.Invoke(this);
                IsCompleted = true;

                Updated = null;
                Completed = null;
            }
        }
    }
}
