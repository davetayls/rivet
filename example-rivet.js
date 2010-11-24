/*
	Rivet file.
    All of the items this file dynamically adds
    should be combined and compressed on deployment.
    
    The following line is used to tag this file as a combiner file
    @rivet[2.0]
    
    1. Change the combinerFileName to be the same as this javascript file
	   (This can be a string or a regular expression)
    2. Add includes.push("javascriptfile.js") line for each file you want to include
    
	After copying to a deployment directory run the Rivet.Console.exe
	pointing to it.
	eg: Rivet.Console.exe "path\to\jsdirecory\"
    
    Full documentation can be found at http://github.com/davetayls/rivet
---------------------------------------------------*/
(function() {

// UPDATE THE FOLLOWING
var combinerFileName = "example-rivet.js"; 	// this is the name or regEx to match the current file
var includes = [];							
includes.push("jsfile.js");					// specifies a file to be included
includes.push("folder/jsfile.js");

	


/* ********* DON'T CHANGE ANYTHING BELOW THIS LINE ******************************/
    var dirSeparator;
	function getJsLocation(jsFileName) {
		var location;
		if (typeof jsFileName === 'string') {jsFileName = new RegExp(jsFileName.toLowerCase());}
		// If used within a browser
		if (typeof document !== 'undefined' && typeof document.getElementsByTagName !== 'undefined'){
			var scriptFiles = document.getElementsByTagName("script"),
				found = false, i = 0;
			for (i=0;i<scriptFiles.length && !found;i+=1){
				var scriptTag = scriptFiles[i], scriptFileName = scriptTag.src.substring(scriptTag.src.lastIndexOf("/")+1).toLowerCase();
				if (jsFileName.test(scriptFileName)){
					location = scriptTag.src.substring(0,scriptTag.src.lastIndexOf("/")+1);
					found = true;
				}
			}
			if (!found) { throw 'Rivet could not match ' + jsFileName + ' to any embeded script references'; }
		} 
		// is used with rhino
		else if (typeof java !== 'undefined'){
			location = new java.io.File(environment['user.dir']).toString();
		} 
		// if used with cscript
		else if (WScript){
			location = WScript.CreateObject("WScript.Shell").CurrentDirectory;
		// cannot be used
		}else{
			throw 'Rivet could not retrieve the working directory';			
		}
		
		// return location string
		dirSeparator = location.indexOf('\\') === -1 ? '/' : '\\';
		location = location.substr(location.length-1,1) === dirSeparator || location === '' ? location : location+dirSeparator;
		return location;
    };
    var combinerJsLocation = getJsLocation(combinerFileName);
	function resolveAbsoluteUrl(baseUrl,relativeUrl) {
		try {
			if (relativeUrl.substr(0, 1) === '/') {
				return baseUrl.substring(0, baseUrl.indexOf('/', baseUrl.indexOf('//') + 2)) + relativeUrl;
			}else if (baseUrl === ''){
				return relativeUrl;
			}else {
				relativeUrl = dirSeparator === '/' ? relativeUrl : relativeUrl.replace(/\//g,'\\');
				var Loc = baseUrl;
				Loc = Loc.substring(0, Loc.lastIndexOf(dirSeparator));
				while (/^\.\./.test(relativeUrl)) {
					Loc = Loc.substring(0, Loc.lastIndexOf(dirSeparator));
					relativeUrl = relativeUrl.substring(3);
				}
				return Loc + dirSeparator + relativeUrl;
			}
		}catch (ex) {
			print('d:'+ex);
		}
	}
    function includeJs() {
		for (var i = 0; i < includes.length; i++) {
			var src = resolveAbsoluteUrl(combinerJsLocation, includes[i]);

			// If used within a browser
			if (typeof document !== 'undefined' && typeof document.getElementsByTagName !== 'undefined') {
				document.write("<script type=\"text/javascript\" src=\"" + src + "\"></script>");
			}
			// is used with rhino
			else if (typeof load !== 'undefined') {
				load(src);
			}
			// if used with cscript
			else if (typeof ActiveXObject !== 'undefined') {
				eval(new ActiveXObject("Scripting.FileSystemObject").OpenTextFile(src, 1).ReadAll());
			}
			// cannot be used
			else {
				throw ('Rivet combiner is not compatible with this JavaScript engine');
			}
		}
        return null;
    };
	includeJs();
})();
