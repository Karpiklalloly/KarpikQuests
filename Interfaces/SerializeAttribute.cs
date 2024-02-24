using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Interfaces
{
    public interface ISerializeAttribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public Version Version { get; set; }
        public bool IsReference { get; set; }
    }
}
