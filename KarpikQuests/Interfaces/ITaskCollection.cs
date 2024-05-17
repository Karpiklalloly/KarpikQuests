using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface ITaskCollection : IReadOnlyTaskCollection, IList<ITask>
    {
        
    }
}