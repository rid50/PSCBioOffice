﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSCBioIdentification.MemoryCacheMatchingService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MatchingResult", Namespace="http://schemas.datacontract.org/2004/07/MemoryCacheService")]
    [System.SerializableAttribute()]
    public partial class MatchingResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<System.Tuple<string, int>> ResultField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<System.Tuple<string, int>> Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MemoryCacheMatchingService.IMatchingService")]
    public interface IMatchingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/getFingerList", ReplyAction="http://tempuri.org/IMatchingService/getFingerListResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(PSCBioIdentification.MemoryCacheMatchingService.MatchingResult))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<object>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<string>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<System.Tuple<string, int>>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Tuple<string, int>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Exception))]
        System.Collections.Generic.List<object> getFingerList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/Terminate", ReplyAction="http://tempuri.org/IMatchingService/TerminateResponse")]
        void Terminate(string guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/verify", ReplyAction="http://tempuri.org/IMatchingService/verifyResponse")]
        bool verify(byte[] probeTemplate, byte[] galleryTemplate, int matchingThreshold);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/match", ReplyAction="http://tempuri.org/IMatchingService/matchResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.Exception), Action="http://tempuri.org/IMatchingService/matchExceptionFault", Name="Exception", Namespace="http://schemas.datacontract.org/2004/07/System")]
        PSCBioIdentification.MemoryCacheMatchingService.MatchingResult match(string guid, System.Collections.Generic.List<string> fingerList, int gender, int firstMatch, byte[] probeTemplate, int matchingThreshold);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMatchingServiceChannel : PSCBioIdentification.MemoryCacheMatchingService.IMatchingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MatchingServiceClient : System.ServiceModel.ClientBase<PSCBioIdentification.MemoryCacheMatchingService.IMatchingService>, PSCBioIdentification.MemoryCacheMatchingService.IMatchingService {
        
        public MatchingServiceClient() {
        }
        
        public MatchingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MatchingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MatchingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MatchingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<object> getFingerList() {
            return base.Channel.getFingerList();
        }
        
        public void Terminate(string guid) {
            base.Channel.Terminate(guid);
        }
        
        public bool verify(byte[] probeTemplate, byte[] galleryTemplate, int matchingThreshold) {
            return base.Channel.verify(probeTemplate, galleryTemplate, matchingThreshold);
        }
        
        public PSCBioIdentification.MemoryCacheMatchingService.MatchingResult match(string guid, System.Collections.Generic.List<string> fingerList, int gender, int firstMatch, byte[] probeTemplate, int matchingThreshold) {
            return base.Channel.match(guid, fingerList, gender, firstMatch, probeTemplate, matchingThreshold);
        }
    }
}