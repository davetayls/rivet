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
using Rivet.Console.Specifications.TestUtils;
using Rivet.MSBuild.Tasks.Specifications.TestUtils;
using Xunit.Specifications;

namespace Rivet.MSBuild.Tasks.Specifications
{
	public class ProgramSpecifications
	{
		[Specification]
		public void CombiningSpecifications()
		{
			Rivet task = null;
			"Given new Rivet task".Context(() => task = new Rivet {BuildEngine = new FakeBuildEngine()});

			"Execute returns false when invoked with invalid parameters".Assert(() => task.Execute().ShouldBeFalse());

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

				        			tempDirectory.CreateFile("main.js", @"@rivet 
																			includes.push(""dirWithComponentFile/include.js"");
																			includes.push(""dirWithNestedComponentFiles/include.js"");
																			includes.push(""dirWithNestedComponentFiles/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile/subdir/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile2/include.js"");
																			includes.push(""dirWithNestedComponentFilesAndStandaloneFile2/subdir/include.js"");
																		");

				        			tempDirectory.CreateFile("secondary.js", @"@rivet 
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

				        			task.TargetDirectory = tempDirectory.Path;
				        			task.Variables = "VARIABLE_1=false;VARIABLE_2=true";
				        			task.Execute().ShouldBeTrue();

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
	}
}