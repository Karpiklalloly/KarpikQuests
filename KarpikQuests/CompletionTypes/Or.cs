using Karpik.Quests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class Or : ICompletionType, ISingleton<Or>
    {
        private static Or _instance;
        public static Or Instance => _instance ??= new Or();

        private Or()
        {
            
        }
        
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            return !arr.Any() || arr.SelectMany(Bundle).Any(Predicates.TaskIsCompleted);
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            return !bundle.Any() || bundle.Any(Predicates.TaskIsCompleted);
        }

        private ITaskBundle Bundle(ITaskBundle bundle)
        {
            return bundle;
        }
    }
}
