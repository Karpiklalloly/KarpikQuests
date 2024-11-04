using Newtonsoft.Json;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Requirements;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
    [Serializable]
    public class QuestAndRequirement
    {
        [DoNotSerializeThis]
        [Property]
        [JsonIgnore]
        public Quest Quest { get => _quest; private set => _quest = value; }

        [DoNotSerializeThis]
        [Property]
        [JsonIgnore]
        public IRequirement Requirement { get => _requirement; private set => _requirement = value; }

        [SerializeThis("Quest")]
        [JsonProperty(PropertyName = "Quest")]
        private Quest _quest;
        [SerializeThis("Requirement")]
        [JsonProperty(PropertyName = "Requirement")]
        private IRequirement _requirement;
        public QuestAndRequirement(Quest quest, IRequirement requirement)
        {
            _quest = quest;
            _requirement = requirement;
        }

        public static implicit operator QuestAndRequirement(Tuple<Quest, IRequirement> tuple)
        {
            return new QuestAndRequirement(tuple.Item1, tuple.Item2);
        }

        public static implicit operator QuestAndRequirement(Quest quest)
        {
            return new QuestAndRequirement(quest, new QuestHasStatus(quest, Status.Completed, Status.Failed));
        }
    }
}