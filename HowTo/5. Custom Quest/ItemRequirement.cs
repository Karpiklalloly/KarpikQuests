using Karpik.Quests;

namespace HowTo._5._Custom_Quest;

public class ItemRequirement : IRequirement
{
    private readonly Player _player;
    private readonly string _item;
    
    public ItemRequirement(Player player, string item)
    {
        _player = player;
        _item = item;
    }
    
    public bool IsSatisfied()
    {
        return _player.Items.Any(x => x.Name == _item);
    }

    public bool IsRuined()
    {
        return false;
    }

    public void SetGraph(IGraph graph)
    {
        
    }
}