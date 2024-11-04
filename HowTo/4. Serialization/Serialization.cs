using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Processors;
using Karpik.Quests.Requirements;
using Newtonsoft.Json;

namespace HowTo._4._Serialization;

public class Serialization : IProgram
{
    public void Run()
    {
        Graph graph = new Graph();
        Quest quest = new Quest(
            new Id("Variative"),
            "Variative quest",
            "You can choose",
            new And(),
            new Disorderly(),
            CreateLeatherQuest(),
            CreateNecessaryQuest(),
            CreateWaterQuest());
        graph.TryAdd(quest);
        
        Printer.Print(quest);

        var settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        var json = JsonConvert.SerializeObject(graph, settings);
        Console.WriteLine(json);
        if (!File.Exists("Quests.json")) File.Create("Quests.json").Close();
        File.WriteAllText("Quests.json", json);

        var graph2 = JsonConvert.DeserializeObject<Graph>(json, settings);
        Printer.Print(graph2);
    }
    
    private Quest CreateLeatherQuest()
    {
        var quest = QuestCreator.Create(
            new Id("Leather"),
            "Leather",
            "Choose leather",
            new Xor(),
            new Disorderly(),
            new Quest("Bear leather"),
            new Quest("Rabbit leather"),
            new Quest("Fox leather"));
        
        return quest;
    }

    private Quest CreateNecessaryQuest()
    {
        var quest = new Quest(
            new Id("Resources"),
            "Resources",
            "Sticks, Leaves, Stones",
            new And(),
            new Disorderly(),
            new Quest("Sticks"),
            new Quest("Leaves"),
            new Quest("Stones"));
        
        return quest;
    }

    private Quest CreateWaterQuest()
    {
        var quest = new Quest(
            new Id("Water"),
            "Water",
            "Select water",
            new Xor(),
            new Disorderly(),
            new Quest("Purified water"),
            new Quest("Salt water"),
            new QuestHasStatus(new Quest("Dirty water"), Status.Failed, Status.Completed));
        
        return quest;
    }
}