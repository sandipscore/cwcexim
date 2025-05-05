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
    public class Wfld_OblEntryController : Controller
    {
        // GET: Import/Wfld_OblEntry
        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            Wfld_OblEntry objOBLEntry = new Wfld_OblEntry();
            try
            {
                Wfld_ImportRepository objIR = new Wfld_ImportRepository();
                WFLD_ExportRepository objExp = new WFLD_ExportRepository();
                // WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                Wfld_ImportRepository rep = new Wfld_ImportRepository();
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
                 Wfld_ImportRepository objPER = new Wfld_ImportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
                objPER.GetAllCommodityForPage("", 0);
                if (objPER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objPER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                objExp.GetPortOfCallForPage("", 0);
                if (objExp.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPortOfCall = Jobject["lstPortOfCall"];
                    ViewBag.StatePortOfCall = Jobject["StatePortOfCall"];
                }
                else
                {
                    ViewBag.lstPortOfCall = null;
                }

                objPER.ListOfPortCodeOriginForPage("", 0);
                if (objPER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objPER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstOriginPortCode = Jobject["LstOriginPortCode"];
                    ViewBag.OriginPortCodeState = Jobject["State"];
                }
                else
                {
                    ViewBag.LstOriginPortCode = null;
                }

                if (OBLEntryId > 0)
                {

                    rep.GetOblEntryDetailsByOblEntryId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (Wfld_OblEntry)rep.DBResponse.Data;
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
            Wfld_ImportRepository objIR = new Wfld_ImportRepository();
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
        public JsonResult GetTContainerNoForOBLEntryByType(string CONTCBT)
        {
            Wfld_ImportRepository objIR = new Wfld_ImportRepository();
            objIR.GetOBLContainerListOrSizeByType(CONTCBT);
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
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public JsonResult SearchByContainer(string Container,String ContCBT)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.GetOBLContCBTSearch(ContCBT, Container);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchOriginPortCodeByName(string PortCode)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfPortCodeOriginForPage(PortCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadOriginPortCodeList(string PortCode, int Page)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfPortCodeOriginForPage(PortCode, Page);
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
            Wfld_ImportRepository objIR = new Wfld_ImportRepository();
            objIR.GetOBLContainerListOrSize(CFSCode);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetCONTCBT(string ContTypeData)
        {
            Wfld_ImportRepository objIR = new Wfld_ImportRepository();
            objIR.GetOBLContCBT(ContTypeData);
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
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetCFSCodeFromContainer(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize,  int OBLEntryId)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetOBLDetails(CFSCode, ContainerNo, ContainerSize,  OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(Wfld_OblEntry objJO, String lstOBLDetail)
        {

            if (ModelState.IsValid)
            {
                string XMLText = "";
                Wfld_ImportRepository objER = new Wfld_ImportRepository();
                List<Wfld_OblEntryDetails> lstOBL = new List<Wfld_OblEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Wfld_OblEntryDetails>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.ArrivalDate = x.ArrivalDate != "" ?Convert.ToDateTime(x.ArrivalDate).ToString("yyyy-MM-dd") : null);
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date != "" ? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd") : null);
                    lstOBL.ForEach(x => x.TSA_Date = x.TSA_Date !="" ? Convert.ToDateTime(x.TSA_Date).ToString("yyyy-MM-dd") : null);
                    lstOBL.ForEach(x => x.IGM_Date = x.IGM_Date != "" ? Convert.ToDateTime(x.IGM_Date).ToString("yyyy-MM-dd") : null);
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
            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            List<Wfld_OblEntry> lstOBLEntry = new List<Wfld_OblEntry>();
            objER.GetAllOblEntry(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Wfld_OblEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Wfld_ImportRepository ObjCR = new Wfld_ImportRepository();
            List<Wfld_OblEntry> LstObl = new List<Wfld_OblEntry>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<Wfld_OblEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntry(OBLEntryId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult ListOfOBLEntrySearch(string Obl)
        {
            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            List<Wfld_OblEntry> lstOBLEntry = new List<Wfld_OblEntry>();
            objER.GetAllOblEntrySearch(0, Obl);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<Wfld_OblEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLEntry", lstOBLEntry);
        }

        [HttpGet]
        public JsonResult SearchGatewayPortCodeByPortCode(string PortCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadGatewayPortCode(string PortCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region OBLAmendment
        public ActionResult OBLAmendment()
        {
            WFLDOBLAmendment objOBLAmendment = new WFLDOBLAmendment();
            try
            {
                Wfld_ImportRepository objIR = new Wfld_ImportRepository();
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

                Wfld_ImportRepository objIRR = new Wfld_ImportRepository();
                // WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfContainerNo = objIR.DBResponse.Data;
                //}
                //ExportRepository objER = new ExportRepository();
                objIRR.ListOfShippingLinePartyCode("", 0);
                if (objIRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIRR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.ShippingLineState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstShippingLine = null;
                    ViewBag.ShippingLineState = null;
                }
                Wfld_ImportRepository rep = new Wfld_ImportRepository();
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
                    ViewBag.ImpState = null;
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
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadOBLNo(string OBLNo, int Page)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OBLAmendmentDetail(string OBLNo)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.OBLAmendmentDetail(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
         
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLAmendment(WFLDOBLAmendment obj)
        {
            if (ModelState.IsValid)
            {
                Wfld_ImportRepository objER = new Wfld_ImportRepository();
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

        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            WFLDOBLWiseContainerEntry objOBLEntry = new WFLDOBLWiseContainerEntry();
            try
            {
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                Wfld_ImportRepository objIR = new Wfld_ImportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ExportRepository objER = new ExportRepository();
                Wfld_ImportRepository ObjIR = new Wfld_ImportRepository();
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

                WFLD_ExportRepository objPER = new WFLD_ExportRepository();
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
                    Wfld_ImportRepository rep = new Wfld_ImportRepository();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (WFLDOBLWiseContainerEntry)rep.DBResponse.Data;
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
        //    WFLD_ExportRepository objPER = new WFLD_ExportRepository();
        //    List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
        //    objPER.GetAllCommodity();
        //    if (objPER.DBResponse.Data != null)
        //        objImp = (List<Areas.Export.Models.Commodity>)objPER.DBResponse.Data;
        //    return Json(objImp, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetOBLWiseContainerDetails(string OBLNo)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(WFLDOBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Wfld_ImportRepository objER = new Wfld_ImportRepository();
                List<WFLDOBLWiseContainerEntryDetails> lstOBL = new List<WFLDOBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLDOBLWiseContainerEntryDetails>>(lstOBLDetail);
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

            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            List<WFLDOBLWiseContainerEntry> lstOBLEntry = new List<WFLDOBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<WFLDOBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }
        [HttpGet]
        public JsonResult LoadListOBLWiseMoreData(int Page)
        {
            Wfld_ImportRepository ObjCR = new Wfld_ImportRepository();
            List<WFLDOBLWiseContainerEntry> lstOBLEntry = new List<WFLDOBLWiseContainerEntry>();
            ObjCR.ListOfOBLWiseContainer(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstOBLEntry = (List<WFLDOBLWiseContainerEntry>)ObjCR.DBResponse.Data;
            }
            return Json(lstOBLEntry, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfOBLWiseEntrySearch(string Obl)
        {
            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            List<WFLDOBLWiseContainerEntry> lstOBLEntry = new List<WFLDOBLWiseContainerEntry>();
            objER.GetAllOblWiseContainerEntrySearch(0, Obl);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<WFLDOBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            Wfld_ImportRepository objER = new Wfld_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}