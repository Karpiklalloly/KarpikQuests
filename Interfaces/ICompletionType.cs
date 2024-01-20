using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ICompletionType
    {
        /// <summary>
        /// Return on success result. Set <see cref="false"/> to invert behaviour
        /// </summary>
        public bool SuccessResult { get; }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles);

        public bool CheckCompletion(ITaskBundle bundle);
    }
}
