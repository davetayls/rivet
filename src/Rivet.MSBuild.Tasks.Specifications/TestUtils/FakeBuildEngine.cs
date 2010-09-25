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
using System;
using System.Collections;
using Microsoft.Build.Framework;

namespace Rivet.MSBuild.Tasks.Specifications.TestUtils
{
	public class FakeBuildEngine : IBuildEngine
	{
		#region IBuildEngine Members

		public void LogErrorEvent(BuildErrorEventArgs e)
		{
			System.Console.WriteLine(e.Message);
		}

		public void LogWarningEvent(BuildWarningEventArgs e)
		{
			throw new NotImplementedException();
		}

		public void LogMessageEvent(BuildMessageEventArgs e)
		{
			System.Console.WriteLine(e.Message);
		}

		public void LogCustomEvent(CustomBuildEventArgs e)
		{
			throw new NotImplementedException();
		}

		public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
		{
			throw new NotImplementedException();
		}

		public bool ContinueOnError
		{
			get { throw new NotImplementedException(); }
		}

		public int LineNumberOfTaskNode
		{
			get { throw new NotImplementedException(); }
		}

		public int ColumnNumberOfTaskNode
		{
			get { throw new NotImplementedException(); }
		}

		public string ProjectFileOfTaskNode
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}