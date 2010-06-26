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
using System.Linq;
using Xunit.Specifications;

namespace Juxtapo.Combiner.Specifications
{
	public class ParserSpecifications
	{
		[Specification]
		public void InputValidationSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles() throws ArgumentNullException when invoked with null".AssertThrows<ArgumentNullException>(() => parser.ParseSourceFiles(null));
			"ParseSourceFiles() throws InvalidOperationException when invoked with 0 source files".AssertThrows<InvalidOperationException>(() => parser.ParseSourceFiles(new SourceFiles()));
			"ParseSourceFiles() throws InvalidOperationException when invoked with source file containing a null Body"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile("filename.js", null)};
				                                         		parser.ParseSourceFiles(sourceFiles);
				                                         	});
			"ParseSourceFiles() throws InvalidOperationException when invoked with source file containing an empty Body"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile("filename.js", string.Empty)};
				                                         		parser.ParseSourceFiles(sourceFiles);
				                                         	});
			"ParseSourceFiles() throws InvalidOperationException when invoked with source file containing a null Identity"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile(null, "@juxtapo.combiner")};
				                                         		parser.ParseSourceFiles(sourceFiles);
				                                         	});
			"ParseSourceFiles() throws InvalidOperationExceptionwhen when invoked with source file containing an empty Identity"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile(string.Empty, "@juxtapo.combiner")};
				                                         		parser.ParseSourceFiles(sourceFiles);
				                                         	});
			"ParseSourceFiles() throws InvalidOperationException when invoked with source files not containing \"@juxtapo.combiner\" token"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile("filename.js", "NO_TOKEN")};
				                                         		parser.ParseSourceFiles(sourceFiles);
				                                         	});
		}

		[Specification]
		public void ParseSourceFilesSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			SourceFiles parserOutput = null;
			"when ParseSourceFiles() is invoked with SourceFiles"
				.Do(() =>
				    	{
				    		var sourceFiles = new SourceFiles
				    		                  	{
				    		                  		new SourceFile("main.js", "@juxtapo.combiner includes.push(\"include.js\"); includes.push(\"dir/include.js\"); includes.push(\"dir/dir/include.js\");"),
				    		                  		new SourceFile("include.js", "BEFORE\r\nTEST//##DEBUG\r\nAFTER\r\n"),
				    		                  		new SourceFile("dir\\include.js", "BEFORE\r\n//##DEBUG_STARTTEST\r\n//##DEBUG_ENDAFTER\r\n"),
				    		                  		new SourceFile("dir\\dir\\include.js", "var i = @VARIABLE_1;var j = @VARIABLE_2;")
				    		                  	};
				    		var parserOptions = new ParserOptions();
				    		parserOptions.AddVariable(new Variable("VARIABLE_1", "1"));
				    		parserOptions.AddVariable(new Variable("VARIABLE_2", "2"));

				    		parserOutput = parser.ParseSourceFiles(sourceFiles, parserOptions);
				    	});

			"output contains 1 SourceFile".Assert(() => parserOutput.Count.ShouldEqual(1));
			"Identity of output file is \"main.js\"".Assert(() => parserOutput.First().Identity.ShouldEqual("main.js"));
			"Body of output file is \"XXX\"".Assert(() => parserOutput.First().Body.ShouldEqual("BEFORE\r\nAFTER\r\nBEFORE\r\nAFTER\r\nvar i = 1;var j = 2;"));
			"first output file has 3 components".Assert(() => parserOutput.First().Components.Count.ShouldEqual(3));
			"Identity of first component of output file is \"include.js\"".Assert(() => parserOutput.First().Components[0].Identity.ShouldEqual("include.js"));
			"Identity of second component of output file is \"dir\\include.js\"".Assert(() => parserOutput.First().Components[1].Identity.ShouldEqual("dir\\include.js"));
			"Identity of third component of output file is \"dir\\dir\\include.js\"".Assert(() => parserOutput.First().Components[2].Identity.ShouldEqual("dir\\dir\\include.js"));
		}
	}
}