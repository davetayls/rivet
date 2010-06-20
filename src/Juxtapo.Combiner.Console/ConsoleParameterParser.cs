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
using System.Text.RegularExpressions;

namespace Juxtapo.Combiner.Console
{
	internal static class ConsoleParameterParser
	{
		private static readonly Regex TargetDirectoryScanExpression;
		private static readonly Regex VariableScanExpression;

		static ConsoleParameterParser()
		{
			// should match "C:\temp"
			// should match "\\PC\c$\temp"
			TargetDirectoryScanExpression = new Regex(
				@"^" +			// beginning of string
				@"([A-Za-z]:" +	// "[drive_letter]:"
				@"|" +			// or
				@"\\\\.+)" +	// \\UNC_ROOT
				@"(\\(.+))*" +	// on or more "/subdirectory"
				@"$"			// end of string
				, RegexOptions.Singleline | RegexOptions.Compiled);

			// should match -v:debug=true
			VariableScanExpression = new Regex(
				@"-v:" + // -v:
				@"(?'key'.*)" + // match "key"
				@"=" + // =
				@"(?'value'.*)" // match "value"
				, RegexOptions.Singleline | RegexOptions.Compiled);
		}

		public static ConsoleParameters Parse(string[] args)
		{
			var parameters = new ConsoleParameters();

			// extract target directory
			var targetDirectoryMatch = TargetDirectoryScanExpression.Match(string.Join(" ", args));
			if (!targetDirectoryMatch.Success)
			{
				parameters.DisplayHelpInformation = true;
				return parameters;
			}

			parameters.TargetDirectory = targetDirectoryMatch.Value;

			// extract variables)
			foreach (var arg in args)
			{
				var matches = VariableScanExpression.Matches(arg);
				if (matches.Count == 1)
				{
					parameters.AddVariable(matches[0].Groups["key"].Value, matches[0].Groups["value"].Value);
				}
			}

			return parameters;
		}
	}
}