using NewKarpikQuests.Interfaces;

namespace NewKarpikQuests.Enumerators
{
    public class GraphCollectionEnumerator : Enumerator<IGraph>
    {
        public GraphCollectionEnumerator(IGraphCollection collection) : base(collection)
        {
        }
    }
}