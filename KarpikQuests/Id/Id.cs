using System;
using System.ComponentModel;
using Karpik.Quests.Saving;

namespace Karpik.Quests.ID
{
    [Serializable][TypeConverter(typeof(IdConverter))]
    public readonly struct Id : IEquatable<Id>
    {
        public static readonly Id Empty = new Id("-1");
        public readonly string Value;
        private readonly string _toString;
    
        public Id(string value)
        {
            Value = string.IsNullOrWhiteSpace(value) || value == Empty.Value ? Empty.Value : value;
            _toString = $"ID: {Value}";
        }

        public static Id NewId() => IDGenerator.GenerateId();
    
        public bool Equals(Id other) => Value == other.Value;
    
        public override bool Equals(object obj) => obj is Id other && Equals(other);
    
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            return _toString;
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