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
		}
	}
}