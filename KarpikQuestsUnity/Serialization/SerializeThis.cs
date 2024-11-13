using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using System;

namespace Karpik.Quests.Serialization
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