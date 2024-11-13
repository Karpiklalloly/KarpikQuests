using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
namespace Karpik.Quests
{
    public interface IProcessorType
    {
        public void Setup(QuestCollection quests);
        public void Update(QuestCollection quests);
    }
}