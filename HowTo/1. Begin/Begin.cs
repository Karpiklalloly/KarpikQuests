using Karpik.Quests;
using Karpik.Quests.Processors;

namespace HowTo._1._Begin;

public class Begin : IProgram
{
    public void Run()
    {
        // Create epic quest
        var mainQuest = new Quest(
            new Id("Main"),
            "Main Quest",
            "Go and kill the dragon!",
            new And(),
            new Orderly());

        var go = new Quest(
            new Id("Go"),
            "Go",
            "You should take your equipment",
            new And(),
            new Disorderly());

        var e1 = new Quest(
            "Sword",
            string.Empty);
        
        var e2 = new Quest(
            "Shield",
            string.Empty);
        
        var e3 = new Quest(
            "Potion",
            string.Empty);
        
        go.Add(e1, e2, e3);
        
        var kill = new Quest(
            new Id("Kill"),
            "Kill",
            "Kill the dragon!",
            new Or(),
            new Disorderly());
        
        var killWithSword = new Quest(
            "Kill with sword",
            "Kill the dragon with sword!");
        
        var killWithPotion = new Quest(
            "Kill with potion",
            "Kill the dragon with potion!");
        
        kill.Add(killWithSword, killWithPotion);
        
        mainQuest.Add(go, kill);
        
        // Add epic quest to graph
        var graph = new Graph();
        graph.TryAdd(mainQuest);
        
        Printer.Print(mainQuest);
    }
}