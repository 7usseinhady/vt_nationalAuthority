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
    using System.Collections.Generic;
    
    public partial class workerOfficeInsurance
    {
        public int workerOfficeInsuranceCode { get; set; }
        public int workerCode { get; set; }
        public int officeInsuranceCode { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userUpdateCode { get; set; }
        public Nullable<System.DateTime> dateUpdate { get; set; }
        public string ipUpdate { get; set; }
    
        public virtual officeInsurance officeInsurance { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual worker worker { get; set; }
    }
}