﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSCBioIdentification {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Credentials : global::System.Configuration.ApplicationSettingsBase {
        
        private static Credentials defaultInstance = ((Credentials)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Credentials())));
        
        public static Credentials Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("t20UAlnTnEk=")]
        public string DBUser {
            get {
                return ((string)(this["DBUser"]));
            }
            set {
                this["DBUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("loUJHMQk2rhYC8VaSC0yXg==")]
        public string ServerName {
            get {
                return ((string)(this["ServerName"]));
            }
            set {
                this["ServerName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SqLELO90qD0tzjWylEecIg==")]
        public string DataBaseName {
            get {
                return ((string)(this["DataBaseName"]));
            }
            set {
                this["DataBaseName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tfjW3E9uUnH5CD3c0UG7L4psZN+3lT0GRCGU9a8o4ng=")]
        public string DBPass {
            get {
                return ((string)(this["DBPass"]));
            }
            set {
                this["DBPass"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IntegratedSecurity {
            get {
                return ((bool)(this["IntegratedSecurity"]));
            }
            set {
                this["IntegratedSecurity"] = value;
            }
        }
    }
}
