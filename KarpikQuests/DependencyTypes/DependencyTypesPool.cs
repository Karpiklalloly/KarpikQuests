using System;
using System.Collections.Generic;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.DependencyTypes
{
    public class DependencyTypesPool : ISingleton<DependencyTypesPool>, IPool<IDependencyType>
    {
        private readonly Dictionary<Type, IDependencyType> _dependencyTypes = new Dictionary<Type, IDependencyType>();
        
        private static DependencyTypesPool _instance;
        public static DependencyTypesPool Instance => _instance ??= new DependencyTypesPool();

        private DependencyTypesPool()
        {
            
        }
    
        public TGet Pull<TGet>() where TGet : IDependencyType
        {
            if (!_dependencyTypes.ContainsKey(typeof(TGet)))
            {
                _dependencyTypes.Add(typeof(TGet), Activator.CreateInstance<TGet>());
            }

            return (TGet)_dependencyTypes[typeof(TGet)];
        }

        public void Push(IDependencyType instance)
        {
            if (_dependencyTypes.ContainsKey(instance.GetType()))
            {
                return;
            }
            
            _dependencyTypes.Add(instance.GetType(), instance);
        }
    }
}