using CwcExim.Areas.GateOperation.Models;
using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.Models;
using System.Web.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using CwcExim.UtilityClasses;
using CwcExim.Areas.Import.Models;
using System.Text;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class DSR_GateOperationController : Controller 
    {
        #region Entry Through Gate Factory Destuffing Empty
        [HttpGet]
        public ActionResult CreateEntryThroughGateFDEmpty(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------

            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;


            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
      
            ObjETR.GetContainerByRoadFDEmpty();
            
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            //if (TransportMode == 2)
            //{
            objEntryThroughGate.TransportMode = 2;
            //}
            //else
            //{
            //    objEntryThroughGate.TransportMode = 1;
            //}

            DSR_EntryThroughGateRepository objImport = new DSR_EntryThroughGateRepository();
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            objEntryThroughGate.CargoType = 2;
            objEntryThroughGate.TransportFrom = 0;

            return PartialView(objEntryThroughGate);


        }

        public JsonResult GetFieldsForFDEmptyContainer(string ContainerName,string CFSCode)
        {
            if (ContainerName != "")
            {
                DSR_EntryThroughGateRepository ObjGOR = new DSR_EntryThroughGateRepository();
              
                ObjGOR.GetFDEmptyContainerDetails(ContainerName, CFSCode);
                              
                DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
                objEntryThroughGate = (DSREntryThroughGate)ObjGOR.DBResponse.Data;
                
                return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
           
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateFDEmpty(DSREntryThroughGate ObjEntryThroughGate)
        {           

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1;
            }

            string Entrytime = Request.Form["time"];
            if (ObjEntryThroughGate.EntryId == 0)
            {

                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }
            if (Entrytime.Length == 7)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }

            if (ObjEntryThroughGate.EntryId > 0)
            {

                Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");
            }
            string SysTime = Request.Form["SysTime"];
            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;
            if (SysTime != null)
            {
                string[] SplitSysTime = SysTime.Split(':');
                string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                string strSysDateTime = ObjEntryThroughGate.SystemDateTime + " " + SysTime;
                ObjEntryThroughGate.SystemDateTime = strSysDateTime;

            }
            else
            {
                ObjEntryThroughGate.SystemDateTime = null;
            }
          
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;  
            
            if (ObjEntryThroughGate.ContainerType == "FDEmpty")
            {
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
                ModelState.Remove("Size");
                ModelState.Remove("TransportMode");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
            }
            
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
           
            DSR_EntryThroughGateRepository ObjEntryThroughGateRepositories = new DSR_EntryThroughGateRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjEntryThroughGate.Uid = ObjLogin.Uid;
            if (ObjEntryThroughGate.ShippingLineId == null)
            {
                ObjEntryThroughGate.ShippingLineId = 0;
            }
           
                ObjEntryThroughGate.TransportFrom = 0;
           

            ObjEntryThroughGateRepositories.AddEditEntryThroughGateFDEmpty(ObjEntryThroughGate);
           
            if (ObjEntryThroughGateRepositories.DBResponse.Status == 1 && ObjEntryThroughGate.ContainerNo1 == null || ObjEntryThroughGate.ContainerNo1 == "") //|| ObjEntryThroughGateRepositories.DBResponse.Status == 2
            {
                int lastInsertedId = Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
                SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
            }

            return Json(ObjEntryThroughGateRepositories.DBResponse);
            
        }

        [HttpGet]
        public ActionResult EntryThroughGateFDEmptyList(string sText, string ContainerType = "",  string OperationType = "")
        {
            //TempData["lstFlag"] = lstFlag;
            //TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateFDEmptyListPage(0, sText);
            if (ObjETR.DBResponse.Data != null)
            {
              LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);                            
            }

            return PartialView("EntryThroughGateFDEmptyList", LstEntryThroughGate);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGateFDEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainerByRoadFDEmpty();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateFDEmpty(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");                    
                    ObjEntryGate.EntryDateTime = strDtae;                   
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }

                }
            }
            return PartialView("EditEntryThroughGateFDEmpty", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughGateFDEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

            DSR_ImportRepository ImpRep = new DSR_ImportRepository();


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainerByRoadFDEmpty();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateFDEmpty(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ObjEntryGate.EntryDateTime = strDtae;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }

                }
            }
            return PartialView("ViewEntryThroughGateFDEmpty", ObjEntryGate);


        }

        public JsonResult LoadMoreEntryThroughGateFDEmptyList(int Page, string sText = "", string OperationType = "", string ContainerType = "")
        {
            

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateFDEmptyListPage(Page, sText);
            if (ObjETR.DBResponse.Data != null)
            {

             LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteEntryThroughGateFDEmpty(int EntryId)
        {
            if (EntryId > 0)
            {
                DSR_EntryThroughGateRepository ObjGOR = new DSR_EntryThroughGateRepository();
                ObjGOR.DeleteEntryThroughGateFDEmpty(EntryId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Entry Through Gate DSR



        [HttpGet]
        public ActionResult CreateEntryThroughGate(int TransportMode = 1)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
           
            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();
            
            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

           
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            if (TransportMode == 2)
            {
                ObjETR.GetContainerByRoad();
            }
            else
            {
                ObjETR.GetContainerByTrain();
            }
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            if (TransportMode == 2)
            {
                objEntryThroughGate.TransportMode = 2;
            }
            else
            {
                objEntryThroughGate.TransportMode = 1;
            }

            string check = "";
            string check1 = "";
            DSR_EntryThroughGateRepository objImport = new DSR_EntryThroughGateRepository();
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            objEntryThroughGate.CargoType = 2;

            return PartialView(objEntryThroughGate);


        }        

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                   // string ReferenceDate = strReferenceDate;
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }

                }
            }
            return PartialView("EditEntryThroughGate", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughGate(int EntryId)
        {


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {

                List<SelectListItem> PortList = new List<SelectListItem>();
                ObjEtGateR.GetPortOfLoading();
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    PortList = (List<SelectListItem>)ObjEtGateR.DBResponse.Data;
                }
                ViewBag.Lstpickup = PortList;

                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;

                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }



            return PartialView("ViewEntryThroughGate", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateImpLoad(int EntryId)
        {
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                List<SelectListItem> PortList = new List<SelectListItem>();
                ObjEtGateR.GetPortOfLoading();
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    PortList = (List<SelectListItem>)ObjEtGateR.DBResponse.Data;
                }
                ViewBag.Lstpickup = PortList;

                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;

                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }



            return PartialView(ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateExpEmp(int EntryId)
        {
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {

                List<SelectListItem> PortList = new List<SelectListItem>();
                //ObjEtGateR.GetPortOfLoading();
                //if (ObjEtGateR.DBResponse.Data != null)
                //{
                //    PortList = (List<SelectListItem>)ObjEtGateR.DBResponse.Data;
                //}
                ViewBag.Lstpickup = PortList;

                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;

                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }
            return PartialView(ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateExpLoad(int EntryId)
        {
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {

                List<SelectListItem> PortList = new List<SelectListItem>();
                //ObjEtGateR.GetPortOfLoading();
                //if (ObjEtGateR.DBResponse.Data != null)
                //{
                //    PortList = (List<SelectListItem>)ObjEtGateR.DBResponse.Data;
                //}
                ViewBag.Lstpickup = PortList;

                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;

                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }



            return PartialView(ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateBond(int EntryId)
        {
            DSR_EntryThroughGateBond ObjEntryGate = new DSR_EntryThroughGateBond();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjEtGateR.GetEntryThroughGateBond(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSR_EntryThroughGateBond)ObjEtGateR.DBResponse.Data;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;
                }
            }
            return PartialView(ObjEntryGate);
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
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateEntryThroughGateByTrain(int TransportMode = 1)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            if (TransportMode == 2)
            {
                ObjETR.GetContainer();
            }
            else
            {
                ObjETR.GetContainerByTrain();
            }
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }





            if (TransportMode == 2)
            {
                objEntryThroughGate.TransportMode = 2;
            }
            else
            {
                objEntryThroughGate.TransportMode = 1;
            }

            string check = "";
            string check1 = "";
            DSR_EntryThroughGateRepository objImport = new DSR_EntryThroughGateRepository();

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            objEntryThroughGate.CargoType = 2;

            return PartialView(objEntryThroughGate);


        }
        [HttpGet]
        public ActionResult CreateEntryThroughGateCBT(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            if (TransportMode == 2)
            {
                ObjETR.GetCBT();
            }
            else
            {
                ObjETR.GetCBT();
            }
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }





            if (TransportMode == 2)
            {
                objEntryThroughGate.TransportMode = 2;
            }
            else
            {
                objEntryThroughGate.TransportMode = 1;
            }

            string check = "";
            string check1 = "";
            DSR_EntryThroughGateRepository objImport = new DSR_EntryThroughGateRepository();
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //commented as cha is hidden in view
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            objEntryThroughGate.CargoType = 2;

            return PartialView(objEntryThroughGate);


        }
        [HttpGet]
        public ActionResult CreateEntryThroughGateEmpty(int TransportMode = 1)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            if (TransportMode == 2)
            {
                ObjETR.GetContainer();
            }
            else
            {
                ObjETR.GetContainerByTrain();
            }
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }





            if (TransportMode == 2)
            {
                objEntryThroughGate.TransportMode = 2;
            }
            else
            {
                objEntryThroughGate.TransportMode = 1;
            }

            string check = "";
            string check1 = "";
            DSR_EntryThroughGateRepository objImport = new DSR_EntryThroughGateRepository();

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }


            objEntryThroughGate.CargoType = 2;

            return PartialView(objEntryThroughGate);


        }
        public ActionResult CreateEntryThroughGateExport()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            //commented as it is not using in view
            //ObjETR.GetReferenceNumber();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //    ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            //}
            //if (ObjReferenceNumber.ReferenceList.Count > 0)
            //{
            //    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            //}
            //else
            //{
            //    ViewBag.lstReferenceNumberList = null;

            //}
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateExport", objEntryThroughGate);


        }
        

        public ActionResult EditEntryThroughGateBond(int EntryId)
        {
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            DSR_EntryThroughGateBond ObjEntryGate = new DSR_EntryThroughGateBond();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateBond(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSR_EntryThroughGateBond)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }
            return PartialView(ObjEntryGate);
        }

        public ActionResult EditEntryThroughGateExport(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            string OpType = "";

            if (EntryId > 0)
            {
                OpType = "Export";
                ViewBag.OpType = OpType;
            }
            else
            {
                ViewBag.OpType = "";
            }
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            //commented as it is not used in view
            //ObjETR.GetReferenceNumber();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //    ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            //}
            //if (ObjReferenceNumber.ReferenceList.Count > 0)
            //{
            //    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            //}
            //else
            //{
            //    ViewBag.lstReferenceNumberList = null;

            //}
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateExport", ObjEntryGate);


        }

        public ActionResult CreateEntryThroughGateExportEmpty()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }


            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateExportEmpty", objEntryThroughGate);


        }
        public ActionResult EditEntryThroughGateExportEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            string OpType = "";

            if (EntryId > 0)
            {
                OpType = "Export";
                ViewBag.OpType = OpType;
            }
            else
            {
                ViewBag.OpType = "";
            }
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateExportEmpty", ObjEntryGate);


        }
        public ActionResult CreateEntryThroughGateExportCBT()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //commented as cha is hidden in view
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateExportCBT", objEntryThroughGate);


        }
        public ActionResult EditEntryThroughGateExportCBT(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            string OpType = "";

            if (EntryId > 0)
            {
                OpType = "Export";
                ViewBag.OpType = OpType;
            }
            else
            {
                ViewBag.OpType = "";
            }
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjETR.ListContainerClass();
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ContClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ContClass = null;
            }
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ViewBag.ContainerClass = ObjETR.DBResponse.Message;
                }
            }

            return PartialView("EditEntryThroughGateExportCBT", ObjEntryGate);


        }
        //public ActionResult EditEntryThroughGateExportCBT(int EntryId)
        //{
        //    //--------------------------------------------------------------------------------
        //    DSR_ImportRepository ImpRep = new DSR_ImportRepository();
        //    ImpRep.GetAllPickupLocation();
        //    List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
        //    if (ImpRep.DBResponse.Data != null)
        //    {
        //        lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
        //    }
        //    List<SelectListItem> picuplists = new List<SelectListItem>();
        //    picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
        //    foreach (var x in lstPick)
        //    {
        //        picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
        //    }

        //    ViewBag.Lstpickup = picuplists;
        //    //--------------------------------------------------------------------------------
        //    string OpType = "";

        //    if (EntryId > 0)
        //    {
        //        OpType = "Export";
        //        ViewBag.OpType = OpType;
        //    }
        //    else
        //    {
        //        ViewBag.OpType = "";
        //    }
        //    List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
        //    List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

        //    List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

        //    DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
        //    // List<container> Lstcontainer = new List<container>();
        //    ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
        //    ObjETR.GetReferenceNumber();
        //    if (ObjETR.DBResponse.Data != null)
        //    {
        //        ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


        //        ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


        //    }
        //    if (ObjReferenceNumber.ReferenceList.Count > 0)
        //    {
        //        ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
        //    }
        //    else
        //    {
        //        ViewBag.lstReferenceNumberList = null;

        //    }
        //    ImpRep.ListOfShippingLinePartyCode("", 0);

        //    if (ImpRep.DBResponse.Data != null)
        //    {
        //        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
        //        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
        //        ViewBag.State = Jobject["State"];
        //    }
        //    ImpRep.ListOfChaForPage("", 0);

        //    if (ImpRep.DBResponse.Data != null)
        //    {
        //        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
        //        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        ViewBag.lstCHA = Jobject["lstCHA"];
        //        ViewBag.CHAState = Jobject["State"];
        //    }
        //    DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
        //    ObjETR.GetTime();
        //    if (ObjETR.DBResponse.Data != null)
        //    {
        //        objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
        //        string ExitTime = objEntryThroughGate.EntryDateTime;
        //        string[] ExitTimeArray = ExitTime.Split(' ');
        //        objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
        //        ViewBag.strTime = objEntryThroughGate.Time;
        //    }
        //    DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
        //    //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
        //    if (EntryId > 0)
        //    {
        //        ObjETR.GetEntryThroughGate(EntryId);
        //        if (ObjETR.DBResponse.Data != null)
        //        {
        //            ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

        //            string strDateTime = ObjEntryGate.EntryDateTime;
        //            string[] dateTimeArray = strDateTime.Split(' ');
        //            string strDtae = dateTimeArray[0];

        //            string strTime = dateTimeArray[1];
        //            var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
        //            string strReferenceDate = ObjEntryGate.ReferenceDate;
        //            string[] DateReferenceDate = strReferenceDate.Split(' ');
        //            string ReferenceDate = DateReferenceDate[0];

        //            ObjEntryGate.EntryDateTime = strDtae;
        //            ObjEntryGate.ReferenceDate = ReferenceDate;
        //            ViewBag.strTime = convertTime;
        //            string SystemDateTime = ObjEntryGate.SystemDateTime;
        //            string[] SplitSystemDateTime = SystemDateTime.Split(' ');
        //            var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
        //            ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
        //            ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
        //        }
        //    }

        //    return PartialView("EditEntryThroughGateExportCBT", ObjEntryGate);


        //}
        public ActionResult CreateEntryThroughGateExportTrain()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateExportTrain", objEntryThroughGate);


        }
        public ActionResult EditEntryThroughGateExportTrain(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            string OpType = "";

            if (EntryId > 0)
            {
                OpType = "Export";
                ViewBag.OpType = OpType;
            }
            else
            {
                ViewBag.OpType = "";
            }
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            }
            if (ObjReferenceNumber.ReferenceList.Count > 0)
            {
                ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            }
            else
            {
                ViewBag.lstReferenceNumberList = null;

            }
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateExportTrain", ObjEntryGate);


        }

        public ActionResult CreateEntryThroughGateLoadContainer()
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;

            DSR_EntryThroughGateRepository etgRep = new DSR_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            //etgRep.GetPortOfLoading();
            //if (etgRep.DBResponse.Data != null)
            //{
            //    PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            //}
            ViewBag.Lstpickup = PortList;
            //--------------------------------------------------------------------------------


            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data; 


                LoadContainerReference = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (LoadContainerReference != null && LoadContainerReference.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(LoadContainerReference);
                }
                else
                {
                    LoadContainerReference = null;
                }



                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();

            }
            ObjETR.GetShippingLineLoadCont();

            lstShippingLine = (List<ShippingLineList>)ObjETR.DBResponse.Data;

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);


            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                objEntryThroughGate.ShippingLine = "";
                objEntryThroughGate.ShippingLineId = 0;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateLoadContainer", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateLoadContainer(int EntryId)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (lstLoadContainer != null && lstLoadContainer.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstLoadContainer);
                }
                else
                {
                    lstLoadContainer = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();

            }

            ObjETR.GetShippingLineLoadCont();

            lstShippingLine = (List<ShippingLineList>)ObjETR.DBResponse.Data;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateLoadContainer", ObjEntryGate);


        }
        public ActionResult CreateEntryThroughGateLoadContainerTrain()
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------


            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                LoadContainerReference = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (LoadContainerReference != null && LoadContainerReference.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(LoadContainerReference);
                }
                else
                {
                    LoadContainerReference = null;
                }



                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();

            }
            ObjETR.GetShippingLineLoadCont();

            lstShippingLine = (List<ShippingLineList>)ObjETR.DBResponse.Data;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);


            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                objEntryThroughGate.ShippingLine = "";
                objEntryThroughGate.ShippingLineId = 0;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateLoadContainerTrain", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateLoadContainerTrain(int EntryId)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (lstLoadContainer != null && lstLoadContainer.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstLoadContainer);
                }
                else
                {
                    lstLoadContainer = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();

            }

            //ObjETR.GetShippingLineLoadCont();

            //lstShippingLine = (List<ShippingLineList>)ObjETR.DBResponse.Data;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            ImpRep.ListOfChaForPage("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateLoadContainerTrain", ObjEntryGate);


        }
        public JsonResult GetFieldsForContainer(string ContainerName, int TransportMode = 2)
        {
            if (ContainerName != "")
            {
                DSR_EntryThroughGateRepository ObjGOR = new DSR_EntryThroughGateRepository();
                if (TransportMode == 2)
                {
                    ObjGOR.GetAutoPopulateData(ContainerName);
                }
                else
                {
                    ObjGOR.GetAutoPopulateDataByTrain(ContainerName);
                }
                DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
                objEntryThroughGate = (DSREntryThroughGate)ObjGOR.DBResponse.Data;
                if (objEntryThroughGate.ReferenceDate != "")
                {
                    string strDate = objEntryThroughGate.ReferenceDate;
                    string[] arrayDate = strDate.Split(' ');
                    objEntryThroughGate.ReferenceDate = arrayDate[0];
                }
                return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
                // ViewBag.JSONResult = ObjGOR.DBResponse.Data;
                //ViewBag.strTime = objEntryThroughGate.EntryTime;

                //return Json(objEntryThroughGate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
                // ViewBag.JSONResult = "Error";
                //return PartialView("CreateEntryThroughGate");
            }

        }

        public JsonResult getLoadedContainerList(int LoadContainerRefId)
        {
            DSR_EntryThroughGateRepository ObjGOR = new DSR_EntryThroughGateRepository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateCBT(DSREntryThroughGate ObjEntryThroughGate)
        {
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1;
            }

            string Entrytime = Request.Form["time"];
            if (ObjEntryThroughGate.EntryId == 0)
            {


                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");

            }
            if (Entrytime.Length == 7)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }

            if (ObjEntryThroughGate.EntryId > 0)
            {


                Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");

            }
            string SysTime = Request.Form["SysTime"];
            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;
            if (SysTime != null)
            {
                string[] SplitSysTime = SysTime.Split(':');
                string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                string strSysDateTime = ObjEntryThroughGate.SystemDateTime + " " + SysTime;
                ObjEntryThroughGate.SystemDateTime = strSysDateTime;

            }
            else
            {
                ObjEntryThroughGate.SystemDateTime = null;
            }

            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;
            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");
            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Empty")
            {
                ModelState.Remove("ShippingLineId");
                ModelState.Remove("ShippingLine");
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }

            if (ObjEntryThroughGate.IsCBT == true)
            {
                ModelState.Remove("Size");

            }

            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Loaded")
            {
                ModelState.Remove("ShippingLineId");
                ModelState.Remove("ShippingLine");
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }
            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.ContainerType == "Loaded")
            {
                ModelState.Remove("TransportFrom");
            }

            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.TransportMode == 2)
            {
                ModelState.Remove("TransportFrom");
                //ObjEntryThroughGate.TransportFrom = "O";
            }

            if (ObjEntryThroughGate.ContainerType == "EmptyVehicle" || ObjEntryThroughGate.ContainerType == "LoadedVehicle")
            {
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
                ModelState.Remove("Size");
                ModelState.Remove("TransportMode");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
            }
            if (ObjEntryThroughGate.ContainerType == "EmptyVehicle")
            {
                ModelState.Remove("CargoType");
            }

            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
            if (ModelState.IsValid)
            {
                DSR_EntryThroughGateRepository ObjEntryThroughGateRepositories = new DSR_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                if (ObjEntryThroughGate.ShippingLineId == null)
                {
                    ObjEntryThroughGate.ShippingLineId = 0;
                }

                ObjEntryThroughGateRepositories.AddEditEntryThroughGateCBT(ObjEntryThroughGate);
                // ModelState.Clear();
                if (ObjEntryThroughGateRepositories.DBResponse.Status == 1 && ObjEntryThroughGate.ContainerNo1 == null || ObjEntryThroughGate.ContainerNo1 == "") //|| ObjEntryThroughGateRepositories.DBResponse.Status == 2
                {
                    int lastInsertedId = Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
                    SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                }

                return Json(ObjEntryThroughGateRepositories.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGate(DSREntryThroughGate ObjEntryThroughGate)
        {
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1;
            }

            string Entrytime = Request.Form["time"];
            if (ObjEntryThroughGate.EntryId == 0)
            {


                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");

            }
            if (Entrytime.Length == 7)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }

            if (ObjEntryThroughGate.EntryId > 0)
            {


                Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");

            }
            string SysTime = Request.Form["SysTime"];
            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;
            if (SysTime != null)
            {
                string[] SplitSysTime = SysTime.Split(':');
                string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                string strSysDateTime = ObjEntryThroughGate.SystemDateTime + " " + SysTime;
                ObjEntryThroughGate.SystemDateTime = strSysDateTime;

            }
            else
            {
                ObjEntryThroughGate.SystemDateTime = null;
            }

            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;
            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");
            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Empty")
            {
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }

            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Loaded")
            {
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }

            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.ContainerType == "Loaded")
            {
                ModelState.Remove("TransportFrom");
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
            }

            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.TransportMode == 2)
            {
                ModelState.Remove("TransportFrom");
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
                //ObjEntryThroughGate.TransportFrom = "O";
            }

            if (ObjEntryThroughGate.ContainerType == "EmptyVehicle" || ObjEntryThroughGate.ContainerType == "LoadedVehicle")
            {
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
                ModelState.Remove("Size");
                ModelState.Remove("TransportMode");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
            }
            if (ObjEntryThroughGate.ContainerType == "EmptyVehicle")
            {
                ModelState.Remove("CargoType");
            }

            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
            //if (ModelState.IsValid)
            //{
                DSR_EntryThroughGateRepository ObjEntryThroughGateRepositories = new DSR_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                if (ObjEntryThroughGate.ShippingLineId == null)
                {
                    ObjEntryThroughGate.ShippingLineId = 0;
                }
                if (ObjEntryThroughGate.TransportFrom == null)
                {
                    ObjEntryThroughGate.TransportFrom = 0;
                }

            ObjEntryThroughGateRepositories.AddEditEntryThroughGate(ObjEntryThroughGate);
                // ModelState.Clear();
                if (ObjEntryThroughGateRepositories.DBResponse.Status == 1 && ObjEntryThroughGate.ContainerNo1 == null || ObjEntryThroughGate.ContainerNo1 == "") //|| ObjEntryThroughGateRepositories.DBResponse.Status == 2
                {
                    int lastInsertedId = Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
                    SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                }

                return Json(ObjEntryThroughGateRepositories.DBResponse);
           // }
            //else
            //{
            //    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            //    var Err = new { Status = -1, Message = ErrorMessage };
            //    return Json(Err);
            //    //var Err = new { Status = 0, Message = "Please fill all the required details" };
            //    //return Json(Err);
            //}
        }

        [HttpGet]
        public ActionResult EntryThroughGateList(string ContainerType="", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(0, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            ViewBag.ContainerType = ContainerType;
            ViewBag.OperationType = OperationType;
            if (OperationType.ToLower() == "bond")
            {
                return PartialView("EntryThroughGateBondList", LstEntryThroughGate);
            }
            else
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }
        }


        [HttpGet]
        public ActionResult EntryThroughGateSearchList(string ContainerNo,string ContainerType = "", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();
            ViewBag.ContainerType = ContainerType;
            ViewBag.OperationType = OperationType;

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateSearchListPage(0, OperationType, ContainerType, ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            if (OperationType.ToLower() == "bond")
            {
                return PartialView("EntryThroughGateBondList", LstEntryThroughGate);
            }
            else if(OperationType.ToLower() == "Export")
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }
            else
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }

        }
        
        public JsonResult LoadMoreEntryThroughGateList(int Page, string lstFlag = "All", string OperationType = "", string ContainerType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(Page, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }

            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughTrainGateList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughTrainGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughTrainGateList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughEmptyList(string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(0, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughEmptyList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughEmptyList(int Page, string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(Page, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughCBTList(string Module, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(Module, 0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughCBTList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughCBTList(string Module,int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            List<DSREntryThroughGate> LstEntryThroughGate = new List<DSREntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(Module,Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<DSREntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

       
        [HttpGet]
        public ActionResult EditEntryThroughGateByTrain(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                }
            }
            return PartialView("EditEntryThroughGateByTrain", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughGateTrain(int EntryId)
        {


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;


                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }



            return PartialView("ViewEntryThroughGateTrain", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult EditEntryThroughCBT(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetCBT();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                }
            }
            return PartialView("EditEntryThroughCBT", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughCBT(int EntryId)
        {


            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetCBT();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                }
            }
            return PartialView("ViewEntryThroughCBT", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughCBTExport(int EntryId)
        {
            //--------------------------------------------------------------------------------
            //DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            //}
            List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            //List<container> Lstcontainer = new List<container>();
            //ObjETR.GetCBT();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            //}
            //ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    //ImpRep.ListOfShippingLinePartyCode("", 0);

                    //if (ImpRep.DBResponse.Data != null)
                    //{
                    //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                    //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    //    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    //    ViewBag.State = Jobject["State"];
                    //}
                    //ImpRep.ListOfChaForPage("", 0);

                    //if (ImpRep.DBResponse.Data != null)
                    //{
                    //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                    //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    //    ViewBag.lstCHA = Jobject["lstCHA"];
                    //    ViewBag.CHAState = Jobject["State"];
                    //}
                }
            }
            return PartialView(ObjEntryGate);
        }
        [HttpGet]
        public ActionResult EditEntryThroughEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                    ImpRep.ListOfShippingLinePartyCode("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    ImpRep.ListOfChaForPage("", 0);

                    if (ImpRep.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                }
            }
            return PartialView("EditEntryThroughEmpty", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughEmpty(int EntryId)
        {


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;


                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }



            return PartialView("ViewEntryThroughEmpty", ObjEntryGate);
        }
        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteEntryThroughGate(int EntryId)
        {
            if (EntryId > 0)
            {
                DSR_EntryThroughGateRepository ObjGOR = new DSR_EntryThroughGateRepository();
                ObjGOR.DeleteEntryThroughGate(EntryId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        [NonAction]
        public int SendEntryMail(string ContainerNo, int lastInsertedId)
        {
            try
            {
                string message = "";
                var file = (dynamic)null;

                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                DSR_EntryThroughGateRepository objEntry = new DSR_EntryThroughGateRepository();
                objEntry.GetDetailsForGateEntryMail(ContainerNo);
                if (objEntry.DBResponse.Data != null)
                {
                    var mailTo = ((EntryThroughGateMail)objEntry.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j = 0; j < mailTo.Length; j++)
                    {
                        mailTo[j].Trim();
                    }

                    //string[] ReceiverEmailArray= ObjEmailDataModel.ReceiverEmail.s
                    var FileName = ((EntryThroughGateMail)objEntry.DBResponse.Data).FileName;
                    //ObjEmailDataModel.Subject = " Container Entered Through Gate";
                    //ObjEmailDataModel.MailBody = "Container Number : "+ ContainerNo+" ,Entred Through Gate";
                    var excelData = ((EntryThroughGateMail)objEntry.DBResponse.Data).lstExcelData;
                    var excelString = string.Empty; ;

                    excelData.ToList().ForEach(item => {
                        excelString += item.Line.PadRight(5)
                       + item.ContainerNumber.PadRight(15)
                       + item.Size.PadRight(10)
                       + item.MoveCode.PadRight(10)
                       + item.EntryDateTime.PadRight(13)
                       + item.CurrentLocation.PadRight(5)
                       + item.ToLocation.PadRight(5)
                       + item.BookingRefNo.PadRight(25)
                       + item.Customer.PadRight(10)
                       + item.Transporter.PadRight(10)
                       + item.TruckNumber.PadRight(25)
                       + item.Condition.PadRight(1)
                       + item.ReportedBy.PadRight(10)
                       + item.ReportDate.PadRight(8)
                       + item.Remarks.PadRight(50)
                       + item.TransportMode.PadRight(1)
                       + item.JobOrder.PadRight(25) + Environment.NewLine;
                    });

                    string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                    string FolderPath = Server.MapPath("~/Uploads/GateEntryExcel/" + UID);
                    if (!System.IO.Directory.Exists(FolderPath))
                    {
                        System.IO.Directory.CreateDirectory(FolderPath);
                    }
                    System.IO.File.WriteAllText((FolderPath + "\\" + FileName), excelString);// System.IO.File.WriteAllText((FolderPath + "/" + FileName), excelString);
                    string[] FileList = new string[1];
                    FileList[0] = FolderPath + "\\" + FileName;
                    string status = UtilityClasses.CommunicationManager.SendMail(
                         "Container Entered Through Gate",
                       "Container Number : " + ContainerNo + " ,Entred Through Gate",
                        mailTo,
                        new[] { FolderPath + "\\" + FileName } // new[] { FolderPath + "/" + FileName }
                        );
                    if (status == "Success")
                    {
                        DSR_EntryThroughGateRepository etgr = new DSR_EntryThroughGateRepository();
                        etgr.EntryMailStatus(ContainerNo, lastInsertedId);
                        if (etgr.DBResponse.Status == 1)
                        {
                            message = "Email Status Updated";
                        }

                        if (System.IO.Directory.Exists(FolderPath))
                        {
                            System.IO.Directory.Delete(FolderPath, true);
                        }

                    }
                    else
                    {
                        string FolderPath2 = Server.MapPath("~/Uploads/EntryEmailError/" + CuurDate);
                        if (!System.IO.Directory.Exists(FolderPath2))
                        {
                            System.IO.Directory.CreateDirectory(FolderPath2);



                        }
                        file = Path.Combine(FolderPath2, time + "_ErrorEntryEmail.txt");
                        string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                        using (var tw = new StreamWriter(file, true))
                        {
                            tw.WriteLine("For Container No :" + ContainerNo + " .Email not Sent To :" + MailIds + "\r\n Error:" + status);
                            tw.Close();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }


        public ActionResult AddExportVehicle(int EntryId)
        {

            DSR_EntryThroughGateRepository ObjDR = new DSR_EntryThroughGateRepository();
            ObjDR.GetDetForExportAddVehicle(EntryId);
            DSR_AddExportVehicle objEV = new DSR_AddExportVehicle();
            if (ObjDR.DBResponse.Data != null)
                objEV = (DSR_AddExportVehicle)ObjDR.DBResponse.Data;
            return PartialView(objEV);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateVehicle(DSR_AddExportVehicle ObjEntryThroughGate)
        {

            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            ModelState.Remove("ExportNoOfPkg");
            ModelState.Remove("ExportGrWeight");
            if (ModelState.IsValid)
            {
                DSR_EntryThroughGateRepository ObjEntryThroughGateRepositories = new DSR_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;


                ObjEntryThroughGateRepositories.AddEditEntryThroughGateVehicle(ObjEntryThroughGate);
                // ModelState.Clear();

                return Json(ObjEntryThroughGateRepositories.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }


        [HttpGet]

        public ActionResult GetVehicleDtlById(int EntryId)
        {
            DSR_EntryThroughGateRepository ObjDR = new DSR_EntryThroughGateRepository();
            ObjDR.GetVehicleDtlbyEntryId(EntryId);
            List<DSR_AddExportVehicle> LstVehicle = new List<DSR_AddExportVehicle>();
            if (ObjDR.DBResponse.Data != null)
                LstVehicle = (List<DSR_AddExportVehicle>)ObjDR.DBResponse.Data;
            return PartialView("ExportVehicleList", LstVehicle);
        }


        #endregion

        public ActionResult CreateEntryThroughGateBond()
        {
            List<BondReferenceNumberList> lstReferenceNumberList = new List<BondReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            // List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetBondRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstReferenceNumberList = (List<BondReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (lstReferenceNumberList != null && lstReferenceNumberList.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();

            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //  ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            DSR_EntryThroughGateBond objEntryThroughGate = new DSR_EntryThroughGateBond();
            ObjETR.GetBondTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSR_EntryThroughGateBond)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.IsVehicle = 0;
            return PartialView("CreateEntryThroughGateBond", objEntryThroughGate);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateBond(DSR_EntryThroughGateBond ObjEntryThroughGate)
        {
            ObjEntryThroughGate.OperationType = "Bond";
            if (!string.IsNullOrEmpty(ObjEntryThroughGate.CBTNo))
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.CBTNo;
            }
            /*if (ObjEntryThroughGate.IsVehicle == 1)
                ObjEntryThroughGate.ContainerType = "Empty";
            else
                ObjEntryThroughGate.ContainerType = "Loaded";*/
            string Entrytime = Request.Form["time"];
            string SysTime = Request.Form["SysTime"];

            if (ObjEntryThroughGate.EntryId == 0)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }
            if (Entrytime.Length == 7)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }
            if (ObjEntryThroughGate.EntryId > 0)
            {
                Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");
            }
            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;
            if (SysTime != null)
            {
                string[] SplitSysTime = SysTime.Split(':');
                string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                // SysTime = SplitSysTime[0] + ":" + SplitSysTime[1] + "  "+ SystemTime;
                SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                string strSysDateTime = ObjEntryThroughGate.SystemDateTime + " " + SysTime;
                ObjEntryThroughGate.SystemDateTime = strSysDateTime;
            }
            else
            {
                ObjEntryThroughGate.SystemDateTime = null;
            }
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;

            ObjEntryThroughGate.BranchId = Convert.ToInt32(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                ModelState.Remove("ShippingLine");
                DSR_EntryThroughGateRepository ObjEntryThroughGateRepositories = new DSR_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                ObjEntryThroughGateRepositories.AddEditEntryThroughGateBond(ObjEntryThroughGate);
                return Json(ObjEntryThroughGateRepositories.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public JsonResult GetSACDetails(int Id)
        {
            DSR_EntryThroughGateRepository obj = new DSR_EntryThroughGateRepository();
            obj.SACDetails(Id);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateEntryThroughGateImportEmpty()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            //commented as it is not using in view
            //ObjETR.GetReferenceNumber();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //    ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            //}
            //if (ObjReferenceNumber.ReferenceList.Count > 0)
            //{
            //    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            //}
            //else
            //{
            //    ViewBag.lstReferenceNumberList = null;

            //}
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            objEntryThroughGate.CargoType = 2;


            return PartialView("CreateEntryThroughGateImportEmpty", objEntryThroughGate);


        }
        public ActionResult EditEntryThroughGateImportEmpty(int EntryId)
        {

            DSR_ImportRepository ImpRep = new DSR_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<DSRPickupModel> lstPick = new List<DSRPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<DSRPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            string OpType = "";

            if (EntryId > 0)
            {
                OpType = "Import";
                ViewBag.OpType = OpType;
            }
            else
            {
                ViewBag.OpType = "";
            }
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            DSR_EntryThroughGateRepository ObjETR = new DSR_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            //commented as it is not used in view
            //ObjETR.GetReferenceNumber();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //    ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            //}
            //if (ObjReferenceNumber.ReferenceList.Count > 0)
            //{
            //    ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            //}
            //else
            //{
            //    ViewBag.lstReferenceNumberList = null;

            //}
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ImpRep.ListOfChaForPage("", 0);

            //if (ImpRep.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.CHAState = Jobject["State"];
            //}

            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateImportEmpty", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughGateImpEmp(int EntryId)
        {
            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjEtGateR = new DSR_EntryThroughGateRepository();
            if (EntryId > 0)
            {

                List<SelectListItem> PortList = new List<SelectListItem>();
                //ObjEtGateR.GetPortOfLoading();
                //if (ObjEtGateR.DBResponse.Data != null)
                //{
                //    PortList = (List<SelectListItem>)ObjEtGateR.DBResponse.Data;
                //}
                ViewBag.Lstpickup = PortList;

                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (DSREntryThroughGate)ObjEtGateR.DBResponse.Data;

                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;

                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "Loaded")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.OperationType = "LoadedContainer";
                    }
                }
            }
            return PartialView(ObjEntryGate);
        }
    }
}