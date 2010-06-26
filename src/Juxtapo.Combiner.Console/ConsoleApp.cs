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
	public sealed class ConsoleApp
	{
		public ConsoleParameters Parameters { get; private set; }

		public void Execute(IEnumerable<string> args)
		{
			Parameters = ConsoleParameterParser.Parse(args);

			if (Parameters.DisplayHelpInformation)
			{
				DisplayHelpInformation();
				return;
			}

			if (!Directory.Exists(Parameters.TargetDirectory))
			{
				SysConsole.WriteLine("Directory \"{0}\" could not be found.", Parameters.TargetDirectory);
				return;
			}

			SourceFiles sourceFiles = GetSourceFiles(Parameters.TargetDirectory);
			// TODO: output source files

			if (sourceFiles.Count == 0)
			{
				// TODO: LOGIC: report empty directory
				return;
			}

			ParserOptions parserOptions = Parameters.ToParserOptions();
			// TODO: output variables

			var outputFiles = Combine(sourceFiles, parserOptions);

			SaveOutputFiles(outputFiles);
			DeleteComponents(outputFiles);
		}

		private void SaveOutputFiles(IEnumerable<SourceFile> outputFiles)
		{
			foreach (var outputFile in outputFiles)
			{
				// TODO: output save trace info
				File.WriteAllText(Path.Combine(Parameters.TargetDirectory, outputFile.Identity), outputFile.Body);
			}
		}

		private static SourceFiles GetSourceFiles(string sourceDirectoryPath)
		{
			const int lengthOfDirectorySeparatorChar = 1;
			const string fileSearchPattern = "*.js";

			var sourceFiles = from path in Directory.GetFiles(sourceDirectoryPath, fileSearchPattern, SearchOption.AllDirectories)
							  let identity = path.Remove(0, sourceDirectoryPath.Length + lengthOfDirectorySeparatorChar)
							  let content = File.ReadAllText(path)
							  select new SourceFile(identity, content);

			return new SourceFiles(sourceFiles);
		}

		private static IEnumerable<SourceFile> Combine(SourceFiles sourceFiles, ParserOptions parserOptions)
		{
			var parser = new Parser();
			return parser.ParseSourceFiles(sourceFiles, parserOptions);
		}

		private void DeleteComponents(IEnumerable<SourceFile> outputFiles)
		{
			foreach (var outputFile in outputFiles)
			{
				// delete components
				foreach (var component in outputFile.Components)
				{
					var componentPath = Path.Combine(Parameters.TargetDirectory, component.Identity);
					if (File.Exists(componentPath))
					{
						// TODO: output delete trace info
						File.Delete(componentPath);
					}
				}

				// delete empty subdirectories
				DeleteSubDirectories(Parameters.TargetDirectory);
			}
		}

		private static void DeleteSubDirectories(string targetDirectory)
		{
			const string fileSearchPattern = "*";
			var subDirectories = Directory.GetDirectories(targetDirectory);

			foreach (var subDirectory in subDirectories)
			{
				if (Directory.GetFiles(subDirectory, fileSearchPattern, SearchOption.AllDirectories).Any())
					DeleteSubDirectories(subDirectory);
				else
				{
					// TODO: output delete trace info
					Directory.Delete(subDirectory, recursive: true);
				}
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