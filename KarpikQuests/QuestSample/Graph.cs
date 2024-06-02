using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Graph : IGraph
    {
        [JsonIgnore]
        public IReadOnlyQuestCollection Quests => _quests;

        [JsonIgnore]
        public IReadOnlyQuestCollection StartQuests => new QuestCollection((_dependencies.Where(pair => pair.Value.Count == 0).Select(pair => GetQuest(pair.Key))).ToList());
        
        [JsonProperty("Quest_matrix")]
        [SerializeThis("Quest_matrix")]
        private Dictionary<Id, List<IGraph.Connection>> _dependencies = new();
        [JsonProperty("Quests")]
        [SerializeThis("Quests", IsReference = true)]
        private IQuestCollection _quests = new QuestCollection();

        public bool TryAdd(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            
            if (Has(quest)) return false;
            
            _dependencies.Add(quest.Id, new List<IGraph.Connection>());
            _quests.Add(quest);
            Subscribe(quest);
            
            return true;
        }
        
        public bool TryRemove(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            if (!Has(quest)) return false;

            _dependencies.Remove(quest.Id);
            _quests.Remove(quest);

            foreach (var pair in _dependencies)
            {
                pair.Value.RemoveAll(x => x.QuestId == quest.Id);
            }
            Unsubscribe(quest);

            return true;
        }

        public bool TryRemove(Id questId)
        {
            return questId.IsValid() && TryRemove(GetQuest(questId));
        }

        public bool TryReplace(IQuest from, IQuest to)
        {
            if (!from.IsValid() || !to.IsValid()) return false;
            if (!Has(from) || Has(to)) return false;

            var deps = _dependencies[from.Id];
            _dependencies.Remove(from.Id);
            _quests.Remove(from);

            foreach (var pair in _dependencies)
            {
                var index =  pair.Value.FindIndex(x => x.QuestId == from.Id);
                if (index < 0) continue;
                var connection = pair.Value[index];
                pair.Value.RemoveAt(index);
                pair.Value.Add(new IGraph.Connection(to.Id, connection.DependencyType));
            }
            
            _dependencies.Add(to.Id, deps);
            _quests.Add(to);
            Unsubscribe(from);
            Subscribe(to);
            
            return true;
        }

        public void Clear()
        {
            foreach (var quest in _quests)
            {
                quest.Clear();
            }
            _quests.Clear();
        }
        
        public bool TryAddDependency(Id questId, Id dependencyQuestId, IDependencyType dependencyType)
        {
            if (!questId.IsValid() || !dependencyQuestId.IsValid()) return false;
            if (!Has(questId) || !Has(dependencyQuestId)) return false;
            
            _dependencies[questId].Add(new IGraph.Connection(dependencyQuestId, dependencyType));
            
            return true;
        }

        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, IDependencyType dependencyType)
        {
            return TryAddDependency(quest.Id, dependencyQuest.Id, dependencyType);
        }

        public bool TryAddDependency(Id questId, Id dependencyQuestId, IGraph.DependencyType dependencyType)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            return dependencyType switch
            {
                IGraph.DependencyType.Completion => TryAddDependency(
                    questId, dependencyQuestId, new Completion()),
                IGraph.DependencyType.Fail => TryAddDependency(
                    questId, dependencyQuestId, new Fail()),
                IGraph.DependencyType.Start => TryAddDependency(
                    questId, dependencyQuestId, new Start()),
                _ => false
            };
        }

        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, IGraph.DependencyType dependencyType = IGraph.DependencyType.Completion)
        {
            return TryAddDependency(quest.Id, dependencyQuest.Id, dependencyType);
        }

        public bool TryRemoveDependencies(Id questId)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            _dependencies[questId].Clear();
            return true;
        }

        public bool TryRemoveDependencies(IQuest quest)
        {
            return TryRemoveDependencies(quest.Id);
        }

        public bool TryRemoveDependents(Id questId)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            var dependents = GetDependentsQuests(questId);
            foreach (var dependent in dependents)
            {
                TryRemoveDependency(dependent.DependentQuest.Id, questId);
            }
            
            return true;
        }

        public bool TryRemoveDependents(IQuest quest)
        {
            return TryRemoveDependents(quest.Id);
        }

        public bool TryRemoveDependency(Id questId, Id dependencyQuestId)
        {
            if (!questId.IsValid() || !dependencyQuestId.IsValid()) return false;
            if (!Has(questId) || !Has(dependencyQuestId)) return false;
            
            var index = _dependencies[questId].FindIndex(x => x.QuestId == dependencyQuestId);
            if (index < 0) return false;
            
            _dependencies[questId].RemoveAt(index);
            
            return true;
        }

        public bool TryRemoveDependency(IQuest quest, IQuest dependencyQuest)
        {
            return TryRemoveDependency(quest.Id, dependencyQuest.Id);
        }

        public bool IsCyclic()
        {
            var startQuests = StartQuests;
            if (!startQuests.Any()) return true;
            
            var visited = new Dictionary<IQuest, bool>();
            var recStack = new Dictionary<IQuest, bool>();
            foreach (var quest in Quests)
            {
                visited.Add(quest, false);
                recStack.Add(quest, false);
            }
            
            foreach (var node in startQuests)
            {
                if (IsCyclicUtil(node, visited, recStack)) return true;
            }

            return visited.Any(x => x.Value != true);
        }

        public bool Has(Id questId)
        {
            return GetQuest(questId).IsValid();
        }

        public bool Has(IQuest quest)
        {
            if (!quest.IsValid()) return false;

            return Has(quest.Id);
        }

        public IQuest GetQuest(Id questId)
        {
            return Quests.FirstOrDefault(x => x.Id == questId);
        }

        public IEnumerable<ConnectionWithQuest> GetDependenciesQuests(Id questId)
        {
            var list = new List<ConnectionWithQuest>();
            
            if (!questId.IsValid()) return list;
            if (!Has(questId)) return list;
            
            foreach (var connection in _dependencies[questId])
            {
                list.Add( new ConnectionWithQuest(
                    dependentQuest: GetQuest(questId),
                    dependencyQuest: GetQuest(connection.QuestId),
                    dependency: connection.DependencyType));
            }

            return list;
        }

        public IEnumerable<ConnectionWithQuest> GetDependenciesQuests(IQuest quest)
        {
            return GetDependenciesQuests(quest.Id);
        }

        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(Id questId)
        {
            if (!questId.IsValid()) return new List<ConnectionWithQuest>();
            if (!Has(questId)) return new List<ConnectionWithQuest>();
            
            return GetDependentsQuests(GetQuest(questId));
        }

        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(IQuest quest)
        {
            var list = new List<ConnectionWithQuest>();
            
            if (!quest.IsValid()) return list;
            if (!Has(quest)) return list;
            
            foreach (var pair in _dependencies)
            {
                var index = pair.Value.FindIndex(x => x.QuestId == quest.Id);
                if (index < 0) continue;
                list.Add(new ConnectionWithQuest(
                    dependentQuest: GetQuest(pair.Key),
                    dependencyQuest: GetQuest(pair.Value[index].QuestId),
                    dependency: pair.Value[index].DependencyType));
            }
            
            return list;
        }
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var quest in _quests)
            {
                Subscribe(quest);
            }
        }

        private bool IsCyclicUtil(IQuest quest, Dictionary<IQuest, bool> visited, Dictionary<IQuest, bool> recStack)
        {
            if (recStack[quest]) return true;
            if (visited[quest]) return false;

            visited[quest] = true;
            recStack[quest] = true;
            
            var dependents = GetDependentsQuests(quest);

            foreach (var dependent in dependents)
            {
                if (IsCyclicUtil(dependent.DependentQuest, visited, recStack)) return true;
            }

            recStack[quest] = false;

            return false;
        }

        private void Subscribe(IQuest quest)
        {
            quest.Started += OnQuestStarted;
            quest.Updated += OnQuestUpdated;
            quest.Failed += OnQuestFailed;
            quest.Completed += OnQuestCompleted;
        }
        
        private void Unsubscribe(IQuest quest)
        {
            quest.Started -= OnQuestStarted;
            quest.Updated -= OnQuestUpdated;
            quest.Failed -= OnQuestFailed;
            quest.Completed -= OnQuestCompleted;
        }
        
        private void OnQuestStarted(IQuest quest)
        {
            
        }
        
        private void OnQuestUpdated(IQuest quest, ITaskBundle bundle)
        {
            var dependents = GetDependentsQuests(quest);

            foreach (var connection in dependents)
            {
                if (!connection.Dependency.IsOk(quest)) continue;
                
                connection.DependentQuest.Start();
            }
        }
        
        private void OnQuestFailed(IQuest quest)
        {
            
        }

        private void OnQuestCompleted(IQuest quest)
        {
            
        }
    }
}