using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuest : IEquatable<IQuest>, IDisposable, ICloneable
    {
        public string Key { get; }
        public string Name { get; }
        public string Description { get; }

        public ITaskBundleCollection TaskBundles { get; }
        public IQuestCompletionType CompletionType { get; }
        public IQuestTaskProcessorType QuestTaskProcessor { get; }

        public IQuestStatus Status { get; }

        public event Action<IQuest> Started;
        public event Action<IQuest, IQuestTask> Updated;
        public event Action<IQuest> Completed;

        public void Reset();
        public void Start();

        protected internal void Init(string key, string name, string description);
        protected internal void SetKey(string key);
        protected internal void AddTask(IQuestTask task);
        protected internal void RemoveTask(IQuestTask task);
        protected internal void AddBundle(ITaskBundle bundle);
        protected internal void RemoveBundle(ITaskBundle bundle);
        protected internal void OnTaskComplete(IQuestTask task);
        protected internal void SetCompletionType(IQuestCompletionType completionType);
        protected internal void SetTaskProcessorType(IQuestTaskProcessorType processor);
    }
}