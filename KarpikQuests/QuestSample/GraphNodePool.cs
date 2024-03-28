using System.Collections.Generic;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.QuestSample;

public class GraphNodePool : ISingleton<GraphNodePool>, IPool<GraphNode>
{
    private Stack<GraphNode> _nodes = new Stack<GraphNode>();

    private static GraphNodePool _instance;
    public static GraphNodePool Instance => _instance ??= new GraphNodePool();
    
    public TGet Pull<TGet>() where TGet : GraphNode
    {
        if (_nodes.Count == 0)
        {
            _nodes.Push(new GraphNode());
        }

        return (TGet)_nodes.Pop();
    }
    
    public TGet Pull<TGet>(IQuest quest) where TGet : GraphNode
    {
        if (_nodes.Count == 0)
        {
            _nodes.Push(new GraphNode(quest));
        }

        return (TGet)_nodes.Pop();
    }

    public void Push(GraphNode instance)
    {
        _nodes.Push(instance);
    }
}