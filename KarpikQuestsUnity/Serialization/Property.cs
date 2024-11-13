using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using System;

namespace Karpik.Quests.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Property : Attribute
    {
    
    }
}