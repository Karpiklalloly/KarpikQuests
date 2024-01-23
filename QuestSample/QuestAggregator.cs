using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using System.Linq;
using System.Runtime.Serialization;
using System;
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
    public class QuestAggregator : IQuestAggregator
    {
        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Quests")]
#endif
        [SerializeThis("Quests")]
        #endregion
        private readonly IQuestCollection _quests = new QuestCollection();

        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Links")]
#endif
        [SerializeThis("Links")]
        #endregion
        private readonly IQuestLinker _linker = new QuestLinker();

        #region noserialize
#if JSON_NEWTONSOFT
        [JsonIgnore]
#endif
        [DoNotSerializeThis]
        #endregion
        public IReadOnlyQuestCollection Quests => _quests;

        public QuestAggregator()
        {

        }

        public bool TryAddQuest(IQuest quest)
        {
            if (!quest.IsValid())
            {
                return false;
            }

            if (Has(quest))
            {
                return false;
            }

            _quests.Add(quest);
            quest.Completed += OnQuestCompleted;
            quest.KeyChanged += OnKeyChanged;
            return true;
        }

        /// <summary>
        /// Note that if there are many dependencies this method will not delete quest
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="autoChangeDependencies"></param>
        /// <returns></returns>
        public bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true)
        {
            if (!quest.IsValid())
            {
                return false;
            }

            if (!Has(quest))
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

        public bool TryAddDependence(IQuest quest, IQuest dependence)
        {
            if (!quest.IsValid())
            {
                return false;
            }

            if (!dependence.IsValid())
            {
                return false;
            }


            return _linker.TryAddDependence(quest.Key, dependence.Key);
        }

        public bool TryRemoveDependence(IQuest quest, IQuest dependence)
        {
            if (!quest.IsValid())
            {
                return false;
            }

            if (!dependence.IsValid())
            {
                return false;
            }

            return _linker.TryRemoveDependence(quest.Key, dependence.Key);
        }

        public bool TryReplace(IQuest quest1, IQuest quest2, bool keysMayBeEquel)
        {
            if (!quest1.IsValid())
            {
                return false;
            }

            if (!quest2.IsValid())
            {
                return false;
            }

            if (!keysMayBeEquel)
            {
                if (quest1.Equals(quest2))
                {
                    return false;
                }

                if (Has(quest2))
                {
                    return false;
                }
            }

            if (!Has(quest1))
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

        public IQuestCollection GetDependencies(IQuest quest)
        {
            if (quest == null) throw new ArgumentNullException(nameof(quest));

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
            if (quest == null) throw new ArgumentNullException(nameof(quest));

            var collection = new QuestCollection();
            var keys = _linker.GetQuestKeyDependents(quest.Key);
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
                if (!GetDependencies(quest).Any() && quest.IsNotStarted())
                {
                    quest.Start();
                }
            }
        }

        public bool TryRemoveDependencies(IQuest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            if (!Has(quest))
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

        public bool TryRemoveDependents(IQuest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            if (!Has(quest))
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

        public bool Has(IQuest quest)
        {
            return _quests.Has(quest);
        }

        public IQuest GetQuest(string questKey)
        {
            return _quests.First(x => x.Key == questKey);
        }

        public void ResetQuests()
        {
            foreach (var quest in _quests)
            {
                quest.Reset();
            }
            Start();
        }

        public override string ToString()
        {
            return _quests.ToString() + '\n' + _linker.ToString();
        }

        public void Clear()
        {
            foreach (var quest in _quests)
            {
                quest.Clear();
            }

            _linker.Clear();
            _quests.Clear();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is IQuestAggregator aggregator)
            {
                Equals(aggregator);
            }

            return false;
        }

        public bool Equals(IQuestAggregator? other)
        {
            var a1 = this;
            if (a1 == null && other == null)
            {
                return true;
            }

            if (a1 == null || other == null)
            {
                return false;
            }

            if (a1.Quests.Count() != other.Quests.Count())
            {
                return false;
            }

            var quests1 = a1.Quests.ToList();
            var quests2 = other.Quests.ToList();

            for (int i = 0; i < quests1.Count; i++)
            {
                if (!quests1[i].Equals(quests2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Quests.GetHashCode();
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

        /// <summary>
        /// Subscribe on events
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var quest in _quests)
            {
                if (quest.IsCompleted())
                {
                    continue;
                }

                quest.Completed += OnQuestCompleted;
            }
        }

        private void OnKeyChanged(string oldKey, string newKey)
        {
            _linker.TryReplace(oldKey, newKey);
        }


    }
}