using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.AI
{
    public class BehaviorContext
    {
        readonly private Dictionary<string, object> localMemory = new Dictionary<string, object>();

        readonly private Dictionary<string, object> globalMemory = new Dictionary<string, object>();

        readonly internal Dictionary<string, BehaviorNode> openNodes = new Dictionary<string, BehaviorNode>();

        public T GetLocalObject<T>(string name)
        {
            return (T)localMemory[name];
        }

        public T GetGlobalObject<T>(string name)
        {
            return (T)globalMemory[name];
        }
    }
}
