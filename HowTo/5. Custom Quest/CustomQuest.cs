using Karpik.Quests;
using Karpik.Quests.Processors;

namespace HowTo._5._Custom_Quest;

public class CustomQuest : IProgram
{
    public void Run()
    {
        Graph graph = new Graph();
        
        Player player = new Player();
        player.Items.Add(new Sword());

        Quest baseQuest = new Quest(
            Id.NewId(),
            "Items",
            null,
            new And(),
            new Disorderly(),
            new PlayerHasItemQuest(player, "Sword"),
            new PlayerHasItemQuest(player, "Apple")
        );
        graph.TryAdd(baseQuest);
        graph.QuestCompleted += OnQuestCompleted;
        graph.Setup();
        
        Printer.Print(baseQuest);

        baseQuest.TryComplete();
        Printer.Print(baseQuest);
        
        player.Items.Add(new Apple());
        baseQuest.TryComplete();
        Printer.Print(baseQuest);
    }

    private void OnQuestCompleted(Quest quest)
    {
        Console.WriteLine($"{quest.Name} Completed");
    }
}