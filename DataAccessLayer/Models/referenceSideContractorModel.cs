using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Reference Sides - Contractors. 
    /// </summary>
    public class ReferenceSideContractorModel : ModelBase<ReferenceSideContractorModel, referenceSideContractor>
    {
        #region Referance Side - Contractors Fields جهات الاسناد - المقاولين
        public int iReferenceSideContractorCode { get; set; }
        public string sReferenceSideContractorName { get; set; }
        [StringLength(9, ErrorMessage = "الرجاء ادخال الرقم التأمينى مكون من 9 ارقام", MinimumLength = 1)]
        public string sReferanceSideContractorNum { get; set; }
        public Nullable<int> ilegalEntityCode { get; set; }
        public int legalEntityCode { get; set; }
        public string sLegalEntityName { get; set; }
        public bool bnIsActive { get; set; }
        public string sIsActive { get; set; }
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        private readonly List<DataDrob> dataDrob = new List<DataDrob>();

        /// <summary>
        ///   Get Object Of 'Reference Side - Contractor'.
        /// </summary>
        /// <param name="Id"> 'Reference Side - Contractor' Code. </param>
        /// <returns> Model Of 'Reference Side - Contractor'. </returns>
        internal override ReferenceSideContractorModel GetById(int Id)
        {
            ReferenceSideContractorModel LcontractorModel = new ReferenceSideContractorModel();
            spGetRefSideContractorsLegal_Result LcontractorEF = db.spGetRefSideContractorsLegal(Id, "-1", "-1", -1, -1).FirstOrDefault();

            if (LcontractorEF != null)
            {
                LcontractorModel = this.ConvertEFToObjectBasic(LcontractorEF);
            }
            return LcontractorModel;
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors From Table.
        /// </summary>
        /// <returns> List Of 'Reference Sides - Contractors' Model. </returns>
        internal override List<ReferenceSideContractorModel> GetAll()
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            List<referenceSideContractor> LcontractorEF = db.referenceSideContractors.ToList();

            if (LcontractorEF != null)
            {
                LcontractorModel = this.ConvertEFsToObjects(LcontractorEF);
            }
            return LcontractorModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<ReferenceSideContractorModel> GetAll(int Id)
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            List<spGetRefSideContractorsLegal_Result> LcontractorEF = db.spGetRefSideContractorsLegal(Id, "-1", "-1", -1, -1).ToList();

            if (LcontractorEF != null)
            {
                LcontractorModel = this.ConvertEFsToObjects(LcontractorEF);
            }
            return LcontractorModel;
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors From Stored Procedure.
        /// </summary>
        /// <returns> List Of 'Reference Sides - Contractors' Model. </returns>
        internal List<ReferenceSideContractorModel> GetAllCont()
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            List<spGetRefSideContractors_Result> LcontractorEF = db.spGetRefSideContractors().Where(x => x.referenceSideContractorCode != -1).ToList();

            if (LcontractorEF != null)
            {
                LcontractorModel = this.ConvertEFsToObjects(LcontractorEF);
            }
            return LcontractorModel;
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors With Spacial Name.
        /// </summary>
        /// <param name="name"> 'Reference Side - Contractor' Name. </param>
        /// <returns></returns>
        public List<ReferenceSideContractorModel> GetContractors(string name)
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            List<referenceSideContractor> LcontractorEF = db.referenceSideContractors.Where(x => x.referenceSideContractorName.Contains(name)).OrderBy(y => y.referenceSideContractorName).ToList();

            if (LcontractorEF != null)
            {
                LcontractorModel = this.ConvertEFsToObjects(LcontractorEF);
            }
            return LcontractorModel;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Attachment Types Model. </returns>
        internal override List<ReferenceSideContractorModel> lSearch(List<string> searchObjs)
        {
            try
            {
                int? iLegalCode = Convert.ToInt32(searchObjs[2]);
                int? bActive = Convert.ToInt32(searchObjs[3]);
                var models = db.spGetRefSideContractorsLegal(-1, searchObjs[0], searchObjs[1], iLegalCode, bActive).ToList();

                List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();

                if (models.Count > 0)
                {
                    LcontractorModel = this.ConvertEFsToObjects(models);
                }

                return LcontractorModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(ReferenceSideContractorModel newObj)
        {
            try
            {
                this.dyError = db.spInsertReferenceSideContractor(newObj.sReferenceSideContractorName, newObj.sReferanceSideContractorNum, Convert.ToInt32(newObj.legalEntityCode), newObj.bnIsActive, Convert.ToInt32(newObj.inUserInsertCode), newObj.dtDateInsert, newObj.sIpInsert);
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
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(ReferenceSideContractorModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Request. </param>
        /// <param name="Id"> Code Of 'Reference Side - Contractor' Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ReferenceSideContractorModel newObj, int Id)
        {
            referenceSideContractor oModel = db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorCode != newObj.iReferenceSideContractorCode && x.referenceSideContractorInsuranceNum == newObj.sReferanceSideContractorNum && x.legalEntityCode == newObj.legalEntityCode);

            if (oModel != null)
            {
                this.dyError = "<script>sAlert('عفوا , هذا العنصر موجود سابقاً',1);</script>";
                return false;
            }

            referenceSideContractor modal = db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorCode == newObj.iReferenceSideContractorCode);
            if (modal == null)
                return false;

            modal.referenceSideContractorName = newObj.sReferenceSideContractorName;
            modal.referenceSideContractorInsuranceNum = newObj.sReferanceSideContractorNum;
            modal.legalEntityCode = newObj.legalEntityCode;
            modal.isActive = newObj.bnIsActive;
            modal.userUpdateCode = newObj.inUserUpdateCode;
            modal.dateUpdate = DateTime.Now;
            modal.ipUpdate = newObj.sIpUpdate;
            if (db.SaveChanges() > 0)
            {
                this.dyError = "<script>sAlert('تم التعديل بنجاح',0);</script>";

                return true;
            }
            this.dyError = "<script>sAlert('عفوا , هذا العنصر موجود سابقاً',1);</script>";

            return false;
        }


        /// <summary>
        ///   Delete Special 'Reference Side - Contractor'.
        /// </summary>
        /// <param name="Id"> Code Of 'Reference Side - Contractor' Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.referenceSideContractors.Remove(db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorCode == Id));
                if (db.SaveChanges() > 0)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Table'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of 'Reference Side - Contractor'. </param>
        /// <returns> List Of 'Reference Side - Contractor' Model. </returns>
        internal override List<ReferenceSideContractorModel> ConvertEFsToObjects(List<referenceSideContractor> lEf)
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            if (lEf != null)
            {
                foreach (referenceSideContractor oEF in lEf)
                {
                    LcontractorModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LcontractorModel;
        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="lEF"> List Of Entity Framework Of 'Reference Side - Contractor' 'Stored Procedure'. </param>
        /// <returns> List Of 'Reference Side - Contractor' Model. </returns>
        private List<ReferenceSideContractorModel> ConvertEFsToObjects(List<spGetRefSideContractors_Result> lEF)
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            if (lEF != null)
            {
                foreach (spGetRefSideContractors_Result oEF in lEF)
                {
                    LcontractorModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LcontractorModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEF"> List Of Entity Framework Of 'Reference Side - Contractor' 'Stored Procedure'. </param>
        /// <returns> List Of 'Reference Side - Contractor' Model. </returns>
        private List<ReferenceSideContractorModel> ConvertEFsToObjects(List<spGetRefSideContractorsLegal_Result> lEF)
        {
            List<ReferenceSideContractorModel> LcontractorModel = new List<ReferenceSideContractorModel>();
            if (lEF != null)
            {
                foreach (spGetRefSideContractorsLegal_Result oEF in lEF)
                {
                    LcontractorModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LcontractorModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ReferenceSideContractorModel> ConvertEFsToObjectsBasic(List<referenceSideContractor> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Convert From Entity Framework To Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="EF"> Entity Framework Of 'Reference Side - Contractor'. </param>
        /// <returns> Model Of 'Reference Side - Contractor'. </returns>
        private ReferenceSideContractorModel ConvertEFToObjectBasic(spGetRefSideContractors_Result EF)
        {
            ReferenceSideContractorModel OcontractorModel = new ReferenceSideContractorModel();
            if (EF != null)
            {
                OcontractorModel.iReferenceSideContractorCode = EF.referenceSideContractorCode;
                OcontractorModel.sReferenceSideContractorName = EF.referenceSideContractorName;
            }
            return OcontractorModel;
        }

        /// <summary>
        ///   Convert From Entity Framework To Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="EF"> Entity Framework Of 'Reference Side - Contractor'. </param>
        /// <returns> Model Of 'Reference Side - Contractor'. </returns>
        private ReferenceSideContractorModel ConvertEFToObjectBasic(spGetRefSideContractorsLegal_Result EF)
        {
            ReferenceSideContractorModel OcontractorModel = new ReferenceSideContractorModel();
            if (EF != null)
            {
                OcontractorModel.iReferenceSideContractorCode = EF.referenceSideContractorCode;
                OcontractorModel.sReferanceSideContractorNum = EF.referenceSideContractorInsuranceNum;
                OcontractorModel.sReferenceSideContractorName = EF.referenceSideContractorName;
                OcontractorModel.legalEntityCode = EF.legalEntityCode;
                OcontractorModel.ilegalEntityCode = EF.legalEntityCode;
                OcontractorModel.sLegalEntityName = EF.legalEntityName;

                if (EF.isActive == true)
                    OcontractorModel.bnIsActive = true;
                else
                    OcontractorModel.bnIsActive = false;

                OcontractorModel.sIsActive = EF.activeName;
                OcontractorModel.inUserInsertCode = EF.userInsertCode;
            }
            return OcontractorModel;
        }

        /// <summary>
        ///   Convert From Entity Framework To Model 'Table'. 
        /// </summary>
        /// <param name="lEf"> Entity Framework Of 'Reference Side - Contractor'. </param>
        /// <returns> Model Of 'Reference Side - Contractor'. </returns>
        internal override ReferenceSideContractorModel ConvertEFToObjectBasic(referenceSideContractor lEf)
        {
            ReferenceSideContractorModel OcontractorModel = new ReferenceSideContractorModel();
            if (lEf != null)
            {
                OcontractorModel.iReferenceSideContractorCode = lEf.referenceSideContractorCode;
                OcontractorModel.sReferenceSideContractorName = lEf.referenceSideContractorName;
                OcontractorModel.sReferanceSideContractorNum = lEf.referenceSideContractorInsuranceNum;
                OcontractorModel.ilegalEntityCode = lEf.legalEntityCode;
                OcontractorModel.inUserInsertCode = lEf.userInsertCode;
            }
            return OcontractorModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ReferenceSideContractorModel ConvertEFToObject(referenceSideContractor ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override referenceSideContractor ConvertObjectToEF(ReferenceSideContractorModel bl)
        {
            throw new NotImplementedException();
        }



        // Ajex Call

        /// <summary>
        ///    Get All Reference Sides - Contractors With Special Insurance Number And Active.
        /// </summary>
        /// <param name="insNum"> 'Reference Side - Contractor' Insurance Number. </param>
        public void vRefSideContByInsurNum(string insNum)
        {
            if (insNum != "null")
            {
                var modal = db.referenceSideContractors.Where(x => x.referenceSideContractorInsuranceNum == insNum && x.isActive != false).ToList();

                if (modal.Count > 0)
                    dataDrob.Add(new DataDrob() { Value = modal[0].referenceSideContractorCode, Text = modal[0].referenceSideContractorName });
            }
            else
            {
                var modal = db.referenceSideContractors.ToList();
                if (modal.Count > 0)
                    for (int i = 0; i < modal.Count; i++)
                        dataDrob.Add(new DataDrob() { Value = modal[i].referenceSideContractorCode, Text = modal[i].referenceSideContractorName });
            }
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors By Special Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> 'Reference Side - Contractor' Insurance Number. </param>
        public void GetRefSideContByInsurNumLegalEntity(int legalEntity, string insNum)
        {
            spGetRefSideContractorsLegal_Result modal = db.spGetRefSideContractorsLegal(-1, "-1", insNum, legalEntity, -1).FirstOrDefault();

            if (modal != null)
            {
                this.iReferenceSideContractorCode = modal.referenceSideContractorCode; // اسم جهه الاسناد - المقاول الرئيسي
                this.sReferenceSideContractorName = modal.referenceSideContractorName; // اسم جهه الاسناد - المقاول الرئيسي
                this.sReferanceSideContractorNum = modal.referenceSideContractorInsuranceNum; // الرقم التأمينى لجهه الاسناد - المقاول الرئيسي
                this.sIsActive = modal.activeName; // فعال - غير فعال
                this.inUserInsertCode = modal.userInsertCode; // كود مدخل جهه الاسناد
            }
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors By Special Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> 'Reference Side - Contractor' Insurance Number. </param>
        /// <returns></returns>
        public List<ReferenceSideContractorModel> lGetRefSideContByInsurNumLegalEntity(int legalEntity, string insNum)
        {
            List<ReferenceSideContractorModel> lModel = new List<ReferenceSideContractorModel>();

            List<spGetRefSideContractorsLegal_Result> modal = db.spGetRefSideContractorsLegal(-1, "-1", insNum, legalEntity, -1).ToList();

            if (modal != null)
            {
                for (int i = 0; i < modal.Count; i++)
                {
                    ReferenceSideContractorModel m = new ReferenceSideContractorModel();

                    m.iReferenceSideContractorCode = modal[i].referenceSideContractorCode; // كود جهه الاسناد - المقاول الرئيسي
                    m.sReferenceSideContractorName = modal[i].referenceSideContractorName; // اسم جهه الاسناد - المقاول الرئيسي
                    m.sReferanceSideContractorNum = modal[i].referenceSideContractorInsuranceNum; // الرقم التأمينى لجهه الاسناد - المقاول الرئيسي
                    m.sLegalEntityName = modal[i].legalEntityName; // الكيان القانونى لجهه الاسناد - المقاول 
                    m.sIsActive = modal[i].activeName; // فعال - غير فعال
                    m.inUserInsertCode = modal[i].userInsertCode; // كود مدخل جهه الاسناد
                    lModel.Add(m);
                }
            }

            return lModel;
        }

        /// <summary>
        ///   Get Object Of 'Reference Side - Contractor' By Code.
        /// </summary>
        /// <param name="refSideContCode"> 'Reference Side - Contractor' Code. </param>
        public void vInsurNumByRefSideCont(string refSideContCode)
        {
            int contRefSideCode = Convert.ToInt32(refSideContCode);
            var modal = db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorCode == contRefSideCode && x.isActive != false);

            if (modal != null)
            {
                this.sReferenceSideContractorName = modal.referenceSideContractorName; // اسم جهه الاسناد - المقاول الرئيسي
                this.sReferanceSideContractorNum = modal.referenceSideContractorInsuranceNum; // الرقم التأمينى لجهه الاسناد - المقاول الرئيسي
            }
        }

    }
}