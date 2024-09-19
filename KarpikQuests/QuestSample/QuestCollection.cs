using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Karpik.Quests.Enumerators;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;

namespace Karpik.Quests.QuestSample
{
    [System.Serializable]
    public class QuestCollection : IQuestCollection
    {
        [SerializeThis("Data", IsReference = true)]
        private List<IQuest> _data = new List<IQuest>();

        public QuestCollection() {}

        public QuestCollection(IEnumerable<IQuest> quests)
        {
            foreach (var quest in quests)
            {
                _data.Add(quest);
            }
        }

#region list

        [DoNotSerializeThis]
        public int Count => _data.Count;

        [DoNotSerializeThis]
        public bool IsReadOnly => false;

        public IQuest this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public void Add(IQuest item)
        {
            if (Has(item)) return;
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(IQuest item)
        {
            return Has(item);
        }

        public void CopyTo(IQuest[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IQuest> GetEnumerator()
        {
            return new QuestCollectionEnumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QuestCollectionEnumerator(this);
        }

        public bool Remove(IQuest item)
        {
            if (!Has(item)) return false;
            var index = IndexOf(item);
            if (index < 0) return false;
            _data.RemoveAt(index);
            return true;
        }

        public int IndexOf(IQuest item)
        {
            return _data.FindIndex(x => x.Equals(item));
        }

        public void Insert(int index, IQuest item)
        {
            _data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }

        public bool Has(IQuest item)
        {
            if (item is null) return false;

            return _data.Contains(item);
        }
#endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($" Count:{Count}\n");
            foreach (IQuest item in _data)
            {
                builder.Append(item.ToString() + '\n');
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QuestCollection collection)) return false;

            return Equals(this, collection);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public bool Equals(IReadOnlyQuestCollection other)
        {
            if (other is null) return false;
        
            for (int i = 0; i < other.Count(); i++)
            {
                if (!this[i].Equals(other.ElementAt(i))) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] IReadOnlyQuestCollection obj)
        {
            return obj.GetHashCode();
        }
    }
}