using KarpikQuests.Interfaces;
using KarpikQuests.QuestCompletionTypes;
using KarpikQuests.QuestTaskProcessorTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarpikQuests.QuestSample
{
    public class TaskBundle : ITaskBundle
    {
        public IQuestTaskCollection QuestTasks {get; private set; } = new QuestTaskCollection();

        public IQuestCompletionType CompletionType { get; private set; } = new QuestCompletionAND();

        public IQuestTaskProcessorType QuestTaskProcessor { get; private set; } = new QuestTaskProcessorOrderly();

        public int Count => QuestTasks.Count;

        public bool IsReadOnly => false;

        public void Add(IQuestTask item)
        {
            QuestTasks.Add(item);
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
                QuestTaskProcessor = QuestTaskProcessor
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
            if (QuestTaskProcessor.GetType() != other.QuestTaskProcessor.GetType())
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
            return QuestTasks.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (QuestTasks as IEnumerable).GetEnumerator();
        }

        public void ResetAll(bool canBeCompleted = false)
        {
            foreach (IQuestTask item in QuestTasks)
            {
                item.Reset(canBeCompleted);
            }
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
    }
}
