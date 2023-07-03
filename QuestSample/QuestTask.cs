using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

#if JSON
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestTask : QuestTaskBase
    {
#if UNITY
[field: SerializeField]
#endif
#if JSON
        [JsonProperty("Key")]

#endif
        public override string Key { get; protected set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON
        [JsonProperty("Name")]
#endif
        public override string Name { get; protected set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON
        [JsonProperty("Status")]
#endif
        public override IQuestTask.TaskStatus Status { get; protected set; } = IQuestTask.TaskStatus.UnCompleted;

#if UNITY
[field: SerializeField]
#endif
#if JSON
        [JsonProperty("CanBeCompleted")]
#endif
        public override bool CanBeCompleted { get; protected set; }

        public override event Action<IQuestTask> Completed;

        public override void Init(string key, string name)
        {
            Key = key;
            Name = name;
        }

        protected override bool TryToComplete()
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

        protected override void ForceBeCompleted()
        {
            CanBeCompleted = true;
        }
    }
}