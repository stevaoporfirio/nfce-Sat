﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnviaSeFaz.RShomologacao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", ConfigurationName="RShomologacao.NfeAutorizacaoSoap")]
    public interface NfeAutorizacaoSoap {
        
        // CODEGEN: Generating message contract since the operation nfeAutorizacaoLote is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteResponse nfeAutorizacaoLote(EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteRequest request);
        
        // CODEGEN: Generating message contract since the operation nfeAutorizacaoLoteZip is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLoteZip", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipResponse nfeAutorizacaoLoteZip(EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
    public partial class nfeCabecMsg : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string cUFField;
        
        private string versaoDadosField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string cUF {
            get {
                return this.cUFField;
            }
            set {
                this.cUFField = value;
                this.RaisePropertyChanged("cUF");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string versaoDados {
            get {
                return this.versaoDadosField;
            }
            set {
                this.versaoDadosField = value;
                this.RaisePropertyChanged("versaoDados");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeAutorizacaoLoteRequest() {
        }
        
        public nfeAutorizacaoLoteRequest(EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeAutorizacaoLoteResult;
        
        public nfeAutorizacaoLoteResponse() {
        }
        
        public nfeAutorizacaoLoteResponse(EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeAutorizacaoLoteResult) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeAutorizacaoLoteResult = nfeAutorizacaoLoteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteZipRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public string nfeDadosMsgZip;
        
        public nfeAutorizacaoLoteZipRequest() {
        }
        
        public nfeAutorizacaoLoteZipRequest(EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, string nfeDadosMsgZip) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsgZip = nfeDadosMsgZip;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteZipResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeAutorizacaoLoteZipResult;
        
        public nfeAutorizacaoLoteZipResponse() {
        }
        
        public nfeAutorizacaoLoteZipResponse(EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeAutorizacaoLoteZipResult) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeAutorizacaoLoteZipResult = nfeAutorizacaoLoteZipResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NfeAutorizacaoSoapChannel : EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NfeAutorizacaoSoapClient : System.ServiceModel.ClientBase<EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap>, EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap {
        
        public NfeAutorizacaoSoapClient() {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeAutorizacaoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteResponse EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap.nfeAutorizacaoLote(EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteRequest request) {
            return base.Channel.nfeAutorizacaoLote(request);
        }
        
        public System.Xml.XmlNode nfeAutorizacaoLote(ref EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteRequest inValue = new EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteResponse retVal = ((EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLote(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeAutorizacaoLoteResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipResponse EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap.nfeAutorizacaoLoteZip(EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipRequest request) {
            return base.Channel.nfeAutorizacaoLoteZip(request);
        }
        
        public System.Xml.XmlNode nfeAutorizacaoLoteZip(ref EnviaSeFaz.RShomologacao.nfeCabecMsg nfeCabecMsg, string nfeDadosMsgZip) {
            EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipRequest inValue = new EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsgZip = nfeDadosMsgZip;
            EnviaSeFaz.RShomologacao.nfeAutorizacaoLoteZipResponse retVal = ((EnviaSeFaz.RShomologacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLoteZip(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeAutorizacaoLoteZipResult;
        }
    }
}
