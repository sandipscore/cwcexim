using CwcExim.Areas.Auction.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Controllers
{
    public class WFLD_AuctionNoticeController : BaseController
    {
        // GET: Auction/WFLD_AuctionNotice

        #region Auction Notice

       
        public ActionResult AuctionNotice()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult PopulateDataAuctionNotice(string operationType, string auctionType, int inPage)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            ObjAR.GetCargoDetailsList(operationType, auctionType, ObjLogin.Uid, inPage);

            return Json(ObjAR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult PopulateDataAuctionNoticeByOBLNoShipBill(string operationType, string auctionType, string OBLNo, string ShipBill)
        {
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjAR.GetCargoDetailsListByOBLNoShipBill(operationType, auctionType, ObjLogin.Uid, OBLNo, ShipBill);

            return Json(ObjAR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAuctionNoticeDataByOBLNoShipBill(string operationType, string auctionType, string AuctionNo)
        {
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjAR.GetAuctionNoticeOBLNoShipBill(operationType, auctionType, AuctionNo, ObjLogin.Uid);
            List<WFLD_SavedAuctionNotice> SavedAuctionNoticeList = new List<WFLD_SavedAuctionNotice>();

            List<WFLD_AuctionNoticeItemDetails> LstAuctionNoticeItemDetails = new List<WFLD_AuctionNoticeItemDetails>();
            if (ObjAR.DBResponse.Data != null)
            {
                LstAuctionNoticeItemDetails = (List<WFLD_AuctionNoticeItemDetails>)ObjAR.DBResponse.Data;

                foreach (var item in LstAuctionNoticeItemDetails)
                {
                    var ifExistSavedAuctionNotice = SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).FirstOrDefault();

                    if (ifExistSavedAuctionNotice == null)
                    {
                        WFLD_SavedAuctionNotice savedAuctionNotice = new WFLD_SavedAuctionNotice();
                        savedAuctionNotice.SavedAuctionNoticeDetailsList = new List<WFLD_SavedAuctionNoticeDetails>();
                        savedAuctionNotice.AuctionNoticeDtlId = item.AuctionNoticeDtlId;
                        savedAuctionNotice.NoticeNo = item.NoticeNo;
                        savedAuctionNotice.AuctionNoticeDate = item.AuctionNoticeDate;
                        savedAuctionNotice.PartyName = item.PartyName;
                        savedAuctionNotice.SecondNotice = item.SecondNotice;
                        savedAuctionNotice.ThirdNotice = item.ThirdNotice;
                        savedAuctionNotice.NoOfNotice = item.NoOfNotice;

                        WFLD_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new WFLD_SavedAuctionNoticeDetails();

                        savedAuctionNoticeDetails.BOENo = item.BOENo;
                        savedAuctionNoticeDetails.CommodityName = item.CommodityName;
                        savedAuctionNoticeDetails.BOLDate = item.BLDate;
                        savedAuctionNoticeDetails.ShipBillNo = item.ShippingBillNo;
                        savedAuctionNoticeDetails.ShipBillDate = item.ShipBillDate;
                        savedAuctionNotice.SavedAuctionNoticeDetailsList.Add(savedAuctionNoticeDetails);

                        SavedAuctionNoticeList.Add(savedAuctionNotice);
                    }
                    else
                    {
                        WFLD_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new WFLD_SavedAuctionNoticeDetails();
                        savedAuctionNoticeDetails.BOENo = item.BOENo;
                        savedAuctionNoticeDetails.CommodityName = item.CommodityName;
                        savedAuctionNoticeDetails.BOLDate = item.BLDate;
                        savedAuctionNoticeDetails.ShipBillNo = item.ShippingBillNo;
                        savedAuctionNoticeDetails.ShipBillDate = item.ShipBillDate;

                        foreach (var itemSavedAuctionNoticeList in SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).ToList())
                        {
                            itemSavedAuctionNoticeList.SavedAuctionNoticeDetailsList.Add(savedAuctionNoticeDetails);
                        }

                    }


                }
            }
            ViewBag.OpType = operationType;
            ViewBag.AuctionType = auctionType;
            return View("AuctionNoticeList", SavedAuctionNoticeList);
        }



        [HttpGet]
        public ActionResult GetAuctionNoticeList(string operationType, string auctionType)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            ObjAR.GetAllAuctionNotice(operationType, auctionType, ObjLogin.Uid);

            List<WFLD_SavedAuctionNotice> SavedAuctionNoticeList = new List<WFLD_SavedAuctionNotice>();

            List<WFLD_AuctionNoticeItemDetails> LstAuctionNoticeItemDetails = new List<WFLD_AuctionNoticeItemDetails>();
            if (ObjAR.DBResponse.Data != null)
            {
                LstAuctionNoticeItemDetails = (List<WFLD_AuctionNoticeItemDetails>)ObjAR.DBResponse.Data;

                foreach (var item in LstAuctionNoticeItemDetails)
                {
                    var ifExistSavedAuctionNotice = SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).FirstOrDefault();

                    if (ifExistSavedAuctionNotice == null)
                    {
                        WFLD_SavedAuctionNotice savedAuctionNotice = new WFLD_SavedAuctionNotice();
                        savedAuctionNotice.SavedAuctionNoticeDetailsList = new List<WFLD_SavedAuctionNoticeDetails>();
                        savedAuctionNotice.AuctionNoticeDtlId = item.AuctionNoticeDtlId;
                        savedAuctionNotice.NoticeNo = item.NoticeNo;
                        savedAuctionNotice.AuctionNoticeDate = item.AuctionNoticeDate;
                        savedAuctionNotice.PartyName = item.PartyName;
                        savedAuctionNotice.SecondNotice = item.SecondNotice;
                        savedAuctionNotice.ThirdNotice = item.ThirdNotice;
                        savedAuctionNotice.NoOfNotice= item.NoOfNotice;

                        WFLD_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new WFLD_SavedAuctionNoticeDetails();

                        savedAuctionNoticeDetails.BOENo = item.BOENo;
                        savedAuctionNoticeDetails.CommodityName = item.CommodityName;
                        savedAuctionNoticeDetails.BOLDate = item.BLDate;
                        savedAuctionNoticeDetails.ShipBillNo = item.ShippingBillNo;
                        savedAuctionNoticeDetails.ShipBillDate = item.ShipBillDate;
                        savedAuctionNoticeDetails.ContainerNo = item.ContainerNo;
                        savedAuctionNotice.SavedAuctionNoticeDetailsList.Add(savedAuctionNoticeDetails);

                        SavedAuctionNoticeList.Add(savedAuctionNotice);
                    }
                    else
                    {
                        WFLD_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new WFLD_SavedAuctionNoticeDetails();
                        savedAuctionNoticeDetails.BOENo = item.BOENo;
                        savedAuctionNoticeDetails.CommodityName = item.CommodityName;
                        savedAuctionNoticeDetails.BOLDate = item.BLDate;
                        savedAuctionNoticeDetails.ShipBillNo = item.ShippingBillNo;
                        savedAuctionNoticeDetails.ShipBillDate = item.ShipBillDate;
                        savedAuctionNoticeDetails.ContainerNo = item.ContainerNo;

                        foreach (var itemSavedAuctionNoticeList in SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).ToList())
                        {
                            itemSavedAuctionNoticeList.SavedAuctionNoticeDetailsList.Add(savedAuctionNoticeDetails);
                        }

                    }


                }
            }
            ViewBag.OpType = operationType;
            ViewBag.AuctionType = auctionType;
            return View("AuctionNoticeList", SavedAuctionNoticeList);
        }

        [HttpPost]
        public JsonResult SaveAuctionIssueNotice(List<WFLD_AuctionNoticeDetails> SelectedAuctionNoticeDetailsList, string operationType, string auctionType, string AuctionDate)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            List<WFLD_AuctionNoticeItemDetails> AuctionNoticeItemDetailsList = new List<WFLD_AuctionNoticeItemDetails>();
            DateTime dt = DateTime.ParseExact(AuctionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string convertAuctionDate = dt.ToString("yyyy-MM-dd");

            foreach (var item in SelectedAuctionNoticeDetailsList)
            {
                WFLD_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new WFLD_AuctionNoticeItemDetails();
                objAuctionNoticeItemDetails.AuctionEligibleDate = item.AuctionEligibleDate;
                objAuctionNoticeItemDetails.CommodityId = item.CommodityId;
                objAuctionNoticeItemDetails.EntryDate = item.EntryDate;
                objAuctionNoticeItemDetails.NoOfPackages = item.NoOfPackages;

                objAuctionNoticeItemDetails.GodownLocation = String.IsNullOrEmpty(item.GodownLocation) ? "###" : item.GodownLocation;
                objAuctionNoticeItemDetails.BLNo = String.IsNullOrEmpty(item.BLNo) ? "###" : item.BLNo;
                // objAuctionNoticeItemDetails.Date= String.IsNullOrEmpty(item.BLNo) ? "###" : item.Date;
                objAuctionNoticeItemDetails.Weight = String.IsNullOrEmpty(item.Weight) ? "###" : item.Weight;
                objAuctionNoticeItemDetails.CUM = String.IsNullOrEmpty(item.CUM) ? "###" : item.CUM;
                objAuctionNoticeItemDetails.SQM = String.IsNullOrEmpty(item.SQM) ? "###" : item.SQM;
                objAuctionNoticeItemDetails.Duty = String.IsNullOrEmpty(item.Duty) ? "###" : item.Duty;
                objAuctionNoticeItemDetails.Fob = String.IsNullOrEmpty(item.Fob) ? "###" : item.Fob;
                objAuctionNoticeItemDetails.CIF = String.IsNullOrEmpty(item.CIF) ? "###" : item.CIF;
                objAuctionNoticeItemDetails.IsInsured = String.IsNullOrEmpty(item.IsInsured) ? "###" : item.IsInsured;
                objAuctionNoticeItemDetails.BOENo = String.IsNullOrEmpty(item.BOENo) ? "###" : item.BOENo;
                objAuctionNoticeItemDetails.LineNo = String.IsNullOrEmpty(item.LineNo) ? "###" : item.LineNo;
                objAuctionNoticeItemDetails.ShippingBillNo = String.IsNullOrEmpty(item.ShippingBillNo) ? "###" : item.ShippingBillNo;
                objAuctionNoticeItemDetails.ContainerNo = String.IsNullOrEmpty(item.ContainerNo) ? "###" : item.ContainerNo;
                objAuctionNoticeItemDetails.CFSCode = String.IsNullOrEmpty(item.CFSCode) ? "###" : item.CFSCode;
                objAuctionNoticeItemDetails.Size = String.IsNullOrEmpty(item.Size) ? "###" : item.Size;
                objAuctionNoticeItemDetails.BLDate = String.IsNullOrEmpty(item.BLDate) ? "###" : item.BLDate;
                objAuctionNoticeItemDetails.ShipBillDate = String.IsNullOrEmpty(item.ShipBillDate) ? "###" : item.ShipBillDate;
                objAuctionNoticeItemDetails.CuttingDate = String.IsNullOrEmpty(item.CuttingDate) ? "###" : item.CuttingDate;
                objAuctionNoticeItemDetails.GodownID = item.GodownID;
                objAuctionNoticeItemDetails.RefId = item.RefId;
                objAuctionNoticeItemDetails.AuctionNoticeDate = convertAuctionDate;
                objAuctionNoticeItemDetails.IGM= String.IsNullOrEmpty(item.IGM) ? "###" : item.IGM;
                objAuctionNoticeItemDetails.TSA = String.IsNullOrEmpty(item.TSANo) ? "###" : item.IGM;
                objAuctionNoticeItemDetails.ShippingLineNo = String.IsNullOrEmpty(item.ShippingLineNo) ? "###" : item.ShippingLineNo;



                AuctionNoticeItemDetailsList.Add(objAuctionNoticeItemDetails);
            }
            string xmlAuctionNotice = Utility.CreateXML(AuctionNoticeItemDetailsList);

            xmlAuctionNotice = xmlAuctionNotice.Replace(">###<", "><");

            ObjAR.SaveAuctionIssueNotice(operationType, auctionType, SelectedAuctionNoticeDetailsList[0].PartyId, Convert.ToInt32(Session["BranchId"]), ObjLogin.Uid, xmlAuctionNotice);
            return Json(ObjAR.DBResponse);
        }

        public JsonResult PrintAuctionIssueNotice(int AuctionNoticeDtlId)
        {
            try
            {
                var Pages = new string[1];
                string FileName = "AuctionNotice.pdf";

                WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
                WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = ObjAR.GetAuctionNoticeDataToPrint(AuctionNoticeDtlId);

                if (auctionNoticePrintViewModel != null)
                {
                    List<string> Tabs = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    int srNo = 0;

                    if (auctionNoticePrintViewModel.OperationType == "Import" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");                        
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'>NOTICE UNDER SECTION 48 OF THE CUSTOM ACT- 1962</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600; width:80%; padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600; width:20%; padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>Sub: Notice for sale of uncleared goods under section 48 of the Custom Act 1962 - REG...<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-size: 9pt;margin-top: 4px;'>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared "+ auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>PARTICULARS OF GOODS</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Container No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: left;padding:5px;'>OBL Number</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.BLNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.NoOfPackages + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Weight + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.IGMNO+"/"+item.ItemNo + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span style='border-bottom: 1px solid #000;'>DESCRIPTION OF GOODS:</span> <span>" + auctionNoticePrintViewModel.CommodityName + "</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:</td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 1. The Asst. Commissioner of Custom(Import) Inland Container Depot. Bangalore</span></td></tr>");
                        sb.Append("<tr><td colspan='12'><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 2. The Regional Manager,CWC,Regional Office, Bangalore</span></td></tr>");
                        sb.Append("<tr><td colspan='12'><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 3. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        sb.Append("<tr><td colspan='12'><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");

                        
                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'>NOTICE UNDER SECTION 48 OF THE CUSTOM ACT- 1962</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>Sub: Notice for sale of uncleared goods under section 48 of the Custom Act 1962 - REG...<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>PARTICULARS OF GOODS</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Container No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: left;padding:5px;'>SB No.</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ShipBillNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.NoOfPackages + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Weight + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.IGMNO + "/" + item.ItemNo + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span style='border-bottom: 1px solid #000;'>DESCRIPTION OF GOODS:</span> <span>" + auctionNoticePrintViewModel.CommodityName + "</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 9pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>1. The Asst. Commissioner of Custom(Import) Inland Container Depot. Bangalore</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>2. The Regional Manager,CWC,Regional Office, Bangalore</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>3. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CONTAINER")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>F/CD/ICD-PPG/24</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'>SHED: <span>" + auctionNoticePrintViewModel.GodownLocation + "</span></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;width:70%;padding: 5px;'>NO." + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'>" + auctionNoticePrintViewModel.CompanyAddress + "</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'>NOTICE UNDER SECTION 48 OF THE CUSTOM ACT-1962</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td style='font-weight: 600;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>Sub: Notice for sale of uncleared goods under section 48 of the Custom Act 1962.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods meant for export are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>PARTICULARS OF GOODS</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Container No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.NoOfPackages + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Weight + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ItemNo + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span style='border-bottom: 1px solid #000;'>DESCRIPTION OF GOODS:</span> <span>" + auctionNoticePrintViewModel.CommodityName + "</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        // sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>The Asst. Commissioner of Custom(Export) Inland Container Depot, Patparganj, Delhi-110096.</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager (ICD)</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1. The Asst. Commissioner of Custom(Import) Inland Container Depot, Patparganj Delhi-110096</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.Shippingline + "</span>");
                        // sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        Pages[0] = sb.ToString();
                    }
                    string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Auction/AuctionNotice/";
                    if (!Directory.Exists(LocalDirectory))
                        Directory.CreateDirectory(LocalDirectory);
                    using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                    {
                        ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);
                    }
                    var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Auction/AuctionNotice/" + FileName };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ObjResult = new { Status = 0, Message = "No Data Available." };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult PrintAuctionIssueSecondNotice(int AuctionNoticeDtlId)
        {
            try
            {
                var Pages = new string[1];
                string FileName = "AuctionSecondNotice.pdf";

                WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
                WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = ObjAR.GetAuctionSecondNoticeDataToPrint(AuctionNoticeDtlId);

                if (auctionNoticePrintViewModel != null)
                {
                    List<string> Tabs = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    int srNo = 0;

                    if (auctionNoticePrintViewModel.OperationType == "Import" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'></span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;padding: 9px;width:80%;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;padding: 5px;text-align: center;width:20%;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>NOTICE -II<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>SUB:AUCTION OF CARGO .....REG.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>1. Whereas the cargo/container as per details below is in storage with CWC.</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Via No.</th><th style='text-align: left;padding:5px;'>OBL No</th><th style='text-align: left;padding:5px;'>TSA/Line No</th><th style='text-align: left;padding:5px;'>Container No</th><th style='text-align: right;padding:5px;'>Size</th><th style='text-align: right;padding:5px;'>Arrival Dt.CFS</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'></td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.BLNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.TSA +"/" + item .LineNo+ "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Size + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.EntryDate + "</td>");                          
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td><span><br/></span></td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='font-size: 9pt;'><b>DESCRIPTION OF GOODS :</b> " + auctionNoticePrintViewModel.CommodityName + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>No Pkg :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].NoOfPackages + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>GR WT :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].Weight + "</td>");                        
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 200;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span>2. whereas the consignee has failed to clear the cargo within the stipulated period as envisaged by the provisions of the customs Act 1962");
                        sb.Append(" read with the different subsequent amendments. CWC CFS WHITEFIELD tenders the  'SECOND NOTICE' to the importer on the address as manifested in the relevent IGM advicing to");
                        sb.Append("clear their cargo within fifteen days of the issue of this letter failing which their cargo would be put in to auction/tender without any subsequent intimation. The sale proceeds will be appropriated for metting out the expenditure incurred in auction,payment of customs duty and the realisation of the accrued warehousing charges Please note that in case the sales proceeds is not good enough to realize the CFS charges after approating auction expenditure and custom duty the sortfall if any will be recoverable from the concern importer as per law in force and keeping in view the various judgements pronounced by the various courts of law. <br /> This may be treated as 'SECOND NOTICE'");
                       
                        sb.Append("</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 9pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>1. The " + auctionNoticePrintViewModel.Shippingline + " for necessary action please.</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>2. The Asst. Commissioner of Custom(Disposal),Whitefield Bangalore for kind information and nec</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>3. The Regional Manager, CWC,R.O.,Bangalore for kind information</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'></span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;padding: 9px;width:80%;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;padding: 5px;text-align: center;width:20%;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>NOTICE -II<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>SUB:AUCTION OF CARGO .....REG.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>1. Whereas the cargo/container as per details below is in storage with CWC.</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Via No.</th><th style='text-align: left;padding:5px;'>SB No.</th><th style='text-align: left;padding:5px;'>TSA/Line No</th><th style='text-align: left;padding:5px;'>Container No</th><th style='text-align: right;padding:5px;'>Size</th><th style='text-align: right;padding:5px;'>Arrival Dt.CFS</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'></td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ShipBillNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.TSA + "/" + item.LineNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Size + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td><span><br/></span></td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='font-size: 9pt;'><b>DESCRIPTION OF GOODS :</b> " + auctionNoticePrintViewModel.CommodityName + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>No Pkg :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].NoOfPackages + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>GR WT :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].Weight + "</td>");
                        sb.Append("</tr>");
                        
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 200;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span>2. whereas the consignee has failed to clear the cargo within the stipulated period as envisaged by the provisions of the customs Act 1962");
                        sb.Append(" read with the different subsequent amendments. CWC CFS WHITEFIELD tenders the  'SECOND NOTICE' to the importer on the address as manifested in the relevent IGM advicing to");
                        sb.Append("clear their cargo within fifteen days of the issue of this letter failing which their cargo would be put in to auction/tender without any subsequent intimation. The sale proceeds will be appropriated for metting out the expenditure incurred in auction,payment of customs duty and the realisation of the accrued warehousing charges Please note that in case the sales proceeds is not good enough to realize the CFS charges after approating auction expenditure and custom duty the sortfall if any will be recoverable from the concern importer as per law in force and keeping in view the various judgements pronounced by the various courts of law. <br /> This may be treated as 'SECOND NOTICE'");

                        sb.Append("</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 9pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>1. The " + auctionNoticePrintViewModel.Shippingline + " for necessary action please.</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");                        
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>2. The Asst. Commissioner of Custom(Disposal),Whitefield Bangalore for kind information and nec</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>3. The Regional Manager, CWC,R.O.,Bangalore for kind information</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'>4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CONTAINER")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>F/CD/ICD-PPG/24</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'>SHED: <span>" + auctionNoticePrintViewModel.GodownLocation + "</span></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;width:70%;padding: 5px;'>NO." + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'>" + auctionNoticePrintViewModel.CompanyAddress + "</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'>NOTICE UNDER SECTION 48 OF THE CUSTOM ACT-1962</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td style='font-weight: 600;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>Sub: Notice for sale of uncleared goods under section 48 of the Custom Act 1962.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods meant for export are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>PARTICULARS OF GOODS</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Container No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.NoOfPackages + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Weight + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ItemNo + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span style='border-bottom: 1px solid #000;'>DESCRIPTION OF GOODS:</span> <span>" + auctionNoticePrintViewModel.CommodityName + "</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        // sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>The Asst. Commissioner of Custom(Export) Inland Container Depot, Patparganj, Delhi-110096.</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager (ICD)</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1. The Asst. Commissioner of Custom(Import) Inland Container Depot, Patparganj Delhi-110096</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.Shippingline + "</span>");
                        // sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        Pages[0] = sb.ToString();
                    }
                    string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Auction/AuctionNotice/";
                    if (!Directory.Exists(LocalDirectory))
                        Directory.CreateDirectory(LocalDirectory);
                    using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                    {
                        ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);
                    }
                    var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Auction/AuctionNotice/" + FileName };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ObjResult = new { Status = 0, Message = "No Data Available." };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult PrintAuctionIssueThirdNotice(int AuctionNoticeDtlId)
        {
            try
            {
                var Pages = new string[1];
                string FileName = "AuctionThirdNotice.pdf";

                WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
                WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = ObjAR.GetAuctionThirdNoticeDataToPrint(AuctionNoticeDtlId);

                if (auctionNoticePrintViewModel != null)
                {
                    List<string> Tabs = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    int srNo = 0;

                    if (auctionNoticePrintViewModel.OperationType == "Import" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'></span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>NOTICE -III<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>SUB:AUCTION OF CARGO .....REG.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>1. Whereas the cargo/container as per details below is in storage with CWC.</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Via No.</th><th style='text-align: left;padding:5px;'>OBL No</th><th style='text-align: left;padding:5px;'>IGM/Item No</th><th style='text-align: left;padding:5px;'>Container No</th><th style='text-align: right;padding:5px;'>Size</th><th style='text-align: right;padding:5px;'>Arrival Dt.CFS</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'></td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.BLNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.IGMNO + "/" + item.ItemNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Size + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td><span><br/></span></td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='font-size: 9pt;'><b>DESCRIPTION OF GOODS :</b> " + auctionNoticePrintViewModel.CommodityName + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>No Pkg :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].NoOfPackages + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>GR WT :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].Weight + "</td>");
                        sb.Append("</tr>");
                        
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 200;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span>2. Whereas the consignee has failed to clear the cargo within the stipulated period as envisaged by the provisions of the customs Act 1962");
                        sb.Append("read with the different subsequent amendments. CWC CFS WHITEFIELD tenders the  'THIRD NOTICE' to the importer on the address as manifested in the relevent IGM advicing to");
                        sb.Append("clear their cargo within fifteen days of the issue of this letter failing which their cargo would be put in to auction/tender without any subsequent intimation. the sale proceeds will be appropriated for the metting out the expenditure incurred in auction, payment of customs duty and the realisation of the accrued warehousing charges Please note that in case the sales proceeds is not good enough to realize the CFS charges after approating auction expenditure and custom duty the sortfall if any will be recoverable from the concern importer as per law in force and keeping in view the various judgements pronounced by the various courts of law. <br /> This may be treated as 'THIRD NOTICE' whereas the consignee have failed to clear the cargo within the stipulated period as envisaged by the provisions.");

                        sb.Append("</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: right;font-weight: 600;font-size: 9pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>1. The " + auctionNoticePrintViewModel.Shippingline + " for necessary action please.</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>2. The Asst. Commissioner of Custom(Disposal),Whitefield Bangalore for kind information and nec</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>3. The Regional Manager, CWC,R.O.,Bangalore for kind information</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");

                        sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='70'/></td>");
                        sb.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                        sb.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                        sb.Append("</td>");
                        //sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                        sb.Append("</tr>");


                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        //sb.Append("<th style='text-align: left;'></th>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        //sb.Append("<th></th>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'>" + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>CFS WHITE FIELD<br />BANGALORE - 560066</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;width:80%;padding: 5px;'></td>");
                        sb.Append("<td colspan='2' align='right' style='font-weight: 600;width:20%;text-align: center;padding: 5px;'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td style='border:1px solid #333;'>");
                        sb.Append("<div style='padding: 5px 0; font-size: 7pt; font-weight: 600; text-align: center;'>F/CD/CFS/31</div>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td width='50%'></td>");
                        sb.Append("<td>");
                        sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                        sb.Append("<tr><td>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");
                        sb.Append("</tbody></table>");
                        sb.Append("</td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'></span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='9' style='font-weight: 600;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td colspan='2' style='font-weight: 600;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>NOTICE -III<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>SUB:AUCTION OF CARGO .....REG.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].EntryDate + " for over 30 days from the date of landing at this ICD.< br /><br /><br />If the goods are not cleared on payment of charges due thereon within 10 days of the receipt of this notice, the goods will be sold through  public auction/tender as per  provisions of the Custom Act 1962 without any further notice to you.<br/><br/><br/>If the goods under reference have already been cleared, this notice may please by treated as cancelled.<br/><br/><br/>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>1. Whereas the cargo/container as per details below is in storage with CWC.</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12'>");
                        sb.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Via No.</th><th style='text-align: left;padding:5px;'>SB No</th><th style='text-align: left;padding:5px;'>IGM/Item No</th><th style='text-align: left;padding:5px;'>Container No</th><th style='text-align: right;padding:5px;'>Size</th><th style='text-align: right;padding:5px;'>Arrival Dt.CFS</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'></td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ShipBillNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.IGMNO + "/" + item.ItemNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Size + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr><td><span><br/></span></td></tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='6' style='font-size: 9pt;'><b>DESCRIPTION OF GOODS :</b> " + auctionNoticePrintViewModel.CommodityName + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>No Pkg :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].NoOfPackages + "</td>");
                        sb.Append("<td colspan='3' valign='top' style='font-size: 9pt;'><b>GR WT :</b> " + auctionNoticePrintViewModel.ParticularsOfGoodsList[0].Weight + "</td>");
                        sb.Append("</tr>");
                        
                        sb.Append("<tr>");
                        sb.Append("<td colspan='12' style='text-align: left;font-weight: 200;font-size: 9pt;'><br/><br/>"); sb.Append("<span>2. Whereas the consignee has failed to clear the cargo within the stipulated period as envisaged by the provisions of the customs Act 1962");
                        sb.Append("read with the different subsequent amendments. CWC CFS WHITEFIELD tenders the  'THIRD NOTICE' to the importer on the address as manifested in the relevent IGM advicing to");
                        sb.Append("clear their cargo within fifteen days of the issue of this letter failing which their cargo would be put in to auction/tender without any subsequent intimation. the sale proceeds will be appropriated for the metting out the expenditure incurred in auction, payment of customs duty and the realisation of the accrued warehousing charges Please note that in case the sales proceeds is not good enough to realize the CFS charges after approating auction expenditure and custom duty the sortfall if any will be recoverable from the concern importer as per law in force and keeping in view the various judgements pronounced by the various courts of law. <br /> This may be treated as 'THIRD NOTICE' whereas the consignee have failed to clear the cargo within the stipulated period as envisaged by the provisions.");

                        sb.Append("</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 9pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/>  Manager - CFS </td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");                        

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>1. The " + auctionNoticePrintViewModel.Shippingline + " for necessary action please.</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>2. The Asst. Commissioner of Custom(Disposal),Whitefield Bangalore for kind information and nec</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>3. The Regional Manager, CWC,R.O.,Bangalore for kind information</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>4. Office Copy</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        sb = sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CONTAINER")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>F/CD/ICD-PPG/24</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;'></th>");
                        sb.Append("<th style='font-size:9pt;text-align: left;border-bottom:1px solid #000;'>SHED: <span>" + auctionNoticePrintViewModel.GodownLocation + "</span></th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;width:70%;padding: 5px;'>NO." + auctionNoticePrintViewModel.AuctionNoticeDocNo + "</td>");
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'>" + auctionNoticePrintViewModel.CompanyAddress + "</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-size: 9pt;font-weight: 600;'><span style='border-bottom:1px solid #000;'>NOTICE UNDER SECTION 48 OF THE CUSTOM ACT-1962</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='font-weight: 600;padding: 9px;'>TO,<br/><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyName + "</span><br/><span style='font-size: 9pt;'>" + auctionNoticePrintViewModel.PartyAddress + "</span></td>");
                        sb.Append("<td style='font-weight: 600;padding: 5px;text-align: center;'>Date: <span>" + auctionNoticePrintViewModel.NoticeDate + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: center;font-weight: 600;font-size: 9pt;padding-top: 2px;'>Sub: Notice for sale of uncleared goods under section 48 of the Custom Act 1962.<br/></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td><span><br/><br/><br/></span></td></tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-size: 8pt;margin-top: 4px;'>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods meant for export are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;'>");
                        sb.Append("<span style='font-size: 9pt;border-bottom: 1px solid #000;font-weight: 600;'>PARTICULARS OF GOODS</span><br/><br/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Container No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ContainerNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.EntryDate + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.NoOfPackages + "</td>");
                            sb.Append("<td style='text-align: right;padding:5px;'>" + item.Weight + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ItemNo + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>");
                        sb.Append("<span style='border-bottom: 1px solid #000;'>DESCRIPTION OF GOODS:</span> <span>" + auctionNoticePrintViewModel.CommodityName + "</span>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        // sb.Append("<tr>");
                        // sb.Append("<td></td>");
                        // sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        //sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>The Asst. Commissioner of Custom(Export) Inland Container Depot, Patparganj, Delhi-110096.</span>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager (ICD)</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='12'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>1. The Asst. Commissioner of Custom(Import) Inland Container Depot, Patparganj Delhi-110096</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='12' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-size: 9pt;'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");
                        

                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.Shippingline + "</span>");
                        // sb.Append("</td>");
                        //sb.Append("</tr>");
                        //sb.Append("<tr>");
                        // sb.Append("<td colspan='2' style='text-align: left;font-weight: 600;font-size: 9pt;'><span style='font-weight: 400;font-weight: 400;font-size: 7pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        Pages[0] = sb.ToString();
                    }
                    string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Auction/AuctionNotice/";
                    if (!Directory.Exists(LocalDirectory))
                        Directory.CreateDirectory(LocalDirectory);
                    using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                    {
                        ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);
                    }
                    var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Auction/AuctionNotice/" + FileName };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ObjResult = new { Status = 0, Message = "No Data Available." };
                    return Json(ObjResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }
        }


        [HttpGet]
        public JsonResult PopulateDataAuctionNoticeByContainer(string operationType, string auctionType, string ContainerNo)
        {
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjAR.GetContainerDetailsListByContainerNo(operationType, auctionType, ContainerNo);

            return Json(ObjAR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Reissue Notice
        public JsonResult AuctionReissueNotice(int AuctionNoticeDtlId)
        {
            try
            {

                WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
                ObjAR.GetAuctionDetailsForReissue(AuctionNoticeDtlId);

                return Json(new { Status = "1", Message = ObjAR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveAuctionReissueNotice(List<WFLD_AuctionNoticeDetails> SelectedAuctionNoticeDetailsList, string operationType, string auctionType, string AuctionDate, int Auctionid)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            WFLD_AuctionRepository ObjAR = new WFLD_AuctionRepository();
            List<WFLD_AuctionNoticeItemDetails> AuctionNoticeItemDetailsList = new List<WFLD_AuctionNoticeItemDetails>();

            DateTime dt = DateTime.ParseExact(AuctionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string convertAuctionDate = dt.ToString("yyyy-MM-dd");

            foreach (var item in SelectedAuctionNoticeDetailsList)
            {



                WFLD_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new WFLD_AuctionNoticeItemDetails();
                objAuctionNoticeItemDetails.AuctionEligibleDate = item.AuctionEligibleDate;
                objAuctionNoticeItemDetails.CommodityId = item.CommodityId;
                objAuctionNoticeItemDetails.EntryDate = item.EntryDate;
                objAuctionNoticeItemDetails.NoOfPackages = item.NoOfPackages;


                objAuctionNoticeItemDetails.GodownLocation = String.IsNullOrEmpty(item.GodownLocation) ? "###" : item.GodownLocation;
                objAuctionNoticeItemDetails.BLNo = String.IsNullOrEmpty(item.BLNo) ? "###" : item.BLNo;
                // objAuctionNoticeItemDetails.Date= String.IsNullOrEmpty(item.BLNo) ? "###" : item.Date;
                objAuctionNoticeItemDetails.Weight = String.IsNullOrEmpty(item.Weight) ? "###" : item.Weight;
                objAuctionNoticeItemDetails.CUM = String.IsNullOrEmpty(item.CUM) ? "###" : item.CUM;
                objAuctionNoticeItemDetails.SQM = String.IsNullOrEmpty(item.SQM) ? "###" : item.SQM;
                objAuctionNoticeItemDetails.Duty = String.IsNullOrEmpty(item.Duty) ? "###" : item.Duty;
                objAuctionNoticeItemDetails.Fob = String.IsNullOrEmpty(item.Fob) ? "###" : item.Fob;
                objAuctionNoticeItemDetails.CIF = String.IsNullOrEmpty(item.CIF) ? "###" : item.CIF;
                objAuctionNoticeItemDetails.IsInsured = String.IsNullOrEmpty(item.IsInsured) ? "###" : item.IsInsured;
                objAuctionNoticeItemDetails.BOENo = String.IsNullOrEmpty(item.BOENo) ? "###" : item.BOENo;
                objAuctionNoticeItemDetails.LineNo = String.IsNullOrEmpty(item.LineNo) ? "###" : item.LineNo;
                objAuctionNoticeItemDetails.ShippingBillNo = String.IsNullOrEmpty(item.ShippingBillNo) ? "###" : item.ShippingBillNo;
                objAuctionNoticeItemDetails.ContainerNo = String.IsNullOrEmpty(item.ContainerNo) ? "###" : item.ContainerNo;
                objAuctionNoticeItemDetails.CFSCode = String.IsNullOrEmpty(item.CFSCode) ? "###" : item.CFSCode;
                objAuctionNoticeItemDetails.Size = String.IsNullOrEmpty(item.Size) ? "###" : item.Size;
                objAuctionNoticeItemDetails.BLDate = String.IsNullOrEmpty(item.BLDate) ? "###" : item.BLDate;
                objAuctionNoticeItemDetails.ShipBillDate = String.IsNullOrEmpty(item.ShipBillDate) ? "###" : item.ShipBillDate;
                objAuctionNoticeItemDetails.CuttingDate = String.IsNullOrEmpty(item.CuttingDate) ? "###" : item.CuttingDate;
                objAuctionNoticeItemDetails.GodownID = item.GodownID;
                objAuctionNoticeItemDetails.RefId = item.RefId;
                objAuctionNoticeItemDetails.AuctionNoticeDate = convertAuctionDate;



                AuctionNoticeItemDetailsList.Add(objAuctionNoticeItemDetails);
            }
            string xmlAuctionNotice = Utility.CreateXML(AuctionNoticeItemDetailsList);

            xmlAuctionNotice = xmlAuctionNotice.Replace(">###<", "><");

            ObjAR.SaveAuctionReissueNotice(operationType, auctionType, SelectedAuctionNoticeDetailsList[0].PartyId, Convert.ToInt32(Session["BranchId"]), ObjLogin.Uid, xmlAuctionNotice, Auctionid, convertAuctionDate);
            return Json(ObjAR.DBResponse);
        }
        #endregion

    }
}