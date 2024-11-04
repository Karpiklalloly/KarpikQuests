﻿using Karpik.Quests.Interfaces;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class Or : ICompletionType
    {
        public Status Check(IEnumerable<IRequirement> quests)
        {
            if (!quests.Any()) return Status.Completed;
            
            var satisfied = quests.Count(Predicates.IsSatisfied);
            var failed = quests.Count(Predicates.IsRuined);
            
            if (satisfied > 0) return Status.Completed;
            if (failed > 0 ) return Status.Failed;
            
            return Status.Locked;
        }
    }
}