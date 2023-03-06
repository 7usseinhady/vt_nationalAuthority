using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    [MetadataType(typeof(UserModel))]
    public class UserModel : ModelBase<UserModel, user>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        #region userDetails
        public int iUserCode { get; set; }
        [Required(ErrorMessage = "ادخال اسم المستخدم بالكامل")]
        public string sUserFullName { get; set; }
        [Required(ErrorMessage = "ادخال اسم المستخدم ")]
        public string sUserName { get; set; }

        [StringLength(18, ErrorMessage = "الرجاء ادخال كلمه المرور مكونه من 8 حروف وارقام على الاقل", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "الرجاء ادخال كلمه المرور مكونه من 8 حروف وارقام على الاقل")]
        [Required(ErrorMessage = "الرجاء ادخال كلمه المرور مكونه من 8 حروف وارقام على الاقل")]
        public string sPassword { get; set; }
        [Required(ErrorMessage = "ادخال الرقم القومى")]
        [RegularExpression(@"^.{14,}$", ErrorMessage = "الرقم القومى لا يزيد او يقل عن 14 رقم")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومى لا يزيد او يقل عن 14 رقم")]
        public string sNationalID { get; set; }
        [Required(ErrorMessage = "ادخال نوع المستخدم ")]
        public int iUserTypes { get; set; }
        [Required(ErrorMessage = "ادخال  المستخدم ")]
        public int iFilterUserType { get; set; }
        [Required(ErrorMessage = "تأكيد  كلمة المرور ")]
        public string sConfirmPassword { get; set; }
        public Nullable<int> iOfficeInsuranceCode { get; set; }
        public Nullable<int> iReferenceSideCode { get; set; }
        public Nullable<int> iContractorCode { get; set; }

        [Required(ErrorMessage = "ادخال حالة المستخدم")]
        public bool bIsActive { get; set; }
        [Required(ErrorMessage = "ادخال العنوان")]
        public string sAddress { get; set; }
        [EmailAddress(ErrorMessage = "أدخال الشكل الصحيح للموقع الالكترونى للمستخدم")]
        [Required(ErrorMessage = "أدخال الشكل الصحيح للموقع الالكترونى للمستخدم")]
        public string sE_mail { get; set; }
        [Required(ErrorMessage = "ادخال رقم الهاتف")]
        [StringLength(11, ErrorMessage = "رقم الهاتف لا يقل عن 8 ارقام ولا يزيد عن 11 رقم", MinimumLength = 8)]
        public string sPhone1 { get; set; }
        [StringLength(11, ErrorMessage = "رقم الهاتف لا يقل عن 8 ارقام ولا يزيد عن 11 رقم", MinimumLength = 8)]
        public string sPhone2 { get; set; }
        public bool bIsAdmin { get; set; }
        public string sImageURL { get; set; }
        public ReferenceSideContractorModel oRefSideCont { get; set; }

        #endregion
        /// <summary>
        /// Save User 
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(UserModel newObj)
        {
            try
            {
                user modal = new user();
                modal.userFullName = newObj.sUserFullName;
                modal.userName = newObj.sUserName;
                modal.password = newObj.sPassword;
                modal.nationalID = newObj.sNationalID;
                if (newObj.iReferenceSideCode != null || newObj.iContractorCode != null)
                    modal.officeInsuranceCode = null;
                else
                    modal.officeInsuranceCode = newObj.iOfficeInsuranceCode;
                modal.referenceSideCode = newObj.iReferenceSideCode;
                modal.contractorCode = newObj.iContractorCode;
                modal.isActive = newObj.bIsActive;
                modal.isAdmin = newObj.bIsAdmin;
                modal.address = newObj.sAddress;
                modal.e_mail = newObj.sE_mail;
                modal.phone1 = newObj.sPhone1;
                modal.phone2 = newObj.sPhone2;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;
                db.users.Add(modal);
                if (db.SaveChanges() > 0)
                {
                    this.iUserCode = modal.userCode;
                    this.oUsers = new DataAccessLayer.user();
                    this.oUsers.phone1 = new UserModel().GetPhone(Convert.ToInt32(modal.userCode.ToString()));

                    // هنا لو عندى يوسر انه داخل انه أدمن فهضيفه في جروب المستخدمين الى اسمه جروب الادمن و ده الكود بتاعه =1 
                    if (newObj.bIsAdmin)
                    {
                        groupUser Newuser = new groupUser();
                        Newuser.groupCode = 1;
                        Newuser.userCode = modal.userCode;
                        Newuser.userInsertCode = newObj.inUserInsertCode;
                        Newuser.dateInsert = dtServerTime;
                        Newuser.ipInsert = newObj.sIpInsert;
                        db.groupUsers.Add(Newuser);
                        db.SaveChanges();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        internal override bool bSave(UserModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        public bool pbSave(UserModel newObj)
        {
            return this.bSave(newObj);
        }
        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">User Code</param>
        /// <returns>Edit Done Or Not</returns>
        internal override bool bEdit(UserModel newObj, int Id)
        {
            try
            {
                bool? isAdmin = null;
                user model = db.users.FirstOrDefault(x => x.userCode == Id);
                if (model == null)
                    return false;

                isAdmin = model.isAdmin;
                model.userFullName = newObj.sUserFullName;
                model.userName = newObj.sUserName;
                model.password = newObj.sPassword;
                model.nationalID = newObj.sNationalID;
                if (newObj.iReferenceSideCode != null || newObj.iContractorCode != null)
                {
                    model.officeInsuranceCode = null;
                    /// لو كان عندى يوسر أدمن وبقي مقاول ولا جهه اسناد المفروض امسحه و اعمله ابديت
                    if (isAdmin == true)
                    {
                        newObj.bIsAdmin = false;
                        db.groupUsers.Remove(db.groupUsers.FirstOrDefault(x => x.userCode == Id && x.groupCode == 1));
                        db.SaveChanges();
                    }
                }
                else
                    model.officeInsuranceCode = newObj.iOfficeInsuranceCode;

                model.referenceSideCode = newObj.iReferenceSideCode;
                model.contractorCode = newObj.iContractorCode;
                model.isActive = newObj.bIsActive;
                model.isAdmin = newObj.bIsAdmin;
                model.address = newObj.sAddress;
                model.e_mail = newObj.sE_mail;
                model.phone1 = newObj.sPhone1;
                model.phone2 = newObj.sPhone2;
                model.userUpdateCode = newObj.inUserUpdateCode;
                model.dateUpdate = DateTime.Now;
                model.ipUpdate = newObj.sIpUpdate;

                if (db.SaveChanges() > 0)
                {
                    // هنا المستخدم حصله تعديل كان موظف عادى بعد كده بقي أدمن
                    if (isAdmin == false && model.isAdmin == true)
                    {
                        groupUser Newuser = new groupUser();
                        Newuser.groupCode = 1;
                        Newuser.userCode = Id;
                        Newuser.userInsertCode = newObj.inUserInsertCode;
                        Newuser.dateInsert = dtServerTime;
                        Newuser.ipInsert = newObj.sIpInsert;
                        db.groupUsers.Add(Newuser);
                        db.SaveChanges();
                    }
                    // هنا الموظف كان أدمن و لغيت هذه الوظيفه
                    else if (isAdmin == true && model.isAdmin == false && model.officeInsuranceCode != null)
                    {
                        // هنا بمسج المستخدم ده من جروب الأدمن
                        db.groupUsers.Remove(db.groupUsers.FirstOrDefault(x => x.userCode == Id && x.groupCode == 1));
                        db.SaveChanges();
                    }

                    return true;
                }

                else return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Search For Users
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Users</returns>
        internal override List<UserModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetUsersSearch(searchObjs[1], searchObjs[2], searchObjs[0], searchObjs[9], searchObjs[3], searchObjs[4], searchObjs[5], searchObjs[6], searchObjs[7], searchObjs[8], searchObjs[10], searchObjs[11], searchObjs[12], Convert.ToInt32(searchObjs[13])).ToList();

                List<UserModel> LuserModel = new List<UserModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        UserModel oUserModellEF = new UserModel();
                        oUserModellEF.iUserCode = (int)item.userCode;
                        oUserModellEF.sUserFullName = item.userFullName;
                        oUserModellEF.sUserName = item.userName;
                        oUserModellEF.sPassword = item.password;
                        oUserModellEF.sNationalID = item.nationalID;
                        oUserModellEF.iOfficeInsuranceCode = item.officeInsuranceCode;
                        oUserModellEF.iReferenceSideCode = item.referenceSideCode;
                        oUserModellEF.iContractorCode = item.contractorCode;
                        oUserModellEF.bIsActive = (bool)item.isActive;
                        oUserModellEF.bIsAdmin = (bool)item.isAdmin;
                        oUserModellEF.sAddress = item.address;
                        oUserModellEF.sE_mail = item.e_mail;
                        oUserModellEF.sPhone1 = item.phone1;
                        oUserModellEF.sPhone2 = item.phone2;
                        oUserModellEF.inUserInsertCode = item.userInsertCode;
                        LuserModel.Add(oUserModellEF);
                    }
                }

                return LuserModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="Id">User Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                bool check = db.groupUsers.Any(x => x.userCode == Id && x.groupCode == 1);
                if (check)
                    db.groupUsers.Remove(db.groupUsers.FirstOrDefault(x => x.userCode == Id && x.groupCode == 1));

                user user = db.users.FirstOrDefault(x => x.userCode == Id);
                if (user == null)
                    return false;

                this.sImageURL = user.imageURL;
                db.users.Remove(user);

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
        /// Get One User
        /// </summary>
        /// <param name="Id">User Code</param>
        /// <returns>User Data</returns>
        internal override UserModel GetById(int Id)
        {
            UserModel OuserModel = new UserModel();
            user OUser = db.users.FirstOrDefault(x => x.userCode == Id);
            if (OUser != null)
            {
                OuserModel = this.ConvertEFToObjectBasic(OUser);
            }
            return OuserModel;
        }
        /// <summary>
        /// Get All Users In Offices Insurance
        /// </summary>
        /// <returns>List Of Users</returns>
        public List<UserModel> GetAllUserGroup()
        {
            List<UserModel> LuserModel = new List<UserModel>();
            List<user> LuserEF = db.users.Where(x => x.officeInsuranceCode != null).ToList();

            if (LuserEF != null)
            {
                LuserModel = this.ConvertEFsToObjects(LuserEF);
            }
            return LuserModel;
        }
        /// <summary>
        ///  Get List Of All User for special User
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <returns> Request. </return>
        internal List<UserModel> GetAllByUser(int userCode)
        {
            List<UserModel> LuserModel = new List<UserModel>();
            List<user> LuserEF = db.users.Where(x => x.userInsertCode == userCode || userCode == -1).Take(1008).ToList();

            if (LuserEF != null)
            {
                LuserModel = this.ConvertEFsToObjects(LuserEF);
            }
            return LuserModel;
        }
        /// <summary>
        /// Convert Object Of Entity Framework 'user' To Oject Of 'userModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'user'</param>
        /// <returns>Oject Of 'userModel'</returns>
        internal override UserModel ConvertEFToObjectBasic(user lEf)
        {
            UserModel OUserModel = new UserModel();
            if (lEf != null)
            {
                OUserModel.iUserCode = lEf.userCode;
                OUserModel.sUserFullName = lEf.userFullName;
                OUserModel.sUserName = lEf.userName;
                OUserModel.sPassword = lEf.password;
                OUserModel.sNationalID = lEf.nationalID;
                OUserModel.iOfficeInsuranceCode = lEf.officeInsuranceCode;
                OUserModel.iReferenceSideCode = lEf.referenceSideCode;
                OUserModel.iContractorCode = lEf.contractorCode;
                OUserModel.bIsActive = lEf.isActive;
                OUserModel.bIsAdmin = (bool)lEf.isAdmin;
                OUserModel.sAddress = lEf.address;
                OUserModel.sE_mail = lEf.e_mail;
                OUserModel.sPhone1 = lEf.phone1;
                OUserModel.sPhone2 = lEf.phone2;
                OUserModel.inUserInsertCode = lEf.userInsertCode;
                OUserModel.dtDateInsert = lEf.dateInsert;
                OUserModel.sIpInsert = lEf.ipInsert;
                OUserModel.inUserUpdateCode = lEf.userUpdateCode;
                OUserModel.dtDateUpdate = lEf.dateUpdate;
                OUserModel.sIpUpdate = lEf.ipUpdate;
                OUserModel.sImageURL = lEf.imageURL;
                OUserModel.inUserInsertCode = lEf.userInsertCode;
            }
            return OUserModel;
        }
        /// <summary>
        /// Convert List Of Entity Framework 'user' To List Of 'userModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'user'</param>
        /// <returns>List Of 'userModel'</returns>
        internal override List<UserModel> ConvertEFsToObjects(List<user> lEf)
        {
            List<UserModel> LUsersModel = new List<UserModel>();
            if (lEf != null)
            {
                foreach (user oEF in lEf)
                {
                    LUsersModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LUsersModel;
        }
        /// <summary>
        /// Convert List Of Entity Framework 'user' To List Of 'userModel'
        /// </summary>
        /// <param name="lEF">List Of Entity Framework 'user'</param>
        /// <returns>List Of 'userModel'</returns>
        internal override List<UserModel> ConvertEFsToObjectsBasic(List<user> lEf)
        {
            List<UserModel> LUsers = new List<UserModel>();
            ConvertEFsToObjects(lEf);
            return LUsers;
        }
        /// <summary>
        /// Convert  Oject Of 'userModel' To Object Of Entity Framework 'user'
        /// </summary>
        /// <param name="bl">Object Of  'userModel'</param>
        /// <returns> Object Of Entity Framework 'user</returns>
        internal override user ConvertObjectToEF(UserModel bl)
        {

            user OUserModel = new user();
            if (bl != null)
            {
                OUserModel.userCode = bl.iUserCode;
                OUserModel.userFullName = bl.sUserFullName;
                OUserModel.userName = bl.sUserName;
                OUserModel.password = bl.sPassword;
                OUserModel.nationalID = bl.sNationalID;
                OUserModel.officeInsuranceCode = bl.iOfficeInsuranceCode;
                OUserModel.referenceSideCode = bl.iReferenceSideCode;
                OUserModel.contractorCode = bl.iContractorCode;
                OUserModel.isActive = bl.bIsActive;
                OUserModel.isAdmin = bl.bIsAdmin;
                OUserModel.address = bl.sAddress;
                OUserModel.e_mail = bl.sE_mail;
                OUserModel.phone1 = bl.sPhone1;
                OUserModel.phone2 = bl.sPhone2;
                OUserModel.userInsertCode = bl.inUserInsertCode;
                OUserModel.dateInsert = bl.dtDateInsert;
                OUserModel.ipInsert = bl.sIpInsert;
                OUserModel.userUpdateCode = bl.inUserUpdateCode;
                OUserModel.dateUpdate = bl.dtDateUpdate;
                OUserModel.ipUpdate = bl.sIpUpdate;
            }
            return OUserModel;
        }


        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>List Of Users</returns>
        internal override List<UserModel> GetAll()
        {
            List<UserModel> LuserModel = new List<UserModel>();
            List<user> LuserEF = db.users.ToList();

            if (LuserEF != null)
            {
                LuserModel = this.ConvertEFsToObjects(LuserEF);
            }
            return LuserModel;
        }
        internal override List<UserModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        internal override UserModel ConvertEFToObject(user ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Update User Image
        /// </summary>
        /// <param name="newObj">Data Need For Update Image</param>
        /// <param name="Id">User Code</param>
        /// <returns>Update Done Or Not</returns>
        internal bool updateImage(UserModel newObj, int Id)
        {
            try
            {
                user model = db.users.FirstOrDefault(x => x.userCode == Id);
                if (model != null)
                {
                    model.imageURL = "/UserImage/" + Id.ToString() + newObj.sImageURL;
                    if (db.SaveChanges() > 0)
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
        /// Change Password Of User
        /// </summary>
        /// <param name="newObj">Data Need For Change</param>
        /// <returns>Change Done Or Not</returns>
        public bool bChangePassword(UserModel newObj)
        {
            try
            {
                user user = db.users.FirstOrDefault(userObj => userObj.userCode == newObj.iUserCode);

                if (user == null)
                    return this.bIsEdit = false;

                user.password = newObj.sPassword;
                user.changePassword = true;
                user.dateChangePassword = DateTime.Now;

                if (db.SaveChanges() > 0)
                    this.bIsEdit = true;
                else
                    this.bIsEdit = false;

            }
            catch { this.bIsEdit = false; }

            return this.bIsEdit;
        }
        /// <summary>
        /// Get Phones For User
        /// </summary>
        /// <param name="Id">User Code</param>
        /// <returns>Phone Number</returns>
        internal string GetPhone(int Id)
        {
            string phone = null;
            user OUser = db.users.FirstOrDefault(x => x.userCode == Id);
            if (OUser != null)
                phone = GetPhone(OUser);

            return phone;
        }

        private string GetPhone(user OUser)
        {
            if (!String.IsNullOrEmpty(OUser.phone1))
            {
                return OUser.phone1;
            }
            return (!String.IsNullOrEmpty(OUser.phone2) ? OUser.phone2 : null);
        }

        /// <summary>
        /// Get User By Llegal Entity
        /// </summary>
        /// <param name="Id">User Code</param>
        /// <returns>User Data</returns>
        internal UserModel GetByIdLegalEntity(int Id)
        {
            UserModel OuserModel = new UserModel();
            user OUser = db.users.FirstOrDefault(x => x.userCode == Id);
            if (OUser != null)
            {
                OuserModel = this.ConvertEFToObjectBasicLegalEntity(OUser);
            }
            return OuserModel;
        }
        /// <summary>
        /// Convert  Object Of Entity Framework 'user' To Object Of 'userModel'
        /// </summary>
        /// <param name="EF">Object Of Entity Framework 'user'/param>
        /// <returns> Object Of 'userModel'</returns>
        internal UserModel ConvertEFToObjectBasicLegalEntity(user EF)
        {
            UserModel OUserModel = new UserModel();
            if (EF != null)
            {
                OUserModel.iUserCode = EF.userCode;
                OUserModel.sUserFullName = EF.userFullName;
                OUserModel.sUserName = EF.userName;
                OUserModel.sPassword = EF.password;
                OUserModel.sNationalID = EF.nationalID;
                OUserModel.iOfficeInsuranceCode = EF.officeInsuranceCode;
                OUserModel.iReferenceSideCode = EF.referenceSideCode;
                OUserModel.iContractorCode = EF.contractorCode;
                OUserModel.bIsActive = EF.isActive;
                OUserModel.bIsAdmin = (bool)EF.isAdmin;
                OUserModel.sAddress = EF.address;
                OUserModel.sE_mail = EF.e_mail;
                OUserModel.sPhone1 = EF.phone1;
                OUserModel.sPhone2 = EF.phone2;
                OUserModel.inUserInsertCode = EF.userInsertCode;
                OUserModel.dtDateInsert = EF.dateInsert;
                OUserModel.sIpInsert = EF.ipInsert;
                OUserModel.inUserUpdateCode = EF.userUpdateCode;
                OUserModel.dtDateUpdate = EF.dateUpdate;
                OUserModel.sIpUpdate = EF.ipUpdate;
                OUserModel.sImageURL = EF.imageURL;
                OUserModel.oRefSideCont = new ReferenceSideContractorModel();

                if (OUserModel.iOfficeInsuranceCode == null)
                {
                    int? refSideCont = OUserModel.iReferenceSideCode == null ? OUserModel.iContractorCode : OUserModel.iReferenceSideCode;
                    OUserModel.oRefSideCont = new ReferenceSideContractorModel().GetById(Convert.ToInt32(refSideCont));
                }


            }
            return OUserModel;
        }

    }
}