//  #######################################################
//  
//  # Copyright (C) 2010, Dave Taylor and Arnold Zokas
//  
//  # This source code is subject to terms and conditions of the New BSD License.
//  # A copy of the license can be found in the license.txt file at the root of this distribution.
//  # If you cannot locate the New BSD License, please send an email to dave@the-taylors.org or arnold.zokas@coderoom.net.
//  # By using this source code in any fashion, you are agreeing to be bound by the terms of the New BSD License.
//  # You must not remove this notice, or any other, from this software.
//  
//  #######################################################
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Juxtapo.Combiner.PreProcessors;
using Juxtapo.Combiner.Resources;

namespace Juxtapo.Combiner
{
	public class Parser
	{
		private const string IncludePushExpressionCaptureGroupName = "filename";
		private const string CombinerToken = "@juxtapo.combiner";
		private static readonly Regex IncludePushExpression = new Regex("includes.push\\(\\\"(?'" + IncludePushExpressionCaptureGroupName + "'[^\\\"\"]*\\.js)\\\"\\)", RegexOptions.Multiline | RegexOptions.Compiled); // TODO: this is just awful. refactor ASAP
		private readonly List<IPreProcessor> _preProcessors;

		public Parser()
		{
			_preProcessors = new List<IPreProcessor>();
			_preProcessors.Add(new DebugBlockPreProcessor());
			_preProcessors.Add(new DebugLinePreProcessor());
		}

		public SourceFiles ParseSourceFiles(SourceFiles sourceFiles)
		{
			if (sourceFiles == null)
				throw new ArgumentNullException("sourceFiles");

			if (sourceFiles.Count == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_NoSourceFilesPassed);

			if (sourceFiles.Count(sourceFile => sourceFile.Body == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_SourceFileContainsNullBody);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_SourceFileContainsEmptyBody);

			if (sourceFiles.Count(sourceFile => sourceFile.FileName == null) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_SourceFileContainsNullFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.FileName.Trim().Length == 0) > 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_SourceFileContainsEmptyFileName);

			if (sourceFiles.Count(sourceFile => sourceFile.Body.Contains(CombinerToken)) == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine_NoSourceFilesContainCombinerToken);

			return ParseSourceFilesInternal(sourceFiles);
		}

		private SourceFiles ParseSourceFilesInternal(IEnumerable<SourceFile> sourceFiles)
		{
			IEnumerable<SourceFile> markedFiles = sourceFiles.Where(sourceFile => sourceFile.Body.Contains(CombinerToken));
			IEnumerable<SourceFile> includes = sourceFiles.Where(sourceFile => !sourceFile.Body.Contains(CombinerToken));
			var outputFiles = new SourceFiles();

			foreach (var markedFile in markedFiles)
			{
				var body = string.Empty;

				foreach (Match match in IncludePushExpression.Matches(markedFile.Body))
				{
					var reference = match.Groups[IncludePushExpressionCaptureGroupName].Value;
					var include = includes.SingleOrDefault(x => x.FileName == reference);

					if (include != null)
					{
						body += _preProcessors.Aggregate(include.Body, (current, preProcessor) => preProcessor.Process(current));
					}
				}

				outputFiles.Add(new SourceFile(body, markedFile.FileName));
			}

			return outputFiles;
		}
	}
}