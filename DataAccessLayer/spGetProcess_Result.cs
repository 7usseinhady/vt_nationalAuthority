//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    
    public partial class spGetProcess_Result
    {
        public int processCode { get; set; }
        public Nullable<int> officeInsuranceCode { get; set; }
        public string officeInsuranceID { get; set; }
        public Nullable<int> areaCode { get; set; }
        public string areaID { get; set; }
        public string processYear { get; set; }
        public string processNumber { get; set; }
        public string incommingNumber { get; set; }
        public string dateDocument { get; set; }
        public string processName { get; set; }
        public int processTypeCode { get; set; }
        public string processTypeName { get; set; }
        public string dateStartProcess { get; set; }
        public string dateEndProcess { get; set; }
        public Nullable<decimal> fullBudget { get; set; }
        public Nullable<int> ContactorCode { get; set; }
        public Nullable<int> legalEntityCont { get; set; }
        public string ContractorName { get; set; }
        public string ContractorNum { get; set; }
        public int RefSideCode { get; set; }
        public Nullable<int> legalEntityRefSide { get; set; }
        public string RefSideName { get; set; }
        public string RefSideNum { get; set; }
        public Nullable<int> documentTypeCode { get; set; }
        public Nullable<bool> isLimited { get; set; }
        public string nameOwnerPermision { get; set; }
        public string addressOwnerPermision { get; set; }
    }
}
