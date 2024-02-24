using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace KarpikQuests.QuestSample
{
    [System.Serializable]
    public class QuestCollection : IQuestCollection
    {
        [SerializeThis(Name = "Data")]
        [JsonProperty("Data")]
        private readonly List<IQuest> _data = new List<IQuest>();

        [property: JsonIgnore]
        [DoNotSerializeThis]
        public int Count => _data.Count;

        [property: JsonIgnore]
        [DoNotSerializeThis]
        public bool IsReadOnly => false;

#region list
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

        public object Clone()
        {
            QuestCollection quests = new QuestCollection();
            foreach (var quest in _data)
            {
                quests.Add((IQuest)quest.Clone());
            }

            return quests;
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

        public override bool Equals(object? obj)
        {
            if (!(obj is QuestCollection collection)) return false;

            return Equals(this, collection);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public bool Equals(IReadOnlyQuestCollection? x, IReadOnlyQuestCollection? y)
        {
            if (x is null && y is null) return true;

            if (x is null || y is null) return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (!x[i].Equals(y[i])) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] IReadOnlyQuestCollection obj)
        {
            return obj.GetHashCode();
        }
    }
}