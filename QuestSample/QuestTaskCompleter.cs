using KarpikQuests.Interfaces;

namespace KarpikQuests.QuestSample
{
    public class QuestTaskCompleter : IQuestTaskCompleter
    {
        private readonly IQuestTaskCollection _tasks = new QuestTaskCollection();

        public bool Complete(IQuestTask task)
        {
            if (!_tasks.Has(task)) return false;

            return task.TryToComplete();
        }

        public IQuestTaskCollection CompleteAll()
        {
            IQuestTaskCollection tasks = new QuestTaskCollection();

            foreach (var task in _tasks)
            {
                if (!task.TryToComplete())
                {
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        public void Subscribe(IQuestTask task)
        {
            Unsubscribe(task);
            _tasks.Add(task);
        }

        public bool Unsubscribe(IQuestTask task)
        {
            if (!_tasks.Has(task)) return false;

            _tasks.Remove(task);
            return true;
        }
    }
}