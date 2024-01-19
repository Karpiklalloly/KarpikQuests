using KarpikQuests.Interfaces;
using KarpikQuests.QuestCompletionTypes;
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
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Tasks")]
#endif
        [SerializeThis("Tasks")]
        public IQuestTaskCollection QuestTasks { get; private set; } = new QuestTaskCollection();

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CompletionType")]
#endif
        [SerializeThis("CompletionType")]
        public ICompletionType CompletionType { get; private set; }

#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("TaskProcessor")]
#endif
        [SerializeThis("TaskProcessor")]
        public ITaskProcessorType TaskProcessor { get; private set; }

        public int Count => QuestTasks.Count;

        public bool IsReadOnly => false;

        public bool IsCompleted { get; private set; } = false;

        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;

        public TaskBundle() : this(new CompletionAND(), new TaskProcessorOrderly())
        {

        }

        public TaskBundle(ICompletionType completionType, ITaskProcessorType questTaskProcessor)
        {
            CompletionType = completionType ?? new CompletionAND();
            TaskProcessor = questTaskProcessor ?? new TaskProcessorOrderly();
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
                QuestTasks = (IQuestTaskCollection)QuestTasks.Clone(),
                CompletionType = CompletionType,
                TaskProcessor = TaskProcessor
            };

            return clone;
        }

        public bool Contains(IQuestTask item)
        {
            return QuestTasks.Contains(item);
        }

        public void CopyTo(IQuestTask[] array, int arrayIndex)
        {
            QuestTasks.CopyTo(array, arrayIndex);
        }

        public bool Equals(ITaskBundle other)
        {
            if (QuestTasks.GetType() != other.QuestTasks.GetType())
            {
                return false;
            }
            if (CompletionType.GetType() != other.CompletionType.GetType())
            {
                return false;
            }
            if (TaskProcessor.GetType() != other.TaskProcessor.GetType())
            {
                return false;
            }

            if (QuestTasks.Count != other.Count)
            {
                return false;
            }
            for (int i = 0; i < QuestTasks.Count; i++)
            {
                if (!QuestTasks.ElementAt(i).Equals(other.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
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

        public bool ContainsTask(IQuestTask task)
        {
            return QuestTasks.Select(x => x.Key).Contains(task.Key);
        }

        public bool ContainsTask(string taskKey)
        {
            return QuestTasks.Select(x => x.Key).Contains(taskKey);
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

        void ITaskBundle.OnTaskCompleted(IQuestTask task)
        {
            OnTaskCompleted(task);
        }

        private void OnTaskCompleted(IQuestTask task)
        {
            Updated?.Invoke(this);
            TaskProcessor.OnTaskCompleted(this, task);

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
