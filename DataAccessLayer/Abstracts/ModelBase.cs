using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstracts
{
    public abstract class ModelBase<BL, EF>
    {
        public DateTime dtServerTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public bool bIsDeleted { get; set; }
        public bool bIsSaved { get; set; }
        public bool bIsEdit { get; set; }
        public Nullable<int> inUserInsertCode { get; set; }
        public Nullable<System.DateTime> dtDateInsert { get; set; }
        public string sIpInsert { get; set; }
        public Nullable<int> inUserUpdateCode { get; set; }
        public Nullable<System.DateTime> dtDateUpdate { get; set; }
        public string sIpUpdate { get; set; }
        public dynamic dyError { get; set; }
        public virtual user oUsers { get; set; }
        public virtual user oUsers1 { get; set; }

        //save 
        internal abstract bool bSave(BL newObj);
        internal abstract bool bSave(BL newObj, int Id);
        // Edit
        internal abstract bool bEdit(BL newObj, int Id);
        // Search
        internal abstract List<BL> lSearch(List<string> searchObjs);
        //Delete by id
        internal abstract bool bDelete(int Id);
        //GEt By Id
        internal abstract BL GetById(int Id);
        //Get all
        internal abstract List<BL> GetAll(int Id);
        internal abstract List<BL> GetAll();
        //convert entity to object
        internal abstract BL ConvertEFToObject(EF ef);
        //convert entity to basic  object
        internal abstract BL ConvertEFToObjectBasic(EF lEf);
        //convert entity to list of object
        internal abstract List<BL> ConvertEFsToObjects(List<EF> lEf);
        //convert entity to basic list of object
        internal abstract List<BL> ConvertEFsToObjectsBasic(List<EF> lEf);
        //convert object to entity 
        internal abstract EF ConvertObjectToEF(BL bl);

    }
}
