﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace apiMobile.prodAttWsdl {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="file://target.files", ConfigurationName="prodAttWsdl.CNTV06PortType")]
    public interface CNTV06PortType {
        
        // CODEGEN: Generating message contract since the operation CNTV06Operation is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="urn:CNTV06", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        apiMobile.prodAttWsdl.CNTV06OperationResponse CNTV06Operation(apiMobile.prodAttWsdl.CNTV06OperationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:CNTV06", ReplyAction="*")]
        System.Threading.Tasks.Task<apiMobile.prodAttWsdl.CNTV06OperationResponse> CNTV06OperationAsync(apiMobile.prodAttWsdl.CNTV06OperationRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.CNTV06I.com/schemas/CNTV06IInterface")]
    public partial class COMMAREA : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string cnttran_code_bField;
        
        private string cnttran_number_bField;
        
        private string cnttran_date_bField;
        
        private string cnt_number_kind_bField;
        
        private string cnt_number_bField;
        
        private string cnt_operation_num_bField;
        
        private string cnt_number_job_bField;
        
        private string cnt_result_code_bField;
        
        private string cnt_result_place_bField;
        
        private string work_date_bField;
        
        private string cnt_today_bField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string cnttran_code_b {
            get {
                return this.cnttran_code_bField;
            }
            set {
                this.cnttran_code_bField = value;
                this.RaisePropertyChanged("cnttran_code_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string cnttran_number_b {
            get {
                return this.cnttran_number_bField;
            }
            set {
                this.cnttran_number_bField = value;
                this.RaisePropertyChanged("cnttran_number_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string cnttran_date_b {
            get {
                return this.cnttran_date_bField;
            }
            set {
                this.cnttran_date_bField = value;
                this.RaisePropertyChanged("cnttran_date_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string cnt_number_kind_b {
            get {
                return this.cnt_number_kind_bField;
            }
            set {
                this.cnt_number_kind_bField = value;
                this.RaisePropertyChanged("cnt_number_kind_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string cnt_number_b {
            get {
                return this.cnt_number_bField;
            }
            set {
                this.cnt_number_bField = value;
                this.RaisePropertyChanged("cnt_number_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string cnt_operation_num_b {
            get {
                return this.cnt_operation_num_bField;
            }
            set {
                this.cnt_operation_num_bField = value;
                this.RaisePropertyChanged("cnt_operation_num_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string cnt_number_job_b {
            get {
                return this.cnt_number_job_bField;
            }
            set {
                this.cnt_number_job_bField = value;
                this.RaisePropertyChanged("cnt_number_job_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string cnt_result_code_b {
            get {
                return this.cnt_result_code_bField;
            }
            set {
                this.cnt_result_code_bField = value;
                this.RaisePropertyChanged("cnt_result_code_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string cnt_result_place_b {
            get {
                return this.cnt_result_place_bField;
            }
            set {
                this.cnt_result_place_bField = value;
                this.RaisePropertyChanged("cnt_result_place_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string work_date_b {
            get {
                return this.work_date_bField;
            }
            set {
                this.work_date_bField = value;
                this.RaisePropertyChanged("work_date_b");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string cnt_today_b {
            get {
                return this.cnt_today_bField;
            }
            set {
                this.cnt_today_bField = value;
                this.RaisePropertyChanged("cnt_today_b");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.CNTV06O.com/schemas/CNTV06OInterface")]
    public partial class DFHCOMMAREA : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string processing_statusdescField;
        
        private string processing_statuscodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string processing_statusdesc {
            get {
                return this.processing_statusdescField;
            }
            set {
                this.processing_statusdescField = value;
                this.RaisePropertyChanged("processing_statusdesc");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string processing_statuscode {
            get {
                return this.processing_statuscodeField;
            }
            set {
                this.processing_statuscodeField = value;
                this.RaisePropertyChanged("processing_statuscode");
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
    public partial class CNTV06OperationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.CNTV06I.com/schemas/CNTV06IInterface", Order=0)]
        public apiMobile.prodAttWsdl.COMMAREA COMMAREA;
        
        public CNTV06OperationRequest() {
        }
        
        public CNTV06OperationRequest(apiMobile.prodAttWsdl.COMMAREA COMMAREA) {
            this.COMMAREA = COMMAREA;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CNTV06OperationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.CNTV06O.com/schemas/CNTV06OInterface", Order=0)]
        public apiMobile.prodAttWsdl.DFHCOMMAREA DFHCOMMAREA;
        
        public CNTV06OperationResponse() {
        }
        
        public CNTV06OperationResponse(apiMobile.prodAttWsdl.DFHCOMMAREA DFHCOMMAREA) {
            this.DFHCOMMAREA = DFHCOMMAREA;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CNTV06PortTypeChannel : apiMobile.prodAttWsdl.CNTV06PortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CNTV06PortTypeClient : System.ServiceModel.ClientBase<apiMobile.prodAttWsdl.CNTV06PortType>, apiMobile.prodAttWsdl.CNTV06PortType {
        
        public CNTV06PortTypeClient() {
        }
        
        public CNTV06PortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CNTV06PortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CNTV06PortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CNTV06PortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        apiMobile.prodAttWsdl.CNTV06OperationResponse apiMobile.prodAttWsdl.CNTV06PortType.CNTV06Operation(apiMobile.prodAttWsdl.CNTV06OperationRequest request) {
            return base.Channel.CNTV06Operation(request);
        }
        
        public apiMobile.prodAttWsdl.DFHCOMMAREA CNTV06Operation(apiMobile.prodAttWsdl.COMMAREA COMMAREA) {
            apiMobile.prodAttWsdl.CNTV06OperationRequest inValue = new apiMobile.prodAttWsdl.CNTV06OperationRequest();
            inValue.COMMAREA = COMMAREA;
            apiMobile.prodAttWsdl.CNTV06OperationResponse retVal = ((apiMobile.prodAttWsdl.CNTV06PortType)(this)).CNTV06Operation(inValue);
            return retVal.DFHCOMMAREA;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<apiMobile.prodAttWsdl.CNTV06OperationResponse> apiMobile.prodAttWsdl.CNTV06PortType.CNTV06OperationAsync(apiMobile.prodAttWsdl.CNTV06OperationRequest request) {
            return base.Channel.CNTV06OperationAsync(request);
        }
        
        public System.Threading.Tasks.Task<apiMobile.prodAttWsdl.CNTV06OperationResponse> CNTV06OperationAsync(apiMobile.prodAttWsdl.COMMAREA COMMAREA) {
            apiMobile.prodAttWsdl.CNTV06OperationRequest inValue = new apiMobile.prodAttWsdl.CNTV06OperationRequest();
            inValue.COMMAREA = COMMAREA;
            return ((apiMobile.prodAttWsdl.CNTV06PortType)(this)).CNTV06OperationAsync(inValue);
        }
    }
}
