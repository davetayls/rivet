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
using Xunit;
using Xunit.Specifications;

namespace Juxtapo.Combiner.Specifications
{
	public class ParserSpecifications
	{
		[Specification]
		public void ParseSourceFilesInputValidationSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles() throws ArgumentNullException when null is passed"
				.Assert(() => Assert.Throws<ArgumentNullException>(() => parser.ParseSourceFiles(null)));

			"ParseSourceFiles() throws InvalidOperationException when 0 source files are passed"
				.Assert(() => Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(new SourceFiles())));

			"ParseSourceFiles() throws InvalidOperationException when any source file passed contains a null Body"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile("filename.js", null)};
				        		Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(sourceFiles));
				        	});

			"ParseSourceFiles() throws InvalidOperationException when any source file passed contains an empty Body"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile("filename.js", string.Empty)};
				        		Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(sourceFiles));
				        	});

			"ParseSourceFiles() throws InvalidOperationException when any source file passed contains a null Identity"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile(null, "@juxtapo.combiner")};
				        		Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(sourceFiles));
				        	});

			"ParseSourceFiles() throws InvalidOperationException when any source file passed contains an empty Identity"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile(string.Empty, "@juxtapo.combiner")};
				        		Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(sourceFiles));
				        	});

			"ParseSourceFiles() throws InvalidOperationException when none of source files passed contain @juxtapo.combiner token"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile("filename.js", "TEST")};
				        		Assert.Throws<InvalidOperationException>(() => parser.ParseSourceFiles(sourceFiles));
				        	});
		}

		[Specification]
		public void ParseSourceFilesSpecifications()
		{
			Parser parser = null;
			"Given new Parser".Context(() => parser = new Parser());

			"ParseSourceFiles() returns a SourceFile when 1 source files are passed"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile("filename.js", "@juxtapo.combiner")};
				        		parser.ParseSourceFiles(sourceFiles).ShouldNotBeNull();
				        	});

			"ParseSourceFiles() returns a SourceFile with a Identity"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles {new SourceFile("filename.js", "@juxtapo.combiner")};
				        		parser.ParseSourceFiles(sourceFiles).First().Identity.ShouldEqual("filename.js");
				        	});

			"ParseSourceFiles() includes files referenced using \"include\" token in the output SourceFile"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles();
				        		sourceFiles.Add(new SourceFile("second.js", "SECOND"));
				        		sourceFiles.Add(new SourceFile("first.js", "@juxtapo.combiner includes.push(\"second.js\"); includes.push(\"third.js\"); FIRST"));
				        		sourceFiles.Add(new SourceFile("third.js", "THIRD"));
				        		parser.ParseSourceFiles(sourceFiles).First().Body.ShouldEqual("SECONDTHIRD");
				        	});

			"ParseSourceFiles() adds files referenced using \"include\" token to Components of output SourceFile"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles();
				        		sourceFiles.Add(new SourceFile("second.js", "SECOND"));
				        		sourceFiles.Add(new SourceFile("first.js", "@juxtapo.combiner includes.push(\"third.js\"); FIRST"));
				        		sourceFiles.Add(new SourceFile("third.js", "THIRD"));
				        		parser.ParseSourceFiles(sourceFiles).First().Components.First().Identity.ShouldEqual("third.js");
				        	});

			"ParseSourceFiles() removes lines marked //## DEBUG from output SourceFile"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles();
				        		sourceFiles.Add(new SourceFile("first.js", "@juxtapo.combiner includes.push(\"second.js\"); FIRST"));
				        		sourceFiles.Add(new SourceFile("second.js", "BEFORE\r\nTEST//##DEBUG\r\nAFTER\r\n"));
				        		parser.ParseSourceFiles(sourceFiles).First().Body.ShouldEqual("BEFORE\r\nAFTER\r\n");
				        	});

			"ParseSourceFiles() removes blocks between //##DEBUG_START //##DEBUG_END tokens from output SourceFile"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles();
				        		sourceFiles.Add(new SourceFile("first.js", "@juxtapo.combiner includes.push(\"second.js\"); FIRST"));
				        		sourceFiles.Add(new SourceFile("second.js", "BEFORE\r\n//##DEBUG_STARTTEST\r\n//##DEBUG_ENDAFTER\r\n"));
				        		parser.ParseSourceFiles(sourceFiles).First().Body.ShouldEqual("BEFORE\r\nAFTER\r\n");
				        	});

			"ParseSourceFiles() populates variable placeholders"
				.Assert(() =>
				        	{
				        		var sourceFiles = new SourceFiles();
				        		sourceFiles.Add(new SourceFile("first.js", "@juxtapo.combiner includes.push(\"second.js\"); FIRST"));
				        		sourceFiles.Add(new SourceFile("second.js", "var i = @VARIABLE_1;var j = @VARIABLE_2;"));

				        		var options = new ParserOptions();
				        		options.AddVariable(new Variable("VARIABLE_1", "1"));
				        		options.AddVariable(new Variable("VARIABLE_2", "2"));

				        		parser.ParseSourceFiles(sourceFiles, options).First().Body.ShouldEqual("var i = 1;var j = 2;");
				        	});
		}
	}
}