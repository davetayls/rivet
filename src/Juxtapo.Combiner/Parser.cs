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
using System.Collections.Generic;
using System.Linq;
using Juxtapo.Combiner.PreProcessors;
using Juxtapo.Combiner.Resources;
using Juxtapo.Combiner.Scanners;

namespace Juxtapo.Combiner
{
	public sealed class Parser
	{
		private const string m_combinerToken = "@juxtapo.combiner";
		private readonly List<IPreProcessor> _preProcessors;

		public Parser()
		{
			// NOTE: A good candidate for constructor injection
			_preProcessors = new List<IPreProcessor>();
			_preProcessors.Add(new DebugBlockPreProcessor());
			_preProcessors.Add(new DebugLinePreProcessor());
			_preProcessors.Add(new VariableReplacementPreProcessor());
		}

		public SourceFiles ParseSourceFiles(SourceFiles sourceFiles)
		{
			return ParseSourceFiles(sourceFiles, ParserOptions.Default);
		}

		public SourceFiles ParseSourceFiles(SourceFiles sourceFiles, ParserOptions parserOptions)
		{
			if (sourceFiles == null)
				throw new ArgumentNullException("sourceFiles");

			if (sourceFiles.Count == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_NoSourceFilesPassed);

			if (sourceFiles.Count(sourceFile => sourceFile.Body == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_SourceFileContainsNullBody);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_SourceFileContainsEmptyBody);

			if (sourceFiles.Count(sourceFile => sourceFile.Identity == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_SourceFileContainsNullFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.Identity.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_SourceFileContainsEmptyFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Contains(m_combinerToken)) == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__CannotCombine_NoSourceFilesContainCombinerToken);

			return ParseSourceFilesInternal(sourceFiles, parserOptions);
		}

		private SourceFiles ParseSourceFilesInternal(IEnumerable<SourceFile> sourceFiles, ParserOptions parserOptions)
		{
			IEnumerable<SourceFile> markedFiles = sourceFiles.Where(sourceFile => sourceFile.Body.Contains(m_combinerToken));
			IEnumerable<SourceFile> includes = sourceFiles.Where(sourceFile => !sourceFile.Body.Contains(m_combinerToken));
			var outputFiles = new SourceFiles();

			foreach (var markedFile in markedFiles)
			{
				var outputFile = new SourceFile(markedFile.Identity, string.Empty);

				foreach (var reference in IncludePushExpressionScanner.GetFilenameReferences(markedFile.Body))
				{
					var include = includes.SingleOrDefault(x => x.Identity == reference);

					if (include != null)
					{
						outputFile.Body += _preProcessors.Aggregate(include.Body, (current, preProcessor) => preProcessor.Process(current, parserOptions));
						outputFile.AddComponent(include);
					}
				}

				outputFiles.Add(outputFile);
			}

			return outputFiles;
		}
	}
}