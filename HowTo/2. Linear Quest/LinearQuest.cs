using Karpik.Quests;
using Karpik.Quests.Processors;

namespace HowTo._2._Linear_Quest;

public class LinearQuest : IProgram
{
    public void Run()
    {
        var mainQuest = new Quest(
            new Id("Main"),
            "Main Quest",
            "Go and kill the dragon!",
            new And(),
            new Orderly(),
            new Quest(
                new Id("Go"),
                "Go",
                "You should take your equipment",
                new And(),
                new Disorderly(),
                new Quest(
                    "Sword"),
                new Quest(
                    "Shield"),
                new Quest(
                    "Potion")),
            new Quest(
                new Id("Kill"),
                "Kill",
                "Kill the dragon!",
                new Or(),
                new Disorderly(),
                new Quest(
                    "Kill with sword",
                    "Kill the dragon with sword!"),
                new Quest(
                    "Kill with potion",
                    "Kill the dragon with potion!"))
        );
        
        var graph = new Graph();
        graph.QuestCompleted += OnQuestCompleted;
        graph.TryAdd(mainQuest);
        
        graph.Setup();
        
        var curQuest = mainQuest.SubQuests.ElementAt(0);
        while (!curQuest.IsCompleted())
        {
            Printer.SubQuests(curQuest);
            var input = Input.Int() - 1;
            if (0 > input || input >= curQuest.SubQuests.Count()) continue;
            
            var subQuest = curQuest.SubQuests.ElementAt(input);
            subQuest.TryComplete();
        }
        
        curQuest = mainQuest.SubQuests.ElementAt(1);
        while (!curQuest.IsCompleted())
        {
            Printer.SubQuests(curQuest);
            var input = Input.Int() - 1;
            if (0 > input || input >= curQuest.SubQuests.Count()) continue;
            
            var subQuest = curQuest.SubQuests.ElementAt(input);
            subQuest.TryComplete();
        }
    }

    private void OnQuestCompleted(Quest quest)
    {
        Console.WriteLine();
        Console.WriteLine($"{quest.Name} completed");
        Console.WriteLine();
    }
}