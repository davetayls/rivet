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
using System.Linq;
using System.Text.RegularExpressions;

namespace Rivet
{
	internal static class ReferenceLocator
	{
		private const string m_captureGroupName = "filename";
		private static readonly Regex IncludeLookupExpression;
		private static readonly Regex MultilineCommentExpression;

		static ReferenceLocator()
		{
			// should match:		includes.push("filename.js")
			// should not match:	includes.push("filename" + variable + ".js")
			// should not match:	// includes.push("example.js")
			var lookupPattern = string.Format("(?<!//.*)includes.push\\(\"(?'{0}'(.*?)\\.js)\"\\)", m_captureGroupName);
			IncludeLookupExpression = new Regex(lookupPattern, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

			MultilineCommentExpression = new Regex("/\\*(.*?)\\*/", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		internal static IEnumerable<string> FindReferences(string body)
		{
			var reducedBody = MultilineCommentExpression.Replace(body, string.Empty);

			return (from Match match in IncludeLookupExpression.Matches(reducedBody)
			        select match.Groups[m_captureGroupName].Value);
		}
	}
}