﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSCBioIdentification.CachePopulateService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CachePopulateService.IPopulateCacheService", CallbackContract=typeof(PSCBioIdentification.CachePopulateService.IPopulateCacheServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IPopulateCacheService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPopulateCacheService/Run")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.ArrayList))]
        void Run(System.Collections.ArrayList fingerList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPopulateCacheService/Run2", ReplyAction="http://tempuri.org/IPopulateCacheService/Run2Response")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.ArrayList))]
        void Run2(System.Collections.ArrayList fingerList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPopulateCacheService/getFingerList", ReplyAction="http://tempuri.org/IPopulateCacheService/getFingerListResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(string), Action="http://tempuri.org/IPopulateCacheService/getFingerListStringFault", Name="string", Namespace="http://schemas.microsoft.com/2003/10/Serialization/")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.ArrayList))]
        System.Collections.ArrayList getFingerList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPopulateCacheService/getExpirationTime", ReplyAction="http://tempuri.org/IPopulateCacheService/getExpirationTimeResponse")]
        System.DateTime getExpirationTime();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPopulateCacheService/Terminate", ReplyAction="http://tempuri.org/IPopulateCacheService/TerminateResponse")]
        void Terminate();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPopulateCacheServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPopulateCacheService/RespondWithRecordNumbers")]
        void RespondWithRecordNumbers(int num);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPopulateCacheService/RespondWithText")]
        void RespondWithText(string str);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPopulateCacheService/RespondWithError")]
        void RespondWithError(string str);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPopulateCacheService/CacheOperationComplete")]
        void CacheOperationComplete();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPopulateCacheServiceChannel : PSCBioIdentification.CachePopulateService.IPopulateCacheService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PopulateCacheServiceClient : System.ServiceModel.DuplexClientBase<PSCBioIdentification.CachePopulateService.IPopulateCacheService>, PSCBioIdentification.CachePopulateService.IPopulateCacheService {
        
        public PopulateCacheServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PopulateCacheServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PopulateCacheServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PopulateCacheServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PopulateCacheServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Run(System.Collections.ArrayList fingerList) {
            base.Channel.Run(fingerList);
        }
        
        public void Run2(System.Collections.ArrayList fingerList) {
            base.Channel.Run2(fingerList);
        }
        
        public System.Collections.ArrayList getFingerList() {
            return base.Channel.getFingerList();
        }
        
        public System.DateTime getExpirationTime() {
            return base.Channel.getExpirationTime();
        }
        
        public void Terminate() {
            base.Channel.Terminate();
        }
    }
}
