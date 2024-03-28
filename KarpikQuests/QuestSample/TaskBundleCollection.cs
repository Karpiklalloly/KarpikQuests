﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    public sealed class TaskBundleCollection : ITaskBundleCollection
    {
        [JsonProperty("Bundles")]
        private readonly List<ITaskBundle> _bundles = new List<ITaskBundle>();
        
        #region list
        
        [JsonIgnore]
        public int Count => _bundles.Count;

        [JsonIgnore]
        public bool IsReadOnly => false;

        public void Add(ITaskBundle item)
        {
            if (Has(item)) return;
            
            _bundles.Add(item);
        }

        public void Clear()
        {
            _bundles.Clear();
        }
        
        public bool Contains(ITaskBundle item)
        {
            return Has(item);
        }

        public bool Has(ITask task)
        {
            return _bundles.Exists((bundle) =>  bundle.Has(task));
        }

        public bool Has(ITaskBundle bundle)
        {
            foreach (var anotherTaskBundle in _bundles)
            {
                if (anotherTaskBundle.Equals(bundle))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(ITaskBundle[] array, int arrayIndex)
        {
            _bundles.CopyTo(array, arrayIndex);
        }
        
        public bool Remove(ITaskBundle item)
        {
            if (!Has(item)) return false;
            
            var index = IndexOf(item);
            if (index < 0) return false;
            _bundles.RemoveAt(index);
            
            return true;
        }
        
        public int IndexOf(ITaskBundle item)
        {
            return _bundles.FindIndex(x => x.Equals(item));
        }

        public void RemoveAt(int index)
        {
            _bundles.RemoveAt(index);
        }
        
        public IEnumerator<ITaskBundle> GetEnumerator()
        {
            return _bundles.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bundles.GetEnumerator();
        }
        
        #endregion

        public object Clone()
        {
            var another = new TaskBundleCollection();
            foreach (var bundle in _bundles)
            {
                another.Add((ITaskBundle)bundle.Clone());
            }
            return another;
        }

        public void Setup(IProcessorType processor)
        {
            processor.Setup(this);
        }

        public void ResetAll()
        {
            foreach (var bundle in _bundles)
            {
                bundle.Reset();
            }
        }

        public void ResetFirst()
        {
            if (!_bundles.Any()) return;

            _bundles[0].Reset();
        }
        
        public override bool Equals(object? obj)
        {
            return obj is TaskBundleCollection collection && Equals(collection);
        }

        public bool Equals(IReadOnlyTaskBundleCollection? collection)
        {
            if (collection is null) return false;
            
            for (int i = 0; i < Count; i++)
            {
                if (!this.ElementAt(i).Equals(collection.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _bundles.GetHashCode();
        }
    }
}
