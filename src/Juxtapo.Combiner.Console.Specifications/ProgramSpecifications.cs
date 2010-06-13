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

				        			var helpText = string.Format(
				        				"{0}Juxtapo Combiner v0.0{0}" +
				        				"Copyright (C) 2010, Dave Taylor and Arnold Zokas{0}{0}" +
				        				"Usage: Juxtapo.Combiner.Console.exe [/help] <path> [options]{0}{0}" +
				        				"\t/help\tShows this help information{0}" +
										"\t<path>\tPath to directory containing javascript files{0}{0}" +
				        				"Options:{0}" +
										"\t-v:name=value\tReplaces token [name] with [value] in processed files.{0}" +
										"\t\t\tThis can be specified multiple times to replace{0}" +
										"\t\t\tmultiple tokens.{0}{0}" +
										"Example:{0}{0}" +
										"\tJuxtapo.Combiner.Console.exe D:\\website\\js -v:debug=false -v:trace=true{0}"
										, Environment.NewLine);
				        			session.StandardOutput.ShouldEqual(helpText);
				        		}
				        	});
		}
	}
}