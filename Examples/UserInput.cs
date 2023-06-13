using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskCompleters;
using System;

namespace KarpikQuests.Examples
{
    public class UserInput
    {
        private QuestBuilder _builder;
        private QuestAggregator _aggregator;
        private QuestTaskCompleter<string> _completer;
        private string _targetString = "";
        private string _targetValue = "UwU";

        public UserInput()
        {
            _aggregator = new QuestAggregator();
            _builder = new QuestBuilder(_aggregator);
            _completer = new QuestTaskCompleter<string>();
        }

        public void Work()
        {
            var task = new QuestTask();
            task.Init("UwU");

            void OnTaskComplete(IQuestTask task) => Console.WriteLine("Yay, you completed quest");

            task.Completed += OnTaskComplete;
            _completer.Subscribe(task, ref _targetString, _targetValue);

            var quest = _builder
                .Start<Quest>("Say UwU", "Impressive quest!")
                .AddTask(task)
                .Create();

            _aggregator.Start();

            Console.WriteLine(quest.Name);
            while (true)
            {
                _targetString = Console.ReadLine();
                _completer.Update();
                if (quest.IsCompleted())
                {
                    break;
                }
            }
            task.Completed -= OnTaskComplete;
        }
    }
}