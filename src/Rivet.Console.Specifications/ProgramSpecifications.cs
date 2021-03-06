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
using Xunit.Specifications;

namespace Rivet.Console.Specifications
{
	public class ProgramSpecifications
	{
		[Specification]
		public void MainSpecifications()
		{
			"Given Program".Context();

			"when Main is invoked with no parameters, DisplayHelpInformation parameter is set to true"
				.Assert(() =>
				        	{
				        		Program.Main(new string[0]);
				        		Program.Parameters.DisplayHelpInformation.ShouldBeTrue();
				        	});

			"Main returns 1 when invoked with no parameters".Assert(() => Program.Main(new string[0]).ShouldEqual(1));
			"Main returns 0 when invoked with parameter \"<path-to-dir>\""
				.Assert(() =>
				        	{
				        		using (var tempDirectory = new TempDirectory())
				        		{
				        			/* create filesystem strucure:
									 * %TEMP%\main.js
									 * %TEMP%\include.js
									 */

				        			tempDirectory.CreateFile("main.js", @"@rivet includes.push(""include.js"");");
				        			tempDirectory.CreateFile("include.js", @"A");

				        			Program.Main(new[] {tempDirectory.Path}).ShouldEqual(0);
				        		}
				        	});
		}
	}
}