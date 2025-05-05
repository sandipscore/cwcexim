using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Import.Controllers
{
    public class Dnd_OblEntryController : BaseController
    {
        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            Dnd_OBLEntry objOBLEntry = new Dnd_OBLEntry();
            try
            {
                Dnd_ImportRepository objIR = new Dnd_ImportRepository();
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfContainerNo = objIR.DBResponse.Data;
                //}
                //ExportRepository objER = new ExportRepository();
                objIR.ListOfShippingLinePartyCode("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                Dnd_ImportRepository rep = new Dnd_ImportRepository();
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
                Ppg_ExportRepository objPER = new Ppg_ExportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                if (OBLEntryId > 0)
                {

                    rep.GetOblEntryDetailsByOblEntryId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (Dnd_OBLEntry)rep.DBResponse.Data;
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
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetOBLContainerListOrSize();
            //if (objIR.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
            //    //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.ListOfContainerNo = Jobj;
            //}
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPortList()
        {
           DNODEMasterRepository objImport = new DNODEMasterRepository();
            objImport.GetAllPort();
            if (objImport.DBResponse.Status > 0)
                return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerSize(string CFSCode)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetOBLContainerListOrSize(CFSCode);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetCFSCodeFromContainer(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(Dnd_OBLEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Dnd_ImportRepository objER = new Dnd_ImportRepository();
                List<Dnd_OblEntryDetails> lstOBL = new List<Dnd_OblEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_OblEntryDetails>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date != "" ? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd") : null);
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objER.AddEditOBLEntry(objJO, XMLText);
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
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            List<Dnd_OBLEntry> lstOBLEntry = new List<Dnd_OBLEntry>();
            objER.GetAllOblEntry(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Dnd_OBLEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_OBLEntry> LstObl = new List<Dnd_OBLEntry>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<Dnd_OBLEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfOBLEntrySearch(string Obl)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            List<Dnd_OBLEntry> lstOBLEntry = new List<Dnd_OBLEntry>();
            objER.GetAllOblEntrySearch(0, Obl);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Dnd_OBLEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLEntry", lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntry(OBLEntryId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            Dnd_OBLWiseContainerEntry objOBLEntry = new Dnd_OBLWiseContainerEntry();
            try
            {
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                Dnd_ImportRepository objIR = new Dnd_ImportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ExportRepository objER = new ExportRepository();
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.ListOfShippingLinePartyCode("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                //objER.GetShippingLine();
                //if (objER.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfShippingLine = objER.DBResponse.Data;
                //}
                BondRepository ObjBond = new BondRepository();
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

                Ppg_ExportRepository objPER = new Ppg_ExportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();

                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                if (impobldtlId > 0)
                {
                    Dnd_ImportRepository rep = new Dnd_ImportRepository();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (Dnd_OBLWiseContainerEntry)rep.DBResponse.Data;
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
        //[HttpGet]
        //public ActionResult GetAllCommodity()
        //{
        //    Ppg_ExportRepository objPER = new Ppg_ExportRepository();
        //    List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
        //    objPER.GetAllCommodity();
        //    if (objPER.DBResponse.Data != null)
        //        objImp = (List<Areas.Export.Models.Commodity>)objPER.DBResponse.Data;
        //    return Json(objImp, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetOBLWiseContainerDetails(string OBLNo)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCONTCBT(string ContainerNo,string ContainerSize)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetOBLContCBT(ContainerNo, ContainerSize);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(Dnd_OBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Dnd_ImportRepository objER = new Dnd_ImportRepository();
                List<Dnd_OBLWiseContainerEntryDetails> lstOBL = new List<Dnd_OBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_OBLWiseContainerEntryDetails>>(lstOBLDetail);
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

            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            List<Dnd_OBLWiseContainerEntry> lstOBLEntry = new List<Dnd_OBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Dnd_OBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfOBLEntrySearchFCL(string Obl)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            List<Dnd_OBLWiseContainerEntry> lstOBLEntry = new List<Dnd_OBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainerSearch(Obl);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Dnd_OBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }
        [HttpGet]
        public JsonResult LoadListMoreOBLData(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_OBLEntry> LstObl = new List<Dnd_OBLEntry>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<Dnd_OBLEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OBLAmendment
        public ActionResult OBLAmendment()
        {
            OBLAmendment objOBLAmendment = new OBLAmendment();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                objIR.ListOfOBLNo("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstObl = Jobject["LstObl"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.LstObl = null;
                    ViewBag.State = null;
                }
            }
            catch (Exception ex)
            {
            }
            return PartialView(objOBLAmendment);
        }

        [HttpGet]
        public JsonResult SearchOBLNo(string OBLNo)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadOBLNo(string OBLNo, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OBLAmendmentDetail(string OBLNo)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.OBLAmendmentDetail(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLAmendment(OBLAmendment obj)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository objER = new Ppg_ImportRepository();
                objER.AddEditOBLAmendment(obj);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        #endregion
        #region ICE Gate Detail
        public ActionResult ICEGateDetail(string SearchBy = "SearchByDB")
        {
            ViewBag.SearchBy = SearchBy;
            OBLWiseContainerEntry obj = new OBLWiseContainerEntry();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                objIR.ListOfICEGateOBLNo(SearchBy, "", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstObl = Jobject["LstObl"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.LstObl = null;
                    ViewBag.State = null;
                }
            }
            catch (Exception ex)
            {
            }
            return PartialView(obj);
        }

        [HttpGet]
        public JsonResult SearchICEGateOBLNo(string SearchBy, string OBLNo)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfICEGateOBLNo(SearchBy, OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadICEGateOBLNo(string SearchBy, string OBLNo, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfICEGateOBLNo(SearchBy, OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetICEGateDetail(string OBLNo, string SearchBy)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetICEGateDetail(OBLNo, SearchBy);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}