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

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class kdl_CWCGateOperationController : BaseController
    {
        #region Entry Through Gate kolkata



        [HttpGet]
        public ActionResult CreateEntryThroughGate(string ContainerName = "")
        {
            //if(ContainerName=="")
            //{
            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer("", 0);
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = ((SearchCont)ObjETR.DBResponse.Data).lstConatiner;
                ViewBag.Lstcontainer = Lstcontainer;
            }
            else
                ViewBag.Lstcontainer = null;
            EntryThroughGate objEntryThroughGate = new EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }

            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }

            return PartialView(objEntryThroughGate);
            //}
            //else
            //{
            //    EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
            //    ObjGOR.GetAutoPopulateData(ContainerName);
            //    //return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            //    //ViewBag.JSONResult = ObjGOR.DBResponse.Data;
            //    EntryThroughGate objEntryThroughGate = new EntryThroughGate();
            //    objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
            //    string strDate = objEntryThroughGate.ReferenceDate;
            //    string[] arrayDate = strDate.Split(' ');
            //    objEntryThroughGate.ReferenceDate = arrayDate[0];
            //    ViewBag.strTime = objEntryThroughGate.EntryTime;
            //    return PartialView(objEntryThroughGate);

            //}

        }


        public ActionResult CreateEntryThroughGateExport()
        {
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (ObjReferenceNumber.listExport != null && ObjReferenceNumber.ReferenceList.Count > 0)
                {
                    lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                if (ObjReferenceNumber.listExport != null && ObjReferenceNumber.listExport.Count > 0)
                {
                    lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                }
                else
                {
                    lstGateEntryExport = null;
                }
                if (ObjReferenceNumber.listShippingLine != null && ObjReferenceNumber.listShippingLine.Count > 0)
                {
                    lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                }
                else
                {
                    lstShippingLine = null;
                }
            }
            ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            EntryThroughGate objEntryThroughGate = new EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
            }

            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }
            return PartialView("CreateEntryThroughGateExport", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateExport(int EntryId)
        {

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

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            ReferenceNumber ObjReferenceNumber = new ReferenceNumber();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (ObjReferenceNumber.listExport != null && ObjReferenceNumber.ReferenceList.Count > 0)
                {
                    lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                if (ObjReferenceNumber.listExport != null && ObjReferenceNumber.listExport.Count > 0)
                {
                    lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                }
                else
                {
                    lstGateEntryExport = null;
                }
                if (ObjReferenceNumber.listShippingLine != null && ObjReferenceNumber.listShippingLine.Count > 0)
                {
                    lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                }
                else
                {
                    lstShippingLine = null;
                }
            }
            ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (EntryThroughGate)ObjETR.DBResponse.Data;

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
                }
            }


            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }

            return PartialView("EditEntryThroughGateExport", ObjEntryGate);


        }


        public ActionResult CreateEntryThroughGateBond()
        {
            List<BondReferenceNumberList> lstReferenceNumberList = new List<BondReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            // List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
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

            EntryThroughGate objEntryThroughGate = new EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                objEntryThroughGate.ShippingLine = "";
                objEntryThroughGate.ShippingLineId = 0;
            }
            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }
            return PartialView("CreateEntryThroughGateBond", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateBond(int EntryId)
        {
            List<BondReferenceNumberList> lstReferenceNumberList = new List<BondReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            // List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
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

            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (EntryThroughGate)ObjETR.DBResponse.Data;

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
                }
            }

            return PartialView("EditEntryThroughGateBond", ObjEntryGate);


        }



        public ActionResult CreateEntryThroughGateLoadContainer()
        {
            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
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
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstShippingLine != null && lstShippingLine.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
                }
                else
                {
                    lstShippingLine = null;
                }
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);


            EntryThroughGate objEntryThroughGate = new EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                objEntryThroughGate.ShippingLine = "";
                objEntryThroughGate.ShippingLineId = 0;
            }

            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }

            return PartialView("CreateEntryThroughGateLoadContainer", objEntryThroughGate);


        }



        public ActionResult EditEntryThroughGateLoadContainer(int EntryId)
        {
            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
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
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstShippingLine != null && lstShippingLine.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
                }
                else
                {
                    lstShippingLine = null;
                }
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            //  EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (EntryThroughGate)ObjETR.DBResponse.Data;

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
                }
            }

            ObjETR.GetEximtraderList("SL");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjETR.GetEximtraderList("CHA");
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.CHAList = ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.CHAList = null;
            }


            return PartialView("EditEntryThroughGateLoadContainer", ObjEntryGate);


        }
        public JsonResult GetFieldsForContainer(string ContainerName)
        {
            if (ContainerName != "")
            {
                EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjGOR.GetAutoPopulateData(ContainerName);
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
            EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGate(EntryThroughGate ObjEntryThroughGate)
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

            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime + " " + Entrytime;


            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;
            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");


            if (ObjEntryThroughGate.OperationType == "Export" && ObjEntryThroughGate.ContainerType == "Empty")
            {
                //ModelState.Remove("Size");
                //ModelState.Remove("ContainerLoadType");
                //ModelState.Remove("TransportFrom");
                ModelState.Remove("CargoType");
            }


            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
            if (ModelState.IsValid)
            {
                EntryThroughGateRepository ObjEntryThroughGateRepositories = new EntryThroughGateRepository();
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

                    //Only For PIL (Pacific International Lines (Pte) Ltd) and Import
                    if (ObjEntryThroughGate.ShippingLine.ToUpper().Equals("PIL") == true)
                    {
                        //SendEntryMailPIL(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                        SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                    }
                    else //For Other
                    {
                        SendEntryMail(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                    }

                }

                //Only For PIL (Pacific International Lines (Pte) Ltd) and Export
                /*if (ObjEntryThroughGateRepositories.DBResponse.Status == 1)
                {
                    int lastInsertedId = Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
                    if (ObjEntryThroughGate.OperationType.Equals("Export"))
                    {
                        if (ObjEntryThroughGate.ShippingLine.ToUpper().Equals("PIL"))
                        {
                            SendEntryMailPIL(ObjEntryThroughGate.ContainerNo, lastInsertedId);
                        }
                    }
                }*/



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
        public ActionResult EntryThroughGateList()
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            List<EntryThroughGate> LstEntryThroughGate = new List<EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<EntryThroughGate>)ObjETR.DBResponse.Data;
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }
        [HttpGet]
        public ActionResult EntryThroughGateListSearch(string ContainerNo)
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            List<EntryThroughGate> LstEntryThroughGate = new List<EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate_kdl(ContainerNo);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<EntryThroughGate>)ObjETR.DBResponse.Data;
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();

            List<container> Lstcontainer = new List<container>();
            /*ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;*/
            ViewBag.Lstcontainer = null;
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (EntryThroughGate)ObjETR.DBResponse.Data;

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
                }

                ObjETR.GetEximtraderList("SL");
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.ShippingLineList = ObjETR.DBResponse.Data;
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }

                ObjETR.GetEximtraderList("CHA");
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.CHAList = ObjETR.DBResponse.Data;
                }
                else
                {
                    ViewBag.CHAList = null;
                }
            }
            return PartialView("EditEntryThroughGateImport", ObjEntryGate);


        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddEditEntryThroughGate(EntryThroughGate ObjEntryGate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
        //        Login ObjLogin = (Login)Session["LoginUser"];
        //        ObjEntryGate.Uid=ObjLogin.Uid;
        //        ObjGOR.AddEditEntryThroughGate(ObjEntryGate);
        //        ModelState.Clear();
        //        return Json(ObjGOR.DBResponse);
        //    }
        //    else
        //    {
        //        var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e=>e.ErrorMessage));
        //        var Err = new { Status = 0, Message = ErrorMessage };
        //        return Json(Err);
        //    }
        //}

        [HttpGet]
        public ActionResult ViewEntryThroughGate(int EntryId)
        {
            //EntryThroughGate ObjEntryGate = new EntryThroughGate();
            ////if (EntryId > 0)
            ////{
            ////    GateOperationRepository ObjGOR = new GateOperationRepository();
            ////    ObjGOR.GetEntryThroughGate();
            ////    if (ObjGOR.DBResponse.Data != null)
            ////    {
            ////        ObjEntryGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
            ////    }
            ////}
            //return View(ObjEntryGate);

            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            EntryThroughGateRepository ObjEtGateR = new EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (EntryThroughGate)ObjEtGateR.DBResponse.Data;
                }
            }
            return PartialView("ViewEntryThroughGate", ObjEntryGate);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteEntryThroughGate(int EntryId)
        {
            if (EntryId > 0)
            {
                EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
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

                EntryThroughGateRepository objEntry = new EntryThroughGateRepository();
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
                        EntryThroughGateRepository etgr = new EntryThroughGateRepository();
                        etgr.EntryMailStatus(ContainerNo, lastInsertedId);
                        if (etgr.DBResponse.Status == 1)
                        {
                            message = "Email Status Updated";
                        }
                        //else
                        //{
                        //    string FolderPath1 = Server.MapPath("~/Uploads/EntryEmailError/" + CuurDate);
                        //    if (!System.IO.Directory.Exists(FolderPath1))
                        //    {
                        //        System.IO.Directory.CreateDirectory(FolderPath1);



                        //    }
                        //    file = Path.Combine(FolderPath1, time + "_ErrorEntryEmail.txt");
                        //    string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                        //    using (var tw = new StreamWriter(file, true))
                        //    {
                        //        tw.WriteLine("For Container No:" + ContainerNo + " Email not Sent To " + MailIds);
                        //        tw.Close();
                        //    }


                        //    message = "Email Status Not Updated";
                        //}
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

        [NonAction]
        public int SendEntryMailPIL(string ContainerNo, int lastInsertedId)
        {
            try
            {
                string message = "";
                var file = (dynamic)null;

                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                EntryThroughGateRepository objEntry = new EntryThroughGateRepository();
                objEntry.GetDetailsForGateEntryMailPIL(ContainerNo);
                if (objEntry.DBResponse.Data != null)
                {
                    var mailTo = ((EntryThroughGateMailPIL)objEntry.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j = 0; j < mailTo.Length; j++)
                    {
                        mailTo[j].Trim();
                    }

                    //string[] ReceiverEmailArray= ObjEmailDataModel.ReceiverEmail.s
                    var FileName = ((EntryThroughGateMailPIL)objEntry.DBResponse.Data).FileName;
                    //ObjEmailDataModel.Subject = " Container Entered Through Gate";
                    //ObjEmailDataModel.MailBody = "Container Number : "+ ContainerNo+" ,Entred Through Gate";
                    var excelData = ((EntryThroughGateMailPIL)objEntry.DBResponse.Data).lstExcelData;
                    var excelString = string.Empty; ;

                    excelData.ToList().ForEach(item =>
                    {
                        excelString += item.Line + Environment.NewLine;
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
                        EntryThroughGateRepository etgr = new EntryThroughGateRepository();
                        etgr.EntryMailStatus(ContainerNo, lastInsertedId);
                        if (etgr.DBResponse.Status == 1)
                        {
                            message = "Email Status Updated";
                        }
                        //else
                        //{
                        //    string FolderPath1 = Server.MapPath("~/Uploads/EntryEmailError/" + CuurDate);
                        //    if (!System.IO.Directory.Exists(FolderPath1))
                        //    {
                        //        System.IO.Directory.CreateDirectory(FolderPath1);



                        //    }
                        //    file = Path.Combine(FolderPath1, time + "_ErrorEntryEmail.txt");
                        //    string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                        //    using (var tw = new StreamWriter(file, true))
                        //    {
                        //        tw.WriteLine("For Container No:" + ContainerNo + " Email not Sent To " + MailIds);
                        //        tw.Close();
                        //    }


                        //    message = "Email Status Not Updated";
                        //}
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
        public JsonResult GetContainer(string Search, int Skip)
        {
            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            //List<container> Lstcontainer = new List<container>();
            SearchCont objSC = new SearchCont();
            ObjETR.GetContainer(Search, Skip);
            if (ObjETR.DBResponse.Data != null)
            {
                /*Lstcontainer = ((SearchCont)ObjETR.DBResponse.Data).lstConatiner;
                Search= ((SearchCont)ObjETR.DBResponse.Data).State;*/
                objSC = (SearchCont)ObjETR.DBResponse.Data;
            }
            return Json(objSC, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
            List<EntryThroughGate> LstEntryThroughGate = new List<EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate(Page);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<EntryThroughGate>)ObjETR.DBResponse.Data;
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }



        #endregion

    }
}