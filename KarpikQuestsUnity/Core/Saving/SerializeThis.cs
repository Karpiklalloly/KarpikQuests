using System;

namespace NewKarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeThis : Attribute
    {
        public string Name;
        public bool IsReference = false;

        public SerializeThis(string name)
        {
            Name = name;
        }
    }
}