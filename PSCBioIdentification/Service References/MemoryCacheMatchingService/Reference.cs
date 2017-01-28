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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MemoryCacheMatchingService.IMatchingService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IMatchingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/getFingerList", ReplyAction="http://tempuri.org/IMatchingService/getFingerListResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Exception))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.ArrayList))]
        System.Collections.ArrayList getFingerList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/Terminate", ReplyAction="http://tempuri.org/IMatchingService/TerminateResponse")]
        int Terminate(string guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/verify", ReplyAction="http://tempuri.org/IMatchingService/verifyResponse")]
        bool verify(byte[] probeTemplate, byte[] galleryTemplate, int matchingThreshold);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMatchingService/match", ReplyAction="http://tempuri.org/IMatchingService/matchResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.Exception), Action="http://tempuri.org/IMatchingService/matchExceptionFault", Name="Exception", Namespace="http://schemas.datacontract.org/2004/07/System")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Exception))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.ArrayList))]
        uint match(string guid, System.Collections.ArrayList fingerList, int gender, byte[] probeTemplate, int matchingThreshold);
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
        
        public System.Collections.ArrayList getFingerList() {
            return base.Channel.getFingerList();
        }
        
        public int Terminate(string guid) {
            return base.Channel.Terminate(guid);
        }
        
        public bool verify(byte[] probeTemplate, byte[] galleryTemplate, int matchingThreshold) {
            return base.Channel.verify(probeTemplate, galleryTemplate, matchingThreshold);
        }
        
        public uint match(string guid, System.Collections.ArrayList fingerList, int gender, byte[] probeTemplate, int matchingThreshold) {
            return base.Channel.match(guid, fingerList, gender, probeTemplate, matchingThreshold);
        }
    }
}
