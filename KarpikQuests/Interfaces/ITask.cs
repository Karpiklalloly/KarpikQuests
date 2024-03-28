using System;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface ITask : IInitable, IEquatable<ITask>, ICloneable
    {
        public event Action<ITask> Started;
        public event Action<ITask> Completed;
        public event Action<ITask> Failed;

        public Id Id { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskStatus Status { get; }
        public bool CanBeCompleted { get; }

        public void Init(string name, string description = "");
        public void Setup();
        public void Start();
        public void Reset();
        public bool TryComplete();
        public void ForceComplete();
        public bool TryFail();
        public void ForceFail();

        public enum TaskStatus
        {
            UnStarted = 1,
            Started = 2,
            Failed = 3,
            Completed = 4,
            
        }
    }
}