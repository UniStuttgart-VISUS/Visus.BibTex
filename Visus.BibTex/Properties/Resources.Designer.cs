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
        ///   Looks up a localized string similar to The builder is already constructing another BibTex entry. Commit previous entries by calling Build() before creating new ones..
        /// </summary>
        internal static string ErrorBuilderActive {
            get {
                return ResourceManager.GetString("ErrorBuilderActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a string variable cannot be empty..
        /// </summary>
        internal static string ErrorEmptyVariableName {
            get {
                return ResourceManager.GetString("ErrorEmptyVariableName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An illegal &quot;{0}&quot; was found in line {1}&quot;..
        /// </summary>
        internal static string ErrorIllegalCharacter {
            get {
                return ResourceManager.GetString("ErrorIllegalCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An opening brace was expected at the begin of the BibTex body instead of &quot;{0}&quot; in line {1}..
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
        ///   Looks up a localized string similar to An opening brace or parenthesis instead of &quot;{0}&quot; was expected at the begin of the string definition line {1}..
        /// </summary>
        internal static string ErrorInvalidStringBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidStringBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A closing &quot;{0}&quot; was expected in line {2}, but &quot;{1}&quot; was found..
        /// </summary>
        internal static string ErrorInvalidStringEnd {
            get {
                return ResourceManager.GetString("ErrorInvalidStringEnd", resourceCulture);
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
        ///   Looks up a localized string similar to An unexpected &quot;{0}&quot; was found at the begin of the name of a string variable in line {1}..
        /// </summary>
        internal static string ErrorInvalidVariableNameBegin {
            get {
                return ResourceManager.GetString("ErrorInvalidVariableNameBegin", resourceCulture);
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
        ///   Looks up a localized string similar to The builder does not have a valid entry to work with. Call Create() to make such an entry..
        /// </summary>
        internal static string ErrorNoItem {
            get {
                return ResourceManager.GetString("ErrorNoItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The end of the input was reached prematurely in line {1}..
        /// </summary>
        internal static string ErrorPrematureEnd {
            get {
                return ResourceManager.GetString("ErrorPrematureEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variable &quot;{0}&quot; found in line {1} was not found in the BibTex file or has not been injected via parser options..
        /// </summary>
        internal static string ErrorUnknownVariable {
            get {
                return ResourceManager.GetString("ErrorUnknownVariable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unmatched brace &quot;{0}&quot; was found in a string literal in line {1}..
        /// </summary>
        internal static string ErrorUnmatchedBraceInString {
            get {
                return ResourceManager.GetString("ErrorUnmatchedBraceInString", resourceCulture);
            }
        }
    }
}
