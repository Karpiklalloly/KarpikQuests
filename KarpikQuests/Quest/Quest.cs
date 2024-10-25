using Karpik.Quests.CompletionTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Processors;
using Karpik.Quests.Sample;
using Karpik.Quests.Extensions;
using Karpik.Quests.Saving;

namespace Karpik.Quests
{
    //TODO: Add sub classes (check gpt)
    //TODo: Add auto notify when status changed
    [Serializable]
    public class Quest : IEquatable<Quest>
    {
        public static readonly Quest Empty = new Quest(Id.Empty, string.Empty, string.Empty, null, null);
    
        public Id Id { get; private set; }
        public Id ParentId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Status Status { get; private set; }
        public ICompletionType CompletionType { get; private set; }
        public IProcessorType Processor { get; private set; }
    
        private QuestCollection _subQuests = new();

        public Quest() : this(Id.NewId())
    {
        
    }

        public Quest(Id id) : this(id, "Quest", "Description")
    {
        
    }

        public Quest(string name, string description) : this(Id.NewId(), name, description)
    {
        
    }

        public Quest(Id id, string name, string description) : this(id, name, description, null, null)
    {
        
    }
    
        public Quest(Id id, string name, string description, ICompletionType? completionType, IProcessorType? processor)
    {
        Id = id;
        Name = name;
        Description = description;
        CompletionType = completionType ?? new And();
        Processor = processor ?? new Disorderly();
        
        Status = Status.Locked;
    }

        public void Setup()
    {
        Status = Status.Locked;
        for (int i = 0; i < _subQuests.Count; i++)
        {
            _subQuests[i].Setup();
        }
    }

        public void UpdateStatus()
    {
        if (_subQuests.Count <= 0) return;
        for (int i = 0; i < _subQuests.Count; i++)
        {
            _subQuests[i].UpdateStatus();
        }
        Status = CompletionType.Check(_subQuests);
        Processor.Update(_subQuests);
    }

        public void Add(Quest quest)
    {
        if (Has(quest)) return;
        _subQuests.Add(quest);
        quest.ParentId = Id;
    }

        public void Remove(Quest quest)
    {
        if (!Has(quest))
        {
            return;
        }

        if (!_subQuests.Remove(quest))
        {
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i].Remove(quest);
            }
        }
        quest.ParentId = Id.Empty;
    }

        public void Clear()
    {
        _subQuests.Clear();
    }
    
        public bool Has(Quest quest)
    {
        if (!quest.IsValid()) return false;
        
        return _subQuests.Any(q => q.Equals(quest))
               || _subQuests.Any(q => q.Has(quest));
    }

        public bool TryComplete()
    {
        if (Status != Status.Unlocked) return false;
        
        ForceComplete();
        return true;
    }

        public bool TryFail()
    {
        if (Status != Status.Unlocked) return false;
        
        ForceFail();
        return true;
    }
        public bool TryUnlock()
    {
        if (Status != Status.Locked) return false;
        
        ForceUnlock();
        return true;
    }
        public void ForceLock()
    {
        Status = Status.Locked;
        
        for (var i = 0; i < _subQuests.Count; i++)
        {
            var subQuest = _subQuests[i];
            subQuest.ForceLock();
        }
    }
        public void ForceUnlock()
    {
        Status = Status.Unlocked;
        Processor.Setup(_subQuests);
    }
        public void ForceComplete()
    {
        Status = Status.Completed;
        
        for (var i = 0; i < _subQuests.Count; i++)
        {
            var subQuest = _subQuests[i];
            if (subQuest.IsFinished()) continue;
            subQuest.ForceLock();
        }
    }
        public void ForceFail()
    {
        Status = Status.Failed;

        for (var i = 0; i < _subQuests.Count; i++)
        {
            var subQuest = _subQuests[i];
            if (subQuest.IsFinished()) continue;
            subQuest.ForceLock();
        }
    }

        public bool Equals(Quest other)
    {
        return Id.Equals(other.Id);
    }

        public override bool Equals(object? obj)
    {
        return obj is Quest other && Equals(other);
    }

        public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

        public static bool operator ==(Quest left, Quest right)
    {
        return left.Equals(right);
    }

        public static bool operator !=(Quest left, Quest right)
    {
        return !(left == right);
    }
    }
}