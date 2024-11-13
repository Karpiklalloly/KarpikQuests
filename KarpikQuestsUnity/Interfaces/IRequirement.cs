using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
namespace Karpik.Quests
{
    public interface IRequirement
    {
        public bool IsSatisfied();
        public bool IsRuined();
        public void SetGraph(IGraph graph);
    }
}