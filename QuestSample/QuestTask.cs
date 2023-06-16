using KarpikQuests.Interfaces;
using System;

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestTask : IQuestTask
    {
#if UNITY
[field: SerializeField]
#endif
        public string Name { get; private set; }

#if UNITY
[field: SerializeField]
#endif
        public IQuestTask.TaskStatus Status { get; private set; } = IQuestTask.TaskStatus.UnCompleted;

#if UNITY
[field: SerializeField]
#endif
        bool IQuestTask.CanBeCompleted { get; set; }

        public event Action<IQuestTask> Completed;

        public void Init(string name)
        {
            Name = name;
        }

        void IQuestTask.Complete()
        {
            if (!(this as IQuestTask).CanBeCompleted)
            {
                return;
            }

            Status = IQuestTask.TaskStatus.Completed;
            Completed?.Invoke(this);
        }

        public override string ToString()
        {
            return $"{Name} -> {Status}";
        }

        void IQuestTask.ForceBeCompleted()
        {
            (this as IQuestTask).CanBeCompleted = true;
        }
    }
}