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

namespace Rivet
{
	public sealed class Parser
	{
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

			if (sourceFiles.Count(IsRivetFile) == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationException__UnableToCombine_NoSourceFilesContainRivetToken);

			return ParseSourceFilesInternal(sourceFiles, parserOptions);
		}

		private SourceFiles ParseSourceFilesInternal(IEnumerable<SourceFile> sourceFiles, ParserOptions parserOptions)
		{
			IEnumerable<SourceFile> rivetFiles = sourceFiles.Where(IsRivetFile);
			var outputFiles = new SourceFiles();

			foreach (var markedFile in rivetFiles)
			{
				var outputFile = ParseSourceFile(markedFile, sourceFiles, parserOptions);
				outputFiles.Add(outputFile);
			}

			return outputFiles;
		}

		private SourceFile ParseSourceFile(SourceFile origin, IEnumerable<SourceFile> sourceFiles, ParserOptions parserOptions)
		{
			var outputFile = new SourceFile(origin.Identity, string.Empty);
			var includeReferences = ReferenceLocator.FindReferences(origin.Body);

			foreach (var includeReference in includeReferences)
			{
				var include = ReferenceResolver.ResolveReference(origin.BasePath, includeReference, sourceFiles);

				if (include != null)
				{
					if (include.IsPredecessorOf(origin))
						throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidOperationException__UnableToCombine_CircularReferenceFound, include.Identity));

					if (IsRivetFile(include))
					{
						include.ParentComponent = origin;
						var nestedSourceFile = ParseSourceFile(include, sourceFiles, parserOptions);

						outputFile.Body += nestedSourceFile.Body;
						foreach (var component in nestedSourceFile.Components)
							outputFile.AddComponent(component);
					}
					else
					{
						outputFile.Body += _preProcessors.Aggregate(include.Body, (current, preProcessor) => preProcessor.Process(current, parserOptions));
						outputFile.AddComponent(include);
					}
				}
				else
					throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidOperationException__UnableToCombine_ReferenceNotFound, includeReference, origin.Identity));
			}

			return outputFile;
		}

		private static bool IsRivetFile(SourceFile sourceFile)
		{
			return sourceFile.Body.Contains(Constants.RivetToken);
		}
	}
}