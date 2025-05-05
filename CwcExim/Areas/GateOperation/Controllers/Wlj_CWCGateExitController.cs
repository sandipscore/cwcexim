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

namespace CwcExim.Areas.GateOperation.Controllers
{   
    public class Wlj_CWCGateExitController : BaseController
    {



        // GET: GateOperation/kol_CWCGateExit
        #region Exit Through Gate kolkata
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateExitThroughGate(string date = "")
        {
            WljExitThroughGateRepository ObjETR = new WljExitThroughGateRepository();
            WljExitThroughGateHeader objExitThroughGateHeader = new WljExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (WljExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                List<WljGatePassList> LstGatePass = new List<WljGatePassList>();
                if (date == "")
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                objExitThroughGateHeader.GateExitDateTime = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                ObjETR.GetGatePassLst(date);
                if (ObjETR.DBResponse.Data != null)
                {

                    LstGatePass = (List<WljGatePassList>)ObjETR.DBResponse.Data;
                    ViewBag.LstGatePass = LstGatePass;
                }


                List<Wljcontainer> Lstcontainer = new List<Wljcontainer>();
                ObjETR.GetContainer();
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
                //}
                //ViewBag.Lstcontainer = Lstcontainer;
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((WljExitThroughGateHeader)ObjETR.DBResponse.Data).containerList);
                }
            }

