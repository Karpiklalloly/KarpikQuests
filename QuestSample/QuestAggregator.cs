using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System;
using System.Collections;
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
#if UNITY
[SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Quests")]
#endif
        [SerializeThis("Quests")]
        private readonly IQuestCollection _quests = new QuestCollection();
#if UNITY
[SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Links")]
#endif
        [SerializeThis("Links")]
        private readonly IQuestLinker _linker = new QuestLinker();

#if JSON_NEWTONSOFT
        [JsonIgnore]
#endif
        [DoNotSerializeThis]
        public IReadOnlyCollection<IQuest> Quests => _quests;

        public QuestAggregator()
        {

        }

        public bool TryAddQuest(IQuest quest)
        {
            if (!DefaultValidation(quest))
            {
                return false;
            }

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
        public bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true)
        {
            if (!DefaultValidation(quest))
            {
                return false;
            }

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

        public bool TryAddDependence(IQuest quest, IQuest dependence)
        {
            if (!DefaultValidation(quest))
            {
                return false;
            }

            if (!DefaultValidation(dependence))
            {
                return false;
            }


            return _linker.TryAddDependence(quest.Key, dependence.Key);
        }

        public bool TryRemoveDependence(IQuest quest, IQuest dependence)
        {
            if (!DefaultValidation(quest))
            {
                return false;
            }

            if (!DefaultValidation(dependence))
            {
                return false;
            }

            return _linker.TryRemoveDependence(quest.Key, dependence.Key);
        }

        public bool TryToReplace(IQuest quest1, IQuest quest2, bool keysMayBeEquel)
        {
            if (!DefaultValidation(quest1))
            {
                return false;
            }

            if (!DefaultValidation(quest2))
            {
                return false;
            }

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

        public IQuestCollection GetDependencies(IQuest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

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
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

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

        public bool TryRemoveDependents(IQuest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

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

        public bool Contains(IQuest quest)
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

        public IQuest GetQuest(string questKey)
        {
            return _quests.First(x => x.Key == questKey);
        }

        public void ResetAll()
        {
            foreach (var quest in _quests)
            {
                quest.Reset();
            }
            Start();
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

        private bool DefaultValidation(IQuest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            if (string.IsNullOrWhiteSpace(quest.Key))
            {
                throw new ArgumentException($"Expected that key is not empty", nameof(quest));
            }

            return true;
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
    }
}