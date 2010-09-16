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
using System.IO;
using SysConsole = System.Console;

namespace Rivet.Console.Specifications.TestUtils
{
	internal class ConsoleSession : IDisposable
	{
		private readonly StringWriter _standardErrorWriter;
		private readonly StringWriter _standardOutputWriter;

		public ConsoleSession()
		{
			_standardOutputWriter = new StringWriter();
			_standardErrorWriter = new StringWriter();

			SysConsole.SetOut(_standardOutputWriter);
			SysConsole.SetError(_standardErrorWriter);
		}

		public string StandardOutput
		{
			get { return _standardOutputWriter.ToString(); }
		}

		public string StandardError
		{
			get { return _standardErrorWriter.ToString(); }
		}

		#region IDisposable Members

		public void Dispose()
		{
			_standardOutputWriter.Dispose();
		}

		#endregion
	}
}