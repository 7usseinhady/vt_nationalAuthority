﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vt_nationalAuthority.COVER_WSDL {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="file://target.files", ConfigurationName="COVER_WSDL.CNTV05PortType")]
    public interface CNTV05PortType {
        
        // CODEGEN: Generating message contract since the operation CNTV05Operation is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="urn:CNTV05", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse CNTV05Operation(vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:CNTV05", ReplyAction="*")]
        System.Threading.Tasks.Task<vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse> CNTV05OperationAsync(vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.CNTV05I.com/schemas/CNTV05IInterface")]
    public partial class COMMAREA2 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ws_arc_insnum_commField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ws_arc_insnum_comm {
            get {
                return this.ws_arc_insnum_commField;
            }
            set {
                this.ws_arc_insnum_commField = value;
                this.RaisePropertyChanged("ws_arc_insnum_comm");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.CNTV05O.com/schemas/CNTV05OInterface")]
    public partial class Commarea_buffer__01 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string comm_area_01Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string comm_area_01 {
            get {
                return this.comm_area_01Field;
            }
            set {
                this.comm_area_01Field = value;
                this.RaisePropertyChanged("comm_area_01");
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
    public partial class CNTV05OperationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.CNTV05I.com/schemas/CNTV05IInterface", Order=0)]
        public vt_nationalAuthority.COVER_WSDL.COMMAREA2 COMMAREA2;
        
        public CNTV05OperationRequest() {
        }
        
        public CNTV05OperationRequest(vt_nationalAuthority.COVER_WSDL.COMMAREA2 COMMAREA2) {
            this.COMMAREA2 = COMMAREA2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CNTV05OperationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.CNTV05O.com/schemas/CNTV05OInterface", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("buffer_01", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public vt_nationalAuthority.COVER_WSDL.Commarea_buffer__01[] COMMAREA;
        
        public CNTV05OperationResponse() {
        }
        
        public CNTV05OperationResponse(vt_nationalAuthority.COVER_WSDL.Commarea_buffer__01[] COMMAREA) {
            this.COMMAREA = COMMAREA;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CNTV05PortTypeChannel : vt_nationalAuthority.COVER_WSDL.CNTV05PortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CNTV05PortTypeClient : System.ServiceModel.ClientBase<vt_nationalAuthority.COVER_WSDL.CNTV05PortType>, vt_nationalAuthority.COVER_WSDL.CNTV05PortType {
        
        public CNTV05PortTypeClient() {
        }
        
        public CNTV05PortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CNTV05PortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CNTV05PortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CNTV05PortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse vt_nationalAuthority.COVER_WSDL.CNTV05PortType.CNTV05Operation(vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest request) {
            return base.Channel.CNTV05Operation(request);
        }
        
        public vt_nationalAuthority.COVER_WSDL.Commarea_buffer__01[] CNTV05Operation(vt_nationalAuthority.COVER_WSDL.COMMAREA2 COMMAREA2) {
            vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest inValue = new vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest();
            inValue.COMMAREA2 = COMMAREA2;
            vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse retVal = ((vt_nationalAuthority.COVER_WSDL.CNTV05PortType)(this)).CNTV05Operation(inValue);
            return retVal.COMMAREA;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse> vt_nationalAuthority.COVER_WSDL.CNTV05PortType.CNTV05OperationAsync(vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest request) {
            return base.Channel.CNTV05OperationAsync(request);
        }
        
        public System.Threading.Tasks.Task<vt_nationalAuthority.COVER_WSDL.CNTV05OperationResponse> CNTV05OperationAsync(vt_nationalAuthority.COVER_WSDL.COMMAREA2 COMMAREA2) {
            vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest inValue = new vt_nationalAuthority.COVER_WSDL.CNTV05OperationRequest();
            inValue.COMMAREA2 = COMMAREA2;
            return ((vt_nationalAuthority.COVER_WSDL.CNTV05PortType)(this)).CNTV05OperationAsync(inValue);
        }
    }
}