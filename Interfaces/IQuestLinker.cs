using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestLinker
    {
        public IReadOnlyCollection<string> GetQuestKeyDependencies(string key);

        public IReadOnlyCollection<string> GetQuestKeyDependents(string key);

        public bool TryAddDependence(string key, string dependenceKey);

        public bool TryRemoveDependence(string key, string dependenceKey);

        public bool TryReplace(string key, string newKey);

        public void Clear();
    }
}