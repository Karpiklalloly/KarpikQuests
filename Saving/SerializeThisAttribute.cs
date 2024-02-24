using Newtonsoft.Json;
using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeThisAttribute : Attribute, ISerializeAttribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public Version Version { get; set; }
        public bool IsReference { get; set; }
    }
}
