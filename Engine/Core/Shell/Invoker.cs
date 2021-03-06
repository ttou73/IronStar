﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Configuration;
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using System.Diagnostics;


namespace Fusion.Core.Shell {
	public partial class Invoker {

		/// <summary>
		/// Game reference.
		/// </summary>
		public Game Game { get; private set; }

		/// <summary>
		/// Invoker's context object to target invoker and commands to particular object.
		/// </summary>
		public object Context { get; private set; }

		Dictionary<string, CommandBinding> commands;

		object lockObject = new object();

		class CommandBinding {
			
			public readonly Type CommandType;
			public readonly CommandLineParser Parser;

			public CommandBinding ( Invoker invoker, Type type, CommandLineParser parser ) 
			{
				CommandType = type;
				Parser = parser;
			}
		}

		Queue<Command> queue	= new Queue<Command>(10000);
		Queue<Command> delayed	= new Queue<Command>(10000);
		Stack<Command> history	= new Stack<Command>(10000);


		/// <summary>
		/// Alphabetically sorted array of command names
		/// </summary>
		public string[] CommandList { get; private set; }


		/// <summary>
		/// Creates instance of Invoker.
		/// </summary>
		/// <param name="game">Game instance</param>
		public Invoker ( Game game )
		{
			Initialize( game, Command.GatherCommands() );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="types"></param>
		void Initialize ( Game game, Type[] types )
		{
			Context		=	null;
			Game		=	game;
			commands	=	types
						.Where( t1 => t1.IsSubclassOf(typeof(Command)) )
						.Where( t2 => t2.HasAttribute<CommandAttribute>() )
						.Select( t3 => new { Name = t3.GetCustomAttribute<CommandAttribute>().Name, Type = t3 } )
						.ToDictionary( pair => pair.Name, pair => new CommandBinding( this, pair.Type, new CommandLineParser( pair.Type, pair.Name ) ) );

			CommandList	=	commands.Select( cmd => cmd.Key ).OrderBy( name => name ).ToArray();
						
			Log.Message("Invoker: {0} commands found", commands.Count);
		}



		/// <summary>
		/// Parses and pushes command to the queue.
		/// http://stackoverflow.com/questions/6417598/why-subsequent-direct-method-call-is-much-faster-than-the-first-call
		/// </summary>
		/// <param name="commandLine"></param>
		/// <returns></returns>
		public Command Push ( string commandLine )
		{				  
			var argList	=	CommandLineParser.SplitCommandLine( commandLine ).ToArray();

			if (!argList.Any()) {
				throw new CommandLineParserException("Empty command line.");
			} 



			var cmdName	=	argList[0];
			argList		=	argList.Skip(1).ToArray();

			var	sw		=	new Stopwatch();
			
			sw.Start();

			lock (lockObject) {

				ConfigVariable variable;

				if (Game.Config.Variables.TryGetValue( cmdName, out variable )) {
					if (argList.Count()==0) {
						Log.Message("{0} = {1}", variable.Name, variable.Get() );
						return null;
					} else {
						return Push( string.Format("set {0} \"{1}\"", cmdName, string.Join(" ", argList) ) );
					}
				}

				var command	=	GetCommand( cmdName );
				var parser	=	GetParser( cmdName );

				string error;

				if (!parser.TryParseCommandLine( command, argList, out error )) {
					throw new CommandLineParserException("Failed to parse command line: " + error );
				}

				Push( command );

				sw.Stop();
				Log.Message("{0} ms", sw.Elapsed.TotalMilliseconds );

				return command;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public object PushAndExecute ( string commandLine )
		{
			var cmd = Push( commandLine );
			ExecuteQueue( new GameTime(), CommandAffinity.Default, true );
			return cmd.Result;
		}

		

		/// <summary>
		/// Parse given string and push parsed command to queue.
		/// </summary>
		/// <param name="command"></param>
		void Push ( Command command )
		{
			lock (lockObject) {
				if (queue.Any() && queue.Last().Terminal) {
					Log.Warning("Attempt to push command after terminal one. Ignored.");
					return;
				}
				queue.Enqueue( command );
			}
		}





		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal Command GetCommand ( string name )
		{
			CommandBinding binding;

			if (commands.TryGetValue( name, out binding )) {
				return (Command)Activator.CreateInstance( binding.CommandType, this );
			}
			
			throw new InvalidOperationException(string.Format("Unknown command '{0}'.", name));
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal CommandLineParser GetParser ( string name )
		{
			CommandBinding binding;

			if (commands.TryGetValue( name, out binding )) {
				return binding.Parser;
			}
			
			throw new InvalidOperationException(string.Format("Unknown command '{0}'.", name));
		}



		/// <summary>
		/// Executes enqueued commands. Updates delayed commands.
		/// </summary>
		/// <param name="gameTime"></param>
		public void ExecuteQueue ( GameTime gameTime, CommandAffinity affinity, bool forceDelayed = false )
		{
			var delta = (int)gameTime.Elapsed.TotalMilliseconds;

			lock (lockObject) {

				delayed.Clear();

				while (queue.Any()) {
					
					var cmd = queue.Dequeue();

					if ( cmd.Affinity == affinity ) {

						if ( cmd.Delay<=0 || forceDelayed ) {
							//	execute :
							cmd.Execute();

							if (cmd.Result!=null) {
								Log.Message( "// Result: {0} //", cmd.Result );
							}

							//	push to history :
							if (!cmd.NoRollback && cmd.Affinity==CommandAffinity.Default) {
								history.Push( cmd );
							}

						} else {

							cmd.Delay -= delta;

							delayed.Enqueue( cmd );

						}

					} else {
						
						delayed.Enqueue( cmd );

					}

				}

				Misc.Swap( ref delayed, ref queue );
			}
		}



		/// <summary>
		/// Undo one command.
		/// </summary>
		public bool Undo ()
		{
			lock (lockObject) {

				if (!history.Any()) {
					return false;
				}

				var cmd = history.Pop();
				cmd.Rollback();

				return true;
			}
		}



		/// <summary>
		/// Purges all history.
		/// </summary>
		public void PurgeHistory ()
		{
			lock (lockObject) {
				history.Clear();
			}
		}
	}
}
