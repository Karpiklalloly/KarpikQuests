using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using System.Linq;
using System.Runtime.Serialization;
using System;
using KarpikQuests.Saving;
using KarpikQuests.Statuses;
using System.Diagnostics.CodeAnalysis;

namespace KarpikQuests.QuestSample
{
    [Serializable]
    public class QuestAggregator : IQuestAggregator
    {
        public IReadOnlyQuestCollection Quests
        {
            get => _quests;
        }

        [SerializeThis("Quests")]
        private readonly IQuestCollection _quests = new QuestCollection();

        [SerializeThis("Links")]
        private readonly IQuestLinker _linker = new QuestLinker();

        public void Start()
        {
            foreach (var quest in _quests)
            {
                if (!GetDependencies(quest).Any() && quest.Status is UnStarted)
                {
                    quest.Start();
                }
            }
        }

        public bool TryAddQuest(IQuest quest)
        {
            if (!quest.IsValid()) return false;

            if (Has(quest)) return false;

            _quests.Add(quest);
            quest.Completed += OnQuestCompleted;
            quest.KeyChanged += OnKeyChanged;
            return true;
        }

        /// <summary>
        /// Note if there are many dependencies <param name="autoChangeDependencies"/> is false, this method will not delete quest
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="autoChangeDependencies"></param>
        /// <returns></returns>
        public bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true)
        {
            if (!quest.IsValid()) return false;

            if (!Has(quest)) return false;

            if (autoChangeDependencies)
            {
                var dependencies = _linker.GetQuestKeyDependencies(quest.Key);
                var dependents = _linker.GetQuestKeyDependents(quest.Key);

                if (dependencies.Count > 1)
                {
                    return false;
                }

                if (dependencies.Count > 0)
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
            if (!quest.IsValid()) return false;

            if (!dependence.IsValid()) return false;

            return _linker.TryAddDependence(quest.Key, dependence.Key);
        }

        public bool TryRemoveDependence(IQuest quest, IQuest dependence)
        {
            if (!quest.IsValid()) return false;

            if (!dependence.IsValid()) return false;

            return _linker.TryRemoveDependence(quest.Key, dependence.Key);
        }

        public bool TryReplaceQuest(IQuest quest1, IQuest quest2, bool keysAreEquel)
        {
            if (!quest1.IsValid()) return false;

            if (!quest2.IsValid()) return false;

            if (!keysAreEquel)
            {
                if (quest1.Equals(quest2)) return false;

                if (Has(quest2)) return false;
            }

            if (!Has(quest1)) return false;

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

        public bool TryRemoveDependencies(IQuest quest)
        {
#if DEBUG
            if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

            if (!Has(quest)) return false;

            var dependencies = GetDependencies(quest);

            foreach (var dep in dependencies)
            {
                if (!TryRemoveDependence(quest, dep)) return false;
            }

            return true;
        }

        public bool TryRemoveDependents(IQuest quest)
        {
#if DEBUG
            if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

            if (!Has(quest)) return false;

            var dependents = GetDependents(quest);

            foreach (var dep in dependents)
            {
                if (!TryRemoveDependence(dep, quest)) return false;
            }

            return true;
        }

        public IQuestCollection GetDependencies(IQuest quest)
        {
#if DEBUG
            if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

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
#if DEBUG
            if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

            var collection = new QuestCollection();
            var keys = _linker.GetQuestKeyDependents(quest.Key);
            foreach (var dep in keys)
            {
                collection.Add(GetQuest(dep));
            }
            return collection;
        }

        public bool HasCollisions()
        {
            var keys = _quests.GroupBy(x => x.Key)
                .Where(group => group.Count() > 1)
                .Select(quest => quest.Key);

            if (keys.Count() > 1)
            {
                return true;
            }

            return false;
        }

        public bool Has(IQuest quest)
        {
            return _quests.Has(quest);
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
            return _quests.ToString() + '\n' + _linker;
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
            return obj switch
            {
                null => false,
                IQuestAggregator aggregator => Equals(this, aggregator),
                _ => false
            };
        }

        public bool Equals(IQuestAggregator? x, IQuestAggregator? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            if (x.Quests.Count != y.Quests.Count) return false;

            var quests1 = x.Quests.ToList();
            var quests2 = y.Quests.ToList();

            return !quests1.Where((t, i) => !t.Equals(quests2[i])).Any();
        }

        public int GetHashCode([DisallowNull] IQuestAggregator obj)
        {
            return obj.GetHashCode();
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
                if (quest.Status is Completed)
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
    
        private IQuest GetQuest(string key)
        {
            return Quests.First(x => x.Key == key);
        }
    }
}