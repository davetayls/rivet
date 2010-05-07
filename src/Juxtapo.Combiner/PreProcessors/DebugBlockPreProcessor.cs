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
using System.Text.RegularExpressions;

namespace Juxtapo.Combiner.PreProcessors
{
	public class DebugBlockPreProcessor : IPreProcessor
	{
		private static readonly Regex Expression = new Regex(@"(//##DEBUG_START)([\s\S]*?)(//##DEBUG_END)", RegexOptions.Multiline | RegexOptions.Compiled);

		#region IPreProcessor Members

		public string Process(string body)
		{
			return Expression.Replace(body, string.Empty);
		}

		#endregion
	}
}