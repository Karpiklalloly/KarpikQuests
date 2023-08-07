using System;

namespace KarpikQuests.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DoNotSerializeThisAttribute : Attribute
    {
    }
}
