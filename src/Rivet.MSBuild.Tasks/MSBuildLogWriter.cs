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
	public sealed class MSBuildLogWriter : ILogWriter
	{
		private readonly IBuildEngine _buildEngine;

		public MSBuildLogWriter(IBuildEngine buildEngine)
		{
			_buildEngine = buildEngine;
		}

		#region ILogWriter Members

		public void WriteMessage(string message)
		{
			_buildEngine.LogMessageEvent(new BuildMessageEventArgs(message, null, "Rivet", MessageImportance.Normal));
		}

		public void WriteImportantMessage(string message)
		{
			_buildEngine.LogMessageEvent(new BuildMessageEventArgs(message, null, "Rivet", MessageImportance.High));
		}

		public void WriteErrorMessage(string message)
		{
			_buildEngine.LogErrorEvent(new BuildErrorEventArgs(null, null, null, 0, 0, 0, 0, message, null, "Rivet"));
		}

		#endregion
	}
}