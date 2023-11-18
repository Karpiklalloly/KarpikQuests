using System;

namespace KarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeThisAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public Version Version { get; set; }

        public SerializeThisAttribute(string name)
        {
            Name = name;
        }
    }
}
