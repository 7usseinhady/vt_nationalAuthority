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
    
    public partial class processNote
    {
        public int processNotesCode { get; set; }
        public int processCode { get; set; }
        public string notes { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<bool> seen { get; set; }
    
        public virtual user user { get; set; }
        public virtual process process { get; set; }
    }
}
