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
using System.Collections.Generic;
using Juxtapo.Combiner.PreProcessors;

namespace Juxtapo.Combiner
{
	public class Parser
	{
		private readonly List<IPreProcessor> _preProcessors;

		public Parser()
		{
			_preProcessors = new List<IPreProcessor>();
			_preProcessors.Add(new DebugBlockPreProcessor());
			_preProcessors.Add(new DebugLinePreProcessor());
		}

		public SourceFile ParseSourceFiles(SourceFiles sourceFiles)
		{
			if (sourceFiles == null)
				throw new ArgumentNullException("sourceFiles");

			if (sourceFiles.Count == 0)
				throw new InvalidOperationException(ExceptionMessages.InvalidOperationExceptions__CannotCombine);

			string body = sourceFiles[0].Body;

			foreach (var preProcessor in _preProcessors)
			{
				body = preProcessor.Process(body);
			}

			return new SourceFile(body);
		}
	}
}