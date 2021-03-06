﻿// #######################################################
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

namespace Rivet.Console
{
	public sealed class RivetParameters
	{
		private readonly IList<Variable> _variables;

		public RivetParameters()
		{
			_variables = new List<Variable>();
		}

		public bool DisplayHelpInformation { get; set; }

		public ReadOnlyCollection<Variable> Variables
		{
			get { return new ReadOnlyCollection<Variable>(_variables); }
		}

		public string TargetDirectory { get; set; }

		public void AddVariable(string key, string value)
		{
			_variables.Add(new Variable(key, value));
		}

		public ParserOptions ToParserOptions()
		{
			var parserOptions = new ParserOptions();
			foreach (var variable in Variables)
			{
				parserOptions.AddVariable(variable);
			}

			return parserOptions;
		}
	}
}