using KarpikQuests;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.Saving;
using System;
using System.Linq;
using Task = KarpikQuests.QuestSample.Task;

namespace KarpikQuestsExample
{
    internal class SerializeLine : IQuestLine
    {
        private IQuestAggregator _aggregator = new QuestAggregator();

        private string _fileName = "Serialize.json";

        public IQuestAggregator Aggregator => _aggregator;

        public void Init()
        {
            ITaskBundle bundle = new TaskBundle();

            ITask firstTask = new Task();
            firstTask.Init("HelloTask", "Hi, you are so beautiful!", "Be like this always");

            ITask secondTask = new Task();
            secondTask.Init("WriteWordTask", "Now write 'Hello'", "Yes, you should do this to continue");

            ITask thirdTask = new Task();
            thirdTask.Init("LastTask", "Goodbye!", "That was awesome");

            bundle.Add(firstTask);
            bundle.Add(secondTask);
            bundle.Add(thirdTask);

            QuestBuilder.Start<Quest>("BasicLinearQuest", "This quest shows basic usage of this library", processor: null, completionType: null)
                .AddBundle(bundle)
                .OnComplete(OnQuestComplete) // quest.Completed += OnQuestComplete
                .AddToAggregatorOnCreate(_aggregator)
                .Create();

            QuestAggregatorSaver.Save(_aggregator, _fileName);
            _aggregator = null;
        }

        public void Start()
        {
            _aggregator = QuestAggregatorSaver.Load(_fileName);

            if (_aggregator == null)
            {
                Console.WriteLine("Something went wrong");
                return;
            }

            _aggregator.Start();
            var quest = _aggregator.Quests.First();

            var curTask = quest.TaskBundles.First().ElementAt(0);

            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);
            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                Console.ReadKey();
                curTask.TryToComplete();
            }

            curTask = quest.TaskBundles.First().ElementAt(1);

            Console.WriteLine();
            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);

            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                while (Console.ReadLine() != "Hello")
                {

                }
                curTask.TryToComplete();
            }

            curTask = quest.TaskBundles.First().ElementAt(2);

            Console.WriteLine();
            Console.WriteLine(curTask.Name);
            Console.WriteLine(curTask.Description);

            while (curTask.Status != ITask.TaskStatus.Completed)
            {
                Console.ReadKey();
                Console.WriteLine();
                curTask.TryToComplete();
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
