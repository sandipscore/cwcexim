using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System.IO;
using DynamicExcel;
using EinvoiceLibrary;
using System.Threading.Tasks;

namespace CwcExim.Areas.Import.Controllers
{
    public class CWCImportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Job Order
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            ImportRepository objIR = new ImportRepository();
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
            ImportRepository objIR = new ImportRepository();
            IList<ImportJobOrderList> lstIJO = new List<ImportJobOrderList>();
            objIR.GetAllImpJO();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            ImportRepository objIR = new ImportRepository();
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
            ImportRepository objIR = new ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            ImportJobOrder objImp = new ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (ImportJobOrder)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
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
                ImportRepository objIR = new ImportRepository();
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
            ImportRepository objIR = new ImportRepository();
            objIR.GetBlNoDtl(FormOneId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            ImportRepository objIR = new ImportRepository();
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
            ImportRepository objIR = new ImportRepository();
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

        #region  Custom Appraisement

        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            ImportRepository ObjIR = new ImportRepository();
            CustomAppraisement ObjAppraisement = new CustomAppraisement();
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetContnrNoForCustomAppraise();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<CustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
            }
            ObjIR.ListOfCHA();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjIR.ListOfShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            return PartialView("CreateCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CreateCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode,string LineNo)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCustomAppraisementList()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<CustomAppraisement> LstAppraisement = new List<CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }

        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            CustomAppraisement ObjAppraisement = new CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (CustomAppraisement)ObjIR.DBResponse.Data;
                    ObjIR.ListOfCHA();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                    }
                    else
                    {
                        ViewBag.CHAList = null;
                    }
                    ObjIR.ListOfShippingLine();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                    }
                    else
                    {
                        ViewBag.ShippingLineList = null;
                    }
                }
            }
            return PartialView("EditCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/EditCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public ActionResult ViewCustomAppraisement(int CustomAppraisementId)
        {
            CustomAppraisement ObjAppraisement = new CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (CustomAppraisement)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/ViewCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelCustomAppraisement(CustomAppraisementId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCustomAppraisement(CustomAppraisement ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<CustomAppraisementDtl> LstAppraisement = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = UtilityClasses.Utility.CreateXML(LstAppraisement);
                }
                ImportRepository ObjIR = new ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCustomAppraisement(ObjAppraisement, AppraisementXML);
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        #endregion

        #region  Custom Appraisement Work Order

        [HttpGet]
        public ActionResult CreateCstmAppraiseWorkOrder()
        {
            CstmAppraiseWorkOrder ObjAppraisement = new CstmAppraiseWorkOrder();
            ImportRepository ObjIR = new ImportRepository();
            YardRepository ObjYR = new YardRepository();
            ObjAppraisement.WorkOrderDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetAppraisementNoForWorkOrdr();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.AppraisementList = new SelectList((List<CstmAppraiseWorkOrder>)ObjIR.DBResponse.Data, "CustomAppraisementId", "AppraisementNo");
            }
            else
            {
                ViewBag.AppraisementList = null;
            }
            ObjYR.GetAllYard();
            if (ObjYR.DBResponse.Data != null)
            {
                ViewBag.YardList = new SelectList((List<CwcExim.Models.Yard>)ObjYR.DBResponse.Data, "YardId", "YardName");
            }
            else
            {
                ViewBag.YardList = null;
            }
            return PartialView("CreateCstmAppraiseWorkOrder", ObjAppraisement);
        }

        [HttpGet]
        public JsonResult GetAppraisementList(int CustomAppraisementId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetCstmAppraiseDtlForWorkOrdr(CustomAppraisementId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCstmAppraiseWorkOrderList()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<CstmAppraiseWorkOrder> LstAppraisement = new List<CstmAppraiseWorkOrder>();
            ObjIR.GetAllCstmAppraiseWorkOrder();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<CstmAppraiseWorkOrder>)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraiseWorkOrderList", LstAppraisement);
        }

        [HttpGet]
        public ActionResult EditCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            CstmAppraiseWorkOrder ObjWorkOrder = new CstmAppraiseWorkOrder();
            if (CstmAppraiseWorkOrderId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjWorkOrder = (CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                    //ObjIR.GetYardWiseLocation(ObjWorkOrder.YardId);
                    //if (ObjIR.DBResponse.Data != null)
                    //{
                    //    ViewBag.YardList = new SelectList((List<Areas.Import.Models.YardWiseLocation>)ObjIR.DBResponse.Data, "YardId", "YardName");
                    //}
                    //else
                    //{
                    //    ViewBag.YardList = null;
                    //}
                }
            }
            return PartialView("EditCstmAppraiseWorkOrder", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            CstmAppraiseWorkOrder ObjWorkOrder = new CstmAppraiseWorkOrder();
            if (CstmAppraiseWorkOrderId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjWorkOrder = (CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCstmAppraiseWorkOrder", ObjWorkOrder);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            if (CstmAppraiseWorkOrderId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCstmAppraiseWorkOrder(CstmAppraiseWorkOrder ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                //List<int> LstYard = new List<int>();
                //string YardXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<CustomAppraisementDtl> LstAppraisement = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                //if (ObjAppraisement.YardWiseLocationIds != null)
                //{
                //    string[] YardList = ObjAppraisement.YardWiseLocationIds.Split(',');
                //    foreach (string Data in YardList)
                //        LstYard.Add(Convert.ToInt32(Data));
                //    YardXML = Utility.CreateXML(LstYard);
                //}
                ImportRepository ObjIR = new ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCstmAppraiseWorkOrdr(ObjAppraisement, AppraisementXML /*, YardXML*/ );
                ModelState.Clear();
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
        public JsonResult PrintCustomAppraisement(int CstmAppraiseWorkOrderId)
        {
            CstmAppraiseWorkOrder ObjCustom = new CstmAppraiseWorkOrder();
            string FilePath = "";
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetCstmAppraiseWOForPreview(CstmAppraiseWorkOrderId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjCustom = (CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                FilePath = GeneratePDFForCstmAppraiseWO(ObjCustom, CstmAppraiseWorkOrderId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }

        [NonAction]
        private string GeneratePDFForCstmAppraiseWO(CstmAppraiseWorkOrder ObjCustom, int CstmAppraiseWorkOrderId)
        {
            string Html = "";
            string CHA = "", Importer = "", NoOfPackages = "", GrossWeight = "", Vessel = "", ContainerNo = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            ObjCustom.LstAppraisementDtl.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item => 
            {
                if (Importer == "")
                    Importer = item.Importer;
                else
                    Importer += "," + item.Importer;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item => 
            {
                if (CHA == "")
                    CHA = item.CHA;
                else
                    CHA += "," + item.CHA;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "," + item.Vessel;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerNo == "")
                    ContainerNo = item.ContainerNo;
                else
                    ContainerNo += "," + item.ContainerNo;
            });
            GrossWeight = ((ObjCustom.LstAppraisementDtl.Sum(x => x.GrossWeight) > 0) ? ObjCustom.LstAppraisementDtl.Sum(x => x.GrossWeight).ToString()+" KG" : "");
            NoOfPackages = ((ObjCustom.LstAppraisementDtl.Sum(x => x.NoOfPackages) > 0) ? ObjCustom.LstAppraisementDtl.Sum(x => x.NoOfPackages).ToString() : "");
            var Location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId + "/CustomAppraisementWorkOrder.pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId);
            }
            //if (System.IO.File.Exists(Location))
            //{
            //    System.IO.File.Delete(Location);
            //}
            string DeliveryTpe = ((ObjCustom.DeliveryType == 1) ? "Cargo Delivery" : "Container Delivery");
          //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:14pt;'>केंद्रीय भंडारण निगम</span><br/><span>भारत सरकार के उपक्रम</span><br/><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,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'/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No. ..................</th><th style='text-align:right;'>Container Freight Station<br/>18, Coal Dock Road,<br/>Kolkata - 700 043</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To सेवा मे,</th><th style='text-align:right;'>Dated "+DateTime.Now.ToString("dd/MM/yyyy")+"</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;'>कार्य-आदेश <br/><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir, महोदय,</td></tr><tr><td style='width:5%;'></td><td>कृपया तुरंत नीचे उल्लेखित कार्य निष्पादित करने की व्यवस्था करें:<br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'>1.</td><td>आयातक, निर्यातक का नाम<br/>Importer's / Exporter's Name "+Importer+"</td></tr><tr> <td style='vertical-align: bottom;'>2.</td> <td>सीएए का नाम<br/>CHA's Name "+CHA+"</td></tr><tr><td style='vertical-align: bottom;'>3.</td><td>डिलिवरी / डिस्टफिंग / कार्टिंग / स्टफ़िंग<br/>Custom Appraisement.</td></tr><tr><td style='vertical-align: bottom;'>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td>पैकेट की संख्या</td><td>भ।र</td></tr><tr><td>No. of packages "+NoOfPackages+"</td><td> Weight "+GrossWeight+"</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'>5.</td><td>ट्रक नं<br/> Truck No. "+ Vessel + "</td></tr><tr><td style='vertical-align: bottom;'>6.</td> <td>स्थान<br/>Location "+ObjCustom.YardWiseLctnNames+"</td></tr><tr><td style='vertical-align: bottom;'>7.</td><td>कंटेनर<br/>Container no. :"+ContainerNo+"</td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>हस्ताक्षर<br/>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: "+ObjCustom.AppraisementNo+"</th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,</th><th style='text-align:right;'>Dated: " + DateTime.Now.ToString("dd/MM/yyyy") + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + Importer + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHA + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Custom Appraisement "+DeliveryTpe+".</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfPackages + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: " + ObjCustom.YardWiseLctnNames + "</td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : " + ContainerNo + "</td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var ReportingHelper = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                ReportingHelper.GeneratePDF(Location, Html);
            }
            return "/Docs/" + Session.SessionID + "/" + CstmAppraiseWorkOrderId + "/CustomAppraisementWorkOrder.pdf";
        }

        #endregion

        #region Custom Appraisement Approval
        [HttpGet]
        public ActionResult ListOfApprsmntAppr()
        {
            ImportRepository objIR = new ImportRepository();
            objIR.NewCustomeAppraisement(0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfApp = Jobject["lstApproval"];

                // ViewBag.ListOfApp =objIR.DBResponse.Data;
                // var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.ListOfApp = jobject["Details"];
            }
            else
            {
                ViewBag.ListOfApp = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult LoadMoredata(int Skip, string status)
        {
            ImportRepository objIR = new ImportRepository();
            if (status == "N")
            {
                objIR.NewCustomeAppraisement(Skip);
            }
            else
            {
                objIR.ApprovalHoldCustomAppraisement(Skip);
            }
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadApprovalPage(int CstmAppraiseWorkOrderId)
        {
            ImportRepository ObjIR = new ImportRepository();
            CstmAppraiseWorkOrder ObjWorkOrder = new CstmAppraiseWorkOrder();
            ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjWorkOrder = (CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraisementApproval", ObjWorkOrder);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApproval(int CstmAppraiseWorkOrderId, int IsApproved)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.UpdateCustomApproval(CstmAppraiseWorkOrderId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

        #region Destuffing Application

        [HttpGet]
        public ActionResult CreateDestuffingApp()
        {
            ImportRepository ObjIR = new ImportRepository();
            Destuffing ObjDestuffing = new Destuffing();
            ObjDestuffing.DestuffingDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetContnrNoForDestuffing(Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<DestuffingDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
            }
            ObjIR.ListOfCHA();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjIR.ListOfShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            return PartialView("CreateDestuffingApp", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult GetCntrDetForDestuffing(string CFSCode, string LineNo)
        {

            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetContnrDetForDestuffing(CFSCode, LineNo, Convert.ToInt32(Session["BranchId"]));
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingAppList()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<DestuffingAppList> LstDestuffing = new List<DestuffingAppList>();
            ObjIR.GetAllDestuffing(Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<DestuffingAppList>)ObjIR.DBResponse.Data;
            }
            return PartialView("GetDestuffingAppList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingApp(int DestuffingId)
        {
            Destuffing ObjDestuffing = new Destuffing();
            if (DestuffingId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetDestuffing(DestuffingId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Destuffing)ObjIR.DBResponse.Data;
                    ObjIR.ListOfCHA();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                    }
                    else
                    {
                        ViewBag.CHAList = null;
                    }
                    ObjIR.ListOfShippingLine();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                    }
                    else
                    {
                        ViewBag.ShippingLineList = null;
                    }
                    ObjIR.GetContnrNoForDestuffing(Convert.ToInt32(Session["BranchId"]));
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.ContainerList = new SelectList((List<DestuffingDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
                    }
                    else
                    {
                        ViewBag.ContainerList = null;
                    }
                }
            }
            return PartialView("EditDestuffingApp", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingApp(int DestuffingId)
        {
            Destuffing ObjDestuffing = new Destuffing();
            if (DestuffingId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetDestuffing(DestuffingId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Destuffing)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingApp", ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteDestuffingApp(int DestuffingId)
        {
            if (DestuffingId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelDestuffing(DestuffingId, Convert.ToInt32(Session["BranchId"]));
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingApp(Destuffing ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingXML = "";
                if (ObjDestuffing.DestuffingXML != null)
                {
                    List<DestuffingDtl> LstAppraisement = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DestuffingDtl>>(ObjDestuffing.DestuffingXML);
                    DestuffingXML = Utility.CreateXML(LstAppraisement);
                }
                ImportRepository ObjIR = new ImportRepository();
                ObjDestuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditDestuffing(ObjDestuffing, DestuffingXML, Convert.ToInt32(Session["BranchId"]));
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetSearchDestuffingAppList(string SearchText)
        {
            ImportRepository ObjIR = new ImportRepository();
            List<DestuffingAppList> LstDestuffing = new List<DestuffingAppList>();
            if (string.IsNullOrEmpty(SearchText))
            {
                ObjIR.GetAllDestuffing(Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    LstDestuffing = (List<DestuffingAppList>)ObjIR.DBResponse.Data;
                }
            }
            else
            {
                ObjIR.GetSearchDestuffing(Convert.ToInt32(Session["BranchId"]), SearchText);
                if (ObjIR.DBResponse.Data != null)
                {
                    LstDestuffing = (List<DestuffingAppList>)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("GetDestuffingAppList", LstDestuffing);
        }

        #endregion

        #region Destuffing Work Order
        [HttpGet]
        public ActionResult CreateDeStuffWO()
        {
            DestuffingWO objWO = new DestuffingWO();
            objWO.WorkOrderDate = DateTime.Now.ToString("dd/MM/yy");
            ImportRepository objIR = new ImportRepository();
            objIR.ListOfGodown();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfGodown = new SelectList((List<Areas.Import.Models.Godown>)objIR.DBResponse.Data, "GodownId", "GodownName");
            objIR.GetContainerDetails();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCont = new SelectList((List<ContDetails>)objIR.DBResponse.Data, "DestuffingId", "ContainerNo");
            return PartialView(objWO);
        }
        [HttpGet]
        public ActionResult EditDeStuffWO(int DeStuffingWOId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.ListOfGodown();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfGodown = new SelectList((List<Areas.Import.Models.Godown>)objIR.DBResponse.Data, "GodownId", "GodownName");
            DestuffingWO objDSWO = new DestuffingWO();
            if (DeStuffingWOId > 0)
                objIR.GetDestuffingWODet(DeStuffingWOId, "edit");
            if (objIR.DBResponse.Data != null)
                objDSWO = (DestuffingWO)objIR.DBResponse.Data;
            return PartialView(objDSWO);
        }
        [HttpGet]
        public ActionResult ViewDeStuffWO(int DeStuffingWOId)
        {
            ImportRepository objIR = new ImportRepository();
            DestuffingWO objDSWO = new DestuffingWO();
            if (DeStuffingWOId > 0)
                objIR.GetDestuffingWODet(DeStuffingWOId);
            if (objIR.DBResponse.Data != null)
                objDSWO = (DestuffingWO)objIR.DBResponse.Data;
            return PartialView(objDSWO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeStuffWO(DestuffingWO objWO)
        {
            if (ModelState.IsValid)
            {
                string XML = "";
                IList<DestuffingWODtl> lstDtl = new List<DestuffingWODtl>();
                IList<int> lstDtlId = new List<int>();
                if (objWO.StringifyXML != null)
                {
                    lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DestuffingWODtl>>(objWO.StringifyXML);
                    foreach (DestuffingWODtl dtl in lstDtl)
                        lstDtlId.Add(Convert.ToInt32(dtl.DestuffingDtlId));
                    XML = Utility.CreateXML(lstDtlId);
                }
                ImportRepository objIR = new ImportRepository();
                objIR.AddEditDeStuffWO(objWO, XML);
                return Json(objIR.DBResponse);
            }
            else
            {
                var Err = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDeStuffWO(int DeStuffingWOId)
        {
            ImportRepository objIR = new ImportRepository();
            if (DeStuffingWOId > 0)
                objIR.DeleteDestuffingWO(DeStuffingWOId);
            return Json(objIR.DBResponse);

        }
        [HttpGet]
        public ActionResult ListDeStuffWO()
        {
            ImportRepository objIR = new ImportRepository();
            objIR.ListOfDeStuffWO();
            IList<ListDestuffingWO> lstWO = new List<ListDestuffingWO>();
            if (objIR.DBResponse.Data != null)
                lstWO = (List<ListDestuffingWO>)objIR.DBResponse.Data;
            return PartialView(lstWO);
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
        public JsonResult GetConatinerWiseWODet(int DestuffingId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.GetContainerWiseWODet(DestuffingId);
            object obj = null;
            if (objIR.DBResponse.Data != null)
                obj = objIR.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDestuffingWO(int DeStuffingWOId)
        {
            DestuffingWO ObjDestuffing = new DestuffingWO();
            string FilePath = "";
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetDestuffingWOForPreview(DeStuffingWOId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDestuffing = (DestuffingWO)ObjIR.DBResponse.Data;
                FilePath = GenerateDestuffingWOPDF(ObjDestuffing, DeStuffingWOId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }

        [NonAction]
        private string GenerateDestuffingWOPDF(DestuffingWO ObjDestuffing, int DeStuffingWOId)
        {
            string Html = "";
            string ImporterName = "", CHAName = "", GrossWeight = "", NoOfPackages = "", Vessel = "", ContainerNo = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            List<DestuffingWODtl> LstDestuffing = new List<DestuffingWODtl>();
            LstDestuffing = JsonConvert.DeserializeObject<List<DestuffingWODtl>>(ObjDestuffing.StringifyXML);
            if (LstDestuffing.Count > 0)
            {
                LstDestuffing.Select(x => new { ImporterName = x.ImporterName }).Distinct().ToList().ForEach(item =>
                {
                    if (ImporterName == "")
                        ImporterName = item.ImporterName;
                    else
                        ImporterName += "," + item.ImporterName;
                });

                LstDestuffing.Select(x => new { CHAName = x.CHAName }).Distinct().ToList().ForEach(item =>
                {
                    if (CHAName == "")
                        CHAName = item.CHAName;
                    else
                        CHAName += "," + item.CHAName;
                });

                LstDestuffing.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
                {
                    if (Vessel == "")
                        Vessel = item.Vessel;
                    else
                        Vessel += "," + item.Vessel;
                });

                LstDestuffing.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
                {
                    if (ContainerNo == "")
                        ContainerNo = item.ContainerNo;
                    else
                        ContainerNo += "," + item.ContainerNo;
                });

                GrossWeight = ((LstDestuffing.Sum(x => x.GrossWeight) > 0) ? LstDestuffing.Sum(x => x.GrossWeight).ToString()+" KG" : "");
                NoOfPackages = ((LstDestuffing.Sum(x => x.NoOfPackages) > 0) ? LstDestuffing.Sum(x => x.NoOfPackages).ToString() : "");
                var Locaion = Server.MapPath("~/Docs/") + Session.SessionID + "/" + DeStuffingWOId + "/DestuffingWorkOrder.pdf";
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + DeStuffingWOId))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID + "/" + DeStuffingWOId);
                }
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,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'/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: "+ObjDestuffing.WorkOrderNo+"</th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,"+ObjDestuffing.ContractorName+"</th><th style='text-align:right;'>Dated: " + DateTime.Now.ToString("dd/MM/yyyy") + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER FOR DESTUFFING</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ImporterName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>"+ObjDestuffing.DestuffingNo+"</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfPackages + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td> <td><br/>Location: " + ObjDestuffing.GodwnWiseLctnNames + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td><td><br/>Container no. : " + ContainerNo + "</td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Shipping Line : " + ObjDestuffing.ShippingLine + "</td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
                using (var ReportingHelper = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    ReportingHelper.GeneratePDF(Locaion, Html);
                }
               
            }
            return "/Docs/" + Session.SessionID + "/" + DeStuffingWOId + "/DestuffingWorkOrder.pdf";
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
            ObjIR.GetContrDetForDestuffingEntry(DestuffingDtlId, Convert.ToInt32(Session["BranchId"]));
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
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
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
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
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
                ObjIR.AddEditDestuffingEntry(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]));
                ModelState.Clear();
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
            ObjIR.GetDestuffEntryForPrint(DestuffingEntryId);
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
            Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;'>CENTRAL WAREHOUSING CORPORATION<br/>(A Govt. of India Undertaking)<br/>CONTAINER FREIGHT STATION<br/>DESTUFFING SHEET FCL/LCL</th></tr></thead><tbody><tr><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing no.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.DestuffingEntryNo + "</td></tr><tr><td style='text-align:left;padding:5px;'>Container no.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.ContainerNo + "</td></tr><tr><td style='text-align:left;padding:5px;'>CFS Code</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.CFSCode + "</td></tr><tr><td style='text-align:left;padding:5px;'>Vessel</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Vessel + "</td><td style='text-align:left;padding:5px;'>Voyage</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Voyage + "</td></tr><tr><td style='text-align:left;padding:5px;'>S/L Seal No.</td><td colspan='3' style='text-align:left;padding:5px;'>" + ObjDestuff.SealNo + "</td></tr></tbody></table></td><td style='width:50%;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;padding:5px;width:30%;'>De-Stuffing Date</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.DestuffingEntryDate + "</td></tr><tr><td style='text-align:left;padding:5px;'>Container Size</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Size + "</td></tr><tr><td style='text-align:left;padding:5px;'>Shipping Line</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.ShippingLine + "</td></tr><tr><td style='text-align:left;padding:5px;'>Rotation</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.Rotation + "</td></tr><tr><td style='text-align:left;padding:5px;'>Custom Seal No.</td><td style='text-align:left;padding:5px;'>" + ObjDestuff.CustomSealNo + "</td></tr></tbody></table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th style='border:1px solid #000;text-align:center;padding:5px;'>Line No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOE Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>BOL Date</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Marks & No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CHA</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Importer</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Desc.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Commodity</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Cargo Type</th><th style='border:1px solid #000;text-align:center;padding:5px;'>No of pack</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Bay No.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Grid Occup</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Gross Wt.</th><th style='border:1px solid #000;text-align:center;padding:5px;'>CIF Value</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Duty</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Area</th><th style='border:1px solid #000;text-align:center;padding:5px;'>Volume</th></tr></thead><tbody>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            string html2 = "";
            ObjDestuff.LstDestuffingEntry.ToList().ForEach(item => 
            {
                html2 += "<tr><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.LineNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOENo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOEDate + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOLNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.BOLDate + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.MarksNo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CHA + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Importer + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CargoDescription + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Commodity + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Cargo + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.NoOfPackages + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.GodownName + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.GodownWiseLctnNames + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + item.GrossWeight + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + item.CIFValue + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.Duty + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.SQM + "</td><td style='border:1px solid #000;text-align:left;padding:5px;'>" + item.CUM + "</td></tr>";
            });
            string html3= "</tbody></table></td></tr><tr><td colspan='2' style='padding-top:100px;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border-top:1px dotted #000;'><tbody><tr><td style='text-align:left;width:25%;'>Variations observed if any<br/>Signature and Designation<br/>with date and seal of<br/>Rep. / Surveyor / S.Agent <br/> Live / CHA</td><td style='text-align:left;width:25%;'>Shed Asstt CWC,CFS</td><td style='text-align:left;width:25%;'>Shed I/C CWC,CFS</td><td style='text-align:left;width:25%;'>Rap./Surveyor of Handling & Transport Contractor</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html + html2 + html3;
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
            {
                RH.GeneratePDF(Path, Html);
            }

            return "/Docs/" + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
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

        #region Cargo Payment Sheet

        [HttpGet]
        public ActionResult CreatePaymentSheet(string type = "Tax")
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
        public JsonResult GetPaymentSheetCargo(int StuffingReqId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDestuffingContForPaymentSheet(StuffingReqId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCargoPaymentSheet(string InvoiceDate, string InvoiceType, int DestuffingId, int DeliveryType, string DestuffingNo, string DestuffingDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, decimal WeighmentCharges=0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetYardPaymentSheet(InvoiceDate, DestuffingId, DeliveryType,"", DestuffingNo, DestuffingDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName,0, InvoiceType, XMLText, WeighmentCharges,InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetCargoPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objImport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCChargesCargo(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
        }

        [NonAction]
        private void CalculateCWCChargesCargo(PaymentSheetFinalModel finalModel, PaySheetChargeDetails baseModel)
        {
            try
            {
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
                //baseModel.lstLoadedGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
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
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Amount = EGRAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Total = EGRAmt;

                var STAmt = 0m;
                baseModel.lstStorageRent.ToList().ForEach(item =>
                {
                    var Amt1 = item.StuffCUM * item.StorageWeeks * baseModel.RateCUMPerWeek;
                    var Amt2 = item.StuffWeight * item.StorageDays * baseModel.RateMTPerDay;
                    var Amt3 = item.StorageDays < 30 ? (item.StuffSQM * item.StorageWeeks * baseModel.RateSQMPerWeek)
                                                    : ((item.StuffSQM * item.StorageMonths * baseModel.RateSQMPerMonth) + (item.StuffSQM * item.StorageMonthWeeks * baseModel.RateSQMPerWeek));
                    STAmt += Enumerable.Max(new[] { Amt1, Amt2, Amt3 });
                });
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
        public JsonResult AddEditCargoPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                if (formData.lstPSContainer != null)
                {
                    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                }
                if (formData.lstChargesType != null)
                {
                    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                }

                ImportRepository objImport = new ImportRepository();
                objImport.AddEditCargoInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                return Json(objImport.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion


        #region Miscellaneous Invoice
        public ActionResult CreateMiscInvoice()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm)
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
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPYard","","");
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

        #region Yard/cargo Payment Sheet

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
        public JsonResult GetPaymentSheetContainer(int AppraisementId,string Type="I")
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId, Type);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId,decimal Weight, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, decimal mechanical, decimal manual, int distance, int InvoiceId = 0,string Type="I")
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, Weight, InvoiceType, XMLText, mechanical, manual, distance, InvoiceId,Type);

            return Json(objChrgRepo.DBResponse.Data);

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetContainerPaymentSheet(InvoiceDate, AppraisementId, XMLText);
            //var model = (PaySheetChargeDetails)objImport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCChargesContainer(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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

                decimal MechanicalWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Mechanical"]) ? "0" : objForm["Mechanical"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Manual"]) ? "0" : objForm["Manual"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["distance"]) ? "0" : objForm["distance"]);
                decimal Incentive = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Incentive"])? "0": objForm["Incentive"]);
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
                objChargeMaster.AddEditYardInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPYard", ExportUnder, MechanicalWeight, ManualWeight, Distance, Incentive);
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


        #region IRN Responce
        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {

            Kol_ImportRepository objType = new Kol_ImportRepository();
            objType.GetInvoiceType(InvoiceNo);
            string InvoiceType =Convert.ToString(objType.DBResponse.Data);
            Kol_ImportRepository objPpgRepo = new Kol_ImportRepository();
            //objChrgRepo.GetAllCharges();
            if (InvoiceType == "Bill")
            {

                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                KOL_IrnB2CDetails irnb2cobj = new KOL_IrnB2CDetails();
                irnb2cobj = (KOL_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                IrnModel irnModelObj = new IrnModel();
                irnModelObj.DocumentDate = irnb2cobj.DocDt;
                irnModelObj.DocumentNo = irnb2cobj.DocNo;
                irnModelObj.DocumentType = irnb2cobj.DocTyp;
                irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                var dt = DateTime.Now;
                QrCodeInfo obj = new QrCodeInfo();
                QrCodeData objQR = new QrCodeData();
                objQR.Irn = ERes;
                objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                objQR.iss = "NIC";
                objQR.ItemCnt = irnb2cobj.ItemCnt;
                objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                objQR.SellerGstin = irnb2cobj.SellerGstin;
                objQR.TotInvVal =Convert.ToInt32(irnb2cobj.TotInvVal);
                objQR.BuyerGstin = irnb2cobj.BuyerGstin;
                objQR.DocDt = irnb2cobj.DocDt;
                objQR.DocNo = irnb2cobj.DocNo;
                objQR.DocTyp = irnb2cobj.DocTyp;
                obj.Data = (QrCodeData)objQR;
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(obj);
                IrnResponse objERes = new IrnResponse();
                objERes.irn = ERes;
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);

            }
            else
            {
                if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
                {
                    objPpgRepo.GetIRNForYard(InvoiceNo);
                    var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                    if (Output.BuyerDtls.Gstin != "" && Output.BuyerDtls.Gstin != null)
                    {
                        objPpgRepo.GetHeaderIRNForYard();

                        HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                        string jsonEInvoice = JsonConvert.SerializeObject(Output);
                        string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
                        log.Info("Invoice json:" + jsonEInvoice);
                        Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                        IrnResponse ERes = await Eobj.GenerateEinvoice();
                        if (ERes.Status == 0)
                        {
                            log.Info(ERes.ErrorDetails.ErrorMessage);
                            log.Info(ERes.ErrorDetails.ErrorCode);
                            log.Info("Invoice No:" + InvoiceNo);
                            log.Info(ERes.Status);
                        }
                        if (ERes.Status == 1)
                        {
                            objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
                        }
                        else
                        {
                            objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                            objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                        }
                    }
                    else
                    {

                        var tn = "GST QR";
                        Einvoice Eobj = new Einvoice();
                        objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                        KOL_IrnB2CDetails irnb2cobj = new KOL_IrnB2CDetails();
                        irnb2cobj = (KOL_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                        UpiQRCodeInfo idata = new UpiQRCodeInfo();
                        idata.ver = irnb2cobj.ver.ToString();
                        idata.mode = irnb2cobj.mode;
                        idata.mode = irnb2cobj.mode;
                        idata.tr = irnb2cobj.tr;
                        idata.tn = tn;
                        idata.tid = irnb2cobj.tid;
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
                else
                {
                    var tn = "GST QR";
                    Einvoice Eobj = new Einvoice();
                    objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                    KOL_IrnB2CDetails irnb2cobj = new KOL_IrnB2CDetails();
                    irnb2cobj = (KOL_IrnB2CDetails)objPpgRepo.DBResponse.Data;
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
                    idata.orgId= irnb2cobj.orgId;
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
        public JsonResult GetDestuffingPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName,decimal Weight, List<PaymentSheetContainer> lstPaySheetContainer,decimal mechanical,decimal manual,int distance, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDestuffingPaymentSheet(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, Weight, InvoiceType, XMLText, mechanical, manual, distance,InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetContainerPaymentSheet(InvoiceDate, AppraisementId, XMLText);
            //var model = (PaySheetChargeDetails)objImport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCChargesContainer(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

                decimal MechanicalWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Mechanical"]) ? "0" : objForm["Mechanical"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Manual"]) ? "0" : objForm["Manual"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["distance"]) ? "0" : objForm["distance"]);
                decimal Incentive = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Incentive"]) ? "0" : objForm["Incentive"]);

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
                objChargeMaster.AddEditDestuffingInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, ExportUnder, "IMPDest",MechanicalWeight,ManualWeight,Distance, Incentive);
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

        #region Delivery Payment Sheet
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheet(string type = "Tax")
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
        public JsonResult GetPaymentSheetDeliveryCont(int AppraisementId)
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
        public JsonResult GetPaymentSheetDeliveryBOE(List<PaymentSheetContainer> lstPaySheetContainer)
        {
            ImportRepository objImport = new ImportRepository();
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            objImport.GetBOEForPaymentSheet(XMLText);
            if (objImport.DBResponse.Status > 0)
                ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.BOEList = null;

            return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, 
            string PayeeName, List<PaymentSheetBOE> lstPaySheetBOE, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetContainerPaymentSheet(InvoiceDate, AppraisementId, XMLText);
            //var model = (PaySheetChargeDetails)objImport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCChargesContainer(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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

                foreach (var item in invoiceData.lstPreInvoiceCargo)
                {
                    item.CartingDate = (string.IsNullOrEmpty(item.CartingDate) ? "1900-01-01" : item.CartingDate);
                    item.StuffingDate = (string.IsNullOrEmpty(item.StuffingDate) ? "1900-01-01" : item.StuffingDate);
                    item.BOEDate = (string.IsNullOrEmpty(item.BOEDate) ? "1900-01-01" : item.BOEDate);
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
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);
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
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            List<PaymentSheetContainer> lstPaySheetContainer,decimal Weight, string InvoiceFor, decimal mechanical, decimal manual, int distance, int InvoiceId=0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetEmptyContPaymentSheetForKol(InvoiceDate, AppraisementId, DeliveryType,SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText,Weight, InvoiceFor,InvoiceId, mechanical, manual, distance);

            return Json(objChrgRepo.DBResponse.Data);

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetContainerPaymentSheet(InvoiceDate, AppraisementId, XMLText);
            //var model = (PaySheetChargeDetails)objImport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCChargesContainer(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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
                string ExporterUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceFor = objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                Decimal Weight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Weight"]) ? "0" : objForm["Weight"]);
                decimal MechanicalWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Mechanical"]) ? "0" : objForm["Mechanical"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Manual"]) ? "0" : objForm["Manual"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["distance"]) ? "0" : objForm["distance"]);
                decimal Incentive = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Incentive"]) ? "0" : objForm["Incentive"]);

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
                string module = InvoiceFor;
                objChargeMaster.AddEditECInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, module, Weight, InvoiceFor, ExporterUnder, MechanicalWeight, ManualWeight, Distance, Incentive);
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

        #region Issue Slip

        [HttpGet]
        public ActionResult CreateIssueSlip()
        {
            ImportRepository ObjIR = new ImportRepository();
            IssueSlip ObjIssueSlip = new IssueSlip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetInvoiceNoForIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<IssueSlip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            }
            else
            {
                ViewBag.InvoiceNoList = null;
            }
            return PartialView("CreateIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public JsonResult GetInvcDetForIssueSlip(int InvoiceId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssueSlipList()
        {
            ImportRepository ObjIR = new ImportRepository();
            List<IssueSlip> LstIssueSlip = new List<IssueSlip>();
            ObjIR.GetAllIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<IssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }

        [HttpGet]
        public ActionResult EditIssueSlip(int IssueSlipId)
        {
            IssueSlip ObjIssueSlip = new IssueSlip();
            if (IssueSlipId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (IssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewIssueSlip(int IssueSlipId)
        {
            IssueSlip ObjIssueSlip = new IssueSlip();
            if (IssueSlipId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (IssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewIssueSlip", ObjIssueSlip);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteIssueSlip(int IssueSlipId)
        {
            if (IssueSlipId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelIssueSlip(IssueSlipId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditIssueSlip(IssueSlip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIssueSlip.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditIssueSlip(ObjIssueSlip);
                ModelState.Clear();
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
        public JsonResult PrintIssueSlip(int IssueSlipId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetIssueSlipForPreview(IssueSlipId);
            if (ObjIR.DBResponse.Data != null)
            {
                IssueSlip ObjIssueSlip = new IssueSlip();
                ObjIssueSlip = (IssueSlip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForIssueSlip(ObjIssueSlip, IssueSlipId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForIssueSlip(IssueSlip ObjIssueSlip, int IssueSlipId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", BOEDate = "", BOENo = "", Vessel = "", CHA = "", Importer = "", ShippingLine = "",
            CargoDescription = "", MarksNo = "", Weight = "", LineNo = "", Rotation = "", ArrivalDate = "", DestuffingDate = "", Location = "";
            int Count = 0;
           // decimal MarksNo = 0, Weight=0;
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
            ObjIssueSlip.LstIssueSlipRpt.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                Size = (Size == "" ? (item.Size) : (Size + "<br/>" + item.Size));
               // BOEDate = (BOEDate == "" ? (item.BOEDate) : (BOEDate + "<br/>" + item.BOEDate));
              //  BOENo = (BOENo == "" ? (item.BOENo) : (BOENo + "<br/>" + item.BOENo));
              //  Vessel = (Vessel == "" ? (item.Vessel) : (Vessel + "<br/>" + item.Vessel));
               // CHA = (CHA == "" ? (item.CHA) : (CHA + "<br/>" + item.CHA));
               // Importer = (Importer == "" ? (item.Importer) : (Importer + "<br/>" + item.Importer));
               // CargoDescription = (CargoDescription == "" ? (item.CargoDescription) : (CargoDescription + "<br/>" + item.CargoDescription));
                MarksNo = (MarksNo == "" ? (item.MarksNo) : (MarksNo + "<br/>" + item.MarksNo));
               // Weight = (Weight == "" ? (item.Weight) : (Weight + "<br/>" + item.Weight));
              //  LineNo = (LineNo == "" ? (item.LineNo) : (LineNo + "<br/>" + item.LineNo));
              //  Rotation = (Rotation == "" ? (item.Rotation) : (Rotation + "<br/>" + item.Rotation));
               // ArrivalDate = (ArrivalDate == "" ? (item.ArrivalDate) : (ArrivalDate + "<br/>" + item.ArrivalDate));
               // DestuffingDate = (DestuffingDate == "" ? (item.DestuffingDate) : (DestuffingDate + "<br/>" + item.DestuffingDate));
               // Location = (Location == "" ? (item.Location) : (Location + "<br/>" + item.Location));
               // ShippingLine = (ShippingLine == "" ? (item.ShippingLine) : (ShippingLine + "<br/>" + item.ShippingLine));
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
            {
                if (CHA == "")
                    CHA = item.CHA;
                else
                    CHA += "<br/>" + item.CHA;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Weight = x.Weight }).Distinct().ToList().ForEach(item =>
            {
                if (Weight == "" || Weight== "0.0000")
                    Weight = item.Weight;
                else
                    Weight += "<br/>" + item.Weight;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { LineNo = x.LineNo }).Distinct().ToList().ForEach(item =>
            {
                if (LineNo == "")
                    LineNo = item.LineNo;
                else
                    LineNo += "<br/>" + item.LineNo;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Rotation = x.Rotation }).Distinct().ToList().ForEach(item =>
            {
                if (Rotation == "")
                    Rotation = item.Rotation;
                else
                    Rotation += "<br/>" + item.Rotation;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Location = x.Location }).Distinct().ToList().ForEach(item =>
            {
                if (Location == "")
                    Location = item.Location;
                else
                    Location += "<br/>" + item.Location;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item =>
            {
                if (Importer == "")
                    Importer = item.Importer;
                else
                    Importer += "<br/>" + item.Importer;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "<br/>" + item.CargoDescription;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ArrivalDate = x.ArrivalDate }).Distinct().ToList().ForEach(item =>
            {
                if (ArrivalDate == "")
                    ArrivalDate = item.ArrivalDate;
                else
                    ArrivalDate += "<br/>" + item.ArrivalDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "<br/>" + item.DestuffingDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLine == "")
                    ShippingLine = item.ShippingLine;
                else
                    ShippingLine += "<br/>" + item.ShippingLine;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "<br/>" + item.Vessel;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOEDate = x.BOEDate }).Distinct().ToList().ForEach(item =>
            {
                if (BOEDate == "")
                    BOEDate = item.BOEDate;
                else
                    BOEDate += "<br/>" + item.BOEDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOENo = x.BOENo }).Distinct().ToList().ForEach(item =>
            {
                if (BOENo == "")
                    BOENo = item.BOENo;
                else
                    BOENo += "<br/>" + item.BOENo;
            });
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            int BranchId = Convert.ToInt32(Session["BranchId"]); ;

            if (BranchId == 4)
            {
                Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='6' style='text-align:center;font-weight:600;'>CENTRAL WAREHOUSING CORPORATION<br/>(A GOVT. OF INDIA UNDERTAKING)<br/><br/>Issue Slip of Container Freight Station, Hyderabad.</th></tr></thead><tbody style='border:1px solid #000;'><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>C.H.A Name.</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + Weight + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";
            }
            else
            {
                Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='6' style='text-align:center;font-weight:600;'>CENTRAL WAREHOUSING CORPORATION<br/>(A GOVT. OF INDIA UNDERTAKING)<br/><br/>Issue Slip of Container Freight Station, Kolkata.</th></tr></thead><tbody style='border:1px solid #000;'><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>C.H.A Name.</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + Weight + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr><tr><td colspan='6' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            }
            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'><span style='border-bottom:1px solid #000;'></span></td><td style='text-align:right;'><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>Issue Slip Of Container Freight Station.</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/></td></tr><tr><td colspan='2' style='text-align:left;'><span style='border-bottom:1px solid #000;'></span><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry No.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + BOENo + "</td><td style='border:1px solid #000;padding:5px;'>" + BOEDate + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td><span></span></td><td><br/><br/></td></tr></tbody></table></td></tr></tbody></table>";


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
        }

        #endregion

        #region PRINT

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF(FormCollection fc)
        {
            try
            {
                var pages = new string[2];
                var type = fc["type"].ToString();
                var id = fc["id"].ToString();
                pages[0] = fc["page"].ToString();
                pages[1] = fc["npage"].ToString();
                var ImgLeft = Server.MapPath("~/Content/Images/CWCPDF.PNG");
                var ImgRight = Server.MapPath("~/Content/Images/SwachhBharat-Logo.png");
                pages[0] = fc["page"].ToString().Replace("IMGLeft", ImgLeft).Replace("IMGRight", ImgRight);
                if (id == "")
                {
                    //string ad = "AB1CE12354F";
                    //Random rnd = new Random();
                    //id = new string(Enumerable.Repeat(ad, ad.Length).Select(s => s[rnd.Next(ad.Length)]).ToArray());
                    id = ((Login)(Session["LoginUser"])).Uid.ToString() + "" + DateTime.Now.ToString().Replace('/', '_').Replace(' ', '_').Replace(':', '_');
                }
                id = id.Replace('/', '_');
                var fileName = id + ".pdf";
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);// Data = fileName 
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

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

        #region Internal Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            ImportRepository ObjIR = new ImportRepository();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjIR.GetBOENoForInternalMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.BOENoList = new SelectList((List<InternalMovement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
            }
            else
            {
                ViewBag.BOENoList = null;
            }
            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetInternalMovementList()
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetAllInternalMovement();
            List<InternalMovement> LstMovement = new List<InternalMovement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<InternalMovement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            ImportRepository ObjIR = new ImportRepository();
            InternalMovement ObjInternalMovement = new InternalMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (InternalMovement)ObjIR.DBResponse.Data;
                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovement(int MovementId)
        {
            ImportRepository ObjIR = new ImportRepository();
            InternalMovement ObjInternalMovement = new InternalMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (InternalMovement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetBOENoDetails(int DestuffingEntryDtlId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.GetBOENoDetForMovement(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovement(InternalMovement ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.AddEditImpInternalMovement(ObjInternalMovement);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
       
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DelInternalMovement(int MovementId)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }

        #endregion


        #region Custom Appraisement Removal
        [HttpGet]
        public ActionResult ListOfApprsmntApprRemoval()
        {
            ImportRepository objIR = new ImportRepository();
            objIR.ApprovalHoldCustomAppraisementForRemoval(0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfApp = Jobject["lstApproval"];

                // ViewBag.ListOfApp =objIR.DBResponse.Data;
                // var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.ListOfApp = jobject["Details"];
            }
            else
            {
                ViewBag.ListOfApp = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult LoadMoredataRemoval(int Skip, string status)
        {
            ImportRepository objIR = new ImportRepository();
            if (status == "N")
            {
                objIR.NewCustomeAppraisement(Skip);
            }
            else
            {
                objIR.ApprovalHoldCustomAppraisementForRemoval(Skip);
            }
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadApprovalPageRemoval(int CstmAppraiseWorkOrderId)
        {
            ImportRepository ObjIR = new ImportRepository();
            CstmAppraiseWorkOrder ObjWorkOrder = new CstmAppraiseWorkOrder();
            ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjWorkOrder = (CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraisementApprovalRemoval", ObjWorkOrder);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApprovalRemoval(int CstmAppraiseWorkOrderId, int IsApproved)
        {
            ImportRepository ObjIR = new ImportRepository();
            ObjIR.RemoveCustomApproval(CstmAppraiseWorkOrderId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

    }
}