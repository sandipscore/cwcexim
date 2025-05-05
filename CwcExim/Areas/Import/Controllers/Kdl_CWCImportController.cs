using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using CwcExim.Filters;
using System.IO;
using System.Web;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using EinvoiceLibrary;
using System.Threading.Tasks;

namespace CwcExim.Areas.Import.Controllers
{
    public class Kdl_CWCImportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Job Order
        [HttpGet]
        public ActionResult CreateEmptyContainerJobOrder()
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            ImportJobOrder objJO = new ImportJobOrder();
            objJO.JobOrderDate = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objJO);
       
        }
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetAllBlNofromFormOne();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfBlNo = objIR.DBResponse.Data;
            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            ImportJobOrder objJO = new ImportJobOrder();
            objJO.JobOrderDate = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            IList<ImportJobOrderList> lstIJO = new List<ImportJobOrderList>();
            objIR.GetAllImpJO();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }
        [HttpGet]
        public ActionResult LstOfEmtcontJobOdrDtls()
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            IList<ImportJobOrderList> lstIJO = new List<ImportJobOrderList>();
            objIR.GetAllImpEmtCntJO();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }
        [HttpGet]
        public ActionResult EditEmtcntJobOrder(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetImpEmptcntJODetails(ImpJobOrderId);
            ImportJobOrder objImp = new ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (ImportJobOrder)objIR.DBResponse.Data;
            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            YardRepository ObjYR = new YardRepository();
            ObjYR.GetAllYard();
            if (ObjYR.DBResponse.Data != null)
                ViewBag.ListOfYard = (List<CwcExim.Models.Yard>)ObjYR.DBResponse.Data;
            objIR.GetYardWiseLocation(objImp.ToYardId);
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfYardWiseLctn = (List<Areas.Import.Models.YardWiseLocation>)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            ImportJobOrder objImp = new ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (ImportJobOrder)objIR.DBResponse.Data;
            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            YardRepository ObjYR = new YardRepository();
            ObjYR.GetAllYard();
            if (ObjYR.DBResponse.Data != null)
                ViewBag.ListOfYard = (List<CwcExim.Models.Yard>)ObjYR.DBResponse.Data;
            objIR.GetYardWiseLocation(objImp.ToYardId);
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfYardWiseLctn = (List<Areas.Import.Models.YardWiseLocation>)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            ImportJobOrder objImp = new ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (ImportJobOrder)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objImp);
        }

        //Add on 24/01/2018
        [HttpGet]
        public ActionResult ViewEmtcntJobOrder(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetImpEmptcntJODetails(ImpJobOrderId);
            ImportJobOrder objImp = new ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (ImportJobOrder)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objImp);
        }

        //Add on 24/01/2018

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        public JsonResult DelemptCntJobOdr(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.DelImpemptcntJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEmptyContainerJobOrder(ImportJobOrder objImp)
        {
            
                List<ImportJobOrderDtl> lstDtl = new List<ImportJobOrderDtl>();
                List<int> lstLctn = new List<int>();
                string XML = "";
                Kdl_ImportRepository objIR = new Kdl_ImportRepository();
                if (objImp.StringifyXML != null)
                {
                    lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImportJobOrderDtl>>(objImp.StringifyXML);
                    if (lstDtl.Count > 0)
                        XML = Utility.CreateXML(lstDtl);
                }
                
                objIR.AddEditImpContJO(objImp, XML);
                return Json(objIR.DBResponse);
            }
            

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(ImportJobOrder objImp)
        {
            if (ModelState.IsValid)
            {
                List<ImportJobOrderDtl> lstDtl = new List<ImportJobOrderDtl>();
                List<int> lstLctn = new List<int>();
                string XML = "", lctnXML = "";
                Kdl_ImportRepository objIR = new Kdl_ImportRepository();
                if (objImp.StringifyXML != null)
                {
                    lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImportJobOrderDtl>>(objImp.StringifyXML);
                    if (lstDtl.Count > 0)
                        XML = Utility.CreateXML(lstDtl);
                }
                if (objImp.YardWiseLocationIds != null)
                {
                    string[] lctns = objImp.YardWiseLocationIds.Split(',');
                    foreach (string data in lctns)
                        lstLctn.Add(Convert.ToInt32(data));
                    lctnXML = Utility.CreateXML(lstLctn);
                }
                objIR.AddEditImpJO(objImp, XML, lctnXML);
                return Json(objIR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }

        }
        [HttpGet]
        public JsonResult GetBlNoDetails(int FormOneId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetBlNoDtl(FormOneId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetYardWiseLocation(YardId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Job Order Print
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJO(int ImpJobOrderId)
        {
            Kdl_ImportRepository objIR = new Kdl_ImportRepository();
            objIR.GetImportJODetailsFrPrint(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                PrintJOModel objMdl = new PrintJOModel();
                objMdl = (PrintJOModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, ImpJobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForJO(PrintJOModel objMdl, int ImpJobOrderId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
            string ContainerNo = "", Size = "", Serial = ""; int Count = 0;
            string Html = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            objMdl.lstDet.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                Size = (Size == "" ? (item.ContainerSize) : (Size + "<br/>" + item.ContainerSize));
                Serial = (Serial == "") ? (++Count).ToString() : (Serial + "<br/>" + (++Count).ToString());
            });
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            if ((Convert.ToInt32(Session["BranchId"])) == 1)
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> " + objMdl.ToLocation + " </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            }
            // string Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>CONTAINER FREIGHT STATION<br/>18, COAL DOCK ROAD, KOLKATA - 700 043</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderNo+"</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderDate+"</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> "+objMdl.FromLocation+" </span> to<span style='border-bottom:1px solid #000;'> "+objMdl.ToLocation+" </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>"+ Serial + "</td><td style='border:1px solid #000;padding:5px;'>"+ContainerNo+"</td><td style='border:1px solid #000;padding:5px;'>"+Size+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ShippingLineName+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ContainerType+"</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
        }
        #endregion

        #endregion


        #region Yard Payment Sheet

        [HttpGet]
        public ActionResult CreateContPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
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

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST,
            int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer,decimal WeighmentCharges=0,
            int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, 0,InvoiceType, XMLText, WeighmentCharges, InvoiceId);
            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            /**********************BOL PRINT****************/
            var BOL = "";
            objChrgRepo.GetBOLForEmptyCont("Yard", Output.RequestId);
            if (objChrgRepo.DBResponse.Status == 1)
                BOL = objChrgRepo.DBResponse.Data.ToString();
            /***********************************************/
            return Json(new { Output, BOL });
        }

        [NonAction]
        private void CalculateCWCChargesContainer(PaymentSheetFinalModel finalModel, PaySheetChargeDetails baseModel)
        {
            try
            {
                //var A = JsonConvert.DeserializeObject<PaySheetChargeDetails>("{   \"lstPSContainer\": [     {       \"CFSCode\": \"CFSCode8\",       \"ContainerNo\": \"CONT1234\",       \"Size\": \"20\",       \"IsReefer\": false,       \"Insured\": \"No\"     },     {       \"CFSCode\": \"CFSCode9\",       \"ContainerNo\": \"CONT0001\",       \"Size\": \"40\",       \"IsReefer\": false,       \"Insured\": \"Yes\"     }   ],   \"lstEmptyGR\": [     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"lstLoadedGR\": [     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 3,       \"RentAmount\": 0.10,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 4,       \"DaysRangeTo\": 15,       \"RentAmount\": 380.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 16,       \"DaysRangeTo\": 999,       \"RentAmount\": 500.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"InsuranceRate\": 12.15,   \"lstStorageRent\": [     {       \"CFSCode\": \"CFSCode9\",       \"ActualCUM\": 1500.00,       \"ActualSQM\": 1800.00,       \"ActualWeight\": 10000.00,       \"StuffCUM\": 225.000,       \"StuffSQM\": 270.000,       \"StuffWeight\": 1500.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     },     {       \"CFSCode\": \"CFSCode8\",       \"ActualCUM\": 5000.00,       \"ActualSQM\": 2500.00,       \"ActualWeight\": 5000.00,       \"StuffCUM\": 2000.000,       \"StuffSQM\": 1000.000,       \"StuffWeight\": 2000.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     }   ],   \"RateSQMPerWeek\": 456.00,   \"RateSQMPerMonth\": 56.00,   \"RateCUMPerWeek\": 4566.00,   \"RateMTPerDay\": 56.00 }");
                var EGRAmt = 0m;
                //baseModel.lstEmptyGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                //{
                //    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                //    {
                //        var grp = x.GroundRentPeriod;
                //        var drf = x.DaysRangeFrom;
                //        var drt = x.DaysRangeTo;
                //        if (grp >= drt)
                //        {
                //            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                //        }
                //        else
                //        {
                //            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                //            break;
                //        }
                //    }
                //});
                baseModel.lstLoadedGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                {
                    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                    {
                        var grp = x.GroundRentPeriod;
                        var drf = x.DaysRangeFrom;
                        var drt = x.DaysRangeTo;
                        if (grp >= drt)
                        {
                            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                        }
                        else
                        {
                            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                            break;
                        }
                    }
                });
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Amount = EGRAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Total = EGRAmt;

                var STAmt = 0m;
                //baseModel.lstStorageRent.ToList().ForEach(item =>
                //{
                //    var Amt1 = item.StuffCUM * item.StorageWeeks * baseModel.RateCUMPerWeek;
                //    var Amt2 = item.StuffWeight * item.StorageDays * baseModel.RateMTPerDay;
                //    var Amt3 = item.StorageDays < 30 ? (item.StuffSQM * item.StorageWeeks * baseModel.RateSQMPerWeek)
                //                                    : ((item.StuffSQM * item.StorageMonths * baseModel.RateSQMPerMonth) + (item.StuffSQM * item.StorageMonthWeeks * baseModel.RateSQMPerWeek));
                //    STAmt += Enumerable.Max(new[] { Amt1, Amt2, Amt3 });
                //});
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Storage Charge").Amount = STAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Storage Charge").Total = STAmt;

                var INSAmt = 0m;
                baseModel.lstInsuranceCharges.Where(o => o.IsInsured == 0).ToList().ForEach(item =>
                {
                    INSAmt += item.FOB * baseModel.InsuranceRate * item.StorageWeeks;
                });
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Insurance").Amount = INSAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Insurance").Total = INSAmt;

            }
            catch (Exception e)
            {

            }

            //return finalModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(FormCollection objForm)
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
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPYard", ExportUnder,"");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Destuffing Payment Sheet
        [HttpGet]
        public ActionResult CreateDestuffingPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeStuffingRequestForImpPaymentSheet();
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

        [HttpGet]
        public JsonResult GetPaymentSheetDestuffingCont(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDestuffingContForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDestuffingPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            List<PaymentSheetContainer> lstPaySheetContainer, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDestuffingPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText,InvoiceId);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            /**************BOL PRINT**********************/
            objChrgRepo.GetBOL(Output.RequestId);
            var BOL = "";
            if (objChrgRepo.DBResponse.Status == 1)
            {
                BOL = objChrgRepo.DBResponse.Data.ToString();
            }
            /********************************************/
            return Json(new { Output, BOL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingPaymentSheet(FormCollection objForm)
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

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.StuffingDate;
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.DestuffingDate;
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.CartingDate;
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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDest");
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
        public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, 
            List<PaymentSheetBOE> lstPaySheetBOE,decimal WeighmentCharges=0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheet_Kandla(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, WeighmentCharges, InvoiceId);
            var Output= new PostPaymentSheet();
            var BOL = "";
            if (objChrgRepo.DBResponse.Data != null)
            {
                 Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
                Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
                Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
                Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
                Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
                Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
                Output.CWCTDS = 0;
                Output.HTTDS = 0;
                Output.TDS = 0;
                Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
                Output.RoundUp = 0;
                Output.InvoiceAmt = Output.AllTotal;
           
            
            }
            /**********BOL PRINT**************/

            objChrgRepo.GetBOLForDeliverApp(Output.RequestId);
            if (objChrgRepo.DBResponse.Status == 1)
                BOL = objChrgRepo.DBResponse.Data.ToString();
            /********************************/
            return Json(new { Output, BOL });
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

                string ExportUnder = Convert.ToString(objForm["SEZValue"]);


                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "", CargoXML="";

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }
                foreach (var item in invoiceData.lstPreInvoiceCargo)
                {
                   
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? "1900-01-01" : item.StuffingDate;
                   
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate) ? "1900-01-01" : item.CartingDate;//item.CartingDate.ToString("yyyy-MM-dd"); 
                                                                                                         //string.IsNullOrEmpty(item.CartingDate) ? "0" : item.CartingDate;
                    //item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
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
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", ExportUnder, CargoXML);
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
        #endregion

        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "YARD:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            ImportRepository objImport = new ImportRepository();
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
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

        [HttpGet]
        public JsonResult GetPaymentSheetEmptyCont(string InvoiceFor, int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, 
            string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor,decimal WeighmentCharges=0, int InvoiceId=0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceFor, WeighmentCharges, InvoiceId);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            /***************BOL PRINT*************/
            objChrgRepo.GetBOLForEmptyCont(InvoiceFor, Output.RequestId);
            var BOL = "";
            if (objChrgRepo.DBResponse.Status == 1)
                BOL = objChrgRepo.DBResponse.Data.ToString();
            /************************************/
            return Json(new { Output, BOL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECDeliveryPaymentSheet(FormCollection objForm)
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
                var invoiceFor = objForm["InvoiceFor"].ToString();
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor, ExportUnder,"");
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
        public ActionResult ListOfInvoice(string Module, string InvoiceNo = null,  string BOENo = null,string InvoiceDate = null, int Page = 0)
        {
            Kdl_ImportRepository objER = new Kdl_ImportRepository();
            objER.ListOfInvoice(Module, InvoiceNo, BOENo, InvoiceDate);
            List<Kdl_ListOfImpInvoice> obj = new List<Kdl_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Kdl_ListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfInvoice", obj);
        }
        [HttpGet]
        public ActionResult ListLoadMoreInvoice(string Module, string InvoiceNo = null,string BOENo=null, string InvoiceDate = null, int Page = 0)
        {
            Kdl_ImportRepository objER = new Kdl_ImportRepository();
            objER.ListLoadMoreInvoice(Module, InvoiceNo, BOENo, InvoiceDate, Page);
            List<Kdl_ListOfImpInvoice> obj = new List<Kdl_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Kdl_ListOfImpInvoice>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tentative Invoice
        [HttpGet]
        public ActionResult TentativeImpInvoice()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateContPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
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

        [HttpGet]
        public ActionResult CreateDestuffingPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeStuffingRequestForImpPaymentSheet();
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

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheetTab(string type = "YARD:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            ImportRepository objImport = new ImportRepository();
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
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
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
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
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
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
                Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
                string DeliveryXml = "";
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
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

        #region Destuffing Entry

        [HttpGet]
        public ActionResult CreateDestuffingEntry()
        {
            ImportRepository ObjIR = new ImportRepository();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            ObjDestuffing.DODate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetContrNoForDestuffingEntry(Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<DestuffingEntry>)ObjIR.DBResponse.Data, "DestuffingDtlId", "ContainerNo");
            }
            else
            {
                ViewBag.ContainerList = null;
            }

            //ObjIR.ListOfCHA();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjIR.ListOfShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            return PartialView("CreateDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(int DestuffingDtlId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetContrDetForDestuffingEntry_Kdl(DestuffingDtlId, Convert.ToInt32(Session["BranchId"]));
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<DestuffingEntryList> LstDestuffing = new List<DestuffingEntryList>();
            ObjIR.GetAllDestuffingEntry(Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<DestuffingEntryList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetKdl_DestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingEntry(int DestuffingEntryId)
        {
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetKdl_DestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntry", ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingEntry(DestuffingEntry ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                // List<int> LstGodown = new List<int>();
                // string GodownXML = "";
                // string ClearLcoationXML = null;
                List<DestuffingEntry> LstDestuffingEntry = new List<DestuffingEntry>();

                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DestuffingEntry>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                    //if (LstDestuffingEntry.Count > 0)
                    //{
                    //    foreach (var item in LstDestuffingEntry)
                    //    {
                    //        string[] GodownList = item.LocationDetails.Split(',');
                    //        foreach (string Data in GodownList)
                    //            LstGodown.Add(Convert.ToInt32(Data));

                    //    }
                    //    GodownXML = Utility.CreateXML(LstGodown);
                    //}
                }
                ////if (ObjDestuffing.GodownWiseLocationIds != null && ObjDestuffing.GodownWiseLocationIds != "")
                ////{
                ////    string[] GodownList = ObjDestuffing.GodownWiseLocationIds.Split(',');
                ////    foreach (string Data in GodownList)
                ////        LstGodown.Add(Convert.ToInt32(Data));
                ////    GodownXML = Utility.CreateXML(LstGodown);
                ////}

                //if (ObjDestuffing.ClearLocation != "" && ObjDestuffing.ClearLocation != null)
                //{
                //    string[] data = ObjDestuffing.ClearLocation.Split(',');
                //    List<int> LstClearLocation = new List<int>();
                //    foreach (var elem in data)
                //        LstClearLocation.Add(Convert.ToInt32(elem));
                //    ClearLcoationXML = Utility.CreateXML(LstClearLocation);
                //}

                ImportRepository ObjIR = new ImportRepository();
                ObjDestuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditDestuffingEntry_Kdl(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]));
               // ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDestuffingSheet(int DestuffingEntryId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetDestuffEntryForPrint_Kdl(DestuffingEntryId);
            if (ObjIR.DBResponse.Data != null)
            {
                DestuffingEntry ObjDestuff = new DestuffingEntry();
                ObjDestuff = (DestuffingEntry)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForDestuffSheet(ObjDestuff, DestuffingEntryId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [NonAction]
        public string GeneratePDFForDestuffSheet(DestuffingEntry ObjDestuff, int DestuffingEntryId)
        {
            // DestuffingEntry ObjDestuff = new DestuffingEntry();
            // ObjDestuff=ObjDestuffing.LstDestuffingEntry[0];
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
            string Html = "";
            //string ContainerNo = "", Size = "", Serial = "", BOEDate = "", BOLDate = "", BOENo = "", Vessel = "", CHA = "", Importer = "", ShippingLine = "",
            //CargoDescription = "", MarksNo = "", Weight = "", LineNo = "", Rotation = "", ArrivalDate = "", DestuffingDate = "", Location = "";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            //if (System.IO.File.Exists(Path))
            //{
            //    System.IO.File.Delete(Path);
            //}
            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;'>CONTAINER FREIGHT STATION<br/>DESTUFFING SHEET FCL/LCL</th></tr></thead><tbody><tr><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing no.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Container no.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>CFS Code</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Vessel</td><td style='text-align:left;padding:5px;'>MODEL</td><td style='text-align:left;padding:5px;'>Voyage</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>S/L Seal No.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing Date</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Container Size</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Shipping Line</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Rotation</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Custom Seal No.</td><td style='text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th style='border:1px solid #000;text-align:center;padding:5px;'>Line No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Marks & No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CHA</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Importer</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Desc.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Commodity</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Type</th><th style='border:1px solid #000;text-align:center;padding:5px;'>No of pack</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Bay No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Grid Occup</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Gross Wt.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CIF Value</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Duty</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:right;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:right;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td></tr><tr><td colspan='2' style='padding-top:100px;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border-top:1px dotted #000;'><tbody><tr><td style='text-align:left;width:25%;'>Variations observed if any<br/>Signature and Designation<br/>with date and seal of</td><td style='text-align:left;width:25%;'>Shed Asstt CWC,CFS</td><td style='text-align:left;width:25%;'>Shed I/C CWC,CFS</td><td style='text-align:left;width:25%;'>Rap./Surveyor of Handling & Transport Contractor</td></tr></tbody></table></td></tr></tbody></table>";
            //Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;'>CONTAINER FREIGHT STATION<br/>DESTUFFING SHEET FCL/LCL</th></tr></thead><tbody><tr><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing no.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Container no.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>CFS Code</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Vessel</td><td style='text-align:left;padding:5px;'>MODEL</td><td style='text-align:left;padding:5px;'>Voyage</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>S/L Seal No.</td><td colspan='3' style='text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;'>De-Stuffing Date</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Container Size</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Shipping Line</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Rotation</td><td style='text-align:left;padding:5px;'>MODEL</td></tr><tr><td style='text-align:left;padding:5px;'>Custom Seal No.</td><td style='text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><th style='border:1px solid #000;text-align:center;padding:5px;'>Line No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Marks & No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CHA</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Importer</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Desc.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Commodity</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Type</th><th style='border:1px solid #000;text-align:center;padding:5px;'>No of pack</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Bay No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Grid Occup</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Gross Wt.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CIF Value</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Duty</th></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:right;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:right;padding:5px;'>MODEL</td><td style='border:1px solid #000;text-align:left;padding:5px;'>MODEL</td></tr></tbody></table></td></tr><tr><td colspan='2' style='padding-top:100px;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border-top:1px dotted #000;'><tbody><tr><td style='text-align:left;width:25%;'>Variations observed if any<br/>Signature and Designation<br/>with date and seal of</td><td style='text-align:left;width:25%;'>Shed Asstt CWC,CFS</td><td style='text-align:left;width:25%;'>Shed I/C CWC,CFS</td><td style='text-align:left;width:25%;'>Rap./Surveyor of Handling & Transport Contractor</td></tr></tbody></table></td></tr></tbody></table>";
            Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;'>CENTRAL WAREHOUSING CORPORATION<br/>(A Govt. of India Undertaking)<br/>CONTAINER FREIGHT STATION<br/>DESTUFFING SHEET FCL/LCL</th></tr></thead><tbody><tr><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing no.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.DestuffingEntryNo + "</td></tr><tr><td style='text-align:left;padding:5px;'>Container no.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.ContainerNo + "</td></tr><tr><td style='text-align:left;padding:5px;'>CFS Code</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.CFSCode + "</td></tr><tr><td style='text-align:left;padding:5px;'>Vessel</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Vessel + "</td><td style='text-align:left;padding:5px;'>Voyage</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Voyage + "</td></tr><tr><td style='text-align:left;padding:5px;'>S/L Seal No.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.SealNo + "</td></tr></tbody></table></td><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing Date</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.DestuffingEntryDate + "</td></tr><tr><td style='text-align:left;padding:5px;'>Container Size</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Size + "</td></tr><tr><td style='text-align:left;padding:5px;'>Shipping Line</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.ShippingLine + "</td></tr><tr><td style='text-align:left;padding:5px;'>Rotation</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Rotation + "</td></tr><tr><td style='text-align:left;padding:5px;'>Custom Seal No.</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.CustomSealNo + "</td></tr></tbody></table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th style='border:1px solid #000;text-align:center;padding:5px;'>Line No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Marks & No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CHA</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Importer</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Desc.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Commodity</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Type</th><th style='border:1px solid #000;text-align:center;padding:5px;'>No of pack</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Bay No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Grid Occup</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Gross Wt.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Destuffing Wt.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CIF Value</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Duty</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Area</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Volume</th></tr></thead><tbody>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            string html2 = "";
            ObjDestuff.LstDestuffingEntry.ToList().ForEach(item =>
            {
                html2 += "<tr><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.LineNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOENo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOEDate + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOLNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOLDate + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.MarksNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CHA + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Importer + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CargoDescription + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Commodity + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Cargo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.NoOfPackages + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.GodownName + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.GodownWiseLctnNames + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + item.GrossWeight + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + item.DestuffingWeight + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + item.CIFValue + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Duty + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.SQM + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CUM + "</td></tr>";
            });
            string html3 = "</tbody></table></td></tr><tr><td colspan='2' style='padding-top:100px;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border-top:1px dotted #000;'><tbody><tr><td style='text-align:left;width:25%;'>Variations observed if any<br/>Signature and Designation<br/>with date and seal of<br/>Rep. / Surveyor / S.Agent <br/> Live / CHA</td><td style='text-align:left;width:25%;'>Shed Asstt CWC,CFS</td><td style='text-align:left;width:25%;'>Shed I/C CWC,CFS</td><td style='text-align:left;width:25%;'>Rap./Surveyor of Handling & Transport Contractor</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html + html2 + html3;
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
            {
                RH.GeneratePDF(Path, Html);
            }

            return "/Docs/" + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
        }


        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetSearchDestuffingEntryList(string ContainerName)
        {
            ImportRepository ObjIR = new ImportRepository();
            List<DestuffingEntryList> LstDestuffing = new List<DestuffingEntryList>();
            if (string.IsNullOrEmpty(ContainerName))
            {
                ObjIR.GetAllDestuffingEntry(Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    LstDestuffing = (List<DestuffingEntryList>)ObjIR.DBResponse.Data;
                }
            }
            else
            {
                ObjIR.GetSearchDestuffingEntry(Convert.ToInt32(Session["BranchId"]), ContainerName);
                if (ObjIR.DBResponse.Data != null)
                {
                    LstDestuffing = (List<DestuffingEntryList>)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }
        #endregion
        #region IAL
        #region IGM Upload

        [HttpGet]
        public ActionResult Kdl_ImportIGM()
        {
            Kdl_ImportIGMModel objImportIGM = new Kdl_ImportIGMModel();
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objImportIGM.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;

            return PartialView(objImportIGM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Kdl_UploadIGM()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    foreach (string fileName in Request.Files)
                    {
                        string fileExtension = fileName.Split('.')[1];
                        var rndfilename = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + "."+fileExtension;
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
        public JsonResult Kdl_ImportIGM(Kdl_ImportIGMModel m)
        {
            /*string cookieToken, formToken;
            string oldCookieToken = Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName] == null ? null : Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName].Value;
            System.Web.Helpers.AntiForgery.GetTokens(oldCookieToken, out cookieToken, out formToken);*/
            string Kdl_FormOneModelListXML = string.Empty;
            List<Kdl_IALModel> Kdl_IALModelList = new List<Kdl_IALModel>();
            try
            {
                if (ModelState.IsValid)
                {
                    #region Datatable                  

                    string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                    string FolderPath = Server.MapPath("~/Uploads/IGM/" + UID);

                    string Filepath = FolderPath + "\\" + Session["UploadedIGM"].ToString();
                    #endregion

                    #region Ial Upload
                    int data = 0;
                    int a = 0;
                    if (Filepath != "" || Filepath !=null)
                    {
                        //HttpPostedFileBase File = Request.Files[0];
                        string extension = Path.GetExtension(Filepath);
                        if (extension == ".xls" || extension == ".xlsx")
                        {
                            DataTable dt = Utility.GetExcelData(Filepath);
                            int cargoType = 1;
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    try
                                    {
                                       
                                        foreach (DataRow dr in dt.Rows)
                                        {

                                            Kdl_IALModel objKdl_FormOneModel = new Kdl_IALModel();
                                            objKdl_FormOneModel.ContainerNo = Convert.ToString(dr["Container No"]);
                                            objKdl_FormOneModel.GrossWeight = Convert.ToString(dr["Grr Wgt"]);

                                            objKdl_FormOneModel.ISO = Convert.ToString(dr["ISO"]);
                                            string categoryStr = Convert.ToString(dr["Category"]);
                                            if (categoryStr == "GEN")
                                            {
                                                cargoType = 2;
                                            }
                                            objKdl_FormOneModel.CargoType = cargoType;
                                            objKdl_FormOneModel.Pol = Convert.ToString(dr["POL"]);
                                            objKdl_FormOneModel.CfsCode = Convert.ToString(dr["CFS CODE"]);
                                            objKdl_FormOneModel.SealCode = Convert.ToString(dr["Seal 1"]);
                                           

                                            if (objKdl_FormOneModel.ContainerNo != "" && objKdl_FormOneModel.ContainerNo != null)
                                            {
                                                Kdl_IALModelList.Add(objKdl_FormOneModel);

                                                //objKdl_FormOneModel.ContainerNo = "";
                                                //objKdl_FormOneModel.GrossWeight = "";
                                                //objKdl_FormOneModel.ISO = "";
                                                //objKdl_FormOneModel.CargoType = 0;
                                                //objKdl_FormOneModel.Pol = "";
                                                //objKdl_FormOneModel.CfsCode = "";
                                                //objKdl_FormOneModel.SealCode = "";
                                                
                                            }
                                            //else
                                            //{
                                            //    objKdl_FormOneModel.ContainerNo = "";
                                            //    objKdl_FormOneModel.GrossWeight = "";
                                            //    objKdl_FormOneModel.ISO = "";
                                            //    objKdl_FormOneModel.CargoType = 0;
                                            //    objKdl_FormOneModel.Pol = "";
                                            //    objKdl_FormOneModel.CfsCode = "";
                                            //    objKdl_FormOneModel.SealCode = "";
                                                

                                            //}
                                            
                                        }

                                         Kdl_FormOneModelListXML = Utility.CreateXML(Kdl_IALModelList);

                                     
                                    }
                                    catch (Exception ex)
                                    {
                                        data = -2;
                                    }
                                }
                                else
                                {
                                    data = -1;
                                }
                            }
                            else
                            {
                                data = 0;
                            }
                        }
                        else
                        {
                            data = -3;
                        }

                    }
                    #endregion

                    string fileName = (string)Session["UploadedIGM"];
                    if (Kdl_FormOneModelListXML !="")
                    {
                      

                        int BranchId = Convert.ToInt32(Session["BranchId"]);
                        Kdl_ImportRepository objIR = new Kdl_ImportRepository();
                        objIR.SaveImportIALFile(m.FileName, ((Login)(Session["LoginUser"])).Uid, m.VesselNo, m.VoyageNo, m.ShippingLineId, m.ShippingLineName, m.RotationNo, BranchId, Kdl_FormOneModelListXML);
                        if (objIR.DBResponse.Status == 1)
                        {
                            if (System.IO.Directory.Exists(FolderPath))
                            {
                                try
                                {
                                    System.IO.Directory.Delete(FolderPath, true);
                                    Session.Remove("UploadedIGM");
                                    Kdl_IALModelList.Clear();
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
                return Json(new { Status = -1, Message = "Server Error" }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion

        #region Form One

        [HttpGet]
        public ActionResult Kdl_CreateFormOne()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddFormOne()
        {
            Kdl_FormOneModel objFormOne = new Kdl_FormOneModel();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");

            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
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
        public ActionResult Kdl_GetFormOneList(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<Kdl_FormOneModel> lstFormOne = new List<Kdl_FormOneModel>();
                Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
                objImportRepo.GetFormOne();
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Kdl_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<Kdl_FormOneModel> lstFormOne = new List<Kdl_FormOneModel>();
                Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
                objImportRepo.GetFormOneByContainer(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Kdl_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOne(Kdl_FormOneModel objFormOne)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    // objFormOne.FormOneDetailsJS.Replace("\"DateOfLanding: \":\"\"", "\"DateOfLanding\":\"null\"");
                    objFormOne.lstFormOneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Kdl_FormOneDetailModel>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetail.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetail);
                    Kdl_ImportRepository objImport = new Kdl_ImportRepository();
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
            Kdl_FormOneModel objFormOne = new Kdl_FormOneModel();
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Kdl_FormOneModel)objImport.DBResponse.Data;

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
            Kdl_FormOneModel objFormOne = new Kdl_FormOneModel();
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Kdl_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);
            return PartialView(objFormOne);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOne(int FormOneId)
        {
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOne(FormOneId);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public JsonResult PrintFormOne(int FormOneId)
        {
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            if (FormOneId > 0)
                objImport.FormOnePrint(FormOneId);
            var model = (Kdl_FormOnePrintModel)objImport.DBResponse.Data;
            var printableData1 = (IList<Kdl_FormOnePrintDetailModel>)model.lstFormOnePrintDetail;

            var fileName = model.FormOneNo + ".pdf";
            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
            string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);

            var cPdf = new CustomPdfGeneratorKdl();
            cPdf.Generate(PdfDirectory + "/" + fileName, model.ShippingLineNo, printableData1);

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

        #region-- CUSTOM PDF EGENRATOR --

        class CustomPdfGeneratorKdl
        {
            private readonly iTextSharp.text.Document _document;
            private iTextSharp.text.Document Document { get { return new iTextSharp.text.Document(); } }

            private IEnumerable<string> Split(string str, int chunkSize)
            {
                return Enumerable.Range(0, str.Length / chunkSize)
                    .Select(i => str.Substring(i * chunkSize, chunkSize));
            }

            public CustomPdfGeneratorKdl()
            {
                _document = Document;
                _document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                _document.SetMargins(10f, 10f, 10f, 10f);
            }

            public void Generate(string filePath, string shippingLine, IList<Kdl_FormOnePrintDetailModel> data)
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

            public void Generate1(string filePath, string shippingLine, IList<Kdl_FormOnePrintDetailModel> data)
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

        #endregion

        #region Empty Container Out  Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContOutPaymentSheet(string type = "YARD:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            ImportRepository objImport = new ImportRepository();
         //   objImport.GetApplicationForEmptyOutContainer(Convert.ToString(ViewData["ForType"]));
          //  if (objImport.DBResponse.Status > 0)
           //     ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
           // else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentPartyforEmptyOutContainer();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDateEmptyOutCont(int ShippingLineId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetEmptyContOutForDate(ShippingLineId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetContainerForEmptyOut(int ShippingLIneId,string in_date)
        {
            List<PaymentSheetContainer> lstContNo = new List<PaymentSheetContainer>();
            ImportRepository objImport = new ImportRepository();
            objImport.GetContainerForEmptyOut(ShippingLIneId, in_date);
            if (objImport.DBResponse.Data != null)
                lstContNo = (List<PaymentSheetContainer>)objImport.DBResponse.Data;
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmptyContainerOutPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int DeliveryType,int SPartyId,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName,int SecondInvoiceFlag,int  PartyIdSec,String PartyNameSec,String PartyGSTSec,String PartyAddressSec,String PartyStateSec,String PartyStateCodeSec,int PayeeIdSec,String PayeeNameSec, List<PaymentSheetContainer> lstPaySheetContainer,decimal WeighmentCharges=0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetEmptyContOutPaymentSheet(InvoiceDate, DeliveryType,SEZ,SPartyId, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, SecondInvoiceFlag, PartyIdSec, PartyNameSec, PartyGSTSec, PartyAddressSec, PartyStateSec, PartyStateCodeSec, PayeeIdSec, PayeeNameSec,InvoiceType, XMLText, WeighmentCharges, InvoiceId);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            /***************BOL PRINT*************/
            objChrgRepo.GetBOLForEmptyContOut(Output.RequestNo,Output.RequestId);
            var BOL = "";
            if (objChrgRepo.DBResponse.Status == 1)
                BOL = objChrgRepo.DBResponse.Data.ToString();
            /************************************/
            return Json(new { Output, BOL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECOutDeliveryPaymentSheet(FormCollection objForm)
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
                var invoiceFor = objForm["InvoiceFor"].ToString();
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                objChargeMaster.AddEditContOutInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "ECOut", ExportUnder,"");
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





        #endregion

        #region Empty Container Movement



        [HttpGet]
        public ActionResult CreateEmptyContMovementEntry()
        {


            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            Kdl_EmptyMovement objmodel = new Kdl_EmptyMovement();

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


            return PartialView(objmodel);
        }

        [HttpPost]
        public JsonResult DeleteECM(string id)
        {


            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            Kdl_EmptyMovement objmodel = new Kdl_EmptyMovement();
            // Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
            objImport.GetEmptyContMovementDelete(id);



            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public ActionResult EditECM(string id)
        {


            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            Kdl_EmptyMovement objmodel = new Kdl_EmptyMovement();
            // Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
            objImport.GetEmptyContMovement(id);
            if (objImport.DBResponse.Data != null)
                objmodel = (Kdl_EmptyMovement)objImport.DBResponse.Data;

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


            return PartialView(objmodel);
        }

        [HttpGet]
        public ActionResult ViewECM(string id)
        {


            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            Kdl_EmptyMovement objmodel = new Kdl_EmptyMovement();
            // Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
            objImport.GetEmptyContMovement(id);
            if (objImport.DBResponse.Data != null)
                objmodel = (Kdl_EmptyMovement)objImport.DBResponse.Data;

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


            return PartialView(objmodel);
        }
        [HttpGet]
        public ActionResult GetEmptyContMovementList(string ContainerNo = "")
        {

            IEnumerable<Kdl_EmptyMovement> lstMovement = new List<Kdl_EmptyMovement>();
            Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
            objImportRepo.GetEmptyContMovementList(ContainerNo);
            if (objImportRepo.DBResponse.Data != null)
                lstMovement = (IEnumerable<Kdl_EmptyMovement>)objImportRepo.DBResponse.Data;

            return PartialView(lstMovement);




            return PartialView(lstMovement);

        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            objImport.GetEmptyContainerListForMovement();
            if (objImport.DBResponse.Status > 0)
                ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.EmptyContList = null;

            return Json(ViewBag.EmptyContList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
        {
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            Kdl_ImportRepository objImport = new Kdl_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeLists(string PartyCode, int Page)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetImpPaymentPayeeForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEmptyContainerMovement(Kdl_EmptyMovement objImp)
        {



            Kdl_ImportRepository objIR = new Kdl_ImportRepository();


            objIR.AddEditEmptyContMovement(objImp, ((Login)(Session["LoginUser"])).Uid);
            return Json(objIR.DBResponse);
        }

        //[HttpGet]
        //public ActionResult GetEmptyContMovementList(string ContainerNo)
        //{

        //    IEnumerable<Kdl_EmptyMovement> lstMovement = new List<Kdl_EmptyMovement>();
        //    Kdl_ImportRepository objImportRepo = new Kdl_ImportRepository();
        //    objImportRepo.GetEmptyContMovementList(ContainerNo);
        //    if (objImportRepo.DBResponse.Data != null)
        //        lstMovement = (IEnumerable<Kdl_EmptyMovement>)objImportRepo.DBResponse.Data;

        //    return PartialView(lstMovement);




        //    return PartialView(lstMovement);

        //}
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
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetPaymentPartyForImportInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //public JsonResult GetOEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId,
        //    List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, string SEZ)
        //{

        //    string XMLText = "";
        //    if (lstPaySheetContainer != null)
        //    {
        //        XMLText = Utility.CreateXML(lstPaySheetContainer);
        //    }

        //    Wfld_ImportRepository objImport = new Wfld_ImportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId, SEZ);
        //    var Output = (WFLDInvoiceYard)objImport.DBResponse.Data;

        //    Output.InvoiceDate = InvoiceDate;

        //    Output.Module = "EC";
        //    Output.lstPrePaymentCont.ToList().ForEach(item =>
        //    {
        //        if (!Output.ShippingLineName.Contains(item.ShippingLineName))
        //            Output.ShippingLineName += item.ShippingLineName + ", ";
        //        if (!Output.CHAName.Contains(item.CHAName))
        //            Output.CHAName += item.CHAName + ", ";
        //        if (!Output.ImporterExporter.Contains(item.ImporterExporter))
        //            Output.ImporterExporter += item.ImporterExporter + ", ";
        //        if (!Output.BOENo.Contains(item.BOENo))
        //            Output.BOENo += item.BOENo + ", ";
        //        if (!Output.BOEDate.Contains(item.BOEDate))
        //            Output.BOEDate += item.BOEDate + ", ";
        //        if (!Output.CFSCode.Contains(item.CFSCode))
        //            Output.CFSCode += item.CFSCode + ", ";
        //        if (!Output.ArrivalDate.Contains(item.ArrivalDate))
        //            Output.ArrivalDate += item.ArrivalDate + ", ";
        //        if (!Output.DestuffingDate.Contains(item.DestuffingDate))
        //            Output.DestuffingDate += item.DestuffingDate + ", ";
        //        if (!Output.StuffingDate.Contains(item.StuffingDate))
        //            Output.StuffingDate += item.StuffingDate + ", ";
        //        if (!Output.CartingDate.Contains(item.CartingDate))
        //            Output.CartingDate += item.CartingDate + ", ";
        //        if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
        //        {
        //            Output.lstPostPaymentCont.Add(new WFLDPostPaymentContainer
        //            {
        //                CargoType = item.CargoType,
        //                CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
        //                CFSCode = item.CFSCode,
        //                CIFValue = item.CIFValue,
        //                ContainerNo = item.ContainerNo,
        //                ArrivalDate = item.ArrivalDate,
        //                ArrivalTime = item.ArrivalTime,
        //                DeliveryType = item.DeliveryType,
        //                DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
        //                Duty = item.Duty,
        //                GrossWt = item.GrossWeight,
        //                Insured = item.Insured,
        //                NoOfPackages = item.NoOfPackages,
        //                Reefer = item.Reefer,
        //                RMS = item.RMS,
        //                Size = item.Size,
        //                SpaceOccupied = item.SpaceOccupied,
        //                SpaceOccupiedUnit = item.SpaceOccupiedUnit,
        //                StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
        //                WtPerUnit = item.WtPerPack,
        //                AppraisementPerct = item.AppraisementPerct,
        //                HeavyScrap = item.HeavyScrap,
        //                StuffCUM = item.StuffCUM
        //            });
        //        }


        //        Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
        //        Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
        //        Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
        //        Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
        //        Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
        //        Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
        //            + Output.lstPrePaymentCont.Sum(o => o.Duty);


        //        Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
        //        Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
        //        Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
        //        Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
        //        Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
        //        Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
        //        Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.HTTotal = 0;
        //        Output.CWCTDS = 0;
        //        Output.HTTDS = 0;
        //        Output.CWCTDSPer = 0;
        //        Output.HTTDSPer = 0;
        //        Output.TDS = 0;
        //        Output.TDSCol = 0;
        //        Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.RoundUp = 0;// Output.InvoiceAmt - Output.AllTotal;
        //    });



        //    return Json(Output, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEMptyContMovement(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<WFLDInvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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
                Wfld_ImportRepository objImport = new Wfld_ImportRepository();
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


        #region IRN Responce
        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {


            Kdl_ImportRepository objPpgRepo = new Kdl_ImportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    log.Info("Invoice No" + ERes);
                    if (ERes.Status == 1)
                    {
                        log.Info("Invoice No" + InvoiceNo);
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
                    }
                    else
                    {
                        log.Info("Invoice No" + InvoiceNo);
                        log.Info(ERes.ErrorDetails.ErrorMessage);
                        log.Info(ERes.ErrorDetails.ErrorCode);
                        objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                        objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                    }
                }
                else
                {

                    Einvoice Eobj = new Einvoice();
                    objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                    Kdl_IrnB2CDetails irnb2cobj = new Kdl_IrnB2CDetails();
                    irnb2cobj = (Kdl_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                    if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                    {
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                        objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }
                    else
                    {
                        var tn = "GST QR";
                        UpiQRCodeInfo idata = new UpiQRCodeInfo();
                        idata.ver = irnb2cobj.ver;
                        idata.mode = irnb2cobj.mode;
                        idata.mode = irnb2cobj.mode;
                        idata.tr = irnb2cobj.tr;
                        idata.tid = irnb2cobj.tid;
                        idata.tn = tn;
                        idata.pa = irnb2cobj.pa;
                        idata.pn = irnb2cobj.pn;
                        idata.mc = irnb2cobj.mc;
                        idata.am = irnb2cobj.TotInvVal;
                        idata.mam = irnb2cobj.TotInvVal;
                        idata.mid = irnb2cobj.mid;
                        idata.msid = irnb2cobj.msid;
                        idata.orgId = irnb2cobj.orgId;
                        idata.mtid = irnb2cobj.mtid;
                        idata.CESS = irnb2cobj.CESS;
                        idata.CGST = irnb2cobj.CGST;
                        idata.SGST = irnb2cobj.SGST;
                        idata.IGST = irnb2cobj.IGST;
                        idata.GSTIncentive = irnb2cobj.GSTIncentive;
                        idata.GSTPCT = irnb2cobj.GSTPCT;
                        idata.qrMedium = irnb2cobj.qrMedium;
                        idata.invoiceNo = irnb2cobj.DocNo;
                        idata.InvoiceDate = irnb2cobj.DocDt;
                        idata.InvoiceName = irnb2cobj.InvoiceName;
                        idata.QRexpire = irnb2cobj.DocDt;
                        idata.pinCode = irnb2cobj.pinCode;
                        idata.tier = irnb2cobj.tier;
                        idata.gstIn = irnb2cobj.gstIn;
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        objresponse = Eobj.GenerateB2cQRCode(idata);
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }


                }

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                Kdl_IrnB2CDetails irnb2cobj = new Kdl_IrnB2CDetails();
                irnb2cobj = (Kdl_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                    objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }
                else
                {
                    var tn = "GST QR";
                    UpiQRCodeInfo idata = new UpiQRCodeInfo();
                    idata.ver = irnb2cobj.ver;
                    idata.mode = irnb2cobj.mode;
                    idata.mode = irnb2cobj.mode;
                    idata.tr = irnb2cobj.tr;
                    idata.tid = irnb2cobj.tid;
                    idata.tn = tn;
                    idata.pa = irnb2cobj.pa;
                    idata.pn = irnb2cobj.pn;
                    idata.mc = irnb2cobj.mc;
                    idata.am = irnb2cobj.TotInvVal;
                    idata.mam = irnb2cobj.TotInvVal;
                    idata.mid = irnb2cobj.mid;
                    idata.msid = irnb2cobj.msid;
                    idata.orgId = irnb2cobj.orgId;
                    idata.mtid = irnb2cobj.mtid;
                    idata.CESS = irnb2cobj.CESS;
                    idata.CGST = irnb2cobj.CGST;
                    idata.SGST = irnb2cobj.SGST;
                    idata.IGST = irnb2cobj.IGST;
                    idata.GSTIncentive = irnb2cobj.GSTIncentive;
                    idata.GSTPCT = irnb2cobj.GSTPCT;
                    idata.qrMedium = irnb2cobj.qrMedium;
                    idata.invoiceNo = irnb2cobj.DocNo;
                    idata.InvoiceDate = irnb2cobj.DocDt;
                    idata.InvoiceName = irnb2cobj.InvoiceName;
                    idata.QRexpire = irnb2cobj.DocDt;
                    idata.pinCode = irnb2cobj.pinCode;
                    idata.tier = irnb2cobj.tier;
                    idata.gstIn = irnb2cobj.gstIn;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }

            }


            return Json(objPpgRepo.DBResponse);
        }
        #endregion



        #region Import Bond Conversion Godown

        [HttpGet]
        public ActionResult ImportBondConversion()
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            List<KDL_OBLNoForBondConversion> lstOBL = new List<KDL_OBLNoForBondConversion>();
            ObjIR.GetAllOBLNo();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.OBLList = ObjIR.DBResponse.Data;// new SelectList((List<DSR_OBLNoForBondConversion>)ObjIR.DBResponse.Data);
            }
            else
            {
                ViewBag.OBLList = null;
            }

            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
            ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
            }
            ViewBag.GodownList = lstGodown;

            List<KDL_ImportBondConversion> lstSac = new List<KDL_ImportBondConversion>();
            //ObjIR.GetAllSacNo();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            //}
            ViewBag.SacList = lstSac;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSacNo(string SacNo = "")
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetAllSacNo(SacNo);
            List<KDL_ImportBondConversion> lstSac = new List<KDL_ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<KDL_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGodownByDestuffingId(int destuffingId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetGodwonById(destuffingId);
            if (ObjIR.DBResponse.Data != null)
            {
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetLocationForBondTransfer(int destuffingId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetLocationForBondTransfer(destuffingId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOBLDetailsByDestuffingId(int DestuffingEntryId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetOBLDetailsByDestuffingIdList(DestuffingEntryId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSACNoByIMPCHA(int ChaId, int ImporterId, string OBLNo, string SacNo)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            List<KDL_ImportBondConversion> lstSac = new List<KDL_ImportBondConversion>();
            ObjIR.GetAllSacNo(SacNo, ChaId, ImporterId, OBLNo);
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<KDL_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return Json(lstSac, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownBySacNo(string SACNo)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
            ObjIR.GetAllGodownExpBond(((Login)(Session["LoginUser"])).Uid, SACNo);
            if (ObjIR.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
            }
            return Json(lstGodown, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            Kdl_ImportRepository objER = new Kdl_ImportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.DSRGodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.DSRGodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetOBLDetailsByDestuffingIdOLD(int DestuffingEntryId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetOBLDetailsByDestuffingId(DestuffingEntryId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBondMovementList()
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            ObjIR.GetAllInternalBondMovement("GODN");
            List<KDL_ImportBondConversion> LstMovement = new List<KDL_ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<KDL_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditBondConversionMovement(int MovementId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            KDL_ImportBondConversion ObjInternalMovement = new KDL_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (KDL_ImportBondConversion)ObjIR.DBResponse.Data;
                try
                {
                    if (ObjInternalMovement.WRDate != null && ObjInternalMovement.WRDate != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(ObjInternalMovement.WRDate, out dDate))
                        {
                            if (dDate < Convert.ToDateTime("01/01/1970"))
                            {
                                ObjInternalMovement.WRDate = "";
                            }
                        }
                        else
                        {
                            ObjInternalMovement.WRDate = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjInternalMovement.WRDate = "";
                }

                //List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                //ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                //}
                //ViewBag.GodownList = lstGodown;

                List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                ObjIR.GetAllGodownExpBond(((Login)(Session["LoginUser"])).Uid, ObjInternalMovement.SACNo);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                }
                ViewBag.GodownList = lstGodown;

                List<KDL_ImportBondConversion> lstSac = new List<KDL_ImportBondConversion>();
                //ObjIR.GetAllSacNo();
                ObjIR.GetAllSacNo("", ObjInternalMovement.CHAId, ObjInternalMovement.ImporterId);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstSac = (List<KDL_ImportBondConversion>)ObjIR.DBResponse.Data;
                }
                ViewBag.SacList = lstSac;
            }
            return PartialView("EditImportBondConversion", ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewBondConversionMovement(int MovementId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            PPG_Internal_Movement ObjInternalMovement = new PPG_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_Movement)ObjIR.DBResponse.Data;

            }
            return PartialView(ObjInternalMovement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditImportBondConversion(KDL_ImportBondConversion objForm)
        {
            try
            {
                Kdl_ImportRepository objChargeMaster = new Kdl_ImportRepository();
                objChargeMaster.AddEditImportBondConversion(objForm);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ViewBondInternalMovement(int MovementId)
        {
            Kdl_ImportRepository ObjIR = new Kdl_ImportRepository();
            KDL_ImportBondConversion ObjInternalMovement = new KDL_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (KDL_ImportBondConversion)ObjIR.DBResponse.Data;
                try
                {
                    if (ObjInternalMovement.WRDate != null && ObjInternalMovement.WRDate != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(ObjInternalMovement.WRDate, out dDate))
                        {
                            if (dDate < Convert.ToDateTime("01/01/1970"))
                            {
                                ObjInternalMovement.WRDate = "";
                            }
                        }
                        else
                        {
                            ObjInternalMovement.WRDate = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjInternalMovement.WRDate = "";
                }
            }
            return PartialView("ViewImportBondConversion", ObjInternalMovement);
        }

        #endregion
    }
}
