using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rivet
{
	internal static class ReferenceLocator
	{
		private const string m_captureGroupName = "filename";
		private static readonly Regex IncludePushExpression;

		static ReferenceLocator()
		{
			// should match:		includes.push("filename.js")
			// should not match:	includes.push("filename" + variable + ".js")

			var pattern = string.Format("includes.push" + // includes.push
			                            @"\(""" + // ("
			                            @"(?'{0}'[^\""\""]*\.js)" + // filename.js		<= capture group
			                            @"""\)" // ")
			                            , m_captureGroupName);

			IncludePushExpression = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Compiled);
		}

		internal static IEnumerable<string> FindReferences(string body)
		{
			return (from Match match in IncludePushExpression.Matches(body)
			        let reference = match.Groups[m_captureGroupName].Value
			        select reference.Replace('/', '\\'));
		}
	}
}