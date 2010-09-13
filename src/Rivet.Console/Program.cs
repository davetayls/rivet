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
namespace Rivet.Console
{
	// ReSharper disable ClassNeverInstantiated.Global
	public sealed class Program
	{
		private static ConsoleApp _consoleApp;

		public static ConsoleParameters Parameters
		{
			get { return _consoleApp.Parameters; }
		}

		public static void Main(string[] args)
		{
			_consoleApp = new ConsoleApp();
			_consoleApp.Execute(args);
		}
	}

	// ReSharper restore ClassNeverInstantiated.Global
}