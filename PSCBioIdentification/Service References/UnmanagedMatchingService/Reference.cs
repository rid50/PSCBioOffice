﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSCBioIdentification.UnmanagedMatchingService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UnmanagedMatchingService.IMatchingService", CallbackContract=typeof(PSCBioIdentification.UnmanagedMatchingService.IMatchingServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IMatchingService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatchingService/fillCache2")]
        void fillCache2(string[] fingerList, int fingerListSize, string[] appSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/fillCache", ReplyAction="http://tempuri.org/IMatchingService/fillCacheResponse")]
        void fillCache(string[] fingerList, int fingerListSize, string[] appSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/match", ReplyAction="http://tempuri.org/IMatchingService/matchResponse")]
        uint match(string[] fingerList, int fingerListSize, byte[] probeTemplate, uint probeTemplateSize, string[] appSettings, ref System.Text.StringBuilder errorMessage, int messageSize);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/terminateMatchingService", ReplyAction="http://tempuri.org/IMatchingService/terminateMatchingServiceResponse")]
        void terminateMatchingService();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMatchingServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatchingService/RespondWithRecordNumbers")]
        void RespondWithRecordNumbers(int num);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatchingService/RespondWithText")]
        void RespondWithText(string str);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatchingService/RespondWithError")]
        void RespondWithError(string str);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatchingService/CacheOperationComplete")]
        void CacheOperationComplete();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMatchingServiceChannel : PSCBioIdentification.UnmanagedMatchingService.IMatchingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MatchingServiceClient : System.ServiceModel.DuplexClientBase<PSCBioIdentification.UnmanagedMatchingService.IMatchingService>, PSCBioIdentification.UnmanagedMatchingService.IMatchingService {
        
        public MatchingServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public MatchingServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public MatchingServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MatchingServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MatchingServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void fillCache2(string[] fingerList, int fingerListSize, string[] appSettings) {
            base.Channel.fillCache2(fingerList, fingerListSize, appSettings);
        }
        
        public void fillCache(string[] fingerList, int fingerListSize, string[] appSettings) {
            base.Channel.fillCache(fingerList, fingerListSize, appSettings);
        }
        
        public uint match(string[] fingerList, int fingerListSize, byte[] probeTemplate, uint probeTemplateSize, string[] appSettings, ref System.Text.StringBuilder errorMessage, int messageSize) {
            return base.Channel.match(fingerList, fingerListSize, probeTemplate, probeTemplateSize, appSettings, ref errorMessage, messageSize);
        }
        
        public void terminateMatchingService() {
            base.Channel.terminateMatchingService();
        }
    }
}
