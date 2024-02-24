using KarpikQuests;
using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.CompletionTypes;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;
using Task = KarpikQuests.QuestSample.Task;
using System.Collections.Generic;
using System;
using System.Linq;

namespace KarpikQuestsExample
{
    internal class BasicVariativeQuest : IQuestLine
    {
        public IQuestAggregator Aggregator { get; private set; }

        private readonly Dictionary<string, ITask> _tasks = new Dictionary<string, ITask>();
        private readonly Dictionary<string, ITaskBundle> _bundles = new Dictionary<string, ITaskBundle>();

        public BasicVariativeQuest()
        {
            Aggregator = new QuestAggregator();
        }

        public void DeInit()
        {
            Aggregator.Clear();
        }

        public void Init()
        {
            Task bearLeather = new Task();
            bearLeather.Init(KeyGenerator.GenerateKey(), "Bear Leather");
            _tasks.Add(bearLeather.Name.ToLower(), bearLeather);

            Task rabbitLeather = new Task();
            rabbitLeather.Init(KeyGenerator.GenerateKey(), "Rabbit Leather");
            _tasks.Add(rabbitLeather.Name.ToLower(), rabbitLeather);

            Task foxLeather = new Task();
            foxLeather.Init(KeyGenerator.GenerateKey(), "Fox Leather");
            _tasks.Add(foxLeather.Name.ToLower(), foxLeather);

            ITaskBundle leatherBundle = new TaskBundle(new OR(),
                new Disorderly())
            {
                bearLeather,
                rabbitLeather,
                foxLeather
            };
            leatherBundle.Completed += OnBundleComplete;
            foreach (var item in leatherBundle)
            {
                _bundles.Add(item.Name.ToLower(), leatherBundle);
            }


            Task sticksNeccesary = new Task();
            sticksNeccesary.Init(KeyGenerator.GenerateKey(), "Sticks");
            _tasks.Add(sticksNeccesary.Name.ToLower(), sticksNeccesary);

            Task leavesNeccesary = new Task();
            leavesNeccesary.Init(KeyGenerator.GenerateKey(), "Leaves");
            _tasks.Add(leavesNeccesary.Name.ToLower(), leavesNeccesary);

            Task stonesNeccesary = new Task();
            stonesNeccesary.Init(KeyGenerator.GenerateKey(), "Stones");
            _tasks.Add(stonesNeccesary.Name.ToLower(), stonesNeccesary);

            ITaskBundle neccesaryBundle = new TaskBundle(new AND(),
                new Disorderly())
            {
                sticksNeccesary,
                leavesNeccesary,
                stonesNeccesary
            };
            neccesaryBundle.Completed += OnBundleComplete;
            foreach (var item in neccesaryBundle)
            {
                _bundles.Add(item.Name.ToLower(), neccesaryBundle);
            }


            Task purifiedWater = new Task();
            purifiedWater.Init(KeyGenerator.GenerateKey(), "Purified Water");
            _tasks.Add(purifiedWater.Name.ToLower(), purifiedWater);

            Task saltWater = new Task();
            saltWater.Init(KeyGenerator.GenerateKey(), "Salt Water");
            _tasks.Add(saltWater.Name.ToLower(), saltWater);

            Task dirtyWater = new Task();
            dirtyWater.Init(KeyGenerator.GenerateKey(), "Dirty Water");
            _tasks.Add(dirtyWater.Name.ToLower(), dirtyWater);

            TaskBundle waterBundle = new TaskBundle(new OR(),
                new Disorderly())
            {
                purifiedWater,
                saltWater,
                dirtyWater
            };
            waterBundle.Completed += OnBundleComplete;

            foreach (var item in waterBundle)
            {
                _bundles.Add(item.Name.ToLower(), waterBundle);
            }

            QuestBuilder.Start<Quest>("VariativeQuest", "Shows power of bundles", new Disorderly(), new AND())
                .AddBundle(leatherBundle)
                .AddBundle(neccesaryBundle)
                .AddBundle(waterBundle)
                .OnComplete(OnQuestComplete)
                .AddToAggregatorOnCreate(Aggregator)
                .Create();
        }

        public void Start()
        {
            Aggregator.Start();
            var quest = Aggregator.Quests[0];

            Console.WriteLine("You can give:");
            foreach (var item in _tasks)
            {
                Console.WriteLine(item.Key);
            }

            while (!quest.IsCompleted())
            {
                Console.WriteLine();
                var input = Console.ReadLine();
                if (input is null)
                {
                    continue;
                }

                if (!_tasks.TryGetValue(input.ToLower(), out var task))
                {
                    Console.WriteLine("No such resource");
                    continue;
                }

                if (task.Status == ITask.TaskStatus.Completed)
                {
                    Console.WriteLine("This resource has already given");
                    continue;
                }

                var bundle = _bundles[input.ToLower()];

                if (bundle.IsCompleted)
                {
                    Console.WriteLine("You can't give this resource because something similar was given");
                    continue;
                }

                task.TryToComplete();
            }
        }

        private void OnQuestComplete(IQuest quest)
        {
            Console.WriteLine($"Quest {quest.Name} completed!");
        }

        private void OnBundleComplete(ITaskBundle bundle)
        {
            Console.WriteLine($"Bundle completed!");
        }

    }
}
