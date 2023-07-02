using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
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
    public class QuestAggregator : QuestAggregatorBase
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
        public override IReadOnlyCollection<IQuest> Quests => _quests;

        public QuestAggregator()
        {

        }

        public override bool TryAddQuest(IQuest quest)
        {
            if (Contains(quest))
            {
                return false;
            }

            _quests.Add(quest);
            quest.Completed += OnQuestCompleted;
            return true;
        }

        /// <summary>
        /// Note that if there are many dependencies this method will not delete quest
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="autoChangeDependencies"></param>
        /// <returns></returns>
        public override bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true)
        {
            if (!Contains(quest))
            {
                return false;
            }

            if (autoChangeDependencies)
            {
                var dependencies = _linker.GetQuestKeyDependencies(quest.Key);
                var dependents = _linker.GetQuestKeyDependents(quest.Key);

                if (dependencies.Count() > 1)
                {
                    return false;
                }

                if (dependencies.Count() > 0)
                {
                    var baseQuest = dependencies.ElementAt(0);
                    _linker.TryRemoveDependence(quest.Key, baseQuest);

                    foreach (var dep in dependents)
                    {
                        _linker.TryAddDependence(dep, baseQuest);
                    }
                }

                foreach (var dep in dependents)
                {
                    _linker.TryRemoveDependence(dep, quest.Key);
                }
            }

            quest.Completed -= OnQuestCompleted;
            _quests.Remove(quest);
            return true;
        }

        public override bool TryAddDependence(IQuest quest, IQuest dependence)
        {
            return _linker.TryAddDependence(quest.Key, dependence.Key);
        }

        public override bool TryRemoveDependence(IQuest quest, IQuest dependence)
        {
            return _linker.TryRemoveDependence(quest.Key, dependence.Key);
        }

        public override bool TryToReplace(IQuest quest1, IQuest quest2, bool keysMayBeEquel)
        {
            if (!keysMayBeEquel)
            {
                if (quest1.Equals(quest2))
                {
                    return false;
                }

                if (Contains(quest2))
                {
                    return false;
                }
            }

            if (!Contains(quest1))
            {
                return false;
            }

            var dependencies = GetDependencies(quest1);
            var dependents = GetDependents(quest1);

            foreach (var dep in dependencies)
            {
                TryRemoveDependence(quest1, dep);
                TryAddDependence(quest2, dep);
            }

            foreach (var dep in dependents)
            {
                TryRemoveDependence(dep, quest1);
                TryAddDependence(dep, quest2);
            }

            TryRemoveQuest(quest1, false);
            TryAddQuest(quest2);

            return true;
        }

        public override IQuestCollection GetDependencies(IQuest quest)
        {
            var keys = _linker.GetQuestKeyDependencies(quest.Key);
            var collection = new QuestCollection();
            foreach (var dep in keys)
            {
                collection.Add(GetQuest(dep));
            }
            return collection;
        }

        public override IQuestCollection GetDependents(IQuest quest)
        {
            var keys = _linker.GetQuestKeyDependents(quest.Key);
            var collection = new QuestCollection();
            foreach (var dep in keys)
            {
                collection.Add(GetQuest(dep));
            }
            return collection;
        }

        public override bool CheckKeyCollisions()
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

        public override void Start()
        {
            foreach (var quest in _quests)
            {
                if (!GetDependencies(quest).Any() && quest.IsNotStarted())
                {
                    Start(quest);
                }
            }
        }

        public override bool TryRemoveDependencies(IQuest quest)
        {
            if (!Contains(quest))
            {
                return false;
            }

            var dependencies = GetDependencies(quest);

            foreach (var dep in dependencies)
            {
                if (!TryRemoveDependence(quest, dep))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool TryRemoveDependents(IQuest quest)
        {
            if (!Contains(quest))
            {
                return false;
            }

            var dependents = GetDependents(quest);

            foreach (var dep in dependents)
            {
                if (!TryRemoveDependence(dep, quest))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnQuestCompleted(IQuest quest)
        {
            quest.Completed -= OnQuestCompleted;
            var dependents = GetDependents(quest);
            foreach (var dependent in dependents)
            {
                Start(dependent);
            }
        }

        private IQuest GetQuest(string key)
        {
            return _quests.First(x =>  x.Key == key);
        }

        private bool Contains(IQuest quest)
        {
            if (quest == null)
            {
                return false;
            }
            foreach (var another in _quests)
            {
                if (another.Equals(quest))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is IQuestAggregator a2)
            {
                var a1 = this;
                if (a1 == null && a2 == null)
                {
                    return true;
                }

                if (a1 == null || a2 == null)
                {
                    return false;
                }

                if (a1.Quests.Count != a2.Quests.Count)
                {
                    return false;
                }

                var quests1 = a1.Quests.ToList();
                var quests2 = a2.Quests.ToList();

                for (int i = 0; i < quests1.Count; i++)
                {
                    if (!quests1[i].Equals(quests2[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
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