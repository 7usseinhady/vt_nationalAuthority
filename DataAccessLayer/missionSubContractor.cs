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
    
    public partial class missionSubContractor
    {
        public int missionSubContractorCode { get; set; }
        public int processSubContractorCode { get; set; }
        public int processTypeCode { get; set; }
    
        public virtual processType processType { get; set; }
        public virtual processSubContractor processSubContractor { get; set; }
    }
}
