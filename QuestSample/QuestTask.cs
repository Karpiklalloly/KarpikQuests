using KarpikQuests.Interfaces;
using KarpikQuests.Saving;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class QuestTask : IQuestTask
    {
#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Key")]
#endif
        [SerializeThis("Key")]
        public string Key { get; private set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name")]
#endif
        [SerializeThis("Name")]
        public string Name { get; private set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description")]
#endif
        [SerializeThis("Description")]
        public string Description { get; private set; }

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status")]
#endif
        [SerializeThis("Status")]
        public IQuestTask.TaskStatus Status { get; private set; } = IQuestTask.TaskStatus.UnCompleted;

#if UNITY
[field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CanBeCompleted")]
#endif
        [SerializeThis("CanBeCompleted")]
        public bool CanBeCompleted { get; private set; }

        public event Action<IQuestTask>? Completed;

        public void Init(string key, string name, string description = "")
        {
            Key = key;
            Name = name;
            Description = description;
        }

        public void Reset(bool canBeCompleted = false)
        {
            CanBeCompleted = canBeCompleted;
            Status = IQuestTask.TaskStatus.UnCompleted;
        }

        public override string ToString()
        {
            return $"{Key} {Name} ({Status})";
        }

        public object Clone()
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

        public bool Equals(IQuestTask other)
        {
            if (other == null) return false;
            if (Key == null) return false;
            return Key.Equals(other.Key);
        }

        void IQuestTask.ForceCanBeCompleted()
        {
            CanBeCompleted = true;
        }

        public bool TryToComplete()
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

        public void Clear()
        {
            Reset(false);
            Completed = null;
        }
    }
}