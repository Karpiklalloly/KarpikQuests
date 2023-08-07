using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using KarpikQuests.Saving;
using System;

#if JSON_NEWTONSOFT
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
#if JSON_NEWTONSOFT
        [JsonProperty("Key")]
#endif
        [SerializeThis("Key")]
        public override string Key { get; protected set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name")]
#endif
        [SerializeThis("Name")]
        public override string Name { get; protected set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description")]
#endif
        [SerializeThis("Description")]
        public override string Description { get; protected set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status")]
#endif
        [SerializeThis("Status")]
        public override IQuestTask.TaskStatus Status { get; protected set; } = IQuestTask.TaskStatus.UnCompleted;

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CanBeCompleted")]
#endif
        [SerializeThis("CanBeCompleted")]
        public override bool CanBeCompleted { get; protected set; }

        public override event Action<IQuestTask> Completed;

        public override void Init(string key, string name, string description = "")
        {
            Key = key;
            Name = name;
            Description = description;
        }

        public override void Reset(bool canBeCompleted = false)
        {
            CanBeCompleted = canBeCompleted;
            Status = IQuestTask.TaskStatus.UnCompleted;
        }

        protected override bool TryToComplete()
        {
            if (!CanBeCompleted)
            {
                return false;
            }

            Status = IQuestTask.TaskStatus.Completed;
            CanBeCompleted = false;
            Completed?.Invoke(this);

            return true;
        }

        public override string ToString()
        {
            return $"{Key} {Name} ({Status})";
        }

        protected override void ForceBeCompleted()
        {
            CanBeCompleted = true;
        }

        public override object Clone()
        {
            QuestTask task = new QuestTask
            {
                Key = Key,
                Name = Name,
                Status = Status,
                CanBeCompleted = CanBeCompleted,
                Completed = (Action<IQuestTask>)Completed?.Clone()
            };

            return task;
        }
    }
}