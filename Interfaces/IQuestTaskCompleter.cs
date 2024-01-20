﻿namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskCompleter
    {
        public void Subscribe(IQuestTask task);
        public bool Unsubscribe(IQuestTask task);

        public bool Complete(IQuestTask task);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Uncompleted tasks</returns>
        public IQuestTaskCollection CompleteAll();
    }
}