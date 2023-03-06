using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstracts
{
    public abstract class RequestBase<BL>
    {
        public DateTime dtServerTime
        {
            get
            {
                return DateTime.Now;
            }
        }
        public List<BL> LModels { get; set; }
        public BL OModel { get; set; }
        public bool bIsDeleted { get; set; }
        public bool bIsSaved { get; set; }
        public bool bIsEdit { get; set; }
        public int iIsActive { get; set; } = -1;
        public long LoggedInUserID { get; set; }
        public string sIpAddress { get; set; }
        public abstract void GetInitObject(int Id);
        public abstract void GetInit();
        public abstract void GetList(string Id);
        public abstract void vSave(BL newObj);
        public abstract void vSave(BL newObj, int Id);
        public abstract void vEdit(BL newObj, int Id);
        public abstract void vDelete(int Id);
        public abstract void vDelete(int IdObj,string id);
        public abstract void vSearch(List<string> searchObjs);
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
