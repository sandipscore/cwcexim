using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
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
    //For V2
    public class Ppg_OblEntryV2Controller : BaseController
    {
        // GET: Import/Ppg_OblEntryV2
        #region OBLWiseContainerEntry
        public ActionResult OBLWiseContainerEntry(int impobldtlId = 0)
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                //}
                
                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 ObjBond = new Ppg_ImportRepositoryV2();
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

                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                    Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
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
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetOBLWiseContainerDetails(OBLNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntry(OBLWiseContainerEntryV2 objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
                List<OBLWiseContainerEntryDetailsV2> lstOBL = new List<OBLWiseContainerEntryDetailsV2>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OBLWiseContainerEntryDetailsV2>>(lstOBLDetail);
                    //lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objJO.IP = Request.UserHostAddress;
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

            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLWiseContainerEntryV2> lstOBLEntry = new List<OBLWiseContainerEntryV2>();
            objER.ListOfOBLWiseContainer();
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLWiseContainerEntryV2>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainer(int OBLEntryId)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainer(OBLEntryId);
            return Json(objER.DBResponse);
        }
               
        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
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
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeGateList(string PartyCode, int Page)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public ActionResult OBLWiseContainerView(int impobldtlId)
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            //objIR.GetOBLContainerListOrSize();
            //if (objIR.DBResponse.Data != null)
            //{
            ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                                           //}
            ExportRepository objER = new ExportRepository();
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
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
            Ppg_ImportRepositoryV2 ObjBond = new Ppg_ImportRepositoryV2();
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

            Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
            List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();

            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
            rep.GetOBLWiseContainerDetailsByID(impobldtlId);
            if (rep.DBResponse.Data != null)
            {
                objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
                objOBLEntry.SelectPortId = objOBLEntry.PortId;
                objOBLEntry.SelectCountryId = objOBLEntry.CountryId;
            }

            return PartialView(objOBLEntry);
        }


        [HttpGet]        
        public JsonResult GetCheckOblEntryFCLApproval(int OBLEntryId)
        {

            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLWiseContainerEntryV2> lstOBLEntry = new List<OBLWiseContainerEntryV2>();
            objER.GetCheckOblEntryFCLApproval(OBLEntryId);
            return Json(objER.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfOBLEntryOblNoForApplication(string Obl)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLWiseContainerEntryV2> lstOBLEntry = new List<OBLWiseContainerEntryV2>();
            objER.GetListOfOBLWiseContainerSearchForApplication(Obl);
            if (objER.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLWiseContainerEntryV2>)objER.DBResponse.Data;
            }
            return PartialView("ListOfOBLWiseContainer", lstOBLEntry);
        }


        #endregion


        #region OBL Entry FCL Approval
        public ActionResult OblEntryFCLApproval(int impobldtlId=0)
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                                               //}

                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 ObjBond = new Ppg_ImportRepositoryV2();
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

                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                    Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
                    rep.GetOBLWiseContainerDetailsByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
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


        public JsonResult GetOblEntryDetails()
        {
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.GetOBLEntryDetailsByOblNo();
            List<OblList> lstOblList = new List<OblList>();
            if(obj.DBResponse.Status==1)
            {
                lstOblList = ((List<OblList>)obj.DBResponse.Data);
            }        
            
            return Json(lstOblList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetOblEntryDetailsByOblNo(int ID)
        {
            Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
            rep.GetOBLWiseContainerDetailsByID(ID);
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            if (rep.DBResponse.Data != null)
            {
                objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
                //objOBLEntry.SelectPortId = objOBLEntry.PortId;
               // objOBLEntry.SelectCountryId = objOBLEntry.CountryId;
            }
            return Json(objOBLEntry, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLWiseContainerEntryFCLApproval(OBLWiseContainerEntryV2 objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
                List<OBLWiseContainerEntryDetailsV2> lstOBL = new List<OBLWiseContainerEntryDetailsV2>();
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OBLWiseContainerEntryDetailsV2>>(lstOBLDetail);
                    //lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    XMLText = Utility.CreateXML(lstOBL);
                }
                if(objJO.IsApproved==true)
                {
                    objJO.Approved = 1; //1 for approved
                }
                else
                {
                    objJO.Approved = 2;// 2 for rejected
                }
                objJO.IP = Request.UserHostAddress;
                objER.AddEditOBLWiseContainerEntryFCLApproval(objJO, XMLText);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }



        [HttpGet]
        public ActionResult ListOfOBLWiseContainerApproval()
        {

            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLWiseContainerEntryV2> lstOBLEntry = new List<OBLWiseContainerEntryV2>();
            objER.ListOfOBLWiseContainerApproval();
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLWiseContainerEntryV2>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        public ActionResult OblEntryFCLApprovalByEdit(int impobldtlId = 0)
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                                               //}

                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 ObjBond = new Ppg_ImportRepositoryV2();
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

                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                    Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
                    rep.GetOBLWiseContainerDetailsApprovalByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
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



        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLWiseContainerApproval(int OBLEntryId)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            if (OBLEntryId > 0)
                objER.DeleteOBLWiseContainerApproval(OBLEntryId);
            return Json(objER.DBResponse);
        }

        public ActionResult OblEntryFCLApprovalView(int impobldtlId = 0)
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                //objIR.GetOBLContainerListOrSize();
                //if (objIR.DBResponse.Data != null)
                //{
                ViewBag.ListOfContainerNo = "";// objIR.DBResponse.Data;
                                               //}

                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 ObjBond = new Ppg_ImportRepositoryV2();
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

                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                    Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
                    rep.GetOBLWiseContainerDetailsApprovalByID(impobldtlId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLWiseContainerEntryV2)rep.DBResponse.Data;
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

        public ActionResult GetListOfOBLEntryOblNo(string Obl)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLWiseContainerEntryV2> lstOBLEntry = new List<OBLWiseContainerEntryV2>();
            objER.GetListOfOBLWiseContainerSearch(Obl);
            if (objER.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLWiseContainerEntryV2>)objER.DBResponse.Data;
            }
            return PartialView("ListOfOBLWiseContainerApproval", lstOBLEntry);
        }

        #endregion

        #region OBLEntry LCL

        [HttpGet]
        public ActionResult OBLEntry(int OBLEntryId = 0)
        {
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                        objOBLEntry = (OBLEntryV2)rep.DBResponse.Data;
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
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
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
        public ActionResult GetContainerSize(string CFSCode)
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetOBLContainerListOrSize(CFSCode);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

      

        [HttpGet]
        public ActionResult GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetCFSCodeFromContainer(ContainerNo, ContainerSize, CFSCode);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetOBLDetails(CFSCode, ContainerNo, ContainerSize, IGM_No, IGM_Date, OBLEntryId);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLEntry(OBLEntryV2 objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
                List<OblEntryDetailsV2> lstOBL = new List<OblEntryDetailsV2>();
                objJO.IP=Request.UserHostAddress;
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OblEntryDetailsV2>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date != "" ? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd") : "null");
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
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> lstOBLEntry = new List<OBLEntryV2>();
            objER.GetAllOblEntry(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLEntryV2>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Ppg_ImportRepositoryV2 ObjCR = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> LstObl = new List<OBLEntryV2>();
            ObjCR.GetAllOblEntry(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<OBLEntryV2>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntry(int OBLEntryId)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntry(OBLEntryId);
            return Json(objER.DBResponse);
        }

        public ActionResult GetListOfOBLEntryContainerNoForApplication(string ContainerNo)
        {
            Ppg_ImportRepositoryV2 ObjETR = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> lstOBLEntry = new List<OBLEntryV2>();
            ObjETR.GetListOfOBLEntryByContainerNoForApplication(ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLEntryV2>)ObjETR.DBResponse.Data;
            }
            return PartialView("ListOfOBLEntry", lstOBLEntry);
        }

        [HttpGet]
        public ActionResult OBLEntryView(int OBLEntryId = 0)
        {
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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
                        objOBLEntry = (OBLEntryV2)rep.DBResponse.Data;
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
        #endregion


        #region OBL Entry LCL Approval
        [HttpGet]
        public ActionResult OBLEntryApproval(int OBLEntryId = 0)
        {
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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

                    rep.GetOblEntryDetailsByOblEntryApprovalId(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLEntryV2)rep.DBResponse.Data;
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
        public JsonResult GetTContainerNoForOBLEntryApproval()
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetOBLContainerListOrSizeApproval();
            //if (objIR.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
            //    //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.ListOfContainerNo = Jobj;
            //}
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddEditOBLEntryApproval(OBLEntryV2 objJO, String lstOBLDetail)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
                List<OblEntryDetailsV2> lstOBL = new List<OblEntryDetailsV2>();
                //objJO.IP = Request.UserHostAddress;
                
                if (lstOBLDetail.Length > 0)
                {
                    lstOBL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OblEntryDetailsV2>>(lstOBLDetail);
                    lstOBL.ForEach(x => x.OBL_Date = Convert.ToDateTime(x.OBL_Date).ToString("yyyy-MM-dd"));
                    lstOBL.ForEach(x => x.SMTP_Date = x.SMTP_Date != "" ? Convert.ToDateTime(x.SMTP_Date).ToString("yyyy-MM-dd") : "null");
                    XMLText = Utility.CreateXML(lstOBL);
                }
                objER.AddEditOBLEntryApproval(objJO, XMLText);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        [HttpGet]
        public ActionResult ListOfOBLEntryApproval()
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> lstOBLEntry = new List<OBLEntryV2>();
            objER.GetAllOblEntryApproval(0);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLEntryV2>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }



        [HttpGet]
        public ActionResult OBLEntryApprovalEdit(int OBLEntryId = 0)
        {
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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

                    rep.GetOblEntryDetailsByOblEntryApprovalIdEdit(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLEntryV2)rep.DBResponse.Data;
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
        public ActionResult OBLEntryApprovalView(int OBLEntryId = 0)
        {
            OBLEntryV2 objOBLEntry = new OBLEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 rep = new Ppg_ImportRepositoryV2();
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
                Ppg_ImportRepositoryV2 objPER = new Ppg_ImportRepositoryV2();
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

                    rep.GetOblEntryDetailsByOblEntryApprovalIdEdit(OBLEntryId);
                    if (rep.DBResponse.Data != null)
                    {
                        objOBLEntry = (OBLEntryV2)rep.DBResponse.Data;
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
        public JsonResult LoadListMoreDataApproval(int Page)
        {
            Ppg_ImportRepositoryV2 ObjCR = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> LstObl = new List<OBLEntryV2>();
            ObjCR.GetAllOblEntryApproval(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstObl = (List<OBLEntryV2>)ObjCR.DBResponse.Data;
            }
            return Json(LstObl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteOBLEntryApproval(int OBLEntryId)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            if (OBLEntryId > 0)
                objER.DeleteOBLEntryApproval(OBLEntryId);
            return Json(objER.DBResponse);
        }

        public ActionResult GetListOfOBLEntryContainerNo(string ContainerNo)
        {
            Ppg_ImportRepositoryV2 ObjETR = new Ppg_ImportRepositoryV2();
            List<OBLEntryV2> lstOBLEntry = new List<OBLEntryV2>();
            ObjETR.GetListOfOBLEntryByContainerNo(ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLEntryV2>)ObjETR.DBResponse.Data;
            }
            return PartialView("ListOfOBLEntryApproval", lstOBLEntry);
        }
        #endregion

        #region ContainerUpdation

        [HttpGet]
        public JsonResult GetTContainerNoForOBLEntryObjl()
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetOBLContainerList();
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        //Gate Entry Container No
        [HttpGet]
        public JsonResult GetTContainerNoForOBLEntryGate()
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetAllContainerNoForGateEntry();
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ContainerUpdation()
        {
            int OBLEntryId = 1;
            OBLEntryGateEntryV2 objOBLEntry = new OBLEntryGateEntryV2();
            try
            {
                Ppg_ImportRepository objIR = new Ppg_ImportRepository();
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
                Ppg_ImportRepository ImpRep = new Ppg_ImportRepository();

                objIR.GetOBLContainerListOrSize();
                if (objIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfContainerNo = objIR.DBResponse.Data;
                }           
                ExportRepository objER = new ExportRepository();
                objIR.ListOfShippingLinePartyCode("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }

                // Gate Shipping 
                ImpRep.ListOfShippingLinePartyCode("", 0);

                if (ImpRep.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLineGate = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                // Gate entry

                ObjETR.GetContainer();
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.ListOfContainerNoGate = objIR.DBResponse.Data;
                }
            }
            catch (Exception ex)
            {
            }

            return PartialView(objOBLEntry);

            //return View();
        }

        [HttpPost]
        public JsonResult UpdateOBLEntryANdGateEntry(OBLEntryGateEntryV2 objEntry)
        {
           
                string XMLText = "";
                Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
                List<OblEntryDetailsV2> lstOBL = new List<OblEntryDetailsV2>();
              if(objEntry.RGateEntry== "GateEntrys")
                {
                    objER.UpdateContainer(objEntry);
                }
              if(objEntry.ROblEntry== "OblEntrys")
                {
                    objER.UpdateContainer(objEntry);
                }
              
                return Json(objER.DBResponse);
            
           
                //return Json(data);
            
        }
        public ActionResult ListOfOBLEntryGateEntry(int Page)
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<OBLEntryGateEntryV2> lstOBLEntry = new List<OBLEntryGateEntryV2>();
            objER.GetAllOblEntryGateEntry(Page);
            if (objER.DBResponse.Data != null)
                lstOBLEntry = (List<OBLEntryGateEntryV2>)objER.DBResponse.Data;
            return PartialView(lstOBLEntry);
        }
        public ActionResult GetListOfOBLEntryGateContainerNoFor(string ContainerNo)
        {
            Ppg_ImportRepositoryV2 ObjETR = new Ppg_ImportRepositoryV2();
            List<OBLEntryGateEntryV2> lstOBLEntry = new List<OBLEntryGateEntryV2>();
            ObjETR.GetListOfOBLEntryByContainerNoFor(ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {
                lstOBLEntry = (List<OBLEntryGateEntryV2>)ObjETR.DBResponse.Data;
            }
            return PartialView("ListOfOBLEntryGateEntry", lstOBLEntry);
        }

        #endregion ContainerUpdation
    }
}