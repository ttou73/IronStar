using Fusion.Engine.Common;
using IronStar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.AI
{
    public class BehaviorTree
    {
        public virtual string Name { get; protected set; }
        protected Dictionary<string, BehaviorNode> nodes;
        List<BehaviorNode> parents;


        protected int totalIDs;
        public BehaviorTree(string name = "")
        {
            nodes = new Dictionary<string, BehaviorNode>();
            parents = new List<BehaviorNode>();
            Name = name == "" ? "Tree" : name;
        }


        /// <summary>
        /// Add a new node to tree.
        /// </summary>
        /// <param name="type">a type of new node</param>
        /// <param name="parent">null if node is parent</param>
        /// <param name="name"></param>
        public virtual BehaviorNode AddNode<T>(BehaviorNode parent, string name = "") where T: BehaviorNode
        {
            return AddNode(typeof(T), parent, name);
        }

        /// <summary>
        /// Add a new node to tree.
        /// </summary>
        /// <param name="type">a type of new node</param>
        /// <param name="parent">null if node is parent</param>
        /// <param name="name"></param>
        public virtual BehaviorNode AddNode(Type type, BehaviorNode parent, string name = "")
        {
            if (!type.IsSubclassOf(typeof(BehaviorNode)))
            {
                throw new ArgumentException("TODO", nameof(type));
            }

            if (name == "")
            {
                name = "New node";
                int i = 0;
                while (nodes.ContainsKey(name))
                {
                    i++;
                    name = $"New node{i}";
                }
            }

            if (nodes.ContainsKey(name))
            {
                throw new ArgumentException("Tree contains key", nameof(name));
            }
            var t = (BehaviorNode)Activator.CreateInstance(type, new object[] { this, totalIDs++, name });
            if (parent == null)
            {
                parents.Add(t);
            }
            else
            {
                parent.Children.Add(t);
            }
            nodes[t.Name] = t;
            return t;
        }

        /// <summary>
        /// Run tree with context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="gameTime"></param>
        public virtual void Run(GameWorld world, Entity entity, GameTime gameTime)
        {
            var openNodes = world.AISystem.BehaviourContext[entity].openNodes;
            var prevOpenNodes = openNodes.Keys.ToList();
            openNodes.Clear();
            foreach (var n in parents)
            {
                n.InternalUpdate(world, entity, gameTime);
            }
            foreach (var s in prevOpenNodes)
            {
                if (!openNodes.ContainsKey(s))
                {
                    nodes[s].Close(world, entity, gameTime);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected virtual void AddChild(BehaviorNode parent, BehaviorNode child)
        {
            parents.Remove(child);
        }

        public virtual IEnumerable<BehaviorNode> Nodes
        {
            get { return nodes.Values; }
        }

        public virtual void Clear()
        {
            nodes.Clear();
            parents.Clear();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
