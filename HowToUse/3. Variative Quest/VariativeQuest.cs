using Karpik.Quests;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace HowToUse._3._Variative_Quest
{
    public class VariativeQuest : IProgram
    {
        private readonly IAggregator _aggregator = new Aggregator();
        private readonly IGraph _graph = new Graph();

        private readonly Dictionary<string, ITask> _tasks = new();
        private readonly Dictionary<string, ITaskBundle> _bundles = new();

        public void Run()
        {
            var leatherBundle = CreateLeatherBundle();
            var necessaryBundle = CreateNecessaryBundle();
            var waterBundle = CreateWaterBundle();

            QuestBuilder.Start<Quest>(
                    "VariativeQuest",
                    "Shows power of bundles",
                    new Disorderly(),
                    new And())
                .AddBundle(leatherBundle)
                .AddBundle(necessaryBundle)
                .AddBundle(waterBundle)
                .OnComplete(OnQuestComplete)
                .SetAggregator(_aggregator)
                .SetGraph(_graph)
                .Build();
            
            _aggregator.Start();
            var quest = _aggregator.Quests.ElementAt(0);

            Console.WriteLine("Let's start!");
            while (!quest.IsCompleted())
            {
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("You can give:");
                foreach (var bundle in quest.TaskBundles)
                {
                    if (bundle.IsCompleted()) continue;
                    foreach (var task in bundle)
                    {
                        if (task.CanBeCompleted)
                        {
                            Console.WriteLine(task.Name);
                        }
                    }
                }
                
                Console.WriteLine();
                var input = Console.ReadLine();
                if (input is null)
                {
                    continue;
                }

                if (!_tasks.TryGetValue(input.ToLower(), out var myTask))
                {
                    Console.WriteLine("No such resource");
                    continue;
                }

                if (myTask.IsCompleted())
                {
                    Console.WriteLine("This resource has already given");
                    continue;
                }

                var myBundle = _bundles[input.ToLower()];

                if (myBundle.IsCompleted())
                {
                    Console.WriteLine("You can't give this resource because something similar was given");
                    continue;
                }

                Console.WriteLine($"You gave {input}");
                myTask.TryComplete();
            }
        }

        private ITaskBundle CreateLeatherBundle()
        {
            ITask bearLeather = new Task();
            bearLeather.Init("Bear Leather");
            _tasks.Add(bearLeather.Name.ToLower(), bearLeather);

            ITask rabbitLeather = new Task();
            rabbitLeather.Init("Rabbit Leather");
            _tasks.Add(rabbitLeather.Name.ToLower(), rabbitLeather);

            ITask foxLeather = new Task();
            foxLeather.Init("Fox Leather");
            _tasks.Add(foxLeather.Name.ToLower(), foxLeather);

            ITaskBundle leatherBundle = new TaskBundle(new Or(),
                new Disorderly());
            leatherBundle.Add(bearLeather);
            leatherBundle.Add(rabbitLeather);
            leatherBundle.Add(foxLeather);
            leatherBundle.Completed += OnBundleComplete;
            foreach (var item in leatherBundle)
            {
                _bundles.Add(item.Name.ToLower(), leatherBundle);
            }

            return leatherBundle;
        }

        private ITaskBundle CreateNecessaryBundle()
        {
            ITask sticksNecessary = new Task();
            sticksNecessary.Init("Sticks");
            _tasks.Add(sticksNecessary.Name.ToLower(), sticksNecessary);

            ITask leavesNecessary = new Task();
            leavesNecessary.Init("Leaves");
            _tasks.Add(leavesNecessary.Name.ToLower(), leavesNecessary);

            ITask stonesNecessary = new Task();
            stonesNecessary.Init("Stones");
            _tasks.Add(stonesNecessary.Name.ToLower(), stonesNecessary);

            ITaskBundle necessaryBundle = new TaskBundle(new And(),
                new Disorderly());
            necessaryBundle.Add(sticksNecessary);
            necessaryBundle.Add(leavesNecessary);
            necessaryBundle.Add(stonesNecessary);
            necessaryBundle.Completed += OnBundleComplete;
            foreach (var item in necessaryBundle)
            {
                _bundles.Add(item.Name.ToLower(), necessaryBundle);
            }

            return necessaryBundle;
        }

        private ITaskBundle CreateWaterBundle()
        {
            ITask purifiedWater = new Task();
            purifiedWater.Init("Purified Water");
            _tasks.Add(purifiedWater.Name.ToLower(), purifiedWater);

            ITask saltWater = new Task();
            saltWater.Init("Salt Water");
            _tasks.Add(saltWater.Name.ToLower(), saltWater);

            ITask dirtyWater = new Task();
            dirtyWater.Init("Dirty Water");
            _tasks.Add(dirtyWater.Name.ToLower(), dirtyWater);

            ITaskBundle waterBundle = new TaskBundle(new Or(),
                new Disorderly());
            waterBundle.Add(purifiedWater);
            waterBundle.Add(saltWater);
            waterBundle.Add(dirtyWater);
            waterBundle.Completed += OnBundleComplete;
            foreach (var item in waterBundle)
            {
                _bundles.Add(item.Name.ToLower(), waterBundle);
            }

            return waterBundle;
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
