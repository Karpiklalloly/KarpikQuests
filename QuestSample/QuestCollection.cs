using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;


#if JSON_NEWTONSOFT
using Newtonsoft.Json;
#endif

#if UNITY
using UnityEngine;
#endif

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestCollection : IQuestCollection
    {
        #region serialize
#if UNITY
        [SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Data")]
#endif
        [SerializeThis("Data")]
        #endregion
        private readonly List<IQuest> _data = new List<IQuest>();

        public int Count => _data.Count;

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
            return _data.GetEnumerator();
        }

        public bool Remove(IQuest item)
        {
            if (!Has(item)) return false;
            var index = IndexOf(item);
            if (index < 0) return false;
            _data.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

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

            foreach (var quest in _data)
            {
                if (quest.Equals(item)) return true;
            }

            return false;
        }

        public object Clone()
        {
            QuestCollection quests = new QuestCollection();
            foreach (var quest in _data)
            {
                quests.Add((IQuest)quest.Clone());
            }

            return quests;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is QuestCollection collection))
            {
                return false;
            }

            return Equals(collection);
        }

        public bool Equals(IReadOnlyQuestCollection? other)
        {
            if (other is null) return false;

            for (int i = 0; i < Count; i++)
            {
                if (!this.ElementAt(i).Equals(other.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }
    }
}


