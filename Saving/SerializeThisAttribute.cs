using System;

namespace KarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SerializeThisAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Order { get; private set; }

        public SerializeThisAttribute(string name)
        {
            Name = name;
        }

        public SerializeThisAttribute(string name, int order) : this(name)
        {
            Order = order;
        }
    }
}
