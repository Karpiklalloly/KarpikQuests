using System;

namespace Karpik.Quests.Saving
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SerializeThis : Attribute
    {
        public string Name;
        public bool IsReference;

        public SerializeThis(string name)
        {
            Name = name;
        }
    }
}