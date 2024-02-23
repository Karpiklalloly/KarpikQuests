using System.Runtime.CompilerServices;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;

namespace KarpikQuests.Factories
{
    public struct QuestTaskFactory : IFactory<ITask>
    {
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
            Task task = new Task();

            task.Init(KeyGenerator.GenerateKey(""), name, description);

            return task;
        }
    }
}