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

        [DoNotSerializeThis][Property]
        public Id Id
        {
            get => _id;
            private set => _id = value;
        }

        [DoNotSerializeThis][Property]
        public Id ParentId
        {
            get => _parentId;
            private set => _parentId = value;
        }

        [DoNotSerializeThis][Property]
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        [DoNotSerializeThis][Property]
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        [DoNotSerializeThis][Property]
        public Status Status
        {
            get => _status;
            private set => _status = value;
        }

        [DoNotSerializeThis][Property]
        public ICompletionType CompletionType
        {
            get => _completionType;
            private set => _completionType = value;
        }

        [DoNotSerializeThis][Property]
        public IProcessorType Processor
        {
            get => _processor;
            private set => _processor = value;
        }

        [DoNotSerializeThis][Property]
        public IEnumerable<Quest> SubQuests
        {
            get => _subQuests;
            private set => _subQuests = new QuestCollection(value);
        }
        
        private Id _id;
        private Id _parentId;
        private string _name;
        private string _description;
        private Status _status;
        private ICompletionType _completionType;
        private IProcessorType _processor;
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