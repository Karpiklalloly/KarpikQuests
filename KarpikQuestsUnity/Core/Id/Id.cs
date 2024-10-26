using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using System.ComponentModel;
using Karpik.Quests.Saving;

namespace Karpik.Quests.ID
{
    [Serializable][TypeConverter(typeof(IdConverter))]
    public struct Id : IEquatable<Id>
    {
        public static readonly Id Empty = new Id("-1");
        [DoNotSerializeThis][Property]
[CreateProperty][JsonIgnore]        public string Value => _value;
        private readonly string _toString;
        [SerializeThis("Value")]
[SerializeField][JsonProperty(PropertyName = "Value")]        private string _value;
    
        public Id(string value)
        {
            _value = string.IsNullOrWhiteSpace(value) || value == Empty.Value ? Empty.Value : value;
            _toString = $"ID: {_value}";
        }

        public static Id NewId() => IDGenerator.GenerateId();
    
        public bool Equals(Id other) => Value == other.Value;
    
        public override bool Equals(object? obj) => obj is Id other && Equals(other);
    
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
    }
}