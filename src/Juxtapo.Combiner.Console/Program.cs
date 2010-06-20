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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SysConsole = System.Console;

namespace Juxtapo.Combiner.Console
{
	public class Program
	{
		public static ConsoleParameters Parameters { get; private set; }

		public static void Main(string[] args)
		{
			Parameters = ConsoleParameterParser.Parse(args);

			if (Parameters.DisplayHelpInformation)
			{
				DisplayHelpInformation();
				return;
			}

			SourceFiles sourceFiles = GetSourceFiles();
			if (sourceFiles.Count == 0)
				return;

			ParserOptions parserOptions = GetParserOptions();

			Combine(sourceFiles, parserOptions);
		}

		private static void Combine(SourceFiles sourceFiles, ParserOptions parserOptions)
		{
			var parser = new Parser();
			var outputFiles = parser.ParseSourceFiles(sourceFiles, parserOptions);

			DeleteComponents(outputFiles);
		}

		private static void DeleteComponents(IEnumerable<SourceFile> outputFiles)
		{
			foreach (var outputFile in outputFiles)
			{
				File.WriteAllText(Path.Combine(Parameters.TargetDirectory, outputFile.Identity), outputFile.Body);

				foreach (var component in outputFile.Components)
				{
					var componentPath = Path.Combine(Parameters.TargetDirectory, component.Identity);
					if (File.Exists(componentPath))
					{
						File.Delete(componentPath);

						var componentDirectoryPath = Path.GetDirectoryName(componentPath);
						if (Directory.GetFiles(componentDirectoryPath).Length == 0)
						{
							Directory.Delete(componentDirectoryPath);
						}
					}
				}
			}
		}

		private static SourceFiles GetSourceFiles()
		{
			const int lengthOfDirectorySeparatorChar = 1;

			var sourceFiles = from path in Directory.GetFiles(Parameters.TargetDirectory, "*.js", SearchOption.AllDirectories)
			                  let identity = path.Remove(0, Parameters.TargetDirectory.Length + lengthOfDirectorySeparatorChar)
			                  let content = File.ReadAllText(path)
			                  select new SourceFile(identity, content);

			return new SourceFiles(sourceFiles);
		}

		private static ParserOptions GetParserOptions()
		{
			if (Parameters.Variables.Count == 0)
			{
				return ParserOptions.Default;
			}

			var parserOptions = new ParserOptions();
			foreach (var variable in Parameters.Variables)
			{
				parserOptions.AddVariable(variable);
			}

			return parserOptions;
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