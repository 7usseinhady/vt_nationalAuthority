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
    
    public partial class attachment
    {
        public int attachmentsCode { get; set; }
        public int screenTypeCode { get; set; }
        public Nullable<int> screenCode { get; set; }
        public Nullable<int> officeCode { get; set; }
        public Nullable<int> contractorCode { get; set; }
        public Nullable<int> refSideCode { get; set; }
        public Nullable<bool> seen { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userCodeSeen { get; set; }
        public Nullable<System.DateTime> dateSeen { get; set; }
    
        public virtual screenType screenType { get; set; }
    }
}
