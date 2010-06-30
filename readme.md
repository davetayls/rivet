# juxtapo combiner #
## 1. Overview ##

*TODO*

## 2. Usage Instructions ##

### Development
1. 	Copy the contents of the /combiner/example-combiner.js for each file you want to 
	split in to separate files.
2.	Change the combinerFileName to filename of the combiner javascript file.
3.	Add includes.push("javascriptfile.js") lines for each of the files you want to
	include with this file.
4. Reference your combiner file in your markup.

When developing you can use the following combiner comments:
*	`//##DEBUG` at the end of a line will remove this line from the combined source
*	Wrapping lines with `//##DEBUGSTART` and `//##DEBUGEND` will remove the whole block from the combined source

You are also able to use variables `var versionNumber = '@VERSION_NUMBER'` within your javascript. 
For each variable add `-v:VERSION_NUMBER=1.0` when using the combiner command line

### On deployment using the commandline tool    
1.	Copy your javascript files in to a separate deployment directory
2.	Run the Juxtapo.Combiner.Console.exe pointing it to your deployment directory
	eg: Juxtapo.Combiner.Console.exe "c:\path\to\jsdirecory\"

		Usage: Juxtapo.Combiner.Console.exe [/help] <path> [options]
				/help   Shows this help information
				<path>  Path to directory containing javascript files

		Options:
				-v:name=value   Replaces token [name] with [value] in processed files.
								This can be specified multiple times to replace
								multiple tokens.

		Example:
				Juxtapo.Combiner.Console.exe D:\website\js -v:debug=false -v:trace=true

3.	This will add all the contents of the included javascript files in to
	the root combiner file and delete the referenced files.

## 3. Build Instructions ##

To build this project on your machine, run batch file <strong>./cfg/build.bat</strong>.

Build output is automatically placed into directory <strong>./_build/</strong>.

## 4. Related Links ##

*TODO*

## 5. License ##

Copyright (c) 2010, Dave Taylor and Arnold Zokas<br /><br />
This source code is subject to terms and conditions of the New BSD License.<br />
A copy of the license can be found in the license.txt file at the root of this distribution.<br />
If you cannot locate the New BSD License, please send an email to dave@the-taylors.org or arnold.zokas@coderoom.net.<br />
By using this source code in any fashion, you are agreeing to be bound by the terms of the New BSD License.<br />
You must not remove this notice, or any other, from this software.
<br />
<br />
Thanks for using juxtapo combiner,<br />
Dave Taylor and Arnold Zokas