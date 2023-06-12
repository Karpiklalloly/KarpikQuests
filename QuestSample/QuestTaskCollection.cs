using KarpikQuests.Interfaces;
using System.Collections;

namespace KarpikQuests.QuestSample;

public class QuestTaskCollection : IQuestTaskCollection
{
    private readonly List<IQuestTask> _tasks = new();

    public int Count => _tasks.Count;

    public bool IsReadOnly => false;

    public void Add(IQuestTask item)
    {
        _tasks.Add(item);
    }

    public void Clear()
    {
        _tasks.Clear();
    }

    public bool Contains(IQuestTask item)
    {
        return _tasks.Contains(item);
    }

    public void CopyTo(IQuestTask[] array, int arrayIndex)
    {
        _tasks.CopyTo(array, arrayIndex);
    }

    public IEnumerator<IQuestTask> GetEnumerator()
    {
        return _tasks.GetEnumerator();
    }

    public bool Remove(IQuestTask item)
    {
        return _tasks.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _tasks.GetEnumerator();
    }
}
