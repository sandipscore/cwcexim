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
using SCMTRLibrary;
using System.Data;
using CwcExim.Areas.Export.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;


namespace CwcExim.Areas.GateOperation.Controllers
{
    public class WFLD_GateExitController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: GateOperation/Ppg_GateExit
        #region Exit Through Gate Ppg
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateExitThroughGate()
        {
            WFLDExitThroughGateRepository ObjETR = new WFLDExitThroughGateRepository();
            WFLDExitThroughGateHeader objExitThroughGateHeader = new WFLDExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (WFLDExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                
            }

            return PartialView(objExitThroughGateHeader);
        }


        [HttpGet]
        public JsonResult GetGatePass()
        {
            WFLDExitThroughGateRepository ObjETR = new WFLDExitThroughGateRepository();

           
            List<dynamic> objImp2 = new List<dynamic>();
            ObjETR.GetGatePassLst();
            if (ObjETR.DBResponse.Data != null)
            {
                ((List<WFLDGatePassList>)ObjETR.DBResponse.Data).ToList().ForEach(item =>
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
            WFLDExitThroughGateRepository ObjGOR = new WFLDExitThroughGateRepository();
            List<WFLDcontainerAgainstGp> LstcontainerAgainstGp = new List<WFLDcontainerAgainstGp>();

            ObjGOR.ContainerForGAtePass_Ppg(GPId);//, objLogin.Uid
            if (ObjGOR.DBResponse.Data != null)
            {
                LstcontainerAgainstGp = (List<WFLDcontainerAgainstGp>)ObjGOR.DBResponse.Data;
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
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.GetAllExitThroughGateList(0);
            List<WFLDExitThroughGateHeader> ListExitThroughGateHeader = new List<WFLDExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WFLDExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public ActionResult getExitHeaderSearchList(string ContainerNo)
        {
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.GetExitThroughGateSearchList(ContainerNo);
            List<WFLDExitThroughGateHeader> ListExitThroughGateHeader = new List<WFLDExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WFLDExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public JsonResult getExitHeaderListData(int Page)
        {
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.GetAllExitThroughGateList(Page);
            List<WFLDExitThroughGateHeader> ListExitThroughGateHeader = new List<WFLDExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WFLDExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            
            return Json(ListExitThroughGateHeader, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult getExitDetailsList(int HeaderId, string ViewMode = "")
        {
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.GetExitThroughGateDetailsForHdr(HeaderId);
            List<WFLDExitThroughGateDetails> ListExitThroughGateDetails = new List<WFLDExitThroughGateDetails>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateDetails = (List<WFLDExitThroughGateDetails>)ETGR.DBResponse.Data;
            }
            if (ViewMode == "")
                return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);
            else return Json( ListExitThroughGateDetails ,JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGate(WFLDExitThroughGateHeader objETG)
        {
            string ExitTime="";
            int ExitHourCal = 0;
            if (objETG.Module == "IMPDeli" || objETG.Module == "IMPYard" || objETG.Module == "EC" || objETG.Module == "ECGodn" || objETG.Module == "BNDadv" || objETG.Module == "BND")
            {
                ModelState.Remove("expectedTimeOfArrival");

            }
            if (ModelState.IsValid)
            {
                //var GateExitlst = (dynamic)null;
                String GateExitlst = objETG.StrExitThroughGateDetails;
                string[] Time = objETG.Time.Split(':');
                string ExitHour = Time[0];
                string ExitTimeDet = Time[1];
                string Exitminute = ExitTimeDet.Substring(0, 2);
                string TimePref= ExitTimeDet.Substring(ExitTimeDet.Length-2);
                if(TimePref == "PM" && Convert.ToInt32(ExitHour) <12)
                {
                     ExitHourCal = Convert.ToInt32(ExitHour) + 12;
                     ExitTime = Convert.ToString(ExitHourCal) +":"+ Exitminute;
                }
                else
                {
                     ExitTime= objETG.Time.Substring(0, 5);
                }
                if (objETG.ExitIdHeader == 0)
                {
                   // ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }
                if (ExitTime.Length == 7)
                {
                  //  ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                }

                string strEntryDateTime = objETG.GateExitDateTime + " " + ExitTime;
                objETG.GateExitDateTime = strEntryDateTime;

                //DateTime EntrydateTime = DateTime.ParseExact(strEntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);


                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<WFLDExitThroughGateDetails> LstExitThroughGateDetails = new List<WFLDExitThroughGateDetails>();
                // var x= Newtonsoft.\Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                WFLDExitThroughGateRepository objETGR = new WFLDExitThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {
                    //var js = new JavaScriptSerializer();
                    //var arrExitThroughGateDetails = js.Deserialize<ExitThroughGateDetails[]>(objETG.StrExitThroughGateDetails);

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
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
        public JsonResult AddToDetailsGateExit(WFLDExitThroughGateHeader objETG)
        {
            if (ModelState.IsValid)
            {
                string GateExitlst = objETG.StrExitThroughGateDetails;
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<WFLDExitThroughGateDetails> LstExitThroughGateDetails = new List<WFLDExitThroughGateDetails>();
                // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
                string XML = Utility.CreateXML(LstExitThroughGateDetails);
                WFLDExitThroughGateRepository objETGR = new WFLDExitThroughGateRepository();
                objETG.StrExitThroughGateDetails = XML;
                objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


                if (objETGR.DBResponse.Status == 1)
                {

                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDExitThroughGateDetails>>(GateExitlst);
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

        

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult EditExitThroughGate(int ExitIdHdr)
        {
            WFLDExitThroughGateHeader ObjExitThroughGateHeader = new WFLDExitThroughGateHeader();
            WFLDExitThroughGateRepository ObjExitR = new WFLDExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WFLDExitThroughGateHeader)ObjExitR.DBResponse.Data;
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


                    List<WFLDGatePassList> LstGatePass = new List<WFLDGatePassList>();
                    ObjExitR.GetGatePassLst();
                    if (ObjExitR.DBResponse.Data != null)
                    {

                        LstGatePass = (List<WFLDGatePassList>)ObjExitR.DBResponse.Data;
                        ViewBag.LstGatePass = LstGatePass;
                    }
                    List<WFLDcontainer> Lstcontainer = new List<WFLDcontainer>();
                    ObjExitR.GetContainer();

                    if (ObjExitR.DBResponse.Data != null)
                    {
                        ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((WFLDExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
                    }



                }
            }
            return PartialView(ObjExitThroughGateHeader);


        }

        [HttpGet]
        public ActionResult EditExitThroughGateDetails(int ExitIdDtls)
        {
            WFLDExitThroughGateDetails ObjExitThroughGateDetails = new WFLDExitThroughGateDetails();
            WFLDExitThroughGateRepository ObjExitR = new WFLDExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WFLDExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.EditMode = "Edit";

            List<WFLDcontainer> Lstcontainer = new List<WFLDcontainer>();
            ObjExitR.GetContainer();
            //if (ObjETR.DBResponse.Data != null)
            //{
            //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
            //}
            //ViewBag.Lstcontainer = Lstcontainer;
            if (ObjExitR.DBResponse.Data != null)
            {
                ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((WFLDExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
            }

            ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

            ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

            return PartialView(ObjExitThroughGateDetails);


        }

        [HttpPost]
        public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        {
            WFLDExitThroughGateDetails ObjExitThroughGateDetails = new WFLDExitThroughGateDetails();
            WFLDExitThroughGateRepository ObjExitR = new WFLDExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WFLDExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }

            return PartialView(ObjExitThroughGateDetails);


        }

        public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        {
            WFLDExitThroughGateDetails ObjExitThroughGateDetails = new WFLDExitThroughGateDetails();
            WFLDExitThroughGateRepository ObjExitR = new WFLDExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (WFLDExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.ViewFlag = "v";
            return PartialView(ObjExitThroughGateDetails);


        }

       

        [HttpGet]
        public ActionResult ViewExitThroughGate(int ExitIdHdr)
        {

            WFLDExitThroughGateHeader ObjExitThroughGateHeader = new WFLDExitThroughGateHeader();
            WFLDExitThroughGateRepository ObjExittGateR = new WFLDExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjExittGateR.GetExitThroughGate(ExitIdHdr);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WFLDExitThroughGateHeader)ObjExittGateR.DBResponse.Data;
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
                WFLDExitThroughGateRepository ObjGOR = new WFLDExitThroughGateRepository();
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
                WFLDExitThroughGateRepository objExit = new WFLDExitThroughGateRepository();
                objExit.GetDetailsForGateExitMail(ShippingLineId);
                if (objExit.DBResponse.Data != null)
                {
                    //EmailDataModel ObjEmailDataModel = new EmailDataModel();
                    //ObjEmailDataModel.ReceiverEmail = ((EntryThroughGateMail)objExit.DBResponse.Data).Email;
                    var mailTo = ((WFLDEntryThroughGateMail)objExit.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j = 0; j < mailTo.Length; j++)
                    {
                        mailTo[j].Trim();
                    }

                    var FileName = ((WFLDEntryThroughGateMail)objExit.DBResponse.Data).FileName;
                    //ObjEmailDataModel.Subject = "Test Subject";
                    //ObjEmailDataModel.MailBody = "Exit Through Gate";
                    //List<string> containerList = new List<string>();
                    var excelData = ((WFLDEntryThroughGateMail)objExit.DBResponse.Data).lstExcelData;
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

                        WFLDExitThroughGateRepository etgr = new WFLDExitThroughGateRepository();
                        foreach (var ContainerNo in containerArray.Split(',').ToArray())
                        {
                            etgr.ExitMailStatus(ContainerNo);
                            if (etgr.DBResponse.Status == 1)
                            {
                                message = "Email Status Updated";
                            }
                           
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
            WFLDRevalidateGatePass objRevalidateGatePass = new Models.WFLDRevalidateGatePass();

            WFLDExitThroughGateRepository ObjGOR = new WFLDExitThroughGateRepository();
            ObjGOR.GetGatePassLstToRevalidate();

            if (ObjGOR.DBResponse.Data != null)
            {
                ViewBag.GpLstJson = JsonConvert.SerializeObject((List<WFLDRevalidateGatePass>)ObjGOR.DBResponse.Data);
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

        public JsonResult SaveRevalidateGatePass(WFLDRevalidateGatePass objRevalidateGatePass)
        {
            if (ModelState.IsValid)
            {
                WFLDExitThroughGateRepository ObjGOR = new WFLDExitThroughGateRepository();
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
            WFLDExitThroughGateRepository ObjETR = new WFLDExitThroughGateRepository();
            WFLDGateExitCBT ObjCBT = new WFLDGateExitCBT();
            WFLDExitThroughGateHeader objExitThroughGateHeader = new WFLDExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (WFLDExitThroughGateHeader)ObjETR.DBResponse.Data;
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
       //     List<WFLDCBTList> LstCBT = new List<WFLDCBTList>();
         //   ObjETR.GetCBTList();
         //   if (ObjETR.DBResponse.Data != null)
         //   {

           //     LstCBT = (List<WFLDCBTList>)ObjETR.DBResponse.Data;
          //      ViewBag.LstCBT = LstCBT;
          //  }
          //  else
           // {
           //     ViewBag.LstCBT = null;
           // }

            return PartialView();
        }



        [HttpGet]
        public JsonResult GetCBTfoExport()
        {
            WFLDExitThroughGateRepository ObjETR = new WFLDExitThroughGateRepository();
            // List<container> Lstcontainer = new List<container>();
          //  ReferenceNumberCCIN ObjReferenceNumber = new ReferenceNumberCCIN();

            ObjETR.GetCBTList();

           /* if (ObjETR.DBResponse.Data != null)
                return Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });*/
                var jsonResult = Json(ObjETR.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGateCBT(WFLDGateExitCBT objETG)
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



                WFLDExitThroughGateRepository objETGR = new WFLDExitThroughGateRepository();

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
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.GetAllExitThroughGateCBT();
            List<WFLDGateExitCBT> ListExitThroughGateHeader = new List<WFLDGateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WFLDGateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }


        [HttpGet]
        public ActionResult EditExitThroughGateCBT(int ExitId)
        {
            WFLDGateExitCBT ObjExitThroughGateHeader = new WFLDGateExitCBT();
            WFLDExitThroughGateRepository ObjExitR = new WFLDExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExitR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WFLDGateExitCBT)ObjExitR.DBResponse.Data;
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

            WFLDGateExitCBT ObjExitThroughGateHeader = new WFLDGateExitCBT();
            WFLDExitThroughGateRepository ObjExittGateR = new WFLDExitThroughGateRepository();
            if (ExitId > 0)
            {
                ObjExittGateR.EditViewExitThroughGateCBT(ExitId);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (WFLDGateExitCBT)ObjExittGateR.DBResponse.Data;
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
            WFLDExitThroughGateRepository ETGR = new WFLDExitThroughGateRepository();
            ETGR.SearchGateExitCBT(CBTNo);
            List<WFLDGateExitCBT> ListExitThroughGateHeader = new List<WFLDGateExitCBT>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<WFLDGateExitCBT>)ETGR.DBResponse.Data;
            }
            return PartialView("CBTGateExitList", ListExitThroughGateHeader);

        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteExitThroughGateCBT(int ExitId)
        {
            if (ExitId > 0)
            {
                WFLDExitThroughGateRepository ObjGOR = new WFLDExitThroughGateRepository();
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
            WFLDExitThroughGateRepository ObjER = new WFLDExitThroughGateRepository();
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

                    WFLDExitThroughGateRepository objExport = new WFLDExitThroughGateRepository();
                    objExport.GetCIMDPDetailsUpdateStatus(ExitId);


                }

                return Json(new { Status = 1, Message = "CIM DT File Send Successfully." });
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
            WFLDExitThroughGateRepository ObjER = new WFLDExitThroughGateRepository();
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

                    WFLDExitThroughGateRepository objExport = new WFLDExitThroughGateRepository();
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