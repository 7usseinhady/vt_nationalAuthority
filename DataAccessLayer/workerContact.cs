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
    
    public partial class workerContact
    {
        public int workerContactCode { get; set; }
        public int workerCode { get; set; }
        public string phone { get; set; }
        public Nullable<bool> Freez { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
    
        public virtual user user { get; set; }
        public virtual worker worker { get; set; }
    }
}
