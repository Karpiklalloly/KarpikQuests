using KarpikQuests.Interfaces;
using KarpikQuests.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
#if UNITY
[SerializeField]
#endif
#if JSON_NEWTONSOFT
        [JsonProperty("Data")]
#endif
        [SerializeThis("Data")]
        private readonly List<IQuest> _data = new List<IQuest>();

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public IQuest this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Add(IQuest item)
        {
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(IQuest item)
        {
            return _data.Contains(item);
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
            return _data.Remove(item);
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
            return _data.IndexOf(item);
        }

        public void Insert(int index, IQuest item)
        {
            _data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }
    }
}


