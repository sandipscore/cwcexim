using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using System.Web.Configuration;
using CwcExim.Filters;
using Newtonsoft.Json;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CwcExim.Areas.Import.Controllers
{
    public class Kol_CWCImportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IGM Upload

        [HttpGet]
        public ActionResult Kol_ImportIGM()
        {
            Kol_ImportIGMModel objImportIGM = new Kol_ImportIGMModel();
            Kol_ImportRepository objImport = new Kol_ImportRepository();

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objImportIGM.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;

            return PartialView(objImportIGM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Kol_UploadIGM()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    foreach (string fileName in Request.Files)
                    {
                        var rndfilename = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".IGM";
                        HttpPostedFileBase file = Request.Files[fileName];
                        string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                        string FolderPath = Server.MapPath("~/Uploads/IGM/" + UID);
                        if (!System.IO.Directory.Exists(FolderPath))
                        {
                            System.IO.Directory.CreateDirectory(FolderPath);
                        }
                        file.SaveAs(FolderPath + "\\" + rndfilename);
                        Session["UploadedIGM"] = rndfilename;

                    }
                    return Json(new { Status = 1, Message = "File Uploaded" }, JsonRequestBehavior.DenyGet);
                }
                else
                    return Json(new { Status = 0, Message = "No file detected" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = -1, Message = "Server Error" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Kol_ImportIGM(Kol_ImportIGMModel m)
        {
            /*string cookieToken, formToken;
            string oldCookieToken = Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName] == null ? null : Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName].Value;
            System.Web.Helpers.AntiForgery.GetTokens(oldCookieToken, out cookieToken, out formToken);*/
            //StreamWriter sw = new StreamWriter(@"E:\IGMLog.log", false);
            int lineNo = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    #region Datatable

                    DataSet dsCargo = new DataSet("Cargo");
                    DataTable dtCargo = new DataTable("CargoDetails");
                    DataSet dsContainer = new DataSet("Container");
                    DataTable dtContainer = new DataTable("ContainerDetails");
                    /*dtCargo.Columns.Add("VesselNo");
                    dtCargo.Columns.Add("VoyageNo");*/
                    dtCargo.Columns.Add("LineNo");
                    dtCargo.Columns.Add("BLNo");
                    dtCargo.Columns.Add("PartyName");
                    dtCargo.Columns.Add("PartyAddress");
                    dtCargo.Columns.Add("PartyName2");
                    dtCargo.Columns.Add("PartyAddress2");
                    dtCargo.Columns.Add("ShippingLinePan");
                    dtCargo.Columns.Add("PacketQty");
                    dtCargo.Columns.Add("PacketUnit");
                    dtCargo.Columns.Add("WeightQty");
                    dtCargo.Columns.Add("WeightUnit");
                    dtCargo.Columns.Add("Commodity");
                    //new added Subir

                    dtCargo.Columns.Add("TrBondNoCargo");

                    //


                    dtContainer.Columns.Add("ContainerNo");
                    dtContainer.Columns.Add("SealNo");
                    dtContainer.Columns.Add("LineNo");
                    dtContainer.Columns.Add("Fcl");
                    dtContainer.Columns.Add("Height");

                    //new added Subir

                    //dtCargo.Columns.Add("TrBondNoContainer");
                    //

                    string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                    string FolderPath = Server.MapPath("~/Uploads/IGM/" + UID);

                    string Filepath = FolderPath + "\\" + Session["UploadedIGM"].ToString();
                    #endregion

                 


                    if (System.IO.File.Exists(Filepath))
                    {
                        #region File Handling
                        StreamReader sr = System.IO.File.OpenText(Filepath);
                        
                        string ContentType = "";
                        
                        while (sr.Peek() != -1)
                        {
                            lineNo = lineNo + 1;
                            //sw.WriteLine(i.ToString());

                            string strContents = sr.ReadLine();
                            strContents = strContents.Replace("'", "''");
                            string strhead = strContents.Substring(0, 4);
                            if (strhead == "HREC" || strContents == "<manifest>" || strContents == "<END-vesinfo>" || strContents == "<END-cargo>" || strContents == "<END-contain>" || strContents == "<END-manifest>")
                            {
                                continue;
                            }
                            else if (strContents == "<vesinfo>")
                            {
                                ContentType = "Vessel";
                                continue;
                            }
                            else if (strContents == "<cargo>")
                            {
                                ContentType = "Cargo";
                                continue;
                            }
                            else if (strContents == "<contain>")
                            {
                                ContentType = "Container";
                                continue;
                            }
                            else if (strhead == "TREC")
                            {
                                break;
                            }
                            else
                            {
                                string[] fields = strContents.Split((char)29);
                                if (ContentType == "Cargo")
                                {
                                    DataRow dr = dtCargo.NewRow();
                                    /*dr["VesselNo"] = fields[3];
                                    dr["VoyageNo"] = fields[4];*/
                                    dr["LineNo"] = fields[7];
                                    dr["BLNo"] = fields[9];

                                    dr["PartyName"] = fields[15];
                                    dr["PartyAddress"] = fields[16] + " " + fields[17] + " " + fields[18];

                                    if (string.IsNullOrEmpty(fields[19]))
                                    {
                                        dr["PartyName2"] = fields[15];
                                        dr["PartyAddress2"] = fields[16] + " " + fields[17] + " " + fields[18];
                                    }
                                    else
                                    {
                                        dr["PartyName2"] = fields[19];
                                        dr["PartyAddress2"] = fields[20] + " " + fields[21] + " " + fields[22];
                                    }

                                    dr["ShippingLinePan"] = fields[40];
                                    dr["PacketQty"] = fields[27];
                                    dr["PacketUnit"] = fields[28];
                                    dr["WeightQty"] = fields[29];
                                    dr["WeightUnit"] = fields[30];
                                    dr["Commodity"] = fields[34];
                                    // adding TrBond Subir
                                    dr["TrBondNoCargo"] = fields[37];
                                    // endAdding
                                    dtCargo.Rows.Add(dr);
                                    dtCargo.AcceptChanges();
                                }
                                else if (ContentType == "Container")
                                {
                                    DataRow dr = dtContainer.NewRow();
                                    dr["ContainerNo"] = fields[9];
                                    dr["SealNo"] = fields[10];
                                    dr["LineNo"] = fields[7];
                                    dr["Fcl"] = fields[12];
                                    string Height = fields[15].ToString().Substring(0, 2);
                                    if (int.Parse(Height) == 20 || int.Parse(Height) == 40)
                                        dr["Height"] = int.Parse(Height);
                                    else if (int.Parse(Height) == 22)
                                        dr["Height"] = 20;
                                    else if(int.Parse(Height) == 44 || int.Parse(Height) == 42)
                                        dr["Height"] = 40;
                                   
                                    // adding TrBond Subir
                                    // dr["TrBondNoContainer"] = fields[37];
                                    // endAdding

                                    dtContainer.Rows.Add(dr);
                                    dtContainer.AcceptChanges();
                                }
                                else
                                {
                                    continue;
                                }
                            }

                        }

                        // subir data table
                       
                        

                        //System.Data.DataView view = new System.Data.DataView(dtCargo);
                        //dtContainer =
                        //view.ToTable("Selected", true, "TrBondNoCargo");
                        // data table 

                        sr.Close();
                        sr.Dispose();
                        
                        #endregion

                        dsCargo.Tables.Add(dtCargo);
                        string CargoXML = dsCargo.GetXml();
                        dsContainer.Tables.Add(dtContainer);
                        string ContainerXML = dsContainer.GetXml();

                        int BranchId = Convert.ToInt32(Session["BranchId"]);
                        Kol_ImportRepository objIR = new Kol_ImportRepository();
                        objIR.SaveImportIgmFile(m.FileName, ((Login)(Session["LoginUser"])).Uid, m.VesselNo, m.VoyageNo, m.ShippingLineId, m.ShippingLineName, m.RotationNo, BranchId, CargoXML, ContainerXML);
                        if (objIR.DBResponse.Status == 1)
                        {
                            if (System.IO.Directory.Exists(FolderPath))
                            {
                                try
                                {
                                    System.IO.Directory.Delete(FolderPath, true);
                                    Session.Remove("UploadedIGM");
                                    dsCargo = null;
                                    dsContainer = null;
                                    return Json(new { Status = 1, Message = objIR.DBResponse.Message/*, Token= cookieToken*/ }, JsonRequestBehavior.DenyGet);
                                }
                                catch
                                {
                                    return Json(new { Status = 1, Message = objIR.DBResponse.Message }, JsonRequestBehavior.DenyGet);
                                }
                            }
                            else
                            {
                                return Json(new { Status = 1, Message = objIR.DBResponse.Message }, JsonRequestBehavior.DenyGet);
                            }
                        }
                        else
                        {
                            return Json(new { Status = 0, Message = objIR.DBResponse.Message }, JsonRequestBehavior.DenyGet);
                        }

                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "Error importing file" }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json(new { Status = 0, Message = "Invalid data submitted" }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "Server Error. IGM File Problem At Line Number. "+lineNo.ToString() }, JsonRequestBehavior.DenyGet);
            }
            finally
            {
                //sw.Close();
                //sw.Dispose();
            }
        }

        #endregion

        #region Form One

        [HttpGet]
        public ActionResult Kol_CreateFormOne()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddFormOne()
        {
            Kol_FormOneModel objFormOne = new Kol_FormOneModel();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");

            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;
            objImport.ListOfForeignLiner();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstForeignLiner = (List<ForeignLiner>)objImport.DBResponse.Data;
            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;
            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;
            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult Kol_GetFormOneList(string ContainerName="")
        {
            if (ContainerName==null || ContainerName == "" )
            {
                IEnumerable<Kol_FormOneModel> lstFormOne = new List<Kol_FormOneModel>();
                Kol_ImportRepository objImportRepo = new Kol_ImportRepository();
                objImportRepo.GetFormOne(0);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Kol_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<Kol_FormOneModel> lstFormOne = new List<Kol_FormOneModel>();
                Kol_ImportRepository objImportRepo = new Kol_ImportRepository();
                objImportRepo.GetFormOneByContainer(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Kol_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOne(Kol_FormOneModel objFormOne)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                   // objFormOne.FormOneDetailsJS.Replace("\"DateOfLanding: \":\"\"", "\"DateOfLanding\":\"null\"");
                    objFormOne.lstFormOneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Kol_FormOneDetailModel>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetail.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetail);
                    Kol_ImportRepository objImport = new Kol_ImportRepository();
                    objImport.AddEditFormOne(objFormOne, BranchId, XML, ((Login)(Session["LoginUser"])).Uid);
                    ModelState.Clear();
                    return Json(objImport.DBResponse);
                }
                else
                {
                    var Err = new { Status = -1, Message = "Error" };
                    return Json(Err);
                }
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditFormOne(int FormOneId)
        {
            Kol_FormOneModel objFormOne = new Kol_FormOneModel();
            Kol_ImportRepository objImport = new Kol_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Kol_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;

            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;

            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;

            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;

            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult ViewFormOne(int FormOneId)
        {
            Kol_FormOneModel objFormOne = new Kol_FormOneModel();
            Kol_ImportRepository objImport = new Kol_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Kol_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);
            return PartialView(objFormOne);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            List<Kol_FormOneModel> LstFormOne = new List<Kol_FormOneModel>();
            objImport.GetFormOne(Page);
            if (objImport.DBResponse.Data != null)
            {
                LstFormOne = (List<Kol_FormOneModel>)objImport.DBResponse.Data;
            }
            return Json(LstFormOne, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOne(int FormOneId)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOne(FormOneId);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public JsonResult PrintFormOne(int FormOneId)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            if (FormOneId > 0)
                objImport.FormOnePrint(FormOneId);
            var model = (Kol_FormOnePrintModel)objImport.DBResponse.Data;
            var printableData = (IList<Kol_FormOnePrintDetailModel>)model.lstFormOnePrintDetail;

            var fileName = model.FormOneNo + ".pdf";
            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
            string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);

            var cPdf = new CustomPdfGenerator();
            cPdf.GenerateKol(PdfDirectory + "/" + fileName, model.ShippingLineNo, model.BLNo, printableData);

            return Json(new { Status = 1, FileUrl = "../../Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateFormOne(FormCollection fc)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            try
            {
                var pages = new string[1];
                var fileName = fc["FormOne"].ToString() + ".pdf";
                string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4Landscape))
                {
                    rh.GeneratePDF(PdfDirectory + "/" + fileName, fc["Page1"].ToString());
                }
                return Json(new { Status = 1, Message = "/Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "" }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion



        #region Delivery Application

        [HttpGet]
        public ActionResult CreateDeliveryApplication()
        {
            ImportRepository ObjIR = new ImportRepository();
            BondRepository ObjBR = new BondRepository();
            ObjIR.GetDestuffEntryNo();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.DestuffingEntryNoList = ObjIR.DBResponse.Data;
            else
                ViewBag.DestuffingEntryNoList = null;
            ObjIR.ListOfCHA();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.CHAList = new SelectList((IList<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            else
                ViewBag.CHAList = null;
            ObjBR.ListOfImporter();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
            else
                ViewBag.ImporterList = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditDeliveryApplication(int DeliveryId)
        {
            ImportRepository ObjIR = new ImportRepository();
            BondRepository ObjBR = new BondRepository();
            DeliveryApplication ObjDelivery = new DeliveryApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (DeliveryApplication)ObjIR.DBResponse.Data;
                ObjIR.ListOfCHA();
                if (ObjIR.DBResponse.Data != null)
                    ViewBag.CHAList = new SelectList((IList<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                else
                    ViewBag.CHAList = null;
                ObjBR.ListOfImporter();
                if (ObjBR.DBResponse.Data != null)
                    ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
                else
                    ViewBag.ImporterList = null;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ViewDeliveryApplication(int DeliveryId)
        {
            ImportRepository ObjIR = new ImportRepository();
            DeliveryApplication ObjDelivery = new DeliveryApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (DeliveryApplication)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryApplication()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<DeliveryApplicationList> LstDelivery = new List<DeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication();
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryApplicationList", LstDelivery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryApplication(DeliveryApplication ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                ImportRepository ObjIR = new ImportRepository();
                string DeliveryXml = "";
               
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
                    ObjDelivery.LstDeliveryAppDtl.ForEach(x => x.BEODate = Convert.ToDateTime(x.BEODate).ToString("yyyy-MM-dd"));
                    DeliveryXml = Utility.CreateXML(ObjDelivery.LstDeliveryAppDtl);
                }
                ObjIR.AddEditDeliveryApplication(ObjDelivery, DeliveryXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetBOEDetForDeliveryApp(int DestuffingEntryDtlId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delivery Payment Sheet
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        /*
        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryCont(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryContForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }
        */

        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryBOE(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetBOEForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.BOEList = null;

            return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);

            //ImportRepository objImport = new ImportRepository();
            //string XMLText = "";
            //if (lstPaySheetContainer != null)
            //{
            //    XMLText = Utility.CreateXML(lstPaySheetContainer);
            //}

            //objImport.GetBOEForPaymentSheet(XMLText);
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.BOEList = null;

            //return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);
        }

      

        [HttpPost]
        public JsonResult GetDelPaymentSheet(string InvoiceDate, string InvoiceType, string SEZ,int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
          int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
          string PayeeName,decimal Weight, List<PaymentSheetBOE> lstPaySheetBOE, decimal mechanical, decimal manual, int distance, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheet(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, Weight, InvoiceType, XMLText, mechanical, manual, distance, InvoiceId);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            /*Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;*/
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            /*Output.RoundUp = 0;*/
            Output.InvoiceAmt = Output.AllTotal;
            return Json(Output);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ImportRepository objImport = new ImportRepository();
                //objImport.AddEditContainerInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objImport.DBResponse);
                Decimal Weight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Weight"]) ? "0" : objForm["Weight"]);
                decimal MechanicalWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Mechanical"]) ? "0" : objForm["Mechanical"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Manual"]) ? "0" : objForm["Manual"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["distance"]) ? "0" : objForm["distance"]);
                decimal Incentive = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Incentive"]) ? "0" : objForm["Incentive"]);
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if(invoiceData.lstPreInvoiceCargo!=null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", Weight, ExportUnder, MechanicalWeight, ManualWeight, Distance, CargoXML, Incentive);
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        #endregion


        #region appraisement By invoice 
        public ActionResult GetAppraisementNo()
        {
            List<AllInvoiceListForappraisemnt> listInvoice = new List<AllInvoiceListForappraisemnt>();
            Kol_ImportRepository objKol_ImportRepository = new Kol_ImportRepository();
            objKol_ImportRepository.GetInvoiceList();
            if (objKol_ImportRepository.DBResponse.Data != null)
            {
                listInvoice = (List<AllInvoiceListForappraisemnt>)objKol_ImportRepository.DBResponse.Data;
                ViewBag.listInvoice = Newtonsoft.Json.JsonConvert.SerializeObject(listInvoice);
            }
            else
            {
                ViewBag.listInvoice = null;
            }
            return PartialView("getAppraisementByInvoice");
        }
        #endregion


        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            Kol_ImportRepository objImport = new Kol_ImportRepository();
          
            objImport.GetImpPaymentPartyForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objImport.GetImpPaymentPayeeForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPayee = Jobject["lstParty"];
                ViewBag.StatePayee = Jobject["State"];
            }
            else
            {
                ViewBag.lstPayee = null;
            }

            return PartialView();
        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetEmptyContainerListForInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.EmptyContList = null;

            return Json(ViewBag.EmptyContList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPayeeNameByPartyCode(string PartyCode)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetImpPaymentPayeeForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeLists(string PartyCode, int Page)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetImpPaymentPayeeForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }





        /*
                [HttpGet]
                public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
                {
                    Wfld_ImportRepository objImport = new Wfld_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, 0);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

                [HttpGet]
                public JsonResult LoadPartyLists(string PartyCode, int Page)
                {
                    Wfld_ImportRepository objImport = new Wfld_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, Page);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

        */





        [HttpGet]
        public JsonResult PartyBinding()
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            objImport.GetPaymentPartyForImportInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetEmptyCont(string InvoiceFor, int AppraisementId)
        {
            Kol_ImportRepository objImport = new Kol_ImportRepository();
            //objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            objImport.GetEmptyContByEntryId(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId,
            List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, string SEZ)
        {

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            Kol_ImportRepository objImport = new Kol_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId, SEZ);
            var Output = (KolInvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;

            Output.Module = "EC";
            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                if (!Output.BOEDate.Contains(item.BOEDate))
                    Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new KOLPostPaymentContainer
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });
                }


                Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
                Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
                Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                    + Output.lstPrePaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
                Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
                Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
                Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.HTTotal = 0;
                Output.CWCTDS = 0;
                Output.HTTDS = 0;
                Output.CWCTDSPer = 0;
                Output.HTTDSPer = 0;
                Output.TDS = 0;
                Output.TDSCol = 0;
                Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.InvoiceAmt =Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp =  Output.InvoiceAmt - Output.AllTotal;
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECDeliveryPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<KolInvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
                string SEZ = "";
                SEZ = objForm["SEZ1"].ToString();

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "EC";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                Kol_ImportRepository objImport = new Kol_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objImport.AddEditEmptyContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor, SEZ);


                invoiceData.InvoiceNo = Convert.ToString(objImport.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                objImport.DBResponse.Data = invoiceData;
                return Json(objImport.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion
        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            Kol_ImportRepository objER = new Kol_ImportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<CwcExim.Areas.Export.Models.WFLDListOfExpInvoice> obj = new List<CwcExim.Areas.Export.Models.WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CwcExim.Areas.Export.Models.WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        [HttpGet]
        public ActionResult ListLoadMoreExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            Kol_ImportRepository objER = new Kol_ImportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<WFLDListOfExpInvoice> obj = new List<WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region --FOIS Data--

        [HttpGet]
        public ActionResult FOISData()
        {
            List<AllInvoiceList> listInvoice = new List<AllInvoiceList>();
            Kol_ImportRepository objKol_ImportRepository = new Kol_ImportRepository();
            objKol_ImportRepository.GetInvoiceNoList();
            if (objKol_ImportRepository.DBResponse.Data != null)
            {
                listInvoice = (List<AllInvoiceList>)objKol_ImportRepository.DBResponse.Data;
                ViewBag.listInvoice = Newtonsoft.Json.JsonConvert.SerializeObject(listInvoice);
            }
            else
            {
                ViewBag.listInvoice = null;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetFoisData(string InvoiceNo)
        {
            Kol_ImportRepository objKol = new Kol_ImportRepository();
            objKol.GetFoisData(InvoiceNo);
            return Json(objKol.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditRakeCargo(RakeWagonHdr Objfd)
        {
            if(!string.IsNullOrEmpty(Objfd.WgonDetailsJS))
            {
                IList<WgonDtl> lstWagonDetails = JsonConvert.DeserializeObject<IList<WgonDtl>>(Objfd.WgonDetailsJS);
                foreach (var item in lstWagonDetails)
                {
                    if (item.ContTareWt == "")
                    {
                        item.ContTareWt = "0";
                    }
                    if (item.ContLodWt == "")
                    {
                        item.ContLodWt = "0";
                    }
                    
                }
                var pxml = Utility.CreateXML(lstWagonDetails);
                Kol_ImportRepository objKol = new Kol_ImportRepository();
                objKol.AddEditRakeCargo(Objfd, pxml, ((Login)(Session["LoginUser"])).Uid);
                return Json(objKol.DBResponse);
            }
            else
            {               
                
                return Json(new { Status = 0, Message = "Error" });
            }
           
        }

        [HttpGet]
        public ActionResult ListOfFOISEntry()
        {
            Kol_ImportRepository objKol = new Kol_ImportRepository();
            List<RakeWagonHdr> lstFOISEntry = new List<RakeWagonHdr>();
            objKol.GetAllFOISEntry(0);
            if (objKol.DBResponse.Data != null)
                lstFOISEntry = (List<RakeWagonHdr>)objKol.DBResponse.Data;
            return PartialView(lstFOISEntry);
        }

        [HttpGet]
        public ActionResult ViewFOISData(int RWHdrId)
        {
            RakeWagonHdr objFOIS = new RakeWagonHdr();
            Kol_ImportRepository objImport = new Kol_ImportRepository();

            objImport.GetFOISEntryById(RWHdrId);
            if (objImport.DBResponse.Data != null)
                objFOIS = (RakeWagonHdr)objImport.DBResponse.Data;

            if (objFOIS.lstWgon != null)
                objFOIS.WgonDetailsJS = JsonConvert.SerializeObject(objFOIS.lstWgon);
            return PartialView(objFOIS);
        }

        [HttpGet]
        public ActionResult EditFOISEntry(int RWHdrId)
        {
            RakeWagonHdr objFOIS = new RakeWagonHdr();
            Kol_ImportRepository objImport = new Kol_ImportRepository();

            objImport.GetFOISEntryById(RWHdrId);
            if (objImport.DBResponse.Data != null)
                objFOIS = (RakeWagonHdr)objImport.DBResponse.Data;

            if (objFOIS.lstWgon != null)
                objFOIS.WgonDetailsJS = JsonConvert.SerializeObject(objFOIS.lstWgon);
            return PartialView(objFOIS);
        }

        [HttpPost]
        public JsonResult AddExcelDataToTempTable(TempRakeWagonHdr ObjData)
        {
            if (!string.IsNullOrEmpty(ObjData.WgonDetailsJS))
            {
                IList<TempWgonDtl> lstWagonDetails = JsonConvert.DeserializeObject<IList<TempWgonDtl>>(ObjData.WgonDetailsJS);
                var pxml = Utility.CreateXML(lstWagonDetails);
                Kol_ImportRepository objKol = new Kol_ImportRepository();
                objKol.AddExcelDataToTempTable(ObjData, pxml);
                return Json(objKol.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { Status = 0, Message = "Error" });
            }

        }

        #endregion

        #region Send Loading Detail
        public async Task<JsonResult> SendLoadingDetails(int RWHdrId = 0)
        {
            try
            {
                string desc = "";
                string msg = "";
                Kol_ImportRepository ObjER = new Kol_ImportRepository();

                ObjER.GetRakeLoadingDetails(RWHdrId);
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;

                    Kol_RakeLoadingDetails objRLD = new Kol_RakeLoadingDetails();

                    RakeAuthentication objRA = new RakeAuthentication();
                    RakeHeader objHdr = new RakeHeader();
                    List<RakeDetails> lstRD = new List<RakeDetails>();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objRA.org = Convert.ToString(dr["org"]);
                            objRA.userid = Convert.ToString(dr["userid"]);
                            objRA.pswd = Convert.ToString(dr["pswd"]);

                            objHdr.sttnfrom = Convert.ToString(dr["sttnfrom"]);
                            objHdr.sttnto = Convert.ToString(dr["sttnto"]);
                            objHdr.invcid = Convert.ToString(dr["invcid"]);
                            objHdr.rakeid = Convert.ToString(dr["rakeid"]);
                            objHdr.rakename = Convert.ToString(dr["rakename"]);
                            objHdr.oprgplcttime = Convert.ToString(dr["oprgplcttime"]);
                            objHdr.relstime = Convert.ToString(dr["relstime"]);
                            objHdr.noofwgon = Convert.ToString(dr["noofwgon"]);
                        }

                        objRLD.auth = objRA;
                        objRLD.rakehdr = objHdr;

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                lstRD.Add(new RakeDetails
                                {
                                    wgonid = Convert.ToString(item["wgonid"]),
                                    sttnfrom = Convert.ToString(item["sttnfrom"]),
                                    sttnto = Convert.ToString(item["sttnto"]),
                                    contnumb = Convert.ToString(item["contnumb"]),
                                    contleflag = Convert.ToString(item["contleflag"]),
                                    contsize = Convert.ToString(item["contsize"]),
                                    contposn = Convert.ToString(item["contposn"]),
                                    cmdtload = Convert.ToString(item["cmdtload"]),
                                    trfctype = Convert.ToString(item["trfctype"]),
                                    cmdtstatcode = Convert.ToString(item["cmdtstatcode"]),
                                    conttarewght = Convert.ToString(item["conttarewght"]),
                                    contloadwght = Convert.ToString(item["contloadwght"]),
                                    smtpno = Convert.ToString(item["smtpno"]),
                                    smtpdate = Convert.ToString(item["smtpdate"]),
                                    hsncode = Convert.ToString(item["hsncode"])

                                });
                            }
                            objRLD.wgondtls = lstRD;

                        }

                    }

                    if(objRLD!=null)
                    {
                        string json = JsonConvert.SerializeObject(objRLD);

                        string _apiUrl = System.Configuration.ConfigurationManager.AppSettings["RLDApiUrl"];

                        log.Info("url :" + _apiUrl);
                        log.Error("Json String Before submit:" + json);
                        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("crisedi" + ":" + "crisedi123"));
                        
                        var _data = new StringContent(json, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add("Authorization", "Basic " + svcCredentials);                            

                            log.Error("Json String Before Post api url:" + _apiUrl);
                            HttpResponseMessage response = await client.PostAsync(_apiUrl, _data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            //var status = joResponse["Status"];

                            desc = joResponse["desc"].ToString();
                            msg = joResponse["response"].ToString();

                            log.Error("Desc:" + desc);
                            log.Error("Response:" + msg);

                            log.Info("Before Status Save" );
                            ObjER.LoadingDetailsUpdateStatus(RWHdrId, json, content);
                            log.Info("After Status Save");

                        }

                       
                        

                        return Json(new { Status = 1, Message = msg + " " + desc });
                    }
                    else
                    {
                        return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                    }
                }
                else
                {
                    return Json(new { Status = 0, Message = "Loading Details send fail." });
                }
                   
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = "Loading Details send fail." });
            }

        }

        #endregion


    }

    #region-- CUSTOM PDF EGENRATOR --

    class CustomPdfGenerator
    {
        private readonly iTextSharp.text.Document _document;
        private iTextSharp.text.Document Document { get { return new iTextSharp.text.Document(); } }

        private IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public CustomPdfGenerator()
        {
            _document = Document;
            _document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _document.SetMargins(10f, 10f, 10f, 10f);
        }

        public void Generate(string filePath, string shippingLine, IList<Kol_FormOnePrintDetailModel> data)
        {
            try
            {
                var pageHeight = 0f;
                var Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, new FileStream(filePath, FileMode.Create));

                /*var img = iTextSharp.text.Image.GetInstance(@"D:\RAHULZ\RKZ\pdfconsole\pdfconsole\bin\Debug\scan0002.jpg");
                img.ScaleToFit(_document.PageSize.Width, _document.PageSize.Height);
                img.SetAbsolutePosition(0, 0);
                Writer.PageEvent = new ImageBackgroundHelper(img);*/

                var font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.BaseColor.BLACK);

                _document.Open();

                var haz =  data.Any(o => o.HazType == "HAZ") ? "HAZ" : "NON-HAZ";
                              
                    var refer = data.Any(o => o.ReferType == "/ REEFER") ? " / REEFER" : "";
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(haz + refer, font), 590, 545, 0);

                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(shippingLine, iTextSharp.text.FontFactory.GetFont("Arial", 9, 1, iTextSharp.text.BaseColor.BLACK)), 640, 470, 0);

                var table = new PdfPTable(8);
                var width = new[] { 148f, 120f, 54f, 40f, 130f, 94f, 80f, 84f };
                table.SetWidthPercentage(width, iTextSharp.text.PageSize.A4.Rotate());

                IList<string> addeditems = new List<string>();
                var pcb = Writer.DirectContent;

                data.GroupBy(o => o.LineNo).ToList().ForEach(groupedLine => {
                    foreach (var item in groupedLine)
                    {
                        var val1 = addeditems.Any(o => o == item.VesselName) ? "" : item.VesselName;
                        var val2 = addeditems.Any(o => o == item.VoyageNo) ? "" : item.VoyageNo;

                        var cell1 = new PdfPCell(new Phrase(val1 + Environment.NewLine + val2, font));
                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1.BorderWidth = 0;
                        table.AddCell(cell1);

                        var val3 = addeditems.Any(o => o == item.ContainerNo) ? "" : item.ContainerNo;
                        var val4 = addeditems.Any(o => o == item.SealNo) ? "" : item.SealNo;
                        var cell2 = new PdfPCell(new Phrase(val3 + Environment.NewLine + val4, font));
                        cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell2.BorderWidth = 0;
                        table.AddCell(cell2);

                        var val5 = addeditems.Any(o => o == item.Type) ? "" : item.Type;
                        var cell3 = new PdfPCell(new Phrase(val5, font));
                        cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell3.BorderWidth = 0;
                        table.AddCell(cell3);

                        var val6 = addeditems.Any(o => o == item.LineNo) ? "" : item.LineNo;
                        var cell4 = new PdfPCell(new Phrase(val6, font));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.BorderWidth = 0;
                        table.AddCell(cell4);

                        var name_address = item.ImpName + Environment.NewLine + item.ImpAddress + Environment.NewLine + item.ImpName2 + Environment.NewLine + item.ImpAddress2;
                        var val7 = addeditems.Any(o => o == name_address) ? "" : name_address;
                        var cell5 = new PdfPCell(new Phrase(val7, font));
                        cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell5.BorderWidth = 0;
                        table.AddCell(cell5);

                        var val8 = addeditems.Any(o => o == item.CargoDesc) ? "" : item.CargoDesc;
                        var cell6 = new PdfPCell(new Phrase(val8, font));
                        cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell6.BorderWidth = 0;
                        table.AddCell(cell6);

                        var val9 = addeditems.Any(o => o == item.DateOfLanding) ? "" : item.DateOfLanding;
                        var cell7 = new PdfPCell(new Phrase(val9, font));
                        cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell7.BorderWidth = 0;
                        table.AddCell(cell7);

                        var val10 = "";
                        var cell8 = new PdfPCell(new Phrase(val10, font));
                        cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell8.BorderWidth = 0;
                        table.AddCell(cell8);

                        table.CompleteRow();
                        pageHeight += table.CalculateHeights();

                        if (pageHeight > 800)
                        {
                            table.WriteSelectedRows(0, -1, 45, 360, pcb);
                            _document.NewPage();
                            pageHeight = 0f;
                            table.Rows.Clear();
                        }

                        addeditems.Add(item.LineNo);
                        if (!addeditems.Any(o => o == item.VesselName))
                            addeditems.Add(item.VesselName);
                        if (!addeditems.Any(o => o == item.VoyageNo))
                            addeditems.Add(item.VoyageNo);
                        if (!addeditems.Any(o => o == item.ContainerNo))
                            addeditems.Add(item.ContainerNo);
                        if (!addeditems.Any(o => o == item.SealNo))
                            addeditems.Add(item.SealNo);
                        if (!addeditems.Any(o => o == item.Type))
                            addeditems.Add(item.Type);
                        if (!addeditems.Any(o => o == name_address))
                            addeditems.Add(name_address);
                        if (!addeditems.Any(o => o == item.CargoDesc))
                            addeditems.Add(item.CargoDesc);
                        if (!addeditems.Any(o => o == item.DateOfLanding))
                            addeditems.Add(item.DateOfLanding);
                    }
                    foreach (var item in groupedLine)
                    {
                        addeditems.Remove(item.ContainerNo);
                        addeditems.Remove(item.SealNo);
                    }
                });

                table.WriteSelectedRows(0, -1, 45, 360, pcb);

                if (_document.IsOpen())
                    _document.Close();
            }
            catch (Exception e)
            {
                if (_document.IsOpen())
                    _document.Close();
            }
        }
        public void GenerateKol(string filePath, string shippingLine,string BLNo, IList<Kol_FormOnePrintDetailModel> data)
        {
            try
            {
                var pageHeight = 0f;
                var Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, new FileStream(filePath, FileMode.Create));

                /*var img = iTextSharp.text.Image.GetInstance(@"D:\RAHULZ\RKZ\pdfconsole\pdfconsole\bin\Debug\scan0002.jpg");
                img.ScaleToFit(_document.PageSize.Width, _document.PageSize.Height);
                img.SetAbsolutePosition(0, 0);
                Writer.PageEvent = new ImageBackgroundHelper(img);*/

                var font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.BaseColor.BLACK);

                _document.Open();

                var haz = data.Any(o => o.HazType == "HAZ") ? "HAZ" : "NON-HAZ";
                haz = haz + " " +"BL Number :"+ BLNo;
                var refer = data.Any(o => o.ReferType == "/ REEFER") ? " / REEFER" : "";
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(haz + refer, font), 590, 545, 0);

                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(shippingLine, iTextSharp.text.FontFactory.GetFont("Arial", 9, 1, iTextSharp.text.BaseColor.BLACK)), 640, 470, 0);

                var table = new PdfPTable(8);
                var width = new[] { 148f, 120f, 54f, 40f, 130f, 94f, 80f, 84f };
                table.SetWidthPercentage(width, iTextSharp.text.PageSize.A4.Rotate());

                IList<string> addeditems = new List<string>();
                var pcb = Writer.DirectContent;

                data.GroupBy(o => o.LineNo).ToList().ForEach(groupedLine => {
                    foreach (var item in groupedLine)
                    {
                        var val1 = addeditems.Any(o => o == item.VesselName) ? "" : item.VesselName;
                        var val2 = addeditems.Any(o => o == item.VoyageNo) ? "" : item.VoyageNo;

                        var cell1 = new PdfPCell(new Phrase(val1 + Environment.NewLine + val2, font));
                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1.BorderWidth = 0;
                        table.AddCell(cell1);

                        var val3 = addeditems.Any(o => o == item.ContainerNo) ? "" : item.ContainerNo;
                        var val4 = addeditems.Any(o => o == item.SealNo) ? "" : item.SealNo;
                        var cell2 = new PdfPCell(new Phrase(val3 + Environment.NewLine + val4, font));
                        cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell2.BorderWidth = 0;
                        table.AddCell(cell2);

                        var val5 = addeditems.Any(o => o == item.Type) ? "" : item.Type;
                        var cell3 = new PdfPCell(new Phrase(val5, font));
                        cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell3.BorderWidth = 0;
                        table.AddCell(cell3);

                        var val6 = addeditems.Any(o => o == item.LineNo) ? "" : item.LineNo;
                        var cell4 = new PdfPCell(new Phrase(val6, font));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.BorderWidth = 0;
                        table.AddCell(cell4);

                        var name_address = item.ImpName + Environment.NewLine + item.ImpAddress + Environment.NewLine + item.ImpName2 + Environment.NewLine + item.ImpAddress2;
                        var val7 = addeditems.Any(o => o == name_address) ? "" : name_address;
                        var cell5 = new PdfPCell(new Phrase(val7, font));
                        cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell5.BorderWidth = 0;
                        table.AddCell(cell5);

                        var val8 = addeditems.Any(o => o == item.CargoDesc) ? "" : item.CargoDesc;
                        var cell6 = new PdfPCell(new Phrase(val8, font));
                        cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell6.BorderWidth = 0;
                        table.AddCell(cell6);

                        var val9 = addeditems.Any(o => o == item.DateOfLanding) ? "" : item.DateOfLanding;
                        var cell7 = new PdfPCell(new Phrase(val9, font));
                        cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell7.BorderWidth = 0;
                        table.AddCell(cell7);

                        var val10 = "";
                        var cell8 = new PdfPCell(new Phrase(val10, font));
                        cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell8.BorderWidth = 0;
                        table.AddCell(cell8);

                        table.CompleteRow();
                        pageHeight += table.CalculateHeights();

                        if (pageHeight > 800)
                        {
                            table.WriteSelectedRows(0, -1, 45, 360, pcb);
                            _document.NewPage();
                            pageHeight = 0f;
                            table.Rows.Clear();
                        }

                        addeditems.Add(item.LineNo);
                        if (!addeditems.Any(o => o == item.VesselName))
                            addeditems.Add(item.VesselName);
                        if (!addeditems.Any(o => o == item.VoyageNo))
                            addeditems.Add(item.VoyageNo);
                        if (!addeditems.Any(o => o == item.ContainerNo))
                            addeditems.Add(item.ContainerNo);
                        if (!addeditems.Any(o => o == item.SealNo))
                            addeditems.Add(item.SealNo);
                        if (!addeditems.Any(o => o == item.Type))
                            addeditems.Add(item.Type);
                        if (!addeditems.Any(o => o == name_address))
                            addeditems.Add(name_address);
                        if (!addeditems.Any(o => o == item.CargoDesc))
                            addeditems.Add(item.CargoDesc);
                        if (!addeditems.Any(o => o == item.DateOfLanding))
                            addeditems.Add(item.DateOfLanding);
                    }
                    foreach (var item in groupedLine)
                    {
                        addeditems.Remove(item.ContainerNo);
                        addeditems.Remove(item.SealNo);
                    }
                });

                table.WriteSelectedRows(0, -1, 45, 360, pcb);

                if (_document.IsOpen())
                    _document.Close();
            }
            catch (Exception e)
            {
                if (_document.IsOpen())
                    _document.Close();
            }
        }

        public void Generate1(string filePath, string shippingLine, IList<Kol_FormOnePrintDetailModel> data)
        {
            var Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, new FileStream(filePath, FileMode.Create));

            var img = iTextSharp.text.Image.GetInstance(@"D:\RAHULZ\RKZ\pdfconsole\pdfconsole\bin\Debug\scan0002.jpg");
            img.ScaleToFit(_document.PageSize.Width, _document.PageSize.Height);
            img.SetAbsolutePosition(0, 0);
            Writer.PageEvent = new ImageBackgroundHelper(img);

            var font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.BaseColor.BLACK);

            _document.Open();

            var haz = data.Any(o => o.HazType == "HAZ") ? "HAZ" : "NON-HAZ";
            var refer = data.Any(o => o.ReferType == "/ REEFER") ? " / REEFER" : "";
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(haz + refer, font), 590, 545, 0);
            
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(shippingLine, iTextSharp.text.FontFactory.GetFont("Arial", 9, 1, iTextSharp.text.BaseColor.BLACK)), 640, 470, 0);

            var VesselName = data.FirstOrDefault().VesselName;
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(VesselName, font), 50, 324, 0);
            var VoyageNo = data.FirstOrDefault().VoyageNo;
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(VoyageNo, font), 50, 312, 0);
            var RotationNo = data.FirstOrDefault().RotationNo;
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(RotationNo, font), 50, 300, 0);

            var Type = data.FirstOrDefault().Type;
            iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(Type, font), 325, 300, 0);
                        
            //var LineNo = data.FirstOrDefault().LineNo;
            //iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(LineNo, font), 380, 300, 0);

            List<string> splitedText = new List<string>();
            var yAxis = 350f;

            var ImpName = data.FirstOrDefault().ImpName;            
            if (ImpName.Length > 21)
            {
                ImpName = ImpName.PadRight((ImpName.Length - 1) + (21 - (ImpName.Length % 21)), ' ') + ".";
                splitedText = Split(ImpName, 21).ToList();
            }
            else
            {
                splitedText.Add(ImpName);
            }
            splitedText.ToList().ForEach(item =>
            {
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                yAxis -= 10f;
            });
            splitedText.Clear();

            yAxis -= 5f;
            var ImpAddress = data.FirstOrDefault().ImpAddress;
            if (ImpAddress.Length > 21)
            {
                ImpAddress = ImpAddress.PadRight((ImpAddress.Length - 1) + (21 - (ImpAddress.Length % 21)), ' ') + ".";
                splitedText = Split(ImpAddress, 21).ToList();
            }
            else
            {
                splitedText.Add(ImpAddress);
            }
            splitedText.ToList().ForEach(item =>
            {
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                yAxis -= 10f;
            });
            splitedText.Clear();

            //Second Name
            var ImpName2 = string.IsNullOrEmpty(data.FirstOrDefault().ImpName2) ? "" : data.FirstOrDefault().ImpName2;
            if (!string.IsNullOrEmpty(ImpName2) && ImpName != ImpName2)
            {
                yAxis -= 5f;
                if (ImpName2.Length > 21)
                {
                    ImpName2 = ImpName2.PadRight((ImpName2.Length - 1) + (21 - (ImpName2.Length % 21)), ' ') + ".";
                    splitedText = Split(ImpName2, 21).ToList();
                }
                else
                {
                    splitedText.Add(ImpName2);
                }
                splitedText.ToList().ForEach(item =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                    yAxis -= 10f;
                });
                splitedText.Clear();

                yAxis -= 5f;
                var ImpAddress2 = data.FirstOrDefault().ImpAddress2;
                if (ImpAddress2.Length > 21)
                {
                    ImpAddress2 = ImpAddress2.PadRight((ImpAddress2.Length - 1) + (21 - (ImpAddress2.Length % 21)), ' ') + ".";
                    splitedText = Split(ImpAddress2, 21).ToList();
                }
                else
                {
                    splitedText.Add(ImpAddress2);
                }
                splitedText.ToList().ForEach(item =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                    yAxis -= 10f;
                });
                splitedText.Clear();
            }

            yAxis = 350f;
            var CargoDesc = data.FirstOrDefault().CargoDesc;
            if (CargoDesc.Length > 16)
            {
                CargoDesc = CargoDesc.PadRight((CargoDesc.Length - 1) + (16 - (CargoDesc.Length % 16)), ' ') + ".";
                splitedText = Split(CargoDesc, 16).ToList();
            }
            else
            {
                splitedText.Add(CargoDesc);
            }
            splitedText.ToList().ForEach(item =>
            {
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 540, yAxis, 0);
                yAxis -= 10f;
            });

            //var DateOfLanding = data.FirstOrDefault().DateOfLanding;
            //iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(DateOfLanding, font), 635, 300, 0);

            yAxis = 350f;
            int counter = 0;
            IList<string> printedLines = new List<string>();
            data.GroupBy(o => o.LineNo).ToList().ForEach(groupedItem => {
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(groupedItem.Key, font), 380, yAxis, 0);
                groupedItem.ToList().ForEach(item => {                    
                    var containerNo = item.ContainerNo;
                    var sealNo = item.SealNo;
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(containerNo, font), 200, yAxis, 0);
                    yAxis -= 10f;
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(sealNo, font), 200, yAxis, 0);
                    yAxis -= 15f;
                    counter += 1;
                    if (counter == 10)
                    {
                        yAxis = 350f;
                        _document.NewPage();
                        counter = 0;
                    }
                });
            });

            if (_document.IsOpen())
                _document.Close();
        }
    }

    #endregion

   





}