using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using EinvoiceLibrary;
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
    public class Ppg_AuctionRepository
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
           List<EMDReceived> lstBidDetails = new List<EMDReceived>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new EMDReceived
                    {
                        BIDId = Convert.ToInt32(Result["BidIdHdr"]),
                        BIDNo = Convert.ToString(Result["BidNo"]),
                        PartyId= Convert.ToInt32(Result["PartyId"]),
                        PartyName= Convert.ToString(Result["PartyName"]),
                        OBL= Convert.ToString(Result["OBL"]),
                        ShippBillNo= Convert.ToString(Result["ShippBillNo"]),
                        BIDDate= Convert.ToString(Result["OBL"])

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
            var model = new AuctionInvoice();
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
                    model.OBL= Convert.ToString(Result["OBL"]);
                    model.ShippingBill=Convert.ToString(Result["ShippBillNo"]);
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Convert.ToString(Result["PartyName"]);
                    model.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                    model.GSTNo = Convert.ToString(Result["GSTNo"]);
                    model.State = Convert.ToString(Result["StateName"]);
                    model.StateCode = Convert.ToString(Result["GstStateCode"]);
                    model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceType = "Tax";
                    model.BidAmount= Convert.ToDecimal(Result["BidAmount"]);
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


        public void GetAuctionCharges(int BidId, int InvoiceId, string InvoiceDate, string FreeUpTo,string HSNCode, decimal CustomDuty, decimal OT, decimal ValuersCharge, decimal AuctionCharge, decimal MISC,string SEZ)
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
                LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(SEZ) });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetAuctionInvoiceCharges", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                AuctionInvoice objAuctionInvoice = new AuctionInvoice();
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

        public void GetAuctionChargesForEdit(int BidId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, decimal CustomDuty, decimal OT, decimal ValuersCharge, decimal AuctionCharge, decimal MISC)
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

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetAuctionInvoiceChargesForEdit", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                AuctionInvoice objAuctionInvoice = new AuctionInvoice();
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

        public void AddEditInvoice(AuctionInvoice objAuctionInvoice)
        {
            int GeneratedInvoiceId = 0;
            string GeneratedInvoiceNo = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            string RefNo = "";
            if(objAuctionInvoice.OBL!=null)
            {
                RefNo = objAuctionInvoice.OBL;
            }
            else if(objAuctionInvoice.ShippingBill!=null)
            {
                RefNo = objAuctionInvoice.ShippingBill;
            }
            else if(objAuctionInvoice.ContainerNo !=null)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentSheetDate", MySqlDbType = MySqlDbType.Date, Value =Convert.ToDateTime(AssesmentSheetDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FreeUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FreeUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomDute", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.CustomDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ot", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.OT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ValuersCharges", MySqlDbType = MySqlDbType.Decimal, Value =Convert.ToDecimal(objAuctionInvoice.ValuersCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AuctionCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.AuctionCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MISCExpenses", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.MISCExpenses) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CWCShare", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.CWCShare )});
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.TDSAmount) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryValid", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DeliveryDateUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "xmlCashReceipt", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.String, Value = objAuctionInvoice.SEZ });
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

                else if(Result == 3)
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

        public void AddEditAuctionInvoice(AuctionInvoice objAuctionInvoice)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(objAuctionInvoice.SEZ) });
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

        public void AddEditAuctionInvoiceForEdit(AuctionInvoice objAuctionInvoice)
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
         //   lstParam.Add(new MySqlParameter { ParameterName = "in_BEODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentType", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.AssesmentType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDesc", MySqlDbType = MySqlDbType.Text, Value = objAuctionInvoice.CargoDescription });
         //   lstParam.Add(new MySqlParameter { ParameterName = "in_AssesmentSheetDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AssesmentSheetDate) });
         //   lstParam.Add(new MySqlParameter { ParameterName = "in_FreeUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FreeUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CustomDute", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.CustomDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Ot", MySqlDbType = MySqlDbType.Decimal, Value = objAuctionInvoice.OT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ValuersCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.ValuersCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AuctionCharges", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.AuctionCharges) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MISCExpenses", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.MISCExpenses) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CWCShare", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.CWCShare) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objAuctionInvoice.TDSAmount) });
         //   lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryValid", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DeliveryDateUpto) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceNo", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceId", MySqlDbType = MySqlDbType.String, Value = GeneratedInvoiceId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditAuctionInvoiceForEdit", CommandType.StoredProcedure, DParam);
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
            List<AuctionInvoice> lstBidDetails = new List<AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new AuctionInvoice
                    {                        
                        BIDNo = Convert.ToString(Result["BidNo"]),                    
                        PartyName = Convert.ToString(Result["Party"]),
                        InvoiceDate= Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo= Convert.ToString(Result["InvoiceNo"]),
                        BidDate= Convert.ToString(Result["BidDate"])  ,
                        ServiceType= Convert.ToString(Result["SupplyType"])
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
            List<AuctionInvoice> lstBidDetails = new List<AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new AuctionInvoice
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
            List<AuctionInvoice> lstBidDetails = new List<AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new AuctionInvoice
                    {
                        
                        InvoiceId = Convert.ToInt32(Result["ID"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        BOENo=Convert.ToString(Result["Boe"]),
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
                        AssesmentType=Convert.ToString(Result["AssesmentType"]),
                        AssesmentSheetDate = Convert.ToString(Result["AssesmentDate"]),
                        CWCShare= Convert.ToDecimal(Result["CWCShare"]) 
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
            List<AuctionInvoice> lstBidDetails = new List<AuctionInvoice>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBidDetails.Add(new AuctionInvoice
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




        #region Edit Auction Payment Sheet

        public void GetAuctionInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAucInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal hours = 0;
            //PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            CwcExim.Areas.Import.Models.PPGInvoiceGodown objPostPaymentSheet = new CwcExim.Areas.Import.Models.PPGInvoiceGodown();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                    objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                    objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                    objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                    objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                    objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                    objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                    objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                    objPostPaymentSheet.Pan = Result["Pan"].ToString();

                    objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                    objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                    objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                        objPostPaymentSheet.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                        objPostPaymentSheet.BidId = Convert.ToInt32(Result["BidId"]);
                        objPostPaymentSheet.AssesmentType = Convert.ToString(Result["AssesmentType"]);
                        objPostPaymentSheet.HSNCode = Convert.ToString(Result["HSNCode"]);
                        objPostPaymentSheet.GSTPer = Convert.ToString(Result["GSTPer"]);
                        objPostPaymentSheet.AssesmentDate = Convert.ToString(Result["AssesmentDate"]);
                        objPostPaymentSheet.FreeUpto = Convert.ToString(Result["FreeUpto"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (Result["ChargeType"].ToString() == "OTI")
                        {
                            if (Convert.ToDecimal(Result["Rate"]) > 0)
                            {
                                decimal hr = Convert.ToDecimal(Result["Amount"].ToString()) / Convert.ToDecimal(Result["Rate"]);
                                hours = hr;

                                objPostPaymentSheet.OTHours = hours;
                            }
                        }


                        objPostPaymentSheet.lstPostPaymentChrg.Add(new CwcExim.Areas.Import.Models.PpgPostPaymentChrg()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new CwcExim.Areas.Import.Models.PpgPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt16(Result["IsReefer"]),
                            Insured = Convert.ToInt16(Result["Insured"]),
                            RMS = Convert.ToInt16(Result["RMS"]),
                            CargoType = Convert.ToInt16(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                          //  LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : "",
                            OBLNo = Convert.ToString(Result["BOLNo"]),
                            BOLDate = Convert.ToString(Result["BOLDate"]),
                            LineNo = Convert.ToString(Result["LineNo"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new CwcExim.Areas.Import.Models.PpgContainerWiseAmount()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                          //  LineNo = HasColumn(Result, "LineNo") ? Result["LineNo"].ToString() : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new CwcExim.Areas.Import.Models.PpgOperationCFSCodeWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstInvoiceCargo.Add(new CwcExim.Areas.Import.Models.PpgInvoiceCargo
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Result["CargoType"].ToString() == "" ? 0 : Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GdnWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GdnWiseLctnIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWt"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
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
                Result.Close();
            }
        }
        #endregion

        #region IRN
        public void GetIRNForYard(String InvoiceNo)
        {
            EinvoicePayload Obj = new EinvoicePayload();

            TranDtls tr = new TranDtls();
            DocDtls doc = new DocDtls();
            SellerDtls seller = new SellerDtls();
            BuyerDtls buyer = new BuyerDtls();
            DispDtls disp = new DispDtls();

            ShipDtls ship = new ShipDtls();
            BchDtls btc = new BchDtls();
            AttribDtls attr = new AttribDtls();
            ValDtls vald = new ValDtls();
            PayDtls payd = new PayDtls();
            RefDtls refd = new RefDtls();
            PrecDocDtls pred = new PrecDocDtls();
            ContrDtls Cont = new ContrDtls();
            AddlDocDtls addl = new AddlDocDtls();
            ExpDtls expd = new ExpDtls();
            EwbDtls ewb = new EwbDtls();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetailsAuction", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.Version = Result["Version"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        tr.TaxSch = Result["TaxSch"].ToString();
                        tr.SupTyp = Result["SupTyp"].ToString();
                        tr.RegRev = Result["RegRev"].ToString();
                        tr.EcmGstin = null;

                        tr.IgstOnIntra = Result["IgstOnIntra"].ToString();

                    }
                }




                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        doc.Typ = Result["Typ"].ToString();
                        doc.No = Result["No"].ToString();
                        doc.Dt = Result["Dt"].ToString();


                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        seller.Gstin = Result["Gstin"].ToString();
                        seller.LglNm = Result["LglNm"].ToString();
                        seller.TrdNm = Result["TrdNm"].ToString();
                        seller.Addr1 = Result["Addr1"].ToString();
                        seller.Addr2 = null;
                        seller.Loc = Result["Loc"].ToString();
                        seller.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        seller.Stcd = Result["Stcd"].ToString();
                        seller.Ph = null;// Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        seller.Em = null;//Result["Em"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        buyer.Gstin = Result["Gstin"].ToString();
                        buyer.LglNm = Result["LglNm"].ToString();
                        buyer.TrdNm = Result["TrdNm"].ToString();
                        buyer.Addr1 = Result["Addr1"].ToString();
                        buyer.Addr2 = null;
                        buyer.Pos = Convert.ToString(Result["Stcd"]);
                        buyer.Loc = Result["Loc"].ToString();
                        buyer.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        buyer.Stcd = Result["Stcd"].ToString();
                        buyer.Ph = null;//Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        buyer.Em = null; //Result["Em"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        disp.Nm = Result["Nm"].ToString();
                        disp.Addr1 = Result["Addr1"].ToString();
                        disp.Addr2 = null;
                        disp.Loc = Result["Loc"].ToString();
                        disp.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        disp.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ship.Gstin = Result["Gstin"].ToString();
                        ship.LglNm = Result["LglNm"].ToString();
                        ship.TrdNm = Result["TrdNm"].ToString();
                        ship.Addr1 = Result["Addr1"].ToString();
                        ship.Addr2 = null;
                        ship.Loc = Result["Loc"].ToString();
                        ship.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        ship.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        btc.Nm = Result["Nm"].ToString();
                        btc.ExpDt = Result["ExpDt"].ToString();
                        btc.WrDt = Result["WrDt"].ToString();
                        // attr.Nm = Result["Nmm"].ToString();
                        // attr.Val= Result["Val"].ToString();
                        attr = null;
                        Obj.ItemList.Add(new ItemList
                        {
                            SlNo = Result["SlNo"].ToString(),
                            PrdDesc =null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit =null,// Result["Unit"].ToString(),
                            UnitPrice = Convert.ToDecimal(Result["UnitPrice"].ToString()),
                            TotAmt = Convert.ToDecimal(Result["TotAmt"].ToString()),
                            Discount = Convert.ToInt32(Result["Discount"].ToString()),
                            PreTaxVal = Convert.ToInt32(Result["PreTaxVal"].ToString()),
                            AssAmt = Convert.ToDecimal(Result["AssAmt"].ToString()),
                            GstRt = Convert.ToDecimal(Result["GstRt"].ToString()),
                            IgstAmt = Convert.ToDecimal(Result["IgstAmt"].ToString()),
                            CgstAmt = Convert.ToDecimal(Result["CgstAmt"].ToString()),
                            SgstAmt = Convert.ToDecimal(Result["SgstAmt"].ToString()),
                            CesRt = Convert.ToInt32(Result["CesRt"].ToString()),
                            CesAmt = Convert.ToDecimal(Result["CesAmt"].ToString()),
                            CesNonAdvlAmt = Convert.ToInt32(Result["CesNonAdvlAmt"].ToString()),
                            StateCesRt = Convert.ToInt32(Result["StateCesRt"].ToString()),
                            StateCesAmt = Convert.ToDecimal(Result["StateCesAmt"].ToString()),
                            StateCesNonAdvlAmt = Convert.ToInt32(Result["StateCesNonAdvlAmt"].ToString()),
                            OthChrg = Convert.ToInt32(Result["OthChrg"].ToString()),
                            TotItemVal = Convert.ToDecimal(Result["TotItemVal"].ToString()),
                            OrdLineRef = Convert.ToString(Result["OrdLineRef"].ToString()),
                            OrgCntry = Result["OrgCntry"].ToString(),
                            PrdSlNo = Convert.ToString(Result["PrdSlNo"].ToString()),
                            BchDtls = null,
                            AttribDtls = null,

                        });



                    }


                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        vald.AssVal = Convert.ToDecimal(Result["AssVal"].ToString());
                        vald.CgstVal = Convert.ToDecimal(Result["CgstVal"].ToString());
                        vald.SgstVal = Convert.ToDecimal(Result["SgstVal"].ToString());
                        vald.IgstVal = Convert.ToDecimal(Result["IgstVal"].ToString());
                        vald.CesVal = Convert.ToDecimal(Result["CesVal"].ToString());
                        vald.StCesVal = Convert.ToDecimal(Result["StCesVal"].ToString());
                        vald.Discount = Convert.ToDecimal(Result["Discount"].ToString());
                        vald.OthChrg = Convert.ToDecimal(Result["OthChrg"].ToString());
                        vald.RndOffAmt = Convert.ToDecimal(Result["RndOffAmt"].ToString());
                        vald.TotInvVal = Convert.ToDecimal(Result["TotInvVal"].ToString());
                        vald.TotInvValFc = Convert.ToDecimal(Result["TotInvValFc"].ToString());


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        payd.Nm = Result["Nm"].ToString();
                        payd.AccDet = Result["AccDet"].ToString();
                        payd.Mode = Result["Mode"].ToString();
                        payd.FinInsBr = Result["FinInsBr"].ToString();
                        payd.PayTerm = Result["PayTerm"].ToString();
                        payd.PayInstr = Result["PayInstr"].ToString();
                        payd.CrTrn = Result["CrTrn"].ToString();
                        payd.DirDr = Result["DirDr"].ToString();
                        payd.CrDay = Result["CrDay"].ToString();
                        payd.PaidAmt = Result["PaidAmt"].ToString();
                        payd.PaymtDue = Result["PaymtDue"].ToString();


                    }
                }


                if (Result.NextResult())
                {
                    DocPerdDtls docp = new DocPerdDtls();
                    while (Result.Read())
                    {
                        refd.InvRm = Result["InvRm"].ToString();
                        docp.InvStDt = Result["InvStDt"].ToString();
                        docp.InvEndDt = Result["InvEndDt"].ToString();
                        refd.DocPerdDtls = docp;


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        pred.InvNo = Result["InvNo"].ToString();
                        pred.InvDt = Result["InvDt"].ToString();
                        pred.OthRefNo = Result["OthRefNo"].ToString();


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Cont.RecAdvRefr = Result["RecAdvRefr"].ToString();
                        Cont.RecAdvDt = Result["RecAdvDt"].ToString();
                        Cont.TendRefr = Result["TendRefr"].ToString();
                        Cont.ContrRefr = Result["ContrRefr"].ToString();
                        Cont.ExtRefr = Result["ExtRefr"].ToString();
                        Cont.ProjRefr = Result["ProjRefr"].ToString();
                        Cont.PORefr = Result["PORefr"].ToString();
                        Cont.PORefDt = Result["PORefDt"].ToString();



                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        addl.Url = Result["Url"].ToString();
                        addl.Docs = Result["Docs"].ToString();
                        addl.Info = Result["Info"].ToString();




                    }
                }

                if (Result.NextResult())
                {
                    // while (Result.Read())
                    // {



                    // Obj.ExpDtls.Add(new ExpDtls
                    // {
                    // ShipBNo = Result["ShipBNo"].ToString(),
                    // ShipBDt = Result["ShipBDt"].ToString(),
                    // Port = Result["Port"].ToString(),
                    // RefClm = Result["RefClm"].ToString(),
                    // ForCur = Result["ForCur"].ToString(),
                    // CntCode = Result["CntCode"].ToString(),
                    // ExpDuty = Result["ExpDuty"].ToString(),


                    // });












                    // }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ewb.TransId = Result["TransId"].ToString();
                        ewb.TransName = Result["TransName"].ToString();
                        ewb.Distance = Result["Distance"].ToString();
                        ewb.TransDocNo = Result["TransDocNo"].ToString();
                        ewb.TransDocDt = Result["TransDocDt"].ToString();
                        ewb.VehNo = Result["VehNo"].ToString();
                        ewb.VehType = Result["VehType"].ToString();
                        ewb.TransMode = Result["TransMode"].ToString();




                    }
                }

                //if (Result.NextResult())
                //{
                // while (Result.Read())
                // {
                // hp.ClientID = Result["ClientID"].ToString();
                // hp.ClientSecret = Result["ClientSecret"].ToString();
                // hp.GSTIN = Result["GSTIN"].ToString();
                // hp.UserName = Result["UserName"].ToString();
                // hp.Password = Result["Password"].ToString();
                // hp.AppKey = "";



                // }
                //}
                Obj.TranDtls = tr;
                Obj.DocDtls = doc;
                Obj.SellerDtls = seller;
                Obj.BuyerDtls = buyer;
                Obj.DispDtls = disp;
                Obj.ShipDtls = ship;
                Obj.AttribDtls = attr;
                Obj.ValDtls = vald;
                Obj.PayDtls = null;
                Obj.RefDtls = null;
                Obj.PrecDocDtls = pred;
                Obj.ContrDtls = Cont;
                Obj.AddlDocDtls = null;
                Obj.ExpDtls = null;
                // Obj.ExpDtls = expd;
                Obj.EwbDtls = null;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetIRNB2CForYard(String InvoiceNo)
        {
            Ppg_IrnB2CDetails Obj = new Ppg_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetailsAuction", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Obj.DocNo = Convert.ToString(Result["DocNo"]);
                    Obj.DocDt = Convert.ToString(Result["DocDt"]);
                    Obj.DocTyp = Convert.ToString(Result["DocTyp"]);
                    Obj.BuyerGstin = Convert.ToString(Result["BuyerGstin"]);
                    Obj.SellerGstin = Convert.ToString(Result["SellerGST"]);

                    Obj.MainHsnCode = Convert.ToString(Result["MainHsnCode"]);
                    Obj.TotInvVal = Convert.ToInt32(Result["TotInvVal"]);
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"]);
                    Obj.SupplierUPIID = Convert.ToString(Result["UPIID"]);
                    Obj.BankAccountNo = Convert.ToString(Result["AccountNo"]);
                    Obj.IFSC = Convert.ToString(Result["IFSC"]);

                    Obj.CGST = Convert.ToDecimal(Result["CGST"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGST"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGST"]);
                    Obj.CESS = Convert.ToDecimal(Result["CESS"]);
                    Obj.iss = null;
                    Obj.ver = Convert.ToString(Result["ver"]);

                    Obj.tier = Convert.ToString(Result["tier"]);
                    Obj.tid = Convert.ToString(Result["tid"]);
                    //Obj.sign = Convert.ToString(Result["sign"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    Obj.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    Obj.CESS = 0;

                    Obj.qrMedium = Convert.ToInt32(Result["qrMedium"]);
                    Obj.QRexpire = Convert.ToString(Result["QRexpireDays"]);

                    Obj.pinCode = Convert.ToInt32(Result["pinCode"]);
                    Obj.pa = Convert.ToString(Result["pa"]);
                    Obj.orgId = Convert.ToInt32(Result["orgId"]);
                    Obj.mtid = Convert.ToString(Result["mtid"]);
                    Obj.msid = Convert.ToString(Result["msid"]);
                    Obj.mode = Convert.ToInt32(Result["mode"]);
                    Obj.mc = Convert.ToString(Result["mc"]);
                    Obj.mam = Convert.ToString(Result["InvoiceAmt"]);
                    Obj.GSTPCT = Convert.ToInt32(Result["IGSTPer"]);
                    Obj.GSTIncentive = 0;
                    Obj.mid = Convert.ToString(Result["mid"]);
                    Obj.InvoiceName = Convert.ToString(Result["PartyName"]);
                    Obj.tr = Convert.ToString(Result["tr"]);
                    Obj.pn = Convert.ToString(Result["pn"]);
                    Obj.gstIn = Convert.ToString(Result["SellerGST"]);

                }



                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddEditIRNResponsec(IrnResponse objOBL, string InvoiceNo)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckNo", MySqlDbType = MySqlDbType.String, Value = objOBL.AckNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckDt", MySqlDbType = MySqlDbType.String, Value = objOBL.AckDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = objOBL.irn }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedInvoice", MySqlDbType = MySqlDbType.String, Value = objOBL.SignedInvoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCode", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.SignedQRCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = objOBL.QRCodeImageBase64 });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnStatus", MySqlDbType = MySqlDbType.String, Value = objOBL.IrnStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbNo", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.EwbNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbDt", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbValidTill", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbValidTill });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnResponsecol", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditAuctionirnresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Duplicate OBL No.!";
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


        public void GetHeaderIRNForYard()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }



                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        public void GetIRNForProductSell(String InvoiceNo)
        {
            EinvoicePayload Obj = new EinvoicePayload();

            TranDtls tr = new TranDtls();
            DocDtls doc = new DocDtls();
            SellerDtls seller = new SellerDtls();
            BuyerDtls buyer = new BuyerDtls();
            DispDtls disp = new DispDtls();

            ShipDtls ship = new ShipDtls();
            BchDtls btc = new BchDtls();
            AttribDtls attr = new AttribDtls();
            ValDtls vald = new ValDtls();
            PayDtls payd = new PayDtls();
            RefDtls refd = new RefDtls();
            PrecDocDtls pred = new PrecDocDtls();
            ContrDtls Cont = new ContrDtls();
            AddlDocDtls addl = new AddlDocDtls();
            ExpDtls expd = new ExpDtls();
            EwbDtls ewb = new EwbDtls();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetailsAuction", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.Version = Result["Version"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        tr.TaxSch = Result["TaxSch"].ToString();
                        tr.SupTyp = Result["SupTyp"].ToString();
                        tr.RegRev = Result["RegRev"].ToString();
                        tr.EcmGstin = null;

                        tr.IgstOnIntra = Result["IgstOnIntra"].ToString();

                    }
                }




                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        doc.Typ = Result["Typ"].ToString();
                        doc.No = Result["No"].ToString();
                        doc.Dt = Result["Dt"].ToString();


                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        seller.Gstin = Result["Gstin"].ToString();
                        seller.LglNm = Result["LglNm"].ToString();
                        seller.TrdNm = Result["TrdNm"].ToString();
                        seller.Addr1 = Result["Addr1"].ToString();
                        seller.Addr2 = null;
                        seller.Loc = Result["Loc"].ToString();
                        seller.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        seller.Stcd = Result["Stcd"].ToString();
                        seller.Ph = null;// Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        seller.Em = null;//Result["Em"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        buyer.Gstin = Result["Gstin"].ToString();
                        buyer.LglNm = Result["LglNm"].ToString();
                        buyer.TrdNm = Result["TrdNm"].ToString();
                        buyer.Addr1 = Result["Addr1"].ToString();
                        buyer.Addr2 = Result["Addr2"].ToString()==""? null: Result["Addr2"].ToString();// Result["Addr2"].ToString();
                        buyer.Pos =  Convert.ToString(Result["Stcd"]);
                        buyer.Loc = Result["Loc"].ToString();
                        buyer.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        buyer.Stcd = Result["Stcd"].ToString();
                        buyer.Ph = null;//Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        buyer.Em = null; //Result["Em"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        disp.Nm = Result["Nm"].ToString();
                        disp.Addr1 = Result["Addr1"].ToString();
                        disp.Addr2 = null;
                        disp.Loc = Result["Loc"].ToString();
                        disp.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        disp.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ship.Gstin = Result["Gstin"].ToString();
                        ship.LglNm = Result["LglNm"].ToString();
                        ship.TrdNm = Result["TrdNm"].ToString();
                        ship.Addr1 = Result["Addr1"].ToString();
                        ship.Addr2 = Result["Addr2"].ToString() == "" ? null : Result["Addr2"].ToString();// Result["Addr2"].ToString();
                        ship.Loc = Result["Loc"].ToString();
                        ship.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        ship.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        btc.Nm = Result["Nm"].ToString();
                        btc.ExpDt = Result["ExpDt"].ToString();
                        btc.WrDt = Result["WrDt"].ToString();
                        // attr.Nm = Result["Nmm"].ToString();
                        // attr.Val= Result["Val"].ToString();
                        attr = null;
                        Obj.ItemList.Add(new ItemList
                        {
                            SlNo = Result["SlNo"].ToString(),
                            PrdDesc =  Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde =null, //Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit =  Result["Unit"].ToString(),
                            UnitPrice = Convert.ToDecimal(Result["UnitPrice"].ToString()),
                            TotAmt = Convert.ToDecimal(Result["TotAmt"].ToString()),
                            Discount = Convert.ToInt32(Result["Discount"].ToString()),
                            PreTaxVal = Convert.ToInt32(Result["PreTaxVal"].ToString()),
                            AssAmt = Convert.ToDecimal(Result["AssAmt"].ToString()),
                            GstRt = Convert.ToDecimal(Result["GstRt"].ToString()),
                            IgstAmt = Convert.ToDecimal(Result["IgstAmt"].ToString()),
                            CgstAmt = Convert.ToDecimal(Result["CgstAmt"].ToString()),
                            SgstAmt = Convert.ToDecimal(Result["SgstAmt"].ToString()),
                            CesRt = Convert.ToInt32(Result["CesRt"].ToString()),
                            CesAmt = Convert.ToDecimal(Result["CesAmt"].ToString()),
                            CesNonAdvlAmt = Convert.ToInt32(Result["CesNonAdvlAmt"].ToString()),
                            StateCesRt = Convert.ToInt32(Result["StateCesRt"].ToString()),
                            StateCesAmt = Convert.ToDecimal(Result["StateCesAmt"].ToString()),
                            StateCesNonAdvlAmt = Convert.ToInt32(Result["StateCesNonAdvlAmt"].ToString()),
                            OthChrg = Convert.ToInt32(Result["OthChrg"].ToString()),
                            TotItemVal = Convert.ToDecimal(Result["TotItemVal"].ToString()),
                            OrdLineRef =null,// Convert.ToString(Result["OrdLineRef"].ToString()),
                            OrgCntry = Result["OrgCntry"].ToString(),
                            PrdSlNo = Convert.ToString(Result["PrdSlNo"].ToString()),
                            BchDtls = null,
                            AttribDtls = null,

                        });



                    }


                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        vald.AssVal = Convert.ToDecimal(Result["AssVal"].ToString());
                        vald.CgstVal = Convert.ToDecimal(Result["CgstVal"].ToString());
                        vald.SgstVal = Convert.ToDecimal(Result["SgstVal"].ToString());
                        vald.IgstVal = Convert.ToDecimal(Result["IgstVal"].ToString());
                        vald.CesVal = Convert.ToDecimal(Result["CesVal"].ToString());
                        vald.StCesVal = Convert.ToDecimal(Result["StCesVal"].ToString());
                        vald.Discount = Convert.ToDecimal(Result["Discount"].ToString());
                        vald.OthChrg = Convert.ToDecimal(Result["OthChrg"].ToString());
                        vald.RndOffAmt = Convert.ToDecimal(Result["RndOffAmt"].ToString());
                        vald.TotInvVal = Convert.ToDecimal(Result["TotInvVal"].ToString());
                        vald.TotInvValFc = Convert.ToDecimal(Result["TotInvValFc"].ToString());


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        payd.Nm = Result["Nm"].ToString();
                        payd.AccDet = Result["AccDet"].ToString();
                        payd.Mode = Result["Mode"].ToString();
                        payd.FinInsBr = Result["FinInsBr"].ToString();
                        payd.PayTerm = Result["PayTerm"].ToString();
                        payd.PayInstr = Result["PayInstr"].ToString();
                        payd.CrTrn = Result["CrTrn"].ToString();
                        payd.DirDr = Result["DirDr"].ToString();
                        payd.CrDay = Result["CrDay"].ToString();
                        payd.PaidAmt = Result["PaidAmt"].ToString();
                        payd.PaymtDue = Result["PaymtDue"].ToString();


                    }
                }


                if (Result.NextResult())
                {
                    DocPerdDtls docp = new DocPerdDtls();
                    while (Result.Read())
                    {
                        refd.InvRm = Result["InvRm"].ToString();
                        docp.InvStDt = Result["InvStDt"].ToString();
                        docp.InvEndDt = Result["InvEndDt"].ToString();
                        refd.DocPerdDtls = docp;


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        pred.InvNo = Result["InvNo"].ToString();
                        pred.InvDt = Result["InvDt"].ToString();
                        pred.OthRefNo = Result["OthRefNo"].ToString();


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Cont.RecAdvRefr = Result["RecAdvRefr"].ToString();
                        Cont.RecAdvDt = Result["RecAdvDt"].ToString();
                        Cont.TendRefr = Result["TendRefr"].ToString();
                        Cont.ContrRefr = Result["ContrRefr"].ToString();
                        Cont.ExtRefr = Result["ExtRefr"].ToString();
                        Cont.ProjRefr = Result["ProjRefr"].ToString();
                        Cont.PORefr = Result["PORefr"].ToString();
                        Cont.PORefDt = Result["PORefDt"].ToString();



                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        addl.Url = Result["Url"].ToString();
                        addl.Docs = Result["Docs"].ToString();
                        addl.Info = Result["Info"].ToString();




                    }
                }

                if (Result.NextResult())
                {
                    // while (Result.Read())
                    // {



                    // Obj.ExpDtls.Add(new ExpDtls
                    // {
                    // ShipBNo = Result["ShipBNo"].ToString(),
                    // ShipBDt = Result["ShipBDt"].ToString(),
                    // Port = Result["Port"].ToString(),
                    // RefClm = Result["RefClm"].ToString(),
                    // ForCur = Result["ForCur"].ToString(),
                    // CntCode = Result["CntCode"].ToString(),
                    // ExpDuty = Result["ExpDuty"].ToString(),


                    // });












                    // }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ewb.TransId = Result["TransId"].ToString();
                        ewb.TransName = Result["TransName"].ToString();
                        ewb.Distance = Result["Distance"].ToString();
                        ewb.TransDocNo = Result["TransDocNo"].ToString();
                        ewb.TransDocDt = Result["TransDocDt"].ToString();
                        ewb.VehNo = Result["VehNo"].ToString();
                        ewb.VehType = Result["VehType"].ToString();
                        ewb.TransMode = Result["TransMode"].ToString();




                    }
                }

                //if (Result.NextResult())
                //{
                // while (Result.Read())
                // {
                // hp.ClientID = Result["ClientID"].ToString();
                // hp.ClientSecret = Result["ClientSecret"].ToString();
                // hp.GSTIN = Result["GSTIN"].ToString();
                // hp.UserName = Result["UserName"].ToString();
                // hp.Password = Result["Password"].ToString();
                // hp.AppKey = "";



                // }
                //}
                Obj.TranDtls = tr;
                Obj.DocDtls = doc;
                Obj.SellerDtls = seller;
                Obj.BuyerDtls = buyer;
                Obj.DispDtls = disp;
                Obj.ShipDtls = ship;
                Obj.AttribDtls = attr;
                Obj.ValDtls = vald;
                Obj.PayDtls = null;
                Obj.RefDtls = null;
                Obj.PrecDocDtls = null;//pred;
                Obj.ContrDtls = Cont;
                Obj.AddlDocDtls = null;
                Obj.ExpDtls = null;
                // Obj.ExpDtls = expd;
                Obj.EwbDtls = null;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #region Bulk EInvoice Generation

        public void GetBulkIrnDetails()
        {
            int Status = 0;

            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PPG_BulkIRN objInvoice = new PPG_BulkIRN();
            // IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedauctionInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new PPG_BulkIRNDetails
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GstNo = Result["GstNo"].ToString(),
                        SupplyType = Result["SupplyType"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        OperationType = Result["InvType"].ToString()
                    });
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

     
        #endregion
    }
}