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
    public class VRN_OblEntryController : Controller
    {
        // GET: Import/VRN_OblEntry

        [HttpGet]
        public ActionResult GetListOfOBLEntryOblNo(string Obl)
        {
            VRN_ImportRepository objER = new VRN_ImportRepository();
            List<VRN_OBLWiseContainerEntry> lstOBLEntry = new List<VRN_OBLWiseContainerEntry>();
            objER.GetListOfOBLWiseContainerSearch(Obl);
            if (objER.DBResponse.Data != null)
            {
                lstOBLEntry = (List<VRN_OBLWiseContainerEntry>)objER.DBResponse.Data;
            }
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }

        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
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
            VRN_OblEntry objOBLEntry = new VRN_OblEntry();
            try
            {
                VRN_ImportRepository objIR = new VRN_ImportRepository();
                objIR.ListOfShippingLinePartyCode("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                VRN_ImportRepository rep = new VRN_ImportRepository();
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
                VRN_ImportRepository objPER = new VRN_ImportRepository();
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
                        objOBLEntry = (VRN_OblEntry)rep.DBResponse.Data;
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
        public JsonResult GetTContainerNoForOBLEntry(string Type)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetContainerListForOBL(Type);
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetContainerListForOBL(CFSCode);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

    

        [HttpGet]
        public ActionResult GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetCFSCodeForOBL(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetCFSCodeWiseOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(VRN_OblEntry objJO, String lstOBLDetail)
        {

            if (ModelState.IsValid)
            {
                string XMLText = "";
                VRN_ImportRepository objER = new VRN_ImportRepository();
                List<VRN_OblEntryDetails> lstOBL = new List<VRN_OblEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VRN_OblEntryDetails>>(lstOBLDetail);
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
            VRN_ImportRepository objER = new VRN_ImportRepository();
            List<VRN_OblEntry> lstOBLEntry = new List<VRN_OblEntry>();
            objER.GetAllOblEntryLCL(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<VRN_OblEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            VRN_ImportRepository ObjCR = new VRN_ImportRepository();
            List<VRN_OblEntry> LstObl = new List<VRN_OblEntry>();
            ObjCR.GetAllOblEntryLCL(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<VRN_OblEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            VRN_ImportRepository objER = new VRN_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntryLCL(OBLEntryId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            VRN_OBLWiseContainerEntry objOBLEntry = new VRN_OBLWiseContainerEntry();
            try
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
                        objOBLEntry = (VRN_OBLWiseContainerEntry)ObjIR.DBResponse.Data;
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
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(VRN_OBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                VRN_ImportRepository objER = new VRN_ImportRepository();
                List<VRN_OBLWiseContainerEntryDetails> lstOBL = new List<VRN_OBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VRN_OBLWiseContainerEntryDetails>>(lstOBLDetail);
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

            VRN_ImportRepository objER = new VRN_ImportRepository();
            List<VRN_OBLWiseContainerEntry> lstOBLEntry = new List<VRN_OBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer();
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<VRN_OBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            VRN_ImportRepository objER = new VRN_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }


        #endregion

    }
}