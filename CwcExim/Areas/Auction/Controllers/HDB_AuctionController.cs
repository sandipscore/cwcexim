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
    public class HDB_AuctionController : BaseController
    {
        // GET: Auction/HDB_Auction

        #region Auction Notice
        public ActionResult AuctionNotice()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult PopulateDataAuctionNotice(string operationType, string auctionType, int inPage)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetCargoDetailsList(operationType, auctionType, ObjLogin.Uid, inPage);

            return Json(ObjAR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult PopulateDataAuctionNoticeByOBLNoShipBill(string operationType, string auctionType, string OBLNo, string ShipBill)
        {
            AuctionRepository ObjAR = new AuctionRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjAR.GetCargoDetailsListByOBLNoShipBill(operationType, auctionType, ObjLogin.Uid, OBLNo, ShipBill);

            return Json(ObjAR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAuctionNoticeDataByOBLNoShipBill(string operationType, string auctionType, string AuctionNo)
        {
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjAR.GetAuctionNoticeOBLNoShipBill(operationType, auctionType, AuctionNo, ObjLogin.Uid);
            List<HDB_SavedAuctionNotice> SavedAuctionNoticeList = new List<HDB_SavedAuctionNotice>();

            List<HDB_AuctionNoticeItemDetails> LstAuctionNoticeItemDetails = new List<HDB_AuctionNoticeItemDetails>();
            if (ObjAR.DBResponse.Data != null)
            {
                LstAuctionNoticeItemDetails = (List<HDB_AuctionNoticeItemDetails>)ObjAR.DBResponse.Data;

                foreach (var item in LstAuctionNoticeItemDetails)
                {
                    var ifExistSavedAuctionNotice = SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).FirstOrDefault();

                    if (ifExistSavedAuctionNotice == null)
                    {
                        HDB_SavedAuctionNotice savedAuctionNotice = new HDB_SavedAuctionNotice();
                        savedAuctionNotice.SavedAuctionNoticeDetailsList = new List<HDB_SavedAuctionNoticeDetails>();
                        savedAuctionNotice.AuctionNoticeDtlId = item.AuctionNoticeDtlId;
                        savedAuctionNotice.NoticeNo = item.NoticeNo;
                        savedAuctionNotice.AuctionNoticeDate = item.AuctionNoticeDate;
                        savedAuctionNotice.PartyName = item.PartyName;

                        HDB_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new HDB_SavedAuctionNoticeDetails();

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
                        HDB_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new HDB_SavedAuctionNoticeDetails();
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
            return View("AuctionNoticeList", SavedAuctionNoticeList);
        }



        [HttpGet]
        public ActionResult GetAuctionNoticeList(string operationType, string auctionType)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetAllAuctionNotice(operationType, auctionType, ObjLogin.Uid);

            List<HDB_SavedAuctionNotice> SavedAuctionNoticeList = new List<HDB_SavedAuctionNotice>();

            List<HDB_AuctionNoticeItemDetails> LstAuctionNoticeItemDetails = new List<HDB_AuctionNoticeItemDetails>();
            if (ObjAR.DBResponse.Data != null)
            {
                LstAuctionNoticeItemDetails = (List<HDB_AuctionNoticeItemDetails>)ObjAR.DBResponse.Data;

                foreach (var item in LstAuctionNoticeItemDetails)
                {
                    var ifExistSavedAuctionNotice = SavedAuctionNoticeList.Where(x => x.NoticeNo == item.NoticeNo).FirstOrDefault();

                    if (ifExistSavedAuctionNotice == null)
                    {
                        HDB_SavedAuctionNotice savedAuctionNotice = new HDB_SavedAuctionNotice();
                        savedAuctionNotice.SavedAuctionNoticeDetailsList = new List<HDB_SavedAuctionNoticeDetails>();
                        savedAuctionNotice.AuctionNoticeDtlId = item.AuctionNoticeDtlId;
                        savedAuctionNotice.NoticeNo = item.NoticeNo;
                        savedAuctionNotice.AuctionNoticeDate = item.AuctionNoticeDate;
                        savedAuctionNotice.PartyName = item.PartyName;

                        HDB_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new HDB_SavedAuctionNoticeDetails();

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
                        HDB_SavedAuctionNoticeDetails savedAuctionNoticeDetails = new HDB_SavedAuctionNoticeDetails();
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
        public JsonResult SaveAuctionIssueNotice(List<HDB_AuctionNoticeDetails> SelectedAuctionNoticeDetailsList, string operationType, string auctionType, string AuctionDate)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            List<HDB_AuctionNoticeItemDetails> AuctionNoticeItemDetailsList = new List<HDB_AuctionNoticeItemDetails>();
            DateTime dt = DateTime.ParseExact(AuctionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string convertAuctionDate = dt.ToString("yyyy-MM-dd");

            foreach (var item in SelectedAuctionNoticeDetailsList)
            {
                HDB_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new HDB_AuctionNoticeItemDetails();
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

            ObjAR.SaveAuctionIssueNotice(operationType, auctionType, SelectedAuctionNoticeDetailsList[0].PartyId, Convert.ToInt32(Session["BranchId"]), ObjLogin.Uid, xmlAuctionNotice);
            return Json(ObjAR.DBResponse);
        }

        public JsonResult PrintAuctionIssueNotice(int AuctionNoticeDtlId)
        {
            try
            {
                var Pages = new string[1];
                string FileName = "AuctionNotice.pdf";

                HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
                HDB_AuctionNoticePrintViewModel auctionNoticePrintViewModel = ObjAR.GetAuctionNoticeDataToPrint(AuctionNoticeDtlId);

                if (auctionNoticePrintViewModel != null)
                {
                    List<string> Tabs = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    int srNo = 0;

                    if (auctionNoticePrintViewModel.OperationType == "Import" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>CFS-Kukatpally</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th ></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='font-weight: 600;text-align: center;padding: 5px;border-bottom:1px solid #000;'>" + auctionNoticePrintViewModel.CompanyAddress + "</th>");
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
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'></td>");
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
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods said to be imported by you are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We the Central Warehousing Corporation, Container Freight Station, therefore, hereby tender the Notice under sec: 48, of customs Act 1962 to the importer on the address as manifested in the relevant documents to advise / suggest / request to make necessary arrangements to clear their cargo in question within 15 days from the date of issue of this notice, paying all their dues to us.  If they are not inclined to clear the cargo within the stipulated time / seek any further time for the same / abandon the goods, they may specifically intimate this to Container Freight Station immediately under intimation to the Assistant Commissioner of Customs, ICD-Sanathnagar, By RPAD / Courier.< br />< br /> Notice is requested to ensure that reply is sent to both CFS and the Assistant Commissioner of Customs, ICD Sanathnagar, by RPAD or acknowledgment is sent in person / by Courier.  Reply should specifically state the intention of the consignee as regards the goods and the time required.<br/><br/>If they have filed any Bill of Entry in the Customs for clearance of the same, they may indicate the same with its present status.<br/><br/> In case, no reply is received from them within the afore said period, it would be presumed that they are not interested in the above cargo lying in this CFS and cargo will be auctioned/disposed of as per the provisions of Customs Act, 1962, without any further reference of intimation.<br/><br/><br/>");
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
                        //  sb.Append("<td style='text-align: center;padding: 5px;'>Yours faithfully<br/><br/><br/><br/><br/>For <span style='font-weight: 600;'> Manager ICD</span>");
                        // sb.Append("</td>");
                        // sb.Append("</tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;'><tbody>");
                        sb.Append("<tr><td><span><br/></span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager CFS</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1. The Asst. Commissioner of Custom(Import) Inland Container Depot,  Hyderabad - 37</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1.The Deputy Commissioner of Customs, ICD, Sanathnagar, Hyderabad.<br/>2.The Superintendent of Customs, CFS, Kukatpally, Hyderabad.<br/>3.Forwarder.</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        Pages[0] = sb.ToString();
                    }
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CARGO")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>CFS-Kukatpally</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;font-weight: 600;text-align: center;padding: 5px;'>" + auctionNoticePrintViewModel.CompanyAddress + "</th>");
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
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'></td>");
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
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods meant for export are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We the Central Warehousing Corporation, Container Freight Station, therefore, hereby tender the Notice under sec: 48, of customs Act 1962 to the importer on the address as manifested in the relevant documents to advise / suggest / request to make necessary arrangements to clear their cargo in question within 15 days from the date of issue of this notice, paying all their dues to us.  If they are not inclined to clear the cargo within the stipulated time / seek any further time for the same / abandon the goods, they may specifically intimate this to Container Freight Station immediately under intimation to the Assistant Commissioner of Customs, ICD-Sanathnagar, By RPAD / Courier.< br />< br /> Notice is requested to ensure that reply is sent to both CFS and the Assistant Commissioner of Customs, ICD Sanathnagar, by RPAD or acknowledgment is sent in person / by Courier.  Reply should specifically state the intention of the consignee as regards the goods and the time required.<br/><br/>If they have filed any Bill of Entry in the Customs for clearance of the same, they may indicate the same with its present status.<br/><br/> In case, no reply is received from them within the afore said period, it would be presumed that they are not interested in the above cargo lying in this CFS and cargo will be auctioned/disposed of as per the provisions of Customs Act, 1962, without any further reference of intimation.<br/><br/><br/>");
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
                        sb.Append("<th style='text-align: left;padding:5px;'>Sr. No.</th><th style='text-align: left;padding:5px;'>Shipbill No.</th><th style='text-align: left;padding:5px;'>ICD Code</th><th style='text-align: left;padding:5px;'>In Date</th><th style='text-align: right;padding:5px;'>No. Pkg</th><th style='text-align: right;padding:5px;'>Gr. Wt.</th><th style='text-align: left;padding:5px;'>IGM/Item No.</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                        foreach (var item in auctionNoticePrintViewModel.ParticularsOfGoodsList)
                        {
                            srNo++;
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + srNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ShipBillNo + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.ICDCode + "</td>");
                            sb.Append("<td style='text-align: left;padding:5px;'>" + item.CartingDate + "</td>");
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
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager CFS</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1. The Asst. Commissioner of Custom(Import) Inland Container Depot,  Hyderabad - 37</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1.The Deputy Commissioner of Customs, ICD, Sanathnagar, Hyderabad.<br/>2.The Superintendent of Customs, CFS, Kukatpally, Hyderabad.<br/>3.Forwarder.</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
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
                    else if (auctionNoticePrintViewModel.OperationType == "Export" && auctionNoticePrintViewModel.AuctionType == "CONTAINER")
                    {
                        sb.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                        sb.Append("<thead>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-size: 12pt;'>CENTRAL WAREHOUSING CORPORATION</th>");
                        sb.Append("<th style='text-align: left;'>Doc. No. <span>CFS-Kukatpally</span></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='width:85%;text-align: center;font-weight: 400;'>(A GOVT. OF INDIA UNDERTAKING)</th>");
                        sb.Append("<th></th>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<th style='border-bottom:1px solid #000;font-weight: 600;text-align: center;padding: 5px;'>" + auctionNoticePrintViewModel.CompanyAddress + "</th>");
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
                        sb.Append("<td style='font-weight: 600;text-align: center;padding: 5px;'></td>");
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
                        //sb.Append("Dear Sir/Madam,<br/><br/>We have to inform you that undermentioned goods meant for export are lying uncleared for over 30 days from the date of landing at this ICD.< br />If the goods are not cleared on payment of charges accrued thereon within 10 days of the receipt of this notice, the goods will be sold by public auction under provisions of the Custom Act 1962 without any further notice to you.<br/>The goods are also liable for sale by tender without any further notice to you.<br/>If the goods under reference have already been cleared, this notice may please be treated as cancelled.<br/><br/><br/>");
                        sb.Append("Dear Sir/Madam,<br/><br/>We the Central Warehousing Corporation, Container Freight Station, therefore, hereby tender the Notice under sec: 48, of customs Act 1962 to the importer on the address as manifested in the relevant documents to advise / suggest / request to make necessary arrangements to clear their cargo in question within 15 days from the date of issue of this notice, paying all their dues to us.  If they are not inclined to clear the cargo within the stipulated time / seek any further time for the same / abandon the goods, they may specifically intimate this to Container Freight Station immediately under intimation to the Assistant Commissioner of Customs, ICD-Sanathnagar, By RPAD / Courier.< br />< br /> Notice is requested to ensure that reply is sent to both CFS and the Assistant Commissioner of Customs, ICD Sanathnagar, by RPAD or acknowledgment is sent in person / by Courier.  Reply should specifically state the intention of the consignee as regards the goods and the time required.<br/><br/>If they have filed any Bill of Entry in the Customs for clearance of the same, they may indicate the same with its present status.<br/><br/> In case, no reply is received from them within the afore said period, it would be presumed that they are not interested in the above cargo lying in this CFS and cargo will be auctioned/disposed of as per the provisions of Customs Act, 1962, without any further reference of intimation.<br/><br/><br/>");
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
                        sb.Append("<tr><td colspan='2' style='text-align: right;font-weight: 600;font-size: 8pt;margin-top:5%;'>Yours faithfully<br/><br/><br/><br/><br/> For Manager CFS</td></tr>");
                        sb.Append("</tbody></table></td>");
                        sb.Append("</tr></tbody></table></td></tr>");

                        sb.Append("<tr><td colspan='2'><table style='width:100%;'><tbody><tr>");
                        sb.Append("<td colspan='12' style='width:100%;'><table style='width:100%;font-size: 9pt;'><tbody>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1. The Asst. Commissioner of Custom(Import) Inland Container Depot,  Hyderabad - 37</span></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;valign:top;'><br/><br/>COPY TO:<br/><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>1.The Deputy Commissioner of Customs, ICD, Sanathnagar, Hyderabad.<br/>2.The Superintendent of Customs, CFS, Kukatpally, Hyderabad.<br/>3.Forwarder.</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>2. " + auctionNoticePrintViewModel.Shippingline + "</span></td></tr>");
                        //sb.Append("<tr><td colspan='2' style='text-align: left;font-weight: 600;'><span style='font-weight: 400;font-weight: 400;font-size: 8pt;'>" + auctionNoticePrintViewModel.ShippinglineAddress + "</span></td></tr>");
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
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
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

                HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
                ObjAR.GetAuctionDetailsForReissue(AuctionNoticeDtlId);

                return Json(new { Status = "1", Message = ObjAR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveAuctionReissueNotice(List<HDB_AuctionNoticeDetails> SelectedAuctionNoticeDetailsList, string operationType, string auctionType, string AuctionDate, int Auctionid)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            List<HDB_AuctionNoticeItemDetails> AuctionNoticeItemDetailsList = new List<HDB_AuctionNoticeItemDetails>();

            DateTime dt = DateTime.ParseExact(AuctionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string convertAuctionDate = dt.ToString("yyyy-MM-dd");

            foreach (var item in SelectedAuctionNoticeDetailsList)
            {



                HDB_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new HDB_AuctionNoticeItemDetails();
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



        #region Mark For Notice

        public ActionResult MarkForNotice()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult PopulateDataForMarkForNotice()
        {
            List<HDB_MarkForNotice> LstMarkForNotice = new List<HDB_MarkForNotice>();
            StringBuilder sb_MarkForNotice = new StringBuilder();
            StringBuilder sb_Marked = new StringBuilder();

            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetAllMarkForNotice(Convert.ToInt32(Session["BranchId"]));
            if (ObjAR.DBResponse.Data != null)
            {
                LstMarkForNotice = (List<HDB_MarkForNotice>)ObjAR.DBResponse.Data;

                foreach (var item in LstMarkForNotice)
                {
                    sb_MarkForNotice.Append("<tr>");
                    sb_MarkForNotice.Append("<td> " + item.NoticeNo + " </td >");
                    sb_MarkForNotice.Append("<td> " + item.AuctionNoticeDate + " </td >");
                    sb_MarkForNotice.Append("<td > " + item.PartyName + " </td >");
                    sb_MarkForNotice.Append("<td style='padding:0px;'>");
                    sb_MarkForNotice.Append("<table class='table nowrap' style='background-color:transparent'>");
                    foreach (var itemMarkForNoticeDetailsList in item.MarkForNoticeDetailsList)
                    {
                        sb_MarkForNotice.Append("<tr>");
                        sb_MarkForNotice.Append("<td style = 'width:70%;border-right:1px solid #ddd;border-top:none;font-size:0.98em !important;'> " + itemMarkForNoticeDetailsList.CommodityName + " </td>");
                        sb_MarkForNotice.Append("<td style='border-top:none;font-size:0.98em !important;'>  " + Math.Round(Convert.ToDecimal(itemMarkForNoticeDetailsList.Weight), 3) + " </td>");
                        sb_MarkForNotice.Append("</tr>");
                    }
                    sb_MarkForNotice.Append("</table>");
                    sb_MarkForNotice.Append("</td>");
                    sb_MarkForNotice.Append("<td style ='text-align:center;' >");
                    sb_MarkForNotice.Append("<input type='checkbox' id='chk_" + item.AuctionNoticeId + "' onchange='MarkForNoticeChecked(" + item.AuctionNoticeId + ")' /> <label for='chk_" + item.AuctionNoticeId + "' style='text-align:center;'><i class='square'></i></label>");
                    sb_MarkForNotice.Append("</td>");
                    sb_MarkForNotice.Append("</tr>");

                }
            }

            return Json(sb_MarkForNotice.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMarkForNotice(List<int> SelectedIdList, string lotNo)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            string ids = String.Join(",", SelectedIdList);
            ObjAR.SaveMarkForNotice(ids, ObjLogin.Uid, lotNo);
            return Json(ObjAR.DBResponse);
        }

        [HttpGet]
        public ActionResult GetAuctionMarkedNoticeList()
        {
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetAuctionMarkedNoticeList();
            List<HDB_MarkedNotice> LstMarkedNotice = new List<HDB_MarkedNotice>();

            if (ObjAR.DBResponse.Data != null)
            {
                LstMarkedNotice = (List<HDB_MarkedNotice>)ObjAR.DBResponse.Data;
            }
            return View("MarkedNoticeList", LstMarkedNotice);
        }


        #endregion



        #region Despatch
        public ActionResult Despatch()
        {
            List<HDB_MarkForNotice> LstMarkForNotice = new List<Models.HDB_MarkForNotice>();
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetAllDespatchForNotice(Convert.ToInt32(Session["BranchId"]));
            LstMarkForNotice = (List<HDB_MarkForNotice>)ObjAR.DBResponse.Data;
            if (LstMarkForNotice != null)
            {
                ViewBag.lstNoticeList = Newtonsoft.Json.JsonConvert.SerializeObject(LstMarkForNotice);
            }
            else
            {
                ViewBag.lstNoticeList = null;
            }


            return PartialView();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Despatch(HDB_Despatch vm)
        {
            HDB_AuctionRepository AR = new HDB_AuctionRepository();
            if (ModelState.IsValid)
            {

                AR.UpdateDespatch(vm);
            }
            return Json(AR.DBResponse);
        }

        public ActionResult GetAllDespatch()
        {
            List<HDB_Despatch> LstDespatch = new List<HDB_Despatch>();
            HDB_AuctionRepository ObjAR = new HDB_AuctionRepository();
            ObjAR.GetAllDespatch(Convert.ToInt32(Session["BranchId"]));
            LstDespatch = (List<HDB_Despatch>)ObjAR.DBResponse.Data;
            if (LstDespatch != null)
            {
                ViewBag.LstDespatch = Newtonsoft.Json.JsonConvert.SerializeObject(LstDespatch);
            }
            else
            {
                ViewBag.LstDespatch = null;
            }


            return PartialView(LstDespatch);
        }


        #endregion


        #region NOC Details
        public ActionResult NOCDetails()
        {

            return PartialView();
        }

        public JsonResult GetRefNoForNoc(int Flag)
        {
            HDB_AuctionRepository obj = new HDB_AuctionRepository();
            obj.GetAllShipbillOBLFor(Flag);
            List<HDB_NocDetails> LstNocDetails = new List<HDB_NocDetails>();
            LstNocDetails = (List<HDB_NocDetails>)obj.DBResponse.Data;

            return Json(LstNocDetails, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult NOCDetails(HDB_NocDetails vm)
        {
            HDB_AuctionRepository AR = new HDB_AuctionRepository();
            if (ModelState.IsValid)
            {
                if (vm.Type == "OBL")
                {
                    vm.Flag = 1;
                }
                else if (vm.Type == "Shipbill")
                {
                    vm.Flag = 2;
                }
                else if (vm.Type == "CONTAINER")
                {
                    vm.Flag = 3;
                }
                Login ObjLogin = (Login)Session["LoginUser"];
                AR.AddEditNOCDetails(vm, Convert.ToInt32(Session["BranchId"]), Convert.ToInt32(ObjLogin.Uid));

            }

            return Json(AR.DBResponse, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetNocList(int NocID)
        {
            HDB_AuctionRepository AR = new HDB_AuctionRepository();
            AR.GetAllNocDetails(NocID);
            List<HDB_NocDetails> lstNocDetails = new List<HDB_NocDetails>();
            lstNocDetails = (List<HDB_NocDetails>)AR.DBResponse.Data;

            return PartialView(lstNocDetails);
        }


        public JsonResult GetNocListForEdit(int NocID)
        {
            HDB_AuctionRepository AR = new HDB_AuctionRepository();
            AR.GetAllNocDetails(NocID);
            List<HDB_NocDetails> lstNocDetails = new List<HDB_NocDetails>();
            lstNocDetails = (List<HDB_NocDetails>)AR.DBResponse.Data;

            return Json(lstNocDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteNoc(int NocID)
        {
            HDB_AuctionRepository AR = new HDB_AuctionRepository();
            AR.DeleteNocDetails(NocID);
            return Json(AR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}