﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Processors;
using Karpik.Quests.Extensions;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
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

        [DoNotSerializeThis][Property]
        public IEnumerable<IRequirement> Requirements
        {
            get => _requirements;
            private set => _requirements = new List<IRequirement>(value);
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
        [SerializeThis("CompletionType", IsReference = true)]
        private ICompletionType _completionType;
        [SerializeThis("Processor", IsReference = true)]
        private IProcessorType _processor;
        [SerializeThis("SubQuests")]
        private QuestCollection _subQuests = new();
        [SerializeThis("Requirements", IsReference = true)]
        private List<IRequirement> _requirements = new();

        public Quest() : this(Id.NewId(), null, null, null, null)
        {
            
        }

        public Quest(string name) : this(Id.NewId(), name, null, null, null)
        {
            
        }

        public Quest(string name, string description) : this(Id.NewId(), name, description, null, null)
        {
            
        }
        
        public Quest(Id id, string? name, string? description, ICompletionType? completionType, IProcessorType? processor, params QuestAndRequirement[] subQuests)
        {
            Id = id;
            _name = name ?? "Quest";
            _description = description ?? "Description";
            _completionType = completionType ?? new And();
            _processor = processor ?? new Disorderly();
            
            Status = Status.Locked;
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

        public void Add(params QuestAndRequirement[] qAndR)
        {
            foreach (var pair in qAndR)
            {
                var quest = pair.Quest;
                if (Has(quest.Id)) continue;
                if (!quest._id.IsValid())
                {
                    quest._parentQuest.Remove(quest);
                }
                _subQuests.Add(quest);
                quest._parentId = _id;
                quest._parentQuest = this;
                quest._graph = _graph;
                
                _requirements.Add(pair.Requirement);
            }
        }

        public void Add(params IRequirement[] requirements)
        {
            foreach (var requirement in requirements)
            {
                _requirements.Add(requirement);
            }
        }

        public void Remove(Quest quest)
        {
            if (!Has(quest.Id))
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

            for (int i = 0; i < _requirements.Count; i++)
            {
                _requirements[i].SetGraph(graph);
            }
        }
    
        public bool Has(Id id)
        {
            if (!id.IsValid()) return false;
            
            return _subQuests.Any(q => q.Id == id)
                   || _subQuests.Any(q => q.Has(id));
        }

        public bool TryGet(Id id, out Quest quest)
        {
            if (!id.IsValid())
            {
                quest = Empty;
                return false;
            }

            if (_subQuests.Any(q => q.Id == id))
            {
                quest = _subQuests.First(q => q.Id == id);
                return true;
            }
            
            if (_subQuests.Any(q => q.Has(id)))
            {
                var q = _subQuests.First(q => q.Has(id));
                return q.TryGet(id, out quest);
            }

            quest = Empty;
            return false;
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
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i].TryComplete();
            }
            if (_completionType.Check(_requirements) != Status.Completed) return false;
            
            ForceComplete();
            return true;
        }
        public bool TryFail()
        {
            if (_status != Status.Unlocked) return false;
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i].TryFail();
            }
            if (_completionType.Check(_requirements) != Status.Failed) return false;
            
            ForceFail();
            return true;
        }
        
        public void ForceLock()
        {
            var oldStatus = _status;
            for (var i = 0; i < _subQuests.Count; i++)
            {
                var subQuest = _subQuests[i];
                subQuest.ForceLock();
            }
            
            _status = Status.Locked;

            UpdateStatus(oldStatus);
        }
        public void ForceUnlock()
        {
            var oldStatus = _status;
            
            _processor.Setup(_subQuests);
            _status = Status.Unlocked;
            UpdateStatus(oldStatus);
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
            _graph?.Update(this, !_parentId.IsValid());
            if (_parentQuest is not null)
            {
                _parentQuest.NotifyFromChild();
            }
        }

        private void NotifyFromChild()
        {
            var oldStatus = _status;
            _status = _completionType.Check(_requirements);
            _processor.Update(_subQuests);
            UpdateStatus(oldStatus);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            for (int i = 0; i < _subQuests.Count; i++)
            {
                _subQuests[i]._parentId = _id;
            }
        }
    }
}