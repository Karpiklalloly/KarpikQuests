using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace HowToUse._1._Begin;

public class Begin : IProgram
{
    public void Run()
    {
        //Task
        ITask task = new Task();
        task.Init("My task name", "My task Description");
        
        //TaskBundle
        ITaskBundle taskBundle = new TaskBundle();
        taskBundle.Add(task);
        
        //Quest
        IQuest quest = new Quest();
        quest.Init("My quest name", 
            "My quest description",
            new TaskBundleCollection(),
            new And(),
            new Disorderly());
        quest.Add(taskBundle);
        
        //Graph
        IGraph graph = new Graph();
        graph.TryAdd(quest);
        
        //Aggregator
        IAggregator aggregator = new Aggregator();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, new Quest());
    }
}