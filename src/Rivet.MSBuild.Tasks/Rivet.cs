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
using Microsoft.Build.Framework;
using Rivet.Console;

namespace Rivet.MSBuild.Tasks
{
	public sealed class Rivet : ITask
	{
		[Required]
		public string TargetDirectory { get; set; }

		public string Variables { private get; set; }

		#region ITask Members

		public IBuildEngine BuildEngine { get; set; }

		public ITaskHost HostObject { get; set; }

		public bool Execute()
		{
			var runner = new Runner(new MSBuildLogWriter(BuildEngine), new MSBuildParameterParser());
			return runner.Execute(new[] {TargetDirectory, Variables ?? string.Empty});
		}

		#endregion
	}
}