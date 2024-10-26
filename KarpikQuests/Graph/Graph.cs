using Karpik.Quests.DependencyTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;
using Karpik.Quests.Saving;
using Karpik.Quests.Extensions;

namespace Karpik.Quests
{
    public class Graph : IGraph
    {
        public event Action<Quest>? QuestUnlocked;
        public event Action<Quest>? QuestCompleted;
        public event Action<Quest>? QuestFailed;
        public IEnumerable<Quest> Quests => _quests;
        [DoNotSerializeThis]
        public IEnumerable<Quest> StartQuests => new QuestCollection((_dependencies.Where(pair => pair.Value.Count == 0).Select(pair => GetQuest(pair.Key))).ToList());
    
        [SerializeThis("Quest_matrix")]
        private Dictionary<Id, List<Connection>> _dependencies = new();
        [SerializeThis("Quests")]
        private IQuestCollection _quests = new QuestCollection();

        public IEnumerable<Quest> StatusQuests(Status status)
        {
            var quests = new QuestCollection();
            for (int i = 0; i < _quests.Count; i++)
            {
                var quest = _quests[i];
                if (quest.Status == status)
                {
                    quests.Add(quest);
                }
            }

            return quests;
        }
    
        public bool TryAdd(Quest quest)
        {
            if (!quest.IsValid()) return false;
            
            if (Has(quest.Id)) return false;
            
            _dependencies.Add(quest.Id, new List<Connection>());
            _quests.Add(quest);
            quest.SetGraph(this);
            
            return true;
        }

        public bool TryRemove(Id quest)
        {
            if (!quest.IsValid()) return false;
            if (!Has(quest)) return false;

            _dependencies.Remove(quest);
            _quests.Remove(GetQuest(quest));

            var id = quest;
            foreach (var pair in _dependencies)
            {
                pair.Value.RemoveAll(x => x.QuestId == id);
            }

            return true;
        }

        public bool TryReplace(Quest from, Quest to)
        {
            if (!from.IsValid() || !to.IsValid()) return false;
            if (!Has(from.Id) || Has(to.Id)) return false;

            var deps = _dependencies[from.Id];
            _dependencies.Remove(from.Id);
            _quests.Remove(from);

            foreach (var pair in _dependencies)
            {
                var index =  pair.Value.FindIndex(x => x.QuestId == from.Id);
                if (index < 0) continue;
                var connection = pair.Value[index];
                pair.Value.RemoveAt(index);
                pair.Value.Add(new Connection(to.Id, connection.DependencyType));
            }
            
            _dependencies.Add(to.Id, deps);
            _quests.Add(to);
            
            return true;
        }

        public void Setup()
        {
            for (int i = 0; i < _quests.Count; i++)
            {
                _quests[i].Setup();
            }

            foreach (var quest in StartQuests)
            {
                quest.ForceUnlock();
            }
        }

        public void Clear()
        {
            _quests.Clear();
        }

        public bool Has(Id questId)
        {
            return GetQuest(questId).IsValid();
        }
    
        public Quest GetQuest(Id questId)
        {
            for (int i = 0; i < _quests.Count; i++)
            {
                if (Quests.ElementAt(i).Id == questId)
                {
                    return _quests[i];
                }
            }

            return Quest.Empty;
        }

        public bool TryAddDependency(Id questId, Id dependencyQuestId, IDependencyType dependencyType)
        {
            if (!questId.IsValid() || !dependencyQuestId.IsValid()) return false;
            if (!Has(questId) || !Has(dependencyQuestId)) return false;
            if (questId == dependencyQuestId) return false;
            
            _dependencies[questId].Add(new Connection(dependencyQuestId, dependencyType));
            
            return true;
        }

        public bool TryAddDependency(Id questId, Id dependencyQuestId, DependencyType dependencyType)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            return dependencyType switch
            {
                DependencyType.Completion => TryAddDependency(
                    questId, dependencyQuestId, new Completion()),
                DependencyType.Fail => TryAddDependency(
                    questId, dependencyQuestId, new Fail()),
                DependencyType.Unlocked => TryAddDependency(
                    questId, dependencyQuestId, new Unlocked()),
                _ => false
            };
        }

