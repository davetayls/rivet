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

namespace Juxtapo.Combiner.Console.Specifications.TestUtils
{
	internal class TempDirectory : IDisposable
	{
		private readonly string _path;

		public TempDirectory()
		{
			var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
			if (string.IsNullOrEmpty(tempDirectoryPath))
				throw new InvalidOperationException("Environment variable \"TEMP\" is not set");

			_path = System.IO.Path.Combine(tempDirectoryPath, Guid.NewGuid().ToString());
			Directory.CreateDirectory(_path);
		}

		public string Path
		{
			get { return _path; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (Directory.Exists(_path))
			{
				Directory.Delete(_path);
			}
		}

		#endregion
	}
}