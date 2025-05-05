using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Controllers
{
    public class DestructionController : BaseController
    {
        // GET: Auction/Destruction

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public DestructionController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;


        }



        public ActionResult Destruction()
        {
            return PartialView();
        }

        public JsonResult GetRefNoForDestruction(int Flag)
        {
            AuctionRepository obj = new AuctionRepository();
            obj.GetRefNoForDestruction(Flag);
            List<DestructionViewModel> LstRefDetails = new List<DestructionViewModel>();
            LstRefDetails = (List<DestructionViewModel>)obj.DBResponse.Data;

            return Json(LstRefDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLstGodown()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            AuctionRepository obj = new AuctionRepository();
            obj.GetGodownRightsWise(ObjLogin.Uid);
            List<GodownList> LstGodownDetails = new List<GodownList>();
            LstGodownDetails = (List<GodownList>)obj.DBResponse.Data;

            return Json(LstGodownDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            AuctionRepository objER = new AuctionRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.GodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.GodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveDestruction(DestructionViewModel vm)
        {
            try
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                AuctionRepository obj = new AuctionRepository();
                if (ModelState.IsValid)
                {

                    if(vm.Type=="OBL")
                    {
                        vm.RefFlag = "1";
                    }
                    else if(vm.Type=="ShipBill")
                    {
                        vm.RefFlag = "2";
                    }
                    else
                    {
                        vm.RefFlag = "3";
                    }
                    obj.SaveDestruction(vm, ObjLogin.Uid);


                    return Json(obj.DBResponse,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ObjResult = new { Status = 0, Message = "Validation Error" };
                    return Json(ObjResult,JsonRequestBehavior.AllowGet);
                }

               
            }
            catch(Exception ex)
            {
                var ObjResult = new { Status = 0, Message = ex.Message };
                return Json(ObjResult,JsonRequestBehavior.AllowGet);
            }


           
        }



        [CustomValidateAntiForgeryToken]
        [HttpPost]       
        public JsonResult DeleteDestruction(int DestructionID)
        {
            try
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                AuctionRepository obj = new AuctionRepository();               
                obj.DeleteDestruction(DestructionID);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);   
            }
            catch (Exception ex)
            {
                var ObjResult = new { Status = 0, Message = ex.Message };
                return Json(ObjResult, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDestructionList(int id)
        {
            try
            {
                AuctionRepository obj = new AuctionRepository();
                obj.GetDestructionDetails(id);
                List<DestructionViewModel> lstDestruction = new List<DestructionViewModel>();
                lstDestruction = (List<DestructionViewModel>)obj.DBResponse.Data;


                return Json(lstDestruction,JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                var ObjResult = new { Status = 0, Message = ex.Message };
                return Json(ObjResult, JsonRequestBehavior.DenyGet);
               
            }
            
        }

        public ActionResult GetAllDestruction()
        {
            List<DestructionViewModel> lstDestruction = new List<DestructionViewModel>();
            try
            {
                AuctionRepository obj = new AuctionRepository();
                obj.GetDestructionDetails(0);
               
                lstDestruction = (List<DestructionViewModel>)obj.DBResponse.Data;


                
            }
            catch (Exception ex)
            {
                var ObjResult = new { Status = 0, Message = ex.Message };
                

            }
            return PartialView( "GetAllDestruction", lstDestruction);
        }


        public JsonResult GetPreviewDestruction(int id)
        {
            AuctionRepository obj = new AuctionRepository();
            obj.GetDestructionGatePassDetails(id);
            List<DestructionGatepassViewModel> lstDestruction = new List<DestructionGatepassViewModel>();
            lstDestruction = (List<DestructionGatepassViewModel>)obj.DBResponse.Data;

            string FilePath = "";
            if (lstDestruction != null)
            {
               
                FilePath = GeneratingPDF(lstDestruction);
                return Json(new { Status = 1, Message = FilePath },JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        private string GeneratingPDF(List<DestructionGatepassViewModel> objGP)
        {
            string html = "";

            string FileName = "Destruction.pdf";

            string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Auction/Destruction/";
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);




            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/ICD/19</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'></label><label style='font-size: 14px; font-weight:bold;'>Cargo Destruction Gatepass</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP[0].GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP[0].GatePassDate + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + objGP[0].VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + objGP[0].ContainerNoAndSize + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP[0].ImporterExporter + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Destruction Agency Name : <span style='font-weight:normal;'>" + objGP[0].DestructionAgencyName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP[0].ShippingLine + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL No./Shipping Bill No. : <span style='font-weight:normal;'>" + objGP[0].OBLNoShippbillNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP[0].DestructionDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + objGP[0].NoOfPkg + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight. : <span style='font-weight:normal;'>" + objGP[0].Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + objGP[0].LocationName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>ICD-Code : <span style='font-weight:normal;'>" + objGP[0].ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + objGP[0].CargoDesc + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gatepass Time : <span style='font-weight:normal;'>" + objGP[0].GatepassTime + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gatepass Expiry Date & Time : <span style='font-weight:normal;'>" + objGP[0].GatepassExpiryDateAndTime + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='12' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Remarks : <span style='font-weight:normal;'>" + objGP[0].Remarks + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of Representative <br/>of destruction agency</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Auction Incharge</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Shed Incharge</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";          
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(LocalDirectory+ FileName, html);
            }
            return "/Docs/" + Session.SessionID + "/Auction/Destruction/" + FileName;
            //return "";
        }

    }
}