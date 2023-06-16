using KarpikQuests.Interfaces;
using System.Collections.Generic;
using System.Linq;

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestLinker : IQuestLinker
    {
#if UNITY
[SerializeField]
#endif
        private readonly Dictionary<IQuest, IQuestCollection> _dependencies = new Dictionary<IQuest, IQuestCollection>();

        public IQuestCollection GetQuestDependencies(IQuest quest)
        {
            if (!_dependencies.ContainsKey(quest))
            {
                return new QuestCollection();
            }
            return _dependencies[quest];
        }

        public IQuestCollection GetQuestDependents(IQuest quest)
        {
            QuestCollection collection = new QuestCollection();
            foreach (var pair in _dependencies)
            {
                if (pair.Value == null || !pair.Value.Any())
                {
                    continue;
                }

                if (pair.Key.Equals(quest))
                {
                    continue;
                }

                if (pair.Value.Contains(quest))
                {
                    collection.Add(pair.Key);
                }
            }
            return collection;
        }

        public bool TryAddDependence(IQuest quest, IQuest dependence)
        {
            if (!_dependencies.ContainsKey(quest))
            {
                _dependencies.Add(quest, new QuestCollection());
            }

            if (_dependencies[quest].Contains(dependence))
            {
                return false;
            }

            if (quest.Equals(dependence))
            {
                return false;
            }

            //Check to not link to each other
            if (_dependencies.ContainsKey(dependence))
            {
                if (_dependencies[dependence].Contains(quest))
                {
                    return false;
                }
            }

            _dependencies[quest].Add(dependence);
            return true;
        }

        public bool TryRemoveDependence(IQuest quest, IQuest dependent)
        {
            if (!_dependencies.ContainsKey(quest))
            {
                return false;
            }

            if (!_dependencies[quest].Contains(dependent))
            {
                return false;
            }

            _dependencies[quest].Remove(dependent);
            return true;
        }
    }
}


