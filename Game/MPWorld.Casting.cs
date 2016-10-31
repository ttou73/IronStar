using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using ShooterDemo.Views;
using ShooterDemo.Controllers;
using BEPUphysics;
using BU = BEPUutilities;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.TwoEntity.Motors;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.CollisionShapes;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;

namespace ShooterDemo {
	public partial class MPWorld : World {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="normal"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool RayCastAgainstAll ( Vector3 from, Vector3 to, out Vector3 normal, out Vector3 pos, out Entity hitEntity, Entity skipEntity = null )
		{
			hitEntity	=	null;
			var dir		=	to - from;
			var dist	=	dir.Length();
			var ndir	=	dir.Normalized();
			Ray ray		=	new Ray( from, ndir );

			normal	= Vector3.Zero;
			pos		= to;

			Func<BroadPhaseEntry, bool> filterFunc = delegate(BroadPhaseEntry bpe) 
			{
				if (skipEntity==null) return true;

				ConvexCollidable cc = bpe as ConvexCollidable;
				if (cc==null) return true;
					
				Entity ent = cc.Entity.Tag as Entity;
				if (ent==null) return true;

				if (ent==skipEntity) return false;

				return true;
			};

			var rcr		= new RayCastResult();	
			var bRay	= MathConverter.Convert( ray );

			bool result = physSpace.RayCast( bRay, dist, filterFunc, out rcr );

			if (!result) {
				return false;
			}

			var convex	=	rcr.HitObject as ConvexCollidable;
			normal		=	MathConverter.Convert( rcr.HitData.Normal ).Normalized();
			pos			=	MathConverter.Convert( rcr.HitData.Location );
			hitEntity	=	(convex == null) ? null : convex.Entity.Tag as Entity;

			return true;
		}





		/// <summary>
		/// Returns the list of ConvexCollidable's and Entities inside or touching the specified sphere.
		/// Result does not include static geometry and non-entity physical objects.
		/// </summary>
		/// <param name="world"></param>
		/// <param name="origin"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public List<Entity> WeaponOverlap ( Vector3 origin, float radius, Entity entToSkip )
		{
			BU.BoundingSphere	sphere		= new BU.BoundingSphere(MathConverter.Convert(origin), radius);
			SphereShape			sphereShape = new SphereShape(radius);
			BU.Vector3			zeroSweep	= BU.Vector3.Zero;
			BU.RigidTransform	rigidXForm	= new BU.RigidTransform( MathConverter.Convert(origin) );	

			var candidates = PhysicsResources.GetBroadPhaseEntryList();
            physSpace.BroadPhase.QueryAccelerator.BroadPhase.QueryAccelerator.GetEntries(sphere, candidates);
			
			var result = new List<Entity>();

			foreach ( var candidate in candidates )	{

				BU.RayHit rayHit;
				bool r = candidate.ConvexCast( sphereShape, ref rigidXForm, ref zeroSweep, out rayHit );

				if (r) {
					
					var collidable	=	candidate as ConvexCollidable;
					var entity		=	collidable==null ? null : collidable.Entity.Tag as Entity;

					if (collidable==null) continue;
					if (entity==null) continue;

					result.Add( entity );
				}
			}
			
			result.RemoveAll( e => e == entToSkip );

			return result;
		}

