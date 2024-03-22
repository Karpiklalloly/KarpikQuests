using System;
using System.Collections.Generic;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests
{
    public class StatusPool: ISingleton<StatusPool>, IPool<IStatus>
    {
        private readonly Dictionary<Type, IStatus> _statuses = new Dictionary<Type, IStatus>();

        private static StatusPool _instance;
        public static StatusPool Instance => _instance ??= new StatusPool();

        private StatusPool()
        {
            
        }


        public TGet Pull<TGet>() where TGet : IStatus
        {
            if (!_statuses.ContainsKey(typeof(TGet)))
            {
                _statuses.Add(typeof(TGet), Activator.CreateInstance<TGet>());
            }

            return (TGet)_statuses[typeof(TGet)];
        }

        public void Push(IStatus instance)
        {
            if (_statuses.ContainsKey(instance.GetType()))
            {
                return;
            }
            
            _statuses.Add(instance.GetType(), instance);
        }
    }
}
