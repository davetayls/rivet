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
using System.IO;
using System.Reflection;
using Juxtapo.Combiner.Console.Specifications.TestUtils;
using Xunit.Specifications;
using SysConsole = System.Console;

namespace Juxtapo.Combiner.Console.Specifications
{
	public class ConsoleAppSpecifications
	{
		[Specification]
		public void HelpSpecifications()
		{
			ConsoleApp consoleApp = null;
			var expectedOutput = string.Format(
				"{0}Usage: Juxtapo.Combiner.Console.exe [/help] <path> [options]{0}{0}" +
				"\t/help\tShows this help information{0}" +
				"\t<path>\tPath to directory containing javascript files{0}{0}" +
				"Options:{0}" +
				"\t-v:name=value\tReplaces token [name] with [value] in processed files.{0}" +
				"\t\t\tThis can be specified multiple times to replace{0}" +
				"\t\t\tmultiple tokens.{0}{0}" +
				"Example:{0}{0}" +
				"\tJuxtapo.Combiner.Console.exe D:\\website\\js -v:debug=false -v:trace=true{0}"
				, Environment.NewLine);

			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			var parameters = new string[0];
			"when Execute is invoked with no parameters".Do(() => consoleApp.Execute(parameters));

			"DisplayHelpInformation parameter is set to true".Assert(() => consoleApp.Parameters.DisplayHelpInformation.ShouldBeTrue());
			"help information is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			consoleApp.Execute(parameters);
				        			session.StandardOutput.ShouldContain(expectedOutput);
				        		}
				        	});
		}

		[Specification]
		public void VersionHeaderSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"version and copyright information is written to console"
				.Assert(() =>
				        	{
				        		var version = Assembly.GetAssembly(typeof (Parser)).GetName().Version;
								var expectedText = string.Format("\r\nJuxtapo Combiner v{0}.{1}\r\nCopyright (C) 2010, Dave Taylor and Arnold Zokas\r\n", version.Major, version.Minor);

				        		using (var session = new ConsoleSession())
				        		{
				        			consoleApp.Execute(new string[0]);
				        			session.StandardOutput.ShouldContain(expectedText);
				        		}
				        	});
		}

		[Specification]
		public void RuntimeInfoSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"when Execute is invoked, TargetDirectory path is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner");

				        				consoleApp.Execute(tempDirectory.Path);

				        				var expectedText = string.Format("\r\nTarget directory: {0}\r\n", tempDirectory.Path);
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
			"when Execute is invoked, list of discovered source files is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner");
				        				tempDirectory.CreateDirectory("dir");
				        				tempDirectory.CreateFile("dir\\include.js", "TEST");

				        				consoleApp.Execute(tempDirectory.Path);

				        				const string expectedText = "Discovered 2 javascript file(s):\r\n\t- main.js\r\n\t- dir\\include.js\r\n";
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
			"when Execute is invoked, list of parser options is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner");

				        				consoleApp.Execute(tempDirectory.Path, "-v:debug=false", "-v:trace=true");

				        				const string expectedText = "Variables:\r\n\t- debug=false\r\n\t- trace=true";
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
			"when Execute is invoked, path of combiner output file is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner");

				        				consoleApp.Execute(tempDirectory.Path);

				        				var expectedText = string.Format("\r\nSaved combined file: {0}\r\n", Path.Combine(tempDirectory.Path, "main.js"));
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
			"when Execute is invoked, list of deleted files is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner includes.push(\"dir/include.js\");");
				        				tempDirectory.CreateDirectory("dir");
				        				tempDirectory.CreateFile("dir\\include.js", "TEST");

				        				consoleApp.Execute(tempDirectory.Path);

				        				var expectedText = string.Format("\r\nDeleting components:\r\n\t- dir\\include.js");
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
			"when Execute is invoked, list of deleted directories is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				tempDirectory.CreateFile("main.js", "@juxtapo.combiner");
				        				tempDirectory.CreateDirectory("dir");
				        				tempDirectory.CreateDirectory("dir2");

				        				consoleApp.Execute(tempDirectory.Path);

				        				var expectedText = string.Format("\r\nDeleted empty subdirectory \"{0}\\dir\"\r\n\r\nDeleted empty subdirectory \"{0}\\dir2\"\r\n", tempDirectory.Path);
				        				session.StandardOutput.ShouldContain(expectedText);
				        			}
				        		}
				        	});
		}

		[Specification]
		public void ParameterParsingSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			string tempDirectoryPath = null;
			"when Execute is invoked with parameters \"<path-to-dir> -v:debug=false -v:trace=true\""
				.Do(() =>
				    	{
				    		using (var tempDirectory = new TempDirectory())
				    		{
				    			consoleApp.Execute(new[] {tempDirectory.Path, "-v:debug=false", "-v:trace=true"});
				    			tempDirectoryPath = tempDirectory.Path;
				    		}
				    	});

			"Parameters contain 2 variables".Assert(() => consoleApp.Parameters.Variables.Count.ShouldEqual(2));
			"key of first variable in Parameters is \"debug\"".Assert(() => consoleApp.Parameters.Variables[0].Key.ShouldEqual("debug"));
			"value of first variable in Parameters is \"false\"".Assert(() => consoleApp.Parameters.Variables[0].Value.ShouldEqual("false"));
			"key of second variable in Parameters is \"trace\"".Assert(() => consoleApp.Parameters.Variables[1].Key.ShouldEqual("trace"));
			"value of second variable in Parameters is \"true\"".Assert(() => consoleApp.Parameters.Variables[1].Value.ShouldEqual("true"));
			"DisplayHelpInformation parameter is set to \"false\"".Assert(() => consoleApp.Parameters.DisplayHelpInformation.ShouldBeFalse());
			"TargetDirectory parameter is set to \"{0}\"".FormatWith(tempDirectoryPath).Assert(() => consoleApp.Parameters.TargetDirectory.ShouldEqual(tempDirectoryPath));
		}

		[Specification]
		public void InvalidDirectoryParameterParsingSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"when Execute is invoked with parameters \"NOT_A_DIRECTORY_PATH\"".Do(() => consoleApp.Execute("NOT_A_DIRECTORY_PATH"));

			"DisplayHelpInformation parameter is set to \"true\"".Assert(() => consoleApp.Parameters.DisplayHelpInformation.ShouldBeTrue());
		}

		[Specification]
		public void MissingDirectoryParameterParsingSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"when Execute is invoked with parameter \"X:\\not_a_real_directory\", message \"Directory X:\\not_a_real_directory could not be found.\" is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			consoleApp.Execute("X:\\not_a_real_directory");
				        			session.StandardError.ShouldContain("Directory \"X:\\not_a_real_directory\" could not be found.");
				        		}
				        	});
		}

		[Specification]
		public void EmptyDirectoryParameterParsingSpecifications()
		{
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"when Execute is invoked with parameter pointing to an empty directory, message \"Directory <path-to-dir> does not contain any javascript files.\" is written to console"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			using (var tempDirectory = new TempDirectory())
				        			{
				        				consoleApp.Execute(tempDirectory.Path);
				        				session.StandardError.ShouldContain(string.Format("Directory \"{0}\" does not contain any javascript files.", tempDirectory.Path));
				        			}
				        		}
				        	});
		}

		[Specification]
		public void CombiningSpecifications()
		{
			// NOTE: this is a good candidate for refactoring. I need to implement some sort of API in xUnit.Specifications to simplify filesystem testing
			ConsoleApp consoleApp = null;
			"Given new ConsoleApp".Context(() => consoleApp = new ConsoleApp());

			"when Execute is invoked with parameters \"<path-to-dir> -v:debug=false -v:trace=true\", javascript files in the target directory are combined"
				.Assert(() =>
				        	{
				        		using (var tempDirectory = new TempDirectory())
				        		{
				        			/* create filesystem strucure:
									 * %TEMP%\main.js
									 * %TEMP%\secondary.js
									 * %TEMP%\dirWithComponentFile\include.js
									 * %TEMP%\dirWithStandaloneFile\standalone.js
									 * %TEMP%\dirWithNestedComponentFiles\include.js
									 * %TEMP%\dirWithNestedComponentFiles\subdir\include.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile\include.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile\standalone.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile\subdir\include.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile2\include.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile2\subdir\include.js
									 * %TEMP%\dirWithNestedComponentFilesAndStandaloneFile2\subdir\standalone.js
									 */

				        			tempDirectory.CreateFile("main.js", @"@juxtapo.combiner 
																			includes.push(""dirWithComponentFile/include.js"");
																			includes.push(""dirWithNestedComponentFiles/include.js"");
																			includes.push(""dirWithNestedComponentFiles/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile2/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile2/subdir/include.js"");
																		");

				        			tempDirectory.CreateFile("secondary.js", @"@juxtapo.combiner 
																			includes.push(""dirWithNestedComponentFiles/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile2/include.js"");
																		");

				        			tempDirectory.CreateDirectory("dirWithComponentFile");
				        			tempDirectory.CreateFile("dirWithComponentFile\\include.js", "BEFORE\r\n//##DEBUG_STARTTEST\r\n//##DEBUG_ENDAFTER\r\n");

				        			tempDirectory.CreateDirectory("dirWithStandaloneFile");
				        			tempDirectory.CreateFile("dirWithStandaloneFile\\standalone.js", "standalone js file");

				        			tempDirectory.CreateDirectory("dirWithNestedComponentFiles");
				        			tempDirectory.CreateFile("dirWithNestedComponentFiles\\include.js", "BEFORE_LINE\r\nTHIS_SHOULD_BE_REMOVED;//##DEBUGAFTER_LINE\r\n");
				        			tempDirectory.CreateDirectory("dirWithNestedComponentFiles\\subdir");
				        			tempDirectory.CreateFile("dirWithNestedComponentFiles\\subdir\\include.js", "A");

				        			tempDirectory.CreateDirectory("dirWithNestedComponentFilesAndStandaloneFile");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile\\include.js", "B");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile\\standalone.js", "standalone js file");
				        			tempDirectory.CreateDirectory("dirWithNestedComponentFilesAndStandaloneFile\\subdir");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile\\subdir\\include.js", "C");

				        			tempDirectory.CreateDirectory("dirWithNestedComponentFilesAndStandaloneFile2");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile2\\include.js", "D");
				        			tempDirectory.CreateDirectory("dirWithNestedComponentFilesAndStandaloneFile2\\subdir");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile2\\subdir\\include.js", "var i = @VARIABLE_1;var j = @VARIABLE_2;");
				        			tempDirectory.CreateFile("dirWithNestedComponentFilesAndStandaloneFile2\\subdir\\standalone.js", "standalone js file");

				        			consoleApp.Execute(new[] {tempDirectory.Path, " -v:VARIABLE_1=false", "-v:VARIABLE_2=true"});

				        			tempDirectory.ReadFile("main.js").ShouldEqual("BEFORE\r\nAFTER\r\nBEFORE_LINEAFTER_LINE\r\nABCDvar i = false;var j = true;");
				        			tempDirectory.ReadFile("secondary.js").ShouldEqual("ABCD");

				        			tempDirectory.DirectoryExists("dirWithComponentFile").ShouldBeFalse();
				        			tempDirectory.FileExists("dirWithStandaloneFile\\standalone.js").ShouldBeTrue();
				        			tempDirectory.DirectoryExists("dirWithNestedComponentFiles").ShouldBeFalse();
				        			tempDirectory.FileExists("dirWithNestedComponentFilesAndStandaloneFile\\standalone.js").ShouldBeTrue();
				        			tempDirectory.DirectoryExists("dirWithNestedComponentFilesAndStandaloneFile\\subdir").ShouldBeFalse();
				        			tempDirectory.DirectoryExists("dirWithNestedComponentFilesAndStandaloneFile2\\subdir").ShouldBeTrue();
				        		}
				        	});
		}

		private static string GetTempDirectoryPath()
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
			if (string.IsNullOrEmpty(tempDirectoryPath))
				throw new InvalidOperationException("Environment variable \"TEMP\" is not set");

			return tempDirectoryPath;
		}
	}
}