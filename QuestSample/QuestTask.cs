using KarpikQuests.Interfaces;
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
    [Serializable]
    public class QuestTask : IQuestTask
    {
        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Key")]
#endif
        [SerializeThis("Key")]
        #endregion
        public string Key { get; private set; }

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Name")]
#endif
        [SerializeThis("Name")]
        #endregion
        public string Name { get; private set; }

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Description")]
#endif
        [SerializeThis("Description")]
        #endregion
        public string Description { get; private set; }

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Status")]
#endif
        [SerializeThis("Status")]
        #endregion
        public IQuestTask.TaskStatus Status { get; private set; } = IQuestTask.TaskStatus.UnCompleted;

        #region serialize
#if UNITY
        [field: SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("CanBeCompleted")]
#endif
        [SerializeThis("CanBeCompleted")]
        #endregion
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

        public object Clone()
        {
            QuestTask task = new QuestTask
            {
                Key = Key,
                Name = Name,
                Status = Status,
                CanBeCompleted = CanBeCompleted,
                Completed = (Action<IQuestTask>?)Completed?.Clone()
            };

            return task;
        }

        public bool Equals(IQuestTask? other)
        {
            if (other is null) return false;
            if (Key is null) return false;
            return Key.Equals(other.Key);
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

        public override string ToString()
        {
            return $"{Key} {Name} ({Status})";
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}