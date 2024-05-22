using Karpik.Quests;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Saving;
using System;
using System.Linq;
using Karpik.Quests.ID;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Example
{
    internal class SerializeLine : IQuestLine
    {
        private IAggregator _aggregator = new Aggregator();

        private string _fileName = "Serialize.json";

        public IAggregator Aggregator => _aggregator;

        public void Init()
        {
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

            QuestBuilder.Start<Quest>(
                    "BasicLinearQuest",
                    "This quest shows basic usage of this library",
                    processor: null,
                    completionType: null)
                .AddBundle(bundle)
                .OnComplete(OnQuestComplete) // quest.Completed += OnQuestComplete
                .SetAggregator(_aggregator)
                .Build();
            
            QuestAggregatorSaver.Serializer = new JsonResolver<IAggregator>();
            QuestAggregatorSaver.Save(_aggregator, _fileName);
            _aggregator = null;
        }

        public void Start()
        {
            var aggregator = QuestAggregatorSaver.Load(_fileName);

            if (aggregator is null)
            {
                Console.WriteLine("Something went wrong");
                return;
            }

            _aggregator = aggregator;

            _aggregator.Start();
            var quest = _aggregator.Quests.First();

            var curTask = quest.TaskBundles.First().Tasks.ElementAt(0);

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
                while (Console.ReadLine() != "Hello")
                {

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
            Console.WriteLine("Post quest completed");
        }

        public void DeInit()
        {
            _aggregator.Clear();
        }

        private void OnQuestComplete(IQuest quest)
        {
            Console.WriteLine($"Quest {quest.Name} completed!");
        }
    }
}
