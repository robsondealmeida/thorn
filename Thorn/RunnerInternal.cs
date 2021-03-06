﻿using System;

namespace Thorn
{
	internal class RunnerInternal : IRunner
	{
		private readonly HelpProvider _helpProvider;
		private readonly CommandProcessor _commandProcessor;

		public RunnerInternal(HelpProvider helpProvider, CommandProcessor commandProcessor)
		{
			_helpProvider = helpProvider;
			_commandProcessor = commandProcessor;
		}

		public void Run(string[] args)
		{
			try
			{
				var cmd = Command.Parse(args);

				if (_helpProvider.CanHandle(cmd))
				{
					_helpProvider.Handle(cmd);
				}
				else
				{
					_commandProcessor.Handle(cmd);
				}
			}
			catch (RoutingException ex)
			{
				Console.WriteLine("Unable to locate command '{0}'", ex.Command);
				_helpProvider.PrintHint();
			}
			catch (InvocationException ex)
			{
				Console.WriteLine("Unable to execute command: {0}", ex.Message);
				if(ex.InnerException != null)
				{
					Console.WriteLine();
					Console.WriteLine(ex.InnerException.ToString());
				}
			}
		}
	}
}