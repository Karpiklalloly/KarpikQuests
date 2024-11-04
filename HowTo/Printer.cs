using Karpik.Quests;

namespace HowTo;

public static class Printer
{
    public static void Print(Quest quest, params Status[] blackFilter)
    {
        Print(quest, 0, blackFilter);
    }

    public static void Print(Graph graph, params Status[] blackFilter)
    {
        var quests = graph.Quests;
        Console.WriteLine("Graph:");
        foreach (var quest in quests)
        {
            Print(quest, 0, blackFilter);
        }
    }

    public static void SubQuests(Quest quest)
    {
        int i = 1;
        foreach (var subQuest in quest.SubQuests)
        {
            Console.Write(i++);
            Console.Write(" ");
            Console.WriteLine(ToString(subQuest));
        }
    }

    public static string ToString(Quest quest)
    {
        return $"{quest.Name}: {quest.Description} ({quest.Status})";
    }

    private static void Print(Quest quest, int depth, params Status[] blackFilter)
    {
        if (blackFilter.Contains(quest.Status))
        {
            return;
        }
        
        Console.Write("|");
        for (int i = 0; i < depth; i++)
        {
            Console.Write("-");
        }
        Console.WriteLine(ToString(quest));
        foreach (var subQuest in quest.SubQuests)
        {
            
            Print(subQuest, depth + 1, blackFilter);
        }
    }
}