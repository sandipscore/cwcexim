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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CwcExim.Areas.Export.Models;
using System.Threading.Tasks;
using System.Text;

namespace CwcExim.Areas.GateOperation.Controllers
{   
    public class VRN_CWCGateExitController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // GET: GateOperation/kol_CWCGateExit
        #region Exit Through Gate kolkata
        string containerArray = "";
        [HttpGet]
        public ActionResult CreateExitThroughGate(string date = "")
        {
            VRN_ExitThroughGateRepository ObjETR = new VRN_ExitThroughGateRepository();
            VRN_ExitThroughGateHeader objExitThroughGateHeader = new VRN_ExitThroughGateHeader();
            ObjETR.GetTime();
            if (ObjETR.DBResponse.Data != null)
            {
                objExitThroughGateHeader = (VRN_ExitThroughGateHeader)ObjETR.DBResponse.Data;
                string ExitTime = objExitThroughGateHeader.GateExitDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objExitThroughGateHeader.GateExitDateTime = ExitTimeArray[0];
                ViewBag.strTime = objExitThroughGateHeader.Time;
                ViewBag.ViewMode = "New";
                List<VRN_GatePassList> LstGatePass = new List<VRN_GatePassList>();
                if (date == "")
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                objExitThroughGateHeader.GateExitDateTime = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                ObjETR.GetGatePassLst(date);
                if (ObjETR.DBResponse.Data != null)
                {

                    LstGatePass = (List<VRN_GatePassList>)ObjETR.DBResponse.Data;
                    ViewBag.LstGatePass = LstGatePass;
                }


                List<VRNcontainer> Lstcontainer = new List<VRNcontainer>();
                ObjETR.GetContainer();
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    Lstcontainer = (List<container>)ObjETR.DBResponse.Data;
                //}
                //ViewBag.Lstcontainer = Lstcontainer;
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((VRN_ExitThroughGateHeader)ObjETR.DBResponse.Data).containerList);
                }
            }

            return PartialView(objExitThroughGateHeader);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerForGatePass(int GPId)
        {
            VRN_ExitThroughGateRepository ObjGOR = new VRN_ExitThroughGateRepository();
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
            VRN_ExitThroughGateRepository ETGR = new VRN_ExitThroughGateRepository();
            ETGR.GetAllExitThroughGate();
            List<VRN_ExitThroughGateHeader> ListExitThroughGateHeader = new List<VRN_ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<VRN_ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        public ActionResult SearchGateExit(string Value)
        {
            VRN_ExitThroughGateRepository ETGR = new VRN_ExitThroughGateRepository();
            ETGR.SearchGateExit(Value);
            List<VRN_ExitThroughGateHeader> ListExitThroughGateHeader = new List<VRN_ExitThroughGateHeader>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateHeader = (List<VRN_ExitThroughGateHeader>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateHdrList", ListExitThroughGateHeader);

        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult getExitDetailsList(int HeaderId)
        {
            VRN_ExitThroughGateRepository ETGR = new VRN_ExitThroughGateRepository();
            ETGR.GetExitThroughGateDetailsForHdr(HeaderId);
            List<VRN_ExitThroughGateDetails> ListExitThroughGateDetails = new List<VRN_ExitThroughGateDetails>();

            if (ETGR.DBResponse.Data != null)
            {
                ListExitThroughGateDetails = (List<VRN_ExitThroughGateDetails>)ETGR.DBResponse.Data;
            }
            return PartialView("ExitThroughGateDetailsList", ListExitThroughGateDetails);

        }

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditExitThroughGate(VRN_ExitThroughGateHeader objETG)
        {
            if (objETG.Module == "IMPDeli" || objETG.Module == "IMPYard" || objETG.Module == "EC" || objETG.Module == "IMPDestuff" || objETG.Module == "BNDadv" || objETG.Module == "BND")
            {
                ModelState.Remove("expectedTimeOfArrival");
             //   ModelState.Remove("ArrivalDate");

            }
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
                IList<VRN_ExitThroughGateDetails> LstExitThroughGateDetails = new List<VRN_ExitThroughGateDetails>();
                // var x= Newtonsoft.\Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                VRN_ExitThroughGateRepository objETGR = new VRN_ExitThroughGateRepository();
                if (objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\"}]" && objETG.StrExitThroughGateDetails != "[{\"ExitIdDtls\":\"\",\"Reefer\":\"false\",\"ShippingLineId\":0}]")
                {
                    //var js = new JavaScriptSerializer();
                    //var arrExitThroughGateDetails = js.Deserialize<ExitThroughGateDetails[]>(objETG.StrExitThroughGateDetails);

                    var arrExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);

                    var connos = arrExitThroughGateDetails.Select(o => o.ContainerNo).ToArray();
                    containerArray = string.Join(",", connos);
                    LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":true").Replace("\"Reefer\":\"false\"", "\"Reefer\":false"));
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
                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_ExitThroughGateDetails>>(GateExitlst);
                    arrExitThroughGateDetailsList.Where(y => y.OperationType == "Import").GroupBy(o => o.ShippingLineId).ToList().ForEach(item =>
                    {

                        //var sid = item.ShippingLineId.ToString();
                        SendExitMail(item.Key.ToString());

                    });


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
        public JsonResult AddToDetailsGateExit(VRN_ExitThroughGateHeader objETG)
        {
            if (ModelState.IsValid)
            {
                string GateExitlst = objETG.StrExitThroughGateDetails;
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                objETG.BranchId = BranchId;
                IList<VRN_ExitThroughGateDetails> LstExitThroughGateDetails = new List<VRN_ExitThroughGateDetails>();
                // var x= Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\""));//.Replace("\"Reefer\":\"true\"", "\"Reefer\":\"1\"").Replace("\"Reefer\":\"false\"", "\"Reefer\":\"0\"")
                LstExitThroughGateDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_ExitThroughGateDetails>>(objETG.StrExitThroughGateDetails);
                string XML = Utility.CreateXML(LstExitThroughGateDetails);
                VRN_ExitThroughGateRepository objETGR = new VRN_ExitThroughGateRepository();
                objETG.StrExitThroughGateDetails = XML;
                objETGR.AddGateEXitToDetails(objETG, ((Login)(Session["LoginUser"])).Uid);


                if (objETGR.DBResponse.Status == 1)
                {

                    var arrExitThroughGateDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_ExitThroughGateDetails>>(GateExitlst);
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
            VRN_ExitThroughGateHeader ObjExitThroughGateHeader = new VRN_ExitThroughGateHeader();
            VRN_ExitThroughGateRepository ObjExitR = new VRN_ExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdHdr);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (VRN_ExitThroughGateHeader)ObjExitR.DBResponse.Data;
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


                }
            }
            return PartialView(ObjExitThroughGateHeader);


        }

        [HttpGet]
        public ActionResult EditExitThroughGateDetails(int ExitIdDtls)
        {
            VRN_ExitThroughGateDetails ObjExitThroughGateDetails = new VRN_ExitThroughGateDetails();
            VRN_ExitThroughGateRepository ObjExitR = new VRN_ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (VRN_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.EditMode = "Edit";

            List<VRNcontainer> Lstcontainer = new List<VRNcontainer>();
            ObjExitR.GetContainer();
           
            if (ObjExitR.DBResponse.Data != null)
            {
                ViewBag.LstcontainerJson = JsonConvert.SerializeObject(((VRN_ExitThroughGateHeader)ObjExitR.DBResponse.Data).containerList);
            }

            ViewBag.ShippingLineId = ObjExitThroughGateDetails.ShippingLineId;

            ViewBag.cfsCode = ObjExitThroughGateDetails.CFSCode;

            return PartialView(ObjExitThroughGateDetails);


        }

        [HttpPost]
        public ActionResult EditExitThroughGatDetls(int ExitIdDtls)
        {
            VRN_ExitThroughGateDetails ObjExitThroughGateDetails = new VRN_ExitThroughGateDetails();
            VRN_ExitThroughGateRepository ObjExitR = new VRN_ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.GetExitThroughGate(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (VRN_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }

            return PartialView(ObjExitThroughGateDetails);


        }

        public ActionResult ViewExitThroughGatDetls(int ExitIdDtls)
        {
            VRN_ExitThroughGateDetails ObjExitThroughGateDetails = new VRN_ExitThroughGateDetails();
            VRN_ExitThroughGateRepository ObjExitR = new VRN_ExitThroughGateRepository();
            if (ExitIdDtls > 0)
            {
                ObjExitR.EditExitThroughGateDetails(ExitIdDtls);
                if (ObjExitR.DBResponse.Data != null)
                {
                    ObjExitThroughGateDetails = (VRN_ExitThroughGateDetails)ObjExitR.DBResponse.Data;
                }
            }
            ViewBag.ViewFlag = "v";
            return PartialView(ObjExitThroughGateDetails);


        }
        
        [HttpGet]
        public ActionResult ViewExitThroughGate(int ExitIdHdr)
        {

            VRN_ExitThroughGateHeader ObjExitThroughGateHeader = new VRN_ExitThroughGateHeader();
            VRN_ExitThroughGateRepository ObjExittGateR = new VRN_ExitThroughGateRepository();
            if (ExitIdHdr > 0)
            {
                //EntryThroughGateRepository ObjGOR = new EntryThroughGateRepository();
                ObjExittGateR.GetExitThroughGate(ExitIdHdr);
                if (ObjExittGateR.DBResponse.Data != null)
                {
                    ObjExitThroughGateHeader = (VRN_ExitThroughGateHeader)ObjExittGateR.DBResponse.Data;
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
                VRN_ExitThroughGateRepository ObjGOR = new VRN_ExitThroughGateRepository();
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
                VRN_ExitThroughGateRepository objExit = new VRN_ExitThroughGateRepository();
                objExit.GetDetailsForGateExitMail(ShippingLineId);
                if (objExit.DBResponse.Data != null)
                {
                    
                    var mailTo = ((EntryThroughGateMail)objExit.DBResponse.Data).Email.Replace(" ", "").Split(',');
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int j = 0; j < mailTo.Length; j++)
                    {
                        mailTo[j].Trim();
                    }

                    var FileName = ((EntryThroughGateMail)objExit.DBResponse.Data).FileName;
                   
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

                        VRN_ExitThroughGateRepository etgr = new VRN_ExitThroughGateRepository();
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
            VRN_RevalidateGatePass objRevalidateGatePass = new Models.VRN_RevalidateGatePass();

            VRN_ExitThroughGateRepository ObjGOR = new VRN_ExitThroughGateRepository();
            ObjGOR.GetGatePassLstToRevalidate();

            if (ObjGOR.DBResponse.Data != null)
            {
                ViewBag.GpLstJson = JsonConvert.SerializeObject((List<VRN_RevalidateGatePass>)ObjGOR.DBResponse.Data);
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

        public JsonResult SaveRevalidateGatePass(VRN_RevalidateGatePass objRevalidateGatePass)
        {
            if (ModelState.IsValid)
            {
                VRN_ExitThroughGateRepository ObjGOR = new VRN_ExitThroughGateRepository();
                ObjGOR.UpdateGatePassValidity(objRevalidateGatePass);
                ModelState.Clear();
                return Json(ObjGOR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
          

        }

        #endregion

        #region Send DT


        public async Task<JsonResult> SendDT(int ExitId = 0)
        {
            int k = 0;
            int j = 1;
            VRN_ExitThroughGateRepository ObjER = new VRN_ExitThroughGateRepository();
            
            ObjER.GetDTDetails(ExitId, "F");
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

                    VRN_ExitThroughGateRepository objExport = new VRN_ExitThroughGateRepository();
                    objExport.GetCIMDTDetailsUpdateStatus(ExitId);


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
            VRN_ExitThroughGateRepository ObjER = new VRN_ExitThroughGateRepository();
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

                    VRN_ExitThroughGateRepository objExport = new VRN_ExitThroughGateRepository();
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