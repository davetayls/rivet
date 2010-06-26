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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Juxtapo.Combiner
{
	[DebuggerDisplay("{Identity}")]
	public sealed class SourceFile
	{
		private readonly SourceFiles _components;

		public SourceFile(string identity, string body)
		{
			Body = body;
			Identity = identity;
			_components = new SourceFiles();
		}

		public string Body { get; internal set; }
		public string Identity { get; private set; }

		// ReSharper disable ReturnTypeCanBeEnumerable.Global
		public ReadOnlyCollection<SourceFile> Components
		{
			get { return new ReadOnlyCollection<SourceFile>(_components); }
		}
		// ReSharper restore ReturnTypeCanBeEnumerable.Global

		internal void AddComponent(SourceFile component)
		{
			_components.Add(component);
		}
	}
}