using CwcExim.Areas.GateOperation.Models;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Filters;
using System.Web.Configuration;
using CwcExim.Controllers;
using Newtonsoft.Json;
using System.Xml;
using System.Web.Script.Serialization;
using System.IO;
using System.Data;
using SCMTRLibrary;
using CwcExim.Areas.Export.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class Ppg_GateExitController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: GateOperation/Ppg_GateExit
        #region Exit Through Gate Ppg
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateExitThroughGate()
        {
            ExitThroughGateRepository ObjETR = new ExitThroughGateRepository();
            ExitThroughGateHeader objExitThroughGateHeader = new ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (ExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                //List<GatePassList> LstGatePass = new List<GatePassList>();
                //ObjETR.GetGatePassLst();
                //if (ObjETR.DBResponse.Data != null)
                //{

                //    LstGatePass = (List<GatePassList>)ObjETR.DBResponse.Data;
                //    ViewBag.LstGatePass = LstGatePass;
                //}


              //  List<container> Lstcontainer = new List<container>();
              //  ObjETR.GetContainer();
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
                //}
                //ViewBag.Lstcontainer = Lstcontainer;
              //  if (ObjETR.DBResponse.Data != null)
             //   {
             //       ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((ExitThroughGateHeader)ObjETR.DBResponse.Data).containerList);
             //   }
            }

            return PartialView(objExitThroughGateHeader);
        }


        [HttpGet]
        public JsonResult GetGatePass()
        {
            ExitThroughGateRepository ObjETR = new ExitThroughGateRepository();

           
            List<dynamic> objImp2 = new List<dynamic>();
            ObjETR.GetGatePassLst();
            if (ObjETR.DBResponse.Data != null)
            {
                ((List<GatePassList>)ObjETR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GatePassId = item.GatePassId, GatePassNo = item.GatePassNo });
                });
             
            }

           

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerForGatePass(int GPId)
        {
            ExitThroughGateRepository ObjGOR = new ExitThroughGateRepository();
            List<containerAgainstGp_PPG> LstcontainerAgainstGp = new List<containerAgainstGp_PPG>();

            ObjGOR.ContainerForGAtePass_Ppg(GPId);//, objLogin.Uid
            if (ObjGOR.DBResponse.Data != null)
            {
                LstcontainerAgainstGp = (List<containerAgainstGp_PPG>)ObjGOR.DBResponse.Data;
                //ObjGOR.DBResponse.Data = JsonConvert.SerializeObject((containerAgainstGp)ObjGOR.DBResponse.Data);

                return Json(ObjGOR.DBResponse);
            }
            else
            {
                return Json(ObjGOR.DBResponse);
            }

        }
        public ActionResult getExitHeaderList()
        {
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.GetAllExitThroughGateList(0);
            List<ExitThroughGateHeader> ListExitThroughGateHeader = new List<ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        public JsonResult getExitHeaderListData(int Page)
        {
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.GetAllExitThroughGateList(Page);
            List<ExitThroughGateHeader> ListExitThroughGateHeader = new List<ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            
            return Json(ListExitThroughGateHeader, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult getExitDetailsList(int HeaderId, string ViewMode = "")
        {
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.GetExitThroughGateDetailsForHdr(HeaderId);
            List<ExitThroughGateDetails> ListExitThroughGateDetails = new List<ExitThroughGateDetails>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateDetails = (List<ExitThroughGateDetails>)ETGR.DBResponse.Data;
            }
            if (ViewMode == "")
                return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
            else return Json( ListExitThroughGateDetails ,JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGate(ExitThroughGateHeader objETG)
        {
            //string ExitTime="";
            //int ExitHourCal = 0;

            if (objETG.Module == "IMPDeli" || objETG.Module == "IMPYard" || objETG.Module == "EC" || objETG.Module == "AUC" || objETG.Module == "IMPGETRN" || objETG.Module == "IMPGERD" || objETG.Module == "GE")
            {
                ModelState.Remove("expectedTimeOfArrival");
                // ModelState.Remove("ArrivalDate");

            }

            if (ModelState.IsValid)
            {
                //var GateExitlst = (dynamic)null;
                String GateExitlst = objETG.StrExitThroughGateDetails;
                //string[] Time = objETG.Time.Split(':');
                //string ExitHour = Time[0];
                //string ExitTimeDet = Time[1];
                //string Exitminute = ExitTimeDet.Substring(0, 2);
                //string TimePref= ExitTimeDet.Substring(ExitTimeDet.Length-2);
                //if(TimePref == "PM" && Convert.ToInt32(ExitHour) <12)
                //{
                //     ExitHourCal = Convert.ToInt32(ExitHour) + 12;
                //     ExitTime = Convert.ToString(ExitHourCal) +":"+ Exitminute;
                //}
                //else
                //{
                //     ExitTime= objETG.Time.Substring(0, 5);
                //}
                //if (objETG.ExitIdHeader == 0)
                //{
                //   // ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                //}
                //if (ExitTime.Length == 7)
                //{
                //  //  ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                //}

                string ExitTime = objETG.Time;
                if (objETG.ExitIdHeader == 0)
                {
                    ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }
                if (ExitTime.Length == 7)
                {
                    ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }
                string strEntryDateTime = objETG.GateExitDateTime + " " + ExitTime;
                objETG.GateExitDateTime = strEntryDateTime;

                //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);


                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<ExitThroughGateDetails> LstExitThroughGateDetails = new List<ExitThroughGateDetails>();
                // var x= Newtonsoft.\Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                ExitThroughGateRepository objETGR = new ExitThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {
                    //var js = new JavaScriptSerializer();
                    //var arrExitThroughGateDetails = js.Deserialize<ExitThroughGateDetails[]>(objETG.StrExitThroughGateDetails);

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
                    string XML = Utility.CreateXML(LstExitThroughGateDetails);
                    var XMLContent = Utility.CreateXML(LstExitThroughGateDetails);
                    objETG.StrExitThroughGateDetails = XML;


                    // Elements("word").Where(x => x.Element("category")).ToList();
                    //.Elements("word").Where(x => x.Element("category").Value.Equals("verb")).ToList(); ;
                }
                else
                {
                    objETG.StrExitThroughGateDetails = "";
                }
                objETGR.AddEditExitThroughGate(objETG, ((Login)(Session["LoginUser"])).Uid);

                //if (objETGR.DBResponse.Status == 1)
                //{
                //    //if (objETGR.DBResponse.Data != null)
                //    //{
                //    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(GateExitlst);
                //    arrExitThroughGateDetailsList.Where(y => y.OperationType == "Import").GroupBy(o => o.ShippingLineId).ToList().ForEach(item =>
                //    {

                //        //var sid = item.ShippingLineId.ToString();
                //        SendExitMail(item.Key.ToString());

                //    });




                //    //for(int i=0;i<= arrExitThroughGateDetails.length;)

                //    //int shippingLineId = int.Parse(objETG.shippingLineId);


                //    // SendExitMail(objETG.shippingLineId);



                //    //int headerId = Convert.ToInt32(objETGR.DBResponse.Data);
                //    //objETGR.GetExitThroughGateDetailsForHdr(headerId);
                //    //if (objETGR.DBResponse.Data != null)
                //    //{
                //    //    IList<ExitThroughGateDetails> listExitThroughGateDetails = new List<ExitThroughGateDetails>();
                //    //    listExitThroughGateDetails = (List<ExitThroughGateDetails>)objETGR.DBResponse.Data;

                //    //    foreach (var cont in listExitThroughGateDetails)
                //    //    {
                //    //        SendExitMail(cont.ContainerNo);

                //    //    }

                //    //}

                //    //}
                //}
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

        //[HttpPost]
        //[CustomValidateAntiForgeryToken]
        public JsonResult AddToDetailsGateExit(ExitThroughGateHeader objETG)
        {
            if (ModelState.IsValid)
            {
                string GateExitlst = objETG.StrExitThroughGateDetails;
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<ExitThroughGateDetails> LstExitThroughGateDetails = new List<ExitThroughGateDetails>();
                // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
                string XML = Utility.CreateXML(LstExitThroughGateDetails);
                ExitThroughGateRepository objETGR = new ExitThroughGateRepository();
                objETG.StrExitThroughGateDetails = XML;
                objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


                if (objETGR.DBResponse.Status == 1)
                {

                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(GateExitlst);
                    arrExitThroughGateDetailsList.GroupBy(o => o.ShippingLineId).ToList().ForEach(item =>
                    {
                        //var sid = item.ShippingLineId.ToString();
                        SendExitMail(item.Key.ToString());
                    });

                }
                ModelState.Clear();
                return Json(objETG);
            }
            else
            {
                //var Err = new { Statua = -1, Messgae = "Error" };
                //return Json(Err);
                string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        //[HttpGet]
        //public ActionResult EntryThroughGateList()
        //{
        //    ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
        //    //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
        //    ////ObjGOR.GetEntryThroughGate();
        //    ////if (ObjGOR.DBResponse.Data != null)
        //    ////{
        //    ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
        //    ////}
        //    //return View(LstGateEntry);

        //    EntryThroughGateRepository ObjETR = new EntryThroughGateRepository();
        //    List<EntryThroughGate> LstEntryThroughGate = new List<EntryThroughGate>();
        //    ObjETR.GetAllEntryThroughGate();
        //    if (ObjETR.DBResponse.Data != null)
        //    {
        //        LstEntryThroughGate = (List<EntryThroughGate>)ObjETR.DBResponse.Data;
        //    }
        //    return PartialView("EntryThroughGateList", LstEntryThroughGate);
        //}

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult EditExitThroughGate(int ExitIdHdr)
        {
            ExitThroughGateHeader ObjExitThroughGateHeader = new ExitThroughGateHeader();
            ExitThroughGateRepository ObjExitR = new ExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (ExitThroughGateHeader)ObjExitR.DBResponse.Data;
                    string strDateTime = ObjExitThroughGateHeader.GateExitDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strGatePassDate = ObjExitThroughGateHeader.GatePassDate;
                    string[] gatePassDateArray = strGatePassDate.Split(' ');
                    string gatePassDate = gatePassDateArray[0];

                    ObjExitThroughGateHeader.GateExitDateTime = strDtae;
                    ObjExitThroughGateHeader.GatePassDate = gatePassDate;
                    ViewBag.strTime = convertTime;

                    string Module = ObjExitThroughGateHeader.Module;
                    List<GatePassList> LstGatePass = new List<GatePassList>();
                    ObjExitR.GetGatePassLst();
                    if (ObjExitR.DBResponse.Data != null)
                    {

                        LstGatePass = (List<GatePassList>)ObjExitR.DBResponse.Data;
                        ViewBag.LstGatePass = LstGatePass;
                    }
                    List<container> Lstcontainer = new List<container>();
                    ObjExitR.GetContainer();

                    if (ObjExitR.DBResponse.Data != null)
                    {
                        ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((ExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
                    }



                }
            }
            return PartialView(ObjExitThroughGateHeader);


        }

        [HttpGet]
        public ActionResult EditExitThroughGateDetails(int ExitIdDtls)
        {
            ExitThroughGateDetails ObjExitThroughGateDetails = new ExitThroughGateDetails();
            ExitThroughGateRepository ObjExitR = new ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.EditMode = "Edit";

            List<container> Lstcontainer = new List<container>();
            ObjExitR.GetContainer();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            //}
            //ViewBag.Lstcontainer = Lstcontainer;
            if (ObjExitR.DBResponse.Data != null)
            {
                ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((ExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
            }

            ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

            ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

            return PartialView(ObjExitThroughGateDetails);


        }

        [HttpPost]
        public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        {
            ExitThroughGateDetails ObjExitThroughGateDetails = new ExitThroughGateDetails();
            ExitThroughGateRepository ObjExitR = new ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }

            return PartialView(ObjExitThroughGateDetails);


        }

        public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        {
            ExitThroughGateDetails ObjExitThroughGateDetails = new ExitThroughGateDetails();
            ExitThroughGateRepository ObjExitR = new ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.ViewFlag = "v";
            return PartialView(ObjExitThroughGateDetails);


        }

        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult AddEditEntryThroughGate(EntryThroughGate ObjEntryGate)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
        ////        Login ObjLogin = (Login)Session["LoginUser"];
        ////        ObjEntryGate.Uid=ObjLogin.Uid;
        ////        ObjGOR.AddEditEntryThroughGate(ObjEntryGate);
        ////        ModelState.Clear();
        ////        return Json(ObjGOR.DBResponse);
        ////    }
        ////    else
        ////    {
        ////        var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e=>e.ErrorMessage));
        ////        var Err = new { Status = 0, Message = ErrorMessage };
        ////        return Json(Err);
        ////    }
        ////}

        [HttpGet]
        public ActionResult ViewExitThroughGate(int ExitIdHdr)
        {

            ExitThroughGateHeader ObjExitThroughGateHeader = new ExitThroughGateHeader();
            ExitThroughGateRepository ObjExittGateR = new ExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjExittGateR.GetExitThroughGate(ExitIdHdr);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (ExitThroughGateHeader)ObjExittGateR.DBResponse.Data;
                }
            }
            ViewBag.ViewMode = "view";
            ViewBag.HeaderId = ExitIdHdr;
            //return PartialView("CreateExitThroughGate", ObjExitThroughGateHeader);
            //return PartialView("CreateExitThroughGate", ObjExitThroughGateHeader);
            return PartialView(ObjExitThroughGateHeader);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGate(int ExitId)
        {
            if (ExitId > 0)
            {
                ExitThroughGateRepository ObjGOR = new ExitThroughGateRepository();
                ObjGOR.DeleteExitThroughGate(ExitId);
                return Json(ObjGOR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        [NonAction]
        public int SendExitMail(string ShippingLineId)//string ContainerNo
        {
            try
            {
                string message = "";
                var file = (dynamic)null;
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                ExitThroughGateRepository objExit = new ExitThroughGateRepository();
                objExit.GetDetailsForGateExitMail(ShippingLineId);
                if (objExit.DBResponse.Data != null)
                {
                    //EmailDataModel ObjEmailDataModel = new EmailDataModel();
                    //ObjEmailDataModel.ReceiverEmail = ((EntryThroughGateMail)objExit.DBResponse.Data).Email;
                    var mailTo = ((EntryThroughGateMail)objExit.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j = 0; j < mailTo.Length; j++)
                    {
                        mailTo[j].Trim();
                    }

                    var FileName = ((EntryThroughGateMail)objExit.DBResponse.Data).FileName;
                    //ObjEmailDataModel.Subject = "Test Subject";
                    //ObjEmailDataModel.MailBody = "Exit Through Gate";
                    //List<string> containerList = new List<string>();
                    var excelData = ((EntryThroughGateMail)objExit.DBResponse.Data).lstExcelData;
                    var excelString = string.Empty;
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
                    //foreach (var container in excelData.ToList())
                    //{
                    //    containerList.Add(container.ContainerNumber);
                    //}

                    //string msgContainers = String.Join(",", containerList);


                    string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                    string FolderPath = Server.MapPath("~/Uploads/GateExitExcel/" + UID);
                    if (!System.IO.Directory.Exists(FolderPath))
                    {
                        System.IO.Directory.CreateDirectory(FolderPath);
                    }
                    System.IO.File.WriteAllText((FolderPath + "\\" + FileName), excelString);
                    string[] FileList = new string[1];
                    FileList[0] = FolderPath + "\\" + FileName;
                    string status = UtilityClasses.CommunicationManager.SendMail(
                           "Container Exited Through Gate",
                       "Container Number : " + containerArray + " ,Exited Through Gate",
                        mailTo,
                        new[] { FolderPath + "\\" + FileName }


                        );//SendMailWithAttachment(ObjEmailDataModel, FileList);

                    if (status == "Success")
                    {

                        ExitThroughGateRepository etgr = new ExitThroughGateRepository();
                        foreach (var ContainerNo in containerArray.Split(',').ToArray())
                        {
                            etgr.ExitMailStatus(ContainerNo);
                            if (etgr.DBResponse.Status == 1)
                            {
                                message = "Email Status Updated";
                            }
                            //else
                            //{

                            //    string FolderPath1 = Server.MapPath("~/Uploads/ExitEmailError/" + CuurDate);
                            //    if (!System.IO.Directory.Exists(FolderPath1))
                            //    {
                            //        System.IO.Directory.CreateDirectory(FolderPath1);



                            //    }
                            //    file = Path.Combine(FolderPath1, time + "_ErrorExitEmail.txt");
                            //    string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                            //    using (var tw = new StreamWriter(file, true))
                            //    {
                            //        tw.WriteLine("For Container No :" + containerArray + " .Email not Sent To: " + MailIds+" \n Error:"+status);
                            //        tw.Close();
                            //    }

                            //    message = "Email Status Not Updated";
                            //}
                        }
                        if (System.IO.Directory.Exists(FolderPath))
                        {
                            System.IO.Directory.Delete(FolderPath, true);
                        }
                    }

                    else
                    {
                        string FolderPath2 = Server.MapPath("~/Uploads/ExitEmailError/" + CuurDate);
                        if (!System.IO.Directory.Exists(FolderPath2))
                        {
                            System.IO.Directory.CreateDirectory(FolderPath2);



                        }
                        file = Path.Combine(FolderPath2, time + "_ErrorExitEmail.txt");
                        string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                        using (var tw = new StreamWriter(file, true))
                        {
                            tw.WriteLine("For Container No:" + containerArray + " Email not Sent To " + MailIds + "Error:" + status);
                            tw.Close();
                        }


                    }
                    //if (System.IO.Directory.Exists(FolderPath))
                    //{
                    //    System.IO.Directory.Delete(FolderPath, true);
                    //}
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        #endregion

        #region 

        [HttpGet]
        public ActionResult RevalidateGatePass()
        {
            RevalidateGatePass objRevalidateGatePass = new Models.RevalidateGatePass();

            ExitThroughGateRepository ObjGOR = new ExitThroughGateRepository();
            ObjGOR.GetGatePassLstToRevalidate();

            if (ObjGOR.DBResponse.Data != null)
            {
                ViewBag.GpLstJson = JsonConvert.SerializeObject((List<RevalidateGatePass>)ObjGOR.DBResponse.Data);
            }
            else
            {
                ViewBag.GpLstJson = null;
            }


            // ObjWeighment.WeightmentDate = DateTime.Now.ToString("dd/MM/yyyy");

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult SaveRevalidateGatePass(RevalidateGatePass objRevalidateGatePass)
        {
            if (ModelState.IsValid)
            {
                ExitThroughGateRepository ObjGOR = new ExitThroughGateRepository();
                ObjGOR.UpdateGatePassValidity(objRevalidateGatePass);
                ModelState.Clear();
                return Json(ObjGOR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }


            //return Json(objRevalidateGatePass);

        }

        #endregion 

        #region CBT Gate Exit

        [HttpGet]
        public ActionResult CreateCBTGateExit(string date = "")
        {
            ExitThroughGateRepository ObjETR = new ExitThroughGateRepository();
            Ppg_GateExitCBT ObjCBT = new Ppg_GateExitCBT();
            ExitThroughGateHeader objExitThroughGateHeader = new ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (ExitThroughGateHeader)ObjETR.DBResponse.Data;
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
            List<Ppg_CBTList> LstCBT = new List<Ppg_CBTList>();
            ObjETR.GetCBTList();
            if (ObjETR.DBResponse.Data != null)
            {

                LstCBT = (List<Ppg_CBTList>)ObjETR.DBResponse.Data;
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
        public ActionResult AddEditExitThroughGateCBT(Ppg_GateExitCBT objETG)
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



                ExitThroughGateRepository objETGR = new ExitThroughGateRepository();

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
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.GetAllExitThroughGateCBT();
            List<Ppg_GateExitCBT> ListExitThroughGateHeader = new List<Ppg_GateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<Ppg_GateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public ActionResult EditExitThroughGateCBT(int ExitId)
        {
            Ppg_GateExitCBT ObjExitThroughGateHeader = new Ppg_GateExitCBT();
            ExitThroughGateRepository ObjExitR = new ExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExitR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (Ppg_GateExitCBT)ObjExitR.DBResponse.Data;
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

            Ppg_GateExitCBT ObjExitThroughGateHeader = new Ppg_GateExitCBT();
            ExitThroughGateRepository ObjExittGateR = new ExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExittGateR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (Ppg_GateExitCBT)ObjExittGateR.DBResponse.Data;
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
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.SearchGateExitCBT(CBTNo);
            List<Ppg_GateExitCBT> ListExitThroughGateHeader = new List<Ppg_GateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<Ppg_GateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGateCBT(int ExitId)
        {
            if (ExitId > 0)
            {
                ExitThroughGateRepository ObjGOR = new ExitThroughGateRepository();
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


        #region Send DT


        public async Task<JsonResult> SendDT(int ExitId = 0)
        {
            int k = 0;
            int j = 1;
            ExitThroughGateRepository ObjER = new ExitThroughGateRepository();
            // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetDPDetails(ExitId, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                foreach (DataRow dr in ds.Tables[7].Rows)
                {
                    string Filenm = Convert.ToString(dr["FileName"]);
                    int Exitdtlid = Convert.ToInt32(dr["Exitdtlid"]);
                    string JsonFile = DTJsonFormat.DTJsonCreation(ds, Exitdtlid);
                    // string Filenm = Convert.ToString(ds.Tables[7].Rows[0]["FileName"]);



                    string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMDT"];
                    string FileName = strFolderName + Filenm;
                    if (!Directory.Exists(strFolderName))
                    {
                        Directory.CreateDirectory(strFolderName);
                    }


                    System.IO.File.Create(FileName).Dispose();

                    System.IO.File.WriteAllText(FileName, JsonFile);
                    string output = "";
                    #region Digital Signature

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                    string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileDT"];
                    string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileDT"];
                    string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                    string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                    string DSCPASSWORD = Convert.ToString(ds.Tables[8].Rows[0]["DSCPASSWORD"]);

                    log.Error("Done All key  .....");
                    if (!Directory.Exists(OUTJsonFile))
                    {
                        Directory.CreateDirectory(OUTJsonFile);
                    }

                    DECSignedModel decSignedModel = new DECSignedModel();
                    decSignedModel.InJsonFile = InJsonFile + Filenm;
                    decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                    decSignedModel.ArchiveInJsonFile = "No";
                    decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                    decSignedModel.DSCPATH = DSCPATH;
                    decSignedModel.DSCPASSWORD = DSCPASSWORD;

                    string FinalOutPutPath = OUTJsonFile + Filenm;
                    log.Error("Json String Before SerializeObject:");

                    string StrJson = JsonConvert.SerializeObject(decSignedModel);
                    log.Error("Json String After SerializeObject:" + StrJson);

                    #endregion
                    log.Error("Json String Before submit:" + StrJson);

                    var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.StackTrace + ":" + ex.Message);
                        }

                    }

                    ExitThroughGateRepository objExport = new ExitThroughGateRepository();
                    objExport.GetCIMDPDetailsUpdateStatus(ExitId);
                    return Json(new { Status = 1, Message = "CIM DT File Send Successfully." });

                }

                return Json(new { Status = 1, Message = "CIM DT File Send Fail." });
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
            }


        }
        #endregion

        #region SendAR
        public async Task<JsonResult> SendAR(int ExitId = 0)
        {
            int k = 0;
            int j = 1;
            ExitThroughGateRepository ObjER = new ExitThroughGateRepository();
            // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetARDetails(ExitId, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                foreach (DataRow dr in ds.Tables[7].Rows)
                {
                    string Filenm = Convert.ToString(dr["FileName"]);
                    int Exitdtlid = Convert.ToInt32(dr["Exitdtlid"]);
                    string JsonFile = ARJsonFormat.ARJsonCreation(ds, Exitdtlid);
                    // string Filenm = Convert.ToString(ds.Tables[7].Rows[0]["FileName"]);



                    string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMAR"];
                    string FileName = strFolderName + Filenm;
                    if (!Directory.Exists(strFolderName))
                    {
                        Directory.CreateDirectory(strFolderName);
                    }


                    System.IO.File.Create(FileName).Dispose();

                    System.IO.File.WriteAllText(FileName, JsonFile);
                    string output = "";
                    #region Digital Signature

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                    string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileAR"];
                    string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileAR"];
                    string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                    string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                    string DSCPASSWORD = Convert.ToString(ds.Tables[8].Rows[0]["DSCPASSWORD"]);

                    log.Error("Done All key  .....");
                    if (!Directory.Exists(OUTJsonFile))
                    {
                        Directory.CreateDirectory(OUTJsonFile);
                    }

                    DECSignedModel decSignedModel = new DECSignedModel();
                    decSignedModel.InJsonFile = InJsonFile + Filenm;
                    decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                    decSignedModel.ArchiveInJsonFile = "No";
                    decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                    decSignedModel.DSCPATH = DSCPATH;
                    decSignedModel.DSCPASSWORD = DSCPASSWORD;

                    string FinalOutPutPath = OUTJsonFile + Filenm;
                    log.Error("Json String Before SerializeObject:");

                    string StrJson = JsonConvert.SerializeObject(decSignedModel);
                    log.Error("Json String After SerializeObject:" + StrJson);

                    #endregion
                    log.Error("Json String Before submit:" + StrJson);

                    var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.StackTrace + ":" + ex.Message);
                        }




                    }

                    ExitThroughGateRepository objExport = new ExitThroughGateRepository();
                    objExport.GetCIMARDetailsUpdateStatus(ExitId);
                    return Json(new { Status = 1, Message = "CIM AR File Send Successfully." });

                }

                return Json(new { Status = 1, Message = "CIM AR File Send Fail." });
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
            }


        }
        #endregion
    }
}