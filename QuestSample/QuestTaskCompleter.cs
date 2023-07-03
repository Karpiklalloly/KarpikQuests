using KarpikQuests.Interfaces;

namespace KarpikQuests.QuestSample
{
    public class QuestTaskCompleter : IQuestTaskCompleter
    {
        private IQuestTaskCollection _tasks = new QuestTaskCollection();

        public bool Complete(IQuestTask task)
        {
            if (!_tasks.Contains(task))
            {
                return false;
            }
            return task.TryToComplete();
        }

        public void CompleteAll()
        {
            foreach (var task in _tasks)
            {
                task.TryToComplete();
            }
        }

        public void Subscribe(IQuestTask task)
        {
            Unsubscribe(task);
            _tasks.Add(task);
        }

        public bool Unsubscribe(IQuestTask task)
        {
            if (!_tasks.Contains(task))
            {
                return false;
            }

            _tasks.Remove(task);
            return true;
        }
    }
}
