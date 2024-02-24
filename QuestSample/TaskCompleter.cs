using KarpikQuests.Interfaces;

namespace KarpikQuests.QuestSample
{
    public class TaskCompleter : ITaskCompleter
    {
        private readonly ITaskCollection _tasks = new TaskCollection();

        public bool TryComplete(ITask task)
        {
            if (!_tasks.Has(task)) return false;

            return task.TryToComplete();
        }

        public ITaskCollection TryCompleteAll()
        {
            ITaskCollection tasks = new TaskCollection();

            foreach (var task in _tasks)
            {
                if (!task.TryToComplete())
                {
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        public void Subscribe(ITask task)
        {
            Unsubscribe(task);
            _tasks.Add(task);
        }

        public bool Unsubscribe(ITask task)
        {
            if (!_tasks.Has(task)) return false;

            _tasks.Remove(task);
            return true;
        }
    }
}