using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Process Stop. 
    /// </summary>
    public class ProcessStopModel : ModelBase<ProcessStopModel, processStop>
    {
        #region Process Stop Fields ايقاف العمليه
        public int iProcessStopCode { get; set; } // كود ايقاف العمليه
        public int iProcessCode { get; set; } // كود العمليه
        public Nullable<int> inStopStatusCode { get; set; } // حاله الايقاف
        [Required(ErrorMessage = "برجاء ادخال تاريخ بدء الايقاف ")]
        public string sDateStartProcessStop { get; set; } // تاريخ بدء الايقاف
        public string sDateEndProcessStop { get; set; } // تاريخ انتهاء الايقاف
        [Required(ErrorMessage = "برجاء ادخال السبب ")]
        public string sProcessStopReason { get; set; } // سبب الايقاف
        public string sDateStartPreviousProcessStop { get; set; } // تاريخ بدء الايقاف السابق
        public string sDateEndPreviousProcessStop { get; set; } // تاريخ انتهاء الايقاف السابق
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Get Object Of Reason Of Stopping Process.
        /// </summary>
        /// <param name="Id"> Reason Of Stopping Process Code. </param>
        internal override ProcessStopModel GetById(int Id)
        {
            ProcessStopModel OProcessStopModel = new ProcessStopModel();
            var OProcessStop = db.spGetPtocessStop(-1, Id, "", new DateTime(1, 1, 1), new DateTime(1, 1, 1)).FirstOrDefault();

            if (OProcessStop != null)
            {
                OProcessStopModel = this.ConvertEFToObjectBasic(OProcessStop);
                spGetPreviousPtocessStop_Result vPrevProcessStop = db.spGetPreviousPtocessStop(Id, null).FirstOrDefault();
                if (vPrevProcessStop != null)
                {
                    OProcessStopModel.sDateStartPreviousProcessStop = String.IsNullOrEmpty(vPrevProcessStop.dateStartProcessStop) ? "NULL" : vPrevProcessStop.dateStartProcessStop;
                    OProcessStopModel.sDateEndPreviousProcessStop = String.IsNullOrEmpty(vPrevProcessStop.dateEndProcessStop) ? "NULL" : vPrevProcessStop.dateEndProcessStop;
                }
                else
                {
                    OProcessStopModel.sDateStartPreviousProcessStop = "NULL";
                    OProcessStopModel.sDateEndPreviousProcessStop = "NULL";
                }
            }
            else
            {
                OProcessStopModel.sDateStartPreviousProcessStop = "NULL";
                OProcessStopModel.sDateEndPreviousProcessStop = "NULL";
            }

            return OProcessStopModel;
        }

        /// <summary>
        ///   
        /// </summary>
        internal override List<ProcessStopModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get All Reasons Of Stopping Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        internal override List<ProcessStopModel> GetAll(int Id)
        {
            int processCode = Convert.ToInt32(Id);
            var LProcessStopEF = db.spGetPtocessStop(processCode, -1, "", new DateTime(1, 1, 1), new DateTime(1, 1, 1)).ToList();

            List<ProcessStopModel> LProcessModel = new List<ProcessStopModel>();
            if (LProcessStopEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LProcessStopEF);

            return LProcessModel;
        }

        /// <summary>
        ///   Get Last Object Of Reasons Of Stopping Process For Validation.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        internal ProcessStopModel GetProcessStopPrevById(int Id)
        {
            ProcessStopModel OProcessStopModel = new ProcessStopModel();

            spGetPreviousPtocessStop_Result vPrevProcessStop = db.spGetPreviousPtocessStop(null, Id).FirstOrDefault();
            if (vPrevProcessStop != null)
            {
                OProcessStopModel.sDateStartPreviousProcessStop = String.IsNullOrEmpty(vPrevProcessStop.dateStartProcessStop) ? "NULL" : vPrevProcessStop.dateStartProcessStop;
                OProcessStopModel.sDateEndPreviousProcessStop = String.IsNullOrEmpty(vPrevProcessStop.dateEndProcessStop) ? "NULL" : vPrevProcessStop.dateEndProcessStop;
            }
            else
            {
                OProcessStopModel.sDateStartPreviousProcessStop = "NULL";
                OProcessStopModel.sDateEndPreviousProcessStop = "NULL";
            }

            return OProcessStopModel;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Reasons Of Stopping Process Model. </returns>
        internal override List<ProcessStopModel> lSearch(List<string> searchObjs)
        {
            try
            {
                int processCode = Convert.ToInt32(searchObjs[0]);
                DateTime? dtStartProcessStop = Convert.ToDateTime(searchObjs[2]);
                DateTime? dtEndProcessStop = Convert.ToDateTime(searchObjs[3]);

                var LProcessStopEF = db.spGetPtocessStop(processCode, -1, searchObjs[1], dtStartProcessStop, dtEndProcessStop).ToList();

                List<ProcessStopModel> LProcessStopModel = new List<ProcessStopModel>();
                if (LProcessStopEF.Count > 0)
                    LProcessStopModel = this.ConvertEFsToObjectsBasic(LProcessStopEF);

                return LProcessStopModel;
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
        internal override bool bSave(ProcessStopModel newObj)
        {
            try
            {
                DateTime? dtStartProcessStop = Convert.ToDateTime(newObj.sDateStartProcessStop);
                DateTime? dtEndProcessStop = newObj.sDateEndProcessStop == null ? new DateTime(2100, 1, 1) : Convert.ToDateTime(newObj.sDateEndProcessStop);

                var countWorkerAttendance = db.spGetCountWorkerAttendance(newObj.iProcessCode, dtStartProcessStop, dtEndProcessStop).FirstOrDefault();

                if (countWorkerAttendance > 0)
                    return false;

                processStop modal = new processStop();
                modal.processCode = newObj.iProcessCode; // كود العمليه
                modal.processStopReason = newObj.sProcessStopReason; // سبب الايقاف
                modal.stopStatusCode = 1;// ايقاف العمليه
                modal.dateStartProcessStop = Convert.ToDateTime(newObj.sDateStartProcessStop); // تاريخ بدايه العمليه 

                // تاريخ نهايه العمليه 
                if (String.IsNullOrEmpty(newObj.sDateEndProcessStop))
                    modal.dateEndProcessStop = null;
                else
                    modal.dateEndProcessStop = Convert.ToDateTime(newObj.sDateEndProcessStop);

                modal.userInsertCode = newObj.inUserInsertCode; // كود المدخل
                modal.dateInsert = dtServerTime; // وقت الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                db.processStops.Add(modal);
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
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(ProcessStopModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Reason Of Stopping Process Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ProcessStopModel newObj, int Id)
        {
            try
            {
                DateTime? dtStartProcessStop = Convert.ToDateTime(newObj.sDateStartProcessStop);
                DateTime? dtEndProcessStop = newObj.sDateEndProcessStop == null ? new DateTime(2100, 1, 1) : Convert.ToDateTime(newObj.sDateEndProcessStop);

                var countWorkerAttendance = db.spGetCountWorkerAttendance(newObj.iProcessCode, dtStartProcessStop, dtEndProcessStop).FirstOrDefault();

                if (countWorkerAttendance > 0)
                    return false;

                processStop model = db.processStops.FirstOrDefault(x => x.processStopCode == Id);
                if (model != null)
                {
                    model.processStopReason = newObj.sProcessStopReason; // سبب ايقاف العمليه
                    model.dateStartProcessStop = Convert.ToDateTime(newObj.sDateStartProcessStop); // تاريخ بدء العمليه

                    // تاريخ نهايه العمليه
                    if (String.IsNullOrEmpty(newObj.sDateEndProcessStop))
                        model.dateEndProcessStop = null;
                    else
                        model.dateEndProcessStop = Convert.ToDateTime(newObj.sDateEndProcessStop);

                    model.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                    model.dateUpdate = dtServerTime; // تاريخ التعديل
                    model.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعدي

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
        ///   Delete Special Reason Of Stopping Process.
        /// </summary>
        /// <param name="Id"> Code Of Reason Of Stopping Process Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.processStops.Remove(db.processStops.FirstOrDefault(x => x.processStopCode == Id));
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
        /// <param name="lEf"> Entity Framework Of Reason Of Stopping Process. </param>
        /// <returns> Model Of Reason Of Stopping Process. </returns>
        internal override ProcessStopModel ConvertEFToObjectBasic(processStop lEf)
        {
            ProcessStopModel OProcessStopeModel = new ProcessStopModel();
            if (lEf != null)
            {
                OProcessStopeModel.iProcessStopCode = lEf.processStopCode; // كود ايقاف العملية
                OProcessStopeModel.iProcessCode = lEf.processCode; // كود العمليه
                OProcessStopeModel.inStopStatusCode = lEf.stopStatusCode; // كود حاله الايقاف
                OProcessStopeModel.sProcessStopReason = lEf.processStopReason; // سبب الايقاف 
                OProcessStopeModel.sDateStartProcessStop = lEf.dateStartProcessStop.ToString(); // تاريخ بدء الايقاف
                OProcessStopeModel.sDateEndProcessStop = lEf.dateEndProcessStop == null ? null : lEf.dateEndProcessStop.ToString(); // تاريخ نهايه الايقاف
            }
            return OProcessStopeModel;
        }

        /// <summary>
        ///    Convert From Entity Framework To Model 'Stored Procedure'.
        /// </summary>
        /// <param name="EF"> Entity Framework Of Reason Of Stopping Process 'Stored Procedure'. </param>
        /// <returns> Model Of Reason Of Stopping Process. </returns>
        internal ProcessStopModel ConvertEFToObjectBasic(spGetPtocessStop_Result EF)
        {
            ProcessStopModel oProcessStoplEF = new ProcessStopModel();
            if (EF != null)
            {
                oProcessStoplEF.iProcessStopCode = EF.processStopCode; // كود ايقاف العمليه
                oProcessStoplEF.iProcessCode = EF.processCode; // كود العمليه
                oProcessStoplEF.sProcessStopReason = EF.processStopReason; // سبب ايقاف العمليه
                oProcessStoplEF.sDateStartProcessStop = EF.dateStartProcessStop.ToString(); // تاريخ بدايه العمليه
                oProcessStoplEF.sDateEndProcessStop = String.IsNullOrEmpty(EF.dateEndProcessStop) ? null : EF.dateEndProcessStop.ToString(); // تاريخ نهايه العمليه
            }
            return oProcessStoplEF;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Reasons Of Stopping Process. </param>
        /// <returns> List Of Reasons Of Stopping Process Model. </returns>
        internal override List<ProcessStopModel> ConvertEFsToObjectsBasic(List<processStop> lEf)
        {
            List<ProcessStopModel> LProcessStopModel = new List<ProcessStopModel>();

            if (lEf.Count > 0)
            {
                foreach (var item in lEf)
                {
                    ProcessStopModel oProcessStopModel = new ProcessStopModel();
                    oProcessStopModel.iProcessStopCode = item.processStopCode; // كود ايقاف العملية
                    oProcessStopModel.iProcessCode = item.processCode; // كود العمليه
                    oProcessStopModel.inStopStatusCode = item.stopStatusCode; // كود حاله الايقاف
                    oProcessStopModel.sProcessStopReason = item.processStopReason; // سبب الايقاف 
                    oProcessStopModel.sDateStartProcessStop = item.dateStartProcessStop.ToString(); // تاريخ بدء الايقاف
                    oProcessStopModel.sDateEndProcessStop = item.dateEndProcessStop == null ? null : item.dateEndProcessStop.ToString(); // تاريخ نهايه الايقاف

                    LProcessStopModel.Add(oProcessStopModel);
                }
            }
            return LProcessStopModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Reasons Of Stopping Process 'Stored Procedure'. </param>
        /// <returns> List Of Reasons Of Stopping Process Model. </returns>
        internal List<ProcessStopModel> ConvertEFsToObjectsBasic(List<spGetPtocessStop_Result> lEf)
        {
            List<ProcessStopModel> LProcessStopModel = new List<ProcessStopModel>();

            if (lEf.Count > 0)
            {
                foreach (var item in lEf)
                    LProcessStopModel.Add(ConvertEFToObjectBasic(item));
            }
            return LProcessStopModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessStopModel ConvertEFToObject(processStop ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessStopModel> ConvertEFsToObjects(List<processStop> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override processStop ConvertObjectToEF(ProcessStopModel bl)
        {
            throw new NotImplementedException();
        }

    }
}