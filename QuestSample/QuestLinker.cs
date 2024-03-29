﻿using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestLinker : IQuestLinker
    {
        [SerializeThis(Name = "Quest_dependencies")]
        [JsonProperty("Quest_dependencies")]
        private readonly Dictionary<string, List<string>> _dependencies = new Dictionary<string, List<string>>();

        public void Clear()
        {
            _dependencies.Clear();
        }

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
            var collection = new List<string>();
            foreach (var pair in _dependencies)
            {
                if (!pair.Value.Any())
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

            if (_dependencies[key].Contains(dependenceKey)) return false;

            if (key.Equals(dependenceKey)) return false;

            //Check to not link to each other
            if (_dependencies.TryGetValue(dependenceKey, out var dependence) && dependence.Contains(key))
            {
                return false;
            }

            _dependencies[key].Add(dependenceKey);
            return true;
        }

#pragma warning disable S927 // Parameter names should match base declaration and other partial definitions
        public bool TryRemoveDependence(string key, string dependentKey)
#pragma warning restore S927 // Parameter names should match base declaration and other partial definitions
        {
            if (!_dependencies.ContainsKey(key)) return false;

            if (!_dependencies[key].Contains(dependentKey)) return false;

            _dependencies[key].Remove(dependentKey);

            if (_dependencies[key].Count == 0)
            {
                _dependencies.Remove(key);
            }

            return true;
        }

        public bool TryReplace(string key, string newKey)
        {
            if (!_dependencies.TryGetValue(key, out var value)) return false;

            _dependencies.Remove(key);
            _dependencies.Add(key, value);
            return true;
        }
    }
}


