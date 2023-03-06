using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Attachment Type. 
    /// </summary>
    public class AttachmentTypeModel : ModelBase<AttachmentTypeModel, attachmentType>
    {
        #region Attachment Type Fields نوع المرفق
        public int iAttachmentTypeCode { get; set; } // كود نوع المرفق
        [Required(ErrorMessage = "برجاء ادخال نوع المرفق ")]
        public string sAttachmentTypeName { get; set; } // اسم نوع المرفق
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Get Object Of Attachment Type.
        /// </summary>
        /// <param name="Id"> Attachment Type Code. </param>
        /// <returns> Model Of Attachment Type. </returns>
        internal override AttachmentTypeModel GetById(int Id)
        {
            AttachmentTypeModel OAttachmentTypeModel = new AttachmentTypeModel();
            attachmentType OAttachmentType = db.attachmentTypes.FirstOrDefault(x => x.attachmentTypeCode == Id);

            if (OAttachmentType != null)
                OAttachmentTypeModel = this.ConvertEFToObjectBasic(OAttachmentType);

            return OAttachmentTypeModel;
        }

        /// <summary>
        ///   Get All Attachment Types.
        /// </summary>
        /// <returns> List Of Attachment Types Model. </returns>
        internal override List<AttachmentTypeModel> GetAll()
        {
            List<AttachmentTypeModel> LAttachmentTypeModel = new List<AttachmentTypeModel>();
            List<attachmentType> LAttachmentTypeEF = db.attachmentTypes.ToList();

            if (LAttachmentTypeEF != null)
                LAttachmentTypeModel = this.ConvertEFsToObjectsBasic(LAttachmentTypeEF);

            return LAttachmentTypeModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<AttachmentTypeModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Attachment Types Model. </returns>
        internal override List<AttachmentTypeModel> lSearch(List<string> searchObjs)
        {
            try
            {
                string sAttachmentName = searchObjs[0];
                var models = db.attachmentTypes.Where(x => x.attachmentTypeName.Contains(sAttachmentName)).ToList();

                List<AttachmentTypeModel> LAttachmentTypeModel = new List<AttachmentTypeModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        AttachmentTypeModel oattachmentTypeModelEF = new AttachmentTypeModel();
                        oattachmentTypeModelEF.iAttachmentTypeCode = item.attachmentTypeCode; // كود نوع المرفق
                        oattachmentTypeModelEF.sAttachmentTypeName = item.attachmentTypeName; // اسم نوع المرفق

                        LAttachmentTypeModel.Add(oattachmentTypeModelEF);
                    }
                }

                return LAttachmentTypeModel;
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
        internal override bool bSave(AttachmentTypeModel newObj)
        {
            try
            {
                attachmentType modal = new attachmentType();
                modal.attachmentTypeName = newObj.sAttachmentTypeName; // اسم نوع المرفق
                modal.userInsertCode = newObj.inUserInsertCode; // كود موظف الادخال
                modal.dateInsert = dtServerTime; // تاريخ الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                db.attachmentTypes.Add(modal);
                if (db.SaveChanges() > 0)
                    return true;
                else
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
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(AttachmentTypeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Request. </param>
        /// <param name="Id"> Code Of Attachment Type Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(AttachmentTypeModel newObj, int Id)
        {
            try
            {
                attachmentType model = db.attachmentTypes.FirstOrDefault(x => x.attachmentTypeCode == Id);
                if (model != null)
                {
                    model.attachmentTypeName = newObj.sAttachmentTypeName; // اسم نوع المرفق
                    model.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                    model.dateUpdate = dtServerTime; // تاريخ التعديل
                    model.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                    if (db.SaveChanges() > 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///   Delete Special Attachment Type.
        /// </summary>
        /// <param name="Id"> Code Of Attachment Type Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.attachmentTypes.Remove(db.attachmentTypes.FirstOrDefault(x => x.attachmentTypeCode == Id));
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
        ///   Convert From Entity Framework To Model. 
        /// </summary>
        /// <param name="lEf"> Entity Framework Of Attachment Type. </param>
        /// <returns> Model Of Attachment Type. </returns>
        internal override AttachmentTypeModel ConvertEFToObjectBasic(attachmentType lEf)
        {
            AttachmentTypeModel OAttachmentTypeModel = new AttachmentTypeModel();
            if (lEf != null)
            {
                OAttachmentTypeModel.iAttachmentTypeCode = lEf.attachmentTypeCode; // كود نوع المرفق
                OAttachmentTypeModel.sAttachmentTypeName = lEf.attachmentTypeName; // اسم نوع المرفق
            }
            return OAttachmentTypeModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Attachment Type. </param>
        /// <returns> List Of Attachment Types Model. </returns>
        internal override List<AttachmentTypeModel> ConvertEFsToObjectsBasic(List<attachmentType> lEf)
        {
            List<AttachmentTypeModel> LAttachmentTypeModel = new List<AttachmentTypeModel>();
            if (lEf != null)
                foreach (attachmentType oEF in lEf)
                    LAttachmentTypeModel.Add(this.ConvertEFToObjectBasic(oEF));

            return LAttachmentTypeModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override AttachmentTypeModel ConvertEFToObject(attachmentType ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<AttachmentTypeModel> ConvertEFsToObjects(List<attachmentType> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override attachmentType ConvertObjectToEF(AttachmentTypeModel bl)
        {
            throw new NotImplementedException();
        }
    }
}