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
using CwcExim.Areas.Bond.Models;


namespace CwcExim.Areas.GateOperation.Controllers
{
    public class HDB_CWCGateOperationController : BaseController
    {
        #region Entry Through Gate Hyderabad 

       

        [HttpGet]
        public ActionResult CreateEntryThroughGate(string ContainerName="")
        {
            //if(ContainerName=="")
            //{
                HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
                //List<container> Lstcontainer = new List<container>();
                //ObjETR.GetContainer();
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
                //}
                //ViewBag.Lstcontainer = Lstcontainer;
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            //ObjIR.ListOfCHAName();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CwcExim.Areas.Import.Models.CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjIR.ListOfShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<CwcExim.Areas.Import.Models.ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}


            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
                ObjETR.GetTime();
                if (ObjETR.DBResponse.Data != null)
                {
                    objEntryThroughGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;
                    string ExitTime = objEntryThroughGate.EntryDateTime;
                    string[] ExitTimeArray = ExitTime.Split(' ');
                    objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                    ViewBag.strTime = objEntryThroughGate.Time;
                    string SystemTime = objEntryThroughGate.SystemDateTime;
                    string[] SystemTimeArray = SystemTime.Split(' ');
                    objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

                    return PartialView(objEntryThroughGate);
            //}
            //else
            //{
            //    HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
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
        [HttpGet]
        public JsonResult LoadContainer()
        {


            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();

            return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public JsonResult LoadCHA()
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.ListOfCHAName();
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadShippingLine()
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.ListOfShippingLine();
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateEntryThroughGateExport()
        {
            List<ReferenceNumberList> lstReferenceNumberList = new List<ReferenceNumberList>();
            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            List<HDB_EntryExport> lstGateEntryExport = new List<HDB_EntryExport>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
          HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber = (HDB_ReferenceNumber)ObjETR.DBResponse.Data;

               
               // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.ReferenceList.Count > 0)
                {
                    lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.listExports.Count >0)
                {
                    lstGateEntryExport = ObjReferenceNumber.listExports.ToList();
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
                if (ObjReferenceNumber.listChaName != null && ObjReferenceNumber.listChaName.Count > 0)
                {
                    lstChaName = ObjReferenceNumber.listChaName.ToList();
                }
                else
                {
                    lstChaName = null;
                }
                if (ObjReferenceNumber.listForwarder != null && ObjReferenceNumber.listForwarder.Count > 0)
                {
                    lstForwarder = ObjReferenceNumber.listForwarder.ToList();
                }
                else
                {
                    lstForwarder = null;
                }

            }

            ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
            ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
            ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                LoadContainerReference = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (LoadContainerReference != null && LoadContainerReference.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstloadReferenceNumberList = JsonConvert.SerializeObject(LoadContainerReference);
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }
            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
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
            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            List<HDB_EntryExport> lstGateEntryExport = new List<HDB_EntryExport>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber = (HDB_ReferenceNumber)ObjETR.DBResponse.Data;


                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.ReferenceList.Count > 0)
                {
                    lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.listExports.Count > 0)
                {
                    lstGateEntryExport = ObjReferenceNumber.listExports.ToList();
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
                if (ObjReferenceNumber.listChaName != null && ObjReferenceNumber.listChaName.Count > 0)
                {
                    lstChaName = ObjReferenceNumber.listChaName.ToList();
                }
                else
                {
                    lstChaName = null;
                }
                if (ObjReferenceNumber.listForwarder != null && ObjReferenceNumber.listForwarder.Count > 0)
                {
                    lstForwarder = ObjReferenceNumber.listForwarder.ToList();
                }
                else
                {
                    lstForwarder = null;
                }


            }
            ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
            ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
            ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (lstLoadContainer != null && lstLoadContainer.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstloadReferenceNumberList = JsonConvert.SerializeObject(lstLoadContainer);
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }
            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
          //  HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;

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
        public JsonResult GetStufftype(string StuffType)
        {
            HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
            // List<LoadContainerReferenceNumberList> lstContList = new List<LoadContainerReferenceNumberList>();

       
            List<LoadContainerReferenceNumberList> LoadContainerReferenceLst = new List<LoadContainerReferenceNumberList>();
        List<StuffingReqList> lstStuffing = new List<StuffingReqList>();
            // List<container> Lstcontainer = new List<container>();
         

            if (StuffType == "ContainerStuff")
            {
                ObjGOR.GetStuffingRefNUmber();
                if (ObjGOR.DBResponse.Data != null)
                {

                    lstStuffing = (List<StuffingReqList>)ObjGOR.DBResponse.Data;
                }
               

            }
            else
            {
                ObjGOR.GetLoadContainerRefNUmb();

                if (ObjGOR.DBResponse.Data != null)
                {

                    LoadContainerReferenceLst = (List<LoadContainerReferenceNumberList>)ObjGOR.DBResponse.Data;
                }
                //   ViewBag.lstContList = JsonConvert.SerializeObject(lstContList);


            }
           
            // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
          
           
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CreateEntryThroughGateBond()
        {
            List<BondReferenceNumberList> lstReferenceNumberList = new List<BondReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            // List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();

            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
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

            HDB_EntryThroughGateBond objEntryThroughGate = new HDB_EntryThroughGateBond();
            ObjETR.GetBondTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (HDB_EntryThroughGateBond)ObjETR.DBResponse.Data;
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

        public JsonResult GetBondSpaceDetailsById(int RefId)
        {
            HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
            ObjGOR.GetBondSpaceDetailsById(RefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditEntryThroughGateBond(int EntryId)
        {
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            HDB_EntryThroughGateBond ObjEntryGate = new HDB_EntryThroughGateBond();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGateBond(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGateBond)ObjETR.DBResponse.Data;

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



        public ActionResult CreateEntryThroughGateLoadContainer()
        {
            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

             List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }

            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);


            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;
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

            return PartialView("CreateEntryThroughGateLoadContainer", objEntryThroughGate);


        }
      

        public ActionResult EditEntryThroughGateLoadContainer(int EntryId)
        {
            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

             List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            //  HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;

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
        public ActionResult CreateEntryThroughGateEmpty()
        {
            List<LoadContainerReferenceNumberList> LoadContainerReference = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }

            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);


            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;
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

            return PartialView("CreateEntryThroughGateEmpty", objEntryThroughGate);


        }

        public ActionResult EditEntryThroughGateEmpty(int EntryId)
        {
            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            // List<GateEntryExport> lstGateEntryExport = new List<GateEntryExport>();

            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }
            // ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            // ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            //ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            //  HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;

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

            return PartialView("EditEntryThroughGateEmpty", ObjEntryGate);


        }
        public JsonResult GetFieldsForContainer(string ContainerName)
        {
            if (ContainerName != "")
            {
                HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
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
        public JsonResult getLoadContainerList(int LoadContainerRefId)
        {
            HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
            ObjGOR.GetLoadedContDat(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getLoadedContainerList(int LoadContainerRefId)
        {
            HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
            ObjGOR.GetLoadedContData(LoadContainerRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getLoadedContainerStuffList(int StuffRefId)
        {
            HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
            ObjGOR.GetLoadedContStuffData(StuffRefId);
            return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGate(HDB_EntryThroughGate ObjEntryThroughGate)
        {
            ModelState.Remove("ShippingLineId");
            ModelState.Remove("ShippingLine");

            if (ObjEntryThroughGate.OperationType== "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1;
            }

            string Entrytime = Request.Form["time"];
            string SysTime = Request.Form["SysTime"];

            if (ObjEntryThroughGate.EntryId == 0)
            {

                
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            
            }
            if(Entrytime.Length==7)
            {
                Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
            }

            if (ObjEntryThroughGate.EntryId > 0)
            {


                Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");

            }

            string strEntryDateTime = ObjEntryThroughGate.EntryDateTime +" "+ Entrytime;
            if (SysTime != null)
            {
                string [] SplitSysTime= SysTime.Split(':');
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



            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;
         
            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");





            int BranchId =Convert.ToInt32( Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
            if (ObjEntryThroughGate.ShippingLineId == 0)
            {
                ObjEntryThroughGate.ShippingLine = "";
            }
            if (ModelState.IsValid)
            {
                ModelState.Remove("ShippingLine");
                HDB_EntryThroughGateRepository ObjEntryThroughGateRepositories = new HDB_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;
               
                ObjEntryThroughGateRepositories.AddEditEntryThroughGate(ObjEntryThroughGate);
                // ModelState.Clear();
                if (ObjEntryThroughGateRepositories.DBResponse.Status == 1 && ObjEntryThroughGate.ContainerNo1==null || ObjEntryThroughGate.ContainerNo1 =="") //|| ObjEntryThroughGateRepositories.DBResponse.Status == 2
                {
                    int lastInsertedId =Convert.ToInt32(ObjEntryThroughGateRepositories.DBResponse.Data);
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
        public ActionResult AddEditEntryThroughGateBond(HDB_EntryThroughGateBond ObjEntryThroughGate)
        {
            ObjEntryThroughGate.OperationType = "Bond";
            if(!string.IsNullOrEmpty(ObjEntryThroughGate.CBTNo))
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
                HDB_EntryThroughGateRepository ObjEntryThroughGateRepositories = new HDB_EntryThroughGateRepository();
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
        public ActionResult EntryThroughGateList(string OperationType="")
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            List<HDB_EntryThroughGate> LstEntryThroughGate = new List<HDB_EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate(0,OperationType);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<HDB_EntryThroughGate>)ObjETR .DBResponse.Data;
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }
        
        public JsonResult LoadMoreEntryThroughGateList(int Page,string OperationType = "")
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            List<HDB_EntryThroughGate> LstEntryThroughGate = new List<HDB_EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate(Page, OperationType);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<HDB_EntryThroughGate>)ObjETR.DBResponse.Data;
            }
            return Json(LstEntryThroughGate, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();

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
                    ObjEntryGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;

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

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            HDB_EntryThroughGateRepository ObjEtGateR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjEtGateR.DBResponse.Data;
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
            return PartialView("ViewEntryThroughGate", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateBond(int EntryId)
        {
            HDB_EntryThroughGateBond ObjEntryGate = new HDB_EntryThroughGateBond();
            HDB_EntryThroughGateRepository ObjEtGateR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjEtGateR.GetEntryThroughGateBond(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGateBond)ObjEtGateR.DBResponse.Data;
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
        public ActionResult ViewEntryThroughGateExport(int EntryId)
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

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            HDB_EntryThroughGateRepository ObjEtGateR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjEtGateR.DBResponse.Data;
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
            return PartialView("ViewEntryThroughGateExport", ObjEntryGate);
        }
        [HttpGet]
        public ActionResult ViewEntryThroughGateEmpty(int EntryId)
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

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            HDB_EntryThroughGateRepository ObjEtGateR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjEtGateR.DBResponse.Data;
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
            return PartialView("ViewEntryThroughGateEmpty", ObjEntryGate);
        }


        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteEntryThroughGate(int EntryId)
        {
            if (EntryId > 0)
            {
                HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
                ObjGOR.DeleteEntryThroughGate(EntryId);
                return Json(ObjGOR.DBResponse,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        [NonAction]
        public int SendEntryMail(string ContainerNo,int lastInsertedId)
        {
            try
            {
                string message = "";
                var file = (dynamic)null;

                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                HDB_EntryThroughGateRepository objEntry = new HDB_EntryThroughGateRepository();
                objEntry.GetDetailsForGateEntryMail(ContainerNo);
                if (objEntry.DBResponse.Data != null)
                {
                    var mailTo = ((EntryThroughGateMail)objEntry.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j=0;j< mailTo.Length;j++)
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
                        HDB_EntryThroughGateRepository etgr = new HDB_EntryThroughGateRepository();
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
                            tw.WriteLine("For Container No :" + ContainerNo + " .Email not Sent To :" + MailIds+"\r\n Error:"+status);
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
        public JsonResult GetSACDetails(int Id)
        {
            HDB_EntryThroughGateRepository obj = new HDB_EntryThroughGateRepository();
            obj.SACDetails(Id);
            return Json(obj.DBResponse,JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[CustomValidateAntiForgeryToken]
        //public JsonResult PrintSealCutting(int EntryId)
        //{
        //    HDB_EntryThroughGateRepository ObjGR = new HDB_EntryThroughGateRepository();
        //    ObjGR.GetPrintSealCutting(EntryId);
        //    if (ObjGR.DBResponse.Data != null)
        //    {
        //        List<GateEntrySealCutting> LstSeal = new List<GateEntrySealCutting>();
        //        LstSeal = (List<GateEntrySealCutting>)ObjGR.DBResponse.Data;
        //        string Path = GeneratePdfForSealCut(LstSeal, EntryId);
        //        return Json(new { Status = 1, Message = Path });
        //    }
        //    else
        //    {
        //        return Json(new { Status = 1, Message = "Error" });
        //    }
        //}

        //[NonAction]
        //public string GeneratePdfForSealCut(List<GateEntrySealCutting> LstSeal,int EntryId)
        //{
        //    string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SealCutting" + EntryId + ".pdf";
        //    string ContainerNo="", Size = "", SealNo="", BLNo="",Importer="";
        //    LstSeal.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
        //    var Html = "";
        //    {
        //        if (ContainerNo == "")
        //            ContainerNo = item.ContainerNo;
        //        else
        //            ContainerNo += "<br/>" + item.ContainerNo;
        //    });
        //    LstSeal.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
        //    {
        //        if (Size == "")
        //            Size = item.Size;
        //        else
        //            Size += "<br/>" + item.Size;
        //    });
        //    LstSeal.Select(x => new { SealNo = x.SealNo }).Distinct().ToList().ForEach(item =>
        //    {
        //        if (SealNo == "")
        //            SealNo = item.SealNo;
        //        else
        //            SealNo += "<br/>"+item.SealNo;
        //    });
        //    LstSeal.Select(x => new { BLNo= x.BLNo }).Distinct().ToList().ForEach(item =>
        //    {
        //        if (BLNo == "")
        //            BLNo = item.BLNo;
        //        else
        //            BLNo += "<br/>"+item.BLNo;
        //    });
        //    LstSeal.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item =>
        //    {
        //        if (Importer == "")
        //            Importer = item.Importer;
        //        else
        //            Importer += "<br/>" + item.Importer;
        //    });

        //    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //    }
        //    if (System.IO.File.Exists(Path))
        //    {
        //      //  System.IO.File.Delete(Path);
        //    }
        //    int Index = 1;
        //    Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody> <tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='90%' valign='top' align='center'><h1 style='font-size: 22px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Government of India Undertaking) </label> </td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr> <tr><td colspan='3' style='font-weight: bold;'><h6 style='margin:0;'>Manager - CFS/The Superintendent(Customs)</h6><h6 style='margin:0;'>CFS, Kukatpally,</h6><h6 style='margin:0;'>Hyderabad</h6></td></tr> <tr><td colspan='3' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Sub : Request for Seal Cutting of Import LCL (CBT)/Containers - Reg.</h2></td></tr> <tr><td colspan='3'><table style='width:100%;'cellspacing='0' cellpadding='0'><tbody><tr><td><p style='font-size:12px; margin-bottom:0;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> With reference to the above cited subject kindly arrange the following LCL(CBT)/Containers for the Customs Examinations for Factory de-stuffing/CFS de-stuffing into godown/direct loading into truck.</p></td></tr><tr><td><table style='margin:5px 0 0; border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width:80px;'>S.No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 200px;'>CBT/Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 60px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 150px;'>Seal No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 150px;'>BL No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 250px;'>Name of the importer</th></tr></thead><tbody>";
        //  //  foreach (GateEntrySealCutting item in LstSeal)
        //   // {
        //        Html += "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width:80px;'>" + Index + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 200px;'>" +ContainerNo + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>" + Size + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px;'>" + SealNo + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px;'>" + BLNo + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 250px;'>" + Importer + "</td></tr>";
        //       // Index++;
        //   // }
        //    Html += "</tbody></table></td></tr></tbody></table><p style='font-size: 13px; margin: 0;'>No Objection for seal cutting & destuffing obtained from IS/liner is enclosed.</p><br/><br/><br/><table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr><td><h6 style='font-size: 13px; margin:0; font-weight: normal;'>Permitted</h6><h3 style='font-size: 15px; margin:0;'>Signature of Customs Official</h3></td><td><h3 style='font-size: 15px; margin:0;font-weight: normal; text-align:right;'>Yours faithfully,</h3></td></tr></tbody></table></td></tr></tbody></table>";
        //    Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
        //    Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
        //    using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //    {
        //        Rh.GeneratePDF(Path, Html);
        //    }
        //    return "/Docs/" + Session.SessionID + "/SealCutting" + EntryId + ".pdf";
        //}

        #endregion


        #region Entry Through Gate -Export

        [HttpGet]
        public ActionResult CreateGateEmptyExport()
        {
            //if(ContainerName=="")
            //{
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            List<container> Lstcontainer = new List<container>();
            ObjETR.GetContainer();
            if (ObjETR.DBResponse.Data != null)
            {
                Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.ListOfCHAName();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CwcExim.Areas.Import.Models.CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjIR.ListOfShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<CwcExim.Areas.Import.Models.ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }


            HDB_EntryThroughGate objEntryThroughGate = new HDB_EntryThroughGate();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objEntryThroughGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            return PartialView(objEntryThroughGate);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEntryThroughGateExport(HDB_EntryThroughGate ObjEntryThroughGate)
        {
            ModelState.Remove("ShippingLineId");
            ModelState.Remove("ShippingLine");

            if (ObjEntryThroughGate.OperationType == "LoadedContainer")
            {
                ObjEntryThroughGate.OperationType = "Export";
            }

            if (ObjEntryThroughGate.ContainerNo1 != null && ObjEntryThroughGate.ContainerNo1 != "")
            {
                ObjEntryThroughGate.ContainerNo = ObjEntryThroughGate.ContainerNo1;
            }

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



            //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            ObjEntryThroughGate.EntryDateTime = strEntryDateTime;

            //ObjEntryThroughGate.EntryDateTime = EntrydateTime.ToString("yyyy/MM/dd hh:mm");





            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjEntryThroughGate.BranchId = BranchId;
            if (ObjEntryThroughGate.ShippingLineId == 0)
            {
                ObjEntryThroughGate.ShippingLine = "";
            }
            if (ModelState.IsValid)
            {
                ModelState.Remove("ShippingLine");
                HDB_EntryThroughGateRepository ObjEntryThroughGateRepositories = new HDB_EntryThroughGateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEntryThroughGate.Uid = ObjLogin.Uid;

                ObjEntryThroughGateRepositories.AddEditEntryThroughGateExport(ObjEntryThroughGate);
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
        public ActionResult EntryThroughGateListExport(string OperationType = "")
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            List<HDB_EntryThroughGate> LstEntryThroughGate = new List<HDB_EntryThroughGate>();
            ObjETR.GetAllEntryThroughGate(0,OperationType);

            if (ObjETR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<HDB_EntryThroughGate>)ObjETR.DBResponse.Data;
                LstEntryThroughGate = LstEntryThroughGate.Where(x => x.OperationType != "Import").ToList();
            }
            return PartialView("EntryThroughGateListExport", LstEntryThroughGate);
        }

        public ActionResult EditEntryThroughGateExportExp(int EntryId)
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
            List<LoadContainerReferenceNumberList> lstLoadContainer = new List<LoadContainerReferenceNumberList>();
            List<HDB_EntryExport> lstGateEntryExport = new List<HDB_EntryExport>();
            List<ChaNameList> lstChaName = new List<ChaNameList>();
            List<ShippingLineList> lstShippingLine = new List<ShippingLineList>();
            List<ForwarderList> lstForwarder = new List<ForwarderList>();
            HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
            HDB_ReferenceNumber ObjReferenceNumber = new HDB_ReferenceNumber();
            ObjETR.GetReferenceNumber();
            if (ObjETR.DBResponse.Data != null)
            {
                ObjReferenceNumber = (HDB_ReferenceNumber)ObjETR.DBResponse.Data;


                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.ReferenceList.Count > 0)
                {
                    lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                }
                else
                {
                    lstReferenceNumberList = null;
                }

                //lstGateEntryExport = ObjReferenceNumber.listExport.ToList();
                if (ObjReferenceNumber.listExports != null && ObjReferenceNumber.listExports.Count > 0)
                {
                    lstGateEntryExport = ObjReferenceNumber.listExports.ToList();
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
                if (ObjReferenceNumber.listChaName != null && ObjReferenceNumber.listChaName.Count > 0)
                {
                    lstChaName = ObjReferenceNumber.listChaName.ToList();
                }
                else
                {
                    lstChaName = null;
                }
                if (ObjReferenceNumber.listForwarder != null && ObjReferenceNumber.listForwarder.Count > 0)
                {
                    lstForwarder = ObjReferenceNumber.listForwarder.ToList();
                }
                else
                {
                    lstForwarder = null;
                }


            }
            ViewBag.lstReferenceNumberList = JsonConvert.SerializeObject(lstReferenceNumberList);
            ViewBag.lstGateEntryExport = JsonConvert.SerializeObject(lstGateEntryExport);
            ViewBag.lstShippingLine = JsonConvert.SerializeObject(lstShippingLine);
            ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
            ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
            ObjETR.GetLoadContainerRefNUmber();
            if (ObjETR.DBResponse.Data != null)
            {
                // ObjReferenceNumber = (ReferenceNumber)ObjETR.DBResponse.Data;


                lstLoadContainer = (List<LoadContainerReferenceNumberList>)ObjETR.DBResponse.Data;

                // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                if (lstLoadContainer != null && lstLoadContainer.Count > 0)
                {
                    // lstReferenceNumberList = ObjReferenceNumber.ReferenceList.ToList();
                    ViewBag.lstloadReferenceNumberList = JsonConvert.SerializeObject(lstLoadContainer);
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
            ObjETR.GetChaNameLoadCont();

            lstChaName = (List<ChaNameList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstChaName != null && lstChaName.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstChaName = JsonConvert.SerializeObject(lstChaName);
                }
                else
                {
                    lstChaName = null;
                }
            }

            ObjETR.ListOfForwarderLoadCont();
            lstForwarder = (List<ForwarderList>)ObjETR.DBResponse.Data;
            if (ObjETR.DBResponse.Data != null)
            {
                if (lstForwarder != null && lstForwarder.Count > 0)
                {
                    //lstShippingLine = ObjReferenceNumber.listShippingLine.ToList();
                    ViewBag.lstForwarder = JsonConvert.SerializeObject(lstForwarder);
                }
                else
                {
                    lstForwarder = null;
                }
            }
            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            //  HDB_EntryThroughGateRepository ObjETR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjETR.GetEntryThroughGate(EntryId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjETR.DBResponse.Data;

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

            return PartialView("EditEntryThroughGateExportExp", ObjEntryGate);


        }


        [HttpGet]
        public ActionResult ViewEntryThroughGateExportExp(int EntryId)
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

            HDB_EntryThroughGate ObjEntryGate = new HDB_EntryThroughGate();
            HDB_EntryThroughGateRepository ObjEtGateR = new HDB_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                //HDB_EntryThroughGateRepository ObjGOR = new HDB_EntryThroughGateRepository();
                ObjEtGateR.GetEntryThroughGate(EntryId);
                if (ObjEtGateR.DBResponse.Data != null)
                {
                    ObjEntryGate = (HDB_EntryThroughGate)ObjEtGateR.DBResponse.Data;
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
            return PartialView("ViewEntryThroughGateExportExp", ObjEntryGate);
        }

        #endregion
    }
}