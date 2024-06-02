using System;
using System.Collections;
using System.Collections.Generic;
using Karpik.Quests.Enumerators;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class GraphCollection : IGraphCollection
    {
        public int Count => _graphs.Count;
        public bool IsReadOnly => false;

        [JsonProperty("Graphs")]
        [SerializeThis("Graphs", IsReference = true)]
        private List<IGraph> _graphs = new List<IGraph>();

        #region list
        public IEnumerator<IGraph> GetEnumerator()
        {
            return new GraphCollectionEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GraphCollectionEnumerator(this);
        }

        public void Add(IGraph item)
        {
            _graphs.Add(item);
        }

        public void Clear()
        {
            _graphs.Clear();
        }

        public bool Contains(IGraph item)
        {
            foreach (var graph in _graphs)
            {
                if (graph.Equals(item)) return true;
            }

            return false;
        }

        public void CopyTo(IGraph[] array, int arrayIndex)
        {
            _graphs.CopyTo(array, arrayIndex);
        }

        public bool Remove(IGraph item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }
        
            _graphs.RemoveAt(index);
            return true;
        }
    
        public int IndexOf(IGraph item)
        {
            for (int i = 0; i < _graphs.Count; i++)
            {
                if (_graphs[i].Equals(item)) return i;
            }

            return -1;
        }

        public void Insert(int index, IGraph item)
        {
            _graphs.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _graphs.RemoveAt(index);
        }

        public IGraph this[int index]
        {
            get => _graphs[index];
            set => _graphs[index] = value;
        }
    
        public bool Has(IGraph item)
        {
            return Contains(item);
        }

        public bool Has(IQuest item)
        {
            foreach (var graph in _graphs)
            {
                if (graph.Has(item)) return true;
            }

            return false;
        }
        #endregion
    
        public object Clone()
        {
            var graphs = new IGraph[_graphs.Count];
            _graphs.CopyTo(graphs, 0);
        
            return new GraphCollection()
            {
                _graphs = new List<IGraph>(graphs)
            };
        }
    
        public bool Equals(IGraphCollection other)
        {
            if (other is null) return false;
        
            for (int i = 0; i < other.Count; i++)
            {
                if (!this[i].Equals(other[i])) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is IGraphCollection collection && Equals(collection);
        }
    }
}