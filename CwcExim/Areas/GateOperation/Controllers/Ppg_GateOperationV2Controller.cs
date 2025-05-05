using CwcExim.Areas.Export.Models;
using CwcExim.Areas.GateOperation.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CwcExim.Areas.GateOperation.Controllers
{
    //For V2 
    public class Ppg_GateOperationV2Controller : BaseController
    {
        // GET: GateOperation/Ppg_GateOperationV2
        #region Entry Through Gate Ppg



        [HttpGet]
        public ActionResult CreateEntryThroughGate(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            // picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<containerV2> Lstcontainer = new List<containerV2>();
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
                Lstcontainer = (List<containerV2>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository objImport = new Ppg_EntryThroughGateV2Repository();
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
            Ppg_EntryThroughGateV2Repository objRepo = new Ppg_EntryThroughGateV2Repository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Ppg_EntryThroughGateV2Repository objRepo = new Ppg_EntryThroughGateV2Repository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            Ppg_EntryThroughGateV2Repository objRepo = new Ppg_EntryThroughGateV2Repository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            Ppg_EntryThroughGateV2Repository objRepo = new Ppg_EntryThroughGateV2Repository();
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<containerV2> Lstcontainer = new List<containerV2>();
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
                Lstcontainer = (List<containerV2>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository objImport = new Ppg_EntryThroughGateV2Repository();

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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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



        public ActionResult EditEntryThroughGateExport(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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
            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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

            return PartialView("EditEntryThroughGateExportCBT", ObjEntryGate);


        }
        public ActionResult CreateEntryThroughGateExportTrain()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
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
            List<ReferenceNumberListV2> lstReferenceNumberList = new List<ReferenceNumberListV2>();
            List<GateEntryExportV2> lstGateEntryExport = new List<GateEntryExportV2>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).ReferenceList;


                ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCINV2)ObjETR.DBResponse.Data).listShippingLine;


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

            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------


            List<LoadContainerReferenceNumberListV2> LoadContainerReference = new List<LoadContainerReferenceNumberListV2>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberV2 ObjReferenceNumber = new ReferenceNumberV2();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                LoadContainerReference = (List<LoadContainerReferenceNumberListV2>)ObjETR.DBResponse.Data;

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

            lstShippingLine = (List<ShippingLineListV2>)ObjETR.DBResponse.Data;

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


            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            List<LoadContainerReferenceNumberListV2> lstLoadContainer = new List<LoadContainerReferenceNumberListV2>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberV2 ObjReferenceNumber = new ReferenceNumberV2();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberListV2>)ObjETR.DBResponse.Data;

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

            lstShippingLine = (List<ShippingLineListV2>)ObjETR.DBResponse.Data;
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

            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------


            List<LoadContainerReferenceNumberListV2> LoadContainerReference = new List<LoadContainerReferenceNumberListV2>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberV2 ObjReferenceNumber = new ReferenceNumberV2();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                LoadContainerReference = (List<LoadContainerReferenceNumberListV2>)ObjETR.DBResponse.Data;

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

            lstShippingLine = (List<ShippingLineListV2>)ObjETR.DBResponse.Data;
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


            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;
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
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            List<LoadContainerReferenceNumberListV2> lstLoadContainer = new List<LoadContainerReferenceNumberListV2>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineListV2> lstShippingLine = new List<ShippingLineListV2>();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberV2 ObjReferenceNumber = new ReferenceNumberV2();
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberListV2>)ObjETR.DBResponse.Data;

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

            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
                Ppg_EntryThroughGateV2Repository ObjGOR = new Ppg_EntryThroughGateV2Repository();
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
            Ppg_EntryThroughGateV2Repository ObjGOR = new Ppg_EntryThroughGateV2Repository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGate(PpgEntryThroughGateV2 ObjEntryThroughGate)
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
                Ppg_EntryThroughGateV2Repository ObjEntryThroughGateRepositories = new Ppg_EntryThroughGateV2Repository();
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
                   // SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
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
        public ActionResult EntryThroughGateList(string ContainerType, string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughGateListPage(0, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughGateList(int Page, string lstFlag = "All", string OperationType = "", string ContainerType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughGateListPage(Page, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughTrainGateList(string ContainerType, string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughTrainListPage(0, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughTrainGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughTrainGateList(int Page, string lstFlag = "All", string ContainerType="", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughTrainListPage(Page, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EntryThroughGateSearchList(string ContainerNo, string ContainerType = "", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughGateSearchListPage(0, OperationType, ContainerType, ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
             if (OperationType.ToLower() == "Export")
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }
            else
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }

        }

        [HttpGet]
        public ActionResult EntryThroughGatetrainSearchList(string ContainerNo, string ContainerType = "", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            ObjETR.GetAllEntryThroughGateSearchTrainListPage(0, OperationType, ContainerType, ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<PpgEntryThroughGateV2>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            if (OperationType.ToLower() == "Export")
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }
            else
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }

        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();

            List<containerV2> Lstcontainer = new List<containerV2>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<containerV2>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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
            return PartialView("EditEntryThroughGate", ObjEntryGate);


        }

        [HttpGet]
        public ActionResult ViewEntryThroughGate(int EntryId)
        {


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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
        public ActionResult EditEntryThroughGateByTrain(int EntryId)
        {
            //--------------------------------------------------------------------------------
            Ppg_EntryThroughGateV2Repository ImpRep = new Ppg_EntryThroughGateV2Repository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModelV2> lstPick = new List<PPGPickupModelV2>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModelV2>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjETR = new Ppg_EntryThroughGateV2Repository();

            List<containerV2> Lstcontainer = new List<containerV2>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<containerV2>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjETR.DBResponse.Data;

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


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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
        public ActionResult ViewEntryThroughGateExport(int EntryId)
        {


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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

        [HttpGet]
        public ActionResult ViewEntryThroughGateExportTrain(int EntryId)
        {


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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



            return PartialView( ObjEntryGate);
        }

        [HttpGet]
        public ActionResult ViewEntryThroughGateLoadedContainer(int EntryId)
        {


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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



            return PartialView("ViewEntryThroughGateLoadedContainer", ObjEntryGate);
        }

        [HttpGet]
        public ActionResult ViewEntryThroughGateLoadedContainerTrain(int EntryId)
        {


            PpgEntryThroughGateV2 ObjEntryGate = new PpgEntryThroughGateV2();
            Ppg_EntryThroughGateV2Repository ObjEtGateR = new Ppg_EntryThroughGateV2Repository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (PpgEntryThroughGateV2)ObjEtGateR.DBResponse.Data;


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



            return PartialView( ObjEntryGate);
        }
        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteEntryThroughGate(int EntryId)
        {
            if (EntryId > 0)
            {
                Ppg_EntryThroughGateV2Repository ObjGOR = new Ppg_EntryThroughGateV2Repository();
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

                Ppg_EntryThroughGateV2Repository objEntry = new Ppg_EntryThroughGateV2Repository();
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
                        Ppg_EntryThroughGateRepository etgr = new Ppg_EntryThroughGateRepository();
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


        #endregion

        
    }
}