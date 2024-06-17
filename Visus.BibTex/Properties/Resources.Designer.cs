﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Visus.BibTex.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Visus.BibTex.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to An opening brace was expected at the begin of the BibTex body instead of &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorInvalidEntryBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidEntryBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The BibTex item builder has created an invalid item..
        /// </summary>
        internal static string ErrorInvalidEntryBuilt {
            get {
                return ResourceManager.GetString("ErrorInvalidEntryBuilt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected &quot;{0}&quot; was found at the begin of a field..
        /// </summary>
        internal static string ErrorInvalidFieldNameBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidFieldNameBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected &quot;{0}&quot; was found at the begin of a value. Field values must be enclosed in quotes, braces or must be literal numbers..
        /// </summary>
        internal static string ErrorInvalidFieldValueBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidFieldValueBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key of a BibTex item cannot be empty..
        /// </summary>
        internal static string ErrorInvalidKey {
            get {
                return ResourceManager.GetString("ErrorInvalidKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A comma was expected after the key of the BibTex item instead of &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorInvalidKeyEnd {
            get {
                return ResourceManager.GetString("ErrorInvalidKeyEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected &quot;{0}&quot; was found at the begin of the BibTex type..
        /// </summary>
        internal static string ErrorInvalidTypeBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidTypeBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name and the value of a field must be delimited by &quot;=&quot; instead of &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorMissingEquals {
            get {
                return ResourceManager.GetString("ErrorMissingEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The end of the input was reached prematurely..
        /// </summary>
        internal static string ErrorPrematureEnd {
            get {
                return ResourceManager.GetString("ErrorPrematureEnd", resourceCulture);
            }
        }
    }
}