		#if false
		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="normal"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool RayCastAgainstAll ( Vector3 from, Vector3 to, out Vector3 normal, out Vector3 pos, out Entity hitEnt, Entity entToSkip=null )
		{
			var dir  = to - from;
			var dist = dir.Length();
			var ndir = dir.Normalized();
			Ray ray  = new Ray( from, ndir );

			normal	= Vector3.Zero;
			pos		= to;
			hitEnt	= null;

			Func<BroadPhaseEntry, bool> filterFunc = delegate(BroadPhaseEntry bpe) 
			{
				if (entToSkip==null) return true;

				ConvexCollidable cc = bpe as ConvexCollidable;
				if (cc==null) return true;
					
				Entity ent = cc.Entity.Tag as Entity;
				if (ent==null) return true;

				if (ent==entToSkip) return false;

				return true;
			};
					

			var rcr = new RayCastResult();					

			bool result = Space.RayCast( ray, dist, filterFunc, out rcr );

			if (!result) {
				return false;
			}

			var convex	=	rcr.HitObject as ConvexCollidable;
			normal		=	rcr.HitData.Normal.Normalized();
			pos			=	rcr.HitData.Location;

			hitEnt		=	(convex == null) ? null : convex.Entity.Tag as Entity;

			return true;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="normal"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public List<Vector3> RayCastAgainstStatic ( Vector3 from, Vector3 to )
		{							 
			var list = new List<Vector3>();

			var dir  = to - from;
			var dist = dir.Length();
			var ndir = dir.Normalized();
			Ray ray  = new Ray( from, ndir );


			Func<BroadPhaseEntry, bool> filterFunc = delegate(BroadPhaseEntry bpe) 
			{
				ConvexCollidable cc = bpe as ConvexCollidable;
				if (cc!=null) return false;

				return true;
			};

			IList<RayCastResult>	rcrList = new List<RayCastResult>();

			bool result = Space.RayCast( ray, dist, filterFunc, rcrList );

			if (!result) {
				return list;
			}

			foreach ( var rcr in rcrList ) {
				list.Add( rcr.HitData.Location );
				list.Add( rcr.HitData.Normal );
			}

			return list;
		}


		void CHECK( float a ) { Debug.Assert( !float.IsNaN(a) || !float.IsInfinity(a) ); }
		void CHECK( Vector3 a ) { CHECK(a.X); CHECK(a.Y); CHECK(a.Z);  }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="szx"></param>
		/// <param name="szy"></param>
		/// <param name="szz"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="normal"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool ConvexCastAgainstStatic ( ConvexShape shape, Vector3 from, Vector3 to, out Vector3 normal, out Vector3 pos )
		{
			BoundingBox		bbox;
			Vector3			sweep		= to - from;
			AffineTransform identity	= AffineTransform.Identity;
			RigidTransform	transform	= new RigidTransform( from );

			CHECK( from );
			CHECK( to );

			normal	= Vector3.Zero;
			pos		= to;

            shape.GetSweptLocalBoundingBox( ref transform, ref identity, ref sweep, out bbox );

			var candidates = Resources.GetCollisionEntryList();
            Space.BroadPhase.QueryAccelerator.BroadPhase.QueryAccelerator.GetEntries( bbox, candidates );

			float minT = float.MaxValue;
			bool  hit  = false;
			
			foreach ( var candidate in candidates )	{
				
				if ( candidate as ConvexCollidable != null ) {
					continue;
				}

				RayHit rayHit;
				bool r = candidate.ConvexCast( shape, ref transform, ref sweep, out rayHit );

				if (!r) continue;

				if ( minT > rayHit.T ) {
					hit   	= true;
					minT	= rayHit.T;
					normal	= rayHit.Normal;
					pos		= rayHit.Location;
				}
			}

			return hit;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="szx"></param>
		/// <param name="szy"></param>
		/// <param name="szz"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="normal"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool ConvexCastAgainstStatic ( ConvexShape shape, Matrix xform, Vector3 initOffset, Vector3 sweep, out Vector3 normal, out Vector3 pos )
		{
			BoundingBox		bbox;
			AffineTransform identity	= AffineTransform.Identity;
			RigidTransform	transform	= new RigidTransform( xform.Translation + initOffset, Quaternion.CreateFromRotationMatrix(xform) );

			normal	= Vector3.Zero;
			pos		= xform.Translation + initOffset + sweep;

            shape.GetSweptLocalBoundingBox( ref transform, ref identity, ref sweep, out bbox );

			var candidates = Resources.GetCollisionEntryList();
            Space.BroadPhase.QueryAccelerator.BroadPhase.QueryAccelerator.GetEntries( bbox, candidates );

			float minT = float.MaxValue;
			bool  hit  = false;
			
			foreach ( var candidate in candidates )	{
				
				if ( candidate as ConvexCollidable != null ) {
					continue;
				}

				RayHit rayHit;
				bool r = candidate.ConvexCast( shape, ref transform, ref sweep, out rayHit );

				if (!r) continue;

				if ( minT > rayHit.T ) {
					hit   	= true;
					minT	= rayHit.T;
					normal	= rayHit.Normal;
					pos		= rayHit.Location;
				}
			}

			return hit;
		}
		#endif
	}
}
