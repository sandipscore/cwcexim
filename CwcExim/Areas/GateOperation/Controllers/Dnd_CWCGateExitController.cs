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
    public class Dnd_CWCGateExitController : Controller
    {
        // GET: GateOperation/Dnd_CWCGateExit
        #region Exit Through Gate D'Node
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateExitThroughGate(string date = "")
        {
            DndExitThroughGateRepository ObjETR = new DndExitThroughGateRepository();
            Dnd_ExitThroughGateHeader objExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (Dnd_ExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                List<GatePassList> LstGatePass = new List<GatePassList>();
                if (date == "")
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                objExitThroughGateHeader.GateExitDateTime = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                //  ObjETR.GetGatePassLst(date);
                ObjETR.GetGatePassLst();
                if (ObjETR.DBResponse.Data != null)
                {

                    LstGatePass = (List<GatePassList>)ObjETR.DBResponse.Data;
                    ViewBag.LstGatePass = LstGatePass;
                }


                List<container> Lstcontainer = new List<container>();
                ObjETR.GetContainer();
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
                //}
                //ViewBag.Lstcontainer = Lstcontainer;
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((Dnd_ExitThroughGateHeader)ObjETR.DBResponse.Data).containerList);
                }
            }

            return PartialView(objExitThroughGateHeader);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerForGatePass(int GPId)
        {
            DndExitThroughGateRepository ObjGOR = new DndExitThroughGateRepository();
            List<Dnd_ContainerForGp> LstcontainerAgainstGp = new List<Dnd_ContainerForGp>();

            ObjGOR.ContainerForGAtePass(GPId);//, objLogin.Uid
            if (ObjGOR.DBResponse.Data != null)
            {
                LstcontainerAgainstGp = (List<Dnd_ContainerForGp>)ObjGOR.DBResponse.Data;
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
            DndExitThroughGateRepository ETGR = new DndExitThroughGateRepository();
            ETGR.GetAllExitThroughGate();
            List<Dnd_ExitThroughGateHeader> ListExitThroughGateHeader = new List<Dnd_ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<Dnd_ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        public ActionResult SearchGateExit(string Value)
        {
            DndExitThroughGateRepository ETGR = new DndExitThroughGateRepository();
            ETGR.SearchGateExit(Value);
            List<Dnd_ExitThroughGateHeader> ListExitThroughGateHeader = new List<Dnd_ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<Dnd_ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult getExitDetailsList(int HeaderId)
        {
            DndExitThroughGateRepository ETGR = new DndExitThroughGateRepository();
            ETGR.GetExitThroughGateDetailsForHdr(HeaderId);
            List<Dnd_ExitThroughGateDetails> ListExitThroughGateDetails = new List<Dnd_ExitThroughGateDetails>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateDetails = (List<Dnd_ExitThroughGateDetails>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);

        }

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGate(Dnd_ExitThroughGateHeader objETG)
        {
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
                IList<Dnd_ExitThroughGateDetails> LstExitThroughGateDetails = new List<Dnd_ExitThroughGateDetails>();
                // var x= Newtonsoft.\Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                DndExitThroughGateRepository objETGR = new DndExitThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {
                    //var js = new JavaScriptSerializer();
                    //var arrExitThroughGateDetails = js.Deserialize<ExitThroughGateDetails[]>(objETG.StrExitThroughGateDetails);

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
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

                if (objETGR.DBResponse.Status == 1)
                {
                    //if (objETGR.DBResponse.Data != null)
                    //{
                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_ExitThroughGateDetails>>(GateExitlst);
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
        public JsonResult AddToDetailsGateExit(Dnd_ExitThroughGateHeader objETG)
        {
            if (ModelState.IsValid)
            {
                string GateExitlst = objETG.StrExitThroughGateDetails;
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<Dnd_ExitThroughGateDetails> LstExitThroughGateDetails = new List<Dnd_ExitThroughGateDetails>();
                // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
                string XML = Utility.CreateXML(LstExitThroughGateDetails);
                DndExitThroughGateRepository objETGR = new DndExitThroughGateRepository();
                objETG.StrExitThroughGateDetails = XML;
                objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


                if (objETGR.DBResponse.Status == 1)
                {

                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_ExitThroughGateDetails>>(GateExitlst);
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
            Dnd_ExitThroughGateHeader ObjExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            DndExitThroughGateRepository ObjExitR = new DndExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (Dnd_ExitThroughGateHeader)ObjExitR.DBResponse.Data;
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
            Dnd_ExitThroughGateDetails ObjExitThroughGateDetails = new Dnd_ExitThroughGateDetails();
            DndExitThroughGateRepository ObjExitR = new DndExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (Dnd_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
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
                ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((Dnd_ExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
            }

            ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

            ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

            return PartialView(ObjExitThroughGateDetails);


        }

        [HttpPost]
        public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        {
            Dnd_ExitThroughGateDetails ObjExitThroughGateDetails = new Dnd_ExitThroughGateDetails();
            DndExitThroughGateRepository ObjExitR = new DndExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (Dnd_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }

            return PartialView(ObjExitThroughGateDetails);


        }

        public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        {
            Dnd_ExitThroughGateDetails ObjExitThroughGateDetails = new Dnd_ExitThroughGateDetails();
            DndExitThroughGateRepository ObjExitR = new DndExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (Dnd_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
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

            Dnd_ExitThroughGateHeader ObjExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            DndExitThroughGateRepository ObjExittGateR = new DndExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjExittGateR.GetExitThroughGate(ExitIdHdr);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (Dnd_ExitThroughGateHeader)ObjExittGateR.DBResponse.Data;
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
                DndExitThroughGateRepository ObjGOR = new DndExitThroughGateRepository();
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
                DndExitThroughGateRepository objExit = new DndExitThroughGateRepository();
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

                        DndExitThroughGateRepository etgr = new DndExitThroughGateRepository();
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


        #region CBT Gate Exit

        [HttpGet]
        public ActionResult CreateCBTGateExit(string date = "")
        {
            DndExitThroughGateRepository ObjETR = new DndExitThroughGateRepository();
            DndGateExitCBT ObjCBT = new DndGateExitCBT();
            Dnd_ExitThroughGateHeader objExitThroughGateHeader = new Dnd_ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (Dnd_ExitThroughGateHeader)ObjETR.DBResponse.Data;
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
        public ActionResult AddEditExitThroughGateCBT(DndGateExitCBT objETG)
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



                DndExitThroughGateRepository objETGR = new DndExitThroughGateRepository();
               
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
            DndExitThroughGateRepository ETGR = new DndExitThroughGateRepository();
            ETGR.GetAllExitThroughGateCBT();
            List<DndGateExitCBT> ListExitThroughGateHeader = new List<DndGateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<DndGateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public ActionResult EditExitThroughGateCBT(int ExitId)
        {
            DndGateExitCBT ObjExitThroughGateHeader = new DndGateExitCBT();
            DndExitThroughGateRepository ObjExitR = new DndExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExitR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (DndGateExitCBT)ObjExitR.DBResponse.Data;
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

            DndGateExitCBT ObjExitThroughGateHeader = new DndGateExitCBT();
            DndExitThroughGateRepository ObjExittGateR = new DndExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExittGateR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (DndGateExitCBT)ObjExittGateR.DBResponse.Data;
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
            DndExitThroughGateRepository ETGR = new DndExitThroughGateRepository();
            ETGR.SearchGateExitCBT(CBTNo);
            List<DndGateExitCBT> ListExitThroughGateHeader = new List<DndGateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<DndGateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGateCBT(int ExitId)
        {
            if (ExitId > 0)
            {
                DndExitThroughGateRepository ObjGOR = new DndExitThroughGateRepository();
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

    }
}