        public bool TryRemoveDependencies(Id questId)
        {
            if (!questId.IsValid()) return false;
            if (!Has(questId)) return false;
            
            if (!_dependencies.TryGetValue(questId, out var deps))
            {
                return false;
            }
            
            deps.Clear();
            return true;
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

        public bool TryRemoveDependency(Id questId, Id dependencyQuestId)
        {
            if (!questId.IsValid() || !dependencyQuestId.IsValid()) return false;
            if (!Has(questId) || !Has(dependencyQuestId)) return false;

            if (!_dependencies.TryGetValue(questId, out var deps))
            {
                return false;
            }
            
            var index = -1;
            for (int i = 0; i < deps.Count; i++)
            {
                var connection = deps[i];
                if (connection.QuestId == dependencyQuestId)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0) return false;
            
            deps.RemoveAt(index);
            
            return true;
        }

        public IEnumerable<QuestConnection> GetDependenciesQuests(Id questId)
        {
            var list = new List<QuestConnection>();
            
            if (!questId.IsValid()) return list;
            if (!Has(questId)) return list;
            
            if (!_dependencies.TryGetValue(questId, out var deps))
            {
                return list;
            }
            
            foreach (var connection in deps)
            {
                list.Add(new QuestConnection(
                    dependentId: questId,
                    dependencyId: connection.QuestId,
                    dependency: connection.DependencyType,
                    graph: this));
            }

            return list;
        }
    
        public IEnumerable<QuestConnection> GetDependentsQuests(Id questId)
        {
            var list = new List<QuestConnection>();
            
            if (!questId.IsValid()) return list;
            if (!Has(questId)) return list;
            
            foreach (var pair in _dependencies)
            {
                var index = -1;
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    var connection = pair.Value[i];
                    if (connection.QuestId == questId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0) continue;
                list.Add(new QuestConnection(
                    dependentId: pair.Key,
                    dependencyId: pair.Value[index].QuestId,
                    dependency: pair.Value[index].DependencyType,
                    graph: this));
            }
            
            return list;
        }

        public bool IsCyclic()
        {
            if (!StartQuests.Any()) return true;
            
            var visited = new Dictionary<Quest, bool>();
            var recStack = new Dictionary<Quest, bool>();
            foreach (var quest in Quests)
            {
                visited.Add(quest, false);
                recStack.Add(quest, false);
            }
            
            foreach (var node in StartQuests)
            {
                if (IsCyclicUtil(in node, visited, recStack)) return true;
            }

            return visited.Any(x => x.Value != true);
        }
    
        public void Dispose()
        {
            QuestUnlocked = null;
            QuestCompleted = null;
            QuestFailed = null;
        
            _dependencies.Clear();
            _dependencies = null;
        
            _quests.Clear();
            _quests = null;
        }

        void IGraph.InternalUpdate(Quest quest, bool inGraph)
        {
            switch (quest.Status)
            {
                case Status.Unlocked:
                    QuestUnlocked?.Invoke(quest);
                    break;
                
                case Status.Completed:
                    QuestCompleted?.Invoke(quest);
                    break;
                
                case Status.Failed:
                    QuestFailed?.Invoke(quest);
                    break;
                
                case Status.Locked:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!inGraph) return;
            
            var dependents = GetDependentsQuests(quest.Id);
            foreach (var connection in dependents)
            {
                if (!connection.Dependency.IsOk(quest)) continue;
                
                connection.DependentQuest.TryUnlock();
            }
        }
    
        private bool IsCyclicUtil(in Quest quest, Dictionary<Quest, bool> visited, Dictionary<Quest, bool> recStack)
        {
            if (recStack[quest]) return true;
            if (visited[quest]) return false;

            visited[quest] = true;
            recStack[quest] = true;
            
            var dependents = GetDependentsQuests(quest.Id);

            if (dependents.Any(dependent => IsCyclicUtil(dependent.DependentQuest, visited, recStack)))
            {
                return true;
            }

            recStack[quest] = false;

            return false;
        }
    }
}