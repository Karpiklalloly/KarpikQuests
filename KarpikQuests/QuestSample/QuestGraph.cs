using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class QuestGraph : IGraph
    {
        [JsonIgnore] public IQuestCollection Quests
        {
            get
            {
                var quests = new QuestCollection();

                foreach (var pair in _dependencies)
                {
                    quests.Add(pair.Key);
                }
                
                return quests;
            }
        }

        private List<IQuest> StartNodes => (from pair in _dependencies where pair.Value.Count == 0 select pair.Key).ToList();
        
        [JsonProperty("Quest matrix")]
        private Dictionary<IQuest, List<IGraph.Connection>> _dependencies = new();

        public bool TryAdd(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            
            if (Has(quest)) return false;
            
            _dependencies.Add(quest, new List<IGraph.Connection>());
            Subscribe(quest);
            
            return true;
        }
        
        public bool TryRemove(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            if (!Has(quest)) return false;

            _dependencies.Remove(quest);

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

            var deps = _dependencies[from];
            _dependencies.Remove(from);

            foreach (var pair in _dependencies)
            {
                var index =  pair.Value.FindIndex(x => x.QuestId == from.Id);
                if (index < 0) continue;
                var connection = pair.Value[index];
                pair.Value.RemoveAt(index);
                pair.Value.Add(new IGraph.Connection(to.Id, connection.DependencyType));
            }
            
            _dependencies.Add(to, deps);
            Unsubscribe(from);
            Subscribe(to);
            
            return true;
        }

        public bool TryAddDependency(Id questId, Id dependencyNodeId, IDependencyType dependencyType)
        {
            return TryAddDependency(GetQuest(questId), GetQuest(dependencyNodeId), dependencyType);
        }

        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, IDependencyType dependencyType)
        {
            if (!quest.IsValid() || !dependencyQuest.IsValid()) return false;
            if (!Has(quest) || !Has(dependencyQuest)) return false;
            
            _dependencies[quest].Add(new IGraph.Connection(dependencyQuest.Id, dependencyType));
            
            return true;
        }

        public bool TryAddDependency(Id questId, Id dependencyNodeId, IGraph.DependencyType dependencyTypeType)
        {
            return dependencyTypeType switch
            {
                IGraph.DependencyType.Completion => TryAddDependency(
                    questId, dependencyNodeId, new Completion()),
                IGraph.DependencyType.Fail => TryAddDependency(
                    questId, dependencyNodeId, new Fail()),
                IGraph.DependencyType.Start => TryAddDependency(
                    questId, dependencyNodeId, new Start()),
                IGraph.DependencyType.Unneccesary => TryAddDependency(
                    questId, dependencyNodeId, new Unneccesary()),
                _ => false
            };
        }

        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, IGraph.DependencyType dependencyTypeType)
        {
            return TryAddDependency(quest.Id, dependencyQuest.Id, dependencyTypeType);
        }

        public bool TryRemoveDependencies(Id questId)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            var node = GetQuest(questId);
            
            _dependencies[node].Clear();

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
            
            var quest = GetQuest(questId);

            var dependents = GetDependentsQuests(quest);
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
            
            var quest = GetQuest(questId);
            
            var index = _dependencies[quest].FindIndex(x => x.QuestId == dependencyQuestId);
            if (index < 0) return false;
            
            _dependencies[quest].RemoveAt(index);
            
            return true;
        }

        public bool IsCyclic()
        {
            if (StartNodes.Count == 0) return true;
            
            var visited = new Dictionary<IQuest, bool>();
            var recStack = new Dictionary<IQuest, bool>();
            foreach (var quest in Quests)
            {
                visited.Add(quest, false);
                recStack.Add(quest, false);
            }
            
            foreach (var node in StartNodes)
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
            
            var quest = GetQuest(questId);

            foreach (var connection in _dependencies[quest])
            {
                list.Add( new ConnectionWithQuest(
                    dependentQuest: quest,
                    dependencyQuest: GetQuest(connection.QuestId),
                    dependency: connection.DependencyType));
            }

            return list;
        }

        public IEnumerable<ConnectionWithQuest> GetDependenciesQuests(IQuest quest)
        {
            if (!quest.IsValid()) return new List<ConnectionWithQuest>();
            if (!Has(quest)) return new List<ConnectionWithQuest>();

            return GetDependenciesQuests(quest.Id);
        }

        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(Id questId)
        {
            var list = new List<ConnectionWithQuest>();

            if (!questId.IsValid()) return list;
            if (!Has(questId)) return list;
            
            foreach (var pair in _dependencies)
            {
                var index = pair.Value.FindIndex(x => x.QuestId == questId);
                if (index < 0) continue;
                list.Add(new ConnectionWithQuest(
                    dependentQuest: pair.Key,
                    dependencyQuest: GetQuest(pair.Value[index].QuestId),
                    dependency: pair.Value[index].DependencyType));
            }
            
            return list;
        }

        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(IQuest quest)
        {
            return GetDependentsQuests(quest.Id);
        }
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {

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