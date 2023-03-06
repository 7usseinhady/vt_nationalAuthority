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
    
    public partial class process
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public process()
        {
            this.extractProcesses = new HashSet<extractProcess>();
            this.processNotes = new HashSet<processNote>();
            this.processOppositions = new HashSet<processOpposition>();
            this.processSites = new HashSet<processSite>();
            this.processStops = new HashSet<processStop>();
            this.processSubContractors = new HashSet<processSubContractor>();
            this.processUserLettersSites = new HashSet<processUserLettersSite>();
            this.workerAttendances = new HashSet<workerAttendance>();
            this.workerInjuries = new HashSet<workerInjury>();
            this.workerPeriods = new HashSet<workerPeriod>();
        }
    
        public int processCode { get; set; }
        public string processName { get; set; }
        public string processNumber { get; set; }
        public string processYear { get; set; }
        public Nullable<int> processTypeCode { get; set; }
        public Nullable<int> officeInsuranceCode { get; set; }
        public string incommingNumber { get; set; }
        public Nullable<System.DateTime> dateStartProcess { get; set; }
        public Nullable<System.DateTime> dateEndProcess { get; set; }
        public Nullable<System.DateTime> realDateStartProcess { get; set; }
        public Nullable<System.DateTime> realDateEndProcess { get; set; }
        public Nullable<int> referenceSideCode { get; set; }
        public Nullable<bool> isLimited { get; set; }
        public Nullable<decimal> fullBudget { get; set; }
        public Nullable<decimal> realFullBudget { get; set; }
        public Nullable<int> documentTypeCode { get; set; }
        public Nullable<System.DateTime> dateDocument { get; set; }
        public Nullable<int> minNumber { get; set; }
        public string nameOwnerPermision { get; set; }
        public string addressOwnerPermision { get; set; }
        public Nullable<int> maxNumber { get; set; }
        public Nullable<bool> processAccept { get; set; }
        public Nullable<System.DateTime> dateAccept { get; set; }
        public Nullable<int> userCodeAccept { get; set; }
        public string ipAcceptProcess { get; set; }
        public Nullable<byte> deleteProcess { get; set; }
        public Nullable<int> userDeleteCode { get; set; }
        public Nullable<System.DateTime> dateDelete { get; set; }
        public Nullable<int> userBackCode { get; set; }
        public Nullable<System.DateTime> dateBack { get; set; }
        public string ipBack { get; set; }
        public Nullable<int> userInsertCode { get; set; }
        public Nullable<System.DateTime> dateInsert { get; set; }
        public string ipInsert { get; set; }
        public Nullable<int> userUpdateCode { get; set; }
        public Nullable<System.DateTime> dateUpdate { get; set; }
        public string ipUpdate { get; set; }
        public Nullable<bool> seen { get; set; }
        public Nullable<bool> seenAcceptProcess { get; set; }
        public Nullable<bool> seenProcessEmployee { get; set; }
        public Nullable<bool> seenProcessEmployeeProcess { get; set; }
        public Nullable<int> UserCodeSeenProcess { get; set; }
        public Nullable<int> UserCodeSeenProcessStop { get; set; }
    
        public virtual documentType documentType { get; set; }
        public virtual officeInsurance officeInsurance { get; set; }
        public virtual processType processType { get; set; }
        public virtual referenceSideContractor referenceSideContractor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<extractProcess> extractProcesses { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual user user2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processNote> processNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processOpposition> processOppositions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processSite> processSites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processStop> processStops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processSubContractor> processSubContractors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<processUserLettersSite> processUserLettersSites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerAttendance> workerAttendances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerInjury> workerInjuries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workerPeriod> workerPeriods { get; set; }
    }
}
