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
    
    public partial class worker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public worker()
        {
            this.manPowers = new HashSet<manPower>();
            this.medicalInsurances = new HashSet<medicalInsurance>();
            this.cardsRequestWorkers = new HashSet<cardsRequestWorker>();
            this.cardWorkerStopActives = new HashSet<cardWorkerStopActive>();
            this.workerContacts = new HashSet<workerContact>();
            this.workerDetails = new HashSet<workerDetail>();
            this.workerInjuries = new HashSet<workerInjury>();
            this.workerOfficeInsurances = new HashSet<workerOfficeInsurance>();
            this.workerPeriods = new HashSet<workerPeriod>();
            this.workerAttendances = new HashSet<workerAttendance>();
        }
    
        public int workerCode { get; set; }
        public string workerName { get; set; }
        public string workerNationalID { get; set; }
        public string workerInsuranceNum { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public Nullable<int> sectorCode { get; set; }
        public Nullable<int> genderCode { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userUpdateCode { get; set; }
        public Nullable<System.DateTime> dateUpdate { get; set; }
        public string ipUpdate { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<int> userCodeDelete { get; set; }
        public Nullable<System.DateTime> dateDelete { get; set; }
        public string natWsdlError { get; set; }
        public string natWsdlResponse { get; set; }
        public Nullable<System.DateTime> natWsdlDate { get; set; }
        public string insWsdlResponse { get; set; }
        public Nullable<System.DateTime> insWsdlDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<manPower> manPowers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<medicalInsurance> medicalInsurances { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cardsRequestWorker> cardsRequestWorkers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cardWorkerStopActive> cardWorkerStopActives { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerContact> workerContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerDetail> workerDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerInjury> workerInjuries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerOfficeInsurance> workerOfficeInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerPeriod> workerPeriods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerAttendance> workerAttendances { get; set; }
    }
}
