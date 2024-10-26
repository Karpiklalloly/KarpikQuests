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
    //TODO: Add negative effect (like wrong poison used)
    [Serializable]
    public class Quest : IEquatable<Quest>
    {
        public static readonly Quest Empty = new(Id.Empty, null, null, null, null);

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
        
        [SerializeThis("Id")]
        private Id _id;
        private Id _parentId = Id.Empty;
        private Quest? _parentQuest = null;
        private IGraph _graph;
        [SerializeThis("Name")]
        private string _name;
        [SerializeThis("Description")]
        private string _description;
        [SerializeThis("Status")]
        private Status _status;
        [SerializeThis("CompletionType")]
        private ICompletionType _completionType;
        [SerializeThis("Processor")]
        private IProcessorType _processor;
        [SerializeThis("SubQuests")]
        private QuestCollection _subQuests = new();

        public Quest() : this(Id.NewId())
        {
            
        }

        public Quest(Id id) : this(id, "Quest", "Description")
        {
            
        }
        
        public Quest(string name) : this(Id.NewId(), name, "Description")
        {
            
        }

        public Quest(string name, string description) : this(Id.NewId(), name, description)
        {
            
        }

        public Quest(Id id, string name, string description) : this(id, name, description, null, null)
        {
            
        }
        
        public Quest(string name, string description, ICompletionType? completionType, IProcessorType? processor)
            : this(Id.NewId(), name, description, completionType, processor)
        {
            
        }
    
        public Quest(Id id, string? name, string? description, ICompletionType? completionType, IProcessorType? processor)
        {
            Id = id;
            _name = name ?? string.Empty;
            _description = description ?? string.Empty;
            _completionType = completionType ?? new And();
            _processor = processor ?? new Disorderly();
            
            Status = Status.Locked;
        }
        
        public Quest(string name, string description, ICompletionType? completionType, IProcessorType? processor, params Quest[] subQuests)
            : this(Id.NewId(), name, description, completionType, processor)
        {
            Add(subQuests);
        }

        public void Setup()
        {
            _status = Status.Locked;
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i].Setup();
            }
        }

        public void Add(params Quest[] quests)
        {
            foreach (var quest in quests)
            {
                if (Has(quest)) continue;
                if (!quest._id.IsValid())
                {
                    quest._parentQuest.Remove(quest);
                }
                _subQuests.Add(quest);
                quest._parentId = _id;
                quest._parentQuest = this;
                quest._graph = _graph;
            }
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
            quest._parentId = Id.Empty;
            quest._parentQuest = null;
            quest._graph = null;
        }

        public void Clear()
        {
            foreach (var quest in _subQuests.ToArray())
            {
                Remove(quest);
            }
        }

        public void SetGraph(IGraph graph)
        {
            _graph = graph;
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i].SetGraph(graph);
            }
        }
    
        public bool Has(Quest quest)
        {
            if (!quest.IsValid()) return false;
            
            return _subQuests.Any(q => q.Equals(quest))
                   || _subQuests.Any(q => q.Has(quest));
        }

        public bool TryUnlock()
        {
            if (_status != Status.Locked) return false;
            
            ForceUnlock();
            return true;
        }
        public bool TryComplete()
        {
            if (_status != Status.Unlocked) return false;
            
            ForceComplete();
            return true;
        }
        public bool TryFail()
        {
            if (_status != Status.Unlocked) return false;
            
            ForceFail();
            return true;
        }
        
        public void ForceLock()
        {
            for (var i = 0; i < _subQuests.Count; i++)
            {
                var subQuest = _subQuests[i];
                subQuest.ForceLock();
            }
            
            _status = Status.Locked;

            Notify();
        }
        public void ForceUnlock()
        {
            _processor.Setup(_subQuests);
            _status = Status.Unlocked;
            Notify();
        }
        public void ForceComplete()
        {
            var oldStatus = _status;
            
            for (var i = 0; i < _subQuests.Count; i++)
            {
                var subQuest = _subQuests[i];
                if (subQuest.IsFinished()) continue;
                subQuest.ForceLock();
            }
            _status = Status.Completed;
            UpdateStatus(oldStatus);
        }
        public void ForceFail()
        {
            var oldStatus = _status;
            

            for (var i = 0; i < _subQuests.Count; i++)
            {
                var subQuest = _subQuests[i];
                if (subQuest.IsFinished()) continue;
                subQuest.ForceLock();
            }
            _status = Status.Failed;
            UpdateStatus(oldStatus);
        }

        public bool Equals(Quest? other)
        {
            return !ReferenceEquals(null, other) && _id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            return obj is Quest other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static bool operator ==(Quest left, Quest right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Quest left, Quest right)
        {
            return !(left == right);
        }
        
        private void UpdateStatus(Status oldStatus)
        {
            if (oldStatus == _status) return;
            
            Notify();
        }

        private void Notify()
        {
            _graph?.InternalUpdate(this, !_parentId.IsValid());
            if (_parentQuest is not null)
            {
                _parentQuest.NotifyUpdate();
            }
        }

        private void NotifyUpdate()
        {
            var oldStatus = _status;
            _status = _completionType.Check(_subQuests);
            _processor.Update(_subQuests);
            UpdateStatus(oldStatus);
        }
    }
}