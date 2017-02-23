using IronStar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.AI
{
    public class AISystem
    {
        public readonly Dictionary<Entity, BehaviorContext> BehaviourContext =  new Dictionary<Entity, BehaviorContext>();
    }

}
