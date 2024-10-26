using System;

namespace Karpik.Quests.Saving
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotSerializeThis : Attribute
    {
    
    }
}