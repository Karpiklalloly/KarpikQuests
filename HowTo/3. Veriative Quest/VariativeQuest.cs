using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.Processors;
using Karpik.Quests.Requirements;

namespace HowTo._3._Veriative_Quest;

public class VariativeQuest : IProgram
{
    private Dictionary<string, Quest> _endQuests = new();
    
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
        graph.QuestCompleted += completedQuest =>
        {
            Console.WriteLine(completedQuest.Id);
            if (completedQuest.Id == quest.Id)
            {
                Console.WriteLine("You completed Quest");
            }
        };
        graph.QuestFailed += completedQuest =>
        {
            if (completedQuest.Id == quest.Id)
            {
                Console.WriteLine("You took wrong water!!!\nYou failed Quest");
            }
        };
        graph.Setup();
        
        while (!quest.IsFinished())
        {
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You can give:");
            Printer.Print(quest, Status.Completed);
                
            Console.WriteLine();
            var input = Input.String();
            

            if (!_endQuests.TryGetValue(input.ToLower(), out var inputQuest))
            {
                Console.WriteLine("No such resource");
                continue;
            }

            if (inputQuest.IsCompleted() && !inputQuest.SubQuests.Any())
            {
                Console.WriteLine("This resource has already given");
                continue;
            }

            if (inputQuest.IsCompleted())
            {
                Console.WriteLine("You can't give this resource because something similar was given");
                continue;
            }

            Console.WriteLine($"You gave {input}");
            inputQuest.TryComplete();
        }
    }

    private Quest CreateLeatherQuest()
    {
        var quest = new Quest(
            new Id("Leather"),
            "Leather",
            "Choose leather",
            new Xor(),
            new Disorderly(),
            new Quest("Bear leather"),
            new Quest("Rabbit leather"),
            new Quest("Fox leather"));

        foreach (var subQuest in quest.SubQuests)
        {
            _endQuests.Add(subQuest.Name.ToLower(), subQuest);
        }
        
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
        
        foreach (var subQuest in quest.SubQuests)
        {
            _endQuests.Add(subQuest.Name.ToLower(), subQuest);
        }
        
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
        
        foreach (var subQuest in quest.SubQuests)
        {
            _endQuests.Add(subQuest.Name.ToLower(), subQuest);
        }
        
        return quest;
    }
}