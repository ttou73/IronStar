﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Fusion.Core.Extensions;
using Fusion.Engine.Common;

namespace Fusion.Core.Shell {

	/// <summary>
	///		set /delay:1000 /norollback /var:"d3d_fullscr" /value:"true"
	/// </summary>
	public abstract class Command {

		[CommandLineParser.Ignore]
		public Game Game { get; private set; }

		[CommandLineParser.Ignore]
		public Invoker Invoker { get; private set; }

		/// <summary>
		/// Execute action
		/// </summary>
		public abstract void Execute ();

		/// <summary>
		/// Rollback action
		/// </summary>
		public abstract void Rollback ();


		/// <summary>
		/// Execution delay in milliseconds.
		/// </summary>
		[CommandLineParser.Name("delay")]
		public virtual int Delay { get; set; }


		/// <summary>
		/// Execution delay in milliseconds.
		/// </summary>
		[CommandLineParser.Name("norollback")]
		public virtual bool NoRollback { get; set; }


		/// <summary>
		/// Breaks execution of command queue.
		/// </summary>
		[CommandLineParser.Name("terminal")]
		public virtual bool Terminal { get; set; }


		/// <summary>
		/// Result of the command.
		/// </summary>
		[CommandLineParser.Ignore]
		public virtual string Result { get; protected set; }


		/// <summary>
		/// Command name.
		/// </summary>
		[CommandLineParser.Ignore]
		public string Name { get; private set; }


		/// <summary>
		/// Command affinity.
		/// </summary>
		[CommandLineParser.Ignore]
		public CommandAffinity Affinity { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		public Command ( Invoker invoker )
		{
			Invoker		=	invoker;
			Game		=	invoker.Game;
			Delay		=	0;
			NoRollback	=	false;
			Terminal	=	false;
			Result		=	null;

			Name		=	GetType().GetCustomAttribute<CommandAttribute>().Name;
			Affinity	=	GetType().GetCustomAttribute<CommandAttribute>().Affinity;
		}



		/// <summary>
		/// Returns list if suggested strings for given argument.
		/// By default this method returns values only for enums.
		/// Otherwice null.
		/// </summary>
		/// <param name="argType"></param>
		/// <param name="argName"></param>
		/// <returns></returns>
		public virtual IEnumerable<string> Suggest ( Type argumentType, string argumentName )
		{
			if (argumentType.IsEnum) {
				return Enum.GetNames( argumentType );
			}

			return new string[0];
		}


		/// <summary>
		/// Gets all avaialble commands.
		/// Command must be inherited from Command class and have attribute [Command]
		/// </summary>
		/// <returns></returns>
		internal static Type[] GatherCommands ()
		{
			return Misc.GetAllSubclassesOf( typeof(Command) )
				.Where( t => t.HasAttribute<CommandAttribute>() )
				.ToArray();
		}
	}
}