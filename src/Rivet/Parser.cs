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
using Rivet.PreProcessors;
using Rivet.Resources;
using Rivet.Scanners;

namespace Rivet
{
	public sealed class Parser
	{
		private const string m_rivetToken = "@rivet";
		private readonly List<IPreProcessor> _preProcessors;

		public Parser()
		{
			// NOTE: Consider using constructor injection.
			_preProcessors = new List<IPreProcessor>();
			_preProcessors.Add(new DebugBlockPreProcessor());
			_preProcessors.Add(new DebugLinePreProcessor());
			_preProcessors.Add(new VariableReplacementPreProcessor());
		}

		public SourceFiles ParseSourceFiles(SourceFiles sourceFiles, ParserOptions parserOptions)
		{
			if (sourceFiles == null)
				throw new ArgumentNullException("sourceFiles");

			if (sourceFiles.Count == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_NoSourceFilesPassed);

			if (sourceFiles.Count(sourceFile => sourceFile.Body == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_SourceFileContainsNullBody);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_SourceFileContainsEmptyBody);

			if (sourceFiles.Count(sourceFile => sourceFile.Identity == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_SourceFileContainsNullFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.Identity.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_SourceFileContainsEmptyFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Contains(m_rivetToken)) == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_NoSourceFilesContainRivetToken);

			return ParseSourceFilesInternal(sourceFiles, parserOptions);
		}

		private SourceFiles ParseSourceFilesInternal(IEnumerable<SourceFile> sourceFiles, ParserOptions parserOptions)
		{
			IEnumerable<SourceFile> markedFiles = sourceFiles.Where(sourceFile => sourceFile.Body.Contains(m_rivetToken));
			var outputFiles = new SourceFiles();

			foreach (var markedFile in markedFiles)
			{
				var outputFile = ParseSourceFile(markedFile, sourceFiles, parserOptions);
				outputFiles.Add(outputFile);
			}

			return outputFiles;
		}

		private SourceFile ParseSourceFile(SourceFile markedFile, IEnumerable<SourceFile> sourceFiles, ParserOptions parserOptions)
		{
			var outputFile = new SourceFile(markedFile.Identity, string.Empty);

			foreach (var reference in IncludePushExpressionScanner.GetSourceFileReferences(markedFile.Body))
			{
				var include = sourceFiles.SingleOrDefault(x => x.Identity == reference);

				if (include != null)
				{
					if (include.Body.Contains(m_rivetToken))
					{
						outputFile.Body += ParseSourceFile(include, sourceFiles, parserOptions).Body;
					}
					else
					{
						outputFile.Body += _preProcessors.Aggregate(include.Body, (current, preProcessor) => preProcessor.Process(current, parserOptions));
					}

					outputFile.AddComponent(include);
				}
				else
					throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidOperationException__UnableToCombine_ReferenceNotFound, reference, markedFile.Identity));
			}

			return outputFile;
		}
	}
}