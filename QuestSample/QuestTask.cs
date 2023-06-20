using KarpikQuests.Interfaces;
using Newtonsoft.Json;
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
        [JsonProperty("Key")]
        public string Key { get; private set; }

#if UNITY
[field: SerializeField]
#endif
        [JsonProperty("Name")]
        public string Name { get; private set; }

#if UNITY
[field: SerializeField]
#endif
        [JsonProperty("Status")]
        public IQuestTask.TaskStatus Status { get; private set; } = IQuestTask.TaskStatus.UnCompleted;

#if UNITY
[field: SerializeField]
#endif
        [JsonProperty("CanBeCompleted")]
        bool IQuestTask.CanBeCompleted { get; set; }

        public event Action<IQuestTask> Completed;

        public void Init(string key, string name)
        {
            Key = key;
            Name = name;
        }

        bool IQuestTask.TryToComplete()
        {
            if (!(this as IQuestTask).CanBeCompleted)
            {
                return false;
            }

            Status = IQuestTask.TaskStatus.Completed;
            (this as IQuestTask).CanBeCompleted = false;
            Completed?.Invoke(this);

            return true;
        }

        public override string ToString()
        {
            return $"{Key} {Name}";
        }

        void IQuestTask.ForceCanBeCompleted()
        {
            (this as IQuestTask).CanBeCompleted = true;
        }
    }
}