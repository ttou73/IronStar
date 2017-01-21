using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Shell;
using IronStar.Core;
using IronStar.Mapping;

namespace IronStar.Editor2 {
	
	/// <summary>
	/// Base command for all editor commands
	/// </summary>
	public abstract class EdCommand : Command {

		protected readonly MapEditor editor;
		
		public EdCommand ( Invoker invoker ) : base( invoker )
		{
			editor = (Game.GameEditor.Instance as MapEditor);

			if (editor==null) {
				throw new Exception("Editor is not running");
			}
		}
	}



	/// <summary>
	/// Saves current map
	/// </summary>
	[Command("ed-save", CommandAffinity.Default)]
	public class EdSave : EdCommand {

		public EdSave ( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute()
		{
			editor.SaveMap();
		}

		public override void Rollback()
		{
		}
	}



	/// <summary>
	/// Creates new map factory
	/// </summary>
	[Command("ed-create", CommandAffinity.Default)]
	public class EdCreate : EdCommand {

		[CommandLineParser.Name("select")]
		public bool Select { get; set; }

		[CommandLineParser.Required]
		public string FactoryType { get; set; }

		MapFactory factory;

		public EdCreate ( Invoker invoker ) : base( invoker )
		{
		}

		public override void Execute()
		{
			var factType		=	EntityFactory.GetFactoryTypes().FirstOrDefault( ft => ft.Name == FactoryType );

			if (factType==null) {
				throw new Exception(string.Format("Entity factory type {0} not found", FactoryType));
			}

			factory			=	new MapFactory();
			factory.Factory	=	(EntityFactory)Activator.CreateInstance( factType );
			factory.Guid	=	Guid.NewGuid();

			editor.Map.Factories.Add( factory );

			Log.Message("Map factory created: {0}", factory.ToString() );
		}

		public override void Rollback()
		{
			(Game.GameEditor.Instance as MapEditor).Map.Factories.Remove( factory );
		}
	}



	/// <summary>
	/// Toggle selection on given object
	/// </summary>
	[Command("ed-select", CommandAffinity.Default)]
	public class EdSelect : EdCommand {

		[CommandLineParser.Required]
		public string TargetGuid { get; set; } = "";

		MapFactory item;
		bool oldState;


		public EdSelect ( Invoker invoker ) : base( invoker )
		{
		}

		public override void Execute()
		{
			var guid = Guid.Parse( TargetGuid );
			item = editor.Map.Factories.First( f => f.Guid==guid );

			oldState		=	item.Selected;
			item.Selected	=	!item.Selected;
		}

		public override void Rollback()
		{
			item.Selected	=	oldState;
		}
	}



	/// <summary>
	/// Selects all
	/// </summary>
	[Command("ed-select-all", CommandAffinity.Default)]
	public class EdSelectAll : EdCommand {

		MapFactory[] oldSelection;

		public EdSelectAll ( Invoker invoker ) : base( invoker )
		{
			oldSelection	=	editor.GetSelection();
		}

		public override void Execute()
		{
			foreach (var item in editor.Map.Factories) {
				item.Selected = true;
			}
		}

		public override void Rollback()
		{
			foreach (var item in editor.Map.Factories) {
				item.Selected = false;
			}

			foreach (var item in oldSelection) {
				item.Selected = true;
			}
		}
	}



	/// <summary>
	/// Selects none
	/// </summary>
	[Command("ed-select-none", CommandAffinity.Default)]
	public class EdSelectNone : EdCommand {

		MapFactory[] oldSelection;

		public EdSelectNone ( Invoker invoker ) : base( invoker )
		{
			oldSelection	=	editor.GetSelection();
		}

		public override void Execute()
		{
			foreach (var item in editor.Map.Factories) {
				item.Selected = false;
			}
		}

		public override void Rollback()
		{
			foreach (var item in editor.Map.Factories) {
				item.Selected = false;
			}

			foreach (var item in oldSelection) {
				item.Selected = true;
			}
		}
	}



	/// <summary>
	/// Deletes selected items
	/// </summary>
	[Command("ed-delete", CommandAffinity.Default)]
	public class EdDelete : EdCommand {

		MapFactory[] oldItems;


		public EdDelete ( Invoker invoker ) : base( invoker )
		{
			oldItems	=	editor.GetSelection();
		}

		public override void Execute()
		{
			foreach ( var item in oldItems ) {
				editor.Map.Factories.Remove( item );
			}
		}

		public override void Rollback()
		{
			foreach (var item in oldItems) {
				editor.Map.Factories.Add( item );
			}
		}
	}





}
