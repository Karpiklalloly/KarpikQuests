using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class And : ICompletionType, ISingleton<And>
    {
        private static And _instance;
        public static And Instance => _instance ??= new And();

        private And()
        {
            
        }
        
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            var enumerable = bundles as ITaskBundle[] ?? bundles.ToArray();
            return !enumerable.Any() || Array.TrueForAll(enumerable, Predicates.BundleIsCompleted);
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            return !bundle.Any() || bundle.All(Predicates.TaskIsCompleted);
        }
    }
}
