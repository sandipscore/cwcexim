using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class HDB_AuctionInvoiceRepository
    {

        private DatabaseResponse _DBResponse;

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }


        public void GetBIDListForInvoice(int BidID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BidID", MySqlDbType = MySqlDbType.Int32, Value = BidID });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLShipBillForInvoice", CommandType.StoredProcedure, DParam);
            List<HDB_EMDReceived> lstBidDetails = new List<HDB_EMDReceived>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new HDB_EMDReceived
                    {
                        BIDId = Convert.ToInt32(Result["BidIdHdr"]),
                        BIDNo = Convert.ToString(Result["BidNo"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        OBL = Convert.ToString(Result["OBL"]),
                        ShippBillNo = Convert.ToString(Result["ShippBillNo"]),
                        BIDDate = Convert.ToString(Result["OBL"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBidDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetBIDDetailsForInvoice(int BIDId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BidID", MySqlDbType = MySqlDbType.Int32, Value = BIDId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLShipBillForInvoice", CommandType.StoredProcedure, DParam);
            var model = new HDB_AuctionInvoice();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.BIDId = Convert.ToInt32(Result["BidIdHdr"]);
                    model.BIDNo = Convert.ToString(Result["BidNo"]);
                    model.BidDate = Convert.ToString(Result["BidDate"]);
                    model.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    model.OBL = Convert.ToString(Result["OBL"]);
                    model.ShippingBill = Convert.ToString(Result["ShippBillNo"]);
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Convert.ToString(Result["PartyName"]);
                    model.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                    model.GSTNo = Convert.ToString(Result["GSTNo"]);
                    model.State = Convert.ToString(Result["StateName"]);
                    model.StateCode = Convert.ToString(Result["GstStateCode"]);
                    model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceType = "Tax";
                    model.BidAmount = Convert.ToDecimal(Result["BidAmount"]);
                    model.EmdAmount = Convert.ToDecimal(Result["EmdAmount"]);
                    model.AdvanceAmount = Convert.ToDecimal(Result["AdvanceAmount"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        public void GetAuctionCharges(int BidId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, decimal CustomDuty, decimal OT, decimal ValuersCharge, decimal AuctionCharge, decimal MISC,string AssesmentType, decimal AuctionHandling,string ExportUnder)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_BidIdHdr", MySqlDbType = MySqlDbType.Int32, Value = BidId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_FreeUpto", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FreeUpTo).ToString("yyyy-MM-dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "in_HSNCode", MySqlDbType = MySqlDbType.String, Value = HSNCode });
                LstParam.Add(new MySqlParameter { ParameterName = "in_CustomDuty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(CustomDuty) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_OT", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(OT) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ValuersCharge", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ValuersCharge) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionCharge", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(AuctionCharge) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_GST", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(MISC) });
                LstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentType", MySqlDbType = MySqlDbType.VarChar, Value = AssesmentType });
                LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionHandling", MySqlDbType = MySqlDbType.Decimal, Value = AuctionHandling });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetAuctionInvoiceCharges", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                HDB_AuctionInvoice objAuctionInvoice = new HDB_AuctionInvoice();
                List<PostPaymentCharge> lstPostPaymentChrg = new List<PostPaymentCharge>();

                if (Result != null && Result.Tables.Count > 0)
                {

                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = lstPostPaymentChrg.Count + 1,
                                OperationId = Convert.ToInt32(dr["OperationId"]),
                                Clause = "",
                                ChargeName = Convert.ToString(dr["ChargeName"]),
                                ChargeType = Convert.ToString(dr["ChargeType"]),
                                SACCode = Convert.ToString(dr["SACCode"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Rate = Convert.ToDecimal(dr["Rate"]),
                                Amount = Convert.ToDecimal(dr["Amount"]),
                                Discount = 0,
                                Taxable = 0,
                                CGSTPer = Convert.ToDecimal(dr["CGSTPer"]),
                                SGSTPer = Convert.ToDecimal(dr["SGSTPer"]),
                                IGSTPer = Convert.ToDecimal(dr["IGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                                SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                                IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                                Total = Convert.ToDecimal(dr["Total"]),
                                /// ActualFullCharge = Convert.ToDecimal(dr["ActualFullCharge"]),
                            });
                        }

                        if (lstPostPaymentChrg.Count > 0)
                        {
                            objAuctionInvoice.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstPostPaymentChrg).ToString();
                        }

                    }

                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        objAuctionInvoice.PartySDBalance = Convert.ToDecimal(Result.Tables[1].Rows[0]["SDBalance"]);
                        objAuctionInvoice.TotalNoOfPackages = Convert.ToInt32(Result.Tables[1].Rows[0]["NoOfPackages"]);
                        objAuctionInvoice.TotalGrossWt = Convert.ToDecimal(Result.Tables[1].Rows[0]["Weight"]);
                        objAuctionInvoice.Remarks = Convert.ToString(Result.Tables[1].Rows[0]["Remarks"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objAuctionInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void AddEditInvoice(HDB_AuctionInvoice objAuctionInvoice)
        {
            int GeneratedInvoiceId = 0;
            string GeneratedInvoiceNo = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            string RefNo = "";
            if (objAuctionInvoice.OBL != null)
            {
                RefNo = objAuctionInvoice.OBL;
            }
            else if (objAuctionInvoice.ShippingBill != null)
            {
                RefNo = objAuctionInvoice.ShippingBill;
            }
            else if (objAuctionInvoice.ContainerNo != null)
            {
                RefNo = objAuctionInvoice.ContainerNo;
            }


            DateTime dt = DateTime.ParseExact(objAuctionInvoice.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String InvoiceDate = dt.ToString("yyyy-MM-dd");

            DateTime dtt = DateTime.ParseExact(objAuctionInvoice.BOEDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String BOEDate = dt.ToString("yyyy-MM-dd");

            DateTime DeliveryDateUptodt = DateTime.ParseExact(objAuctionInvoice.DeliveryDateUpto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String DeliveryDateUpto = DeliveryDateUptodt.ToString("yyyy-MM-dd");

            DateTime FreeUptodt = DateTime.ParseExact(objAuctionInvoice.FreeUpto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String FreeUpto = FreeUptodt.ToString("yyyy-MM-dd");

            DateTime AssesmentSheetDatedt = DateTime.ParseExact(objAuctionInvoice.AssesmentSheetDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String AssesmentSheetDate = AssesmentSheetDatedt.ToString("yyyy-MM-dd");




            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.String, Value = "Tax" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objAuctionInvoice.InvoiceDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.BIDId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.BIDNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objAuctionInvoice.ReceiptDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalCGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalSGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalIGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.AllTotal });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.RoundUp });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.InvoiceValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.TotalNoOfPackages });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalGrossWt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.String, Value = "AUC" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_HSNCode", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.HSNCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GST", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.GST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ref", MySqlDbType = MySqlDbType.Text, Value = RefNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEONO", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.BOENo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentType", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.AssesmentType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDesc", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentSheetDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AssesmentSheetDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FreeUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FreeUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomDute", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.CustomDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ot", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.OT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ValuersCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.ValuersCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AuctionCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.AuctionCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MISCExpenses", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.MISCExpenses) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CWCShare", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.CWCShare) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.TDSAmount) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Cess", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.Cess) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EmdAmt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.EmdAdj) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AdvAmt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.AdvAdj) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryValid", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DeliveryDateUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(objAuctionInvoice.SEZ) });
            lstParam.Add(new MySqlParameter { ParameterName = "xmlCashReceipt", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceNo", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceId", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditAuctionAssessmentSheetInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    GeneratedInvoiceNo = Convert.ToString(DParam.Where(x => x.ParameterName == "GeneratedInvoiceNo").Select(x => x.Value).FirstOrDefault());
                    _DBResponse.Data = GeneratedInvoiceNo;
                    _DBResponse.Message = (Result == 1) ? "Auction Invoice Saved Successfully" : "Auction Invoice Updated Successfully";
                    _DBResponse.Status = Result;
                }

                else if (Result == 3)
                {
                    //  GeneratedInvoiceNo = Convert.ToString(DParam.Where(x => x.ParameterName == "GeneratedInvoiceNo").Select(x => x.Value).FirstOrDefault());
                    //  _DBResponse.Data = GeneratedInvoiceNo;
                    _DBResponse.Message = "Invoice can not be generated. SD Balance is low";
                    _DBResponse.Status = Result;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }

        public void AddEditAuctionInvoice(HDB_AuctionInvoice objAuctionInvoice)
        {
            int GeneratedInvoiceId = 0;
            string GeneratedInvoiceNo = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            string RefNo = "";
            if (objAuctionInvoice.OBL != null)
            {
                RefNo = objAuctionInvoice.OBL;
            }
            else if (objAuctionInvoice.ShippingBill != null)
            {
                RefNo = objAuctionInvoice.ShippingBill;
            }
            else if (objAuctionInvoice.ContainerNo != null)
            {
                RefNo = objAuctionInvoice.ContainerNo;
            }


            DateTime dt = DateTime.ParseExact(objAuctionInvoice.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String InvoiceDate = dt.ToString("yyyy-MM-dd");

            DateTime dtt = DateTime.ParseExact(objAuctionInvoice.BOEDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String BOEDate = dt.ToString("yyyy-MM-dd");

            DateTime DeliveryDateUptodt = DateTime.ParseExact(objAuctionInvoice.DeliveryDateUpto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String DeliveryDateUpto = DeliveryDateUptodt.ToString("yyyy-MM-dd");

            DateTime FreeUptodt = DateTime.ParseExact(objAuctionInvoice.FreeUpto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String FreeUpto = FreeUptodt.ToString("yyyy-MM-dd");

            DateTime AssesmentSheetDatedt = DateTime.ParseExact(objAuctionInvoice.AssesmentSheetDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String AssesmentSheetDate = AssesmentSheetDatedt.ToString("yyyy-MM-dd");




            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.String, Value = "Tax" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objAuctionInvoice.InvoiceDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.AssesmentInvoiceID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.AssesmentInvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objAuctionInvoice.ReceiptDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalCGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalSGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalIGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.AllTotal });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.RoundUp });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.InvoiceValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterName", MySqlDbType = MySqlDbType.String, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = objAuctionInvoice.TotalNoOfPackages });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.TotalGrossWt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.String, Value = "AUC" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_HSNCode", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.HSNCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GST", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.GST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ref", MySqlDbType = MySqlDbType.Text, Value = RefNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEONO", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.BOENo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BEODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentType", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.AssesmentType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDesc", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentSheetDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AssesmentSheetDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FreeUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FreeUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomDute", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.CustomDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ot", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.OT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ValuersCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.ValuersCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AuctionCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.AuctionCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MISCExpenses", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.MISCExpenses) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CWCShare", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.CWCShare) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.TDSAmount) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryValid", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DeliveryDateUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(objAuctionInvoice.ExportUnder) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceNo", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceId", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditAuctionInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                if (Result == 1 || Result == 2)
                {
                    GeneratedInvoiceNo = Convert.ToString(DParam.Where(x => x.ParameterName == "GeneratedInvoiceNo").Select(x => x.Value).FirstOrDefault());
                    _DBResponse.Data = GeneratedInvoiceNo;
                    _DBResponse.Message = (Result == 1) ? "Auction Invoice Saved Successfully" : "Auction Invoice Updated Successfully";
                    _DBResponse.Status = Result;
                }

                else if (Result == 3)
                {
                    //  GeneratedInvoiceNo = Convert.ToString(DParam.Where(x => x.ParameterName == "GeneratedInvoiceNo").Select(x => x.Value).FirstOrDefault());
                    //  _DBResponse.Data = GeneratedInvoiceNo;
                    _DBResponse.Message = "Invoice can not be generated. SD Balance is low";
                    _DBResponse.Status = Result;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }


        public void GenericBulkInvoiceDetailsForPrint(string InvoiceNo)
        {

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = InvoiceNo });

            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetAuctionAssessmentSheetprint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();

            }
        }


        public void GetAssessmentList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAuctionAssessmentSheetList", CommandType.StoredProcedure, DParam);
            List<HDB_AuctionInvoice> lstBidDetails = new List<HDB_AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new HDB_AuctionInvoice
                    {
                        BIDNo = Convert.ToString(Result["BidNo"]),
                        PartyName = Convert.ToString(Result["Party"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        BidDate = Convert.ToString(Result["BidDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBidDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        public void GetAssessmentInvoiceList(int id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceID", MySqlDbType = MySqlDbType.Int32, Value = id });
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAuctionAssessmentInvoiceList", CommandType.StoredProcedure, DParam);
            List<HDB_AuctionInvoice> lstBidDetails = new List<HDB_AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new HDB_AuctionInvoice
                    {

                        InvoiceId = Convert.ToInt32(Result["ID"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBidDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }

        }

        public void GetAssessmentInvoiceListByID(int id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceID", MySqlDbType = MySqlDbType.Int32, Value = id });
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAuctionAssessmentInvoiceList", CommandType.StoredProcedure, DParam);
            List<HDB_AuctionInvoice> lstBidDetails = new List<HDB_AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new HDB_AuctionInvoice
                    {

                        InvoiceId = Convert.ToInt32(Result["ID"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        // AssesmentInvoiceID= Convert.ToString(Result["ID"]),
                        BOENo = Convert.ToString(Result["Boe"]),
                        BOEDate = Convert.ToString(Result["BoeDate"]),
                        BIDNo = Convert.ToString(Result["BidNo"]),
                        BIDId = Convert.ToInt32(Result["BidId"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        ShippingBill = Convert.ToString(Result["ShippBillNo"]),
                        OBL = Convert.ToString(Result["OBL"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyAddress = Convert.ToString(Result["Address"]),
                        ValuersCharges = Convert.ToDecimal(Result["ValuesCharges"]),
                        AuctionCharges = Convert.ToDecimal(Result["AuctionCharges"]),
                        MISCExpenses = Convert.ToDecimal(Result["MiscCharges"]),
                        CustomDuty = Convert.ToDecimal(Result["CustomDuty"]),
                        DeliveryDateUpto = Convert.ToString(Result["DeliveryValidUpto"]),
                        FreeUpto = Convert.ToString(Result["FreeUpto"]),
                        TDSAmount = Convert.ToDecimal(Result["TDSAmount"]),
                        OT = Convert.ToDecimal(Result["OT"]),
                        HSNCode = Convert.ToString(Result["HSNCode"]),
                        GST = Convert.ToInt32(Result["GSTPer"]),
                        AssesmentType = Convert.ToString(Result["AssesmentType"]),
                        AssesmentSheetDate = Convert.ToString(Result["AssesmentDate"]),
                        CWCShare = Convert.ToDecimal(Result["CWCShare"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBidDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }

        }


        public void GetInvoiceList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAuctionInvoiceList", CommandType.StoredProcedure, DParam);
            List<HDB_AuctionInvoice> lstBidDetails = new List<HDB_AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new HDB_AuctionInvoice
                    {

                        PartyName = Convert.ToString(Result["Party"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBidDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetInvoiceList(string FromDate, string ToDate, string invoiceType)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = invoiceType });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("AuctionInvoiceListWithDate", CommandType.StoredProcedure, DParam);
            IList<invoiceLIst> LstInvoice = new List<invoiceLIst>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new invoiceLIst
                    {



                        InvoiceNumber = Result["InvoiceNumber"].ToString()



                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice); ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }

        public void ModuleListWithInvoice(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int PartyId = ObjBulkInvoiceReport.PartyId;

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            DParam = LstParam.ToArray();
            DataSet LstInvoice = new DataSet();
            LstInvoice = DataAccess.ExecuteDataSet("AuctionModuleListWithInvoice", CommandType.StoredProcedure, DParam);

            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (LstInvoice != null)
                {
                    Status = 1;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Close();
                //Result.Dispose();

            }
        }
    }
}