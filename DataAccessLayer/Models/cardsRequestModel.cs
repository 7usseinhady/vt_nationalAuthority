using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class CardsRequestModel : ModelBase<CardsRequestModel, cardsRequest>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iCardsRequestCode { get; set; }
        public int iCardsStatusCode { get; set; }
        [Required(ErrorMessage = "برجاء ادخال اسم طلبية الكروت ")]
        public string sCardsRequestName { get; set; }
        public Nullable<bool> bnIsDelete { get; set; }
        public string sUserFullName { get; set; }
        public string sOfficeInsuranceName { get; set; }
        public string sCardStatusName { get; set; }
        public int iCardStatusCode { get; set; }
        public string sRequestDate { get; set; }
        public string sDeliveryDate { get; set; }
        public string sReciveDate { get; set; }

        public int iCardsCout { get; set; }
        // worker
        public string sWorkerName { get; set; }
        public string sWorkerNationalId { get; set; }
        public string sWorkerInsuranceNumber { get; set; }
        public string sCareerName { get; set; }
        public string sSkillDegree { get; set; }
        public int iCardsRequestWorkersCode { get; set; }

        #endregion
        /// <summary>
        /// Delete Card Request 
        /// </summary>
        /// <param name="Id">Card Request Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                int y = 0;
                IEnumerable<cardsRequestWorker> list = db.cardsRequestWorkers.Where(x => x.cardsRequestCode == Id).ToList();
                db.cardsRequestWorkers.RemoveRange(list);
                if (db.SaveChanges() > 0)
                {
                    db.cardsRequests.Remove(db.cardsRequests.FirstOrDefault(x => x.cardsRequestCode == Id));
                    y = db.SaveChanges();
                }
                if (y > 0)
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
        /// Ddelete Worker From Card Rrequest
        /// </summary>
        /// <param name="CardRequest">Worker Card Request Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal bool bDeleteWorkerRequest(int CardRequest)
        {
            try
            {
                int y = 0;
                db.cardsRequestWorkers.Remove(db.cardsRequestWorkers.FirstOrDefault(x => x.cardsRequestWorkersCode == CardRequest));
                y = db.SaveChanges();
                if (y > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        internal override bool bEdit(CardsRequestModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        internal override bool bSave(CardsRequestModel newObj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Card Request Status
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <returns>Edit Done Or Not</returns>
        public bool EditStatusRequest(CardsRequestModel newObj)
        {
            try
            {
                cardsRequest model = db.cardsRequests.FirstOrDefault(x => x.cardsRequestCode == newObj.iCardsRequestCode);
                if (model != null)
                {
                    model.cardsStatusCode = newObj.iCardsStatusCode;
                    model.userUpdateCode = newObj.inUserUpdateCode;
                    model.dateUpdate = DateTime.Now;
                    if (db.SaveChanges() > 0)
                    {
                        companyCardRequest company = new companyCardRequest();
                        company.cardsRequestCode = newObj.iCardsRequestCode;
                        company.cardsStatusCode = newObj.iCardsStatusCode;
                        company.dateInsert = DateTime.Now;
                        db.companyCardRequests.Add(company);
                        if (db.SaveChanges() > 0)
                            return true;
                        else
                            return false;
                    }
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
        /// Save Workers Card Request
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="WorkerId">Workers Codes</param>
        /// <returns>Save Done Or Not</returns>
        public bool bSave(CardsRequestModel newObj, List<string> WorkerId)
        {
            try
            {
                int id = newObj.iCardsRequestCode;
                cardsRequest oCardsRequest = new cardsRequest();

                if (newObj.iCardsRequestCode != 0) // edit
                {
                    cardsRequest modelEdit = db.cardsRequests.FirstOrDefault(x => x.cardsRequestCode == newObj.iCardsRequestCode);
                    if (modelEdit != null)
                    {
                        modelEdit.cardsRequestName = newObj.sCardsRequestName;
                        modelEdit.cardsStatusCode = 1;
                        modelEdit.isDelete = false;
                        modelEdit.userInsertCode = newObj.inUserUpdateCode;
                        modelEdit.dateInsert = dtServerTime;
                        modelEdit.ipInsert = newObj.sIpUpdate;
                        // كود الطلب الى لسه مبعوت  
                    }

                }
                else
                {
                    oCardsRequest.cardsRequestName = newObj.sCardsRequestName;
                    oCardsRequest.cardsStatusCode = 1;
                    oCardsRequest.isDelete = false;
                    oCardsRequest.userInsertCode = newObj.inUserInsertCode;
                    oCardsRequest.dateInsert = dtServerTime;
                    oCardsRequest.ipInsert = newObj.sIpInsert;
                    db.cardsRequests.Add(oCardsRequest);
                }

                if (db.SaveChanges() > 0)
                {
                    if (id == 0)
                        id = oCardsRequest.cardsRequestCode; // كود الطلب الى لسه مبعوت                                 
                    int y = 0;
                    for (int i = 0; i < WorkerId.Count; i++)
                    {
                        cardsRequestWorker oCardRequestWorker = new cardsRequestWorker();
                        oCardRequestWorker.cardsRequestCode = id;
                        oCardRequestWorker.workerCode = Convert.ToInt32(WorkerId[i]);
                        int worker = Convert.ToInt32(WorkerId[i]);

                        int code = (int)(from o in db.workerDetails
                                         orderby o.workerDetailsCode descending
                                         where o.workerCode == worker
                                         select o.careerCode).FirstOrDefault();
                        oCardRequestWorker.careerCode = code;
                        db.cardsRequestWorkers.Add(oCardRequestWorker);
                        y = db.SaveChanges();
                    }
                    if (y > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        internal override bool bSave(CardsRequestModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<CardsRequestModel> ConvertEFsToObjects(List<cardsRequest> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<CardsRequestModel> ConvertEFsToObjectsBasic(List<cardsRequest> lEf)
        {
            throw new NotImplementedException();
        }

        internal override CardsRequestModel ConvertEFToObject(cardsRequest ef)
        {
            throw new NotImplementedException();
        }

        internal override CardsRequestModel ConvertEFToObjectBasic(cardsRequest lEf)
        {
            throw new NotImplementedException();
        }

        internal override cardsRequest ConvertObjectToEF(CardsRequestModel bl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All  Cards Requests
        /// </summary>
        /// <returns>List Of Cards Requests</returns>
        internal override List<CardsRequestModel> GetAll()
        {
            try
            {
                var models = db.GetCardsRequest(null, null, null, null, null, null, null).ToList();
                List<CardsRequestModel> LcardsRequestModel = new List<CardsRequestModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sUserFullName = item.userFullName;
                        ocardsRequestModel.sOfficeInsuranceName = item.officeInsuranceName;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.requestDate;
                        ocardsRequestModel.sDeliveryDate = item.deliveryDate;
                        ocardsRequestModel.sCardStatusName = item.cardsStatusName;
                        ocardsRequestModel.sReciveDate = item.receiveDate;
                        ocardsRequestModel.iCardStatusCode = (int)item.cardsStatusCode;
                        LcardsRequestModel.Add(ocardsRequestModel);
                    }
                }

                return LcardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get All Cards Requests From Office Insurance Code
        /// </summary>
        /// <param name="str">Office Insurance Code</param>
        /// <returns>List Of Cards Requests</returns>
        internal List<CardsRequestModel> GetAll(string str)
        {
            try
            {
                var models = db.GetCardsRequest(str, null, null, null, null, null, null).ToList();
                List<CardsRequestModel> LcardsRequestModel = new List<CardsRequestModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sUserFullName = item.userFullName;
                        ocardsRequestModel.sOfficeInsuranceName = item.officeInsuranceName;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.requestDate;
                        ocardsRequestModel.sDeliveryDate = item.deliveryDate;
                        ocardsRequestModel.sCardStatusName = item.cardsStatusName;
                        ocardsRequestModel.sReciveDate = item.receiveDate;
                        ocardsRequestModel.iCardStatusCode = (int)item.cardsStatusCode;
                        LcardsRequestModel.Add(ocardsRequestModel);
                    }
                }

                return LcardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get All Workers In Card Request
        /// </summary>
        /// <param name="Id">Card Request Code</param>
        /// <returns>List Of Workers</returns>
        internal override List<CardsRequestModel> GetAll(int Id)
        {
            try
            {
                var models = db.GetWorkersCardRequest(Id).ToList();
                List<CardsRequestModel> LcardsRequestModel = new List<CardsRequestModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.sWorkerName = item.workerName;
                        ocardsRequestModel.sWorkerNationalId = item.workerNationalID;
                        ocardsRequestModel.sWorkerInsuranceNumber = item.workerInsuranceNum;
                        ocardsRequestModel.sCareerName = item.careerName;
                        ocardsRequestModel.sSkillDegree = item.skillDegreeName;
                        ocardsRequestModel.iCardsRequestWorkersCode = item.cardsRequestWorkersCode;
                        LcardsRequestModel.Add(ocardsRequestModel);
                    }
                }

                return LcardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get One Card Request
        /// </summary>
        /// <param name="Id">Card Request Code</param>
        /// <returns>Card Request</returns>
        internal override CardsRequestModel GetById(int Id)
        {
            try
            {
                var models = db.GetCardsRequest(null, null, null, null, null, null, null).Where(x => x.cardsRequestCode == Id).ToList();
                CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sUserFullName = item.userFullName;
                        ocardsRequestModel.sOfficeInsuranceName = item.officeInsuranceName;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.requestDate;
                        ocardsRequestModel.sDeliveryDate = item.deliveryDate;
                        ocardsRequestModel.sCardStatusName = item.cardsStatusName;
                        ocardsRequestModel.sReciveDate = item.receiveDate;
                        ocardsRequestModel.iCardStatusCode = (int)item.cardsStatusCode;
                    }
                }
                return ocardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
        /// <summary>
        /// Search For Cards Requests
        /// </summary>
        /// <param name="searchObjs">List Of Data Need For Search</param>
        /// <returns>List Of Cards Requests</returns>
        internal override List<CardsRequestModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetCardsRequest(searchObjs[0] == "" ? null : searchObjs[0], searchObjs[1] == "" ? null : searchObjs[1], searchObjs[2] == "" ? null : searchObjs[2],
                    searchObjs[3] == "" ? null : searchObjs[3], searchObjs[4] == "" ? null : searchObjs[4],
                    searchObjs[5] == "" ? null : searchObjs[5], searchObjs[6] == "" ? null : searchObjs[6]).ToList();
                List<CardsRequestModel> LcardsRequestModel = new List<CardsRequestModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sUserFullName = item.userFullName;
                        ocardsRequestModel.sOfficeInsuranceName = item.officeInsuranceName;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.requestDate;
                        ocardsRequestModel.sDeliveryDate = item.deliveryDate;
                        ocardsRequestModel.sCardStatusName = item.cardsStatusName;
                        ocardsRequestModel.sReciveDate = item.receiveDate;
                        ocardsRequestModel.iCardStatusCode = (int)item.cardsStatusCode;
                        LcardsRequestModel.Add(ocardsRequestModel);
                    }
                }

                return LcardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Search For Cards Reqests In Company
        /// </summary>
        /// <param name="searchObjs">Llist Of Data Need For Search</param>
        /// <returns>List Of Cards Requests</returns>
        public List<CardsRequestModel> lSearchCompany(List<string> searchObjs)
        {
            try
            {
                var models = db.CompanyCard(searchObjs[0] == "" ? null : searchObjs[0], searchObjs[1] == "" ? null : searchObjs[1], searchObjs[2] == "" ? null : searchObjs[2], searchObjs[3] == "" ? null : searchObjs[3]).ToList();
                List<CardsRequestModel> LcardsRequestModel = new List<CardsRequestModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardsRequestModel ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.DateRequest.ToString();
                        ocardsRequestModel.iCardsCout = (int)item.CardsCount;
                        ocardsRequestModel.sCardStatusName = item.StatusName;
                        ocardsRequestModel.sDeliveryDate = item.DateDelivery.ToString();
                        ocardsRequestModel.sReciveDate = item.DateReciving.ToString();
                        LcardsRequestModel.Add(ocardsRequestModel);
                    }
                }

                return LcardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get One Card Rrequest
        /// </summary>
        /// <param name="RequestId">Card Request Code</param>
        /// <returns>Card Request Data</returns>
        public CardsRequestModel lSearchCompanyByID(int RequestId)
        {
            try
            {
                CardsRequestModel ocardsRequestModel = new CardsRequestModel();

                var model = db.cardsRequests.FirstOrDefault(x => x.cardsRequestCode == RequestId);

                string RequestName = model != null ? model.cardsRequestName : null;
                var models = db.CompanyCard(null, null, null, RequestName).ToList();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        ocardsRequestModel = new CardsRequestModel();
                        ocardsRequestModel.iCardsRequestCode = item.cardsRequestCode;
                        ocardsRequestModel.sCardsRequestName = item.cardsRequestName;
                        ocardsRequestModel.sRequestDate = item.DateRequest.ToString();
                        ocardsRequestModel.iCardsCout = (int)item.CardsCount;
                        ocardsRequestModel.sCardStatusName = item.StatusName;
                        ocardsRequestModel.sDeliveryDate = item.DateDelivery.ToString();
                        ocardsRequestModel.sReciveDate = item.DateReciving.ToString();
                    }
                }

                return ocardsRequestModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
