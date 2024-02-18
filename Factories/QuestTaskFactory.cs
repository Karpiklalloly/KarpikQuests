using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;

namespace KarpikQuests.Factories
{
    public class QuestTaskFactory : IFactory<IQuestTask>
    {
        public IQuestTask Create()
        {
            return Create("Task", "Description");
        }

        public IQuestTask Create(string name)
        {
            return Create(name, "Description");
        }

        public IQuestTask Create(string name, string description)
        {
            QuestTask task = new QuestTask();

            task.Init(KeyGenerator.GenerateKey(""), name, description);

            return task;
        }
    }
}