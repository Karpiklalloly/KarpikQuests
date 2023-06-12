using KarpikQuests.Interfaces;

namespace KarpikQuests.QuestSample;

public class QuestAggregator : IQuestAggregator
{
    private readonly IQuestCollection _quests = new QuestCollection();
    private readonly IQuestLinker _linker = new QuestLinker();

    public IReadOnlyCollection<IQuest> Quests => _quests;
    public ReadOnlySpan<IQuest> GetQuests() => new(_quests.ToArray());

    public bool TryAddQuest(IQuest quest)
    {
        if (_quests.Contains(quest))
        {
            return false;
        }

        _quests.Add(quest);
        quest.Completed += OnQuestCompleted;
        return true;
    }

    public bool TryRemoveQuest(IQuest quest)
    {
        if (!_quests.Contains(quest))
        {
            return false;
        }

        var dependencies = _linker.GetQuestDependencies(quest);
        
        if (dependencies.Count() > 1)
        {
            return false;
        }

        var baseQuest = dependencies.ElementAt(0);
        _linker.TryRemoveDependence(quest, baseQuest);

        var dependents = _linker.GetQuestDependents(quest);
        foreach (var dep in dependents)
        {
            _linker.TryRemoveDependence(dep, quest);
            _linker.TryAddDependence(dep, baseQuest);
        }

        _quests.Remove(quest);
        return true;
    }

    public bool TryAddDependence(IQuest quest, IQuest dependence)
    {
        return _linker.TryAddDependence(quest, dependence);
    }

    public bool TryRemoveDependence(IQuest quest, IQuest dependence)
    {
        return _linker.TryRemoveDependence(quest, dependence);
    }

    public IQuestCollection GetDependencies(IQuest quest)
    {
        return _linker.GetQuestDependencies(quest);
    }

    public IQuestCollection GetDependents(IQuest quest)
    {
        return _linker.GetQuestDependents(quest);
    }

    public bool CheckKeyCollisions()
    {
        var keys = _quests.GroupBy(x => x.Key)
            .Where(group => group.Count() > 1)
            .Select(quest => quest.Key);
        if (keys.Count() > 1)
        {
            return false;
        }
        return true;
    }

    private void OnQuestCompleted(IQuest quest)
    {
        quest.Completed -= OnQuestCompleted;
        var dependents = GetDependents(quest);
        foreach (var dependent in dependents)
        {
            dependent.Start();
        }
    }

    public void Start()
    {
        foreach (var quest in _quests)
        {
            if (!GetDependencies(quest).Any())
            {
                quest.Start();
            }
        }
    }
}
