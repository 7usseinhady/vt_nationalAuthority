using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Users
{
    public class GroupController : Controller
    {
        // GET: Group
        ConnectionApi conApi = new ConnectionApi();
        GroupRequest OgroupRequest = new GroupRequest();
        UserRequest OuserRequest = new UserRequest();
        GeneralMethods generalMethods = new GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        static int? insPageNumber,GroudId;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public GroupController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        #region Group
        /// <summary>
        /// Page Show Of Groups
        /// </summary>
        /// <param name="inPage">Current Number Page</param>
        /// <param name="ddlGroups">Groups Codes For Search</param>
        /// <param name="IDDelete">Group Code Want To Delete</param>
        /// <returns>View Page Of Groups</returns>
        public ActionResult vGroupIndex(int? inPage, string[] ddlGroups, int? IDDelete)
        {
            try
            {
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                // Search
                if (ddlGroups != null)
                {
                    string groupList = generalMethods.sConcatString(ddlGroups, ',');
                    List<string> Groups = new List<string>();
                    Groups.Add(groupList);

                    OgroupRequest = conApi.connectionApiSearchList<GroupRequest>("apiGroup", "PostSearchGroup", Groups);
                }
                else if (IDDelete != null) // Delete
                {
                    OgroupRequest = conApi.connectionApiDelete<GroupRequest>("apiGroup", "DeleteGroup", IDDelete.ToString());
                    if (OgroupRequest.bIsDeleted)
                        TempData["msg"] = generalVariables.DeleteDone;
                    else
                    {
                        TempData["msg"] = generalVariables.DeleteNotDone;
                        OgroupRequest = conApi.connectionApiGetList<GroupRequest>("apiGroup", "GetGroup", null);
                    }
                }
                else // Index
                    OgroupRequest = conApi.connectionApiGetList<GroupRequest>("apiGroup", "GetGroup", null);
                if (OgroupRequest.LModels == null)
                    OgroupRequest.LModels = new List<DataAccessLayer.Models.GroupModel>();
                return View(OgroupRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Page Of Save /  Edit Group
        /// </summary>
        /// <param name="inPage">Current Number Page</param>
        /// <param name="ID">Group Code</param>
        /// <returns>View Page </returns>
        public ActionResult _vpSaveEditGroup(int? inPage, int?ID)
        {
            try
            {
                insPageNumber = inPage == null ? 1 : inPage;
                GroupRequest Groups = new GroupRequest();
                // Edit 
                if (ID != null)
                {
                    Groups = conApi.connectionApiGetList<GroupRequest>("apiGroup", "GetGroup", ID.ToString());
                    return PartialView(Groups);
                }
                else // Insert
                    return PartialView(Groups);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save / Edit Group
        /// </summary>
        /// <param name="oModalRequest">Data Of Group</param>
        /// <returns>View Page </returns>
        [HttpPost]
        public ActionResult _vpSaveEditGroup(GroupRequest oModalRequest)
        {
            try
            {
                oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                if (oModalRequest.OModel.iGroupCode > 0) // Edit
                {
                    string id = oModalRequest.OModel.iGroupCode.ToString();
                    OgroupRequest = conApi.connectionApiPost<GroupRequest>("apiGroup", "PostEditGroup", oModalRequest, id);
                    if (OgroupRequest.bIsEdit)
                        TempData["msg"] = generalVariables.EditDone;
                    else
                        TempData["msg"] = generalVariables.EditNotDone;
                }
                else // Save
                {
                    OgroupRequest = conApi.connectionApiPost<GroupRequest>("apiGroup", "PostSaveGroup", oModalRequest, null);
                    if (OgroupRequest.bIsSaved)
                        TempData["msg"] = generalVariables.SaveDone;
                    else
                        TempData["msg"] = generalVariables.SaveNotDone;
                }
                return RedirectToAction("vGroupIndex", new {inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        ///  Search Page Of Group
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult _vpSearchGroup()
        {
            ViewBag.Groups = new SelectList(db.groups.ToList(), "groupCode", "groupName");
            return PartialView();
        }
        /// <summary>
        /// Cancel Search
        /// </summary>
        /// <returns>View Page Of Groups</returns>
        public ActionResult _vpSearchCancel()
        {
            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction("vGroupIndex");
        }
        #endregion
        #region GroupUser
        /// <summary>
        /// Page Of Group Users
        /// </summary>
        /// <param name="GroupCode">Group Code</param>
        /// <returns>View Page</returns>
        public ActionResult vGroupUsers(int GroupCode)
        {
            try
            {
                GroudId = GroupCode;
                TempData["groupName"] = db.groups.FirstOrDefault(x => x.groupCode == (int)GroupCode).groupName;
                //موظفين التأمينات
                OuserRequest = conApi.connectionApiGetList<UserRequest>("apiUsers", "GetUserGroup", null);
                // موظفين المجموعه
                OgroupRequest = conApi.connectionApiGetList<GroupRequest>("apiGroup", "GetGroupUser", GroupCode.ToString());

                List<int?> lstusers = new List<int?>();
                lstusers = db.GetGroupUsers().ToList();
                TempData["lstusers"] = lstusers;  //
                TempData["userList"] = OuserRequest.LModels;

                //var model = OuserRequest.LModels.Where(x => !lstusers.Contains(x.iUserCode)).OrderBy(x => x.sUserFullName);
                if (OgroupRequest.LgroupUserModel == null)
                    OgroupRequest.LgroupUserModel = new List<DataAccessLayer.Models.GroupUserModel>();
                return View(OgroupRequest.LgroupUserModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save / Edit Group Users
        /// </summary>
        /// <param name="formCollection">Data of Users</param>
        /// <returns>View Page Of Group Users</returns>
        [HttpPost]
        public ActionResult vGroupUserSave(FormCollection formCollection)
        {
            try
            {
                string UsersCode = "";
                if (!string.IsNullOrEmpty(formCollection["check"]))
                {
                    string[] temp = formCollection["check"].Split(',');

                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i] != "false")
                        {
                            UsersCode += temp[i] + ',';
                        }
                    }
                    GroupRequest NewReq = new GroupRequest();
                    NewReq.OgroupUserModel = new DataAccessLayer.Models.GroupUserModel();
                    NewReq.OgroupUserModel.iGroupCode = (int)GroudId;
                    NewReq.OgroupUserModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                    NewReq.OgroupUserModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                    OgroupRequest = conApi.connectionApiPost<GroupRequest>("apiGroup", "PostSaveGroupUsers", NewReq, UsersCode);
                    if (OgroupRequest.bIsSaved)
                        TempData["msg"] = generalVariables.SaveDone;
                    else
                        TempData["msg"] = generalVariables.SaveNotDone;
                }
                return RedirectToAction("vGroupUsers", new { GroupCode = GroudId });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Delete Users From Group
        /// </summary>
        /// <param name="formCollection">Data Of Users</param>
        /// <returns>View Page Of Group Users</returns>
        [HttpPost]
        public ActionResult vGroupUserDelete(FormCollection formCollection)
        {
            try
            {
                string DeleteCode = "";
                if (!string.IsNullOrEmpty(formCollection["chkUserGroup"]))
                {
                    if (!string.IsNullOrEmpty(formCollection["chkUserGroup"]))
                    {
                        string[] temp = formCollection["chkUserGroup"].Split(',');
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i] != "false")
                            {
                                DeleteCode += temp[i] + ','; ;  // users code
                            }
                        }
                    }
                    OgroupRequest = conApi.connectionApiDelete<GroupRequest>("apiGroup", "DeleteUserGroup", DeleteCode + GroudId.ToString());
                }
                return RedirectToAction("vGroupUsers", new { GroupCode = GroudId });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region permision     
        /// <summary>
        /// Permissions For Group
        /// </summary>
        /// <param name="GroupCode">Group Code</param>
        /// <returns></returns>
        public ActionResult vGroupPermision(int? GroupCode)
        {
            try
            {
                TempData["group_name"] = db.groups.FirstOrDefault(x => x.groupCode == GroupCode).groupName;
                OgroupRequest = conApi.connectionApiGetList<GroupRequest>("apiGroup", "GetAllFunctions", GroupCode.ToString());
                if (OgroupRequest.LfunctionModel == null)
                    OgroupRequest.LfunctionModel = new List<DataAccessLayer.Models.FunctionModel>();
                return View(OgroupRequest.LfunctionModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save / Edit Permisssions For Group
        /// </summary>
        /// <param name="permissionValue">Permissions Codes</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>View Page Permissions For Group</returns>
        public bool EditGroupPermision(string permissionValue, int groupCode)
        {
            try
            {
                //PostSaveGroupPerm
                string GroupPerm = groupCode.ToString() + "," + permissionValue + "1";
                OgroupRequest.OfunctionModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                OgroupRequest.OfunctionModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                OgroupRequest = conApi.connectionApiPost<GroupRequest>("apiGroup", "PostSaveGroupPerm", OgroupRequest, GroupPerm);
                if (OgroupRequest.bIsSaved)
                    TempData["msg"] = generalVariables.SaveDone;
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
                return true;
            }
            catch(Exception ex)
            {
                TempData["msg"] = generalVariables.SaveNotDone;
                return false;
            }
        }
        #endregion
    }
}