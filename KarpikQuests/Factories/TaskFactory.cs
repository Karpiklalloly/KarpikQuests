using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Factories
{
    public class TaskFactory : IFactory<ITask>, ISingleton<TaskFactory>
    {
        private static TaskFactory _instance;
        public static TaskFactory Instance => _instance ??= new TaskFactory();

        private TaskFactory()
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