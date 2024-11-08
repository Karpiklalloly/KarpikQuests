using Karpik.Quests;

namespace HowTo._5._Custom_Quest;

public class PlayerHasItemQuest : Quest
{
    public PlayerHasItemQuest(Player player, string name) : base($"Player has {name}")
    {
        Add(new ItemRequirement(player, name));
    }
}