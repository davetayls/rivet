﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Juxtapo.Combiner.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Juxtapo.Combiner.Resources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. None of the source files passed contain a @juxtapo.combiner token..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_NoSourceFilesContainCombinerToken {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_NoSourceFilesContainCombinerToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. No source files have been passed..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_NoSourceFilesPassed {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_NoSourceFilesPassed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. Source file contains an empty body..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_SourceFileContainsEmptyBody {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_SourceFileContainsEmptyBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. Source file contains an empty file name..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_SourceFileContainsEmptyFileName {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_SourceFileContainsEmptyFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. Source file contains a null body..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_SourceFileContainsNullBody {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_SourceFileContainsNullBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to combine. Source file contains a null file name..
        /// </summary>
        internal static string InvalidOperationException__UnableToCombine_SourceFileContainsNullFileName {
            get {
                return ResourceManager.GetString("InvalidOperationException__UnableToCombine_SourceFileContainsNullFileName", resourceCulture);
            }
        }
    }
}
