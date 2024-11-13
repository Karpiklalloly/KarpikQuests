using Newtonsoft.Json;
using System;

namespace Karpik.Quests.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotSerializeThis : Attribute
    {
    
    }
}