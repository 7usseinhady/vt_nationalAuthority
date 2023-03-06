using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Process Users. 
    /// </summary>
    public class ProcessUsersModel : ModelBase<ProcessUsersModel, processUser>
    {
        #region Process Users Fields مسخدمىن العمليه
        public int iProcessUserCode { get; set; } // كود مستخدم العمليه
        public Nullable<int> inUserCode { get; set; } // كود المستخدم
        public string sUserName { get; set; } // اسم مستخدم العمليه
        public Nullable<int> inProcessSubContractorCode { get; set; } // كود المقاول بالعملية
        public Nullable<int> inProcessCode { get; set; } // كود العملية
        public Nullable<bool> bnContractorType { get; set; } // نوع المقاول 0 => باطن 1=> رئيسي
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ProcessUsersModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override List<ProcessUsersModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get All Process Users.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="contractorType"> Contractor Type 'Main - Sub'. </param>
        /// <param name="userCode"> User Code. </param>
        /// <returns> List Of Process Users Model. </returns>
        internal List<ProcessUsersModel> GetAll(int processCode, int contractorType, int userCode)
        {

            var LProcessUserEF = db.spGetProcessUser(processCode, contractorType, userCode).ToList();

            List<ProcessUsersModel> LProcessModel = new List<ProcessUsersModel>();

            if (LProcessUserEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LProcessUserEF);

            return LProcessModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override ProcessUsersModel GetById(int Id)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchObjs"></param>
        /// <returns></returns>
        internal override List<ProcessUsersModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(ProcessUsersModel newObj)
        {
            try
            {
                processUser modal = new processUser();
                modal.userCode = newObj.inUserCode; // كود المستخدم
                modal.contractorType = newObj.bnContractorType; // نوع المقاول 0 => باطن 1=> رئيسي
                modal.userInsertCode = newObj.inUserInsertCode; // كود المدخل
                modal.dateInsert = dtServerTime; // وقت الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال
                modal.seen = false;
                //processSubContractorCode كود العمليه بالمقاول
                int userCode = Convert.ToInt32(newObj.inUserCode.ToString());
                UserModel user = new UserModel().GetById(userCode);
                int? contractorCode = Convert.ToInt32(user.iContractorCode.ToString());

                var processSubContractors = db.processSubContractors.FirstOrDefault(x => x.processCode == newObj.inProcessCode && x.contractorCode == contractorCode);
                if (processSubContractors != null)
                    modal.processSubContractorCode = processSubContractors.processSubContractorCode;

                db.processUsers.Add(modal);
                if (db.SaveChanges() > 0)
                {
                    this.iProcessUserCode = modal.processUserCode;
                    this.oUsers = new DataAccessLayer.user();
                    this.oUsers.phone1 = new UserModel().GetPhone(Convert.ToInt32(modal.userCode.ToString()));
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
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(ProcessUsersModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bEdit(ProcessUsersModel newObj, int Id)
        {
            throw new NotImplementedException();
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
        ///    Delete Special Process  User.
        /// </summary>
        /// <param name="Id"> Process User Code. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal bool bDelete(string Id)
        {
            try
            {
                var rowCountDelete = db.spDeleteProcessUsers(Id);
                if (rowCountDelete != null && rowCountDelete.ToString() != "0")
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///    Convert From Entity Framework To Model 'Stored Procedure'.
        /// </summary>
        /// <param name="EF"> Entity Framework Of Process Users 'Stored Procedure'. </param>
        /// <returns> Model Of Process Users. </returns>
        private ProcessUsersModel ConvertEFToObjectBasic(spGetProcessUser_Result EF)
        {
            ProcessUsersModel OProcessUsereModel = new ProcessUsersModel();
            if (EF != null)
            {
                OProcessUsereModel.iProcessUserCode = EF.processUserCode;  // كود مستخدم العمليه 
                OProcessUsereModel.inUserCode = EF.userCode;// كود المستخدم
                OProcessUsereModel.sUserName = EF.userFullName; // اسم مستخدم العمليه
            }
            return OProcessUsereModel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override ProcessUsersModel ConvertEFToObjectBasic(processUser lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Process Users 'Stored Procedure'. </param>
        /// <returns> List Of Process Users Model. </returns>
        internal List<ProcessUsersModel> ConvertEFsToObjectsBasic(List<spGetProcessUser_Result> lEf)
        {
            List<ProcessUsersModel> LProcessUsersModel = new List<ProcessUsersModel>();

            if (lEf.Count > 0)
                foreach (var item in lEf)
                    LProcessUsersModel.Add(ConvertEFToObjectBasic(item));

            return LProcessUsersModel;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessUsersModel> ConvertEFsToObjectsBasic(List<processUser> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessUsersModel> ConvertEFsToObjects(List<processUser> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessUsersModel ConvertEFToObject(processUser ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override processUser ConvertObjectToEF(ProcessUsersModel bl)
        {
            throw new NotImplementedException();
        }

    }
}
