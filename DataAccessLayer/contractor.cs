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
    
    public partial class contractor
    {
        public int contractorCode { get; set; }
        public int legalEntityCode { get; set; }
        public string contractorInsuranceNum { get; set; }
        public string contractorName { get; set; }
        public Nullable<bool> isActive { get; set; }
    
        public virtual legalEntity legalEntity { get; set; }
    }
}
