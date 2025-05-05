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
using CwcExim.Areas.ExpSealCheking.Models;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class CHN_GateOperationController : BaseController
    {
        // GET: GateOperation/CHN_GateOperation

        #region Entry Through Gate Factory Destuffing Empty
        [HttpGet]
        public ActionResult CreateEntryThroughGateFDEmpty(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------

            CHN_EntryThroughGateRepository etgRep = new CHN_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;


            //--------------------------------------------------------------------------------
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();

            ObjETR.GetContainerByRoadFDEmpty();

            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            DSREntryThroughGate objEntryThroughGate = new DSREntryThroughGate();
            DSR_EntryThroughGateRepository ObjETRR = new DSR_EntryThroughGateRepository();
            ObjETRR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (DSREntryThroughGate)ObjETRR.DBResponse.Data;
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

            CHN_EntryThroughGateRepository objImport = new CHN_EntryThroughGateRepository();
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
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

        public JsonResult GetFieldsForFDEmptyContainer(string ContainerName, string CFSCode)
        {
            if (ContainerName != "")
            {
                CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();

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

            CHN_EntryThroughGateRepository ObjEntryThroughGateRepositories = new CHN_EntryThroughGateRepository();
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
        public ActionResult EntryThroughGateFDEmptyList(string sText, string ContainerType = "", string OperationType = "")
        {
            //TempData["lstFlag"] = lstFlag;
            //TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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
            CHN_EntryThroughGateRepository etgRep = new CHN_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

            Chn_ImportRepository ImpRep = new Chn_ImportRepository();


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();

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
            CHN_EntryThroughGateRepository etgRep = new CHN_EntryThroughGateRepository();

            List<SelectListItem> PortList = new List<SelectListItem>();
            etgRep.GetPortOfLoading();
            if (etgRep.DBResponse.Data != null)
            {
                PortList = (List<SelectListItem>)etgRep.DBResponse.Data;
            }
            ViewBag.Lstpickup = PortList;

            Chn_ImportRepository ImpRep = new Chn_ImportRepository();


            DSREntryThroughGate ObjEntryGate = new DSREntryThroughGate();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();

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


            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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
                CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
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



        #region Entry Through Gate CHN



        [HttpGet]
        public ActionResult CreateEntryThroughGate(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            /*if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }*/
            List<SelectListItem> picuplists = new List<SelectListItem>();
            // picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = "OTH", Value = "O" });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<CHN_Container> lstcontainer = new List<CHN_Container>();
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
                lstcontainer = (List<CHN_Container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = lstcontainer;
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                objEntryThroughGate.TerminalOutDateTime = ExitTimeArray[0];
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
            CHN_EntryThroughGateRepository objImport = new CHN_EntryThroughGateRepository();
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
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Chn_ImportRepository objRepo = new Chn_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Chn_ImportRepository objRepo = new Chn_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            Chn_ImportRepository objRepo = new Chn_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            Chn_ImportRepository objRepo = new Chn_ImportRepository();
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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            CHN_EntryThroughGateRepository objImport = new CHN_EntryThroughGateRepository();

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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            CHN_EntryThroughGateRepository objImport = new CHN_EntryThroughGateRepository();
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
        public ActionResult CreateEntryThroughGateEmpty(int TransportMode = 1)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            CHN_EntryThroughGateRepository objImport = new CHN_EntryThroughGateRepository();

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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
              
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                objEntryThroughGate.TerminalOutDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateEntryThroughGateExport", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateExport(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

           //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];


                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;

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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;

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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ImpRep.DBResponse.Data;
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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;


                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;




                    ObjEntryGate.ReferenceDate = ReferenceDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }

            return PartialView("EditEntryThroughGateExportCBT", ObjEntryGate);


        }
        public ActionResult CreateEntryThroughGateExportTrain()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

           //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

           ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;

                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;


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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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


            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                objEntryThroughGate.TerminalOutDateTime = ExitTimeArray[0];
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
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ImpRep.DBResponse.Data;
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

          // CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;




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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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


            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
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

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;

                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;



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
                CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
                if (TransportMode == 2)
                {
                    ObjGOR.GetAutoPopulateData(ContainerName);
                }
                else
                {
                    ObjGOR.GetAutoPopulateDataByTrain(ContainerName);
                }
                return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
                // ViewBag.JSONResult = ObjGOR.DBResponse.Data;
                //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                //objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                //string strDate = objEntryThroughGate.ReferenceDate;
                //string[] arrayDate = strDate.Split(' ');
                //objEntryThroughGate.ReferenceDate = arrayDate[0];
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
            CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateCBT(ChnEntryThroughGate ObjEntryThroughGate)
        {
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }
            if (ObjEntryThroughGate.ContainerType != "LoadedVehicle")
            {
                ModelState.Remove("ReferenceNo");
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
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
            }

            if (ObjEntryThroughGate.IsCBT == true)
            {
                ModelState.Remove("Size");

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
            }

            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.TransportMode == 2)
            {
                ModelState.Remove("TransportFrom");
                //ObjEntryThroughGate.TransportFrom = "O";
            }

            if (ObjEntryThroughGate.ContainerType == "EmptyVehicle" || ObjEntryThroughGate.ContainerType == "LoadedVehicle" || ObjEntryThroughGate.ContainerType == "Empty")
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
                CHN_EntryThroughGateRepository ObjEntryThroughGateRepositories = new CHN_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                if (ObjEntryThroughGate.ShippingLineId == null)
                {
                    ObjEntryThroughGate.ShippingLineId = 0;
                }
                string XML = "";
                if (ObjEntryThroughGate.StringifyData != "" && ObjEntryThroughGate.StringifyData != null)
                {
                    IList<CHN_AddMoreRefForCCIN> LstAddRef = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHN_AddMoreRefForCCIN>>(ObjEntryThroughGate.StringifyData);
                    XML = Utility.CreateXML(LstAddRef);
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
        public ActionResult AddEditEntryThroughGate(ChnEntryThroughGate ObjEntryThroughGate)
        {
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }
           
                ModelState.Remove("ReferenceNo");
            
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



            string Entrytime1 = Request.Form["time1"];
            if (ObjEntryThroughGate.EntryId == 0)
            {


                Entrytime1 = Entrytime1.Replace("PM", " PM").Replace("AM", " AM");

            }




            if (Entrytime1.Length == 7)
            {
                Entrytime1 = Entrytime1.Replace("PM", " PM").Replace("AM", " AM");
            }

            if (ObjEntryThroughGate.EntryId > 0)
            {


                Entrytime1 = Entrytime1.Replace("  PM", " PM").Replace("  AM", " AM");

            }


            string SysTime = Request.Form["SysTime"];
            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;
            string strEntryDateTime1 = ObjEntryThroughGate.TerminalOutDateTime + " " + Entrytime1;


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
            ObjEntryThroughGate.TerminalOutDateTime = strEntryDateTime1;

            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");
            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Empty")
            {
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
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
                CHN_EntryThroughGateRepository ObjEntryThroughGateRepositories = new CHN_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                if (ObjEntryThroughGate.ShippingLineId == null)
                {
                    ObjEntryThroughGate.ShippingLineId = 0;
                }

                ObjEntryThroughGateRepositories.AddEditEntryThroughGate(ObjEntryThroughGate);
                // ModelState.Clear();
                if (ObjEntryThroughGateRepositories.DBResponse.Status == 1 && ObjEntryThroughGate.ContainerNo1 == null || ObjEntryThroughGate.ContainerNo1 == "") //|| ObjEntryThroughGateRepositories.DBResponse.Status == 2
                {
                    int lastInsertedId = Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
                    //SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
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

        [HttpGet]
        public ActionResult EntryThroughGateList(string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughGateList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughTrainGateList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughTrainGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughTrainGateList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughEmptyList(string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(0, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughEmptyList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughEmptyList(int Page, string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateRepository ObjETR = new Ppg_EntryThroughGateRepository();
            List<PpgEntryThroughGate> LstEntryThroughGate = new List<PpgEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(Page, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data);
                }
else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughCBTList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(0);                
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughCBTList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughCBTList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

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




                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                   ;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;
                 
                    ViewBag.strTime1 = convertTime1;






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


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGateDt(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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

                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;

                    ViewBag.strTime1 = convertTime1;

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
        public ActionResult EditEntryThroughGateByTrain(int EntryId)
        {
            //--------------------------------------------------------------------------------
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
     

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
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

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


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGateDt(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();

            List<container> Lstcontainer = new List<container>();
            ObjETR.GetCBT();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;


                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;


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


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGateDt(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
                    if (ObjEntryGate.OperationType == "Export" && ObjEntryGate.ContainerType == "LoadedVehicle")
                    {
                        //ObjEntryGate.ContainerType = null;
                        ObjEntryGate.ContainerType = "LoadedVehicle";
                    }
                }
            }



            return PartialView("ViewEntryThroughCBT", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult EditEntryThroughEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();

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
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strReferenceDate = ObjEntryGate.ReferenceDate;
                    string[] DateReferenceDate = strReferenceDate.Split(' ');
                    string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;


                    string strDateTime1 = ObjEntryGate.TerminalOutDateTime;
                    string[] dateTimeArray1 = strDateTime1.Split(' ');
                    string strDtae1 = dateTimeArray1[0];

                    string strTime1 = dateTimeArray1[1];
                    var convertTime1 = DateTime.ParseExact(strTime1, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ViewBag.strTime1 = convertTime1;

                    ObjEntryGate.TerminalOutDateTime = strDtae1;


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


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
                CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
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

                CHN_EntryThroughGateRepository objEntry = new CHN_EntryThroughGateRepository();
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
                        CHN_EntryThroughGateRepository etgr = new CHN_EntryThroughGateRepository();
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

        [HttpGet]
        public ActionResult ViewEntryThroughGateExport(int EntryId)
        {


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGateDt(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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



            return PartialView("ViewEntryThroughGateExport", ObjEntryGate);
        }
        #endregion

        #region CBT Gate Exit

        [HttpGet]
        public ActionResult CreateCBTGateExit(string date = "")
        {
            CHN_GateExitRepository ObjETR = new CHN_GateExitRepository();
            CHN_GateExitCBT ObjCBT = new CHN_GateExitCBT();
            CHN_ExitThroughGateHeader objExitThroughGateHeader = new CHN_ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (CHN_ExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                if (date == "")
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                ObjCBT.GateExitDateTime = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
            }
            //  ObjETR.GetGatePassLst(date);
            List<CBTList> LstCBT = new List<CBTList>();
            ObjETR.GetCBTList();
            if (ObjETR.DBResponse.Data != null)
            {

                LstCBT = (List<CBTList>)ObjETR.DBResponse.Data;
                ViewBag.LstCBT = LstCBT;
            }
            else
            {
                ViewBag.LstCBT = null;
            }

            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGateCBT(CHN_GateExitCBT objETG)
        {
            if (ModelState.IsValid)
            {
                string ExitTime = objETG.Time;
                if (objETG.ExitId == 0)
                {
                    ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }
                if (ExitTime.Length == 7)
                {
                    ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }

                string strEntryDateTime = objETG.GateExitDateTime + " " + ExitTime;
                objETG.GateExitDateTime = strEntryDateTime;



                CHN_GateExitRepository objETGR = new CHN_GateExitRepository();

                objETGR.AddEditExitThroughGateCBT(objETG, ((Login)(Session["LoginUser"])).Uid);

                ModelState.Clear();
                return Json(objETGR.DBResponse);


            }
            else
            {
                string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetAllCBTExitThroughGate()
        {
            CHN_GateExitRepository ETGR = new CHN_GateExitRepository();
            ETGR.GetAllExitThroughGateCBT();
            List<CHN_GateExitCBT> ListExitThroughGateHeader = new List<CHN_GateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<CHN_GateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public ActionResult EditExitThroughGateCBT(int ExitId)
        {
            CHN_GateExitCBT ObjExitThroughGateHeader = new CHN_GateExitCBT();
            CHN_GateExitRepository ObjExitR = new CHN_GateExitRepository();
            if (ExitId > 0)
            {
                ObjExitR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (CHN_GateExitCBT)ObjExitR.DBResponse.Data;
                    string strDateTime = ObjExitThroughGateHeader.GateExitDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");

                    ObjExitThroughGateHeader.GateExitDateTime = strDtae;
                    ViewBag.strTime = convertTime;


                }
            }
            return PartialView(ObjExitThroughGateHeader);


        }


        [HttpGet]
        public ActionResult ViewExitThroughGateCBT(int ExitId)
        {

            CHN_GateExitCBT ObjExitThroughGateHeader = new CHN_GateExitCBT();
            CHN_GateExitRepository ObjExittGateR = new CHN_GateExitRepository();
            if (ExitId > 0)
            {
                ObjExittGateR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (CHN_GateExitCBT)ObjExittGateR.DBResponse.Data;
                    string strDateTime = ObjExitThroughGateHeader.GateExitDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ObjExitThroughGateHeader.GateExitDateTime = strDtae;
                    ViewBag.strTime = convertTime;
                }
            }

            return PartialView(ObjExitThroughGateHeader);
        }


        [HttpGet]
        public ActionResult SearchGateExitCBT(string CBTNo)
        {
            CHN_GateExitRepository ETGR = new CHN_GateExitRepository();
            ETGR.SearchGateExitCBT(CBTNo);
            List<CHN_GateExitCBT> ListExitThroughGateHeader = new List<CHN_GateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<CHN_GateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGateCBT(int ExitId)
        {
            if (ExitId > 0)
            {
                CHN_GateExitRepository ObjGOR = new CHN_GateExitRepository();
                ObjGOR.DeleteExitThroughGateCBT(ExitId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion
        #region Vehicle wise Shipbill Entry
        public ActionResult CreateVehicleWiseShipbillGateEntry()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ObjETR.DBResponse.Data;
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

            //CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetVehicleWiseReferenceNumber();
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

            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            objEntryThroughGate.CargoType = 2;
            return PartialView("CreateVehicleWiseShipbillGateEntry", objEntryThroughGate);


        }
        public ActionResult EditVehicleWiseShipbillGateEntry(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Chn_ImportRepository ImpRep = new Chn_ImportRepository();
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            ObjETR.GetAllPickupLocation();
            List<CHN_PickupModel> lstPick = new List<CHN_PickupModel>();
            if (ObjETR.DBResponse.Data != null)
            {
                lstPick = (List<CHN_PickupModel>)ImpRep.DBResponse.Data;
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


            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            ObjETR.GetVehicleWiseReferenceNumber();
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
            ChnEntryThroughGate objEntryThroughGate = new ChnEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateVehicleWiseCCINDt(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjETR.DBResponse.Data;

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

            return PartialView("EditVehicleWiseShipbillGateEntry", ObjEntryGate);


        }
        [HttpGet]
        public ActionResult ViewVehicleWiseShipbillGateEntry(int EntryId)
        {


            ChnEntryThroughGate ObjEntryGate = new ChnEntryThroughGate();
            CHN_EntryThroughGateRepository ObjEtGateR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGateVehicleWiseCCINDt(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (ChnEntryThroughGate)ObjEtGateR.DBResponse.Data;


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



            return PartialView("ViewVehicleWiseShipbillGateEntry", ObjEntryGate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateVehicle(Dnd_AddExportVehicle ObjEntryThroughGate)
        {





            ModelState.Remove("ReferenceNo");

            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            ModelState.Remove("ExportNoOfPkg");
            ModelState.Remove("ExportGrWeight");
            if (ModelState.IsValid)
            {
                string Entrytime = ObjEntryThroughGate.VehicleEntryTime;
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
                string strEntryDateTime = ObjEntryThroughGate.VehicleEntryDt + " " + Entrytime;
                ObjEntryThroughGate.VehicleEntryDt = strEntryDateTime;
                Dnd_EntryThroughGateRepository ObjEntryThroughGateRepositories = new Dnd_EntryThroughGateRepository();
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

        public ActionResult AddExportVehicle(int EntryId)
        {

            Dnd_EntryThroughGateRepository ObjDR = new Dnd_EntryThroughGateRepository();
            DndEntryThroughGate DndEntryThroughGate = new DndEntryThroughGate();
            ObjDR.GetTime();
            if (ObjDR.DBResponse.Data != null)
            {
                DndEntryThroughGate = (DndEntryThroughGate)ObjDR.DBResponse.Data;
                // string EntryTime = DndEntryThroughGate.EntryDateTime;
                //  string[] EntryTimeArray = EntryTime.Split(' ');
                //  DndEntryThroughGate.EntryDateTime = EntryTimeArray[0];
                ViewBag.strTime = DndEntryThroughGate.Time;
            }
            ObjDR.GetDetForExportAddVehicle(EntryId);
            Dnd_AddExportVehicle objEV = new Dnd_AddExportVehicle();
            if (ObjDR.DBResponse.Data != null)
                objEV = (Dnd_AddExportVehicle)ObjDR.DBResponse.Data;
            return PartialView(objEV);
        }


        [HttpGet]
        public ActionResult ViewAddExportVehicle(int EntryId)
        {

            Dnd_EntryThroughGateRepository ObjDR = new Dnd_EntryThroughGateRepository();
            DndEntryThroughGate DndEntryThroughGate = new DndEntryThroughGate();
            ObjDR.GetTime();
            if (ObjDR.DBResponse.Data != null)
            {
                DndEntryThroughGate = (DndEntryThroughGate)ObjDR.DBResponse.Data;
                // string EntryTime = DndEntryThroughGate.EntryDateTime;
                //  string[] EntryTimeArray = EntryTime.Split(' ');
                //  DndEntryThroughGate.EntryDateTime = EntryTimeArray[0];
                ViewBag.strTime = DndEntryThroughGate.Time;
            }
            ObjDR.GetDetForExportAddVehicle(EntryId);
            Dnd_AddExportVehicle objEV = new Dnd_AddExportVehicle();
            if (ObjDR.DBResponse.Data != null)
                objEV = (Dnd_AddExportVehicle)ObjDR.DBResponse.Data;
            return PartialView(objEV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditVehicleWiseShipBillGateEntry(ChnEntryThroughGate ObjEntryThroughGate)
        {

            ModelState.Remove("ReferenceNo");
           
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
                ModelState.Remove("ShippingLine");
                ModelState.Remove("ShippingLineId");
            }

            if (ObjEntryThroughGate.IsCBT == true)
            {
                ModelState.Remove("Size");

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
                CHN_EntryThroughGateRepository ObjEntryThroughGateRepositories = new CHN_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                if (ObjEntryThroughGate.ShippingLineId == null)
                {
                    ObjEntryThroughGate.ShippingLineId = 0;
                }
                string XML = "";
                if (ObjEntryThroughGate.StringifyData != "" && ObjEntryThroughGate.StringifyData != null)
                {
                    IList<CHN_AddMoreRefForCCIN> LstAddRef = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHN_AddMoreRefForCCIN>>(ObjEntryThroughGate.StringifyData);
                    XML = Utility.CreateXML(LstAddRef);
                }
                ObjEntryThroughGateRepositories.AddEditVehicleWiseShipBillGateEntry(ObjEntryThroughGate, XML);
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
        [HttpGet]
        public ActionResult EntryThroughVehicleWiseSbList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            ObjETR.GetAllEntryThroughVehicleWiseShipbillListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughVehicleWiseSbList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughVehicleWiseSbList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            List<ChnEntryThroughGate> LstEntryThroughGate = new List<ChnEntryThroughGate>();
            ObjETR.GetAllEntryThroughVehicleWiseShipbillListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<ChnEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }
        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteVehicleWiseSBGateEntry(int EntryId)
        {
            if (EntryId > 0)
            {
                CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
                ObjGOR.DeleteVehicleWiseSBGateEntry(EntryId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region Exit Through Gate Factory Stuffing
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateGateExitFactoryStuffing()
        {
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();
            DSRGateExitFactoryStuffing objGateExitForFS = new DSRGateExitFactoryStuffing();
            ObjETR.GetFSGateExitTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objGateExitForFS = (DSRGateExitFactoryStuffing)ObjETR.DBResponse.Data;
                string ExitTime = objGateExitForFS.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objGateExitForFS.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objGateExitForFS.Time;
                ViewBag.ViewMode = "New";

            }

            return PartialView(objGateExitForFS);
        }


        [HttpGet]
        public JsonResult GetFSRequestNo()
        {
            CHN_EntryThroughGateRepository ObjETR = new CHN_EntryThroughGateRepository();


            List<dynamic> objImp2 = new List<dynamic>();
            ObjETR.GetFactoryStuffingRequestNoLst();
            if (ObjETR.DBResponse.Data != null)
            {
                ((List<DSRFSRequestNoList>)ObjETR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { FSRequestId = item.FSRequestId, FSRequestNo = item.FSRequestNo, FSRequestDate = item.FSRequestDate });
                });

            }

            objImp2 = objImp2.OrderByDescending(a => a.FSRequestId).ToList();// objImp2.OrderByDescending(d => objImp2.IndexOf(d.FSRequestNo)).ToList();

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerForFSRequestNo(int FSRId)
        {
            CHN_EntryThroughGateRepository ObjGOR = new CHN_EntryThroughGateRepository();
            List<DSRcontainerExitFS> lstcontainerAgainstFS = new List<DSRcontainerExitFS>();

            ObjGOR.ContainerForGateExitFS(FSRId);//, objLogin.Uid
            if (ObjGOR.DBResponse.Data != null)
            {
                lstcontainerAgainstFS = (List<DSRcontainerExitFS>)ObjGOR.DBResponse.Data;

                return Json(ObjGOR.DBResponse);
            }
            else
            {
                return Json(ObjGOR.DBResponse);
            }

        }
        public ActionResult getGateExitFactoryStuffingList()
        {
            CHN_EntryThroughGateRepository ETGR = new CHN_EntryThroughGateRepository();
            ETGR.GetAllGateExitFactoryStuffingList(0);
            List<DSRGateExitFactoryStuffing> listGateExitFactoryStuffingHeader = new List<DSRGateExitFactoryStuffing>();

            if (ETGR.DBResponse.Data != null)
            {
                listGateExitFactoryStuffingHeader = (List<DSRGateExitFactoryStuffing>)ETGR.DBResponse.Data;
            }
            return PartialView("GateExitFacroryStuffingHdrList", listGateExitFactoryStuffingHeader);

        }


        public ActionResult getGateExitFactoryStuffingListSearchByContainerNo(string ContainerNo)
        {
            CHN_EntryThroughGateRepository ETGR = new CHN_EntryThroughGateRepository();
            ETGR.GetAllGateExitFactoryStuffingList(ContainerNo);
            List<DSRGateExitFactoryStuffing> listGateExitFactoryStuffingHeader = new List<DSRGateExitFactoryStuffing>();

            if (ETGR.DBResponse.Data != null)
            {
                listGateExitFactoryStuffingHeader = (List<DSRGateExitFactoryStuffing>)ETGR.DBResponse.Data;
            }
            return PartialView("GateExitFacroryStuffingHdrList", listGateExitFactoryStuffingHeader);

        }
        //[HttpGet]
        //public JsonResult getExitHeaderListData(int Page)
        //{
        //    DSRExitThroughGateRepository ETGR = new DSRExitThroughGateRepository();
        //    ETGR.GetAllExitThroughGateList(Page);
        //    List<DSRExitThroughGateHeader> ListExitThroughGateHeader = new List<DSRExitThroughGateHeader>();

        //    if (ETGR.DBResponse.Data != null)
        //    {
        //        ListExitThroughGateHeader = (List<DSRExitThroughGateHeader>)ETGR.DBResponse.Data;
        //    }

        //    return Json(ListExitThroughGateHeader, JsonRequestBehavior.AllowGet);
        //}




        //[HttpGet]
        //[CustomValidateAntiForgeryToken]
        //public ActionResult getExitDetailsList(int HeaderId, string ViewMode = "")
        //{
        //    DSRExitThroughGateRepository ETGR = new DSRExitThroughGateRepository();
        //    ETGR.GetExitThroughGateDetailsForHdr(HeaderId);
        //    List<DSRExitThroughGateDetails> ListExitThroughGateDetails = new List<DSRExitThroughGateDetails>();

        //    if (ETGR.DBResponse.Data != null)
        //    {
        //        ListExitThroughGateDetails = (List<DSRExitThroughGateDetails>)ETGR.DBResponse.Data;
        //    }
        //    if (ViewMode == "")
        //        return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
        //    else return Json(ListExitThroughGateDetails, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditGateExitFactoryStuffing(DSRGateExitFactoryStuffing objETG)
        {
            string ExitTime = "";
            int ExitHourCal = 0;
            if (ModelState.IsValid)
            {
                //var GateExitlst = (dynamic)null;
                String GateExitlst = objETG.StrExitThroughGateDetails;
                string[] Time = objETG.Time.Split(':');
                string ExitHour = Time[0];
                string ExitTimeDet = Time[1];
                string Exitminute = ExitTimeDet.Substring(0, 2);
                string TimePref = ExitTimeDet.Substring(ExitTimeDet.Length - 2);
                if (TimePref == "PM" && Convert.ToInt32(ExitHour) < 12)
                {
                    ExitHourCal = Convert.ToInt32(ExitHour) + 12;
                    ExitTime = Convert.ToString(ExitHourCal) + ":" + Exitminute;
                }
                else
                {
                    ExitTime = objETG.Time.Substring(0, 5);
                }

                string strEntryDateTime = objETG.GateExitDateTime + " " + ExitTime;
                objETG.GateExitDateTime = strEntryDateTime;


                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<DSRGateExitFactoryStuffingDtl> LstGateExitFactoryStuffingDtl = new List<DSRGateExitFactoryStuffingDtl>();

                CHN_EntryThroughGateRepository objETGR = new CHN_EntryThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRGateExitFactoryStuffingDtl>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstGateExitFactoryStuffingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRGateExitFactoryStuffingDtl>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
                    string XML = Utility.CreateXML(LstGateExitFactoryStuffingDtl);
                    var XMLContent = Utility.CreateXML(LstGateExitFactoryStuffingDtl);
                    objETG.StrExitThroughGateDetails = XML;



                }
                else
                {
                    objETG.StrExitThroughGateDetails = "";
                }
                objETGR.AddEditGateExitFactoryStuffing(objETG, ((Login)(Session["LoginUser"])).Uid);


                ModelState.Clear();
                return Json(objETGR.DBResponse);
            }
            else
            {
                string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        ////[HttpPost]
        ////[CustomValidateAntiForgeryToken]
        //public JsonResult AddToDetailsGateExit(DSRExitThroughGateHeader objETG)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string GateExitlst = objETG.StrExitThroughGateDetails;
        //        int BranchId = Convert.ToInt32(Session["BranchId"]);
        //        objETG.BranchId = BranchId;
        //        IList<DSRExitThroughGateDetails> LstExitThroughGateDetails = new List<DSRExitThroughGateDetails>();
        //        // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
        //        LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
        //        string XML = Utility.CreateXML(LstExitThroughGateDetails);
        //        DSRExitThroughGateRepository objETGR = new DSRExitThroughGateRepository();
        //        objETG.StrExitThroughGateDetails = XML;
        //        objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


        //        if (objETGR.DBResponse.Status == 1)
        //        {

        //            var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRExitThroughGateDetails>>(GateExitlst);
        //            arrExitThroughGateDetailsList.GroupBy(o => o.ShippingLineId).ToList().ForEach(item =>
        //            {

        //                SendExitMail(item.Key.ToString());
        //            });

        //        }
        //        ModelState.Clear();
        //        return Json(objETG);
        //    }
        //    else
        //    {
        //        //var Err = new { Statua = -1, Messgae = "Error" };
        //        //return Json(Err);
        //        string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
        //        var Err = new { Statua = -1, Messgae = "Error" };
        //        return Json(Err);
        //    }
        //}



        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult EditGateExitFactoryStuffing(int ExitIdHdr)
        {
            DSRGateExitFactoryStuffing ObjGateExitFactoryStuffingHeader = new DSRGateExitFactoryStuffing();
            CHN_EntryThroughGateRepository ObjExitR = new CHN_EntryThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetGateExitFactoryStuffing(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjGateExitFactoryStuffingHeader = (DSRGateExitFactoryStuffing)ObjExitR.DBResponse.Data;
                    string strDateTime = ObjGateExitFactoryStuffingHeader.GateExitDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDate = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strFSRequestDate = ObjGateExitFactoryStuffingHeader.FSRequestDate;
                    string[] FSRequestDateArray = strFSRequestDate.Split(' ');
                    string FSRequestDate = FSRequestDateArray[0];

                    ObjGateExitFactoryStuffingHeader.GateExitDateTime = strDate;
                    ObjGateExitFactoryStuffingHeader.FSRequestDate = FSRequestDate;
                    ViewBag.strTime = convertTime;


                    //List<DSRGatePassList> LstGatePass = new List<DSRGatePassList>();
                    //ObjExitR.GetGatePassLst();
                    //if (ObjExitR.DBResponse.Data != null)
                    //{

                    //    LstGatePass = (List<DSRGatePassList>)ObjExitR.DBResponse.Data;
                    //    ViewBag.LstGatePass = LstGatePass;
                    //}
                    //List<DSRcontainer> Lstcontainer = new List<DSRcontainer>();
                    //ObjExitR.GetContainer();

                    //if (ObjExitR.DBResponse.Data != null)
                    //{
                    //    ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((DSRExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
                    //}



                }
            }
            return PartialView(ObjGateExitFactoryStuffingHeader);


        }

        //[HttpGet]
        //public ActionResult EditExitThroughGateDetails(int ExitIdDtls)
        //{
        //    DSRExitThroughGateDetails ObjExitThroughGateDetails = new DSRExitThroughGateDetails();
        //    DSRExitThroughGateRepository ObjExitR = new DSRExitThroughGateRepository();
        //    if (ExitIdDtls > 0)
        //    {
        //        ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
        //        if (ObjExitR.DBResponse.Data != null)
        //        {
        //            ObjExitThroughGateDetails = (DSRExitThroughGateDetails)ObjExitR.DBResponse.Data;
        //        }
        //    }
        //    ViewBag.EditMode = "Edit";

        //    List<DSRcontainer> Lstcontainer = new List<DSRcontainer>();
        //    ObjExitR.GetContainer();
        //    //if (ObjETR.DBResponse.Data != null)
        //    //{
        //    //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
        //    //}
        //    //ViewBag.Lstcontainer = Lstcontainer;
        //    if (ObjExitR.DBResponse.Data != null)
        //    {
        //        ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((DSRExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
        //    }

        //    ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

        //    ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

        //    return PartialView(ObjExitThroughGateDetails);


        //}

        //[HttpPost]
        //public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        //{
        //    DSRExitThroughGateDetails ObjExitThroughGateDetails = new DSRExitThroughGateDetails();
        //    DSRExitThroughGateRepository ObjExitR = new DSRExitThroughGateRepository();
        //    if (ExitIdDtls > 0)
        //    {
        //        ObjExitR.GetExitThroughGate(ExitIdDtls);
        //        if (ObjExitR.DBResponse.Data != null)
        //        {
        //            ObjExitThroughGateDetails = (DSRExitThroughGateDetails)ObjExitR.DBResponse.Data;
        //        }
        //    }

        //    return PartialView(ObjExitThroughGateDetails);


        //}

        //public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        //{
        //    DSRExitThroughGateDetails ObjExitThroughGateDetails = new DSRExitThroughGateDetails();
        //    DSRExitThroughGateRepository ObjExitR = new DSRExitThroughGateRepository();
        //    if (ExitIdDtls > 0)
        //    {
        //        ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
        //        if (ObjExitR.DBResponse.Data != null)
        //        {
        //            ObjExitThroughGateDetails = (DSRExitThroughGateDetails)ObjExitR.DBResponse.Data;
        //        }
        //    }
        //    ViewBag.ViewFlag = "v";
        //    return PartialView(ObjExitThroughGateDetails);


        //}



        [HttpGet]
        public ActionResult ViewGateExitFactoryStuffing(int ExitIdHdr)
        {

            DSRGateExitFactoryStuffing ObjGateExitFactoryStuffingHeader = new DSRGateExitFactoryStuffing();
            DSRExitThroughGateRepository ObjExitR = new DSRExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {

                ObjExitR.GetGateExitFactoryStuffing(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjGateExitFactoryStuffingHeader = (DSRGateExitFactoryStuffing)ObjExitR.DBResponse.Data;
                    string strDateTime = ObjGateExitFactoryStuffingHeader.GateExitDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDate = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strFSRequestDate = ObjGateExitFactoryStuffingHeader.FSRequestDate;
                    string[] FSRequestDateArray = strFSRequestDate.Split(' ');
                    string FSRequestDate = FSRequestDateArray[0];

                    ObjGateExitFactoryStuffingHeader.GateExitDateTime = strDate;
                    ObjGateExitFactoryStuffingHeader.FSRequestDate = FSRequestDate;
                    ViewBag.strTime = convertTime;
                }
            }
            ViewBag.ViewMode = "view";
            ViewBag.HeaderId = ExitIdHdr;

            return PartialView(ObjGateExitFactoryStuffingHeader);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteGateExitFactoryStuffing(int ExitId)
        {
            if (ExitId > 0)
            {
                DSRExitThroughGateRepository ObjGOR = new DSRExitThroughGateRepository();
                ObjGOR.DeleteGateExitFactoryStuffing(ExitId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        //[NonAction]
        //public int SendExitMail(string ShippingLineId)//string ContainerNo
        //{
        //    try
        //    {
        //        string message = "";
        //        var file = (dynamic)null;
        //        string time = DateTime.Now.ToString("H:mm").Replace(":", "");
        //        string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
        //        DSRExitThroughGateRepository objExit = new DSRExitThroughGateRepository();
        //        objExit.GetDetailsForGateExitMail(ShippingLineId);
        //        if (objExit.DBResponse.Data != null)
        //        {
        //            //EmailDataModel ObjEmailDataModel = new EmailDataModel();
        //            //ObjEmailDataModel.ReceiverEmail = ((EntryThroughGateMail)objExit.DBResponse.Data).Email;
        //            var mailTo = ((DSREntryThroughGateMail)objExit.DBResponse.Data).Email.Replace(" ", "").Split(',');
        //            mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            for (int j = 0; j < mailTo.Length; j++)
        //            {
        //                mailTo[j].Trim();
        //            }

        //            var FileName = ((DSREntryThroughGateMail)objExit.DBResponse.Data).FileName;
        //            //ObjEmailDataModel.Subject = "Test Subject";
        //            //ObjEmailDataModel.MailBody = "Exit Through Gate";
        //            //List<string> containerList = new List<string>();
        //            var excelData = ((DSREntryThroughGateMail)objExit.DBResponse.Data).lstExcelData;
        //            var excelString = string.Empty;
        //            excelData.ToList().ForEach(item =>
        //            {
        //                excelString += item.Line.PadRight(5)
        //               + item.ContainerNumber.PadRight(15)
        //               + item.Size.PadRight(10)
        //               + item.MoveCode.PadRight(10)
        //               + item.EntryDateTime.PadRight(13)
        //               + item.CurrentLocation.PadRight(5)
        //               + item.ToLocation.PadRight(5)
        //               + item.BookingRefNo.PadRight(25)
        //               + item.Customer.PadRight(10)
        //               + item.Transporter.PadRight(10)
        //               + item.TruckNumber.PadRight(25)
        //               + item.Condition.PadRight(1)
        //               + item.ReportedBy.PadRight(10)
        //               + item.ReportDate.PadRight(8)
        //               + item.Remarks.PadRight(50)
        //               + item.TransportMode.PadRight(1)
        //               + item.JobOrder.PadRight(25) + Environment.NewLine;
        //            });
        //            //foreach (var container in excelData.ToList())
        //            //{
        //            //    containerList.Add(container.ContainerNumber);
        //            //}

        //            //string msgContainers = String.Join(",", containerList);


        //            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
        //            string FolderPath = Server.MapPath("~/Uploads/GateExitExcel/" + UID);
        //            if (!System.IO.Directory.Exists(FolderPath))
        //            {
        //                System.IO.Directory.CreateDirectory(FolderPath);
        //            }
        //            System.IO.File.WriteAllText((FolderPath + "\\" + FileName), excelString);
        //            string[] FileList = new string[1];
        //            FileList[0] = FolderPath + "\\" + FileName;
        //            string status = UtilityClasses.CommunicationManager.SendMail(
        //                   "Container Exited Through Gate",
        //               "Container Number : " + containerArray + " ,Exited Through Gate",
        //                mailTo,
        //                new[] { FolderPath + "\\" + FileName }


        //                );//SendMailWithAttachment(ObjEmailDataModel, FileList);

        //            if (status == "Success")
        //            {

        //                DSRExitThroughGateRepository etgr = new DSRExitThroughGateRepository();
        //                foreach (var ContainerNo in containerArray.Split(',').ToArray())
        //                {
        //                    etgr.ExitMailStatus(ContainerNo);
        //                    if (etgr.DBResponse.Status == 1)
        //                    {
        //                        message = "Email Status Updated";
        //                    }

        //                }
        //                if (System.IO.Directory.Exists(FolderPath))
        //                {
        //                    System.IO.Directory.Delete(FolderPath, true);
        //                }
        //            }

        //            else
        //            {
        //                string FolderPath2 = Server.MapPath("~/Uploads/ExitEmailError/" + CuurDate);
        //                if (!System.IO.Directory.Exists(FolderPath2))
        //                {
        //                    System.IO.Directory.CreateDirectory(FolderPath2);



        //                }
        //                file = Path.Combine(FolderPath2, time + "_ErrorExitEmail.txt");
        //                string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

        //                using (var tw = new StreamWriter(file, true))
        //                {
        //                    tw.WriteLine("For Container No:" + containerArray + " Email not Sent To " + MailIds + "Error:" + status);
        //                    tw.Close();
        //                }


        //            }

        //        }
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }

        //}
        #endregion

        #region Gate Operation Print

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GateInPrint(int GateInID)
        {
            CHN_EntryThroughGateRepository objGPR = new CHN_EntryThroughGateRepository();
            objGPR.GetDetailsForGatePassPrint(GateInID);
            CHN_GateEntryPrint objGP = new CHN_GateEntryPrint();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {

                objGP = (CHN_GateEntryPrint)objGPR.DBResponse.Data;
                       FilePath = GeneratingPDF(objGP, GateInID);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        private string GeneratingPDF(CHN_GateEntryPrint objGP, int GateInID)
        {
            string html = "";
                 var ContainerNo = ""; var VehicleNo = "";
           

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GatePass" + GateInID + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
           // string PortName = objGP.PortName;
            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td><td style='font-weight:600;text-align:right;'>Valid Upto</td><td><span>" + objGP.ValidTill + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Port Name</td><td  colspan='3'><br/><span>" + PortName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center;width:40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center;width:30%;'> PO<br/>CWC-"+objGP.CompanyLocation+"</th><th style='border-top:1px solid #000;text-align:center;width:40%;'> Delivered By<br/> Shed Incharge/CWC- "+objGP.CompanyLocation+"</th></tr></thead></table></td></tr></tbody></table></td></tr><tr><td colspan='2'><br/><br/><br/><br/><br/><br/><br/><br/>***Material handed over in good condition</td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/19</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + objGP.CompanyAdrress + "</span><br/><span style='font-size:12px;'>Email - " + objGP.CompanyMail + "</span><br/><label style='font-size: 14px; font-weight:bold;'>GATE PASS IN</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass In No. : <span style='font-weight:normal;'>" + objGP.GateInNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass In Date : <span style='font-weight:normal;'>" + objGP.EntryDate + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + objGP.VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass In Time : <span style='font-weight:normal;'>" + objGP.EntryTime + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. /size : <span style='font-weight:normal;'>" + objGP.ContainerNo + "/" + objGP.Size + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + objGP.ShippingLineSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'>" + objGP.CustomSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description: <span style='font-weight:normal;'>" + objGP.CargoDescription + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLine + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + objGP.NoOfPackages + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + objGP.GrossWeight + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Operation Type : <span style='font-weight:normal;'>" + objGP.OperationType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + objGP.CargoType + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container Type : <span style='font-weight:normal;'>" + objGP.ContainerType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container Load Type : <span style='font-weight:normal;'>" + objGP.ContainerLoadType + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Terminal Date : <span style='font-weight:normal;'>" + objGP.TerminalDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Terminal Time : <span style='font-weight:normal;'>" + objGP.TerminalTime + "</span></th></tr>";


            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + objGP.DisplayCfs + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA</th>";
            html += "<th width='1%'></th>";
           

            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC - " + objGP.CompanyLocation + "</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));









            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GateInID + ".pdf";
        }

        #endregion
    }
}