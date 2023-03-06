using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class ProcessUserLettersSiteModel : ModelBase<ProcessUserLettersSiteModel, processUserLettersSite>
    {
        #region ProcessUserLettersSite Fields عنوان المخطار للمراسله 
        public int iProcessUserLettersSiteCode { get; set; } // كود عنوان المخطار للمراسله
        public int iProcessCode { get; set; } // كود العمليه
        public Nullable<int> inBuildingNumberProcessUserLettersSite { get; set; } // رقم العقار
        public string sSiteAddressProcessUserLettersSite { get; set; } // شارع - حاره
        public Nullable<int> inGovermentCodeProcessUserLettersSite { get; set; } // كود المحافظه
        public Nullable<int> inCenterCodeProcessUserLettersSite { get; set; } // كود المركز - القسم
        public Nullable<int> inVillageCodeProcessUserLettersSite { get; set; } // كود الشياخه - القريه
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        ///   Get Object Of Process Letter Site.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Process Site Model. </returns>
        internal override ProcessUserLettersSiteModel GetById(int Id)
        {
            processUserLettersSite OProcessUserLettersSite = db.processUserLettersSites.FirstOrDefault(x => x.processCode == Id);
            ProcessUserLettersSiteModel oProcessUserLettersSiteModel = new ProcessUserLettersSiteModel();

            if (OProcessUserLettersSite != null)
                oProcessUserLettersSiteModel = this.ConvertEFToObjectBasic(OProcessUserLettersSite);

            return oProcessUserLettersSiteModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ProcessUserLettersSiteModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<ProcessUserLettersSiteModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Process Letter Site Model. </returns>
        internal override List<ProcessUserLettersSiteModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///    Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(ProcessUserLettersSiteModel newObj, int Id)
        {
            try
            {
                if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessUserLettersSite)))
                {
                    processUserLettersSite modal = new processUserLettersSite();
                    modal.processCode = Id; // كود العملية
                    modal.buildingNumber = newObj.inBuildingNumberProcessUserLettersSite; // رقم العقار
                    modal.siteAddress = newObj.sSiteAddressProcessUserLettersSite; // شارع - حاره
                    modal.govermentCode = newObj.inGovermentCodeProcessUserLettersSite; // كود المحافظه
                    modal.centerCode = newObj.inCenterCodeProcessUserLettersSite; // كود المركز - القسم
                    modal.villageCode = newObj.inVillageCodeProcessUserLettersSite; // كود الشياخه - القريه
                    modal.userInsertCode = newObj.inUserInsertCode; // كود موظف الادخال
                    modal.dateInsert = DateTime.Now; // تاريخ الادخال
                    modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                    db.processUserLettersSites.Add(modal);
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
        internal override bool bSave(ProcessUserLettersSiteModel newObj)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Process Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ProcessUserLettersSiteModel newObj, int Id)
        {
            try
            {
                processUserLettersSite modal = db.processUserLettersSites.FirstOrDefault(x => x.processCode == Id);
                if (modal != null)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessUserLettersSite)))
                    {
                        modal.buildingNumber = newObj.inBuildingNumberProcessUserLettersSite; // رقم العقار
                        modal.siteAddress = newObj.sSiteAddressProcessUserLettersSite; // شارع - حاره
                        modal.govermentCode = newObj.inGovermentCodeProcessUserLettersSite; // كود المحافظه
                        modal.centerCode = newObj.inCenterCodeProcessUserLettersSite; // كود المركز - القسم
                        modal.villageCode = newObj.inVillageCodeProcessUserLettersSite; // كود الشياخه - القريه
                        modal.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                        modal.dateUpdate = DateTime.Now; // تاريخ التعديل
                        modal.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                        if (db.SaveChanges() > 0)
                            return true;

                        return false;
                    }
                    // مابقاش فيه عنوان (اتشال)
                    else if (String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessUserLettersSite)) && String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessUserLettersSite))
                        && String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessUserLettersSite)) && String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessUserLettersSite))
                        && String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessUserLettersSite)))
                    {
                        db.processUserLettersSites.Remove(modal);
                        if (db.SaveChanges() > 0)
                            return true;

                        return false;
                    }
                }
                else  // First Time Insert
                {
                    // Insert
                    if (!String.IsNullOrEmpty(Convert.ToString(newObj.inBuildingNumberProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.sSiteAddressProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inGovermentCodeProcessUserLettersSite)) || !String.IsNullOrEmpty(Convert.ToString(newObj.inCenterCodeProcessUserLettersSite))
                        || !String.IsNullOrEmpty(Convert.ToString(newObj.inVillageCodeProcessUserLettersSite)))
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
        /// <param name="lEf"> Entity Framework Of Process Letter site. </param>
        /// <returns> Model Of Process Letter Site. </returns>
        internal override ProcessUserLettersSiteModel ConvertEFToObjectBasic(processUserLettersSite lEf)
        {
            ProcessUserLettersSiteModel oProcessUserLettersSiteModel = new ProcessUserLettersSiteModel();

            if (lEf != null)
            {
                oProcessUserLettersSiteModel.iProcessUserLettersSiteCode = lEf.processUserLettersSiteCode; // كود عنوان المخطار للمراسله
                oProcessUserLettersSiteModel.iProcessCode = lEf.processCode; // كود العملية
                oProcessUserLettersSiteModel.inBuildingNumberProcessUserLettersSite = lEf.buildingNumber; // رقم عقار المخطار للمراسله
                oProcessUserLettersSiteModel.sSiteAddressProcessUserLettersSite = lEf.siteAddress; // عنوان المخطار للمراسله
                oProcessUserLettersSiteModel.inGovermentCodeProcessUserLettersSite = lEf.govermentCode; // محافظه المخطار للمراسله
                oProcessUserLettersSiteModel.inCenterCodeProcessUserLettersSite = lEf.centerCode; // مركز - قسم المخطار للمراسله
                oProcessUserLettersSiteModel.inVillageCodeProcessUserLettersSite = lEf.villageCode; // قريه - شياخه المخطار للمراسله
            }
            return oProcessUserLettersSiteModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessUserLettersSiteModel ConvertEFToObject(processUserLettersSite ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override processUserLettersSite ConvertObjectToEF(ProcessUserLettersSiteModel bl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessUserLettersSiteModel> ConvertEFsToObjects(List<processUserLettersSite> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessUserLettersSiteModel> ConvertEFsToObjectsBasic(List<processUserLettersSite> lEf)
        {
            throw new NotImplementedException();
        }

    }
}
