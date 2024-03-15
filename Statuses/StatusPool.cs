using System;
using System.Collections.Generic;
using KarpikQuests.Interfaces;

namespace KarpikQuests
{
    public static class StatusPool
    {
        private static readonly Dictionary<Type, IStatus> _statuses = new Dictionary<Type, IStatus>();

        public static T Get<T>() where T : IStatus, new()
        {
            if (!_statuses.ContainsKey(typeof(T)))
            {
                _statuses.Add(typeof(T), new T());
            }

            return (T)_statuses[typeof(T)];
        }
        
        public static void Return<T>(T status) where T : IStatus
        {

        }
    }
}
