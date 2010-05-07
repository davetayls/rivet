﻿//  #######################################################
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
using Xunit;

namespace Juxtapo.Combiner.Specifications.TestUtils
{
	public static class AssertExtensions
	{
		public static void ShouldEqual<T>(this T actual, T expected)
		{
			Assert.Equal(expected, actual);
		}

		public static void ShouldNotBeNull<T>(this T target)
		{
			Assert.NotNull(target);
		}
	}
}