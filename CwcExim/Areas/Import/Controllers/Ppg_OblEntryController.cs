using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Import.Controllers
{
    public class Ppg_OblEntryController : BaseController
    {
        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            OBLEntry objOBLEntry = new OBLEntry();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfContainerNo = objIR.DBResponse.Data;
                //}
                //ExportRepository objER = new ExportRepository();
                objIR.ListOfShippingLinePartyCode("",0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                Ppg_ImportRepository rep = new Ppg_ImportRepository();
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
                        objOBLEntry = (OBLEntry)rep.DBResponse.Data;
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
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPortList()
        {
            PPGMasterRepository objImport = new PPGMasterRepository();
            objImport.GetAllPort();
            if (objImport.DBResponse.Status > 0)
                return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerSize(string CFSCode)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
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
        public ActionResult GetCFSCodeFromContainer(string ContainerNo, string ContainerSize,string CFSCode)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetCFSCodeFromContainer(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(OBLEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepository objER = new Ppg_ImportRepository();
                List<OblEntryDetails> lstOBL = new List<OblEntryDetails>();
                if (lstOBLDetail.Length >0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OblEntryDetails>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date!=""? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd"):null);
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
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            List<OBLEntry> lstOBLEntry = new List<OBLEntry>();
            objER.GetAllOblEntry(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }
        [HttpGet]
        public ActionResult GetListOfOBLEntryContainerNo(string ContainerNo)
        {
            Ppg_ImportRepository ObjETR = new Ppg_ImportRepository();
            List<OBLEntry> lstOBLEntry = new List<OBLEntry>();
            ObjETR.GetListOfOBLEntryByContainerNo(ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLEntry>)ObjETR.DBResponse.Data;
            }
            return PartialView("ListOfOBLEntry", lstOBLEntry);
        }

       



        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Ppg_ImportRepository ObjCR = new Ppg_ImportRepository();
            List<OBLEntry> LstObl = new List<OBLEntry>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<OBLEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public ActionResult GetListOfOBLEntryOblNo(string Obl)
        {
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            List<OBLWiseContainerEntry> lstOBLEntry = new List<OBLWiseContainerEntry>();
            objER.GetListOfOBLWiseContainerSearch(Obl);
            if (objER.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLWiseContainerEntry>)objER.DBResponse.Data;
            }
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            OBLWiseContainerEntry objOBLEntry = new OBLWiseContainerEntry();
            try
            {
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ExportRepository objER = new ExportRepository();
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
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
                    Ppg_ImportRepository rep = new Ppg_ImportRepository();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLWiseContainerEntry)rep.DBResponse.Data;
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(OBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepository objER = new Ppg_ImportRepository();
                List<OBLWiseContainerEntryDetails> lstOBL = new List<OBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OBLWiseContainerEntryDetails>>(lstOBLDetail);
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

            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            List<OBLWiseContainerEntry> lstOBLEntry = new List<OBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer();
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
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
        public ActionResult ICEGateDetail(string SearchBy= "SearchByDB")
        {
            ViewBag.SearchBy = SearchBy;
            OBLWiseContainerEntry obj = new OBLWiseContainerEntry();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                objIR.ListOfICEGateOBLNo(SearchBy,"", 0);
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
            objRepo.ListOfICEGateOBLNo(SearchBy,OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadICEGateOBLNo(string SearchBy,string OBLNo, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfICEGateOBLNo(SearchBy,OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetICEGateDetail(string OBLNo, string SearchBy)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetICEGateDetail(OBLNo,SearchBy);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OBL Entry_Update 
        public ActionResult OblEntry_Update()
        {
            OblEntry_Update objOblEntUp = new OblEntry_Update();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                objIR.ListOfEntryUpOBLNo("", 0);
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
            return PartialView(objOblEntUp);
        }

        [HttpGet]
        public JsonResult SearchOfEntryUpOBLNo(string OBLNo)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfEntryUpOBLNo(OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EntryUpdateLoadOBLNo(string OBLNo, int Page)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfEntryUpOBLNo(OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OBLEntryUpdateDetail(string OBLNo)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.OBLEntryUpdateDetail(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOblEntry_Update(OblEntry_Update obj)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository objER = new Ppg_ImportRepository();
                objER.AddEditOblEntry_Update(obj);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }


        #endregion
    }
}
