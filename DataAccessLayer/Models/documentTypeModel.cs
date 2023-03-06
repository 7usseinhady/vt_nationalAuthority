using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Document Types. 
    /// </summary>
    public class DocumentTypeModel : ModelBase<DocumentTypeModel, documentType>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        public int iDocumentTypeCode { get; set; }
        public string sDocumentTypeName { get; set; }


        /// <summary>
        ///   Get Object Of Document Type.
        /// </summary>
        /// <param name="Id"> Document Type Code. </param>
        /// <returns> Document Type Model. </returns>
        internal override DocumentTypeModel GetById(int Id)
        {
            DocumentTypeModel ODocumentTypeModel = new DocumentTypeModel();
            documentType ODocumentType = db.documentTypes.FirstOrDefault(x => x.documentTypeCode == Id);
            if (ODocumentType != null)
            {
                ODocumentTypeModel = this.ConvertEFToObjectBasic(ODocumentType);
            }
            return ODocumentTypeModel;
        }

        /// <summary>
        ///   Get All Document Types.
        /// </summary>
        /// <returns> List Of Document Types Model. </returns>
        internal override List<DocumentTypeModel> GetAll()
        {
            List<DocumentTypeModel> LDocumentTypeModel = new List<DocumentTypeModel>();
            List<documentType> LDocumentTypeEF = db.documentTypes.ToList();

            if (LDocumentTypeEF != null)
            {
                LDocumentTypeModel = this.ConvertEFsToObjectsBasic(LDocumentTypeEF);
            }
            return LDocumentTypeModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<DocumentTypeModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Document Types Model. </returns>
        internal override List<DocumentTypeModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.spGetDocumentTypesWithSpecialCodes(searchObjs[0]).ToList();

                List<DocumentTypeModel> LDocumentTypeModel = new List<DocumentTypeModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        DocumentTypeModel oDocumentTypeModelEF = new DocumentTypeModel();
                        oDocumentTypeModelEF.iDocumentTypeCode = item.documentTypeCode;
                        oDocumentTypeModelEF.sDocumentTypeName = item.documentTypeName;

                        LDocumentTypeModel.Add(oDocumentTypeModelEF);
                    }
                }

                return LDocumentTypeModel;
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
        internal override bool bSave(DocumentTypeModel newObj)
        {
            try
            {
                documentType modal = new documentType();
                modal.documentTypeName = newObj.sDocumentTypeName;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;

                db.documentTypes.Add(modal);
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
        internal override bool bSave(DocumentTypeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Document Type Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(DocumentTypeModel newObj, int Id)
        {
            try
            {
                documentType model = db.documentTypes.FirstOrDefault(x => x.documentTypeCode == Id);
                if (model != null)
                {
                    model.documentTypeName = newObj.sDocumentTypeName;
                    model.userUpdateCode = newObj.inUserUpdateCode;
                    model.dateUpdate = DateTime.Now;
                    model.ipUpdate = newObj.sIpUpdate;

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
        ///   Delete Special Document Type.
        /// </summary>
        /// <param name="Id"> Code Of Document Type Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.documentTypes.Remove(db.documentTypes.FirstOrDefault(x => x.documentTypeCode == Id));
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
        ///   Convert From Entity Framework To Model. 
        /// </summary>
        /// <param name="lEf"> Entity Framework Of Document Type. </param>
        /// <returns> Model Of Document Type. </returns>
        internal override DocumentTypeModel ConvertEFToObjectBasic(documentType lEf)
        {
            DocumentTypeModel ODocumentTypeModel = new DocumentTypeModel();
            if (lEf != null)
            {
                ODocumentTypeModel.iDocumentTypeCode = lEf.documentTypeCode;
                ODocumentTypeModel.sDocumentTypeName = lEf.documentTypeName;
                ODocumentTypeModel.inUserInsertCode = lEf.userInsertCode;
                ODocumentTypeModel.dtDateInsert = lEf.dateInsert;
                ODocumentTypeModel.sIpInsert = lEf.ipInsert;
                ODocumentTypeModel.inUserUpdateCode = lEf.userUpdateCode;
                ODocumentTypeModel.dtDateUpdate = lEf.dateUpdate;
                ODocumentTypeModel.sIpUpdate = lEf.ipUpdate;
            }
            return ODocumentTypeModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Document Types. </param>
        /// <returns> List Of Document Types Model. </returns>
        internal override List<DocumentTypeModel> ConvertEFsToObjectsBasic(List<documentType> lEf)
        {
            List<DocumentTypeModel> LDocumentTypeModel = new List<DocumentTypeModel>();
            if (lEf != null)
            {
                foreach (documentType oEF in lEf)
                {
                    LDocumentTypeModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LDocumentTypeModel;
        }

        /// <summary>
        ///   Convert From Entity Framework To Model. 
        /// </summary>
        /// <param name="ef"> Entity Framework Of Document Type. </param>
        /// <returns> Model Of Document Type. </returns>
        internal override DocumentTypeModel ConvertEFToObject(documentType ef)
        {
            DocumentTypeModel ODocumentTypeModel = new DocumentTypeModel();
            if (ef != null)
            {
                ODocumentTypeModel.iDocumentTypeCode = ef.documentTypeCode;
                ODocumentTypeModel.sDocumentTypeName = ef.documentTypeName;
                ODocumentTypeModel.inUserInsertCode = ef.userInsertCode;
                ODocumentTypeModel.dtDateInsert = ef.dateInsert;
                ODocumentTypeModel.sIpInsert = ef.ipInsert;
                ODocumentTypeModel.inUserUpdateCode = ef.userUpdateCode;
                ODocumentTypeModel.dtDateUpdate = ef.dateUpdate;
                ODocumentTypeModel.sIpUpdate = ef.ipUpdate;
            }

            return ODocumentTypeModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Document Types. </param>
        /// <returns> List Of Document Types Model. </returns>
        internal override List<DocumentTypeModel> ConvertEFsToObjects(List<documentType> lEf)
        {
            List<DocumentTypeModel> LDocumentTypeModel = new List<DocumentTypeModel>();
            if (lEf != null)
            {
                foreach (documentType oEF in lEf)
                {
                    LDocumentTypeModel.Add(this.ConvertEFToObject(oEF));
                }
            }
            return LDocumentTypeModel;
        }

        /// <summary>
        ///   Convert From Model To Entity Framework. 
        /// </summary>
        /// <param name="bl"> Model Of Document Type. </param>
        /// <returns> Entity Framework Of Document Type. </returns>
        internal override documentType ConvertObjectToEF(DocumentTypeModel bl)
        {
            documentType ODocumentTypeModel = new documentType();
            if (bl != null)
            {
                ODocumentTypeModel.documentTypeCode = bl.iDocumentTypeCode;
                ODocumentTypeModel.documentTypeName = bl.sDocumentTypeName;
                ODocumentTypeModel.userInsertCode = bl.inUserInsertCode;
                ODocumentTypeModel.dateInsert = bl.dtDateInsert;
                ODocumentTypeModel.ipInsert = bl.sIpInsert;
                ODocumentTypeModel.userUpdateCode = bl.inUserUpdateCode;
                ODocumentTypeModel.dateUpdate = bl.dtDateUpdate;
                ODocumentTypeModel.ipUpdate = bl.sIpUpdate;
            }
            return ODocumentTypeModel;
        }

    }
}