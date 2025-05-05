using CwcExim.Areas.Import.Models;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Import.Controllers
{
    public class Wlj_OblEntryController : Controller
    {
        // GET: Import/Wlj_OblEntry

        [HttpGet]
        public ActionResult GetListOfOBLEntryOblNo(string Obl)
        {
            Wlj_ImportRepository objER = new Wlj_ImportRepository();
            List<Wlj_OBLWiseContainerEntry> lstOBLEntry = new List<Wlj_OBLWiseContainerEntry>();
            objER.GetListOfOBLWiseContainerSearch(Obl);
            if (objER.DBResponse.Data != null)
            {
                lstOBLEntry = (List<Wlj_OBLWiseContainerEntry>)objER.DBResponse.Data;
            }
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }

        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Wlj_ImportRepository objRepo = new Wlj_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            Wlj_ImportRepository objImport = new Wlj_ImportRepository();
            objImport.GetAllPort();
            if (objImport.DBResponse.Status > 0)
                return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllCountry()
        {
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Status > 0)
                return Json(ObjCR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            Wlj_OblEntry objOBLEntry = new Wlj_OblEntry();
            try
            {
                Wlj_ImportRepository objIR = new Wlj_ImportRepository();
                objIR.ListOfShippingLinePartyCode("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                Wlj_ImportRepository rep = new Wlj_ImportRepository();
                rep.ListOfImporterForPage("", 0);
                if (rep.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(rep.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstImporter = Jobject["lstImporter"];
                    ViewBag.ImpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstImporter = null;
                }
                Wlj_ImportRepository objPER = new Wlj_ImportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
                objPER.GetAllCommodityForPage("", 0);
                if (objPER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objPER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                if (OBLEntryId > 0)
                {

                    rep.GetOblEntryLCLDetailsByOblEntryId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (Wlj_OblEntry)rep.DBResponse.Data;
                        objOBLEntry.SelectPortId = objOBLEntry.PortId;
                        objOBLEntry.SelectCountryId = objOBLEntry.CountryId;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return PartialView(objOBLEntry);
        }

        [HttpGet]
        public JsonResult GetTContainerNoForOBLEntry()
        {
            Wlj_ImportRepository objIR = new Wlj_ImportRepository();
            objIR.GetContainerListForOBL();
            //if (objIR.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
            //    //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.ListOfContainerNo = Jobj;
            //}
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetContainerSize(string CFSCode)
        {
            Wlj_ImportRepository objIR = new Wlj_ImportRepository();
            objIR.GetContainerListForOBL(CFSCode);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

    

        [HttpGet]
        public ActionResult GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            Wlj_ImportRepository objImport = new Wlj_ImportRepository();
            objImport.GetCFSCodeForOBL(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            Wlj_ImportRepository objImport = new Wlj_ImportRepository();
            objImport.GetCFSCodeWiseOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(Wlj_OblEntry objJO, String lstOBLDetail)
        {

            if (ModelState.IsValid)
            {
                string XMLText = "";
                Wlj_ImportRepository objER = new Wlj_ImportRepository();
                List<Wlj_OblEntryDetails> lstOBL = new List<Wlj_OblEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Wlj_OblEntryDetails>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date != "" ? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd") : null);
                    lstOBL.ForEach(x => x.TSA_Date = x.TSA_Date != "" ? Convert.ToDateTime(x.TSA_Date).ToString("yyyy-MM-dd") : null);
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objER.AddEditOBLEntryLCL(objJO, XMLText);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        [HttpGet]
        public ActionResult ListOfOBLEntry()
        {
            Wlj_ImportRepository objER = new Wlj_ImportRepository();
            List<Wlj_OblEntry> lstOBLEntry = new List<Wlj_OblEntry>();
            objER.GetAllOblEntryLCL(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Wlj_OblEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Wlj_ImportRepository ObjCR = new Wlj_ImportRepository();
            List<Wlj_OblEntry> LstObl = new List<Wlj_OblEntry>();
            ObjCR.GetAllOblEntryLCL(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<Wlj_OblEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            Wlj_ImportRepository objER = new Wlj_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntryLCL(OBLEntryId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            Wlj_OBLWiseContainerEntry objOBLEntry = new Wlj_OBLWiseContainerEntry();
            try
            {
                Wlj_ImportRepository ObjIR = new Wlj_ImportRepository();
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ObjIR.ListOfShippingLinePartyCode("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                ObjIR.ListOfImporterForPage("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstImporter = Jobject["lstImporter"];
                    ViewBag.ImpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstImporter = null;
                }
                // List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();

                ObjIR.GetAllCommodityForPage("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                if (impobldtlId > 0)
                {
                    ObjIR.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (ObjIR.DBResponse.Data != null)
                    {
                        objOBLEntry = (Wlj_OBLWiseContainerEntry)ObjIR.DBResponse.Data;
                        objOBLEntry.SelectPortId = objOBLEntry.PortId;
                        objOBLEntry.SelectCountryId = objOBLEntry.CountryId;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return PartialView(objOBLEntry);
        }

        [HttpGet]
        public ActionResult GetOBLWiseContainerDetails(string OBLNo)
        {
            Wlj_ImportRepository objImport = new Wlj_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(Wlj_OBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Wlj_ImportRepository objER = new Wlj_ImportRepository();
                List<Wlj_OBLWiseContainerEntryDetails> lstOBL = new List<Wlj_OBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Wlj_OBLWiseContainerEntryDetails>>(lstOBLDetail);
                    //lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objER.AddEditOBLWiseContainerEntry(objJO, XMLText);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }
        [HttpGet]
        public ActionResult ListOfOBLWiseContainer()
        {

            Wlj_ImportRepository objER = new Wlj_ImportRepository();
            List<Wlj_OBLWiseContainerEntry> lstOBLEntry = new List<Wlj_OBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer();
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Wlj_OBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            Wlj_ImportRepository objER = new Wlj_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }


        #endregion
    }
}