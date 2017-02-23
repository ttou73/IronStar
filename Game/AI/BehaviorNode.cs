using Fusion.Engine.Common;
using IronStar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.AI
{
    public abstract class BehaviorNode
    {

        public readonly List<BehaviorNode> Children;
        public String Name { get; internal set; }
        public int ID { get; internal set; }

        public virtual string FullName { get { return tree.Name + "." + Name; } }

        private BehaviorTree tree;

        internal BehaviorNode(BehaviorTree tree, int id, string name = "")
        {
            this.tree = tree; 
            this.ID = id;
            this.Name = name == "" ? $"New node {id}" : name;
            Children = new List<BehaviorNode>();
        }

        internal Status InternalUpdate(GameWorld world, Entity entity, GameTime gameTime)
        {
            InternalEnter(world, entity, gameTime);

            if (!world.AISystem.BehaviourContext[entity].openNodes.ContainsKey(FullName))
            {
                InternalOpen(world, entity, gameTime);
            }

            var status = Update(world, entity, gameTime);

            if (status != Status.Running)
            {
                InternalClose(world, entity, gameTime);
            }

            Leave(world, entity, gameTime);


            return status;
        }


        internal void InternalEnter(GameWorld world, Entity entity, GameTime gameTime)
        {

            Enter(world, entity, gameTime);
        }

        internal void InternalOpen(GameWorld world, Entity entity, GameTime gameTime)
        {

            Open(world, entity, gameTime);
        }

        internal void InternalClose(GameWorld world, Entity entity, GameTime gameTime)
        {
            world.AISystem.BehaviourContext[entity].openNodes.Remove(FullName);
            Close(world, entity, gameTime);
        }

        internal protected virtual T GetLocalVariable<T>(GameWorld world, Entity entity, String name)
        {
            return world.AISystem.BehaviourContext[entity].GetLocalObject<T>(name);
        }

        internal protected virtual T GetGlobalVariable<T>(GameWorld world, Entity entity, String name)
        {
            return world.AISystem.BehaviourContext[entity].GetGlobalObject<T>(name);
        }

        abstract internal protected Status Update(GameWorld world, Entity entity, GameTime gameTime);

        abstract internal protected void Enter(GameWorld world, Entity entity, GameTime gameTime);

        abstract internal protected void Open(GameWorld world, Entity entity, GameTime gameTime);

        abstract internal protected void Close(GameWorld world, Entity entity, GameTime gameTime);

        abstract internal protected void Leave(GameWorld world, Entity entity, GameTime gameTime);

        /// <summary>
        /// Add child to this node
        /// </summary>
        /// <param name="node"></param>
        public virtual void AddChild(BehaviorNode node)
        {
            tree.AddChild(this, node);
            this.Children.Add(node);
        }
    }

    public enum Status
    {
        Success,
        Running,
        Failure,
        Error
    }
}
