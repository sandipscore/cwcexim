using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.CashManagement.Models;
using Newtonsoft.Json;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class WljPostPaymentSheet
    {
        private decimal _CWCTotal = 0M;
        private decimal _HTTotal = 0M;
        private decimal _CWCTDS = 0M;
        private decimal _HTTDS = 0M;

        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string ApproveOn { get; set; } = string.Empty;
        public string ROAddress { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public int RequestId { get; set; } = 0;
        public string RequestNo { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAddress { get; set; } = string.Empty;
        public string PartyState { get; set; } = string.Empty;
        public string PartyStateCode { get; set; } = string.Empty;
        public string PartyGST { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal TotalAmt { get; set; } = 0M;
        public decimal TotalDiscount { get; set; } = 0M;
        public decimal TotalTaxable { get; set; } = 0M;
        public decimal TotalCGST { get; set; } = 0M;
        public decimal TotalSGST { get; set; } = 0M;
        public decimal TotalIGST { get; set; } = 0M;
        public decimal CWCAmtTotal { get; set; } = 10M;
        public decimal CWCTotal
        {
            get { return _CWCTotal; }
            set { _CWCTotal = value; }
        }
        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal
        {
            get { return _HTTotal; }
            set { _HTTotal = value; }
        }
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS
        {
            get { return _CWCTDS; }
            set { _CWCTDS = value; }
        }
        public decimal HTTDS
        {
            get { return _HTTDS; }
            set { _HTTDS = value; }
        }
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal InvoiceAmt { get; set; } = 0M;
        public string ShippingLineName { get; set; } = string.Empty;
        public string CHAName { get; set; } = string.Empty;
        public string ImporterExporter { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public string BOEDate { get; set; } = string.Empty;
        public string CFSCode { get; set; } = string.Empty;
        public string DestuffingDate { get; set; } = string.Empty;
        public string StuffingDate { get; set; } = string.Empty;
        public string CartingDate { get; set; } = string.Empty;
        public string ArrivalDate { get; set; } = string.Empty;
        public int TotalNoOfPackages { get; set; } = 0;
        public decimal TotalGrossWt { get; set; } = 0M;
        public decimal TotalWtPerUnit { get; set; } = 0M;
        public decimal TotalSpaceOccupied { get; set; } = 0M;
        public string TotalSpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal TotalValueOfCargo { get; set; } = 0M;
        public decimal PdaAdjustedAmount { get; set; } = 0M;
        public string CompGST { get; set; } = string.Empty;
        public string CompPAN { get; set; } = string.Empty;
        public string CompStateCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        //subir for edir receipt
        public string CashierRemarks { get; set; } = string.Empty;

        public string PDAadjustedCashReceiptEdit { get; set; } = string.Empty;
        // end 
        public int DeliveryType { get; set; } = 1;
        public string BillType { get; set; } = string.Empty;
        public string StuffingDestuffDateType { get; set; } = string.Empty;
        public string StuffingDestuffingDate { get; set; } = string.Empty;
        public string ImporterExporterType { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string UptoDate { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0M;
        public IList<WljPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WljPostPaymentContainer>();
        public IList<WljPostPaymentContainer> lstStorPostPaymentCont { get; set; } = new List<WljPostPaymentContainer>();
        public IList<WljPostPaymentCharge> lstPostPaymentChrg { get; set; } = new List<WljPostPaymentCharge>();
        public IList<WljContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WljContainerWiseAmount>();
        public IList<PreContainerWiseAmount> lstPreContWiseAmount { get; set; } = new List<PreContainerWiseAmount>();
        public IList<PreInvoiceCargo> lstPreInvoiceCargo { get; set; } = new List<PreInvoiceCargo>();
        public List<WljPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WljPreInvoiceContainer>();
        public List<CashReceipt> CashReceiptDetails { get; set; } = new List<CashReceipt>();

        public List<WljOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WljOperationCFSCodeWiseAmount>();

        public int OperationType { get; set; }

        public string ExportUnder { get; set; } = string.Empty;
        public string SEZ { get; set; } = string.Empty;
        // SUBIR 

        // public List<ClauseTypeForHT> lstClauseTypeForHT = new List<ClauseTypeForHT>();
        public List<WljCfsCodewiseRateHT> lstCfsCodewiseRateHT = new List<WljCfsCodewiseRateHT>();
        // End

        public void CalculateCharges(int Type, GenericChargesModel objGenericCharges, int GodownTypeId)
        {
            this.lstStorPostPaymentCont.ToList().ForEach(item =>
            {
                this.lstContWiseAmount.Add(new WljContainerWiseAmount()
                {
                    CFSCode = item.CFSCode,
                    LineNo = item.LineNo
                });
            });
            switch (Type)
            {
                case 1:
                    CalculateYardCargo(objGenericCharges);
                    break;
                case 2:
                    CalculateYard(objGenericCharges);
                    break;
                case 3:
                    CalculateExport(objGenericCharges);
                    break;
                case 4:
                    CalculateGodownDestuffing(objGenericCharges);
                    break;
                case 5:
                    CalculateGodownDelivery(objGenericCharges, GodownTypeId);
                    break;
                case 6:
                    CalculateEmptyContDelivery(objGenericCharges);
                    break;
                case 7:
                    CalculateBTT(objGenericCharges);
                    break;
                case 8:
                    CalculateLoadedContainerExport(objGenericCharges);
                    break;
                case 9:
                    CalculateAuctionInvoice(objGenericCharges);
                    break;
                default:
                    break;
            }
            this.lstPostPaymentChrg.ToList().ForEach(item =>
            {
                item.Amount = Math.Round(item.Amount, 2);
                item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                item.Total = Math.Round(item.Total, 2);
            });

            this.TotalAmt = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalDiscount = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            this.TotalTaxable = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalCGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            this.TotalSGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            this.TotalIGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            this.CWCTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            this.HTTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            this.CWCAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            this.HTAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            this.CWCTDS = Math.Round((this.CWCAmtTotal * this.CWCTDSPer) / 100);
            this.HTTDS = Math.Round((this.HTAmtTotal * this.HTTDSPer) / 100);
            this.TDS = Math.Round(this.CWCTDS + this.HTTDS);
            this.TDSCol = this.TDS;
            //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
            this.AllTotal = this.CWCTotal + this.HTTotal + this.TDSCol - this.TDS;
            this.RoundUp = Math.Ceiling(this.AllTotal) - this.AllTotal;
            this.InvoiceAmt = Math.Ceiling(this.AllTotal);
        }

        public void CalculateChargesForKol(int Type, GenericChargesModel objGenericCharges, int GodownTypeId)
        {
            this.lstStorPostPaymentCont.ToList().ForEach(item =>
            {
                this.lstContWiseAmount.Add(new WljContainerWiseAmount()
                {
                    CFSCode = item.CFSCode,
                    LineNo = item.LineNo
                });
            });
            switch (Type)
            {
                case 1:
                    CalculateYardCargo(objGenericCharges);
                    break;
                case 2:
                    CalculateYard(objGenericCharges);
                    break;
                case 3:
                    CalculateExport(objGenericCharges);
                    break;
                case 4:
                    CalculateGodownDestuffing(objGenericCharges);
                    break;
                case 5:
                    CalculateGodownDeliveryForKol(objGenericCharges, GodownTypeId);
                    break;
                case 6:
                    CalculateEmptyContDelivery(objGenericCharges);
                    break;
                case 7:
                    CalculateBTT(objGenericCharges);
                    break;
                case 8:
                    CalculateLoadedContainerExport(objGenericCharges);
                    break;
                case 9:
                    CalculateAuctionInvoice(objGenericCharges);
                    break;
                default:
                    break;
            }
            this.lstPostPaymentChrg.ToList().ForEach(item =>
            {
                item.Amount = Math.Round(item.Amount, 2);
                item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                item.Total = Math.Round(item.Total, 2);
            });

            this.TotalAmt = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalDiscount = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            this.TotalTaxable = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalCGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            this.TotalSGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            this.TotalIGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            this.CWCTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            this.HTTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            this.CWCAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            this.HTAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            this.CWCTDS = Math.Round((this.CWCAmtTotal * this.CWCTDSPer) / 100);
            this.HTTDS = Math.Round((this.HTAmtTotal * this.HTTDSPer) / 100);
            this.TDS = Math.Round(this.CWCTDS + this.HTTDS);
            this.TDSCol = this.TDS;
            //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
            this.AllTotal = this.CWCTotal + this.HTTotal + this.TDSCol - this.TDS;
            this.RoundUp = Math.Ceiling(this.AllTotal) - this.AllTotal;
            this.InvoiceAmt = Math.Ceiling(this.AllTotal);
        }

        public void CalculateChargesForKol(int Type, GenericChargesModel objGenericCharges)
        {
            this.lstStorPostPaymentCont.ToList().ForEach(item =>
            {
                this.lstContWiseAmount.Add(new WljContainerWiseAmount()
                {
                    CFSCode = item.CFSCode,
                    LineNo = item.LineNo
                });
            });
            switch (Type)
            {
                case 1:
                    CalculateYardCargo(objGenericCharges);
                    break;
                case 2:
                    CalculateYard(objGenericCharges);
                    break;
                case 3:
                    CalculateExportForKol(objGenericCharges);
                    break;
                case 4:
                    CalculateGodownDestuffing(objGenericCharges);
                    break;
                case 5:
                    CalculateGodownDeliveryForKol(objGenericCharges);
                    break;
                case 6:
                    CalculateEmptyContDelivery(objGenericCharges);
                    break;
                case 7:
                    CalculateBTTForKol(objGenericCharges);
                    break;
                case 8:
                    CalculateLoadedContainerExport(objGenericCharges);
                    break;
                case 9:
                    CalculateAuctionInvoice(objGenericCharges);
                    break;
                case 10:
                    CalculateEmptyContDeliveryGateOut(objGenericCharges);
                    break;
                case 11:
                    CalculateEmptyContDeliveryGateIn(objGenericCharges);
                    break;
                default:
                    break;
            }
            this.lstPostPaymentChrg.ToList().ForEach(item =>
            {
                item.Amount = Math.Round(item.Amount, 2);
                item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                item.Total = Math.Round(item.Total, 2);
            });

            this.TotalAmt = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalDiscount = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            this.TotalTaxable = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalCGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            this.TotalSGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            this.TotalIGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            this.CWCTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            this.HTTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            this.CWCAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            this.HTAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            this.CWCTDS = Math.Round((this.CWCAmtTotal * this.CWCTDSPer) / 100);
            this.HTTDS = Math.Round((this.HTAmtTotal * this.HTTDSPer) / 100);
            this.TDS = Math.Round(this.CWCTDS + this.HTTDS);
            this.TDSCol = this.TDS;
            //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
            this.AllTotal = this.CWCTotal + this.HTTotal + this.TDSCol - this.TDS;
            this.RoundUp = Math.Ceiling(this.AllTotal) - this.AllTotal;
            this.InvoiceAmt = Math.Ceiling(this.AllTotal);
        }

        public void CalculateCharges(int Type, GenericChargesModel objGenericCharges)
        {
            this.lstStorPostPaymentCont.ToList().ForEach(item =>
            {
                this.lstContWiseAmount.Add(new WljContainerWiseAmount()
                {
                    CFSCode = item.CFSCode,
                    LineNo = item.LineNo
                });
            });
            switch (Type)
            {
                case 1:
                    CalculateYardCargo(objGenericCharges);
                    break;
                case 2:
                    CalculateYard(objGenericCharges);
                    break;
                case 3:
                    CalculateExport(objGenericCharges);
                    break;
                case 4:
                    CalculateGodownDestuffing(objGenericCharges);
                    break;
                case 5:
                    CalculateGodownDelivery(objGenericCharges);
                    break;
                case 6:
                    CalculateEmptyContDelivery(objGenericCharges);
                    break;
                case 7:
                    CalculateBTT(objGenericCharges);
                    break;
                case 8:
                    CalculateLoadedContainerExport(objGenericCharges);
                    break;
                case 9:
                    CalculateAuctionInvoice(objGenericCharges);
                    break;
                case 10:
                    CalculateEmptyContDeliveryGateOut(objGenericCharges);
                    break;
                case 11:
                    CalculateEmptyContDeliveryGateIn(objGenericCharges);
                    break;
                default:
                    break;
            }
            this.lstPostPaymentChrg.ToList().ForEach(item =>
            {
                item.Amount = Math.Round(item.Amount, 2);
                item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                item.Total = Math.Round(item.Total, 2);
            });

            this.TotalAmt = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalDiscount = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            this.TotalTaxable = Math.Round(this.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            this.TotalCGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            this.TotalSGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            this.TotalIGST = Math.Round(this.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            this.CWCTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            this.HTTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            this.CWCAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            this.HTAmtTotal = Math.Round(this.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            this.CWCTDS = Math.Round((this.CWCAmtTotal * this.CWCTDSPer) / 100);
            this.HTTDS = Math.Round((this.HTAmtTotal * this.HTTDSPer) / 100);
            this.TDS = Math.Round(this.CWCTDS + this.HTTDS);
            this.TDSCol = this.TDS;
            //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
            this.AllTotal = this.CWCTotal + this.HTTotal + this.TDSCol - this.TDS;
            this.RoundUp = Math.Ceiling(this.AllTotal) - this.AllTotal;
            this.InvoiceAmt = Math.Ceiling(this.AllTotal);
        }


        private void CalculateYard(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");

            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = Math.Round(objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0), 2);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    var ContWiseGR = 0M;
                    if (System.Web.HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = Math.Round(ContWiseGR - ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0)), 2);
                });
                var ActualGrLoaded = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrLoaded,
                    Discount = 0,
                    Taxable = ActualGrLoaded,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2),
                    Total = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrLoaded + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2)))) : ActualGrLoaded, 2)
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPYard", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        var InvDt = Convert.ToDateTime(this.InvoiceDate).ToString("dd/MM/yyyy");
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((DateTime.ParseExact(InvDt, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += Math.Round(TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ?
                                    this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Chare
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //For kandla
                {
                    #region Insurance kandla
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0 && o.LCLFCL == "FCL").ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                              - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge =
                            Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                    });

                    var ActualInsurance = Math.Round(Insurance, 2) - Math.Round((this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    //var ActualInsurance = Math.Round(Insurance, 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                    #endregion
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge =
                            Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                    });

                    var ActualInsurance = Math.Round(Insurance, 2) - Math.Round((this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                    }
                });

                var ActualWeighmentCharges = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighmentCharges = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighmentCharges,
                    Discount = 0,
                    Taxable = ActualWeighmentCharges,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighmentCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                // subir
                var RateForHT = 0M;
                var TotalQuantity = 0M;
                lstCfsCodewiseRateHT.Clear();
                //
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            /*
                            var checkMatchedContainer = this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType);



                            if (checkMatchedContainer.Count() > 0)
                            {

                                foreach (var contInfo in checkMatchedContainer)
                                {
                                    TotalQuantity += contInfo.NoOfPackages;
                                }

                                RateForHT = item1.RateCWC;
                                lstClauseTypeForHT.Add(new ClauseTypeForHT
                                {

                                    OperationCode = item1.OperationCode,
                                    ClauseOrder = item1.ClauseOrder.ToString(),
                                    RateCWC = item1.RateCWC.ToString(),
                                    CommodityType = item1.CommodityType.ToString(),
                                    ContainerType = item1.ContainerType.ToString(),
                                    OperationDesc = item1.OperationSDesc,
                                    SacCode = item1.SacCode,
                                    ChargeType = "HT",
                                    Size = item1.Size,
                                    Quantity = TotalQuantity.ToString()
                                });
                            }
                            */
                            //
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {

                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 1 && item1.OperationCode == "3")
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                              .ToList().ForEach(c =>
                              {
                                  this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                  {
                                      CFSCode = c.CFSCode,
                                      Clause = item1.OperationCode,
                                      CommodityType = c.CargoType.ToString(),
                                      Size = c.Size,
                                      Rate = item1.RateCWC
                                  });
                              });

                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                /*
                                var checkMatchedContainer = this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType);
                                

                                

                                if (checkMatchedContainer.Count() > 0)
                                {
                                    foreach (var contInfo in checkMatchedContainer)
                                    {
                                        TotalQuantity += contInfo.NoOfPackages;
                                    }


                                    RateForHT = item1.RateCWC;
                                    lstClauseTypeForHT.Add(new ClauseTypeForHT
                                    {

                                        OperationCode = item1.OperationCode,
                                        ClauseOrder = item1.ClauseOrder.ToString(),
                                        RateCWC = item1.RateCWC.ToString(),
                                        CommodityType = item1.CommodityType.ToString(),
                                        ContainerType = item1.ContainerType.ToString(),
                                        OperationDesc = item1.OperationSDesc,
                                        SacCode = item1.SacCode,
                                        ChargeType = "HT",
                                        Size = item1.Size,
                                        Quantity= TotalQuantity.ToString()
                                    });
                                }
                                */
                                //

                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 2), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,//RateForHT, //
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    RateForHT = 0M;
                    TotalQuantity = 0M;
                }
                //subir
                // var checkingHTBeforeGrpBy = lstClauseTypeForHT.ToList();
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                // var checkingHT = lstClauseTypeForHT.GroupBy(m=>new {m.Size,m.OperationCode,m.CommodityType,m.ContainerType,m.SacCode,m.RateCWC,m.ChargeType }).ToList(); 
                //HttpContext.Current.Session["lstClauseTypeForHT"] = checkingHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateYardCargo(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion
            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");
            //  var GSTType = this.PartyStateCode == CompStateCode;

            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = Math.Round(objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0), 2);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                    var ContWiseGR = 0M;
                    if (HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }

                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = Math.Round(ContWiseGR - ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0)), 2);
                });
                var ActualGrLoaded = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrLoaded,
                    Discount = 0,
                    Taxable = ActualGrLoaded,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrLoaded + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2)))) : ActualGrLoaded
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPCargo", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += Math.Round(TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //For Kandla
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0 && o.LCLFCL == "FCL").ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                              - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge =
                            Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                    });
                    var ActualInsurance = Math.Round(Insurance, 2) - Math.Round((this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    //var ActualInsurance = Math.Round(Insurance, 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge =
                            Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                    });
                    var ActualInsurance = Math.Round(Insurance, 2) - Math.Round((this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                    }
                });

                var ActualWeighmentCharges = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighmentCharges = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighmentCharges,
                    Discount = 0,
                    Taxable = ActualWeighmentCharges,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighmentCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYeardPSCD == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                //subir
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                //

                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            ////subir
                            //var checkMatchedContainer = this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType);


                            //if (checkMatchedContainer.Count() > 0)
                            //{
                            //    foreach (var contInfo in checkMatchedContainer)
                            //    {
                            //        TotalQuantity += contInfo.NoOfPackages;
                            //    }
                            //    RateForHT = item1.RateCWC;
                            //    lstClauseTypeForHT.Add(new ClauseTypeForHT
                            //    {

                            //        OperationCode = item1.OperationCode,
                            //        ClauseOrder = item1.ClauseOrder.ToString(),
                            //        RateCWC = item1.RateCWC.ToString(),
                            //        CommodityType = item1.CommodityType.ToString(),
                            //        ContainerType = item1.ContainerType.ToString(),
                            //        OperationDesc = item1.OperationSDesc,
                            //        SacCode = item1.SacCode,
                            //        ChargeType = "HT",
                            //        Size = item1.Size,
                            //        Quantity = TotalQuantity.ToString()
                            //    });
                            //}

                            ////
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            ////subir
                            //var checkMatchedContainer = this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType);


                            //if (checkMatchedContainer.Count() > 0)
                            //{
                            //    foreach (var contInfo in checkMatchedContainer)
                            //    {
                            //        TotalQuantity += contInfo.NoOfPackages;
                            //    }

                            //    RateForHT = item1.RateCWC;
                            //    lstClauseTypeForHT.Add(new ClauseTypeForHT
                            //    {

                            //        OperationCode = item1.OperationCode,
                            //        ClauseOrder = item1.ClauseOrder.ToString(),
                            //        RateCWC = item1.RateCWC.ToString(),
                            //        CommodityType = item1.CommodityType.ToString(),
                            //        ContainerType = item1.ContainerType.ToString(),
                            //        OperationDesc = item1.OperationSDesc,
                            //        SacCode = item1.SacCode,
                            //        ChargeType = "HT",
                            //        Size = item1.Size,
                            //        Quantity = TotalQuantity.ToString()
                            //    });
                            //}

                            ////
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,//RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end

                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateGodownDelivery(GenericChargesModel objGenericCharges, int GodownTYpeId)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //KANDLA
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    var LoadedGR = 0M;
                    this.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.DestuffingDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                        //               - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);


                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.DestuffingDate).Date
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                        var ContWiseGR = 0M;

                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                    });
                    var ActualLoadedGr = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualLoadedGr,
                        Discount = 0,
                        Taxable = ActualLoadedGr,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr + (ActualLoadedGr * (cgst / 100)) + (ActualLoadedGr * (sgst / 100))) : (ActualLoadedGr + (ActualLoadedGr * (igst / 100)))) : ActualLoadedGr
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualLoadedGr + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2)))) : ActualLoadedGr
                    });
                }
                else
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });

                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType, GodownTYpeId);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.DestuffingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).CFSCode = item.LineNo;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    #region Insurance For Kandla

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    //var ActualInsurance = Math.Round(Insurance, 2);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });

                    #endregion
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                    var WeighmentCharges = 0M;
                    objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                    {
                        WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                    });
                    this.lstContWiseAmount.ToList().ForEach(item =>
                    {
                        if (this.InvoiceType == "Tax")
                        {
                            var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                            item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                        }
                    });

                    var ActualWeighmentCharges = 0M;
                    if (this.InvoiceType == "Tax")
                        ActualWeighmentCharges = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "8",
                        ChargeName = "Weighment Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualWeighmentCharges,
                        Discount = 0,
                        Taxable = ActualWeighmentCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualWeighmentCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                    });
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "8",
                        ChargeName = "Weighment Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDelivery == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "5")
                    {
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end

                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateExport(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;

            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                /*
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate;
                });*/
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = TotalEntryFees,
                    Discount = 0,
                    Taxable = TotalEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (TotalEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2)))) : TotalEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                var EmptyGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.StuffingDate).Date
                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    /*var TotalDays = Convert.ToInt32((DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                                    - DateTime.ParseExact(this.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);*/
                    var ContWiseGR = 0M;

                    if (System.Web.HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.EmptyGroundRent
                                               .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 2)
                                               .OrderBy(o => o.DaysRangeFrom)
                                               .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                EmptyGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                EmptyGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = EmptyGR;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.EmptyGroundRent
                                               .Where(o => o.Size == item.Size)// && o.CommodityType == item.CargoType)
                                               .OrderBy(o => o.DaysRangeFrom)
                                               .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                EmptyGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                EmptyGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = EmptyGR;
                                break;
                            }
                        }
                    }

                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty = Math.Round(ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty : 0) : 0)), 2);
                });
                var ActualGrEmpty = Math.Round(EmptyGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrEmpty) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrEmpty,
                    Discount = 0M,
                    Taxable = ActualGrEmpty,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrEmpty + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2)))) : ActualGrEmpty
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - Convert.ToDateTime(item.StuffingDate).Date).TotalDays + 1);
                    var ContWiseGR = 0M;

                    if (HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 2)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = LoadedGR;
                                break;
                            }
                        }
                    }

                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = Math.Round(ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0)), 2);
                });
                var ActualGrLoaded = LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrLoaded,
                    Discount = 0,
                    Taxable = ActualGrLoaded,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrLoaded + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2)))) : ActualGrLoaded
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "EXP", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += Math.Round(TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32(((DateTime)item.StuffingDate - (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += Math.Round((WtAmt > VolAmt) ? WtAmt : VolAmt, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                var Insurance = 0M;
                this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                    //                   - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);



                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                       - (DateTime)item.CartingDate).TotalDays + 1);

                    Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge = Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                });
                var ActualInsurance = Math.Round(Insurance, 2) - Math.Round(this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0, 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualInsurance,
                    Discount = 0,
                    Taxable = ActualInsurance,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                    }
                });

                var ActualWeighment = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighment = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighment,
                    Discount = 0,
                    Taxable = ActualWeighment,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighment + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2)))) : ActualWeighment
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ExportPS == 1).Select(o => new { Clause = o.Clause });
                //var ApplicableHT = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;

                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {

                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5" || item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else if ((item1.OperationCode == "XXI-5") && item1.OperationType == 2)
                        {
                            var HtChrg = 0M;
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });

                                    HtChrg += item1.RateCWC * (c.GrossWt / 1000);
                                });
                            HTCharges += Math.Round(HtChrg, 2);
                            //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type), 2);
                        }
                        else if ((item1.OperationCode == "XXI-6") && item1.OperationType == 2)
                        {
                            var HtChrg = 0M;
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });

                                    HtChrg += item1.RateCWC;
                                });
                            HTCharges += Math.Round(HtChrg, 2);
                            //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 2), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,//RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });

                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateExportForKol(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");


            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                /*
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate;
                });*/
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = TotalEntryFees,
                    Discount = 0,
                    Taxable = TotalEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (TotalEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2)))) : TotalEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                var EmptyGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.StuffingDate).Date
                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    /*var TotalDays = Convert.ToInt32((DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                                    - DateTime.ParseExact(this.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);*/
                    var ContWiseGR = 0M;

                    if (System.Web.HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.EmptyGroundRent
                                               .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 2)
                                               .OrderBy(o => o.DaysRangeFrom)
                                               .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                EmptyGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                EmptyGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = EmptyGR;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.EmptyGroundRent
                                               .Where(o => o.Size == item.Size)// && o.CommodityType == item.CargoType)
                                               .OrderBy(o => o.DaysRangeFrom)
                                               .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                EmptyGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                EmptyGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = EmptyGR;
                                break;
                            }
                        }
                    }

                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty = Math.Round(ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty : 0) : 0)), 2);
                });
                var ActualGrEmpty = Math.Round(EmptyGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrEmpty) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrEmpty,
                    Discount = 0M,
                    Taxable = ActualGrEmpty,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrEmpty + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2)))) : ActualGrEmpty
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - Convert.ToDateTime(item.StuffingDate).Date).TotalDays + 1);
                    var ContWiseGR = 0M;

                    if (HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 2)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR = LoadedGR;
                                break;
                            }
                        }
                    }

                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = Math.Round(ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0)), 2);
                });
                var ActualGrLoaded = LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrLoaded,
                    Discount = 0,
                    Taxable = ActualGrLoaded,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrLoaded + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2)))) : ActualGrLoaded
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "EXP", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += Math.Round(TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    int counter = 0;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        counter++;

                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32(((DateTime)item.StuffingDate - (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        if (counter == 1)
                        {
                            foreach (var item1 in objGenericCharges.StorageRent
                                .Where(o => o.WarehouseType == 2)
                                .OrderBy(o => o.DaysRangeFrom)
                                .ToList())
                            {
                                var MTRate = item1.RateMetricTonePerDay;
                                var CBMRate = item1.RateCubMeterPerDay;
                                var From = item1.DaysRangeFrom;
                                var To = item1.DaysRangeTo;

                                if (TotalDays >= To)
                                {
                                    WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                    VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                                }
                                else
                                {
                                    WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                    VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                    break;
                                }
                            }
                            StorageChrg += Math.Round((WtAmt > VolAmt) ? WtAmt : VolAmt, 2);
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                var Insurance = 0M;
                this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                    //                   - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);



                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                       - (DateTime)item.CartingDate).TotalDays + 1);

                    Insurance += Math.Round(item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge = Math.Round((item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0)), 2);
                });
                var ActualInsurance = Math.Round(Insurance, 2) - Math.Round(this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0, 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualInsurance,
                    Discount = 0,
                    Taxable = ActualInsurance,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                    }
                });

                var ActualWeighment = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighment = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighment,
                    Discount = 0,
                    Taxable = ActualWeighment,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighment + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2)))) : ActualWeighment
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ExportPS == 1).Select(o => new { Clause = o.Clause });
                //var ApplicableHT = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;

                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {

                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5" || item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.Clauseweight)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else if ((item1.OperationCode == "XXI-5") && item1.OperationType == 2)
                        {
                            var HtChrg = 0M;
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });

                                    HtChrg += item1.RateCWC * (c.GrossWt / 1000);
                                });
                            HTCharges += Math.Round(HtChrg, 2);
                            //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type), 2);
                        }
                        else if ((item1.OperationCode == "XXI-6") && item1.OperationType == 2)
                        {
                            var HtChrg = 0M;
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });

                                    HtChrg += item1.RateCWC;
                                });
                            HTCharges += Math.Round(HtChrg, 2);
                            //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == item1.Type), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 2), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,//RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });

                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateGodownDestuffing(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");

            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    var ContWiseGR = 0M;
                    if (System.Web.HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                });
                var ActualLoadedGr = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualLoadedGr,
                    Discount = 0,
                    Taxable = ActualLoadedGr,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr + (ActualLoadedGr * (cgst / 100)) + (ActualLoadedGr * (sgst / 100))) : (ActualLoadedGr + (ActualLoadedGr * (igst / 100)))) : ActualLoadedGr
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualLoadedGr + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2)))) : ActualLoadedGr
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPDest", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = (TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0));
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer + (ActualReefer * (cgst / 100)) + (ActualReefer * (sgst / 100))) : (ActualReefer + (ActualReefer * (igst / 100)))) : ActualReefer
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0));
                    }
                });

                var ActualWeighment = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighment = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighment,
                    Discount = 0,
                    Taxable = ActualWeighment,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment + (ActualWeighment * (cgst / 100)) + (ActualWeighment * (sgst / 100))) : (ActualWeighment + (ActualWeighment * (igst / 100)))) : ActualWeighment
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighment + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2)))) : ActualWeighment
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDSPS == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,//RateForHT, //
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });

                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateGodownDelivery(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //KANDLA
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    var LoadedGR = 0M;
                    this.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.DestuffingDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        var ContWiseGR = 0M;

                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                    });
                    var ActualLoadedGr = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualLoadedGr,
                        Discount = 0,
                        Taxable = ActualLoadedGr,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr + (ActualLoadedGr * (cgst / 100)) + (ActualLoadedGr * (sgst / 100))) : (ActualLoadedGr + (ActualLoadedGr * (igst / 100)))) : ActualLoadedGr
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualLoadedGr + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2)))) : ActualLoadedGr
                    });
                }
                else
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });

                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.DestuffingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).CFSCode = item.LineNo;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    #region Insurance For Kandla

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    // var ActualInsurance = Math.Round(Math.Round(Insurance, 2));
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });

                    #endregion
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDelivery == 1).Select(o => new { Clause = o.Clause });
                //var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var ApplicableHT = objGenericCharges.HTChargeRent.ToList();
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();

                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "5")
                    {


                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateGodownDeliveryForKol(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //KANDLA
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    var LoadedGR = 0M;
                    this.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.DestuffingDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        var ContWiseGR = 0M;

                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                    });
                    var ActualLoadedGr = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualLoadedGr,
                        Discount = 0,
                        Taxable = ActualLoadedGr,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr + (ActualLoadedGr * (cgst / 100)) + (ActualLoadedGr * (sgst / 100))) : (ActualLoadedGr + (ActualLoadedGr * (igst / 100)))) : ActualLoadedGr
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualLoadedGr + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2)))) : ActualLoadedGr
                    });
                }
                else
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });

                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.DestuffingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).CFSCode = item.LineNo;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    #region Insurance For Kandla

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    // var ActualInsurance = Math.Round(Math.Round(Insurance, 2));
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });

                    #endregion
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDelivery == 1).Select(o => new { Clause = o.Clause });
                //var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var ApplicableHT = objGenericCharges.HTChargeRent.ToList();
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();

                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.Clauseweight)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.Clauseweight)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });

                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "5")
                    {


                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        //With GodownTYpeId (For Kanlda)
        private void CalculateGodownDeliveryForKol(GenericChargesModel objGenericCharges, int GodownTYpeId)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1) //KANDLA
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    var LoadedGR = 0M;
                    this.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.DestuffingDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                        //               - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);


                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.DestuffingDate).Date
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                        var ContWiseGR = 0M;

                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                    });
                    var ActualLoadedGr = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualLoadedGr,
                        Discount = 0,
                        Taxable = ActualLoadedGr,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr + (ActualLoadedGr * (cgst / 100)) + (ActualLoadedGr * (sgst / 100))) : (ActualLoadedGr + (ActualLoadedGr * (igst / 100)))) : ActualLoadedGr
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualLoadedGr * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualLoadedGr + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualLoadedGr * (igst / 100))) : 0, 2)))) : ActualLoadedGr
                    });
                }
                else
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "2(2)",
                        ChargeName = "Ground Rent Loaded",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });

                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType, GodownTYpeId);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.DestuffingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).CFSCode = item.LineNo;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    #region Insurance For Kandla

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    //var ActualInsurance = Math.Round(Insurance, 2);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });

                    #endregion
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;
                    this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                        this.lstContWiseAmount.FirstOrDefault(o => o.LineNo == item.LineNo).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                    });
                    var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "5",
                        ChargeName = "Insurance Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualInsurance,
                        Discount = 0,
                        Taxable = ActualInsurance,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                    var WeighmentCharges = 0M;
                    objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                    {
                        WeighmentCharges += Math.Round(item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                    });
                    this.lstContWiseAmount.ToList().ForEach(item =>
                    {
                        if (this.InvoiceType == "Tax")
                        {
                            var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                            item.WeighmentCharge = Math.Round(objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0)), 2);
                        }
                    });

                    var ActualWeighmentCharges = 0M;
                    if (this.InvoiceType == "Tax")
                        ActualWeighmentCharges = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "8",
                        ChargeName = "Weighment Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualWeighmentCharges,
                        Discount = 0,
                        Taxable = ActualWeighmentCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighmentCharges * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualWeighmentCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighmentCharges * (igst / 100))) : 0, 2)))) : ActualWeighmentCharges
                    });
                }
                else
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "8",
                        ChargeName = "Weighment Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDelivery == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }

                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "5")
                    {
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end

                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateEmptyContDelivery(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                var EmptyGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - (DateTime)item.DestuffingDate).TotalDays + 1);

                    var ContWiseGR = 0M;

                    foreach (var item1 in objGenericCharges.EmptyGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == 2 && o.ImpExp == 1)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                    {
                        var From = item1.DaysRangeFrom;
                        var To = item1.DaysRangeTo;
                        var Amt = item1.RentAmount;
                        if (TotalDays >= To)
                        {
                            EmptyGR += Amt * ((To - From) + 1);
                            ContWiseGR += Amt * ((To - From) + 1);
                        }
                        else
                        {
                            EmptyGR += Amt * ((TotalDays - From) + 1);
                            ContWiseGR += Amt * ((TotalDays - From) + 1);
                            break;
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty = ContWiseGR - ((this.lstPreContWiseAmount.Count() > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty : 0) : 0));
                });
                var ActualGrEmpty = Math.Round(EmptyGR - (this.lstPreContWiseAmount.Count() > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrEmpty) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrEmpty,
                    Discount = 0M,
                    Taxable = ActualGrEmpty,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty + (ActualGrEmpty * (cgst / 100)) + (ActualGrEmpty * (sgst / 100))) : (ActualGrEmpty + (ActualGrEmpty * (igst / 100)))) : ActualGrEmpty
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrEmpty + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2)))) : ActualGrEmpty
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "ECDelv", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0,
                        SGSTAmt = 0,
                        IGSTAmt = 0,
                        Total = 0
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.EmptyContainerDelivery == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if (item1.OperationCode == "6")
                        {
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });

                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                   .ToList().ForEach(c =>
                                   {
                                       this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                       {
                                           CFSCode = c.CFSCode,
                                           Clause = item1.OperationCode,
                                           CommodityType = c.CargoType.ToString(),
                                           Size = c.Size,
                                           Rate = item1.RateCWC
                                       });
                                   });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                                .ToList().ForEach(c =>
                                                {
                                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                                    {
                                                        CFSCode = c.CFSCode,
                                                        Clause = item1.OperationCode,
                                                        CommodityType = c.CargoType.ToString(),
                                                        Size = c.Size,
                                                        Rate = item1.RateCWC
                                                    });
                                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                                //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateBTT(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "BTT", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                var Insurance = 0M;
                this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                        - (DateTime)item.CartingDate).TotalDays + 1);
                    Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                });
                var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualInsurance,
                    Discount = 0,
                    Taxable = ActualInsurance,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.BTT == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList())
                    : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.Where(x => x.Size == "20").ToList())
                    {
                        if (item1.OperationCode == "5" || item1.OperationCode == "6" || item1.OperationCode == "11" || item1.OperationCode == "12")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                                 .ToList().ForEach(c =>
                                                 {
                                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                                     {
                                                         CFSCode = c.CFSCode,
                                                         Clause = item1.OperationCode,
                                                         CommodityType = c.CargoType.ToString(),
                                                         Size = c.Size,
                                                         Rate = item1.RateCWC
                                                     });
                                                 });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size), 2);
                                }
                                //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size), 2);
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "6")
                    {
                        var clzOrder1 = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder1
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateBTTForKol(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");
            //CalculateStorageReeferForKandla(objGenericCharges, "IMPDelv", GSTType);
            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "BTT", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                var Insurance = 0M;
                this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                        - (DateTime)item.CartingDate).TotalDays + 1);
                    Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                });
                var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualInsurance,
                    Discount = 0,
                    Taxable = ActualInsurance,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.BTT == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList())
                    : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.Where(x => x.Size == "20").ToList())
                    {
                        if (item1.OperationCode == "5" || item1.OperationCode == "6" || item1.OperationCode == "11" || item1.OperationCode == "12")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                                 .ToList().ForEach(c =>
                                                 {
                                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                                     {
                                                         CFSCode = c.CFSCode,
                                                         Clause = item1.OperationCode,
                                                         CommodityType = c.CargoType.ToString(),
                                                         Size = c.Size,
                                                         Rate = item1.RateCWC
                                                     });
                                                 });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.CargoType == item1.CommodityType).Sum(o => o.Clauseweight)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 3), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size), 2);
                                }
                                //HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size), 2);
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "6")
                    {
                        var clzOrder1 = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder1
                        });
                    }
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }

                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateLoadedContainerExport(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = (this.PartyStateCode == CompStateCode) || (this.PartyStateCode == "");

            //Entry Fees Calculation
            #region Entry Fees
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                var TotalEntryFees = 0M;

                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = Math.Round(objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate, 2);
                });
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = TotalEntryFees,
                    Discount = 0,
                    Taxable = TotalEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (TotalEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (TotalEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (TotalEntryFees * (igst / 100))) : 0, 2)))) : TotalEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                var EmptyGR = 0M;
                /*
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32(((DateTime)item.StuffingDate - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    //var TotalDays = Convert.ToInt32((DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    //                - DateTime.ParseExact(this.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    var ContWiseGR = 0M;
                    foreach (var item1 in objGenericCharges.EmptyGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                    {
                        var From = item1.DaysRangeFrom;
                        var To = item1.DaysRangeTo;
                        var Amt = item1.RentAmount;
                        if (TotalDays >= To)
                        {
                            EmptyGR += Amt * ((To - From) + 1);
                        }
                        else
                        {
                            EmptyGR += Amt * ((TotalDays - From) + 1);
                            ContWiseGR = EmptyGR;
                            break;
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty = ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty : 0) : 0));
                });*/
                var ActualGrEmpty = Math.Round(EmptyGR, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrEmpty) : 0);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrEmpty,
                    Discount = 0M,
                    Taxable = ActualGrEmpty,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty + (ActualGrEmpty * (cgst / 100)) + (ActualGrEmpty * (sgst / 100))) : (ActualGrEmpty + (ActualGrEmpty * (igst / 100)))) : ActualGrEmpty
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrEmpty + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2)))) : ActualGrEmpty
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var LoadedGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    var ContWiseGR = 0M;
                    if (System.Web.HttpContext.Current.Session["BranchId"].ToString() == "1")
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType && o.ImpExp == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in objGenericCharges.LoadedGroundRent
                            .Where(o => o.Size == item.Size && o.CommodityType == item.CargoType)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;
                            var Amt = item1.RentAmount;
                            if (TotalDays >= To)
                            {
                                LoadedGR += Amt * ((To - From) + 1);
                                ContWiseGR += Amt * ((To - From) + 1);
                            }
                            else
                            {
                                LoadedGR += Amt * ((TotalDays - From) + 1);
                                ContWiseGR += Amt * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded = ContWiseGR -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrLoaded : 0) : 0));
                });
                var ActualGrLoaded = Math.Round(LoadedGR - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrLoaded) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrLoaded,
                    Discount = 0,
                    Taxable = ActualGrLoaded,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded + (ActualGrLoaded * (cgst / 100)) + (ActualGrLoaded * (sgst / 100))) : (ActualGrLoaded + (ActualGrLoaded * (igst / 100)))) : ActualGrLoaded
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrLoaded * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrLoaded + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrLoaded * (igst / 100))) : 0, 2)))) : ActualGrLoaded
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "EXP", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                            - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        ReeferChrg += TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = (TotalDays * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0));
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    /*
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32(((DateTime)item.StuffingDate - (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });*/
                    var ActualStorage = Math.Round(StorageChrg, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                var Insurance = 0M;
                this.lstStorPostPaymentCont.Where(o => o.Insured == 0).ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                        - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                    Insurance += item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0;
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge = (item.Insured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0) -
                    ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).InsuranceCharge : 0) : 0));
                });
                var ActualInsurance = Math.Round(Math.Round(Insurance, 2) - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.InsuranceCharge) : 0), 2);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualInsurance,
                    Discount = 0,
                    Taxable = ActualInsurance,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance + (ActualInsurance * (cgst / 100)) + (ActualInsurance * (sgst / 100))) : (ActualInsurance + (ActualInsurance * (igst / 100)))) : ActualInsurance
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualInsurance * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualInsurance + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualInsurance * (igst / 100))) : 0, 2)))) : ActualInsurance
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                var WeighmentCharges = 0M;
                /*
                objGenericCharges.WeighmentCharge.ToList().ForEach(item =>
                {
                    WeighmentCharges += item.ContainerRate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize);
                });
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    if (this.InvoiceType == "Tax")
                    {
                        var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                        item.WeighmentCharge = objGenericCharges.WeighmentCharge.FirstOrDefault(o => o.ContainerSize == CurCFSSize).ContainerRate -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).WeighmentCharge : 0) : 0));
                    }
                });*/

                var ActualWeighment = 0M;
                if (this.InvoiceType == "Tax")
                    ActualWeighment = Math.Round(WeighmentCharges - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.WeighmentCharge) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualWeighment,
                    Discount = 0,
                    Taxable = ActualWeighment,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment + (ActualWeighment * (cgst / 100)) + (ActualWeighment * (sgst / 100))) : (ActualWeighment + (ActualWeighment * (igst / 100)))) : ActualWeighment
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualWeighment * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualWeighment + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualWeighment * (igst / 100))) : 0, 2)))) : ActualWeighment
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.LoadedExport == 1).Select(o => new { Clause = o.Clause });
                //var ApplicableHT = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5" || item1.OperationCode == "6")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //

                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 2), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    if (item.Key == "8")
                    {
                        var clzOrder1 = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder1
                        });
                    }

                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end

                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });

                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        private void CalculateStorageReeferForKandla(GenericChargesModel objGenericCharges, string Phase, bool GSTType)
        {
            if (Phase == "IMPYard")
            {
                #region Reefer
                try
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        var TotalHours = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHours) / Convert.ToDecimal(4));

                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPCargo")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPDest")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPDelv")
            {
                #region Reefer
                try
                {
                    if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)//KANDLA
                    {
                        var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                        var ReeferChrg = 0M;
                        this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                        {
                            //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                            //var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            //              - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);

                            var TotalHrs = Math.Ceiling(Math.Abs((Convert.ToDateTime(DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm")) -
                            (DateTime)item.DestuffingDate).TotalHours + 1));

                            var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                            ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                        });
                        var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = "3",
                            ChargeName = "Plug-in Charges for Reefer Container",
                            ChargeType = "CWC",
                            SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = ActualReefer,
                            Discount = 0,
                            Taxable = ActualReefer,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                                (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                                (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                        });
                    }
                    else
                    {
                        var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = "3",
                            ChargeName = "Plug-in Charges for Reefer Container",
                            ChargeType = "CWC",
                            SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = 0M,
                            Discount = 0,
                            Taxable = 0M,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = 0M,
                            SGSTAmt = 0M,
                            IGSTAmt = 0M,
                            Total = 0M
                        });
                    }
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            Convert.ToDateTime(item.DestuffingDate).Date).TotalDays + 1);
                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                        var CargoWt = item.GrossWt / 1000;
                        var CargoArea = item.SpaceOccupied;
                        var WtAmt = 0M;
                        var AreaAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 1)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;

                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 1 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 1 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMPerWeek))), 2);

                        StorageChrg += Math.Round((WtAmt > AreaAmt) ? WtAmt : AreaAmt, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(((WtAmt > AreaAmt) ? WtAmt : AreaAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "EXP")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        /*
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        */
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                       - (DateTime)item.StuffingDate).TotalHours + 1);

                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));

                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.StuffingDate).Date - (DateTime)item.CartingDate).TotalDays + 1);
                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                        var CargoWt = item.GrossWt / 1000;
                        var CargoArea = item.SpaceOccupied;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var AreaAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;

                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMPerWeek))), 2);
                        VolAmt = Math.Round((CargoVol * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerMonth)))
                                + (CargoVol * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerWeek))), 2);
                        if (WtAmt > AreaAmt)
                        {
                            if (WtAmt > VolAmt)
                            {
                                StorageChrg += WtAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(WtAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                            else
                            {
                                StorageChrg += VolAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                        }
                        else if (AreaAmt > VolAmt)
                        {
                            StorageChrg += AreaAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(AreaAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }
                        else
                        {
                            StorageChrg += VolAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }

                        //StorageChrg += (WtAmt > AreaAmt) ? WtAmt : AreaAmt;

                        //this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = (WtAmt > AreaAmt) ? WtAmt : AreaAmt;
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "ECDelv")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "BTT")
            {

                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            (DateTime)item.CartingDate).TotalDays + 1);

                        var CargoWt = item.GrossWt / 1000;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;
                            var CBMRate = item1.RateCubMeterPerDay;
                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                VolAmt += CargoVol * CBMRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }
                        StorageChrg += (WtAmt > VolAmt) ? WtAmt : VolAmt;
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = ((WtAmt > VolAmt) ? WtAmt : VolAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0));
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage + (ActualStorage * (cgst / 100)) + (ActualStorage * (sgst / 100))) : (ActualStorage + (ActualStorage * (igst / 100)))) : ActualStorage
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion


                // Old code below

                //#region Reefer
                //try
                //{
                //    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                //    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                //    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);

                //    this.lstPostPaymentChrg.Add(new PostPaymentCharge()
                //    {
                //        ChargeId = this.lstPostPaymentChrg.Count + 1,
                //        Clause = "3",
                //        ChargeName = "Plug-in Charges for Reefer Container",
                //        ChargeType = "CWC",
                //        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                //        Quantity = 0,
                //        Rate = 0M,
                //        Amount = 0M,
                //        Discount = 0,
                //        Taxable = 0M,
                //        CGSTPer = cgst,
                //        SGSTPer = sgst,
                //        IGSTPer = igst,
                //        CGSTAmt = 0M,
                //        SGSTAmt = 0M,
                //        IGSTAmt = 0M,
                //        Total = 0M
                //    });
                //}
                //catch (Exception e)
                //{

                //}
                //#endregion

                //#region Storage
                //try
                //{
                //    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                //    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                //    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                //    var StorageChrg = 0M;
                //    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                //      {
                //        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)


                //        //var TotalDays = Convert.ToInt32(((DateTime)item.CartingDate -
                //        //    DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                //       var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                //            (DateTime)item.CartingDate).TotalDays + 1);

                //          var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                //        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                //        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                //        var CargoWt = item.GrossWt / 1000;
                //        var CargoArea = item.SpaceOccupied;
                //        var CargoVol = item.StuffCUM;
                //        var WtAmt = 0M;
                //        var AreaAmt = 0M;
                //        var VolAmt = 0M;
                //        foreach (var item1 in objGenericCharges.StorageRent
                //            .Where(o => o.WarehouseType == 2)
                //            .OrderBy(o => o.DaysRangeFrom)
                //            .ToList())
                //        {
                //            var MTRate = item1.RateMetricTonePerDay;

                //            var From = item1.DaysRangeFrom;
                //            var To = item1.DaysRangeTo;

                //            if (TotalDays >= To)
                //            {
                //                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                //            }
                //            else
                //            {
                //                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                //                break;
                //            }
                //        }

                //        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                //                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                //                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                //                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                //                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                //                                                    .FirstOrDefault().RateSqMPerWeek))), 2);
                //        VolAmt = Math.Round((CargoVol * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                //                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                //                                                    .FirstOrDefault().RateCubMeterPerMonth)))
                //                + (CargoVol * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                //                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                //                                                    .FirstOrDefault().RateCubMeterPerWeek))), 2);
                //        if (WtAmt > AreaAmt)
                //        {
                //            if (WtAmt > VolAmt)
                //            {
                //                StorageChrg += WtAmt;
                //                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(WtAmt -
                //                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                //            }
                //            else
                //            {
                //                StorageChrg += VolAmt;
                //                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                //                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                //            }
                //        }
                //        else if (AreaAmt > VolAmt)
                //        {
                //            StorageChrg += AreaAmt;
                //            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(AreaAmt -
                //                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                //        }
                //        else
                //        {
                //            StorageChrg += VolAmt;
                //            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                //                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                //        }

                //        //StorageChrg += (WtAmt > AreaAmt) ? WtAmt : AreaAmt;

                //        //this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = (WtAmt > AreaAmt) ? WtAmt : AreaAmt;
                //    });
                //    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                //    this.lstPostPaymentChrg.Add(new PostPaymentCharge()
                //    {
                //        ChargeId = this.lstPostPaymentChrg.Count + 1,
                //        Clause = "4",
                //        ChargeName = "Storage Charges",
                //        ChargeType = "CWC",
                //        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                //        Quantity = 0,
                //        Rate = 0M,
                //        Amount = ActualStorage,
                //        Discount = 0M,
                //        Taxable = ActualStorage,
                //        CGSTPer = cgst,
                //        SGSTPer = sgst,
                //        IGSTPer = igst,
                //        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                //        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                //        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                //        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                //            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                //            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                //            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                //    });
                //}
                //catch (Exception e)
                //{

                //}
                //#endregion
            }
            else
            {

            }
        }

        private void CalculateStorageReeferForKandla(GenericChargesModel objGenericCharges, string Phase, bool GSTType, int GodownTypeId)
        {
            if (Phase == "IMPYard")
            {
                #region Reefer
                try
                {

                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        var TotalHours = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHours) / Convert.ToDecimal(4));
                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPCargo")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge =
                            Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPDest")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "IMPDelv")
            {
                #region Reefer
                try
                {
                    if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)//KANDLA
                    {
                        var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                        var ReeferChrg = 0M;
                        this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                        {
                            //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                            //var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            //              - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);

                            var TotalHrs = Math.Ceiling(Math.Abs((Convert.ToDateTime(DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm")) -
                            (DateTime)item.DestuffingDate).TotalHours + 1));

                            var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));
                            ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                            ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                        });
                        var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = "3",
                            ChargeName = "Plug-in Charges for Reefer Container",
                            ChargeType = "CWC",
                            SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = ActualReefer,
                            Discount = 0,
                            Taxable = ActualReefer,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                            Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                                (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                                (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                        });
                    }
                    else
                    {
                        var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                        var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                        this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                        {
                            ChargeId = this.lstPostPaymentChrg.Count + 1,
                            Clause = "3",
                            ChargeName = "Plug-in Charges for Reefer Container",
                            ChargeType = "CWC",
                            SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = 0M,
                            Discount = 0,
                            Taxable = 0M,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = 0M,
                            SGSTAmt = 0M,
                            IGSTAmt = 0M,
                            Total = 0M
                        });
                    }
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")) -
                            Convert.ToDateTime(item.DestuffingDate).Date).TotalDays + 1);
                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                        var CargoWt = item.GrossWt / 1000;
                        var CargoArea = item.SpaceOccupied;
                        var WtAmt = 0M;
                        var AreaAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == GodownTypeId)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;

                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == GodownTypeId && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == GodownTypeId && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMPerWeek))), 2);

                        StorageChrg += Math.Round((WtAmt > AreaAmt) ? WtAmt : AreaAmt, 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(((WtAmt > AreaAmt) ? WtAmt : AreaAmt) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "EXP")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    var ReeferChrg = 0M;
                    this.lstPostPaymentCont.Where(o => o.Reefer == 1).ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        //var TotalHrs = Math.Ceiling(Math.Abs((Convert.ToDateTime(DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm")) -
                        //(DateTime)item.DestuffingDate).TotalHours + 1));
                        /*
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                        - DateTime.ParseExact(item.ArrivalDate + " " + item.ArrivalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).TotalHours);
                        */
                        var TotalHrs = Math.Ceiling((DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                                       - (DateTime)item.StuffingDate).TotalHours + 1);


                        var ChargableHrs = Math.Ceiling(Convert.ToDecimal(TotalHrs) / Convert.ToDecimal(4));

                        ReeferChrg += Math.Round(ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge), 2);
                        this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge = Math.Round((ChargableHrs * (objGenericCharges.Reefer.SingleOrDefault(o => o.ContainerSize == item.Size).ElectricityCharge)) -
                        ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).ReeferCharge : 0) : 0)), 2);
                    });
                    var ActualReefer = Math.Round(ReeferChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.ReeferCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualReefer,
                        Discount = 0,
                        Taxable = ActualReefer,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualReefer * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualReefer + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualReefer * (igst / 100))) : 0, 2)))) : ActualReefer
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32((Convert.ToDateTime(item.StuffingDate).Date - (DateTime)item.CartingDate).TotalDays + 1);
                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                        var CargoWt = item.GrossWt / 1000;
                        var CargoArea = item.SpaceOccupied;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var AreaAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;

                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMPerWeek))), 2);
                        VolAmt = Math.Round((CargoVol * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerMonth)))
                                + (CargoVol * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerWeek))), 2);
                        if (WtAmt > AreaAmt)
                        {
                            if (WtAmt > VolAmt)
                            {
                                StorageChrg += WtAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(WtAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                            else
                            {
                                StorageChrg += VolAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                        }
                        else if (AreaAmt > VolAmt)
                        {
                            StorageChrg += AreaAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(AreaAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }
                        else
                        {
                            StorageChrg += VolAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }

                        //StorageChrg += (WtAmt > AreaAmt) ? WtAmt : AreaAmt;

                        //this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = (WtAmt > AreaAmt) ? WtAmt : AreaAmt;
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "ECDelv")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else if (Phase == "BTT")
            {
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception e)
                {

                }
                #endregion

                #region Storage
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var StorageChrg = 0M;
                    this.lstStorPostPaymentCont.ToList().ForEach(item =>
                    {
                        //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        var TotalDays = Convert.ToInt32(((DateTime)item.CartingDate -
                            DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7));
                        var TotalMonths = Math.Floor(Convert.ToDecimal(TotalWeeks) / Convert.ToDecimal(4));
                        TotalWeeks = Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(TotalWeeks) - Convert.ToDecimal(TotalMonths * 4)) / Convert.ToDecimal(7));

                        var CargoWt = item.GrossWt / 1000;
                        var CargoArea = item.SpaceOccupied;
                        var CargoVol = item.StuffCUM;
                        var WtAmt = 0M;
                        var AreaAmt = 0M;
                        var VolAmt = 0M;
                        foreach (var item1 in objGenericCharges.StorageRent
                            .Where(o => o.WarehouseType == 2)
                            .OrderBy(o => o.DaysRangeFrom)
                            .ToList())
                        {
                            var MTRate = item1.RateMetricTonePerDay;

                            var From = item1.DaysRangeFrom;
                            var To = item1.DaysRangeTo;

                            if (TotalDays >= To)
                            {
                                WtAmt += CargoWt * MTRate * ((To - From) + 1);
                            }
                            else
                            {
                                WtAmt += CargoWt * MTRate * ((TotalDays - From) + 1);
                                break;
                            }
                        }

                        AreaAmt = Math.Round((CargoArea * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMeterPerMonth)))
                                + (CargoArea * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateSqMPerWeek))), 2);
                        VolAmt = Math.Round((CargoVol * TotalMonths * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerMonth)))
                                + (CargoVol * TotalWeeks * (Convert.ToDecimal(objGenericCharges.StorageRent
                                                                    .Where(o => o.WarehouseType == 2 && o.CommodityType == item.CargoType)
                                                                    .FirstOrDefault().RateCubMeterPerWeek))), 2);
                        if (WtAmt > AreaAmt)
                        {
                            if (WtAmt > VolAmt)
                            {
                                StorageChrg += WtAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(WtAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                            else
                            {
                                StorageChrg += VolAmt;
                                this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                            }
                        }
                        else if (AreaAmt > VolAmt)
                        {
                            StorageChrg += AreaAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(AreaAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }
                        else
                        {
                            StorageChrg += VolAmt;
                            this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = Math.Round(VolAmt -
                                ((this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge : 0) : 0)), 2);
                        }

                        //StorageChrg += (WtAmt > AreaAmt) ? WtAmt : AreaAmt;

                        //this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).StorageCharge = (WtAmt > AreaAmt) ? WtAmt : AreaAmt;
                    });
                    var ActualStorage = Math.Round(StorageChrg - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.StorageCharge) : 0), 2);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = ActualStorage,
                        Discount = 0M,
                        Taxable = ActualStorage,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                            (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                            (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                    });
                }
                catch (Exception e)
                {

                }
                #endregion
            }
            else
            {

            }
        }
        private void CalculateAuctionInvoice(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;

            #region Storage

            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                var StorageChrg = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm",
                    //    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                    //                - (DateTime)item.DestuffingDate).TotalDays + 1);

                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays);

                    var FileterList = objGenericCharges.LoadedGroundRent
                    .Where(o => o.Size == item.Size && o.ImpExp == this.OperationType && o.CommodityType == item.CargoType)
                    .OrderBy(o => o.DaysRangeFrom)
                    .ToList();

                    if (FileterList.Count > 0)
                    {
                        FileterList[0].RentAmount = FileterList[1].RentAmount;
                    }

                    foreach (var item1 in FileterList)
                    {
                        var From = item1.DaysRangeFrom;
                        var To = item1.DaysRangeTo;
                        var Amt = item1.RentAmount;
                        if (TotalDays >= To)
                        {
                            StorageChrg += Amt * ((To - From) + 1);
                        }
                        else
                        {
                            StorageChrg += Amt * ((TotalDays - From) + 1);
                            break;
                        }
                    }
                });

                var ActualStorage = Math.Round(StorageChrg);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "4",
                    ChargeName = "Storage Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualStorage,
                    Discount = 0M,
                    Taxable = ActualStorage,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualStorage + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2)))) : ActualStorage
                });
            }
            catch (Exception e)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYeardPSCD == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType);
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0,
                        SGSTAmt = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0,
                        IGSTAmt = this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end

                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });

                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }


        private void CalculateEmptyContDeliveryGateOut(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;

            #region Entry Fees
            try
            {
                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = Math.Round(objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0), 2);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);
                var EmptyGR = 0M;
                this.lstPostPaymentCont.ToList().ForEach(item =>
                {
                    //DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    //var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm",
                    //    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                    //                - (DateTime)item.DestuffingDate).TotalDays + 1);

                    var TotalDays = Convert.ToInt32((Convert.ToDateTime(DateTime.ParseExact(this.InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"))
                                    - DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                    var ContWiseGR = 0M;

                    foreach (var item1 in objGenericCharges.EmptyGroundRent
                        .Where(o => o.Size == item.Size && o.CommodityType == 2 && o.ImpExp == 2)
                        .OrderBy(o => o.DaysRangeFrom)
                        .ToList())
                    {
                        var From = item1.DaysRangeFrom;
                        var To = item1.DaysRangeTo;
                        var Amt = item1.RentAmount;
                        if (TotalDays >= To)
                        {
                            EmptyGR += Amt * ((To - From) + 1);
                            ContWiseGR += Amt * ((To - From) + 1);
                        }
                        else
                        {
                            EmptyGR += Amt * ((TotalDays - From) + 1);
                            ContWiseGR += Amt * ((TotalDays - From) + 1);
                            break;
                        }
                    }
                    this.lstContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty = ContWiseGR - ((this.lstPreContWiseAmount.Count() > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).GrEmpty : 0) : 0));
                });
                var ActualGrEmpty = Math.Round(EmptyGR - (this.lstPreContWiseAmount.Count() > 0 ? this.lstPreContWiseAmount.Sum(o => o.GrEmpty) : 0), 2);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = ActualGrEmpty,
                    Discount = 0M,
                    Taxable = ActualGrEmpty,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2),
                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty + (ActualGrEmpty * (cgst / 100)) + (ActualGrEmpty * (sgst / 100))) : (ActualGrEmpty + (ActualGrEmpty * (igst / 100)))) : ActualGrEmpty
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualGrEmpty * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualGrEmpty + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualGrEmpty * (igst / 100))) : 0, 2)))) : ActualGrEmpty
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                CalculateStorageReeferForKandla(objGenericCharges, "ECDelv", GSTType);
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0,
                        SGSTAmt = 0,
                        IGSTAmt = 0,
                        Total = 0
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.EmptyContainerDelivery == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void CalculateEmptyContDeliveryGateIn(GenericChargesModel objGenericCharges)
        {
            #region Company Details
            var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
            this.ROAddress = _ROAddress;
            var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
            this.CompanyId = _CompanyId ?? (int)_CompanyId;
            var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
            this.CompanyName = _CompanyName;
            var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
            this.CompanyShortName = _CompanyShortName;
            var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
            this.CompanyAddress = _CompanyAddress;
            var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
            this.PhoneNo = _PhoneNo;
            var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
            this.FaxNumber = _FaxNumber;
            var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
            this.EmailAddress = _EmailAddress;
            var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
            this.StateId = _StateId ?? (int)_StateId;
            var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
            this.CityId = _CityId ?? (int)_CityId;

            var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
            this.CompGST = CompGST;
            var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
            this.CompStateCode = CompStateCode;
            var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
            this.CompPAN = CompPAN;
            #endregion

            var GSTType = this.PartyStateCode == CompStateCode;

            #region Entry Fees
            try
            {


                var TotalEntryFees = 0M;
                objGenericCharges.EntryFees.ToList().ForEach(item =>
                {
                    TotalEntryFees += Math.Round(item.Rate * this.lstPostPaymentCont.Count(o => o.Size == item.ContainerSize), 2);
                });
                //Container Wise Entry Fee
                this.lstContWiseAmount.ToList().ForEach(item =>
                {
                    var CurCFSSize = this.lstPostPaymentCont.FirstOrDefault(m => m.CFSCode == item.CFSCode).Size;
                    item.EntryFee = Math.Round(objGenericCharges.EntryFees.FirstOrDefault(o => o.ContainerSize == CurCFSSize).Rate -
                        (this.lstPreContWiseAmount.Count > 0 ? (this.lstPreContWiseAmount.Any(o => o.CFSCode == item.CFSCode) ? this.lstPreContWiseAmount.FirstOrDefault(o => o.CFSCode == item.CFSCode).EntryFee : 0) : 0), 2);
                });
                var ActualEntryFees = Math.Round(TotalEntryFees - (this.lstPreContWiseAmount.Count > 0 ? this.lstPreContWiseAmount.Sum(o => o.EntryFee) : 0), 2);
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EntryFees.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1",
                    ChargeName = "Entry Fee",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EntryFees.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0,
                    Amount = ActualEntryFees,
                    Discount = 0,
                    Taxable = ActualEntryFees,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2),
                    SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2),
                    IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2),
                    Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees * (sgst / 100)) : 0) : 0, 2))) :
                        (ActualEntryFees + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualEntryFees * (igst / 100))) : 0, 2)))) : ActualEntryFees
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Custom Revenue
            #region Custom Revenue
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.CustomRevenue.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "1A",
                    ChargeName = "Custom Revenue Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.CustomRevenue.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Empty
            #region Empty GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(1)",
                    ChargeName = "Ground Rent Empty",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.EmptyGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Ground Rent Loaded
            #region Loaded GR
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "2(2)",
                    ChargeName = "Ground Rent Loaded",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.LoadedGroundRent.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
            {
                //CalculateStorageReeferForKandla(objGenericCharges, "ECDelv", GSTType);
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0,
                        SGSTAmt = 0,
                        IGSTAmt = 0,
                        Total = 0
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }
            else
            {
                //Reefer
                #region Reefer
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.Reefer.FirstOrDefault().SacCode).Gst);
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "3",
                        ChargeName = "Plug-in Charges for Reefer Container",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.Reefer.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0M,
                        SGSTAmt = 0M,
                        IGSTAmt = 0M,
                        Total = 0M
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Storage Charge
                #region Storage Charge
                try
                {
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);

                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = "4",
                        ChargeName = "Storage Charges",
                        ChargeType = "CWC",
                        SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,
                        Amount = 0M,
                        Discount = 0M,
                        Taxable = 0M,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = 0,
                        SGSTAmt = 0,
                        IGSTAmt = 0,
                        Total = 0
                    });
                }
                catch (Exception ex)
                {

                }
                #endregion
            }

            //Insurance
            #region Insurance
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);

                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "5",
                    ChargeName = "Insurance Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Other
            #region Other Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "6",
                    ChargeName = "Others",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Port Charges
            #region Port Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.PortCharges.FirstOrDefault().SACCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "7",
                    ChargeName = "Port Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.PortCharges.FirstOrDefault().SACCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0M,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //Weighment
            #region Weighment
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode).Gst);
                this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                {
                    ChargeId = this.lstPostPaymentChrg.Count + 1,
                    Clause = "8",
                    ChargeName = "Weighment Charges",
                    ChargeType = "CWC",
                    SACCode = objGenericCharges.WeighmentCharge.FirstOrDefault().SacCode,
                    Quantity = 0,
                    Rate = 0M,
                    Amount = 0M,
                    Discount = 0,
                    Taxable = 0M,
                    CGSTPer = cgst,
                    SGSTPer = sgst,
                    IGSTPer = igst,
                    CGSTAmt = 0M,
                    SGSTAmt = 0M,
                    IGSTAmt = 0M,
                    Total = 0M
                });
            }
            catch (Exception ex)
            {

            }
            #endregion

            //H & T Charges
            #region H&T Charges
            try
            {
                var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.EmptyContainerDelivery == 1).Select(o => new { Clause = o.Clause });
                var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                var HTCharges = 0M;
                var TotalQuantity = 0M;
                var RateForHT = 0M;
                lstCfsCodewiseRateHT.Clear();
                foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                {
                    foreach (var item1 in item.ToList())
                    {
                        if (item1.OperationCode == "5")
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                            //
                            HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(this.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                        }
                        else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                        {
                            //subir
                            this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                            //
                            HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                        }
                        else
                        {
                            if (item1.OperationType == 5)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 4)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                               .ToList().ForEach(c =>
                               {
                                   this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                   {
                                       CFSCode = c.CFSCode,
                                       Clause = item1.OperationCode,
                                       CommodityType = c.CargoType.ToString(),
                                       Size = c.Size,
                                       Rate = item1.RateCWC
                                   });
                               });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                            }
                            else if (item1.OperationType == 6)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                            }
                            else if (item1.OperationType == 7)
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                 .ToList().ForEach(c =>
                                 {
                                     this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                     {
                                         CFSCode = c.CFSCode,
                                         Clause = item1.OperationCode,
                                         CommodityType = c.CargoType.ToString(),
                                         Size = c.Size,
                                         Rate = item1.RateCWC
                                     });
                                 });
                                //
                                HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                            }
                            else
                            {
                                //subir
                                this.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                .ToList().ForEach(c =>
                                {
                                    this.lstCfsCodewiseRateHT.Add(new WljCfsCodewiseRateHT
                                    {
                                        CFSCode = c.CFSCode,
                                        Clause = item1.OperationCode,
                                        CommodityType = c.CargoType.ToString(),
                                        Size = c.Size,
                                        Rate = item1.RateCWC
                                    });
                                });
                                //
                                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && item1.ContainerType == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * this.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                    }
                    var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    this.lstPostPaymentChrg.Add(new WljPostPaymentCharge()
                    {
                        ChargeId = this.lstPostPaymentChrg.Count + 1,
                        Clause = item.Key,
                        ChargeName = item.FirstOrDefault().OperationSDesc,
                        ChargeType = "HT",
                        SACCode = item.FirstOrDefault().SacCode,
                        Quantity = 0,
                        Rate = 0M,// RateForHT,//
                        Amount = HTCharges,
                        Discount = 0,
                        Taxable = HTCharges,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                        Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                        (HTCharges + (Math.Round(this.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                        ClauseOrder = clzOrder
                    });
                    HTCharges = 0M;
                    TotalQuantity = 0M;
                    RateForHT = 0M;
                }
                //subir
                HttpContext.Current.Session["lstCfsCodewiseRateHT"] = lstCfsCodewiseRateHT;
                //end
                var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                actual.ForEach(item =>
                {
                    if (this.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    {
                        this.ActualApplicable.Add(item.OperationCode);
                    }
                });
                var sortedString = JsonConvert.SerializeObject(this.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                this.lstPostPaymentChrg.Clear();
                this.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<WljPostPaymentCharge>>(sortedString);
            }
            catch (Exception ex)
            {

            }
            #endregion
        }


    }
    public class WljPostPaymentContainer
    {
        public string CFSCode { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Reefer { get; set; } = 0;
        public int Insured { get; set; } = 0;
        public int RMS { get; set; } = 0;
        public int HeavyScrap { get; set; } = 0;
        public int AppraisementPerct { get; set; } = 0;
        public int CargoType { get; set; } = 0;
        public string ArrivalDate { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public DateTime? CartingDate { get; set; } = null;
        public DateTime? StuffingDate { get; set; } = null;
        public DateTime? DestuffingDate { get; set; } = null;
        public int NoOfPackages { get; set; } = 0;
        public decimal GrossWt { get; set; } = 0M;
        public decimal WtPerUnit { get; set; } = 0M;
        public decimal SpaceOccupied { get; set; } = 0M;
        public string SpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int DeliveryType { get; set; } = 0;
        public decimal StuffCUM { get; set; } = 0M;
        public string LCLFCL { get; set; }

        public decimal NoOfPackagesDel { get; set; } = 0M;
        public int ISODC { get; set; } = 0;
        public decimal Clauseweight { get; set; } = 0M;
        //// subir
        //public string cfscodeWiseHTRate { get; set; }
        ////
    }
    public class WljPostPaymentCharge
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;
        public decimal Discount { get; set; } = 0M;
        public decimal Taxable { get; set; } = 0M;
        public decimal IGSTPer { get; set; } = 0M;
        public decimal IGSTAmt { get; set; } = 0M;
        public decimal CGSTPer { get; set; } = 0M;
        public decimal CGSTAmt { get; set; } = 0M;
        public decimal SGSTPer { get; set; } = 0M;
        public decimal SGSTAmt { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public decimal ActualFullCharge { get; set; } = 0M;
        public int OperationId { get; set; }
    }
    public class WljContainerWiseAmount
    {
        public int InvoiceId { get; set; }
        public int ContainerId { get; set; }
        public string CFSCode { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public decimal EntryFee { get; set; } = 0M;
        public decimal CstmRevenue { get; set; } = 0M;
        public decimal GrEmpty { get; set; } = 0M;
        public decimal GrLoaded { get; set; } = 0M;
        public decimal ReeferCharge { get; set; } = 0M;
        public decimal StorageCharge { get; set; } = 0M;
        public decimal InsuranceCharge { get; set; } = 0M;
        public decimal PortCharge { get; set; } = 0M;
        public decimal WeighmentCharge { get; set; } = 0M;
    }



    public class WljClauseTypeForHT
    {
        public string OperationCode { get; set; }
        public string ClauseOrder { get; set; }
        public string CommodityType { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }
        public string OperationDesc { get; set; }
        public string SacCode { get; set; }
        public string RateCWC { get; set; }
        public string ChargeType { get; set; }

        public string Quantity { get; set; }
    }
    public class WljCfsCodewiseRateHT
    {
        public string CFSCode { get; set; } = string.Empty;
        public string Clause { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string CommodityType { get; set; } = string.Empty;
        public decimal Rate { get; set; } = 0;
    }

    public class WljPreInvoiceContainer
    {
        public string ApproveOn { get; set; } = string.Empty;
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public int CargoType { get; set; }
        public string LineNo { get; set; } = string.Empty;
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string Size { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal WtPerPack { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public int Reefer { get; set; }
        public int RMS { get; set; }
        public int Insured { get; set; }
        public int HeavyScrap { get; set; }
        public int AppraisementPerct { get; set; }
        public int DeliveryType { get; set; }
        public string ShippingLineName { get; set; }
        public string CHAName { get; set; }
        public string ImporterExporter { get; set; }
        public string DestuffingDate { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public decimal SpaceOccupied { get; set; }
        public string SpaceOccupiedUnit { get; set; }
        public decimal StuffCUM { get; set; } = 0M;
        public int OperationType { get; set; }
        public string LCLFCL { get; set; }

        public decimal NoOfPackagesDel { get; set; }

        public decimal Clauseweight { get; set; }
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public int ISODC { get; set; } = 0;
    }

    public class WljOperationCFSCodeWiseAmount
    {
        public int InvoiceId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Clause { get; set; }
        public string BOLNo { get; set; }
    }
}
