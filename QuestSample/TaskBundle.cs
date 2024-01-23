using KarpikQuests.Interfaces;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KarpikQuests.TaskProcessorTypes;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    public class TaskBundle : ITaskBundle
    {
        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks")]
#endif
        [SerializeThis("Tasks")]
        #endregion
        public IReadOnlyQuestTaskCollection QuestTasks { get; private set; } = new QuestTaskCollection();

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CompletionType")]
#endif
        [SerializeThis("CompletionType")]
        #endregion
        public ICompletionType CompletionType { get; private set; }

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("TaskProcessor")]
#endif
        [SerializeThis("TaskProcessor")]
        #endregion
        public IProcessorType TaskProcessor { get; private set; }

        public int Count => QuestTasks.Count;

        public bool IsReadOnly => false;

        public bool IsCompleted { get; private set; } = false;

        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;

        public TaskBundle() : this(new AND(), new Orderly())
        {

        }

        public TaskBundle(ICompletionType completionType, IProcessorType questTaskProcessor)
        {
            CompletionType = completionType ?? new AND();
            TaskProcessor = questTaskProcessor ?? new Orderly();
        }

        public void Add(IQuestTask item)
        {
            QuestTasks.Add(item);
            item.Completed += OnTaskCompleted;
        }

        public void Clear()
        {
            QuestTasks.Clear();
        }

        public object Clone()
        {
            TaskBundle clone = new TaskBundle
            {
                QuestTasks = (IReadOnlyQuestTaskCollection)QuestTasks.Clone(),
                CompletionType = CompletionType,
                TaskProcessor = TaskProcessor
            };

            return clone;
        }

        public bool Contains(IQuestTask item)
        {
            return QuestTasks.Has(item);
        }

        public void CopyTo(IQuestTask[] array, int arrayIndex)
        {
            QuestTasks.CopyTo(array, arrayIndex);
        }

        public bool Equals(ITaskBundle? other)
        {
            if (other is null) return false;

            if (QuestTasks.GetType() != other.QuestTasks.GetType())
            {
                return false;
            }

            if (QuestTasks.Count != other.Count)
            {
                return false;
            }

            return QuestTasks.Equals(other.QuestTasks);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is TaskBundle bundle))
            {
                return false;
            }

            return Equals(bundle);
        }

        public override int GetHashCode()
        {
            return QuestTasks.GetHashCode();
        }

        public IEnumerator<IQuestTask> GetEnumerator()
        {
            return QuestTasks.GetEnumerator();
        }

        public bool Remove(IQuestTask item)
        {
            item.Completed -= OnTaskCompleted;
            return QuestTasks.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (QuestTasks as IEnumerable).GetEnumerator();
        }

        public void ResetAll(bool canBeCompleted = false)
        {
            TaskProcessor.Setup(this);
        }

        public void ResetFirst(bool canBeCompleted = false)
        {
            QuestTasks?.First()?.Reset(canBeCompleted);
        }

        public bool Has(IQuestTask task)
        {
            return QuestTasks.Has(task);
        }

        public bool Has(string taskKey)
        {
            var task = QuestTasks.First(x => x.Key.Equals(taskKey));
            return QuestTasks.Has(task);
        }

        public void ClearTasks()
        {
            foreach (IQuestTask task in QuestTasks)
            {
                task.Clear();
            }
            Updated = null;
            Completed = null;
        }

        public bool CheckCompletion()
        {
            return CompletionType.CheckCompletion(this);
        }

        private void OnTaskCompleted(IQuestTask task)
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
