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

namespace Rivet.Specifications
{
	public class ParserSpecifications
	{
		[Specification]
		public void InputValidationSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles throws ArgumentNullException when invoked with null".AssertThrows<ArgumentNullException>(() => parser.ParseSourceFiles(null, ParserOptions.Default));
			"ParseSourceFiles throws InvalidOperationException when invoked with source file containing a null Body"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile("filename.js", null)};
				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
			"ParseSourceFiles throws InvalidOperationException when invoked with source file containing a null Identity"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile(null, "@rivet")};
				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
			"ParseSourceFiles throws InvalidOperationExceptionwhen when invoked with source file containing an empty Identity"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile(string.Empty, "@rivet")};
				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
			"ParseSourceFiles throws InvalidOperationException when invoked with source files referencing a file that does not exist"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles {new SourceFile("filename.js", "@rivet includes.push(\"include.js\");")};
				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
			"ParseSourceFiles throws InvalidOperationException when invoked with an include file containing an empty Body"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles
				                                         		                  	{
				                                         		                  		new SourceFile("main.js", "@rivet includes.push(\"include.js\");"),
				                                         		                  		new SourceFile("include.js", string.Empty)
				                                         		                  	};
				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
			"ParseSourceFiles does not throw an exception when invoked with an empty source file that is not referenced by a Rivet file"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles
				        		                  	{
				        		                  		new SourceFile("main.js", "@rivet includes.push(\"include.js\");"),
				        		                  		new SourceFile("include.js", "TEST"),
				        		                  		new SourceFile("unreferenced.js", string.Empty)
				        		                  	};
				        		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default).Count.ShouldEqual(1);
				        	});
		}

		[Specification]
		public void ParseSourceFilesSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			SourceFiles parserOutput = null;
			"when ParseSourceFiles is invoked with SourceFiles"
				.Do(() =>
				    	{
				    		var sourceFiles = new SourceFiles
				    		                  	{
				    		                  		new SourceFile("main.js", "@rivet includes.push(\"include.js\"); includes.push(\"dir/include.js\"); includes.push(\"dir/dir/include.js\");"),
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
			"Body of output file is \"BEFORE\r\nAFTER\r\nBEFORE\r\nAFTER\r\nvar i = 1;var j = 2;\"".Assert(() => parserOutput.First().Body.ShouldEqual("BEFORE\r\nAFTER\r\nBEFORE\r\nAFTER\r\nvar i = 1;var j = 2;"));
			"first output file has 3 components".Assert(() => parserOutput.First().Components.Count.ShouldEqual(3));
			"Identity of first component of output file is \"include.js\"".Assert(() => parserOutput.First().Components[0].Identity.ShouldEqual("include.js"));
			"Identity of second component of output file is \"dir\\include.js\"".Assert(() => parserOutput.First().Components[1].Identity.ShouldEqual("dir\\include.js"));
			"Identity of third component of output file is \"dir\\dir\\include.js\"".Assert(() => parserOutput.First().Components[2].Identity.ShouldEqual("dir\\dir\\include.js"));
		}

		[Specification]
		public void ParseSourceFilesWithCommentsSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			SourceFiles parserOutput = null;
			"when ParseSourceFiles is invoked with SourceFiles containing include.push() expression in multiline comment"
				.Do(() =>
				    	{
				    		var sourceFiles = new SourceFiles
				    		                  	{
				    		                  		new SourceFile("main.js", @"@rivet
																			/*
																				example: includes.push(""include.js"");
																			*/
																			// example: includes.push(""include.js"");
																			includes.push(""include.js"");"),
				    		                  		new SourceFile("include.js", "TEST")
				    		                  	};

				    		parserOutput = parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				    	});

			"Body of output file is \"TEST\"".Assert(() => parserOutput.First().Body.ShouldEqual("TEST"));
		}

		[Specification]
		public void ParseNestedSourceFilesSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			SourceFiles parserOutput = null;
			"when ParseSourceFiles is invoked with SourceFiles containing nested references"
				.Do(() =>
				    	{
				    		var sourceFiles = new SourceFiles
				    		                  	{
				    		                  		new SourceFile("rivet-A.js", "@rivet includes.push(\"A.js\"); includes.push(\"rivet-B.js\");"),
				    		                  		new SourceFile("A.js", "A"),
				    		                  		new SourceFile("rivet-B.js", "@rivet includes.push(\"B.js\"); includes.push(\"rivet-C.js\");"),
				    		                  		new SourceFile("B.js", "B"),
				    		                  		new SourceFile("rivet-C.js", "@rivet includes.push(\"C1.js\"); includes.push(\"C2.js\");"),
				    		                  		new SourceFile("C1.js", "@VARIABLE"),
				    		                  		new SourceFile("C2.js", "C2"),
				    		                  	};
				    		var parserOptions = new ParserOptions();
				    		parserOptions.AddVariable(new Variable("VARIABLE", "C1"));

				    		parserOutput = parser.ParseSourceFiles(sourceFiles, parserOptions);
				    	});

			"output contains 3 SourceFile".Assert(() => parserOutput.Count.ShouldEqual(3));
			"group A nested references are resolved".Assert(() => parserOutput[0].Body.ShouldEqual("ABC1C2"));
			"Rivet file A is made up of 4 components".Assert(() => parserOutput[0].Components.Count.ShouldEqual(4));
			"group B nested references are resolved".Assert(() => parserOutput[1].Body.ShouldEqual("BC1C2"));
			"Rivet file B is made up of 3 components".Assert(() => parserOutput[1].Components.Count.ShouldEqual(3));
			"group C nested references are resolved".Assert(() => parserOutput[2].Body.ShouldEqual("C1C2"));
			"Rivet file C is made up of 2 components".Assert(() => parserOutput[2].Components.Count.ShouldEqual(2));
		}

		[Specification]
		public void DetectNestedSourceFilesCircularReferencesSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles throws InvalidOperationException when invoked with SourceFiles containing circular nested references"
				.AssertThrows<InvalidOperationException>(() =>
				                                         	{
				                                         		var sourceFiles = new SourceFiles
				                                         		                  	{
				                                         		                  		new SourceFile("rivet-A.js", "@rivet includes.push(\"rivet-B.js\");"),
				                                         		                  		new SourceFile("rivet-B.js", "@rivet includes.push(\"rivet-C.js\");"),
				                                         		                  		new SourceFile("rivet-C.js", "@rivet includes.push(\"rivet-A.js\");"),
				                                         		                  	};

				                                         		parser.ParseSourceFiles(sourceFiles, ParserOptions.Default);
				                                         	});
		}
	}
}