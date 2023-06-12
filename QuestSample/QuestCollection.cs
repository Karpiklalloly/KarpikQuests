using KarpikQuests.Interfaces;
using System.Collections;
using System.Text;

namespace KarpikQuests.QuestSample;

public class QuestCollection : IQuestCollection
{
    private readonly List<IQuest> _data = new();

    public int Count => _data.Count;

    public bool IsReadOnly => false;

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
        StringBuilder builder = new();
        builder.Append($" Count:{Count}\n");
        foreach (IQuest item in _data)
        {
            builder.Append(item.ToString() + '\n');
        }
        return builder.ToString();
    }
}
