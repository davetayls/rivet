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
using Juxtapo.Combiner.Console.Specifications.TestUtils;
using Xunit.Specifications;
using SysConsole = System.Console;

namespace Juxtapo.Combiner.Console.Specifications
{
	public class ProgramSpecifications
	{
		[Specification]
		public void MainSpecifications()
		{
			"Given Program".Context();

			"Main() outputs help information to console when no command-line parameters are passed"
				.Assert(() =>
				        	{
				        		using (var session = new ConsoleSession())
				        		{
				        			Program.Main(new string[0]);

				        			var helpText = GenerateHelpText();
				        			session.StandardOutput.ShouldEqual(helpText);
				        		}
				        	});
		}

		private static string GenerateHelpText()
		{
			return string.Format(
				"Juxtapo Combiner v0.0{0}" +
				"Copyright (C) 2010, Dave Taylor and Arnold Zokas{0}{0}" +
				"Usage: Juxtapo.Combiner.Console.exe [/help] <path-to-directory> [options]{0}{0}" +
				"\t/help\t\tshows this help information{0}{0}" +
				"Options:{0}", Environment.NewLine);
		}
	}
}