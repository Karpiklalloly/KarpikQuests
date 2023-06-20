using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskCompleters;
using System;

namespace KarpikQuests.Examples
{
    public class MultipleUserInput
    {
        private QuestBuilder _builder;
        private QuestAggregator _aggregator;
        private QuestTaskCompleter<string> _completer;
        private string _targetString = "";
        private string _targetValue1 = "UwU";
        private string _targetValue2 = "uWu";

        public MultipleUserInput()
        {
            _aggregator = new QuestAggregator();
            _builder = new QuestBuilder(_aggregator);
            _completer = new QuestTaskCompleter<string>();
        }

        public void Work()
        {
            var task1 = new QuestTask();
            task1.Init("1", _targetValue1);
            var task2 = new QuestTask();
            task2.Init("2", _targetValue2);

            _completer.Subscribe(task1, _targetValue1);
            _completer.Subscribe(task2, _targetValue2);

            var quest1 = _builder
                .Start<Quest>("Say " + _targetValue1, "Impressive quest!1")
                .AddTask(task1)
                .Create();
            var quest2 = _builder
                .Start<Quest>("Say " + _targetValue2, "Impressive quest!2")
                .AddTask(task2)
                .Create();

            _aggregator.TryAddDependence(quest2, quest1);
            _aggregator.Start();

            var currentQuest = quest1;
            task1.Completed += OnQuest1Complete;
            task2.Completed += OnQuest2Complete;

            void OnQuest1Complete(IQuestTask task)
            {
                currentQuest = quest2;
                Console.WriteLine("Yay, you completed quest1");
            }
            void OnQuest2Complete(IQuestTask task)
            {
                Console.WriteLine("Yay, you completed quest2");
            }

            while (true)
            {
                Console.WriteLine(currentQuest.Name);
                _targetString = Console.ReadLine();
                _completer.Update(_targetString);
                if (currentQuest.IsCompleted())
                {
                    break;
                }
            }

            task1.Completed -= OnQuest1Complete;
            task2.Completed -= OnQuest2Complete;
        }
    }
}