using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Factories
{
    public class QuestTaskFactory : IFactory<ITask>, ISingleton<QuestTaskFactory>
    {
        private static QuestTaskFactory _instance;
        public static QuestTaskFactory Instance => _instance ??= new QuestTaskFactory();

        private QuestTaskFactory()
        {
            
        }
        
        public ITask Create()
        {
            return Create("Task", "Description");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITask Create(string name)
        {
            return Create(name, "Description");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITask Create(string name, string description)
        {
            var task = new Task(Id.NewId());

            task.Init(name, description);

            return task;
        }
    }
}