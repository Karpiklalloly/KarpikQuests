using Karpik.Quests;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.ID;
using Task = Karpik.Quests.QuestSample.Task;

namespace HowToUse._2._LinearQuest
{
    public class SimpleQuest : IProgram
    {
        public void Run()
        {
            IAggregator aggregator = new Aggregator();
            IGraph graph = new Graph();
            
            ITaskBundle bundle = new TaskBundle();

            ITask firstTask = new Task(new Id("HelloTask"));
            firstTask.Init("Hi, you are so beautiful!", "Be like this always");

            ITask secondTask = new Task(new Id("WriteWordTask"));
            secondTask.Init("Now write 'Hello'", "Yes, you should do this to continue");

            ITask thirdTask = new Task(new Id("LastTask"));
            thirdTask.Init("Goodbye!", "That was awesome");

            bundle.Add(firstTask);
            bundle.Add(secondTask);
            bundle.Add(thirdTask);

            QuestBuilder.Start<Quest>("Simple Quest", "This quest shows basic usage of this library")
                .AddBundle(bundle)
                .OnComplete(OnQuestComplete)
                .SetAggregator(aggregator)
                .SetGraph(graph)
                .Build();
            
            aggregator.Start();
            var quest = aggregator.Quests.First();

            var curTask = quest.TaskBundles.First().Tasks.First();

            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);
            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                Console.ReadKey();
                curTask.TryComplete();
            }

            curTask = quest.TaskBundles.First().Tasks.ElementAt(1);

            Console.WriteLine();
            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);
            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                while (true)
                {
                    if (Console.ReadLine() == "Hello")
                    {
                        break;
                    }
                }
                curTask.TryComplete();
            }

            curTask = quest.TaskBundles.First().Tasks.ElementAt(2);

            Console.WriteLine();
            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);
            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                Console.ReadKey();
                Console.WriteLine();
                curTask.TryComplete();
            }
            
        }

        private void OnQuestComplete(IQuest quest)
        {
            Console.WriteLine($"Quest {quest.Name} completed!");
        }
    }
}
