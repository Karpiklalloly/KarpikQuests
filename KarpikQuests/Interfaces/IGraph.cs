using System.Collections.Generic;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface IGraph
    {
        public IQuestCollection Quests { get; }
        public IReadOnlyList<IGraphNode> Nodes { get; }
        
        public bool TryAdd(IGraphNode node);

        public bool TryRemove(IGraphNode node);
        public bool TryRemove(IQuest quest);
        public bool TryRemove(Id nodeId);

        public bool TrySetDependency(Id nodeId, Id dependencyNodeId, IDependencyType dependencyType);
        public bool TrySetDependency(IQuest quest, IQuest dependencyQuest, IDependencyType dependencyType);
        public bool TrySetDependency(Id nodeId, Id dependencyNodeId, DependencyType dependencyTypeType);
        public bool TrySetDependency(IQuest quest, IQuest dependencyQuest, DependencyType dependencyTypeType);
        
        public bool TryRemoveDependencies(Id nodeId);
        public bool TryRemoveDependencies(IQuest quest);
        public bool TryRemoveDependents(Id nodeId);
        public bool TryRemoveDependents(IQuest quest);
        
        public bool TryRemoveDependency(Id nodeId, Id dependencyNodeId);
        
        public IEnumerable<IGraphNode> GetDependenciesNodes(Id nodeId);
        public IEnumerable<IGraphNode> GetDependenciesNodes(IQuest quest);
        public IEnumerable<IGraphNode> GetDependentsNodes(Id nodeId);
        public IEnumerable<IGraphNode> GetDependentsNodes(IQuest quest);

        public bool IsCyclic();

        public bool Has(Id nodeId);
        public bool Has(IQuest quest);
        
        public IGraphNode? GetNode(Id nodeId);
        public IGraphNode? GetNode(IQuest quest);
        
        public enum DependencyType
        {
            Completion,
            Fail,
            Start,
            Unneccesary,
        }
    }
}