using System;
using System.Collections.Generic;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface ITaskBundle : ICollection<ITask>, ICloneable, IEquatable<ITaskBundle>
    {
        public event Action<ITaskBundle>? Updated;
        public event Action<ITaskBundle>? Completed;
        public event Action<ITaskBundle>? Failed;

        public IStatus Status { get; }
        public IReadOnlyTaskCollection Tasks { get; }

        public bool Has(ITask task);
        public bool Has(Id taskKey);

        public void Setup();

        public void Reset();
        public void ResetFirst();

        public void ClearTasks();
    }
}
