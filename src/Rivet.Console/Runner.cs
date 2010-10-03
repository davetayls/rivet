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

namespace Rivet.Console
{
	public sealed class Runner
	{
		private readonly ILogWriter _logWriter;
		private readonly IParameterParser _parameterParser;

		public Runner(ILogWriter logWriter, IParameterParser parameterParser)
		{
			_logWriter = logWriter;
			_parameterParser = parameterParser;
		}

		public RivetParameters Parameters { get; private set; }

		public bool Execute(params string[] args)
		{
			try
			{
				DisplayVersionAndCopyrightInformation();

				Parameters = _parameterParser.Parse(args);

				if (Parameters.DisplayHelpInformation)
				{
					DisplayHelpInformation();
					return false;
				}

				if (!Directory.Exists(Parameters.TargetDirectory))
				{
					_logWriter.WriteErrorMessage(string.Format("Directory \"{0}\" could not be found.", Parameters.TargetDirectory));

					Parameters.DisplayHelpInformation = true;
					DisplayHelpInformation();
					return false;
				}
				DisplayTargetDirectory(Parameters.TargetDirectory);

				SourceFiles sourceFiles = GetSourceFiles(Parameters.TargetDirectory);
				if (sourceFiles.Count == 0)
				{
					_logWriter.WriteErrorMessage(string.Format("Directory \"{0}\" does not contain any javascript files.", Parameters.TargetDirectory));
					return false;
				}
				DisplayDiscoveredSourceFiles(sourceFiles);

				ParserOptions parserOptions = Parameters.ToParserOptions();
				DisplayParserOptions(parserOptions);

				var outputFiles = Combine(sourceFiles, parserOptions);

				SaveOutputFiles(outputFiles);
				DeleteComponents(outputFiles);

				return true;
			}
			catch (Exception ex)
			{
				_logWriter.WriteErrorMessage(ex.Message);
				return false;
			}
		}

		private void SaveOutputFiles(IEnumerable<SourceFile> outputFiles)
		{
			foreach (var outputFile in outputFiles)
			{
				var path = Path.Combine(Parameters.TargetDirectory, outputFile.Identity);
				File.WriteAllText(path, outputFile.Body);

				DisplaySavedOutputFilePath(path);
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
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("Deleting components:");

			foreach (var outputFile in outputFiles)
			{
				// delete components
				foreach (var component in outputFile.Components)
				{
					var componentPath = Path.Combine(Parameters.TargetDirectory, component.Identity);
					if (File.Exists(componentPath))
					{
						File.Delete(componentPath);
						_logWriter.WriteMessage(string.Format("\t- {0}", component.Identity));
					}
				}

				// delete empty subdirectories
				DeleteSubDirectories(Parameters.TargetDirectory);
			}
		}

		private void DeleteSubDirectories(string targetDirectory)
		{
			const string fileSearchPattern = "*";
			var subDirectories = Directory.GetDirectories(targetDirectory);

			foreach (var subDirectory in subDirectories)
			{
				if (Directory.GetFiles(subDirectory, fileSearchPattern, SearchOption.AllDirectories).Any())
					DeleteSubDirectories(subDirectory);
				else
				{
					Directory.Delete(subDirectory, recursive: true);
					_logWriter.WriteMessage(string.Empty);
					_logWriter.WriteMessage(string.Format("Deleted empty subdirectory \"{0}\"", subDirectory));
				}
			}
		}

		private void DisplayVersionAndCopyrightInformation()
		{
			var rivetAssembly = Assembly.GetAssembly(typeof (Parser));
			var version = rivetAssembly.GetName().Version;
			var title = GetAttribute<AssemblyDescriptionAttribute>(rivetAssembly).Description;
			var copyright = GetAttribute<AssemblyCopyrightAttribute>(rivetAssembly).Copyright;

			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteImportantMessage(string.Format("{0} v{1}.{2}", title, version.Major, version.Minor));
			_logWriter.WriteImportantMessage(copyright);
		}

		private void DisplayHelpInformation()
		{
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("Usage: Rivet.Console.exe [/help] <path> [options]");
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("\t/help\tShows this help information");
			_logWriter.WriteMessage("\t<path>\tPath to directory containing javascript files");
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("Options:");
			_logWriter.WriteMessage("\t-v:name=value\tReplaces token [name] with [value] in processed files.");
			_logWriter.WriteMessage("\t\t\tThis can be specified multiple times to replace");
			_logWriter.WriteMessage("\t\t\tmultiple tokens.");
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("Example:");
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("\tRivet.Console.exe D:\\website\\js -v:debug=false -v:trace=true");
		}

		private void DisplayTargetDirectory(string targetDirectory)
		{
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage(string.Format("Target directory: {0}", targetDirectory));
		}

		private void DisplayDiscoveredSourceFiles(SourceFiles sourceFiles)
		{
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage(string.Format("Discovered {0} javascript file(s):", sourceFiles.Count));

			foreach (var sourceFile in sourceFiles)
			{
				_logWriter.WriteMessage(string.Format("\t- {0}", sourceFile.Identity));
			}
		}

		private void DisplayParserOptions(ParserOptions parserOptions)
		{
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage("Variables:");

			foreach (var variable in parserOptions.Variables)
			{
				_logWriter.WriteMessage(string.Format("\t- {0}={1}", variable.Key, variable.Value));
			}
		}

		private void DisplaySavedOutputFilePath(string path)
		{
			_logWriter.WriteMessage(string.Empty);
			_logWriter.WriteMessage(string.Format("Saved combined file: {0}", path));
		}

		private static TAttribute GetAttribute<TAttribute>(Assembly assembly)
		{
			return ((TAttribute)assembly.GetCustomAttributes(typeof (TAttribute), inherit: false).First());
		}
	}
}