using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Areas.SLA.Models;
using CwcExim.Controllers;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;
using CwcExim.Areas.Report.Models;

namespace CwcExim.Areas.SLA.Controllers
{
    public class PPG_SLAController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public string BranchName { get; set; }

        public string EffectVersionLogoFile { get; set; }
        //List<string> lstSBPrint = new List<string>();
        public PPG_SLAController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            BranchName = objCompanyDetails.BranchName;
        }

        #region Token Registration
        [HttpGet]
       public ActionResult TicketGeneration()
       {
            Login ObjLogin = (Login)Session["LoginUser"];
            ViewBag.UserName = ObjLogin.LoginId;
            return PartialView();
       }     

        [HttpPost]
        //[ValidateAntiForgeryTicket]
        public ActionResult AddEditSLARegistration()
        {
            string fname = "";
            string Filename = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            Filename = fname;
                        }
                        else
                        {
                            fname = file.FileName;
                            Filename = fname;
                        }

                        // Get the complete folder path and store the file inside it.
                        string pFileName = "";
                        pFileName = DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss");

                        string ext = Path.GetExtension(fname);
                        string FileWithoutExtension = Path.GetFileNameWithoutExtension(fname);
                        fname = FileWithoutExtension + "_" + pFileName + ext;
                        Filename = fname;
                        string folderPath = "";
                        folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SLA_Files");

                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        fname = Path.Combine(Server.MapPath("~/Content/SLA_Files/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    //return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }

            PPGSLA ObjSLA = new PPGSLA();
                           
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjSLA.Uid = ObjLogin.Uid;
            ObjSLA.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
            //DateTime DT = Convert.ToDateTime(Request.Form["RaisedOn"].ToString()) ;

            string ROn, IDescription, RLevel, IType = "";

            ROn = Request.Form["RaisedOn"].ToString();
            IDescription = Request.Form["IssueDescription"].ToString();
            RLevel = Request.Form["ResolutionLevel"].ToString();
            IType = Request.Form["IssueType"].ToString();

            ObjSLA.RaisedOn = Request.Form["RaisedOn"].ToString();
            ObjSLA.IssueDescription = Request.Form["IssueDescription"].ToString();
            ObjSLA.ResolutionLevel = Request.Form["ResolutionLevel"].ToString();
            ObjSLA.IssueType = Request.Form["IssueType"].ToString();
            ObjSLA.FileName = Filename;
            PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.AddEditSLARegistration(ObjSLA);

            //send Email

            string Subject = "SLA Ticket Registration";
            string TicketNo = Newtonsoft.Json.JsonConvert.SerializeObject(ObjCR.DBResponse.Data).ToString(); ;
            //TicketNo = TicketNo.Substring(1, TicketNo.Length - 2);
            Subject = "Issue Notice on dated " + ROn + " " + "by " + BranchName + " Ticket Number " + TicketNo;
            //TicketNo = ObjCR.DBResponse.Data.;            
            
            SendMail(Subject, ROn, IDescription, RLevel, IType, TicketNo);
            ModelState.Clear();
            return Json(ObjCR.DBResponse);                        
        }
        private void SendMail(string subject,string ROn,string IDescription,string RLevel,string IType, string TicketNo)
        {           

            log.Info("Send Mail Method Started ");

            Login ObjLogin = (Login)Session["LoginUser"];

            //string EmailFrom = "";
            //string EmailPwd = "";
            //string SmtpHost = "";
            //string SmtpPort = "";

            TicketNo = TicketNo.Trim('"');

            var message = new MailMessage();
            string to = "cwc.helpdesk@score.co.in";            
            message.To.Add(new MailAddress(to));


            if(BranchName == "WhiteField")
            {
                string to1 = "cfswfd.cwc@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Patparganj")
            {
                string to1 = "cwcicd-ppg@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Dashrath")
            {
                string to1 = "icd.dashrath@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "HYDERABAD")
            {
                string to1 = "cfs.kukatpally@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Kandla")
            {
                string to1 = "cwccfs-kpt@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Kolkata")
            {
                string to1 = "cfskol@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Ambad")
            {
                string to1 = "cwcambad.2@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            log.Info("To: " + to);

            string bcc = "vineet.kumar@score.co.in";
            message.Bcc.Add(new MailAddress(bcc));

            log.Info("bcc: " + bcc);

            //message.From = new MailAddress(WebConfigurationManager.AppSettings["EmailFrom"]);
            //message.From = new MailAddress(EmailFrom);
            message.Subject = subject;
            string html = "";
            html = "Ticket No : " + TicketNo + "<br/><br/>";
            html += "Ticket Date : " + ROn + "<br/><br/>";
            html += "Raised By : " + ObjLogin.LoginId + "<br/><br/>";
            html += "From CFS/ICD : " + BranchName + "<br/><br/>";
            html += "Level : " + RLevel + "<br/><br/>";
            html += "Description : " + IDescription + "<br/><br/>";
            html += "Issue Type : " + IType + "<br/><br/>";


            message.Body = html;
            message.IsBodyHtml = true;

            log.Info("Subject : " + subject);
            log.Info("html : " + html);

            //string FromMail = WebConfigurationManager.AppSettings["EmailFrom"];
            //string FromMailPassword = WebConfigurationManager.AppSettings["EmailPwd"];
            //string SmtpHost = WebConfigurationManager.AppSettings["SmtpHost"];
            //int SmtpPort = Convert.ToInt32(WebConfigurationManager.AppSettings["SmtpPort"]);

            string FromMail = Session["EmailFrom"].ToString();
            string FromMailPassword = Session["EmailPwd"].ToString();
            string SmtpHost = Session["SmtpHost"].ToString();
            int SmtpPort = Convert.ToInt32(Session["SmtpPort"]);

            log.Info("FromMail : " + FromMail);
            log.Info("FromMailPassword : " + FromMailPassword);
            log.Info("SmtpHost : " + SmtpHost);
            log.Info("SmtpPort : " + SmtpPort);

            message.From = new MailAddress(FromMail);

            using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromMail, FromMailPassword);

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    log.Info("Exception Message : " + ex.Message);
                    log.Info("Exception StackTrace: " + ex.StackTrace);
                    log.Info("Exception Inner Exception: " + ex.InnerException);
                }
            }

            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = WebConfigurationManager.AppSettings["EmailFrom"],
            //        Password = WebConfigurationManager.AppSettings["EmailPwd"]
            //    };

            //    smtp.Host = WebConfigurationManager.AppSettings["SmtpHost"];
            //    smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SmtpPort"]);
            //    smtp.EnableSsl = false;

            //    await smtp.SendMailAsync(message);
            //}
        }

        [HttpGet]
        public ActionResult GetRegistrationList()
        {
                Login ObjLogin = (Login)Session["LoginUser"];
            
                PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.GetAllSLARegistration(0,ObjLogin.Uid,0);

            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return PartialView("SLARegistrationList", LstRegistration);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataRegistration(int Page)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            PPGSLARepository ObjCR = new PPGSLARepository();
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            ObjCR.GetAllSLARegistration(0, ObjLogin.Uid, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstRegistration, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRegistrationSearchList(string SearchValue, int Page = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            PPGSLARepository ObjER = new PPGSLARepository();
            ObjER.GetAllRegistrationSearch(((Login)Session["LoginUser"]).Uid, SearchValue, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjER.DBResponse.Data;
            }
            return PartialView("SLARegistrationList", LstRegistration);
        }

        [HttpGet]
        public ActionResult ViewSLARegistration(int TicketId)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            
            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGSLARegistrationList ObjSLA = new PPGSLARegistrationList();
            if (TicketId > 0)
            {
                ObjCR.GetViewSLARegistration(TicketId,ObjLogin.Uid,0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGSLARegistrationList)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewSLARegistration", ObjSLA);
        }

        #endregion

        #region Registration SITL
        [HttpGet]
        public ActionResult GetRegistrationListSitl()
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.GetAllSLARegistrationSitl(0,0);

            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return PartialView("SLARegistrationListSitl", LstRegistration);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataSitl(int Page)
        {
            PPGSLARepository ObjCR = new PPGSLARepository();
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            ObjCR.GetAllSLARegistrationSitl(0,Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstRegistration, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRegistrationSearchListSitl(string SearchValue, int Page = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            PPGSLARepository ObjER = new PPGSLARepository();
            ObjER.GetAllRegistrationSearch(((Login)Session["LoginUser"]).Uid, SearchValue, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjER.DBResponse.Data;
            }
            return PartialView("SLARegistrationListSitl", LstRegistration);
        }

        [HttpGet]
        public ActionResult OpenSLARegistration(int TicketId)
        {           

            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGSLARegistrationList ObjSLA = new PPGSLARegistrationList();
            
            if (TicketId > 0)
            {
                ObjCR.GetSLARegistrationDetail(TicketId,0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGSLARegistrationList)ObjCR.DBResponse.Data;
                }
            }
            ObjSLA.ResolveDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return PartialView("SLARegistrationSitl", ObjSLA);
        }

        [HttpPost]
        //[ValidateAntiForgeryTicket]
        public ActionResult AddEditSLAResolution(PPGSLARegistrationList objResolution)
        {
            if (ModelState.IsValid)
            {
                PPGSLARepository ObjIR = new PPGSLARepository();
                ObjIR.AddEditSLAResolution(objResolution);

                //check if response is ok or not

                string ROn, IComment, RLevel, IType, Subject, TicketNo, IStatus, ResolvedOn = "";

                ROn = Request.Form["RaisedOn"].ToString();
                ResolvedOn = Request.Form["ResolveDate"].ToString();
                IComment = Request.Form["Remarks"].ToString();
                RLevel = Request.Form["ResolutionLevel"].ToString();
                IType = Request.Form["IssueType"].ToString();
                TicketNo = Request.Form["TicketNo"].ToString();
                IStatus = Request.Form["IssueStatus"].ToString();

                //Send Return mail to the client on adding any resolution
                Subject = "Re: Issue Notice on dated " + ROn + " " + "by " + BranchName + " Ticket Number " + TicketNo;
                if(ObjIR.DBResponse.Status==1)
                {
                    SendResolutionMail(Subject, ResolvedOn, IComment, IStatus);
                }
            


                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }

        private void SendResolutionMail(string subject, string ResolvedOn, string IComment, string Status)
        {

            log.Info("Send Mail Method Started ");

            Login ObjLogin = (Login)Session["LoginUser"];
            

            var message = new MailMessage();
            string to = "cwc.helpdesk@score.co.in";
            //string to = "vineet.kumar@score.co.in";
            message.To.Add(new MailAddress(to));

            if (BranchName == "WhiteField")
            {
                string to1 = "cfswfd.cwc@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Patparganj")
            {
                string to1 = "cwcicd-ppg@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Dashrath")
            {
                string to1 = "icd.dashrath@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "HYDERABAD")
            {
                string to1 = "cfs.kukatpally@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Kandla")
            {
                string to1 = "cwccfs-kpt@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Kolkata")
            {
                string to1 = "cfskol@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Ambad")
            {
                string to1 = "cwcambad.2@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            log.Info("To: " + to);

            string bcc = "vineet.kumar@score.co.in";
            message.Bcc.Add(new MailAddress(bcc));

            //string bcc1 = "tonmoy.ghosh@score.co.in";
            //message.Bcc.Add(new MailAddress(bcc1));

            log.Info("bcc: " + bcc);

            if(Status == "New")
            {
                Status = "New Requirement";
            }

            if (Status == "OnHold")
            {
                Status = "On Hold";
            }

            //message.From = new MailAddress(WebConfigurationManager.AppSettings["EmailFrom"]);
            //message.From = new MailAddress(EmailFrom);
            message.Subject = subject;
            string html = "";
            html = "Dear User, <br/><br/>";
            html += "Please find your status of your ticket cited above. <br/><br/>";
            html += "Resolved on : " + ResolvedOn + "<br/><br/>";
            html += "Status : " + Status + "<br/><br/>";
            html += "Comments : " + IComment + "<br/><br/>";
            html += "Regards, <br/><br/>";
            html += "Help Desk Team <br/><br/>";
            html += "WWW.CWC-CFS.COM";

            message.Body = html;
            message.IsBodyHtml = true;

            log.Info("Subject : " + subject);
            log.Info("html : " + html);

            string FromMail = Session["EmailFrom"].ToString();
            string FromMailPassword = Session["EmailPwd"].ToString();
            string SmtpHost = Session["SmtpHost"].ToString();
            int SmtpPort = Convert.ToInt32(Session["SmtpPort"]);

            log.Info("FromMail : " + FromMail);
            log.Info("FromMailPassword : " + FromMailPassword);
            log.Info("SmtpHost : " + SmtpHost);
            log.Info("SmtpPort : " + SmtpPort);

            message.From = new MailAddress(FromMail);

            using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromMail, FromMailPassword);

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    log.Info("Exception Message : " + ex.Message);
                    log.Info("Exception StackTrace: " + ex.StackTrace);
                    log.Info("Exception Inner Exception: " + ex.InnerException);
                }
            }            
        }

        #endregion

        #region Registration List User
        [HttpGet]
        public ActionResult GetRegistrationListUser()
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.GetSLARegistrationListUser(0,ObjLogin.Uid, 0);

            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return PartialView("SLARegistrationListUser", LstRegistration);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataUser(int Page)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            PPGSLARepository ObjCR = new PPGSLARepository();
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            ObjCR.GetSLARegistrationListUser(0, ObjLogin.Uid, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstRegistration, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRegistrationSearchListUser(string SearchValue, int Page = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            List<PPGSLARegistrationList> LstRegistration = new List<PPGSLARegistrationList>();
            PPGSLARepository ObjER = new PPGSLARepository();
            ObjER.GetAllRegistrationSearch(((Login)Session["LoginUser"]).Uid, SearchValue, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGSLARegistrationList>)ObjER.DBResponse.Data;
            }
            return PartialView("SLARegistrationListUser", LstRegistration);
        }

        [HttpGet]
        public ActionResult OpenSLARegistrationUser(int TicketId)
        {

            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGSLARegistrationList ObjSLA = new PPGSLARegistrationList();

            if (TicketId > 0)
            {
                ObjCR.GetSLARegistrationDetail(TicketId, 0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGSLARegistrationList)ObjCR.DBResponse.Data;
                }
            }            
            return PartialView("ViewSLARegistrationUser", ObjSLA);
        }

        [HttpPost]
        public ContentResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath("~/Content/SLA_Files/");

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path + fileName);

            //Send the File to Download.
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

            return Content(base64);
        }

        #endregion

        #region SLA Report
        [HttpGet]
        public ActionResult SlaReportExcel()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult SLAReportExcel(FormCollection fc)
        {

            PPGSLAReport ObjSla = new PPGSLAReport();
            ObjSla.Year = (fc["Year"].ToString());
            ObjSla.Quarter = (fc["Quarter"].ToString());
            PPGSLARepository ObjRR = new PPGSLARepository();
            ObjRR.SLAReportEXCEL(ObjSla);
            string Path = "";
            string excelName = "";
            excelName = "SLAReportDetail" + ".xls";
            if (ObjRR.DBResponse.Data == null)
                ObjRR.DBResponse.Data = string.Empty;

            if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            else
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }

        #endregion

        #region DownTime SITL

        [HttpGet]
        public ActionResult UnplannedDownTimeSitl()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            ViewBag.UserName = ObjLogin.LoginId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDTSitl(PPGDownTime ObjDowntime)
        {
            if (ModelState.IsValid)
            {                
                PPGSLARepository ObjER = new PPGSLARepository();
                ObjDowntime.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditDTSitl(ObjDowntime);

                //Send Mail

                //string ROn, DTReason, StTime, EndTime = "";
                //string Subject = "";
                //string TicketNo = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data).ToString(); ;
                ////TicketNo = TicketNo.Substring(1, TicketNo.Length - 2);
                //Subject = "Downtime Notice on dated " + ObjDowntime.RaisedOn + " " + "by " + BranchName + " Ticket Number " + TicketNo;
                ////TicketNo = ObjCR.DBResponse.Data.;            

                SendMailDTSitl(ObjDowntime);


                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrMsg };
                return Json(Err);
            }
        }

        public void SendMailDTSitl(PPGDownTime ObjDowntime)
        {            
            string subject = "";
            //TicketNo = TicketNo.Substring(1, TicketNo.Length - 2);
            subject = "Re: Downtime Notice on dated " + ObjDowntime.RaisedOn + " " + "by " + BranchName + " Ticket Number " + ObjDowntime.TicketNo;
            //TicketNo = ObjCR.DBResponse.Data.; 

            log.Info("Send Mail Method Started ");

            Login ObjLogin = (Login)Session["LoginUser"];
            var TicketNo = ObjDowntime.TicketNo.Trim('"');

            var message = new MailMessage();
            string to = "cwc.helpdesk@score.co.in";
            message.To.Add(new MailAddress(to));


            if (BranchName == "WhiteField")
            {
                string to1 = "cfswfd.cwc@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Patparganj")
            {
                string to1 = "cwcicd-ppg@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Dashrath")
            {
                string to1 = "icd.dashrath@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "HYDERABAD")
            {
                string to1 = "cfs.kukatpally@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Kandla")
            {
                string to1 = "cwccfs-kpt@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Kolkata")
            {
                string to1 = "cfskol@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Ambad")
            {
                string to1 = "cwcambad.2@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            log.Info("To: " + to);

            string bcc = "vineet.kumar@score.co.in";
            message.Bcc.Add(new MailAddress(bcc));

            log.Info("bcc: " + bcc);

            message.Subject = subject;
            string html = "";
            html += "Dear User, <br/><br/>";
            html += "Re: Status of Downtime has been updated whose details are given below. <br/><br/>";
            html += "Ticket No : " + TicketNo + "<br/><br/>";
            html += "Ticket Date : " + ObjDowntime.RaisedOn + "<br/><br/>";
            html += "Updated By : " + ObjLogin.LoginId + "<br/><br/>";
            html += "CFS/ICD : " + BranchName + "<br/><br/>";            
            html += "Start Date Time : " + ObjDowntime.StartDateTime + "<br/><br/>";
            html += "Completion Date Time : " + ObjDowntime.CompletionDateTime + "<br/><br/>";
            html += "Completion Status : " + ObjDowntime.CompletionStatus + "<br/><br/>";
            html += "Remarks SITL : " + ObjDowntime.RemarksSitl + "<br/><br/>";
            html += "Regards, <br/><br/>";
            html += "Help Desk Team <br/><br/>";
            html += "WWW.CWC-CFS.COM";

            message.Body = html;
            message.IsBodyHtml = true;

            log.Info("Subject : " + subject);
            log.Info("html : " + html);

            string FromMail = Session["EmailFrom"].ToString();
            string FromMailPassword = Session["EmailPwd"].ToString();
            string SmtpHost = Session["SmtpHost"].ToString();
            int SmtpPort = Convert.ToInt32(Session["SmtpPort"]);

            log.Info("FromMail : " + FromMail);
            log.Info("FromMailPassword : " + FromMailPassword);
            log.Info("SmtpHost : " + SmtpHost);
            log.Info("SmtpPort : " + SmtpPort);

            message.From = new MailAddress(FromMail);

            using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromMail, FromMailPassword);

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    log.Info("Exception Message : " + ex.Message);
                    log.Info("Exception StackTrace: " + ex.StackTrace);
                    log.Info("Exception Inner Exception: " + ex.InnerException);
                }
            }            
        }

        [HttpGet]
        public ActionResult GetDTSitlRegistrationList()
        {
            PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.GetAllDTSitlRegistration(0,0);

            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjCR.DBResponse.Data;
            }
            return PartialView("DTSitlRegistrationList", LstRegistration);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataDTSitlRegistration(int Page)
        {            
            PPGSLARepository ObjCR = new PPGSLARepository();
            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            ObjCR.GetAllDTSitlRegistration(0,Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjCR.DBResponse.Data;
            }
            return Json(LstRegistration, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDTSitlRegistrationSearchList(string SearchValue, int Page = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            PPGSLARepository ObjER = new PPGSLARepository();
            ObjER.GetAllDTSitlRegistrationSearch(SearchValue, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjER.DBResponse.Data;
            }
            return PartialView("DTSitlRegistrationList", LstRegistration);
        }

        [HttpGet]
        public ActionResult ViewDTSitlRegistration(int TicketId)
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGDownTime ObjSLA = new PPGDownTime();
            if (TicketId > 0)
            {
                ObjCR.GetViewDTSitlRegistration(TicketId, 0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGDownTime)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewDTSitlRegistration", ObjSLA);
        }

        [HttpGet]
        public ActionResult EditDTSitlRegistration(int TicketId)
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGDownTime ObjSLA = new PPGDownTime();
            if (TicketId > 0)
            {
                ObjCR.GetViewDTSitlRegistration(TicketId, 0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGDownTime)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("UnplannedDownTimeSitl", ObjSLA);
        }

        #endregion

        #region DOwnTime CWC

        [HttpGet]
        public ActionResult UnplannedDownTimeCWC()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            ViewBag.UserName = ObjLogin.LoginId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDTCwc(PPGDownTime ObjDowntime)
        {
            if (ModelState.IsValid)
            {
                PPGSLARepository ObjER = new PPGSLARepository();
                ObjDowntime.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditDTCwc(ObjDowntime);

                //Send Mail
                
                string Subject = "";
                string TicketNo = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data).ToString(); ;
                //TicketNo = TicketNo.Substring(1, TicketNo.Length - 2);
                Subject = "Downtime Notice on dated " + ObjDowntime.RaisedOn + " " + "by " + BranchName + " Ticket Number " + TicketNo;
                //TicketNo = ObjCR.DBResponse.Data.;            

                SendMailDTCwc(Subject, ObjDowntime.RaisedOn, ObjDowntime.RemarksCwc, ObjDowntime.StartDateTime, ObjDowntime.CompletionDateTime, TicketNo);


                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrMsg };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetDTCwcRegistrationList()
        {
            PPGSLARepository ObjCR = new PPGSLARepository();
            ObjCR.GetAllDTCwcRegistration(0, 0);

            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjCR.DBResponse.Data;
            }
            return PartialView("DTCwcRegistrationList", LstRegistration);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataDTCwcRegistration(int Page)
        {
            PPGSLARepository ObjCR = new PPGSLARepository();
            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            ObjCR.GetAllDTCwcRegistration(0, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjCR.DBResponse.Data;
            }
            return Json(LstRegistration, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDTCwcRegistrationSearchList(string SearchValue, int Page = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            List<PPGDownTime> LstRegistration = new List<PPGDownTime>();
            PPGSLARepository ObjER = new PPGSLARepository();
            ObjER.GetAllDTCwcRegistrationSearch(SearchValue, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstRegistration = (List<PPGDownTime>)ObjER.DBResponse.Data;
            }
            return PartialView("DTCwcRegistrationList", LstRegistration);
        }

        [HttpGet]
        public ActionResult ViewDTCwcRegistration(int TicketId)
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            PPGSLARepository ObjCR = new PPGSLARepository();
            PPGDownTime ObjSLA = new PPGDownTime();
            if (TicketId > 0)
            {
                ObjCR.GetViewDTCwcRegistration(TicketId, 0);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjSLA = (PPGDownTime)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewDTCwcRegistration", ObjSLA);
        }


        private void SendMailDTCwc(string subject, string ROn, string Remarks, string StartDateTime, string EndDateTime, string TicketNo)
        {

            log.Info("Send Mail Method Started ");

            Login ObjLogin = (Login)Session["LoginUser"];

            TicketNo = TicketNo.Trim('"');

            var message = new MailMessage();
            string to = "cwc.helpdesk@score.co.in";
            message.To.Add(new MailAddress(to));


            if (BranchName == "WhiteField")
            {
                string to1 = "cfswfd.cwc@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Patparganj")
            {
                string to1 = "cwcicd-ppg@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Dashrath")
            {
                string to1 = "icd.dashrath@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "HYDERABAD")
            {
                string to1 = "cfs.kukatpally@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }
            if (BranchName == "Kandla")
            {
                string to1 = "cwccfs-kpt@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Kolkata")
            {
                string to1 = "cfskol@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            if (BranchName == "Ambad")
            {
                string to1 = "cwcambad.2@cewacor.nic.in";
                message.To.Add(new MailAddress(to1));
            }

            log.Info("To: " + to);

            string bcc = "vineet.kumar@score.co.in";
            message.Bcc.Add(new MailAddress(bcc));

            log.Info("bcc: " + bcc);

            message.Subject = subject;
            string html = "";
            html = "Ticket No : " + TicketNo + "<br/><br/>";
            html += "Ticket Date : " + ROn + "<br/><br/>";
            html += "Raised By : " + ObjLogin.LoginId + "<br/><br/>";
            html += "CFS/ICD : " + BranchName + "<br/><br/>";
            html += "Reason : " + Remarks + "<br/><br/>";
            html += "Start Date Time : " + StartDateTime + "<br/><br/>";
            //html += "End Date Time : " + EndDateTime + "<br/><br/>";


            message.Body = html;
            message.IsBodyHtml = true;

            log.Info("Subject : " + subject);
            log.Info("html : " + html);

            string FromMail = Session["EmailFrom"].ToString();
            string FromMailPassword = Session["EmailPwd"].ToString();
            string SmtpHost = Session["SmtpHost"].ToString();
            int SmtpPort = Convert.ToInt32(Session["SmtpPort"]);

            log.Info("FromMail : " + FromMail);
            log.Info("FromMailPassword : " + FromMailPassword);
            log.Info("SmtpHost : " + SmtpHost);
            log.Info("SmtpPort : " + SmtpPort);

            message.From = new MailAddress(FromMail);

            using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromMail, FromMailPassword);

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    log.Info("Exception Message : " + ex.Message);
                    log.Info("Exception StackTrace: " + ex.StackTrace);
                    log.Info("Exception Inner Exception: " + ex.InnerException);
                }
            }
        }
        #endregion

        #region SLA Report DownTime
        [HttpGet]
        public ActionResult SlaReportUPDTExcel()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult SLAReportUPDTExcel(FormCollection fc)
        {

            PPGSLAReportDT ObjSla = new PPGSLAReportDT();
            ObjSla.Year = (fc["Year"].ToString());
            ObjSla.Quarter = (fc["Quarter"].ToString());
            PPGSLARepository ObjRR = new PPGSLARepository();
            ObjRR.SLAReportDTEXCEL(ObjSla);
            string Path = "";
            string excelName = "";
            excelName = "SLAReportDetail" + ".xls";
            if (ObjRR.DBResponse.Data == null)
                ObjRR.DBResponse.Data = string.Empty;

            if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            else
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }

        #endregion
    }
}


