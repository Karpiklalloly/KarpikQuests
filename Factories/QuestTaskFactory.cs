using System.Runtime.CompilerServices;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;

namespace KarpikQuests.Factories
{
    public readonly struct QuestTaskFactory : IFactory<ITask>
    {
        public readonly ITask Create()
        {
            return Create("Task", "Description");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITask Create(string name)
        {
            return Create(name, "Description");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITask Create(string name, string description)
        {
            var task = new Task();

            task.Init(KeyGenerator.GenerateKey(), name, description);

            return task;
        }
    }
}