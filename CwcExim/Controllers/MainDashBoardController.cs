using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Configuration;

namespace CwcExim.Controllers
{
    public class MainDashBoardController : BaseController
    {
        // GET: MainDashBoard
        public ActionResult Index(int BranchId)
        {
            string urlReferrer = Request.UrlReferrer.ToString();
            string mainDomainUrl = ConfigurationManager.AppSettings["MainDomainUrl"].ToString();
            Session["BranchId"] = BranchId;
            if (Session["LoginUser"] != null)
            {
                Session.Abandon();
                return Redirect(mainDomainUrl);
            }
            else
            {
                try
                {

                    if (mainDomainUrl.Split(new char[] { '/' })[2] == urlReferrer.Split(new char[] { '/' })[2])
                    {
                        SaveSessionData(BranchId);

                        return View("MainDashBoard");
                    }
                    else
                    {
                        return Redirect(mainDomainUrl);
                    }
                }
                catch (Exception ex)
                {
                    return Redirect(mainDomainUrl);
                }
            }
        }
        [NonAction]
        private void SaveSessionData(int BranchId)
        {
            var Config = Server.MapPath("~/Content/CwcConfig.xml");
            XDocument doc = XDocument.Load(Config);
            var objBranch = from r in doc.Descendants("Branch")
                            select new
                            {
                                BranchId = r.Element("BranchId").Value,
                                BranchType = r.Element("BranchType").Value,
                                Name = r.Element("Name").Value,
                                ConnectionString = r.Element("ConnectionString").Value,
                               // DRConnectionString = r.Element("DRConnectionString").Value,
                                EmailFrom = r.Element("EmailFrom").Value,
                                EmailPwd = r.Element("EmailPwd").Value,
                                SmtpHost = r.Element("SmtpHost").Value,
                                SmtpPort = r.Element("SmtpPort").Value,
                                GateEntryMailSent = r.Element("GateEntryMailSent").Value,
                                GateExitMailSent = r.Element("GateExitMailSent").Value,
                                FtpServerIP = r.Element("ResourceUrl").Value,
                                FtpUid = r.Element("FtpUid").Value,
                                FtpPwd = r.Element("FtpPwd").Value,
                                PortNo = r.Element("SFTPPortNo").Value
                                
                            };
            foreach (var item in objBranch)
            {
                if (Convert.ToInt32(item.BranchId) == BranchId)
                {
                    Session["BranchId"] = item.BranchId;
                    Session["BranchType"] = item.BranchType;
                    Session["Name"] = item.Name;
                    Session["ConnectionString"] = item.ConnectionString;
                  //  Session["DRConnectionString"] = item.DRConnectionString;
                    Session["EmailFrom"] = item.EmailFrom;
                    Session["EmailPwd"] = item.EmailPwd;
                    Session["SmtpHost"] = item.SmtpHost;
                    Session["SmtpPort"] = item.SmtpPort;
                    Session["GateEntryMailSent"] = item.GateEntryMailSent;
                    Session["GateExitMailSent"] = item.GateExitMailSent;
                    Session["FtpServerIP"] = item.FtpServerIP;
                    Session["FtpUid"] = item.FtpUid;
                    Session["FtpPwd"] = item.FtpPwd;
                    Session["PortNo"] = item.PortNo;
                   
                    break;
                }
            }
        }
        public ActionResult MainLandingPage()
        {

            return View("MainLandingPage");
        }

        [HttpGet]
        public ActionResult CheckSession()
        {
            int SessionStatus = 0;
            if (Session["LoginUser"] != null)
            {
                SessionStatus = 1;
            }
            var Data = new { Status = SessionStatus };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        //Add New On 21st JAN 2019

        [HttpGet]
        public JsonResult LoadCompany()
        {
            MainDashBoardRepository ObjCR = new MainDashBoardRepository();
            Company Company = new Company();
            ObjCR.GetCompanyInfo();
            if (ObjCR.DBResponse.Data != null)
            {
                Company = (Company)ObjCR.DBResponse.Data;
            }
            return Json(Company, JsonRequestBehavior.AllowGet);
            //return PartialView("LoadCompany", LstCompany);
        }
        //Add New On 21st JAN 2019


    }
}