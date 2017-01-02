using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics {

	public class SceneMapping {

		Dictionary<string,Node> pathNodeMapping;
		Dictionary<Node,int> nodeIndexMapping;

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		public SceneMapping ( Scene scene )
		{
			pathNodeMapping		=	new Dictionary<string,Node>();
			nodeIndexMapping	=	new Dictionary<Node, int>();

			for ( int i = 0; i<scene.Nodes.Count; i++ ) {
				pathNodeMapping.Add( scene.GetFullNodePath( scene.Nodes[i] ), scene.Nodes[i] );
			}

			for ( int i=0; i<scene.Nodes.Count; i++) {
				nodeIndexMapping.Add( scene.Nodes[i], i );
			}
		}


		public Node this[string fullNodePath] {
			get {
				return pathNodeMapping[fullNodePath];
			}
		}


		public int this[Node node] {
			get {
				return nodeIndexMapping[node];
			}
		}
	}
}
