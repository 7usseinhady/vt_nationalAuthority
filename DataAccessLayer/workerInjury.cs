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
    
    public partial class workerInjury
    {
        public int workerInjuriesCode { get; set; }
        public int workerCode { get; set; }
        public Nullable<System.DateTime> dateStartInjuries { get; set; }
        public Nullable<System.DateTime> dateEndInjuries { get; set; }
        public string injuriesReasons { get; set; }
        public Nullable<int> processCode { get; set; }
        public string injuriesNotes { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userUpdateCode { get; set; }
        public Nullable<System.DateTime> dateUpdate { get; set; }
        public string ipUpdate { get; set; }
        public Nullable<System.DateTime> dateInjuries { get; set; }
    
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual worker worker { get; set; }
        public virtual process process { get; set; }
    }
}
