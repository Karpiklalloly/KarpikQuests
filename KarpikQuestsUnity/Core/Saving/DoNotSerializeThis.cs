using System;

namespace NewKarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotSerializeThis : Attribute
    {
    
    }
}