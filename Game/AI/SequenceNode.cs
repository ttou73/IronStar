using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using IronStar.Core;

namespace IronStar.AI
{
    public class SequenceNode : BehaviorNode
    {
        public SequenceNode(BehaviorTree tree, int id, string name = "") : base(tree, id, name)
        {
        }

        protected internal override void Close(GameWorld world, Entity entity, GameTime gameTime)
        {
        }

        protected internal override void Enter(GameWorld world, Entity entity, GameTime gameTime)
        {
        }

        protected internal override void Leave(GameWorld world, Entity entity, GameTime gameTime)
        {
        }

        protected internal override void Open(GameWorld world, Entity entity, GameTime gameTime)
        {
        }

        protected internal override Status Update(GameWorld world, Entity entity, GameTime gameTime)
        {
            foreach (var n in Children)
            {
                var status = n.InternalUpdate(world, entity, gameTime);
                if (status != Status.Success)
                {
                    return status;
                }
            }
            return Status.Success;
        }
    }
}
