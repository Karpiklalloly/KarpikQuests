using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Karpik.Quests
{
    public interface IQuestCollection : IReadOnlyQuestCollection, IList<Quest>
    {
        
    }
}