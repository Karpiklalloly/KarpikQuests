using Newtonsoft.Json;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
    [Serializable]
    public class NeededCount : ICompletionType
    {
        [DoNotSerializeThis]
        [Property]
        [JsonIgnore]
        public int Count { get => _count; set => _count = value; }

        [SerializeThis("Count")]
        [JsonProperty(PropertyName = "Count")]
        private int _count;
        public NeededCount() : this(0)
        {
        }

        public NeededCount(int count)
        {
            _count = count;
        }

        public Status Check(IEnumerable<IRequirement> quests)
        {
            if (Count == 0)
                return Status.Completed;
            if (!quests.Any())
                return Status.Unlocked;
            var satisfied = quests.Count(Predicates.IsSatisfied);
            var failed = quests.Count(Predicates.IsRuined);
            if (satisfied >= Count)
                return Status.Completed;
            if (failed > 0)
                return Status.Failed;
            if (satisfied > 0)
                return Status.Unlocked;
            return Status.Locked;
        }
    }
}