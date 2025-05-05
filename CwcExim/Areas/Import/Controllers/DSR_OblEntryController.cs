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
    public class DSR_OblEntryController : Controller
    {
        // GET: Import/DSR_OblEntry
        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            DSR_OblEntry objOBLEntry = new DSR_OblEntry();
            try
            {
                DSR_ImportRepository objIR = new DSR_ImportRepository();
               // DSR_ExportRepository ObjER = new DSR_ExportRepository();
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
                DSR_ImportRepository rep = new DSR_ImportRepository();
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
                 DSR_ImportRepository objPER = new DSR_ImportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
                objPER.GetAllCommodityForPage("", 0);
                if (objPER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objPER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                objIR.ListOfChaForPage("", 0);

                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }

                if (OBLEntryId > 0)
                {

                    rep.GetOblEntryDetailsByOblEntryId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (DSR_OblEntry)rep.DBResponse.Data;
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
            DSR_ImportRepository objIR = new DSR_ImportRepository();
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
            DSR_ImportRepository objIR = new DSR_ImportRepository();
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
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPortList(int Page=0, int CountryId=0, string SearchValue="")
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetAllPort(Page,CountryId, SearchValue);
            if (objImport.DBResponse.Status > 0)
                return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        

        [HttpGet]
        public ActionResult GetContainerSize(string CFSCode)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
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
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetCFSCodeFromContainer(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(DSR_OblEntry objJO, String lstOBLDetail)
        {

           // if (ModelState.IsValid)
            //{
                string XMLText = "";
                DSR_ImportRepository objER = new DSR_ImportRepository();
                List<DSR_OblEntryDetails> lstOBL = new List<DSR_OblEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_OblEntryDetails>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));                    
                    //lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date == "" ? "null": Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd"));
                    //lstOBL.ForEach(x => x.TSA_Date = x.TSA_Date == "" ? "null": Convert.ToDateTime(x.TSA_Date).ToString("yyyy-MM-dd"));
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objER.AddEditOBLEntry(objJO, XMLText);
                return Json(objER.DBResponse);
           // }
            //else
            //{
               // var data = new { Status = -1 };
                //return Json(data);
            //}
        }

        [HttpPost]
        public JsonResult DeleteOBLEntryByObl(string OblNo)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.DeleteOBLEntryByObl(OblNo);
            return Json(objER.DBResponse);

        }

        [HttpGet]
        public ActionResult ListOfOBLEntry()
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSR_OblEntry> lstOBLEntry = new List<DSR_OblEntry>();
            objER.GetAllOblEntry(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<DSR_OblEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSR_OblEntry> LstObl = new List<DSR_OblEntry>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<DSR_OblEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntry(OBLEntryId);
            return Json(objER.DBResponse);
        }

        public ActionResult ViewOBLEntry(int OBLEntryId = 0)
        {
            DSR_OblEntry objOBLEntry = new DSR_OblEntry();
            try
            {
                DSR_ImportRepository objIR = new DSR_ImportRepository();
                // DSR_ExportRepository ObjER = new DSR_ExportRepository();
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
                DSR_ImportRepository rep = new DSR_ImportRepository();
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
                DSR_ImportRepository objPER = new DSR_ImportRepository();
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

                    rep.GetOblEntryDetailsByOblEntryId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (DSR_OblEntry)rep.DBResponse.Data;
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
        public ActionResult SearchOfContainerWiseOBL(string SearchValue)
        {

            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSR_OblEntry> lstOBLEntry = new List<DSR_OblEntry>();
            objER.SearchOfContainerWiseOBL(SearchValue);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<DSR_OblEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLEntry", lstOBLEntry);
        }

        
        #endregion

        #region OBLAmendment
        public ActionResult OBLAmendment()
        {
            DSROBLAmendment objOBLAmendment = new DSROBLAmendment();
            try
            {
                DSR_ImportRepository objIR = new DSR_ImportRepository();
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
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadOBLNo(string OBLNo, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfOBLNo(OBLNo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OBLAmendmentDetail(string OBLNo)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.OBLAmendmentDetail(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLAmendment(DSROBLAmendment obj)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository objER = new DSR_ImportRepository();
                objER.AddEditOBLAmendment(obj);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        public ActionResult ListOBLAmendment(string SearchOBLNo)
        {
            List<DSROBLAmendment> lstOBLAmendment = new List<DSROBLAmendment>();
            DSR_ImportRepository obj = new DSR_ImportRepository();
            obj.GetOBLAmendmentList(SearchOBLNo);
            if (obj.DBResponse.Data != null)
            {
                lstOBLAmendment = (List<DSROBLAmendment>)obj.DBResponse.Data;
            }
            return PartialView("ListOBLAmendment", lstOBLAmendment);
        }
        public JsonResult GetOBLAmendmentList( string SearchOBLNo)
        {
            try
            {
                DSR_ImportRepository obj = new DSR_ImportRepository();
                obj.GetOBLAmendmentList(SearchOBLNo);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            DSROBLWiseContainerEntry objOBLEntry = new DSROBLWiseContainerEntry();
            try
            {
                DSR_ExportRepository ObjER = new DSR_ExportRepository();
                DSR_ImportRepository objIR = new DSR_ImportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ExportRepository objER = new ExportRepository();
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.ListOfShippingLinePartyCode("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }

                ObjIR.ListOfChaForPage("", 0);

                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }
                
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

                DSR_ExportRepository objPER = new DSR_ExportRepository();
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();

                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPort = Jobject["lstPort"];
                    ViewBag.PortState = Jobject["State"];
                }


                if (impobldtlId > 0)
                {
                    DSR_ImportRepository rep = new DSR_ImportRepository();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (DSROBLWiseContainerEntry)rep.DBResponse.Data;
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
        //    DSR_ExportRepository objPER = new DSR_ExportRepository();
        //    List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();
        //    objPER.GetAllCommodity();
        //    if (objPER.DBResponse.Data != null)
        //        objImp = (List<Areas.Export.Models.Commodity>)objPER.DBResponse.Data;
        //    return Json(objImp, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetOBLWiseContainerDetails(string OBLNo)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(DSROBLWiseContainerEntry objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                DSR_ImportRepository objER = new DSR_ImportRepository();
                List<DSROBLWiseContainerEntryDetails> lstOBL = new List<DSROBLWiseContainerEntryDetails>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSROBLWiseContainerEntryDetails>>(lstOBLDetail);
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

            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSROBLWiseContainerEntry> lstOBLEntry = new List<DSROBLWiseContainerEntry>();
            objER.ListOfOBLWiseContainer(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<DSROBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            DSR_ExportRepository objRepo = new DSR_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            DSR_ExportRepository objRepo = new DSR_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewOBLWiseContainerEntry(int impobldtlId = 0)
        {
            DSROBLWiseContainerEntry objOBLEntry = new DSROBLWiseContainerEntry();
            try
            {
                DSR_ExportRepository ObjER = new DSR_ExportRepository();
                DSR_ImportRepository objIR = new DSR_ImportRepository();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                ExportRepository objER = new ExportRepository();
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
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

                DSR_ExportRepository objPER = new DSR_ExportRepository();
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
                    DSR_ImportRepository rep = new DSR_ImportRepository();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (DSROBLWiseContainerEntry)rep.DBResponse.Data;
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
        public ActionResult SearchOfOBLWiseContainer(string SearchValue)
        {

            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSROBLWiseContainerEntry> lstOBLEntry = new List<DSROBLWiseContainerEntry>();
            objER.SearchOfOBLWiseContainer(SearchValue);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<DSROBLWiseContainerEntry>)objER.DBResponse.Data;
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreListOBLWiseContainer(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSROBLWiseContainerEntry> LstObl = new List<DSROBLWiseContainerEntry>();
            ObjCR.ListOfOBLWiseContainer(Page); 
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<DSROBLWiseContainerEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}