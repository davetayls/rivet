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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rivet
{
	internal static class ReferenceResolver
	{
		public static SourceFile ResolveReference(string basePath, string includeReference, IEnumerable<SourceFile> sourceFiles)
		{
			return sourceFiles.SingleOrDefault(x => string.Compare(Path.GetFullPath(x.Identity), CalculateAbsolutePath(basePath, includeReference), ignoreCase: true) == 0);
		}

		private static string CalculateAbsolutePath(string basePath, string includeReference)
		{
			return Path.GetFullPath(Path.Combine(basePath, includeReference.Replace('/', '\\')));
		}
	}
}