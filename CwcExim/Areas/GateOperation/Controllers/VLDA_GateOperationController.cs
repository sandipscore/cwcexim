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
using System.Text;
using CwcExim.Areas.Export.Models;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class VLDA_GateOperationController : Controller
    {
        #region Entry Through Gate VLDA

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }

        [HttpGet]
        public ActionResult CreateEntryThroughGate(int TransportMode = 2)
        {

            AccessRightsRepository ACCR = new AccessRightsRepository();
            //    ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            //   ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            List<WFLDPickupModel> lstPick = new List<WFLDPickupModel>();
            /*if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }*/
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //// picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = "OTH", Value = "O" });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            //ObjETR.ListOfTransPort();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    ViewBag.txtListOfTransPortFrom = Newtonsoft.Json.JsonConvert.SerializeObject(ObjETR.DBResponse.Data);

            //}
            //else
            //{
            //    ViewBag.txtListOfTransPortFrom = null;
            //}

            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
            //  ObjETR.ListOfTransPort();
            //  if (ObjETR.DBResponse.Data != null)
            //  {
            //     LstPort = (List<Port>)ObjETR.DBResponse.Data;
            //  }
            //  ViewBag.LstTransport = LstPort;
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_EntryThroughGateRepository objImport = new VLDA_EntryThroughGateRepository();
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
        public ActionResult ViewEntryThroughGateBond(int EntryId)
        {
            Wfld_EntryThroughGateBond ObjEntryGate = new Wfld_EntryThroughGateBond();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjEtGateR.GetEntryThroughGateBond(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (Wfld_EntryThroughGateBond)ObjEtGateR.DBResponse.Data;
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
            VLDA_ImportRepository objRepo = new VLDA_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            VLDA_ImportRepository objRepo = new VLDA_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            VLDA_ImportRepository objRepo = new VLDA_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            VLDA_ImportRepository objRepo = new VLDA_ImportRepository();
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_EntryThroughGateRepository objImport = new VLDA_EntryThroughGateRepository();

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
            //     ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            //  ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_EntryThroughGateRepository objImport = new VLDA_EntryThroughGateRepository();
            //   ObjETR.ListOfTransPort();
            // if (ObjETR.DBResponse.Data != null)
            //   {
            //       LstPort = (List<Port>)ObjETR.DBResponse.Data;
            //   }
            //    ViewBag.LstTransport = LstPort;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_EntryThroughGateRepository objImport = new VLDA_EntryThroughGateRepository();

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}


            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            // List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            // List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // // List<container> Lstcontainer = new List<container>();
            // ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            // List<Port> LstPort = new List<Port>();
            //ObjETR.GetReferenceNumber();
            // if (ObjETR.DBResponse.Data != null)
            // {
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //     ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            // }
            // if (ObjReferenceNumber.ReferenceList.Count > 0)
            // {
            //     ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            // }
            // else
            // {
            //    ViewBag.lstReferenceNumberList = null;

            // }



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
            ObjETR.ListOfForwarderForPage("", 0);
            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstFwd = Jobject["lstFwd"];
                ViewBag.FwdState = Jobject["State"];
            }
            else
            {
                ViewBag.lstFwd = null;
            }
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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


        [HttpGet]
        public JsonResult GetRefNoExport()
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            ObjETR.GetReferenceNumberforExportCBT();

            if (ObjETR.DBResponse.Data != null)
                return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }



        [HttpGet]
        public JsonResult GetPortInfoExport()
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            ObjETR.ListOfTransPort();

            if (ObjETR.DBResponse.Data != null)
                return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        [HttpGet]
        public JsonResult GetRefNoDetExport(int Id)
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            //  ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            ObjETR.GetReferenceNumberDetforExportCBT(Id);

            if (ObjETR.DBResponse.Data != null)
                return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        public ActionResult EditEntryThroughGateBond(int EntryId)
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            Wfld_EntryThroughGateBond ObjEntryGate = new Wfld_EntryThroughGateBond();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateBond(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (Wfld_EntryThroughGateBond)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            List<Port> LstPort = new List<Port>();
            if (OpType != "Export")
            {
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
            }

            else
            {
                ViewBag.lstReferenceNumberList = null;

            }

            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
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
            ObjETR.ListOfForwarderForPage("", 0);
            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstFwd = Jobject["lstFwd"];
                ViewBag.FwdState = Jobject["State"];
            }
            else
            {
                ViewBag.lstFwd = null;
            }
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<Port> LstPort = new List<Port>();
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
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
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

            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
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
            List<Port> LstPort = new List<Port>();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
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

            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            string ListOfCCIN = "";
            string check1 = string.Empty;
            //   List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            //   List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            //   List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
            //   ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();
            //   ObjETR.GetReferenceNumber();
            //   if (ObjETR.DBResponse.Data != null)
            //  {
            //    ObjReferenceNumber.ReferenceList = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).ReferenceList;


            //  ObjReferenceNumber.listShippingLine = ((ReferenceNumberCCIN)ObjETR.DBResponse.Data).listShippingLine;


            //  }
            //if (ObjReferenceNumber.ReferenceList.Count > 0)
            //  {
            //     ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(ObjReferenceNumber.ReferenceList);
            // }
            // else
            //  {
            //      ViewBag.lstReferenceNumberList = null;

            //  }
            //   ObjETR.ListOfTransPort();
            //   if (ObjETR.DBResponse.Data != null)
            //  {
            //      LstPort = (List<Port>)ObjETR.DBResponse.Data;
            // }
            //  ViewBag.LstTransport = LstPort;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            VLDA_EntryThroughGateRepository objImportFor = new VLDA_EntryThroughGateRepository();
            objImportFor.ListOfForwarderForPage("", 0);

            if (objImportFor.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImportFor.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstFwd"];
                ViewBag.CHAState = Jobject["State"];
            }

            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            VLDA_EntryThroughGateRepository objImportFor = new VLDA_EntryThroughGateRepository();
            objImportFor.ListOfForwarderForPage("", 0);

            if (objImportFor.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImportFor.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstFwd"];
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
        public ActionResult EditEntryThroughGateExportCBTEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            VLDA_EntryThroughGateRepository objImportFor = new VLDA_EntryThroughGateRepository();
            objImportFor.ListOfForwarderForPage("", 0);

            if (objImportFor.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImportFor.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstFwd"];
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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

            return PartialView("EditEntryThroughGateExportCBTEmpty", ObjEntryGate);


        }
        public ActionResult EditEntryThroughGateExportCBTVehicle(int EntryId)
        {
            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            VLDA_EntryThroughGateRepository objImportFor = new VLDA_EntryThroughGateRepository();
            objImportFor.ListOfForwarderForPage("", 0);

            if (objImportFor.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImportFor.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstFwd"];
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
            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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

            return PartialView("EditEntryThroughGateExportCBTVehicle", ObjEntryGate);


        }
        //public ActionResult EditEntryThroughGateExportCBT(int EntryId)
        //{
        //    //--------------------------------------------------------------------------------
        //    Ppg_ImportRepository ImpRep = new Ppg_ImportRepository();
        //    ImpRep.GetAllPickupLocation();
        //    List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
        //    if (ImpRep.DBResponse.Data != null)
        //    {
        //        lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

        //    VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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
        //    WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
        //    ObjETR.GetTime();
        //    if (ObjETR.DBResponse.Data != null)
        //    {
        //        objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
        //        string ExitTime = objEntryThroughGate.EntryDateTime;
        //        string[] ExitTimeArray = ExitTime.Split(' ');
        //        objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
        //        ViewBag.strTime = objEntryThroughGate.Time;
        //    }
        //    WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
        //    //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
        //    if (EntryId > 0)
        //    {
        //        ObjETR.GetEntryThroughGate(EntryId);
        //        if (ObjETR.DBResponse.Data != null)
        //        {
        //            ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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

            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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

            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }
            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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


            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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


            WfldEntryThroughGate objEntryThroughGate = new WfldEntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;
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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
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

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
                VLDA_EntryThroughGateRepository ObjGOR = new VLDA_EntryThroughGateRepository();
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
            VLDA_EntryThroughGateRepository ObjGOR = new VLDA_EntryThroughGateRepository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateCBT(WfldEntryThroughGate ObjEntryThroughGate)

        {
            if (ObjEntryThroughGate.OperationType == "Export" || ObjEntryThroughGate.OperationType == "Import")
            {
                if (ObjEntryThroughGate.ContainerType != "Empty")
                {
                    ModelState.Remove("Size");
                    ModelState.Remove("CHAName");
                    ModelState.Remove("ContainerNo1");


                }
            }


            ModelState.Remove("ShippingLine");
            ModelState.Remove("ShippingLineId");
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1.ToUpper();
                ObjEntryThroughGate.ContainerNo1 = ObjEntryThroughGate.ContainerNo1.ToUpper();
            }
            ObjEntryThroughGate.VehicleNo = ObjEntryThroughGate.VehicleNo.ToUpper();
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
            if (SysTime != null && SysTime != "")
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
                //ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }
            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.ContainerType == "Empty")
            {
                ModelState.Remove("Size");
                ModelState.Remove("ContainerLoadType");
                ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }
            if (ObjEntryThroughGate.IsCBT == true)
            {
                //  ModelState.Remove("Size");

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
                VLDA_EntryThroughGateRepository ObjEntryThroughGateRepositories = new VLDA_EntryThroughGateRepository();
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
        public ActionResult AddEditEntryThroughGate(WfldEntryThroughGate ObjEntryThroughGate)
        {

            ModelState.Remove("ContainerNo1");
            ModelState.Remove("CHAName");
            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1.ToUpper();
                ObjEntryThroughGate.ContainerNo1 = ObjEntryThroughGate.ContainerNo1.ToUpper();
            }
            ObjEntryThroughGate.VehicleNo = ObjEntryThroughGate.VehicleNo.ToUpper();
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
            if (ObjEntryThroughGate.OperationType == "Import" && ObjEntryThroughGate.ContainerType == "Empty")
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
            if (ObjEntryThroughGate.ContainerType == "Empty" && ObjEntryThroughGate.OperationType == "Import")
            {
                ModelState.Remove("ContainerNo");
            }
            else
            {
                if (ObjEntryThroughGate.ContainerNo != null)
                {
                    if (!IsValidContainerNo(ObjEntryThroughGate.ContainerNo))
                    {
                        ModelState.AddModelError("ContainerNo", "Container No should be 4 alpha characters and 7 numeric characters.");
                    }
                }
                else
                {

                    ModelState.AddModelError("ContainerNo", "Container No should be 4 alpha characters and 7 numeric characters.");
                }

            }

            if (ModelState.IsValid)
            {
                VLDA_EntryThroughGateRepository ObjEntryThroughGateRepositories = new VLDA_EntryThroughGateRepository();
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
        public ActionResult EntryThroughGateList(string ContainerType = "", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(0, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            if (OperationType.ToLower() == "bond")
            {
                return PartialView("EntryThroughGateBondList", LstEntryThroughGate);
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

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateListPage(Page, OperationType, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughTrainGateList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughTrainGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughTrainGateList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughTrainListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EntryThroughGateSearchList(string ContainerNo, string ContainerType = "", string lstFlag = "All", string OperationType = "")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateSearchListPage(0, OperationType, ContainerType, ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            if (OperationType.ToLower() == "bond")
            {
                return PartialView("EntryThroughGateBondList", LstEntryThroughGate);
            }
            else if (OperationType.ToLower() == "Export")
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }
            else
            {
                return PartialView("EntryThroughGateList", LstEntryThroughGate);
            }

        }
        [HttpGet]
        public ActionResult EntryThroughGateCBTSearchList(string ContainerNo, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTSearchListPage(0, ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }

            return PartialView("EntryThroughCBTList", LstEntryThroughGate);
        }
        [HttpGet]
        public ActionResult EntryThroughEmptyList(string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(0, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughEmptyList", LstEntryThroughGate);
        }

        [HttpGet]
        public JsonResult LoadMoreEntryThroughEmptyList(int Page, string ContainerType, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateEmptyListPage(Page, ContainerType);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EntryThroughCBTList(string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(0);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return PartialView("EntryThroughCBTList", LstEntryThroughGate);
        }

        public JsonResult LoadMoreEntryThroughCBTList(int Page, string lstFlag = "All")
        {
            TempData["lstFlag"] = lstFlag;
            TempData.Keep();

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<WfldEntryThroughGate> LstEntryThroughGate = new List<WfldEntryThroughGate>();
            ObjETR.GetAllEntryThroughGateCBTListPage(Page);
            if (ObjETR.DBResponse.Data != null)
            {

                if (lstFlag == "All")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data);
                }
                else if (lstFlag == "Container")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode != "").ToList();
                }
                else if (lstFlag == "Vehicle")
                {
                    LstEntryThroughGate = ((List<WfldEntryThroughGate>)ObjETR.DBResponse.Data).Where(x => x.CFSCode == "").ToList();
                }
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();

            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            ObjETR.ListOfTransPort();
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjETR.DBResponse.Data;
            }
            ViewBag.LstTransport = LstPort;
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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


            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

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
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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


            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
                    ObjETR.ListOfTransPort();
                    if (ObjETR.DBResponse.Data != null)
                    {
                        LstPort = (List<Port>)ObjETR.DBResponse.Data;
                    }
                    ViewBag.LstTransport = LstPort;
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
        public ActionResult EditEntryThroughCBTImportEmpty(int EntryId)
        {
            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
                    ObjETR.ListOfTransPort();
                    if (ObjETR.DBResponse.Data != null)
                    {
                        LstPort = (List<Port>)ObjETR.DBResponse.Data;
                    }
                    ViewBag.LstTransport = LstPort;
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
            return PartialView("EditEntryThroughCBTImportEmpty", ObjEntryGate);


        }
        [HttpGet]
        public ActionResult EditEntryThroughCBTImportVehicle(int EntryId)
        {
            //--------------------------------------------------------------------------------
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            //ImpRep.GetAllPickupLocation();
            //List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            //if (ImpRep.DBResponse.Data != null)
            //{
            //    lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            //}
            //List<SelectListItem> picuplists = new List<SelectListItem>();
            //picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            //foreach (var x in lstPick)
            //{
            //    picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            //}

            //ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            List<Port> LstPort = new List<Port>();
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
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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
                    ObjETR.ListOfTransPort();
                    if (ObjETR.DBResponse.Data != null)
                    {
                        LstPort = (List<Port>)ObjETR.DBResponse.Data;
                    }
                    ViewBag.LstTransport = LstPort;
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
            return PartialView("EditEntryThroughCBTImportVehicle", ObjEntryGate);


        }
        [HttpGet]
        public ActionResult ViewEntryThroughCBT(int EntryId)
        {


            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
            VLDA_ImportRepository ImpRep = new VLDA_ImportRepository();
            ImpRep.GetAllPickupLocation();
            List<PPGPickupModel> lstPick = new List<PPGPickupModel>();
            if (ImpRep.DBResponse.Data != null)
            {
                lstPick = (List<PPGPickupModel>)ImpRep.DBResponse.Data;
            }
            List<SelectListItem> picuplists = new List<SelectListItem>();
            picuplists.Add(new SelectListItem { Text = "----Select----", Value = "" });
            foreach (var x in lstPick)
            {
                picuplists.Add(new SelectListItem { Text = x.pickup_location, Value = x.alias });
            }

            ViewBag.Lstpickup = picuplists;
            //--------------------------------------------------------------------------------

            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

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
                    ObjEntryGate = (WfldEntryThroughGate)ObjETR.DBResponse.Data;

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


            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjEtGateR.DBResponse.Data;


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
                VLDA_EntryThroughGateRepository ObjGOR = new VLDA_EntryThroughGateRepository();
                ObjGOR.DeleteEntryThroughGate(EntryId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintGateEntryDet(int EntryId, string OperationType, string ContainerType)
        {
            VLDA_EntryThroughGateRepository ObjGOR = new VLDA_EntryThroughGateRepository();
            ObjGOR.GetEntryThroughGate(EntryId);           
            
            WfldEntryThroughGate objGP = new WfldEntryThroughGate();
            string FilePath = "";
            if (ObjGOR.DBResponse.Data != null)
            {
                objGP = (WfldEntryThroughGate)ObjGOR.DBResponse.Data;
                if (OperationType == "Import" && ContainerType == "Loaded")
                {
                    FilePath = GeneratingPDFImport(objGP, EntryId);
                }
                else if (OperationType == "Export" && ContainerType == "Empty")
                {
                    FilePath = GeneratingPDFExport(objGP, EntryId);
                }
                else if (OperationType == "Export" && ContainerType == "Loaded")
                {
                    FilePath = GeneratingPDFExportLoad(objGP, EntryId);
                }
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        public string GeneratingPDFImport(WfldEntryThroughGate objGP, int EntryId)
        {
            var FileName = "";
            var location = "";

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<string> lstSB = new List<string>();

            StringBuilder html = new StringBuilder();
            /*Header Part*/

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='85%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>ADANI FORWARDING AGENT PRIVATE LIMITED</h1><label style='font-size: 7pt;'>Survey No. 44/1, 44/1/2, Village Tumb, Taluka Umbergaon Village, Dist. Valsad, Gujarat, Pin:396150</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'><u>IMPORT-GATE IN</u></span><br/><label style='font-size: 7pt;'><u>Loaded Container In</u></label></td>");
            html.Append("</tr>");
            html.Append("</tbody></table></td>");

            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; font-size:7pt;'><tbody>");

            html.Append("<tr><td colspan='12'><table style='border-top: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
            html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Gate Pass No</th><th>:</th><td colspan='6' width='70%'>"+ objGP.GateInNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Shipping Line</th><th>:</th><td colspan='6' width='70%'>" + objGP.ShippingLine + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("<td colspan='5' width='40%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Gate Pass Date</th><th>:</th><td colspan='6' width='50%'>" + objGP.EntryDateTime + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Vehicle No.</th><th>:</th><td colspan='6' width='50%'>" + objGP.VehicleNo + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; width:50px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Type</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Status</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Seal No</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Seal No (II)</th>");
            html.Append("<th style='width:150px;'>Remarks</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/

            html.Append("<tr><td style='border-right: 1px solid #000; border-top: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ContainerNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Size + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>"+ objGP.Type +"</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ContainerLoadType + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ShippingLineSealNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'></td>");
            html.Append("<td style='border-top: 1px solid #000;'>" + objGP.Remarks + "</td>");
            html.Append("</tr>");

            /***************/
            html.Append("</tbody></table></td></tr>");

            html.Append("<tr><th style='font-size:7pt;' colspan='6' width='50%'>Bay No: ______________________________</th>");
            html.Append("<th style='font-size:7pt;' colspan='6' width='50%' align='right'>ADANI FORWARDING AGENT PRIVATE LIMITED</th></tr>");

            html.Append("<tr><th><br/><br/></th></tr>");

            html.Append("<tr><th style='font-size:7pt;' colspan='12' width='100%' align='right'>Surveyor's Name & Sign</th></tr>");

            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            /***************/

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/ADANIpdf.PNG"));            
            lstSB.Add(html.ToString());


            FileName = "IMPORT-GATEIN" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }



            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingPDFExport(WfldEntryThroughGate objGP, int EntryId)
        {
            var FileName = "";
            var location = "";

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<string> lstSB = new List<string>();

            StringBuilder html = new StringBuilder();
            /*Header Part*/

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='85%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>ADANI FORWARDING AGENT PRIVATE LIMITED</h1><label style='font-size: 7pt;'>Survey No. 44/1, 44/1/2, Village Tumb, Taluka Umbergaon Village, Dist. Valsad, Gujarat, Pin:396150</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'><u>GATE IN</u></span><br/><label style='font-size: 7pt;'><u>Export Empty Container</u></label></td>");
            html.Append("</tr>");
            html.Append("</tbody></table></td>");

            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; font-size:7pt;'><tbody>");

            html.Append("<tr><td colspan='12'><table style='border-top: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
            html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Gate Pass No</th><th>:</th><td colspan='6' width='70%'>" + objGP.GateInNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>On Account</th><th>:</th><td colspan='6' width='70%'>" + objGP.OnAccount + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'></th><th></th><td colspan='6' width='70%'></td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Shipping Line</th><th>:</th><td colspan='6' width='70%'>" + objGP.ShippingLine + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("<td colspan='5' width='40%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Gate Pass Date</th><th>:</th><td colspan='6' width='50%'>" + objGP.EntryDateTime + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Vehicle No.</th><th>:</th><td colspan='6' width='50%'>" + objGP.VehicleNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Transporter</th><th>:</th><td colspan='6' width='50%'>" + objGP.Transpoter + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>BK NO</th><th>:</th><td colspan='6' width='50%'>" + objGP.BKNO + "</td></tr>");
            
            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; width:50px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Type</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Status</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Tare Wt.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Seal No</th>");            
            html.Append("<th style='width:150px;'>Remarks</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/

            html.Append("<tr><td style='border-right: 1px solid #000; border-top: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ContainerNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Size + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Type + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>Empty</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.TareWeight + "</td>");
            //html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ShippingLineSealNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.SealNo + "</td>");
            html.Append("<td style='border-top: 1px solid #000;'>" + objGP.Remarks + "</td>");
            html.Append("</tr>");

            /***************/
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><th style='font-size:7pt;' colspan='6' width='50%'></th>");
            html.Append("<th style='font-size:7pt;' colspan='6' width='50%' align='right'>ADANI FORWARDING AGENT PRIVATE LIMITED</th></tr>");

            html.Append("<tr><th><br/><br/></th></tr>");

            html.Append("<tr><th style='font-size:7pt;' colspan='12' width='100%' align='right'>Surveyor's Name & Sign</th></tr>");

            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            /***************/

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/ADANIpdf.PNG"));
            lstSB.Add(html.ToString());


            FileName = "EXPORT-GATEIN" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }



            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingPDFExportLoad(WfldEntryThroughGate objGP, int EntryId)
        {
            var FileName = "";
            var location = "";

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<string> lstSB = new List<string>();

            StringBuilder html = new StringBuilder();
            /*Header Part*/

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='85%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>ADANI FORWARDING AGENT PRIVATE LIMITED</h1><label style='font-size: 7pt;'>Survey No. 44/1, 44/1/2, Village Tumb, Taluka Umbergaon Village, Dist. Valsad, Gujarat, Pin:396150</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'><u>Export Loaded Out-Gate Pass</u></span><br/><label style='font-size: 7pt;'><u>Factory Stuffed </u></label></td>");
            html.Append("</tr>");
            html.Append("</tbody></table></td>");

            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; font-size:7pt;'><tbody>");

            html.Append("<tr><td colspan='12'><table style='border-top: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
            html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Gate Pass No</th><th>:</th><td colspan='6' width='70%'>" + objGP.GateInNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Vehicle No.</th><th>:</th><td colspan='6' width='70%'>" + objGP.VehicleNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Transporter</th><th>:</th><td colspan='6' width='70%'>" + objGP.Transpoter + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>A/C Holder</th><th>:</th><td colspan='6' width='70%'>" + objGP.Accountholder + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Exporter</th><th>:</th><td colspan='6' width='70%'>" + objGP.Exporter + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>CHA Name</th><th>:</th><td colspan='6' width='70%'>" + objGP.CHAName + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Driver</th><th>:</th><td colspan='6' width='70%'>" + objGP.Driver + "</td></tr>");
            if (objGP.TransportMode==2)
            {
                html.Append("<tr><th colspan='6' width='30%'>Transport Type</th><th>:</th><td colspan='6' width='70%'>By Road</td></tr>");
            }
            else 
            {
                html.Append("<tr><th colspan='6' width='30%'>Transport Type</th><th>:</th><td colspan='6' width='70%'></td></tr>");
            }
           
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("<td colspan='5' width='40%' style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Gate Pass Date</th><th>:</th><td colspan='6' width='50%'>" + objGP.EntryDateTime + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Vessel</th><th>:</th><td colspan='6' width='50%'>" + objGP.Vessel + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Via No</th><th>:</th><td colspan='6' width='50%'>" + objGP.ViaNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>POD</th><th>:</th><td colspan='6' width='50%'>" + objGP.POD + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>POL</th><th>:</th><td colspan='6' width='50%'>" + objGP.POL + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>SB No</th><th>:</th><td colspan='6' width='50%'>" + objGP.SBNO + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'></th><th></th><td colspan='6' width='50%'></td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Train No</th><th>:</th><td colspan='6' width='50%'>" + objGP.TrainNo + "</td></tr>");

            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; width:50px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Type</th>");
            html.Append("<th style='border-right: 1px solid #000; width:80px;'>Carge Type</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Agent Seal</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Custom Seal</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>PKGS</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Net Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Gross Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; width:100px;'>Shipping Line</th>");            
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/

            html.Append("<tr><td style='border-right: 1px solid #000; border-top: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ContainerNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Size + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Type + "</td>");
            if (objGP.CargoType == 1)
            {
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>HAZ</td>");
            }
            else if (objGP.CargoType == 2)
            {
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>NON-HAZ</td>");
            }
            else
            {
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>ODC</td>");
            }
           
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.AgentSeal + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.CustomSealNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.NoOfPackages + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.Netweight + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.GrossWeight + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + objGP.ShippingLine + "</td>");            
            html.Append("</tr>");

            /***************/
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><th><br/><br/><br/></th></tr>");
            html.Append("<tr><th style='font-size:7pt; border-bottom:1px solid #000; text-align:center;' colspan='3' width='30%'>NCL Executive Name & Sign</th>");
            html.Append("<th colspan='5' width='60%'></th>");
            html.Append("<th style='font-size:7pt; border-bottom:1px solid #000; text-align:center;' colspan='3' width='30%' align='right'>Surveyor's Name & Sign</th></tr>");

            html.Append("<tr><th><br/><br/></th></tr>");

            

            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            /***************/

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/ADANIpdf.PNG"));
            lstSB.Add(html.ToString());


            FileName = "EXPORT-GATEIN" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }



            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
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

                VLDA_EntryThroughGateRepository objEntry = new VLDA_EntryThroughGateRepository();
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

                    excelData.ToList().ForEach(item =>
                    {
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
                        VLDA_EntryThroughGateRepository etgr = new VLDA_EntryThroughGateRepository();
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

            VLDA_EntryThroughGateRepository ObjDR = new VLDA_EntryThroughGateRepository();
            ObjDR.GetDetForExportAddVehicle(EntryId);
            WFLD_AddExportVehicle objEV = new WFLD_AddExportVehicle();
            if (ObjDR.DBResponse.Data != null)
                objEV = (WFLD_AddExportVehicle)ObjDR.DBResponse.Data;
            return PartialView(objEV);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateVehicle(WFLD_AddExportVehicle ObjEntryThroughGate)
        {

            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            ModelState.Remove("ExportNoOfPkg");
            ModelState.Remove("ExportGrWeight");
            if (ModelState.IsValid)
            {
                VLDA_EntryThroughGateRepository ObjEntryThroughGateRepositories = new VLDA_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
                ObjEntryThroughGate.ExportVehicleNo = ObjEntryThroughGate.ExportVehicleNo.ToUpper();

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
            VLDA_EntryThroughGateRepository ObjDR = new VLDA_EntryThroughGateRepository();
            ObjDR.GetVehicleDtlbyEntryId(EntryId);
            List<WFLD_AddExportVehicle> LstVehicle = new List<WFLD_AddExportVehicle>();
            if (ObjDR.DBResponse.Data != null)
                LstVehicle = (List<WFLD_AddExportVehicle>)ObjDR.DBResponse.Data;
            return PartialView("ExportVehicleList", LstVehicle);
        }
        [HttpGet]
        public JsonResult SearchForwarderByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfForwarderForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadForwarderList(string PartyCode, int Page)
        {
            VLDA_EntryThroughGateRepository ObjDR = new VLDA_EntryThroughGateRepository();
            ObjDR.ListOfForwarderForPage(PartyCode, Page);
            return Json(ObjDR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughExportGate(int EntryId)
        {


            WfldEntryThroughGate ObjEntryGate = new WfldEntryThroughGate();
            VLDA_EntryThroughGateRepository ObjEtGateR = new VLDA_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (WfldEntryThroughGate)ObjEtGateR.DBResponse.Data;


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



            return PartialView("ViewEntryThroughExportGate", ObjEntryGate);
        }
        #endregion

        public ActionResult CreateEntryThroughGateBond()
        {

            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();


            Wfld_EntryThroughGateBond objEntryThroughGate = new Wfld_EntryThroughGateBond();
            ObjETR.GetBondTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (Wfld_EntryThroughGateBond)ObjETR.DBResponse.Data;
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
        public ActionResult AddEditEntryThroughGateBond(Wfld_EntryThroughGateBond ObjEntryThroughGate)
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
                VLDA_EntryThroughGateRepository ObjEntryThroughGateRepositories = new VLDA_EntryThroughGateRepository();
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
        public JsonResult GetBondNoforBondEntry()
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            // ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetBondRefNUmber();

            return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSACDetails(int Id)
        {
            VLDA_EntryThroughGateRepository obj = new VLDA_EntryThroughGateRepository();
            obj.SACDetails(Id);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public bool IsValidContainerNo(string ContainerNo)
        {
            return ContainerNo.Length == 11 &&
                    ContainerNo.Count(char.IsNumber) == 7 &&
                    ContainerNo.Count(char.IsLetter) == 4;
        }


        #region Lorry Recipt

        public ActionResult CreateLorryReceipt()
         {
           VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            VLDA_LorryReceipt ObjSRtime = new VLDA_LorryReceipt();
            ObjSRtime.LR_DATE = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            return PartialView("CreateLorryReceipt", ObjSRtime);
        }
        [HttpGet]
        public JsonResult GetPartyNameList(int Page, string PartyCode)
        {
            VLDA_EntryThroughGateRepository objRepo = new VLDA_EntryThroughGateRepository();
            objRepo.GetPartyNameList(Page,PartyCode);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetShippingLineList()
        {
            VLDA_EntryThroughGateRepository objRepo = new VLDA_EntryThroughGateRepository();
          
            objRepo.GetShippingLineForLorryReceipt();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCreateLorryReceipt(VLDA_LorryReceipt objLR )
        {
            VLDA_EntryThroughGateRepository objRepo = new VLDA_EntryThroughGateRepository();
            List<VLDA_LorryReceiptDtl> lstLorryDtl = new List<VLDA_LorryReceiptDtl>();
            lstLorryDtl= Newtonsoft.Json.JsonConvert.DeserializeObject<List<VLDA_LorryReceiptDtl>>(objLR.lstLorryDtl);
            string XmlLorryDtl = Utility.CreateXML(lstLorryDtl);
            objLR.Uid = ((Login)Session["LoginUser"]).Uid;
            objRepo.AddEditLorryReceipt(objLR, XmlLorryDtl);

            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllLorryReceiptDetails()
        {         
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
            List<VLDA_LorryReceipt> lstLorryReceipt = new List<VLDA_LorryReceipt>();
            ObjETR.GetAllLorryReceiptDetails();
            if (ObjETR.DBResponse.Data != null)
            {
                lstLorryReceipt = ((List<VLDA_LorryReceipt>)ObjETR.DBResponse.Data);
                           
               
            }
            return PartialView("EntryLorryReceiptList", lstLorryReceipt);
        }
        [HttpGet]
        public ActionResult GetLorryReceiptForEdit(int LorryId)
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();
           
            VLDA_LorryReceipt objLR = new VLDA_LorryReceipt();
            ObjETR.GetLorryReceiptForEdit(LorryId);
            if (ObjETR.DBResponse.Data != null)
            {
                objLR = ((VLDA_LorryReceipt)ObjETR.DBResponse.Data);

                ViewBag.LorryReceiptDetails = objLR.lstLRDtl;
            }
            return PartialView("EditLorryReceipt",  objLR);
        }
        [HttpGet]
        public ActionResult GetLorryReceiptForView(int LorryId)
        {
            VLDA_EntryThroughGateRepository ObjETR = new VLDA_EntryThroughGateRepository();

            VLDA_LorryReceipt objLR = new VLDA_LorryReceipt();
            ObjETR.GetLorryReceiptForEdit(LorryId);
            if (ObjETR.DBResponse.Data != null)
            {
                objLR = ((VLDA_LorryReceipt)ObjETR.DBResponse.Data);
            }
            return PartialView("ViewLorryReceipt", objLR);
        }
        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteLorryReceipt(int LorryId)
        {
            if (LorryId > 0)
            {
                VLDA_EntryThroughGateRepository ObjGOR = new VLDA_EntryThroughGateRepository();
                ObjGOR.DeleteLorryReceipt(LorryId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }


        #endregion
    }
}