using DataAccessLayer;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace vt_nationalAuthority.Models
{
    public class FilesPDF
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Constructor Function.
        /// </summary>
        public FilesPDF()
        {
        }

        public string pdf { get { return "pdf"; } }
        public string images { get { return "jpg,jpeg,png,tiff"; } }
        public string imagesPdf { get { return "jpg,jpeg,png,tiff,pdf"; } }

        /// <summary>
        ///  Documents => 1 => 2 => 3 => 4
        /// </summary>
        /// <param name="rootPathDirection_"> Documents بحدد الفايل الرئيسي </param>
        /// <param name="nestedFolders"> 1 => 2 => 3 => 4 مين الفولدرات اللى جوا بعض لحد ما اوصل للى هخزن فيه  </param>
        /// <returns> String. </returns>
        public string createSingleFolders(string rootFolder, string[] nestedFolders)
        {
            string rootPathDirection = HttpContext.Current.Server.MapPath("~/" + rootFolder + "/");

            foreach (string nestedFolder in nestedFolders)
            {
                rootPathDirection = System.IO.Path.Combine(rootPathDirection, nestedFolder);

                if (!System.IO.Directory.Exists(rootPathDirection))
                    System.IO.Directory.CreateDirectory(rootPathDirection);
            }
            return rootPathDirection;
        }

        /// <summary>
        /// Documents 1 => 2 => 3 => 4  
        ///                       => 5 
        ///                       => 6
        ///                       => 7
        /// </summary>
        /// <param name="rootPathDirection_"> Documents بحدد الفايل الرئيسي </param>
        /// <param name="nestedFolders"> 1 => 2 => 3  مين الفولدرات اللى جوا بعض لحد ما اوصل للى هخزن فيه </param>
        /// <param name="subFolders">=> 4 / 5 / 6 / 7لو فيه اكتر من فولدر جوا فولدر واحد </param>
        public void createManyFolders(string rootFolder, string[] nestedFolders, string[] subFolders)
        {
            string rootPathDirection = HttpContext.Current.Server.MapPath("~/" + rootFolder + "/");

            foreach (string nestedFolder in nestedFolders)
            {
                rootPathDirection = System.IO.Path.Combine(rootPathDirection, nestedFolder);

                if (!System.IO.Directory.Exists(rootPathDirection))
                    System.IO.Directory.CreateDirectory(rootPathDirection);
            }

            foreach (string subFolder in subFolders)
            {
                string subFold = System.IO.Path.Combine(rootPathDirection, subFolder);

                if (!System.IO.Directory.Exists(subFold))
                    System.IO.Directory.CreateDirectory(subFold);
            }
        }

        /// <summary>
        /// مختار فايل ولا لا  و نوعه ايه ؟
        /// </summary>
        /// <param name="hasFile"> fileUpload.HasFile مختار فايل و لا  </param>
        /// <param name="fileName">fileUpload.FileName  اسم الفايل اللى اختاره</param>
        /// <param name="extention">pdf OR jpg , jpeg , png ...  نوع الفايل صور و لا ملفات و لا ايه بالضبط</param>
        /// <returns> String. </returns>
        public string checkChooseWithSpecialExtention(HttpPostedFileBase fileUpload, string specialExtention)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)//
            {
                string[] specialExtentions = specialExtention.Replace(" ", "").Split(',');
                string extention_ = Path.GetExtension(fileUpload.FileName);

                foreach (string ex in specialExtentions)
                    if (extention_.ToLower() == "." + ex.ToLower())
                        return "found";
            }
            return specialExtention + " يجب ادخال فايل من نوع " + "/" + "Please Choose File Type " + specialExtention;
        }

        /// <summary>
        /// مختار فايل ولا لا من اى نوع
        /// </summary>
        /// <param name="hasFile"> fileUpload.HasFile مختار فايل و لا  </param>
        /// <returns> String. </returns>
        public string checkChooseAcceptAnyExtention(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)//
                return "found";

            return "برجاء ادخال ملف/Please Choose File";
        }

        /// <summary>
        /// بضيف فايل ف فولدر محدد
        /// </summary>
        /// <param name="rootFolder">Documents بحدد الفايل الرئيسي </param>
        /// <param name="nestedFolders">1 => 2 => 3 => 4 مين الفولدرات اللى جوا بعض لحد ما اوصل للى هخزن فيه</param>
        /// <param name="fileUpload">object fileUpload</param>
        /// <param name="summary">ببدا اسم الفايل بايه ؟ Docu_1_FileName => summary_numOfDocu_FileName</param>
        /// <param name="fileName">اسم الفايل اللى هخزنه</param>
        /// <returns> String. </returns>
        public string addFile(string rootFolder, string[] nestedFolders, HttpPostedFileBase fileUpload, string summary, string fileName)
        {
            string path = @"~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // ContractsPDF/1021/EFSA/
            int numOfDocu = getNumOfFile(fullPath, Path.GetExtension(fileUpload.FileName), summary);
            //string FileName = "";

            //if (paper_type == "")
            //{
            //    FileName = summary + "$$" + numOfDocu + "$$" + fileName;
            //}
            //else
            //{
            //    string recDate = recieved_date.ToString("dd-MM-yyyy");

            fileName = fileName != "" ? fileName : fileUpload.FileName;
            string FileName = summary + "$$" + numOfDocu + "$$" + fileName;
            //string FileName = summary + "$$" + numOfDocu + "$$" + fileName + Path.GetExtension(fileUpload.FileName);
            //}

            fullPath = System.IO.Path.Combine(fullPath, FileName);
            fileUpload.SaveAs(fullPath);

            return "تم الحفظ/Save File";
        }

        /// <summary>
        /// نفس وصف function addFile
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <param name="nestedFolders"></param>
        /// <param name="fileUpload"></param>
        /// <param name="fileName"></param>
        /// <returns> String. </returns>
        public string addFileWithoutNum(string rootFolder, string[] nestedFolders, HttpPostedFileBase fileUpload, string FileName, string specialExtention)
        {
            string msg = checkChooseWithSpecialExtention(fileUpload, specialExtention);
            if (msg != "found")
                return msg;

            string path = "../" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // ContractsPDF/1021/EFSA/
            //string FileName = fileName;

            //fullPath = System.IO.Path.Combine(fullPath, FileName);
            fullPath = System.IO.Path.Combine(fullPath, FileName + Path.GetExtension(fileUpload.FileName));
            fileUpload.SaveAs(fullPath);

            return "تم الحفظ/Save File";
        }

        /// <summary>
        /// بجيب رقم الفولدر اللى هضيفه 
        /// </summary>
        /// <param name="fullPath">مسار الفولدر</param>
        /// <param name="summary">بدايه الفايلات اللى هعدها ايه ؟</param>
        /// <returns> Int. </returns>
        public int getNumOfFile(string fullPath, string ext, string summary)
        {
            string[] files = Directory.GetFiles(fullPath, summary + "$$" + "*" + ext);

            if (files.Length != 0)
            {
                string LastFileName = Path.GetFileName(files[files.Length - 1]);
                string[] i = LastFileName.Split(new[] { "$$" }, StringSplitOptions.None);

                return Convert.ToInt32(i[1]) + 1;
            }
            else
                return 1;
        }

        /// <summary>
        ///   Get Last Number Of Folder.
        /// </summary>
        /// <param name="fullPath"> Full Path. </param>
        /// <returns> Int. </returns>
        public int getNumOfFolder(string fullPath)
        {
            string[] folders = Directory.GetDirectories(fullPath);

            if (folders.Length != 0)
            {
                string LastFolderName = Path.GetFileName(folders[folders.Length - 1]);
                string[] i = LastFolderName.Split(new[] { "$$" }, StringSplitOptions.None);

                return Convert.ToInt32(i[2]) + 1;
            }
            else
                return 1;
        }

        /// <summary>
        /// برجع الفايلات من مكان محدد و بدايه اسمها ايه 
        /// </summary>
        /// <param name="rootFolder">Documents بحدد الفايل الرئيسي </param>
        /// <param name="nestedFolders"> 1 => 2 => 3 => 4 مين الفولدرات اللى جوا بعض لحد ما اوصل للى هخزن فيه</param>
        /// <param name="summary">لما ادور على الفايلات بدايه اسمها ايه ؟ Docu_1_fileName</param>
        /// <returns> DataTable. </returns>
        public DataTable fillAttach(string rootFolder, string[] nestedFolders, string summary)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // Processes/20249/pAttachments/

            // Get All Files In This Folder
            string[] files = Directory.GetFiles(fullPath + "\\");

            DataTable dtfiles = new DataTable();

            dtfiles.Columns.Add("docuFullName", typeof(string));
            dtfiles.Columns.Add("docuName", typeof(string));
            dtfiles.Columns.Add("doc_create_time", typeof(string));
            dtfiles.Columns.Add("docFullPath", typeof(string));
            dtfiles.Columns.Add("uc", typeof(string));

            // Get All Folder With Files
            for (int i = 0; i < files.Length; i++)
            {
                string[] r = files[i].Split('\\');

                string[] FileName = Path.GetFileName(files[i]).Split(new[] { "$$" }, StringSplitOptions.None); // file name as دورة العمل
                string time = System.IO.File.GetCreationTime(files[i]).ToString("yyyy/MM/dd"); // time Creation

                dtfiles.Rows.Add(new string[] { r[r.Length - 1], FileName[2], time, files[i], FileName[0] }); //  File Full Name , File Name , time Creation , File Full Path 
            }

            return dtfiles;

        }

        /// <summary>
        ///   Get Files In Special Folder.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Og  Nested Folder. </param>
        /// <param name="summary"> Summary. </param>
        /// <returns> DataTable. </returns>
        public DataTable fillAttachFolders(string rootFolder, string[] nestedFolders, string summary)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // Processes/20249/pAttachments/

            string[] folders = Directory.GetDirectories(fullPath, summary + "$$" + "*");

            DataTable dtfiles = new DataTable();

            dtfiles.Columns.Add("docuFullName", typeof(string));
            dtfiles.Columns.Add("docuName", typeof(string));
            dtfiles.Columns.Add("doc_create_time", typeof(string));
            dtfiles.Columns.Add("docFullPath", typeof(string));
            dtfiles.Columns.Add("count", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("rootFolder", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("uc", typeof(string));
            dtfiles.Columns.Add("attachmentTypeCode", typeof(string));
            dtfiles.Columns.Add("attachmentTypeName", typeof(string));
            dtfiles.Columns.Add("contractorBy", typeof(string)); // Contractor Name اسم المقاول
            dtfiles.Columns.Add("userName", typeof(string)); // User Name اسم المستخدم

            // Get All Folder With Files
            for (int i = 0; i < folders.Length; i++)
            {
                string[] r = folders[i].Split('\\');
                string[] folderName = r[r.Length - 1].Split(new[] { "$$" }, StringSplitOptions.None);
                string time = System.IO.File.GetCreationTime(folders[i]).ToString("yyyy/MM/dd"); // time Creation


                // Get All Files In This Folder
                string[] files = Directory.GetFiles(folders[i] + "\\");
                var folderData = db.spGetAttachmentFolderData(Convert.ToInt32(folderName[3]), Convert.ToInt32(folderName[2]), Convert.ToInt32(folderName[0])).FirstOrDefault();
                dtfiles.Rows.Add(new string[] { r[r.Length - 1], folderName[4], time, folders[i], files.Length.ToString(), i.ToString(), folderName[2], folderName[3], folderData.attachmentTypeName, folderData.uploadedByName, folderData.userName }); //  Folder Full Name , Folder Name , time Creation , Folder Full Path , Folder 
            }

            return dtfiles;
        }

        /// <summary>
        ///   Add File With Special Extention.
        /// </summary>
        /// <param name="rootPathDirection_"> Root Path. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="fileUpload"> Lsit Of File Upload. </param>
        /// <param name="specialExtention"> Special Extentions. </param>
        /// <param name="summary"> Summary. </param>
        /// <param name="fileName"> File Name. </param>
        /// <returns> string. </returns>
        public string addFileWithSpecialExtention(string rootPathDirection_, string[] nestedFolders, HttpPostedFileBase fileUpload, string specialExtention, string summary, string fileName)
        {
            createSingleFolders(rootPathDirection_, nestedFolders);

            string msg = checkChooseWithSpecialExtention(fileUpload, specialExtention);
            if (msg != "found")
                return msg;

            return addFile(rootPathDirection_, nestedFolders, fileUpload, summary, fileName);
        }

        /// <summary>
        ///   Add Multi Files With Special Extention.
        /// </summary>
        /// <param name="rootPathDirection_"> Root Path. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="fileUpload"> List Of Files Upload. </param>
        /// <param name="specialExtention"> Special Extentions. </param>
        /// <param name="summary"> Summary. </param>
        /// <param name="fileName"> File Name. </param>
        /// <returns> string. </returns>
        public string addMultiFileWithSpecialExtention(string rootPathDirection_, string[] nestedFolders, HttpPostedFileBase[] fileUpload, string specialExtention, string summary, string fileName)
        {
            string errorFilesName = "";

            for (int i = 0; i < fileUpload.Length; i++)
            {
                string[] message;

                if (fileUpload.Length == 1)
                    message = addFileWithSpecialExtention(rootPathDirection_, nestedFolders, fileUpload[i], specialExtention, summary, fileName == "***" ? fileUpload[i].FileName : fileName).Split('/'); // "***" عشان لو عايزه اسم الفايل الاساسى هو اللى يتسجل مش الاسم اللى انا كتباه
                else
                    message = addFileWithSpecialExtention(rootPathDirection_, nestedFolders, fileUpload[i], specialExtention, summary, fileName == "***" ? fileUpload[i].FileName : fileName + " ( " + (i + 1) + " )").Split('/');

                if (message[0] != "تم الحفظ" && errorFilesName == "")
                    errorFilesName = fileUpload[i].FileName;
                else if (message[0] != "تم الحفظ" && errorFilesName != "")
                    errorFilesName += " , " + fileUpload[i].FileName;
            }

            return errorFilesName;
        }

        /// <summary>
        ///   Show File.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="fileUpload"> file Upload. </param>
        /// <returns> String. </returns>
        public string showFileOnly(string rootFolder, HttpPostedFileBase fileUpload)
        {
            string msg = checkChooseAcceptAnyExtention(fileUpload);

            if (msg != "found")
                return msg;

            string rootPathDirection = HttpContext.Current.Server.MapPath("~/" + rootFolder + "/");
            string fullPath = System.IO.Path.Combine(rootPathDirection, fileUpload.FileName); //

            fileUpload.SaveAs(fullPath);

            return fullPath;
        }

        /// <summary>
        ///   Delete Folders.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="folderName"> Folder Name. </param>
        /// <returns> Bool. </returns>
        public bool deleteFolders(string rootFolder, string[] nestedFolders, string folderName)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path + folderName);
            if (System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.Delete(fullPath, true);
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Delete File.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="fileName"> file Name. </param>
        /// <returns> Bool. </returns>
        public bool deleteFile(string rootFolder, string[] nestedFolders, string fileName)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path + fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Edit Folder Name.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="oldFolderName"> Old Folder Name. </param>
        /// <param name="newFolderName"> New Folder Name. </param>
        /// <returns> Bool. </returns>
        public bool editFolders(string rootFolder, string[] nestedFolders, string oldFolderName, string newFolderName)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string sourceDirName = HttpContext.Current.Server.MapPath(path + oldFolderName);
            string destinationDirName = HttpContext.Current.Server.MapPath(path + newFolderName);
            if (System.IO.Directory.Exists(sourceDirName))
            {
                System.IO.Directory.Move(sourceDirName, destinationDirName);
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Create Single Folders With Summary.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="uc"> User code. </param>
        /// <param name="attachmentType"> Attachment Type. </param>
        /// <param name="folderName"> Folder Name. </param>
        /// <param name="numOfFolder"> Number Of Folder. </param>
        /// <returns> String. </returns>
        public string createSingleFoldersWithSumary(string rootFolder, string[] nestedFolders, string uc, string attachmentType, string folderName, string numOfFolder)
        {
            string summary = "1";
            string rootPathDirection = HttpContext.Current.Server.MapPath("~/" + rootFolder + "/");

            for (int i = 0; i < nestedFolders.Length; i++)
            {
                // Last Folder And First Time
                if (i == nestedFolders.Length - 1 && String.IsNullOrEmpty(numOfFolder))
                {
                    summary = getNumOfFolder(rootPathDirection).ToString();
                    nestedFolders[i] = uc + "$$" + attachmentType + "$$" + summary + "$$" + folderName;
                    rootPathDirection = System.IO.Path.Combine(rootPathDirection, nestedFolders[i]);
                }

                if (!System.IO.Directory.Exists(rootPathDirection))
                    System.IO.Directory.CreateDirectory(rootPathDirection);
            }
            return summary;
        }

        /// <summary>
        ///   Add File With Special Extention And Summary.
        /// </summary>
        /// <param name="rootPathDirection_"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="fileUpload" File Upload. ></param>
        /// <param name="specialExtention"> Special Extention. </param>
        /// <param name="uc"> User code. </param>
        /// <param name="attachmentType"> Attachment Type. </param>
        /// <param name="folderName"> Folder Name. <</param>
        /// <param name="fileName"> Folder Name. </param>
        /// <param name="numOfFolder"> Number Of Folder. </param>
        /// <returns> String. </returns>
        public string addFileWithSpecialExtentionAndSummary(string rootPathDirection_, string[] nestedFolders, HttpPostedFileBase fileUpload, string specialExtention, string uc, string attachmentType, string folderName, string fileName, string numOfFolder)
        {
            string summary = createSingleFoldersWithSumary(rootPathDirection_, nestedFolders, uc, attachmentType, folderName, numOfFolder);

            string msg = checkChooseWithSpecialExtention(fileUpload, specialExtention);
            if (msg != "found")
                return msg + "/" + summary;

            return addFile(rootPathDirection_, nestedFolders, fileUpload, summary, fileName) + "/" + summary;
        }

        /// <summary>
        ///   Fet Folders With Files.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="summary"> Summary. </param>
        /// <returns> DataTable. </returns>
        public DataTable foldersWithFiles(string rootFolder, string[] nestedFolders, string summary)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // Processes/20249/pAttachments/

            string[] folders = Directory.GetDirectories(fullPath, summary + "$$" + "*");

            DataTable dtfiles = new DataTable();

            dtfiles.Columns.Add("docuFullName", typeof(string));
            dtfiles.Columns.Add("docuName", typeof(string));
            dtfiles.Columns.Add("doc_create_time", typeof(string));
            dtfiles.Columns.Add("docFullPath", typeof(string));
            dtfiles.Columns.Add("count", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("rootFolder", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("uc", typeof(string));
            dtfiles.Columns.Add("attachmentTypeCode", typeof(string));
            dtfiles.Columns.Add("attachmentTypeName", typeof(string));
            dtfiles.Columns.Add("contractorBy", typeof(string)); // Contractor Name اسم المقاول
            dtfiles.Columns.Add("userName", typeof(string)); // User Name اسم المستخدم

            // Get All Folder With Files
            for (int i = 0; i < folders.Length; i++)
            {
                string[] r = folders[i].Split('\\');
                string[] folderName = r[r.Length - 1].Split(new[] { "$$" }, StringSplitOptions.None);
                string time = System.IO.File.GetCreationTime(folders[i]).ToString("yyyy/MM/dd"); // time Creation

                var folderData = db.spGetAttachmentFolderData(Convert.ToInt32(folderName[3]), Convert.ToInt32(folderName[2]), Convert.ToInt32(folderName[0])).FirstOrDefault();

                // Get All Files In This Folder
                string[] files = Directory.GetFiles(folders[i] + "\\");
                dtfiles.Rows.Add(new string[] { r[r.Length - 1], folderName[4], time, folders[i], files.Length.ToString(), i.ToString(), folderName[2], folderName[3], folderData.attachmentTypeName, folderData.uploadedByName, folderData.userName }); //  Folder Full Name , Folder Name , time Creation , Folder Full Path , Folder 

                for (int j = 0; j < files.Length; j++)
                {
                    r = files[j].Split('\\');
                    string[] FileName = Path.GetFileName(files[j]).Split(new[] { "$$" }, StringSplitOptions.None); // file name as دورة العمل
                    time = System.IO.File.GetCreationTime(files[j]).ToString("yyyy/MM/dd"); // time Creation

                    dtfiles.Rows.Add(new string[] { r[r.Length - 1], FileName[2], time, files[j], r[r.Length - 2], i.ToString(), FileName[0], FileName[1], "", "", "" }); //  File Full Name , File Name , time Creation , File Full Path , File
                }
            }

            return dtfiles;
        }

        /// <summary>
        ///   Get Files In Folder.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="summary"> Summary. </param>
        /// <returns> DataTable. </returns>
        public DataTable filesFolder(string rootFolder, string[] nestedFolders, string summary)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // Processes/20249/pAttachments/

            DataTable dtfiles = new DataTable();

            dtfiles.Columns.Add("docuFullName", typeof(string));
            dtfiles.Columns.Add("docuName", typeof(string));
            dtfiles.Columns.Add("doc_create_time", typeof(string));
            dtfiles.Columns.Add("docFullPath", typeof(string));
            dtfiles.Columns.Add("count", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("rootFolder", typeof(string)); //  Folder => directory , File => file

            // Get All Files In This Folder
            string[] files = Directory.GetFiles(fullPath + "\\");
            for (int j = 0; j < files.Length; j++)
            {
                string[] r = files[j].Split('\\');
                string time = System.IO.File.GetCreationTime(files[j]).ToString("yyyy/MM/dd"); // time Creation

                dtfiles.Rows.Add(new string[] { r[r.Length - 1], Path.GetFileName(files[j]), time, files[j], r[r.Length - 2], j.ToString() }); //  File Full Name , File Name , time Creation , File Full Path , File
            }

            return dtfiles;
        }

        /// <summary>
        ///   Search In Folders.
        /// </summary>
        /// <param name="rootFolder"> Root Folder. </param>
        /// <param name="nestedFolders"> List Of Nested Folders. </param>
        /// <param name="searchName"> Search Name. </param>
        /// <param name="attachmentType"> Attachment Type. </param>
        /// <param name="summary"> Summary. </param>
        /// <returns> DataTable. </returns>
        public DataTable searchFolders(string rootFolder, string[] nestedFolders, string searchName, string attachmentType, string summary)
        {
            createSingleFolders(rootFolder, nestedFolders);

            string path = "~/" + rootFolder + "/";

            foreach (string folder in nestedFolders)
                path += folder + "/";

            string fullPath = HttpContext.Current.Server.MapPath(path); // Processes/20249/pAttachments/
            string[] folders = Directory.GetDirectories(fullPath, summary + "$$" + "*");

            DataTable dtfiles = new DataTable();
            dtfiles.Columns.Add("docuFullName", typeof(string));
            dtfiles.Columns.Add("docuName", typeof(string));
            dtfiles.Columns.Add("doc_create_time", typeof(string));
            dtfiles.Columns.Add("docFullPath", typeof(string));
            dtfiles.Columns.Add("count", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("rootFolder", typeof(string)); //  Folder => directory , File => file
            dtfiles.Columns.Add("uc", typeof(string));
            dtfiles.Columns.Add("attachmentTypeCode", typeof(string));
            dtfiles.Columns.Add("attachmentTypeName", typeof(string));
            dtfiles.Columns.Add("contractorBy", typeof(string)); // Contractor Name اسم المقاول
            dtfiles.Columns.Add("userName", typeof(string)); // User Name اسم المستخدم

            // Get All Folder With Files
            for (int i = 0; i < folders.Length; i++)
            {
                string[] r = folders[i].Split('\\');
                string[] folderName = r[r.Length - 1].Split(new[] { "$$" }, StringSplitOptions.None);
                string time = System.IO.File.GetCreationTime(folders[i]).ToString("yyyy/MM/dd"); // time Creation

                //int attachmentTypeCode = Convert.ToInt32(FolderName[1]);
                //string attachmentTypeName = db.attachmentTypes.FirstOrDefault(x => x.attachmentTypeCode == attachmentTypeCode).attachmentTypeName;


                // Get All Files In This Folder
                string[] files = Directory.GetFiles(folders[i] + "\\");
                var folderData = db.spGetAttachmentFolderData(Convert.ToInt32(folderName[3]), Convert.ToInt32(folderName[2]), Convert.ToInt32(folderName[0])).FirstOrDefault();
                //dtfiles.Rows.Add(new string[] { r[r.Length - 1], folderName[4], time, folders[i], files.Length.ToString(), i.ToString(), folderName[2], folderName[3], folderData.attachmentTypeName, folderData.uploadedByName, folderData.userName }); //  Folder Full Name , Folder Name , time Creation , Folder Full Path , Folder 


                if ((!String.IsNullOrEmpty(searchName) && folderName[4].Contains(searchName)) || folderName[3] == attachmentType || (String.IsNullOrEmpty(searchName) && String.IsNullOrEmpty(attachmentType)))
                    dtfiles.Rows.Add(new string[] { r[r.Length - 1], folderName[4], time, folders[i], files.Length.ToString(), i.ToString(), folderName[2], folderName[3], folderData.attachmentTypeName, folderData.uploadedByName, folderData.userName }); //  Folder Full Name , Folder Name , time Creation , Folder Full Path , Folder 

                // dtfiles.Rows.Add(new string[] { r[r.Length - 1], FolderName[2], time, folders[i], FolderName[0], FolderName[1], attachmentTypeName }); //  Folder Full Name , Folder Name , time Creation , Folder Full Path , Folder 

            }

            return dtfiles;
        }
    }
}