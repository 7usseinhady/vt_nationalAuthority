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
    
    public partial class function
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public function()
        {
            this.function1 = new HashSet<function>();
            this.groupPermissions = new HashSet<groupPermission>();
        }
    
        public int functionCode { get; set; }
        public string functionName { get; set; }
        public Nullable<int> parentFunctionCode { get; set; }
        public int moduleCode { get; set; }
        public string functionPath { get; set; }
        public int treeColorCode { get; set; }
        public string ViewHtml { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<function> function1 { get; set; }
        public virtual function function2 { get; set; }
        public virtual module module { get; set; }
        public virtual treeColor treeColor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<groupPermission> groupPermissions { get; set; }
    }
}