            return PartialView(objExitThroughGateHeader);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerForGatePass(int GPId)
        {
            WljExitThroughGateRepository ObjGOR = new WljExitThroughGateRepository();
            List<containerAgainstGp_Hdb> LstcontainerAgainstGp = new List<containerAgainstGp_Hdb>();

            ObjGOR.ContainerForGAtePassHdb(GPId);//, objLogin.Uid
            if (ObjGOR.DBResponse.Data != null)
            {
                LstcontainerAgainstGp = (List<containerAgainstGp_Hdb>)ObjGOR.DBResponse.Data;
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
            WljExitThroughGateRepository ETGR = new WljExitThroughGateRepository();
            ETGR.GetAllExitThroughGate();
            List<WljExitThroughGateHeader> ListExitThroughGateHeader = new List<WljExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WljExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        public ActionResult SearchGateExit(string Value)
        {
            WljExitThroughGateRepository ETGR = new WljExitThroughGateRepository();
            ETGR.SearchGateExit(Value);
            List<WljExitThroughGateHeader> ListExitThroughGateHeader = new List<WljExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WljExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

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
            // return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
            if (ViewMode == "")
                return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
            else return Json(ListExitThroughGateDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGate(WljExitThroughGateHeader objETG)
        {
            string SCMTRXML = "";
            if (ModelState.IsValid)
            {
                //var GateExitlst = (dynamic)null;
                String GateExitlst = objETG.StrExitThroughGateDetails;
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
                IList<WljExitThroughGateDetails> LstExitThroughGateDetails = new List<WljExitThroughGateDetails>();
                // var x= Newtonsoft.\Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                WljExitThroughGateRepository objETGR = new WljExitThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {
                    //var js = new JavaScriptSerializer();
                    //var arrExitThroughGateDetails = js.Deserialize<ExitThroughGateDetails[]>(objETG.StrExitThroughGateDetails);

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WljExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
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

                if (objETG.SCMTRXML != null && objETG.SCMTRXML != "")
                {
                    List<WLJGateExitSCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WLJGateExitSCMTR>>(objETG.SCMTRXML);
                    SCMTRXML = Utility.CreateXML(LstSCMTR);
                }               

                objETGR.AddEditExitThroughGate(objETG, ((Login)(Session["LoginUser"])).Uid, SCMTRXML);

                if (objETGR.DBResponse.Status == 1)
                {
                    //if (objETGR.DBResponse.Data != null)
                    //{
                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WljExitThroughGateDetails>>(GateExitlst);
                    arrExitThroughGateDetailsList.Where(y => y.OperationType == "Import").GroupBy(o => o.ShippingLineId).ToList().ForEach(item =>
                    {

                        //var sid = item.ShippingLineId.ToString();
                        SendExitMail(item.Key.ToString());

                    });




                    //for(int i=0;i<= arrExitThroughGateDetails.length;)

                    //int shippingLineId = int.Parse(objETG.shippingLineId);


                    // SendExitMail(objETG.shippingLineId);



                    //int headerId = Convert.ToInt32(objETGR.DBResponse.Data);
                    //objETGR.GetExitThroughGateDetailsForHdr(headerId);
                    //if (objETGR.DBResponse.Data != null)
                    //{
                    //    IList<ExitThroughGateDetails> listExitThroughGateDetails = new List<ExitThroughGateDetails>();
                    //    listExitThroughGateDetails = (List<ExitThroughGateDetails>)objETGR.DBResponse.Data;

                    //    foreach (var cont in listExitThroughGateDetails)
                    //    {
                    //        SendExitMail(cont.ContainerNo);

                    //    }

                    //}

                    //}
                }
                ModelState.Clear();
                return Json(objETGR.DBResponse,JsonRequestBehavior.AllowGet);
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
        public JsonResult AddToDetailsGateExit(WljExitThroughGateHeader objETG)
        {
            if (ModelState.IsValid)
            {
                string GateExitlst = objETG.StrExitThroughGateDetails;
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<WljExitThroughGateDetails> LstExitThroughGateDetails = new List<WljExitThroughGateDetails>();
                // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WljExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
                string XML = Utility.CreateXML(LstExitThroughGateDetails);
                WljExitThroughGateRepository objETGR = new WljExitThroughGateRepository();
                objETG.StrExitThroughGateDetails = XML;
                objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


                if (objETGR.DBResponse.Status == 1)
                {

                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WljExitThroughGateDetails>>(GateExitlst);
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
            WljExitThroughGateHeader ObjExitThroughGateHeader = new WljExitThroughGateHeader();
            WljExitThroughGateRepository ObjExitR = new WljExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WljExitThroughGateHeader)ObjExitR.DBResponse.Data;
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

                    /*
                    List<GatePassList> LstGatePass = new List<GatePassList>();
                    ObjExitR.GetGatePassLst(Convert.ToDateTime(strDtae).ToString("yyyy-MM-dd"));
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
                    }*/



                }
            }
            return PartialView(ObjExitThroughGateHeader);


        }

        [HttpGet]
        public ActionResult EditExitThroughGateDetails(int ExitIdDtls)
        {
            WljExitThroughGateDetails ObjExitThroughGateDetails = new WljExitThroughGateDetails();
            WljExitThroughGateRepository ObjExitR = new WljExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WljExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.EditMode = "Edit";

            List<Wljcontainer> Lstcontainer = new List<Wljcontainer>();
            ObjExitR.GetContainer();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            //}
            //ViewBag.Lstcontainer = Lstcontainer;
            if (ObjExitR.DBResponse.Data != null)
            {
                ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((WljExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
            }

            ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

            ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

            return PartialView(ObjExitThroughGateDetails);


        }

        [HttpPost]
        public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        {
            WljExitThroughGateDetails ObjExitThroughGateDetails = new WljExitThroughGateDetails();
            WljExitThroughGateRepository ObjExitR = new WljExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WljExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }

            return PartialView(ObjExitThroughGateDetails);


        }

        public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        {
            WljExitThroughGateDetails ObjExitThroughGateDetails = new WljExitThroughGateDetails();
            WljExitThroughGateRepository ObjExitR = new WljExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WljExitThroughGateDetails)ObjExitR.DBResponse.Data;
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

            WljExitThroughGateHeader ObjExitThroughGateHeader = new WljExitThroughGateHeader();
            WljExitThroughGateRepository ObjExittGateR = new WljExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjExittGateR.GetExitThroughGate(ExitIdHdr);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WljExitThroughGateHeader)ObjExittGateR.DBResponse.Data;
                }
            }
            ViewBag.ViewMode = "view";
            ViewBag.HeaderId = ExitIdHdr;
            //return PartialView("CreateExitThroughGate", ObjExitThroughGateHeader);
            return PartialView(ObjExitThroughGateHeader);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGate(int ExitId)
        {
            if (ExitId > 0)
            {
                WljExitThroughGateRepository ObjGOR = new WljExitThroughGateRepository();
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
                WljExitThroughGateRepository objExit = new WljExitThroughGateRepository();
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

                        WljExitThroughGateRepository etgr = new WljExitThroughGateRepository();
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
            WljRevalidateGatePass objRevalidateGatePass = new Models.WljRevalidateGatePass();

            WljExitThroughGateRepository ObjGOR = new WljExitThroughGateRepository();
            ObjGOR.GetGatePassLstToRevalidate();

            if (ObjGOR.DBResponse.Data != null)
            {
                ViewBag.GpLstJson = JsonConvert.SerializeObject((List<WljRevalidateGatePass>)ObjGOR.DBResponse.Data);
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

        public JsonResult SaveRevalidateGatePass(WljRevalidateGatePass objRevalidateGatePass)
        {
            if (ModelState.IsValid)
            {
                WljExitThroughGateRepository ObjGOR = new WljExitThroughGateRepository();
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

        #region SCMTR

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult getExitDetailsListSCMTRWLJ(int HeaderId, string ViewMode = "")
        {
            ExitThroughGateRepository ETGR = new ExitThroughGateRepository();
            ETGR.GetExitThroughGateDetailsForHdrWLJ(HeaderId);
            List<WLJGateExitSCMTR> ListExitThroughGateDetails = new List<WLJGateExitSCMTR>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateDetails = (List<WLJGateExitSCMTR>)ETGR.DBResponse.Data;
            }
            if (ViewMode == "")
                return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
            else return Json(ListExitThroughGateDetails, JsonRequestBehavior.AllowGet);

        }


        #endregion
    }
}