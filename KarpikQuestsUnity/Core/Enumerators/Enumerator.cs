using System;
using System.Collections;
using System.Collections.Generic;

namespace NewKarpikQuests.Enumerators
{
    public abstract class Enumerator<T> : IEnumerator<T>
    {
        public T Current => _collection[_position];

        object IEnumerator.Current => Current;
    
        protected IList<T> _collection;
        protected int _position = -1;

        public Enumerator(IList<T> collection)
        {
            _collection = collection;
        }
    
        public bool MoveNext()
        {
            if (_position >= _collection.Count - 1) return false;
            _position++;
            return true;
        }

        public void Reset()
        {
            _position = -1;
        }
    
        public virtual void Dispose()
        {
            _collection = null;
        }
    }
}