using System;
using System.Collections.Generic;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.CompletionTypes
{
    public class CompletionTypesPool : ISingleton<CompletionTypesPool>, IPool<ICompletionType>
    {
        private readonly Dictionary<Type, Stack<ICompletionType>> _completionTypes = new Dictionary<Type, Stack<ICompletionType>>();
        
        private static CompletionTypesPool _instance;
        public static CompletionTypesPool Instance => _instance ??= new CompletionTypesPool();

        private CompletionTypesPool()
        {
            
        }
        
        public TGet Pull<TGet>() where TGet : ICompletionType
        {
            var key = typeof(TGet);
            if (!_completionTypes.ContainsKey(typeof(TGet)))
            {
                _completionTypes.Add(key, new Stack<ICompletionType>());
            }

            if (_completionTypes[key].Count == 0)
            {
                _completionTypes[typeof(TGet)].Push(Activator.CreateInstance<TGet>());
            }
            
            return (TGet)_completionTypes[typeof(TGet)].Pop();
        }

        public void Push(ICompletionType instance)
        {
            var key = instance.GetType();
            if (!_completionTypes.ContainsKey(instance.GetType()))
            {
                _completionTypes.Add(key, new Stack<ICompletionType>());
            }
            
            _completionTypes[key].Push(instance);
        }
    }
}