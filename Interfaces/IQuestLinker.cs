namespace KarpikQuests.Interfaces;

public interface IQuestLinker
{
    public IQuestCollection GetQuestDependencies(IQuest quest);

    public IQuestCollection GetQuestDependents(IQuest quest);

    public bool TryAddDependence(IQuest quest, IQuest dependence);

    public bool TryRemoveDependence(IQuest quest, IQuest dependence);
}
