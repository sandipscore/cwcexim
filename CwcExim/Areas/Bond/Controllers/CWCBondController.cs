using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Areas.Bond.Models;
using CwcExim.Filters;
using CwcExim.Models;
using System.IO;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;

namespace CwcExim.Areas.Bond.Controllers
{
    public class CWCBondController : BaseController
    {
        #region Space Availability Certificate

        [HttpGet]
        public ActionResult GetSpaceAvailList()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetSpaceAvailCert(1, 0);
            if (ObjBR.DBResponse.Data != null)
            {
                var JObject = Newtonsoft.Json.JsonConvert.SerializeObject(ObjBR.DBResponse.Data);
                var JObj = Newtonsoft.Json.Linq.JObject.Parse(JObject);
                ViewBag.SpaceAvailList = JObj["List"];
            }
            else
            {
                ViewBag.SpaceAvailList = null;
            }
            return PartialView("SpaceAvailList");
        }

        [HttpGet]
        public ActionResult LoadSpaceAvailCert(int SpaceappId)
        {
            BondRepository ObjBR = new BondRepository();
            SpaceAvailableCert ObjSpaceAvail = new SpaceAvailableCert();
            ObjBR.GetSpaceAvailCertById(SpaceappId);
            if (ObjBR.DBResponse.Data != null)
            {
                ObjSpaceAvail = (SpaceAvailableCert)ObjBR.DBResponse.Data;
            }
            return PartialView("SpaceAvailCert", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult LoadMoredata(int Status, int Skip)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetSpaceAvailCert(Status, Skip);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSpaceAvailCert(SpaceAvailableCert ObjSpaceAvail)
        {
            if (ModelState.IsValidField("SacNo") && ModelState.IsValidField("SacDate") && ModelState.IsValidField("AreaReserved") && ModelState.IsValidField("ValidUpto"))
            {
                ObjSpaceAvail.ApprovedBy = ((Login)Session["LoginUser"]).Uid;
                BondRepository ObjBR = new BondRepository();
                ObjBR.AddEditBondSpaceAvailCert(ObjSpaceAvail);
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult PrintSpaceAvailCert(int SpaceappId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetSpaceAvailCertForPrint(SpaceappId);
            if (ObjBR.DBResponse.Data != null)
            {
                SpaceAvailCertPdf ObjSpaceAvail = new SpaceAvailCertPdf();
                ObjSpaceAvail = (SpaceAvailCertPdf)ObjBR.DBResponse.Data;
                string Path = GeneratePDFForSpaceAvailCert(ObjSpaceAvail, SpaceappId);
                return Json(new { Status = 1, Message = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GeneratePDFForSpaceAvailCert(SpaceAvailCertPdf ObjSpaceAvail, int SpaceappId)
        {
            string Html = "";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.Importer + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOENoDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.ValidUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
        }

        #endregion

        #region Application For Space Availability Certificate

        [HttpGet]
        public ActionResult CreateBondApp()
        {
            BondRepository objBR = new BondRepository();
            BondApp objBA = new BondApp();
            ViewData["cha"] = ((Login)Session["LoginUser"]).CHA;
            if (Convert.ToBoolean(ViewData["cha"]) == false)
            {
                objBR.ListOfCHA();
                if (objBR.DBResponse.Data != null)
                    ViewBag.ListOfCHA = objBR.DBResponse.Data;
                else ViewBag.ListOfCHA = null;
            }
            else
            {
                objBA.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                objBA.CHAName = ((Login)Session["LoginUser"]).Name;
            }
            ViewData["importer"] = ((Login)Session["LoginUser"]).Importer;
            if (Convert.ToBoolean(ViewData["importer"]) == false)
            {
                objBR.ListOfImporter();
                if (objBR.DBResponse.Data != null)
                    ViewBag.ListOfImporter = objBR.DBResponse.Data;
                else ViewBag.ListOfImporter = null;
            }
            else
            {
                objBA.ImporterId = ((Login)Session["LoginUser"]).EximTraderId;
                objBA.ImporterName = ((Login)Session["LoginUser"]).Name;
            }
            objBA.ApplicationDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult EditBondApp(int SpaceappId)
        {
            BondRepository objBR = new BondRepository();
            BondApp objBA = new BondApp();
            ViewData["cha"] = ((Login)Session["LoginUser"]).CHA;
            if (Convert.ToBoolean(ViewData["cha"]) == false)
            {
                objBR.ListOfCHA();
                if (objBR.DBResponse.Data != null)
                    ViewBag.ListOfCHA = objBR.DBResponse.Data;
                else ViewBag.ListOfCHA = null;
            }
            else
            {
                objBA.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                objBA.CHAName = ((Login)Session["LoginUser"]).Name;
            }
            ViewData["importer"] = ((Login)Session["LoginUser"]).Importer;
            if (Convert.ToBoolean(ViewData["importer"]) == false)
            {
                objBR.ListOfImporter();
                if (objBR.DBResponse.Data != null)
                    ViewBag.ListOfImporter = objBR.DBResponse.Data;
                else ViewBag.ListOfImporter = null;
            }
            else
            {
                objBA.ImporterId = ((Login)Session["LoginUser"]).EximTraderId;
                objBA.ImporterName = ((Login)Session["LoginUser"]).Name;
            }
            objBR.SpaceAvailabilityDetails(SpaceappId);
            if (objBR.DBResponse.Data != null)
                objBA = (BondApp)objBR.DBResponse.Data;
            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult ViewBondApp(int SpaceappId)
        {
            BondRepository objBR = new BondRepository();
            BondApp objBA = new BondApp();
            objBR.SpaceAvailabilityDetails(SpaceappId);
            if (objBR.DBResponse.Data != null)
                objBA = (BondApp)objBR.DBResponse.Data;
            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult ListOfBondApp()
        {
            BondRepository objBR = new BondRepository();
            IList<ListOfBondApp> lstApp = new List<ListOfBondApp>();
            objBR.ListOfSpaceAvailability();
            if (objBR.DBResponse.Data != null)
                lstApp = (List<ListOfBondApp>)objBR.DBResponse.Data;
            return PartialView(lstApp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteBondApp(int SpaceappId)
        {
            BondRepository objBR = new BondRepository();
            objBR.DeleteBondApp(SpaceappId);
            return Json(objBR.DBResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondApp(BondApp objBA)
        {
            if (ModelState.IsValid)
            {
                BondRepository objBR = new BondRepository();
                objBR.AddEditBondApp(objBA);
                return Json(objBR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion

        #region Space Availability Certificate (Extend)

        [HttpGet]
        public ActionResult CreateSpaceAvailCertExtend()
        {
            SpaceAvailCertExtend ObjSpaceAvail = new SpaceAvailCertExtend();
            ObjSpaceAvail.ExtendOn = DateTime.Now.ToString("dd/MM/yyyy");
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetSacNo();
            if (ObjBR.DBResponse.Data != null)
            {
                ViewBag.SacNoList = new SelectList((List<SpaceAvailableCert>)ObjBR.DBResponse.Data, "SpaceappId", "SacNo");
            }
            else
            {
                ViewBag.SacNoList = null;
            }
            return PartialView("CreateSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult EditSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            SpaceAvailCertExtend ObjSpaceAvail = new SpaceAvailCertExtend();
            if (SpaceAvailCertExtId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.GetSpaceAvailCertExt(SpaceAvailCertExtId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjSpaceAvail = (SpaceAvailCertExtend)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("EditSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult ViewSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            SpaceAvailCertExtend ObjSpaceAvail = new SpaceAvailCertExtend();
            if (SpaceAvailCertExtId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.GetSpaceAvailCertExt(SpaceAvailCertExtId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjSpaceAvail = (SpaceAvailCertExtend)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult GetSacNoDetails(int SpaceappId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetSacNoDet(SpaceappId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSpaceAvailCertExtendList()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetAllSpaceAvailCertExt();
            List<SpaceAvailCertExtend> LstSpaceAvail = new List<SpaceAvailCertExtend>();
            if (ObjBR.DBResponse.Data != null)
            {
                LstSpaceAvail = (List<SpaceAvailCertExtend>)ObjBR.DBResponse.Data;
            }
            return PartialView("SpaceAvailCertExtendList", LstSpaceAvail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSpaceAvailCertExtend(SpaceAvailCertExtend ObjSpaceAvail)
        {
            if (ModelState.IsValidField("AreaReserved") && ModelState.IsValidField("ExtendUpto") && ModelState.IsValidField("ExtendOn"))
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.AddEditBondSpaceAvailCertExt(ObjSpaceAvail);
                ModelState.Clear();
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DelSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.DelBondSpaceAvailCertExt(SpaceAvailCertExtId);
            return Json(ObjBR.DBResponse);
        }


        #endregion

        #region  Approval for Space Availability Criteria (Extend)
        [HttpGet]
        public ActionResult AppSpcAvailExtend()
        {
            BondRepository objBR = new BondRepository();
            objBR.ListOfSpaceAvailabilityExt("N", 0);
            if (objBR.DBResponse.Data != null)
            {
                var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objBR.DBResponse.Data);
                var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                ViewBag.Pending = jobject["List"];
            }
            else
                ViewBag.Pending = null;
            return PartialView("AppSpcAvailExtend");
        }
        [HttpGet]
        public JsonResult LoadMoreSpcAvailExtend(string Status, int Skip)
        {
            BondRepository objBR = new BondRepository();
            objBR.ListOfSpaceAvailabilityExt(Status, Skip);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSpaceAvailabilityExt(int SpaceAvailCertExtId)
        {
            BondRepository objBR = new BondRepository();
            BondAppExtendDetails objBD = new BondAppExtendDetails();
            objBR.GetSpaceAvailabilityExt(SpaceAvailCertExtId);
            if (objBR.DBResponse.Data != null)
                objBD = (BondAppExtendDetails)objBR.DBResponse.Data;
            return PartialView(objBD);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintExtendDetails(int SpaceAvailCertExtId)
        {
            BondRepository objBR = new BondRepository();
            PrintSACExt objSAC = new PrintSACExt();
            objBR.PrintSACExt(SpaceAvailCertExtId, Convert.ToInt32(Session["BranchId"]));
            if (objBR.DBResponse.Data != null)
            {
                objSAC = (PrintSACExt)objBR.DBResponse.Data;
                string path = GenerateExtSAC(objSAC, SpaceAvailCertExtId);
                return Json(new { Status = 1, Message = path });
            }
            return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GenerateExtSAC(PrintSACExt objSAC, int SpaceAvailCertExtId)
        {
            string Html = "";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailCertExt" + SpaceAvailCertExtId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate(Extension)<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.ImporterName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.BOE + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.ExtendUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailCertExt" + SpaceAvailCertExtId + ".pdf";
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult UpdateAppExtend(FormCollection fc)
        {
            BondRepository objBR = new BondRepository();
            objBR.UpdateAppExtend(Convert.ToInt32(fc["SpaceAvailCertExtId"]), Convert.ToInt32(fc["IsApproved"]));
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Deposit Application
        [HttpGet]
        public ActionResult CreateDepositApp()
        {
            DepositApp ObjDepositApp = new DepositApp();
            ObjDepositApp.DepositDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjDepositApp.WRDate = DateTime.Now.ToString("dd/MM/yyyy");
            BondRepository ObjBR = new BondRepository();
            ImportRepository ObjIR = new ImportRepository();
            ObjBR.GetSacNoForDepositApp();
            if (ObjBR.DBResponse.Data != null)
            {
                ViewBag.SacNoList = new SelectList((List<DepositApp>)ObjBR.DBResponse.Data, "SpaceappId", "SacNo");
            }
            else
            {
                ViewBag.SacNoList = null;
            }
            ObjBR.ListOfImporter();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
            else ViewBag.ListOfImporter = null;

            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            else
                ViewBag.GodownList = null;
            return PartialView("CreateDepositApp", ObjDepositApp);
        }

        [HttpGet]
        public ActionResult ViewDepositApp(int DepositAppId)
        {
            DepositApp ObjDepositApp = new DepositApp();
            ImportRepository ObjIR = new ImportRepository();
            if (DepositAppId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.GetDepositApp(DepositAppId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjDepositApp = (DepositApp)ObjBR.DBResponse.Data;
                    ObjBR.ListOfImporter();
                    if (ObjBR.DBResponse.Data != null)
                        ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
                    else ViewBag.ListOfImporter = null;

                    ObjIR.ListOfGodown();
                    if (ObjIR.DBResponse.Data != null)
                        ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                    else
                        ViewBag.GodownList = null;
                }
            }
            return PartialView("ViewDepositApp", ObjDepositApp);
        }

        [HttpGet]
        public ActionResult EditDepositApp(int DepositAppId)
        {
            DepositApp ObjDepositApp = new DepositApp();
            ImportRepository ObjIR = new ImportRepository();
            if (DepositAppId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.GetDepositApp(DepositAppId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjDepositApp = (DepositApp)ObjBR.DBResponse.Data;
                    ObjBR.ListOfImporter();
                    if (ObjBR.DBResponse.Data != null)
                        ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
                    else ViewBag.ListOfImporter = null;

                    ObjIR.ListOfGodown();
                    if (ObjIR.DBResponse.Data != null)
                        ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                    else
                        ViewBag.GodownList = null;
                }
            }
            return PartialView("EditDepositApp", ObjDepositApp);
        }

        //[HttpGet]
        //public ActionResult GetSacNoDetailForDepositApp(int SpaceappId)
        //{
        //    BondRepository ObjBR = new BondRepository();
        //   // ObjBR.GetSacNoDetForDepositApp(SpaceappId);
        //    return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetDepositAppList()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetAllDepositApp();
            List<DepositApp> LstDepositApp = new List<DepositApp>();
            if (ObjBR.DBResponse.Data != null)
            {
                LstDepositApp = (List<DepositApp>)ObjBR.DBResponse.Data;
            }
            return PartialView("DepositAppList", LstDepositApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditDepositApp(DepositApp ObjDepositApp)
        {
            if (ModelState.IsValid)
            {
                BondRepository ObjBR = new BondRepository();
                ObjBR.AddEditBondDepositApp(ObjDepositApp);
                ModelState.Clear();
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDepositeApp(int DepositAppId)
        {
            if (DepositAppId > 0)
            {
                BondRepository objBR = new BondRepository();
                PrintDA objDA = new PrintDA();
                objBR.PrintDA(DepositAppId);
                if (objBR.DBResponse.Data != null)
                {
                    objDA = (PrintDA)objBR.DBResponse.Data;
                    string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='4' style='padding-bottom:25px;text-align:center;font-size:12pt;'>Deposit Application</th></tr></thead><tbody><tr><th style='text-align:left;padding:5px;'>Deposit No:</th><td style='text-align:left;padding:5px;'>" + objDA.DepositeNo + "</td><th style='text-align:left;padding:5px;'>Deposit Date:</th><td style='text-align:left;padding:5px;'>" + objDA.DepositeDate + "</td></tr><tr><th style='text-align:left;padding:5px;'>Bond No:</th><td style='text-align:left;padding:5px;'>" + objDA.BondNo + "</td><th style='text-align:left;padding:5px;'>Bond Date:</th><td style='text-align:left;padding:5px;'>" + objDA.BondDt + "</td></tr><tr><th style='text-align:left;padding:5px;'>WR No:</th><td style='text-align:left;padding:5px;'>" + objDA.WRNo + "</td><th style='text-align:left;padding:5px;'>WR Date:</th><td style='text-align:left;padding:5px;'>" + objDA.WRDt + "</td></tr><tr><th style='text-align:left;padding:5px;'>In Bond BOE No:</th> <td style='text-align:left;padding:5px;'>" + objDA.BondBOENo + "</td> <th style='text-align:left;padding:5px;'>In Bond BOE Date:</th><td style='text-align:left;padding:5px;'>" + objDA.BondBOEDate + "</td></tr><tr><th style='text-align:left;padding:5px;'>Godown:</th><td style='text-align:left;padding:5px;' colspan='3'>" + objDA.GodownName + "</td></tr></tbody></table>";
                    html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>SAC No</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>SAC Date</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Name of Importer</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>CHA</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Cargo Description</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Units</th></tr></thead><tbody>";
                    objDA.lstSAC.ForEach(item =>
                    {
                        html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.SacNo + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.SacDate + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ImporterName + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CHAName + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.NoOfUnits + "</td></tr>";
                    });
                    html += "</tbody></table>";
                    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                    if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf"))
                        System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf");
                    using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                    {
                        rh.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf", html);
                    }
                    return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf" });
                }
                else return Json(new { Status = -1, Message = "Error" });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        #endregion

        #region Work Order For Unloading & Delivery
        [HttpGet]
        public ActionResult CreateWODeli()
        {
            BondRepository objBR = new BondRepository();
            objBR.GetBondNoForWODelivery();
            if (objBR.DBResponse.Data != null)
                ViewBag.BondNo = objBR.DBResponse.Data;
            else
                ViewBag.BondNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpGet]
        public ActionResult EditWODeli(int BondWOId)
        {
            BondRepository objBR = new BondRepository();
            BondWODeli objBond = new BondWODeli();
            objBR.ListOfWODeliDetails(BondWOId);
            if (objBR.DBResponse.Data != null)
                objBond = (BondWODeli)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ViewWODeli(int BondWOId)
        {
            BondRepository objBR = new BondRepository();
            BondWODeli objBond = new BondWODeli();
            objBR.ListOfWODeliDetails(BondWOId);
            if (objBR.DBResponse.Data != null)
                objBond = (BondWODeli)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ListOfWODeli()
        {
            BondRepository objBR = new BondRepository();
            List<ListOfBondWODeli> objList = new List<ListOfBondWODeli>();
            objBR.ListOfWODeli();
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfBondWODeli>)objBR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditWODeli(FormCollection fc)
        {
            BondRepository objBR = new BondRepository();
            objBR.AddEditWODeliDetails(Convert.ToInt32(fc["BondWOId"]), Convert.ToInt32(fc["DepositAppId"]), fc["WorkOrderFor"].ToString(), fc["WorkOrderDate"].ToString(), fc["DeliveryNo"].ToString(), Convert.ToInt32(fc["CargoUnits"]), Convert.ToDecimal(fc["CargoWeight"]));
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDetailsAgainstBondNo(int DepositAppId)
        {
            BondRepository objBR = new BondRepository();
            objBR.GetDetailsforBondNo(DepositAppId);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Unloading at Bonded Warehouse
        [HttpGet]
        public ActionResult CreateWOUnloading()
        {
            BondRepository objBR = new BondRepository();
            objBR.GetBondNoForWOUnloading();
            if (objBR.DBResponse.Data != null)
                ViewBag.WorkOrderNo = objBR.DBResponse.Data;
            else
                ViewBag.WorkOrderNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

            objBR.GetCFSCodeForWOUnloading();
            if (objBR.DBResponse.Data != null)
                ViewBag.CFSCodeList = JsonConvert.SerializeObject(objBR.DBResponse.Data);
            else
                ViewBag.CFSCodeList = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult EditWOUnloading(int UnloadingId)
        {
            BondRepository objBR = new BondRepository();
            BondWOUnloading objBond = new BondWOUnloading();
            objBR.WOUnloadingDetails(UnloadingId, "EDIT");
            if (objBR.DBResponse.Data != null)
            {
                var jobj = JsonConvert.SerializeObject(objBR.DBResponse.Data);
                var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                ViewBag.Godown = jobject["lstGodown"];
                objBond = JsonConvert.DeserializeObject<BondWOUnloading>(JsonConvert.SerializeObject(jobject["objBond"]));
            }

            objBR.GetCFSCodeForWOUnloading();
            if (objBR.DBResponse.Data != null)
                ViewBag.CFSCodeList = JsonConvert.SerializeObject(objBR.DBResponse.Data);
            else
                ViewBag.CFSCodeList = null;

            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ViewWOUnloading(int UnloadingId)
        {
            BondRepository objBR = new BondRepository();
            BondWOUnloading objBond = new BondWOUnloading();
            objBR.WOUnloadingDetails(UnloadingId);
            if (objBR.DBResponse.Data != null)
                objBond = (BondWOUnloading)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ListOfWOUnloading()
        {
            BondRepository objBR = new BondRepository();
            List<ListOfWOunloading> objList = new List<ListOfWOunloading>();
            objBR.ListForWOUnloading();
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWOunloading>)objBR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditWOUnloading(BondWOUnloading obj)
        {
            if (ModelState.IsValid)
            {
                obj.CfsCodeList = JsonConvert.DeserializeObject<List<BondUnloadingCFSCode>>(obj.CfsCodes == null ? "" : obj.CfsCodes);
                string ContainerXML = "";
                if (obj.CfsCodeList != null)
                {
                    ContainerXML = Utility.CreateXML(obj.CfsCodeList);
                }

                BondRepository objBR = new BondRepository();
                objBR.AddEditWOUnloading(obj, ContainerXML);
                return Json(objBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                // return Json(new { Status = 1, Message = ErrorMessage });
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetDetailsfrUnloading(int DepositAppId)
        {
            BondRepository objBR = new BondRepository();
            objBR.GetBondNoForWOUnloadingDet(DepositAppId);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bond Advance Payment Sheet

        [HttpGet]
        public ActionResult CreateBondAdvPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForAdvBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondAdvPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName)
        {

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            var objGenericCharges = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");



            var objBondPostPaymentSheet = new BondPostPaymentSheet();
            objBondPostPaymentSheet.InvoiceType = InvoiceType;
            objBondPostPaymentSheet.InvoiceDate = InvoiceDate;
            objBondPostPaymentSheet.RequestId = StuffingReqId;
            objBondPostPaymentSheet.RequestNo = StuffingReqNo;
            objBondPostPaymentSheet.RequestDate = StuffingReqDate;
            objBondPostPaymentSheet.UptoDate = UptoDate;
            objBondPostPaymentSheet.Area = Area;
            objBondPostPaymentSheet.PartyId = PartyId;
            objBondPostPaymentSheet.PartyName = PartyName;
            objBondPostPaymentSheet.PartyAddress = PartyAddress;
            objBondPostPaymentSheet.PartyState = PartyState;
            objBondPostPaymentSheet.PartyStateCode = PartyStateCode;
            objBondPostPaymentSheet.PartyGST = PartyGST;
            objBondPostPaymentSheet.PayeeId = PayeeId;
            objBondPostPaymentSheet.PayeeName = PayeeName;
            objBondPostPaymentSheet.DeliveryType = DeliveryType;

            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            objBondPostPaymentSheet.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            objBondPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            objBondPostPaymentSheet.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            objBondPostPaymentSheet.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            objBondPostPaymentSheet.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            objBondPostPaymentSheet.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            objBondPostPaymentSheet.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            objBondPostPaymentSheet.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            objBondPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            objBondPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            objBondPostPaymentSheet.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            objBondPostPaymentSheet.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            objBondPostPaymentSheet.CompPAN = CompPAN;
            #endregion

            var GSTType = objBondPostPaymentSheet.PartyStateCode == CompStateCode || objBondPostPaymentSheet.PartyStateCode == "";
            if (SEZ == "SEZWP")
            {
                GSTType = false;
            }
            var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
            if (SEZ == "SEZWOP")
            {
                cgst = 0;
                sgst = 0;
                igst = 0;
            }


            var ActualStorage = 0M;
            var TotalDays = Convert.ToInt32((DateTime.ParseExact(objBondPostPaymentSheet.UptoDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) -
                            DateTime.ParseExact(objBondPostPaymentSheet.RequestDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
            var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / 7);

            ActualStorage = Math.Round(objBondPostPaymentSheet.Area * TotalWeeks * objGenericCharges.StorageRent.Where(o => o.WarehouseType == 3).FirstOrDefault().RateSqMPerWeek, 2);
            decimal PrevStorage = 0;
          //  BondRepository objBond = new BondRepository();
          //  objBond.GetPrevioudStorage(objBondPostPaymentSheet.RequestNo);
         //   if (objBond.DBResponse.Status > 0)
          //      PrevStorage = Convert.ToDecimal(objBond.DBResponse.Data);
         //   if (ActualStorage >= PrevStorage)
          //      ActualStorage = ActualStorage - PrevStorage;
         //   else
         //       ActualStorage = 0;
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 1,
                Clause = "4",
                ChargeName = "Storage Charges",
                ChargeType = "CWC",
                SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = ActualStorage,
                Discount = 0M,
                Taxable = ActualStorage,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                    (Math.Round(ActualStorage * (cgst / 100), 2)) +
                    (Math.Round(ActualStorage * (sgst / 100), 2))) :
                    (ActualStorage + (Math.Round(ActualStorage * (igst / 100), 2)))) : ActualStorage
            });


            objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
            objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
            objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
            objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
            objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
            objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
            objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

            //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
            //    InvoiceType);

            return Json(objBondPostPaymentSheet, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondAdvPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ExportRepository objExport = new ExportRepository();
                //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objExport.DBResponse);

                var invoiceData = JsonConvert.DeserializeObject<BondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

                BondRepository objChargeMaster = new BondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDadv", ExportUnder);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region  Delivery Order

        [HttpGet]
        public ActionResult CreateDeliveryOrder()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetWOForDeliveryOrder();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.SacNoList = ObjBR.DBResponse.Data;
            else
                ViewBag.SacNoList = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditDeliveryOrder(int DeliveryOrderId)
        {
            BondRepository ObjBR = new BondRepository();
            DeliveryOrder ObjDeliveryOrder = new DeliveryOrder();
            ObjBR.GetDeliveryOrder(DeliveryOrderId);
            if (ObjBR.DBResponse.Data != null)
            {
                ObjDeliveryOrder = (DeliveryOrder)ObjBR.DBResponse.Data;
                //var jobj = JsonConvert.SerializeObject(ObjBR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.Godown = jobject["lstGodown"];
                //objBond = JsonConvert.DeserializeObject<BondWOUnloading>(JsonConvert.SerializeObject(jobject["objBond"]));
            }
            return PartialView(ObjDeliveryOrder);
        }

        [HttpGet]
        public ActionResult ViewDeliveryOrder(int DeliveryOrderId)
        {
            BondRepository ObjBR = new BondRepository();
            DeliveryOrder ObjDeliveryOrder = new DeliveryOrder();
            ObjBR.GetDeliveryOrder(DeliveryOrderId);
            if (ObjBR.DBResponse.Data != null)
                ObjDeliveryOrder = (DeliveryOrder)ObjBR.DBResponse.Data;
            return PartialView(ObjDeliveryOrder);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryOrder()
        {
            BondRepository ObjBR = new BondRepository();
            List<ListOfDeliveryOrder> LstDeliveryOrder = new List<ListOfDeliveryOrder>();
            ObjBR.GetAllDeliveryOrder();
            if (ObjBR.DBResponse.Data != null)
                LstDeliveryOrder = (List<ListOfDeliveryOrder>)ObjBR.DBResponse.Data;
            return PartialView("DeliveryOrderList", LstDeliveryOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryOrder(DeliveryOrder ObjDeliveryOrder)
        {
            if (ModelState.IsValid)
            {
                BondRepository ObjBR = new BondRepository();
                string DeliveryOrderXml = "";
                if (ObjDeliveryOrder.DeliveryOrderXml != "")
                {
                    ObjDeliveryOrder.LstDeliveryOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeliveryOrderDtl>>(ObjDeliveryOrder.DeliveryOrderXml);
                    DeliveryOrderXml = Utility.CreateXML(ObjDeliveryOrder.LstDeliveryOrder);
                }
                ObjBR.AddEditDeliveryOrder(ObjDeliveryOrder, DeliveryOrderXml);
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetDetForDeliveryOrder(int SpaceAppId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetWODetForDeliveryOrder(SpaceAppId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepositNoForDelvOrder(int SpaceAppId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetDepositNoForDelvOrder(SpaceAppId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepositNoDetForDelvOrder(int DepositAppId)
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.GetDepositDetForDelvOrder(DepositAppId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Bond Delivery Payment Sheet

        [HttpGet]
        public ActionResult BondDeliPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForDelBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
            {
                var BondList = (IList<BondSacDetails>)objBond.DBResponse.Data;
                TempData["SACList"] = BondList;

                IList<BondSacDetails> DistinctSacList = new List<BondSacDetails>();
                BondList.ToList().ForEach(item =>
                {
                    if (!DistinctSacList.Any(o => o.DepositAppId == item.DepositAppId))
                    {
                        DistinctSacList.Add(item);
                    }
                });
                ViewBag.SACList = JsonConvert.SerializeObject(DistinctSacList);

            }
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondDelPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal weight, decimal CIFValue, decimal Duty,
            int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate,string Size, decimal MechanicalWeight = 0, decimal ManualWeight = 0)
        {

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();

            //-------------------------------------------------------------------------------------------------------------------------
            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForDelBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
            {
                var BondList = (IList<BondSacDetails>)objBond.DBResponse.Data;
                TempData["SACList"] = BondList;
            }
            //-------------------------------------------------------------------------------------------------------------------------


            string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = MechanicalWeight });
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = ManualWeight });

            IDataParameter[] DParam = { };
            DParam = LstParamAllNew.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            var objGenericCharges = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges_Bond", DParam);
            var objBondList = ((IList<BondSacDetails>)TempData.Peek("SACList")).Where(o => o.DepositAppId == StuffingReqId).ToList();

            var objBondPostPaymentSheet = new BondPostPaymentSheet();
            objBondPostPaymentSheet.InvoiceType = InvoiceType;
            objBondPostPaymentSheet.InvoiceDate = InvoiceDate;
            objBondPostPaymentSheet.RequestId = StuffingReqId;
            objBondPostPaymentSheet.RequestNo = StuffingReqNo;
            objBondPostPaymentSheet.RequestDate = StuffingReqDate;
            objBondPostPaymentSheet.UptoDate = UptoDate;
            objBondPostPaymentSheet.Area = Area;
            objBondPostPaymentSheet.PartyId = PartyId;
            objBondPostPaymentSheet.PartyName = PartyName;
            objBondPostPaymentSheet.PartyAddress = PartyAddress;
            objBondPostPaymentSheet.PartyState = PartyState;
            objBondPostPaymentSheet.PartyStateCode = PartyStateCode;
            objBondPostPaymentSheet.PartyGST = PartyGST;
            objBondPostPaymentSheet.PayeeId = PayeeId;
            objBondPostPaymentSheet.PayeeName = PayeeName;
            objBondPostPaymentSheet.DeliveryType = DeliveryType;
            objBondPostPaymentSheet.DepositDate = DepositDate;
            objBondPostPaymentSheet.BOEDate = BOEDate;
            objBondPostPaymentSheet.BOENo = BOENo;



            objBondPostPaymentSheet.CIFValue = objBondList.ToList().Sum(o => o.CIFValue);
            objBondPostPaymentSheet.Duty = objBondList.ToList().Sum(o => o.Duty);
            objBondPostPaymentSheet.Units = objBondList.ToList().Sum(o => o.Units);
            //objBondPostPaymentSheet.TotalGrossWt = objBondList.ToList().Sum();
            objBondPostPaymentSheet.TotalGrossWt = ManualWeight+ MechanicalWeight;
            objBondPostPaymentSheet.TotalNoOfPackages = objBondList.ToList().Sum(o => o.Units);
            //objBondPostPaymentSheet.TotalWtPerUnit = (objBondList.ToList().Sum(o => o.Weight) / objBondList.ToList().Sum(o => o.Units));
            objBondPostPaymentSheet.TotalWtPerUnit = ((ManualWeight + MechanicalWeight )/ objBondList.ToList().Sum(o => o.Units));

            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            objBondPostPaymentSheet.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            objBondPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            objBondPostPaymentSheet.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            objBondPostPaymentSheet.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            objBondPostPaymentSheet.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            objBondPostPaymentSheet.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            objBondPostPaymentSheet.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            objBondPostPaymentSheet.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            objBondPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            objBondPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            objBondPostPaymentSheet.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            objBondPostPaymentSheet.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            objBondPostPaymentSheet.CompPAN = CompPAN;
            #endregion

            var GSTType = objBondPostPaymentSheet.PartyStateCode == CompStateCode || objBondPostPaymentSheet.PartyStateCode=="";
            if (SEZ == "SEZWP")
            {
                GSTType = false;
            }
            #region Storage Charge
            var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
            if (SEZ == "SEZWOP")
            {
                cgst = 0;
                sgst = 0;
                igst = 0;
            }


            var ActualStorage = 0M;
            var validDate = DateTime.ParseExact(objBondPostPaymentSheet.UptoDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var exitDate = DateTime.ParseExact(objBondPostPaymentSheet.RequestDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var TotalDays = 0;
            if (exitDate > validDate)
            {
                TotalDays = Convert.ToInt32((exitDate - validDate).TotalDays + 1);
                var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / 7);
                ActualStorage = Math.Round(objBondPostPaymentSheet.Area * TotalWeeks * objGenericCharges.StorageRent.Where(o => o.WarehouseType == 3).FirstOrDefault().RateSqMPerWeek, 2);
            }
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 1,
                Clause = "4",
                ChargeName = "Storage Charges",
                ChargeType = "CWC",
                SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = ActualStorage,
                Discount = 0M,
                Taxable = ActualStorage,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                    (Math.Round(ActualStorage * (cgst / 100), 2)) +
                    (Math.Round(ActualStorage * (sgst / 100), 2))) :
                    (ActualStorage + (Math.Round(ActualStorage * (igst / 100), 2)))) : ActualStorage
            });
            #endregion

            #region Insurance
             cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
             sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
             igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
            if (SEZ == "SEZWOP")
            {
               cgst = 0;
                sgst = 0;
               igst = 0;
            }


            var Insurance = 0M;

            objBondList.ToList().ForEach(item =>
            {
                TotalDays = Convert.ToInt32((DateTime.ParseExact(item.DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) -
                            DateTime.ParseExact(item.UnloadedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                Insurance += Math.Round(item.IsInsured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
            });

            //Insurance += Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (objBondPostPaymentSheet.CIFValue + objBondPostPaymentSheet.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000);            
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 2,
                Clause = "5",
                ChargeName = "Insurance Charges",
                ChargeType = "CWC",
                SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = Insurance,
                Discount = 0,
                Taxable = Insurance,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (Insurance * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance +
                    (Math.Round(Insurance * (cgst / 100), 2)) +
                    (Math.Round(Insurance * (sgst / 100), 2))) :
                    (Insurance + (Math.Round(Insurance * (igst / 100), 2)))) : Insurance
            });
            #endregion

            #region Weighment
            try
            {
                var cgstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var ActualWeighmentCharges = 0M;
                
                if(Size == "20") {
                    ActualWeighmentCharges += 150;
                }
                if (Size == "40")
                {
                    ActualWeighmentCharges += 300;
                }               

                objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                {
                    ChargeId = objBondPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighmentCharges,
                    Discount = 0,
                    Taxable = ActualWeighmentCharges,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                        (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighmentCharges + (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region Other Charges
            try
            {
                var Ocgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var Osgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var Oigst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);


                if (SEZ == "SEZWOP")
                {
                    Ocgst = 0;
                    Osgst = 0;
                    Oigst = 0;
                }

                objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                {
                    ChargeId = 2,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = Ocgst,
                    SGSTPer = Osgst,
                    IGSTPer = Oigst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region H & T Charges
            var htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "5");
            if (SEZ == "SEZWOP")
            {
               cgst = 0;
                sgst = 0;
               igst = 0;
            }
            weight = objBondList.ToList().Sum(o => o.Weight);
            //var HTCharges = Math.Round(((ManualWeight / 100) * htc.RateCWC), 2);
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 4,
                Clause = "5",
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });


            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "8" && o.Size == "20");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 5,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });

            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "8" && o.Size == "40");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 6,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });

            // Add Chiranjit Das Mail By Sandip Chakroborty 2021-10-07
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "6");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 7,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "10A");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 8,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "10B");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 9,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });
            #endregion
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "30B");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 10,
                Clause = "30B",
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = 0,
                Discount = 0M,
                Taxable = 0,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (0 * (cgst / 100)) : 0) : 0,
                SGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (0 * (sgst / 100)) : 0) : 0,
                IGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (0 * (igst / 100))) : 0,
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (0 + (0 * (cgst / 100)) + (0 * (sgst / 100))) : (0 + (0 * (igst / 100)))) : 0
            });

            objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
            objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
            objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
            objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
            objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
            objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
            objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

            //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
            //    InvoiceType);
            objBondPostPaymentSheet.ActualApplicable.Clear();
  //         objBondPostPaymentSheet.ActualApplicable.Add("6");

            return Json(objBondPostPaymentSheet, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondDelPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ExportRepository objExport = new ExportRepository();
                //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objExport.DBResponse);

                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<BondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                BondRepository objChargeMaster = new BondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BND", ExportUnder);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region Bond Unloading Payment Sheet

        [HttpGet]
        public ActionResult BondUnloadPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForUnlodBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBondUnloadPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, 
            string StuffingReqDate, List<BondPaymentSheetContainer> lstPaySheetContainer, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal CIFValue, decimal Duty, decimal MechanicalWeight, decimal ManualWeight,
            int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate)
        {

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();

            string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = MechanicalWeight });
            LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = ManualWeight });

            IDataParameter[] DParam = { };
            DParam = LstParamAllNew.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            var objGenericCharges = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges_Bond", DParam);

            var objBondPostPaymentSheet = new BondPostPaymentSheet();
            objBondPostPaymentSheet.InvoiceType = InvoiceType;
            objBondPostPaymentSheet.InvoiceDate = InvoiceDate;
            objBondPostPaymentSheet.RequestId = StuffingReqId;
            objBondPostPaymentSheet.RequestNo = StuffingReqNo;
            objBondPostPaymentSheet.RequestDate = StuffingReqDate;
            objBondPostPaymentSheet.UptoDate = UptoDate;
            objBondPostPaymentSheet.Area = Area;
            objBondPostPaymentSheet.PartyId = PartyId;
            objBondPostPaymentSheet.PartyName = PartyName;
            objBondPostPaymentSheet.PartyAddress = PartyAddress;
            objBondPostPaymentSheet.PartyState = PartyState;
            objBondPostPaymentSheet.PartyStateCode = PartyStateCode;
            objBondPostPaymentSheet.PartyGST = PartyGST;
            objBondPostPaymentSheet.PayeeId = PayeeId;
            objBondPostPaymentSheet.PayeeName = PayeeName;
            objBondPostPaymentSheet.DeliveryType = DeliveryType;
            objBondPostPaymentSheet.CIFValue = CIFValue;
            objBondPostPaymentSheet.Duty = Duty;
            objBondPostPaymentSheet.Units = Units;
            objBondPostPaymentSheet.TotalGrossWt = (ManualWeight+ MechanicalWeight);
            objBondPostPaymentSheet.TotalNoOfPackages = Units;
            objBondPostPaymentSheet.TotalWtPerUnit = ((ManualWeight + MechanicalWeight) / Units);
            objBondPostPaymentSheet.IsInsured = IsInsured;
            objBondPostPaymentSheet.DepositDate = DepositDate;
            objBondPostPaymentSheet.BOEDate = BOEDate;
            objBondPostPaymentSheet.BOENo = BOENo;
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            objBondPostPaymentSheet.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            objBondPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            objBondPostPaymentSheet.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            objBondPostPaymentSheet.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            objBondPostPaymentSheet.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            objBondPostPaymentSheet.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            objBondPostPaymentSheet.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            objBondPostPaymentSheet.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            objBondPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            objBondPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            objBondPostPaymentSheet.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            objBondPostPaymentSheet.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            objBondPostPaymentSheet.CompPAN = CompPAN;
            #endregion

            var GSTType = objBondPostPaymentSheet.PartyStateCode == CompStateCode || objBondPostPaymentSheet.PartyStateCode == "";


            if (SEZ == "SEZWP")
            {
                GSTType = false;
            }
            var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
            if (SEZ == "SEZWOP")
            {
                cgst = 0;
                sgst = 0;
                igst = 0;
            }
            #region Storage Charge
            //var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            //var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
            //var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
            //if (SEZ == "SEZWOP")
            //{
            //    cgst = 0;
            //    sgst = 0;
            //    igst = 0;
            //}


            var ActualStorage = 0M;
            var validDate = DateTime.ParseExact(objBondPostPaymentSheet.UptoDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var exitDate = DateTime.ParseExact(objBondPostPaymentSheet.RequestDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var TotalDays = 0;
            if (exitDate > validDate)
            {
                TotalDays = Convert.ToInt32((exitDate - validDate).TotalDays + 1);
                var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / 7);
                ActualStorage = Math.Round(objBondPostPaymentSheet.Area * TotalWeeks * objGenericCharges.StorageRent.Where(o => o.WarehouseType == 3).FirstOrDefault().RateSqMPerWeek, 2);
            }
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 1,
                Clause = "4",
                ChargeName = "Storage Charges",
                ChargeType = "CWC",
                SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = ActualStorage,
                Discount = 0M,
                Taxable = ActualStorage,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                    (Math.Round(ActualStorage * (cgst / 100), 2)) +
                    (Math.Round(ActualStorage * (sgst / 100), 2))) :
                    (ActualStorage + (Math.Round(ActualStorage * (igst / 100), 2)))) : ActualStorage
            });
            #endregion

            #region Insurance
            cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
            sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
            igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
            if (SEZ == "SEZWOP")
            {
                cgst = 0;
                sgst = 0;
                igst = 0;
            }


            var Insurance = 0M;

            //objBondList.ToList().ForEach(item =>
            //{
            //    TotalDays = Convert.ToInt32((DateTime.ParseExact(item.DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) -
            //                DateTime.ParseExact(item.UnloadedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
            //    Insurance += Math.Round(item.IsInsured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
            //});

            //Insurance += Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (objBondPostPaymentSheet.CIFValue + objBondPostPaymentSheet.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000);            
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 2,
                Clause = "5",
                ChargeName = "Insurance Charges",
                ChargeType = "CWC",
                SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = Insurance,
                Discount = 0,
                Taxable = Insurance,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (Insurance * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance +
                    (Math.Round(Insurance * (cgst / 100), 2)) +
                    (Math.Round(Insurance * (sgst / 100), 2))) :
                    (Insurance + (Math.Round(Insurance * (igst / 100), 2)))) : Insurance
            });
            #endregion
            
            #region Weighment
            try
            {
                var cgstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igstw = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var ActualWeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    ActualWeighmentCharges += Math.Round(item.ContainerRate * lstPaySheetContainer.Count(o => o.Size == item.ContainerSize), 2);
                });                

                //var ActualWeighmentCharges = 0M;
                //if (objBondPostPaymentSheet.InvoiceType == "Tax")
                //    ActualWeighmentCharges = Math.Round(WeighmentCharges - (objBondPostPaymentSheet.lsPaymentSheetContainer.Count > 0 ? objBondPostPaymentSheet.lsPaymentSheetContainer.Sum(o => o.WeighmentCharge) : 0), 2);

                objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                {
                    ChargeId = 3,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighmentCharges,
                    Discount = 0,
                    Taxable = ActualWeighmentCharges,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                        (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighmentCharges + (Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region Other Charges
            try
            {
                var Ocgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var Osgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var Oigst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);


                if (SEZ == "SEZWOP")
                {
                    Ocgst = 0;
                    Osgst = 0;
                    Oigst = 0;
                }

                objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                {
                    ChargeId = 2,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = Ocgst,
                    SGSTPer = Osgst,
                    IGSTPer = Oigst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion


            var htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "2");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 5,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = 0M,
                Discount = 0M,
                Taxable = 0M,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = 0M,
                SGSTAmt = 0M,
                IGSTAmt = 0M,
                Total = 0M
            });

            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "10A");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 6,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = 0M,
                Discount = 0M,
                Taxable = 0M,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = 0M,
                SGSTAmt = 0M,
                IGSTAmt = 0M,
                Total = 0M
            });

            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "10B");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 7,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = 0M,
                Discount = 0M,
                Taxable = 0M,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = 0M,
                SGSTAmt = 0M,
                IGSTAmt = 0M,
                Total = 0M
            });
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "5");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId =8,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = 0M,
                Discount = 0M,
                Taxable = 0M,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = 0M,
                SGSTAmt = 0M,
                IGSTAmt = 0M,
                Total = 0M
            });
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "6");
            //var HTCharges = Math.Round((((ManualWeight+MechanicalWeight)/100) * htc.RateCWC), 2);
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 9,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });

            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "6");
            //var HTCharges = Math.Round((((ManualWeight+MechanicalWeight)/100) * htc.RateCWC), 2);
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 10,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = 0M,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });


            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "8" && o.Size=="20");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 11,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });

            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "8" && o.Size == "40");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 12,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });


            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "S-1" );
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 13,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });
            htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "30B");
            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
            {
                ChargeId = 14,
                Clause = htc.OperationCode,
                ChargeName = htc.OperationSDesc,
                ChargeType = "HT",
                SACCode = htc.SacCode,
                Quantity = 0,
                Rate = htc.RateCWC,
                Amount = htc.RateCWC,
                Discount = 0M,
                Taxable = htc.RateCWC,
                CGSTPer = cgst,
                SGSTPer = sgst,
                IGSTPer = igst,
                CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (cgst / 100)) : 0) : 0, 2),
                SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC * (sgst / 100)) : 0) : 0, 2),
                IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (htc.RateCWC * (igst / 100))) : 0, 2),
                Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (htc.RateCWC +
                    (Math.Round(htc.RateCWC * (cgst / 100), 2)) +
                    (Math.Round(htc.RateCWC * (sgst / 100), 2))) :
                    (htc.RateCWC + (Math.Round(htc.RateCWC * (igst / 100), 2)))) : htc.RateCWC
            });


            objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
            objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
            objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
            objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
            objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
            objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
            objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

            //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
            //    InvoiceType);
            objBondPostPaymentSheet.ActualApplicable.Clear();
            objBondPostPaymentSheet.ActualApplicable.Add("2");
            objBondPostPaymentSheet.ActualApplicable.Add("10A");
            objBondPostPaymentSheet.ActualApplicable.Add("10B");
            objBondPostPaymentSheet.ActualApplicable.Add("6");
            objBondPostPaymentSheet.ActualApplicable.Add("S-1");
            objBondPostPaymentSheet.ActualApplicable.Add("5");

            return Json(objBondPostPaymentSheet, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondUnloadPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ExportRepository objExport = new ExportRepository();
                //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objExport.DBResponse);

                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<BondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                BondRepository objChargeMaster = new BondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDUnld", ExportUnder);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public JsonResult GetContainersByDepositAppId(int DepositAppId = 0)
        {
            try
            {
               
                BondRepository objbndrepo = new BondRepository();
                objbndrepo.GetContainersByDepositAppId(DepositAppId);
                return Json(objbndrepo.DBResponse,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" ,Data=""};
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
