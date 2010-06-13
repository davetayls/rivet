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
			SysConsole.WriteLine("Juxtapo Combiner v{0}.{1}", version.Major, version.Minor);
			SysConsole.WriteLine("Copyright (C) 2010, Dave Taylor and Arnold Zokas{0}", Environment.NewLine);
			SysConsole.WriteLine("Usage: Juxtapo.Combiner.Console.exe [/help] <path-to-directory> [options]{0}", Environment.NewLine);
			SysConsole.Write("\t/help\t\tshows this help information{0}{0}", Environment.NewLine);
			SysConsole.WriteLine("Options:");
		}
	}
}