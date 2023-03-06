using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class ProcessSiteModel : ModelBase<ProcessSiteModel, processSite>
    {
        #region Process Site Fields عنوان العمليه
        public int iProcessSiteCode { get; set; } // كود عنوان العمليه
        public int iProcessCode { get; set; } // كود العمليه
        public Nullable<int> inBuildingNumberProcessSite { get; set; } // رقم العقار للعمليه
        public string sSiteAddressProcessSite { get; set; } // شارع - حاره
        public Nullable<int> inGovermentCodeProcessSite { get; set; } // كود المحافظه
        public Nullable<int> inCenterCodeProcessSite { get; set; } // كود المركز - القسم
        public Nullable<int> inVillageCodeProcessSite { get; set; } // كود الشياخه - القريه
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        ///   Get Object Of Process Site.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Process Site Model. </returns>
        internal override ProcessSiteModel GetById(int Id)
        {
            processSite OProcessSite = db.processSites.FirstOrDefault(x => x.processCode == Id);
            ProcessSiteModel oProcessSiteModel = new ProcessSiteModel();

            if (OProcessSite != null)
                oProcessSiteModel = this.ConvertEFToObjectBasic(OProcessSite);

            return oProcessSiteModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ProcessSiteModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<ProcessSiteModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Process Site Model. </returns>
        internal override List<ProcessSiteModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///    Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(ProcessSiteModel newObj, int Id)
        {
            try
            {
                if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessSite)))
                {
                    processSite modal = new processSite();

                    modal.processCode = Id; // كود العملية
                    modal.buildingNumber = newObj.inBuildingNumberProcessSite; // رقم العقار
                    modal.siteAddress = newObj.sSiteAddressProcessSite; // شارع - حاره
                    modal.govermentCode = newObj.inGovermentCodeProcessSite; // كود المحافظه
                    modal.centerCode = newObj.inCenterCodeProcessSite; // كود المركز - القسم
                    modal.villageCode = newObj.inVillageCodeProcessSite; // كود الشياخه - القريه
                    modal.userInsertCode = newObj.inUserInsertCode; // كود موظف الادخال
                    modal.dateInsert = DateTime.Now; // تاريخ الادخال
                    modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                    db.processSites.Add(modal);
                    if (db.SaveChanges() > 0)
                        return true;

                    return false;
                }

                // Becuase No Error
                // مفيش داتا تتخزن .. فبالتالى مش هرجع false 
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <returns></returns>
        internal override bool bSave(ProcessSiteModel newObj)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Process Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ProcessSiteModel newObj, int Id)
        {
            try
            {
                processSite modal = db.processSites.FirstOrDefault(x => x.processCode == Id);
                if (modal != null)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessSite)))
                    {
                        modal.buildingNumber = newObj.inBuildingNumberProcessSite; // رقم العقار
                        modal.siteAddress = newObj.sSiteAddressProcessSite; // شارع - حاره
                        modal.govermentCode = newObj.inGovermentCodeProcessSite; // كود المحافظه
                        modal.centerCode = newObj.inCenterCodeProcessSite; // كود المركز - القسم
                        modal.villageCode = newObj.inVillageCodeProcessSite; // كود الشياخه - القريه
                        modal.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                        modal.dateUpdate = DateTime.Now; // تاريخ التعديل
                        modal.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                        if (db.SaveChanges() > 0)
                            return true;

                        return false;
                    }
                    // مابقاش فيه عنوان (اتشال)
                    else if (String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessSite)) && String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessSite))
                        && String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessSite)) && String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessSite))
                        && String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessSite)))
                    {
                        db.processSites.Remove(modal);
                        if (db.SaveChanges() > 0)
                            return true;

                        return false;
                    }
                }
                else // First Time Insert
                {
                    // Insert
                    if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessSite))
                           || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessSite))
                           || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessSite)))
                        return bSave(newObj, Id);

                    // Becuase No Error
                    // مفيش داتا تتخزن .. فبالتالى مش هرجع false 
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }



       /// <summary>
       /// 
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Convert From Entity Framework To Model. 
        /// </summary>
        /// <param name="lEf"> Entity Framework Of Process site. </param>
        /// <returns> Model Of Process Site. </returns>
        internal override ProcessSiteModel ConvertEFToObjectBasic(processSite lEf)
        {
            ProcessSiteModel oProcessSiteModel = new ProcessSiteModel();
            if (lEf != null)
            {
                oProcessSiteModel.iProcessSiteCode = lEf.processSiteCode; // كود عنوان العملية
                oProcessSiteModel.iProcessCode = lEf.processCode; // كود العملية
                oProcessSiteModel.inBuildingNumberProcessSite = lEf.buildingNumber; // رقم عقار العملية
                oProcessSiteModel.sSiteAddressProcessSite = lEf.siteAddress; // عنوان العملية
                oProcessSiteModel.inGovermentCodeProcessSite = lEf.govermentCode; // محافظه العملية
                oProcessSiteModel.inCenterCodeProcessSite = lEf.centerCode; // مركز - قسم العملية
                oProcessSiteModel.inVillageCodeProcessSite = lEf.villageCode; // قريه - شياخه العملية
            }
            return oProcessSiteModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessSiteModel ConvertEFToObject(processSite ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override processSite ConvertObjectToEF(ProcessSiteModel bl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessSiteModel> ConvertEFsToObjects(List<processSite> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessSiteModel> ConvertEFsToObjectsBasic(List<processSite> lEf)
        {
            throw new NotImplementedException();
        }

    }
}
