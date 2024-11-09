using System;
using Karpik.Quests.Extensions;
using Karpik.Quests.Serialization;

namespace Karpik.Quests.Requirements
{
    [Serializable]
    public class QuestHasStatus : IRequirement
    {
        [DoNotSerializeThis]
        public Quest Quest => _quest;
        
        private Quest _quest;
        [SerializeThis("Satisfied")]
        private Status _satisfiedStatus;
        [SerializeThis("Ruined")]
        private Status _ruinedStatus;
        [SerializeThis("Id")]
        private Id _questId;
    
        public QuestHasStatus(Quest quest, Status satisfiedStatus = Status.Completed, Status ruinedStatus = Status.Failed)
        {
            _quest = quest;
            _satisfiedStatus = satisfiedStatus;
            _ruinedStatus = ruinedStatus;

            _questId = quest.IsValid() ? quest.Id : Id.Empty;
        }
    
        public bool IsSatisfied()
        {
            return _quest.Status == _satisfiedStatus;
        }

        public bool IsRuined()
        {
            return _quest.Status == _ruinedStatus;
        }

        public void SetGraph(IGraph graph)
        {
            if (!_quest.IsValid())
            {
                _quest = graph.GetQuestDeep(_questId);
            }
        }
    
        public static implicit operator QuestHasStatus((Quest, Status, Status) info)
        {
            return new QuestHasStatus(info.Item1, info.Item2, info.Item3);
        }

        public static implicit operator QuestAndRequirement(QuestHasStatus questHasStatus)
        {
            return new QuestAndRequirement(questHasStatus._quest, questHasStatus);
        }
    }
}