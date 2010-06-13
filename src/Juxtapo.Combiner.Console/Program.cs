// #######################################################
// 
// # Copyright (C) 2010, Dave Taylor and Arnold Zokas
// 
// # This source code is subject to terms and conditions of the New BSD License.
// # A copy of the license can be found in the license.txt file at the root of this distribution.
// # If you cannot locate the New BSD License, please send an email to dave@the-taylors.org or arnold.zokas@coderoom.net.
// # By using this source code in any fashion, you are agreeing to be bound by the terms of the New BSD License.
// # You must not remove this notice, or any other, from this software.
// 
// #######################################################
using System;
using System.Reflection;
using SysConsole = System.Console;

namespace Juxtapo.Combiner.Console
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ConsoleParameters parameters = ConsoleParameterParser.Parse(args);

			if (parameters.DisplayHelpInformation)
			{
				DisplayHelpInformation();
				return;
			}
		}

		private static void DisplayHelpInformation()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			SysConsole.ForegroundColor = ConsoleColor.DarkCyan;
			SysConsole.WriteLine();
			SysConsole.WriteLine("Juxtapo Combiner v{0}.{1}", version.Major, version.Minor);
			SysConsole.WriteLine("Copyright (C) 2010, Dave Taylor and Arnold Zokas");
			SysConsole.WriteLine();
			SysConsole.ResetColor();
			SysConsole.WriteLine("Usage: Juxtapo.Combiner.Console.exe [/help] <path> [options]");
			SysConsole.WriteLine();
			SysConsole.WriteLine("\t/help\tShows this help information");
			SysConsole.WriteLine("\t<path>\tPath to directory containing javascript files");
			SysConsole.WriteLine();
			SysConsole.WriteLine("Options:");
			SysConsole.WriteLine("\t-v:name=value\tReplaces token [name] with [value] in processed files.");
			SysConsole.WriteLine("\t\t\tThis can be specified multiple times to replace");
			SysConsole.WriteLine("\t\t\tmultiple tokens.");
			SysConsole.WriteLine();
			SysConsole.WriteLine("Example:");
			SysConsole.WriteLine();
			SysConsole.WriteLine("\tJuxtapo.Combiner.Console.exe D:\\website\\js -v:debug=false -v:trace=true");
		}
	}
}