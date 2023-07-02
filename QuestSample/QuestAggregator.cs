using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#if UNITY
using UnityEngine;
#endif

//TODO: в ремув удалять и ссылки (ивенты всякие)
//Добавить метод для замены квеста (типа просто заменяется на новый, соответственно и ссылки переназначить)
//Добавить возможно убрать всее зависимости у квеста
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

        }

        public bool TryAddQuest(IQuest quest)
        {
            if (Contains(quest))
            {
                return false;
            }

            _quests.Add(quest);
            quest.Completed += OnQuestCompleted;
            return true;
        }

        public bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true)
        {
            if (!Contains(quest))
            {
                return false;
            }

            if (autoChangeDependencies)
            {
                var dependencies = _linker.GetQuestKeyDependencies(quest.Key);
                var dependents = _linker.GetQuestKeyDependents(quest.Key);

                //TODO: Как нибудь придумать переадресацию (ну или пусть вручную все делают :), гдавное чтобы все едино было, а не как сейчас)
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

                quest.Completed -= OnQuestCompleted;
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

        public bool TryToReplace(IQuest quest1, IQuest quest2, bool keysMayBeEquel)
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
                if (!GetDependencies(quest).Any() && quest.IsNotStarted())
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

        private bool Contains(IQuest quest)
        {
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

        void IQuestAggregator.Start(IQuest quest)
        {
            throw new System.NotImplementedException();
        }
    }
}