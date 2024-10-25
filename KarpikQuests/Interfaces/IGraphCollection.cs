using Karpik.Quests.Sample;

namespace Karpik.Quests.Interfaces
{
    public interface IGraphCollection : IList<IGraph>, ICloneable, IEquatable<IGraphCollection>
    {
        public bool Has(IGraph item);
        public bool Has(in Quest item);
    }
}