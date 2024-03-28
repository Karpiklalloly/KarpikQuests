using System;

namespace Karpik.Quests.ID
{
    [Serializable]
    public readonly struct Id : IEquatable<Id>
    {
        public static readonly Id Empty = new Id("-1");
        public string Value { get; }
    
        public Id(string value)
        {
            Value = string.IsNullOrWhiteSpace(value) ? Empty.Value : value;
        }

        public static Id NewId() => new Id(IDGenerator.GenerateId());
    
        public bool Equals(Id other) => Value == other.Value;
    
        public override bool Equals(object? obj) => obj is Id other && Equals(other);
    
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            return $"ID: {Value}";
        }

        public static bool operator ==(Id left, Id right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Id left, Id right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(Id left, string right)
        {
            return left.Value == right;
        }

        public static bool operator !=(Id left, string right)
        {
            return !(left == right);
        }
    }
}