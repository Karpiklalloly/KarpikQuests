using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Processors;

namespace HowTo._1._Begin;

public class BeginAlternative : IProgram
{
    public void Run()
    {
        // Create epic quest
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
        
        // Add epic quest to graph
        var graph = new Graph();
        graph.TryAdd(mainQuest);
        
        Printer.Print(mainQuest);
    }
}