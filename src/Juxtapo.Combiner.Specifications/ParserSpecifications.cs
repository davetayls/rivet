//  #######################################################
//  
//  # Copyright (C) 2010,  Dave Taylor and Arnold Zokas
//  
//  # This source code is subject to terms and conditions of the New BSD License.
//  # A copy of the license can be found in the license.txt file at the root of this distribution.
//  # If you cannot locate the New BSD License, please send an email to TODO or arnold.zokas@coderoom.net.
//  # By using this source code in any fashion, you are agreeing to be bound by the terms of the New BSD License.
//  # You must not remove this notice, or any other, from this software.
//  
//  #######################################################
using System;
using Juxtapo.Combiner.Specifications.TestUtils;
using Xunit;
using Xunit.Specifications;

namespace Juxtapo.Combiner.Specifications
{
	public class ParserSpecifications
	{
		[Specification]
		public void ParseSourceFiles()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles() throws ArgumentNullException when null is passed"
				.Assert(() => Assert.Throws<ArgumentNullException>(() => parser.ParseSourceFiles(null)));

			"ParseSourceFiles() throws InvalidOperationException when 0 source files are passed"
				.Assert(() => Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(new SourceFiles())));

			"ParseSourceFiles() returns a SourceFile when 1 source files are passed"
				.Assert(() => parser.ParseSourceFiles(new SourceFiles {new SourceFile(string.Empty)}).ShouldNotBeNull());

			"ParseSourceFiles() removes lines marked //## DEBUG from output SourceFile"
				.Assert(() => parser.ParseSourceFiles(new SourceFiles {new SourceFile("BEFORE\r\nTEST//##DEBUG\r\nAFTER\r\n")}).Body.ShouldEqual("BEFORE\r\nAFTER\r\n"));

			"ParseSourceFiles() removes blocks between //##DEBUG_START //##DEBUG_END tokens from output SourceFile"
				.Assert(() => parser.ParseSourceFiles(new SourceFiles {new SourceFile("BEFORE\r\n//##DEBUG_STARTTEST\r\n//##DEBUG_ENDAFTER\r\n")}).Body.ShouldEqual("BEFORE\r\nAFTER\r\n"));
		}
	}
}