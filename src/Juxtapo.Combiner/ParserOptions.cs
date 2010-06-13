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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Juxtapo.Combiner
{
	[DebuggerDisplay("{Variables.Count}")]
	public sealed class ParserOptions
	{
		private static readonly ParserOptions DefaultParserOptions = new ParserOptions();
		private readonly IList<Variable> _variables;

		public ParserOptions()
		{
			_variables = new List<Variable>();
		}

		public ReadOnlyCollection<Variable> Variables
		{
			get { return new ReadOnlyCollection<Variable>(_variables); }
		}

		public static ParserOptions Default
		{
			get { return DefaultParserOptions; }
		}

		public void AddVariable(Variable variable)
		{
			_variables.Add(variable);
		}
	}
}