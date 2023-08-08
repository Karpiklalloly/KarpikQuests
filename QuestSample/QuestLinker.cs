using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections.Generic;
using System.Linq;

#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

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
#if JSON_NEWTONSOFT
        [JsonProperty("Quest_dependencies")]
#endif
        [SerializeThis("Quest_dependencies")]
        private readonly Dictionary<string, List<string>> _dependencies = new Dictionary<string, List<string>>();

        public IReadOnlyCollection<string> GetQuestKeyDependencies(string key)
        {
            if (!_dependencies.ContainsKey(key))
            {
                return new List<string>();
            }
            return _dependencies[key];
        }

        public IReadOnlyCollection<string> GetQuestKeyDependents(string key)
        {
            List<string> collection = new List<string>();
            foreach (var pair in _dependencies)
            {
                if (pair.Value == null || !pair.Value.Any())
                {
                    continue;
                }

                if (pair.Key.Equals(key))
                {
                    continue;
                }

                if (pair.Value.Contains(key))
                {
                    collection.Add(pair.Key);
                }
            }
            return collection;
        }

        public bool TryAddDependence(string key, string dependenceKey)
        {
            if (!_dependencies.ContainsKey(key))
            {
                _dependencies.Add(key, new List<string>());
            }

            if (_dependencies[key].Contains(dependenceKey))
            {
                return false;
            }

            if (key.Equals(dependenceKey))
            {
                return false;
            }

            //Check to not link to each other
            if (_dependencies.ContainsKey(dependenceKey))
            {
                if (_dependencies[dependenceKey].Contains(key))
                {
                    return false;
                }
            }

            _dependencies[key].Add(dependenceKey);
            return true;
        }

        public bool TryRemoveDependence(string key, string dependentKey)
        {
            if (!_dependencies.ContainsKey(key))
            {
                return false;
            }

            if (!_dependencies[key].Contains(dependentKey))
            {
                return false;
            }

            _dependencies[key].Remove(dependentKey);

            if (_dependencies[key].Count == 0)
            {
                _dependencies.Remove(key);
            }
            return true;
        }
    }
}


