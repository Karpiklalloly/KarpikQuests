using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
    [Serializable]
    [TypeConverter(typeof(IdConverter))]
    public struct Id : IEquatable<Id>
    {
        public static readonly Id Empty = new Id("-1");
        [DoNotSerializeThis]
        [Property]
        [JsonIgnore]
        public string Value => _value;

        [SerializeThis("Value")]
        [JsonProperty(PropertyName = "Value")]
        private string _value;
        public Id(string value)
        {
            _value = string.IsNullOrWhiteSpace(value) || value == Empty.Value ? Empty.Value : value;
        }

        public static Id NewId() => new Id(Guid.NewGuid().ToString());
        public bool Equals(Id other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is Id other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString()
        {
            return $"ID: {_value}";
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