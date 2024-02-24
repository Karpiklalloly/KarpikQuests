namespace KarpikQuests.Interfaces
{
    public interface ITaskCompleter
    {
        public void Subscribe(ITask task);
        public bool Unsubscribe(ITask task);

        public bool TryComplete(ITask task);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Uncompleted tasks</returns>
        public ITaskCollection TryCompleteAll();
    }
}