using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestAggregator : IQuestAggregator
    {
#if UNITY
[SerializeField]
#endif
        [JsonProperty("Quests")]
        private readonly IQuestCollection _quests = new QuestCollection();
#if UNITY
[SerializeField]
#endif
        [JsonProperty("Links")]
        private readonly IQuestLinker _linker = new QuestLinker();

        [JsonIgnore]
        public IReadOnlyCollection<IQuest> Quests => _quests;

        public QuestAggregator()
        {
            QuestInfo.RegisterAggregator(this);
        }

        public bool TryAddQuest(IQuest quest)
        {
            if (_quests.Contains(quest))
            {
                return false;
            }

            _quests.Add(quest);
            quest.Completed += OnQuestCompleted;
            return true;
        }

        public bool TryRemoveQuest(IQuest quest)
        {
            if (!_quests.Contains(quest))
            {
                return false;
            }

            var dependencies = _linker.GetQuestKeyDependencies(quest.Key);

            if (dependencies.Count() > 1)
            {
                return false;
            }

            var baseQuest = dependencies.ElementAt(0);
            _linker.TryRemoveDependence(quest.Key, baseQuest);

            var dependents = _linker.GetQuestKeyDependents(quest.Key);
            foreach (var dep in dependents)
            {
                _linker.TryRemoveDependence(dep, quest.Key);
                _linker.TryAddDependence(dep, baseQuest);
            }

            _quests.Remove(quest);
            return true;
        }

        public bool TryAddDependence(IQuest quest, IQuest dependence)
        {
            return _linker.TryAddDependence(quest.Key, dependence.Key);
        }

        public bool TryRemoveDependence(IQuest quest, IQuest dependence)
        {
            return _linker.TryRemoveDependence(quest.Key, dependence.Key);
        }

        public IQuestCollection GetDependencies(IQuest quest)
        {
            var keys = _linker.GetQuestKeyDependencies(quest.Key);
            var collection = new QuestCollection();
            foreach (var dep in keys)
            {
                collection.Add(GetQuest(dep));
            }
            return collection;
        }

        public IQuestCollection GetDependents(IQuest quest)
        {
            var keys = _linker.GetQuestKeyDependents(quest.Key);
            var collection = new QuestCollection();
            foreach (var dep in keys)
            {
                collection.Add(GetQuest(dep));
            }
            return collection;
        }

        public bool CheckKeyCollisions()
        {
            var keys = _quests.GroupBy(x => x.Key)
                .Where(group => group.Count() > 1)
                .Select(quest => quest.Key);
            if (keys.Count() > 1)
            {
                return false;
            }
            return true;
        }

        public void Start()
        {
            foreach (var quest in _quests)
            {
                if (!GetDependencies(quest).Any())
                {
                    quest.Start();
                }
            }
        }

        private void OnQuestCompleted(IQuest quest)
        {
            quest.Completed -= OnQuestCompleted;
            var dependents = GetDependents(quest);
            foreach (var dependent in dependents)
            {
                dependent.Start();
            }
        }

        private IQuest GetQuest(string key)
        {
            return _quests.First(x =>  x.Key == key);
        }

        /// <summary>
        /// Subscribe to events
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            //OnComplete
            foreach (var quest in _quests)
            {
                if (quest.IsCompleted())
                {
                    continue;
                }

                quest.Completed += OnQuestCompleted;
            }
        }

        public override string ToString()
        {
            return _quests.ToString() + '\n' + _linker.ToString();
        }
    }
}