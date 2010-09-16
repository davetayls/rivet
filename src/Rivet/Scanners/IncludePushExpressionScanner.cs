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

namespace Rivet.Scanners
{
	internal static class IncludePushExpressionScanner
	{
		private const string m_captureGroupName = "filename";
		private static readonly Regex IncludePushExpression;

		static IncludePushExpressionScanner()
		{
			// should match:		includes.push("filename.js")
			// should not match:	includes.push("filename" + variable + ".js")

			var pattern = string.Format("includes.push" +			// includes.push
			                            @"\(""" +					// ("
			                            @"(?'{0}'[^\""\""]*\.js)" + // filename.js		<= capture group
			                            @"""\)"						// ")
			                            , m_captureGroupName);

			IncludePushExpression = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Compiled);
		}

		internal static IEnumerable<string> GetSourceFileReferences(string body)
		{
			return (from Match match in IncludePushExpression.Matches(body)
					let reference = match.Groups[m_captureGroupName].Value
			        select reference.Replace('/', '\\'));
		}
	}
}