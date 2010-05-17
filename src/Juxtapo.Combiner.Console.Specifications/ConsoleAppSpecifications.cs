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
using Xunit.Specifications;

namespace Juxtapo.Combiner.Console.Specifications
{
	public class ConsoleAppSpecifications
	{
		[Specification]
		public void RunSpecifications()
		{
			int x;
			"Given static ConsoleApp".Context(() => x = 0); // NOTE: Waiting for optional context to be implemented in xUnit.net Specifications

			"Run() returns false when null is passed".Assert(() => ConsoleApp.Run(null).ShouldBeFalse());
			"Run() returns false when empty string[] is passed".Assert(() => ConsoleApp.Run(new string[] {}).ShouldBeFalse());
			"Run() returns true when when array with 1 string is passed".Assert(() => ConsoleApp.Run(new[] {""}).ShouldBeTrue());
		}
	}
}