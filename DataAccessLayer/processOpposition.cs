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
    
    public partial class processOpposition
    {
        public int processOppositionCode { get; set; }
        public Nullable<int> processCode { get; set; }
        public Nullable<int> oppositionTypeCode { get; set; }
        public string processOppositionReason { get; set; }
        public string processOppositionNotes { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userUpdateCode { get; set; }
        public Nullable<System.DateTime> dateUpdate { get; set; }
        public string ipUpdate { get; set; }
        public Nullable<bool> seen { get; set; }
        public Nullable<int> userCodeSeen { get; set; }
    
        public virtual oppositionType oppositionType { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual user user2 { get; set; }
        public virtual process process { get; set; }
    }
}