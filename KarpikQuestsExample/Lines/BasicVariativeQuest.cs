using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;
using System.Collections.Generic;
using System;

namespace Karpik.Quests.Example
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
            bearLeather.Init("Bear Leather");
            _tasks.Add(bearLeather.Name.ToLower(), bearLeather);

            Task rabbitLeather = new Task();
            rabbitLeather.Init("Rabbit Leather");
            _tasks.Add(rabbitLeather.Name.ToLower(), rabbitLeather);

            Task foxLeather = new Task();
            foxLeather.Init("Fox Leather");
            _tasks.Add(foxLeather.Name.ToLower(), foxLeather);

            ITaskBundle leatherBundle = new TaskBundle(new Or(), 
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


            Task sticksNecessary = new Task();
            sticksNecessary.Init("Sticks");
            _tasks.Add(sticksNecessary.Name.ToLower(), sticksNecessary);

            Task leavesNecessary = new Task();
            leavesNecessary.Init("Leaves");
            _tasks.Add(leavesNecessary.Name.ToLower(), leavesNecessary);

            Task stonesNecessary = new Task();
            stonesNecessary.Init("Stones");
            _tasks.Add(stonesNecessary.Name.ToLower(), stonesNecessary);

            ITaskBundle necessaryBundle = new TaskBundle(new And(), 
                new Disorderly())
            {
                sticksNecessary,
                leavesNecessary,
                stonesNecessary
            };
            necessaryBundle.Completed += OnBundleComplete;
            foreach (var item in necessaryBundle)
            {
                _bundles.Add(item.Name.ToLower(), necessaryBundle);
            }


            Task purifiedWater = new Task();
            purifiedWater.Init("Purified Water");
            _tasks.Add(purifiedWater.Name.ToLower(), purifiedWater);

            Task saltWater = new Task();
            saltWater.Init("Salt Water");
            _tasks.Add(saltWater.Name.ToLower(), saltWater);

            Task dirtyWater = new Task();
            dirtyWater.Init("Dirty Water");
            _tasks.Add(dirtyWater.Name.ToLower(), dirtyWater);

            TaskBundle waterBundle = new TaskBundle(new Or(), 
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

            QuestBuilder.Start<Quest>(
                    "VariativeQuest",
                    "Shows power of bundles",
                    new Disorderly(),
                    new And())
                .AddBundle(leatherBundle)
                .AddBundle(necessaryBundle)
                .AddBundle(waterBundle)
                .OnComplete(OnQuestComplete)
                .SetAggregator(Aggregator)
                .Build();
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

                if (bundle.IsCompleted())
                {
                    Console.WriteLine("You can't give this resource because something similar was given");
                    continue;
                }

                task.TryComplete();
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
