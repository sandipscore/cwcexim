using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.ExpSealCheking.Models;

namespace CwcExim.Repositories
{
    public class CHN_ExportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Seal Checking

        public void GetTruckSlipNo(int EntryId = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTruckSlipListForSealChekingPaymentSheet", CommandType.StoredProcedure, DParam);
            IList<Chn_PaySheetTruckSlipRequest> lstSealPymentSlip = new List<Chn_PaySheetTruckSlipRequest>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstSealPymentSlip.Add(new Chn_PaySheetTruckSlipRequest
                    {
                        EntryId = Convert.ToInt32(result["EntryId"].ToString()),
                        TruckSlipNo = result["TruckSlipNo"].ToString(),
                        TruckSlipDate = result["TruckSlipDate"].ToString(),
                        CHAId = Convert.ToInt32(result["CHAId"]),
                        CHAName = Convert.ToString(result["CHAName"]),
                        CHAGSTNo = Convert.ToString(result["GSTNo"]),
                        Address = Convert.ToString(result["Address"]),
                        State = Convert.ToString(result["StateName"]),
                        StateCode = Convert.ToString(result["StateCode"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstSealPymentSlip;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetPaymentPartyForSealChekingInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForSealChekingInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.PaymentPartyName> objPaymentPayeeName = new List<Areas.Export.Models.PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPayeeName.Add(new Areas.Export.Models.PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPayeeName;
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

        public void GetContainerForSealCheckingPaymentSheet(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTruckSlipListForSealChekingPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Chn_PaymentSheetContainer> objPaymentSheetContainer = new List<Chn_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Chn_PaymentSheetContainer()
                    {
                        TruckSlipNo = Convert.ToString(Result["TruckSlipNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Result["Size"].ToString(),
                        IsHaz = Result["IsHaz"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        TruckSlipDate = Result["TruckSlipDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
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

        internal void GetSealCheckingPaymentSheet(string InvoiceDate, int EntryId, string InvoiceType, string SEZ,string ContainerXML, int InvoiceId, int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Chn_InvoiceSealChecking objInvoice = new Chn_InvoiceSealChecking();
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetSealCheckingPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[0].Rows)
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[1].Rows)
                {
                    objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                    objInvoice.CHAName = Result["CHAName"].ToString();
                    objInvoice.PartyName = Result["CHAName"].ToString();
                    objInvoice.PartyGST = Result["GSTNo"].ToString();
                    objInvoice.RequestId = Convert.ToInt32(Result["EntryId"]);
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();

                }
                //}

                // if (Result.NextResult())
                //{

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new Chn_PreInvoiceContainer
                    {
                        TruckSlipNo = Result["TruckSlipNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Result["Size"].ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        ArrivalDate = Result["EntryDateTime"].ToString(),
                        ArrivalTime = Result["EntryDateTime"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        //CargoType = Convert.ToInt32(Result["CargoType"]),
                        //BOENo = Result["BOENo"].ToString(),

                        //WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),

                        //CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        //Duty = Convert.ToDecimal(Result["Duty"]),
                        //Reefer = Convert.ToInt32(Result["Reefer"]),
                        //RMS = Convert.ToInt32(Result["RMS"]),
                        //DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        //Insured = Convert.ToInt32(Result["Insured"]),


                        //HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                        //AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                        //LCLFCL = Result["LCLFCL"].ToString(),

                        //ApproveOn = Result["ApproveOn"].ToString(),
                        //BOEDate = Result["BOEDate"].ToString(),

                        //ImporterExporter = Result["ImporterExporter"].ToString(),
                        //LineNo = Result["LineNo"].ToString(),
                        //OperationType = Convert.ToInt32(Result["OperationType"]),
                        //ShippingLineName = Result["ShippingLineName"].ToString(),


                    });
                    objInvoice.lstPostPaymentCont.Add(new Chn_PostPaymentContainer
                    {
                        TruckSlipNo = Result["TruckSlipNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        Size = Result["Size"].ToString(),
                        ArrivalDate = Result["EntryDateTime"].ToString(),
                        ArrivalTime = Result["EntryDateTime"].ToString(),
                        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                        //AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),                        
                        CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                        DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                        StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                    });
                }
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new Chn_PostPaymentChrg
                    {
                        ChargeId = Convert.ToInt32(Result["ChargeId"]),
                        Clause = Result["Clause"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        Discount = Convert.ToDecimal(Result["Discount"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                    });
                }


                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[4].Rows)
                {
                    objInvoice.lstContWiseAmount.Add(new Chn_ContainerWiseAmount
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        RFIDEntryFee = Convert.ToDecimal(Result["RFIDEntryFee"]),
                        DocumentVerificationFee = Convert.ToDecimal(Result["DocumentVerificationFee"]),
                        ContainerHandlingCharge=Convert.ToDecimal(Result["ContainerHandlingCharge"]),
                        SealVerificationWith = Convert.ToDecimal(Result["SealVerificationWith"]),
                        SealVerificationWithout = Convert.ToDecimal(Result["SealVerificationWithout"]),
                        ReSealing = Convert.ToDecimal(Result["ReSealing"]),
                        GrLoaded = Convert.ToDecimal(Result["Detention"]),
                        MiscCharge = Convert.ToDecimal(Result["MiscCharge"]),
                        Weighment = Convert.ToDecimal(Result["WeighmentCharge"]),
                        ContainerId = 0,
                        InvoiceId = 0,
                        LineNo = ""
                    });
                }
                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[5].Rows)
                {
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new Chn_OperationCFSCodeWiseAmount
                    {
                        InvoiceId = InvoiceId,
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        OperationId = Convert.ToInt32(Result["OperationID"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        Quantity = Convert.ToDecimal(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        Clause = Convert.ToString(Result["Clause"]),
                    });
                }
                //}
                //if (Result.NextResult())
                //{
                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.ActualApplicable.Add(Convert.ToString(Result["Clause"]));
                }
                //}

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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

            }
        }

        public void AddEditContPaymentSheet(Chn_InvoiceSealChecking ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceSealChecking", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        #endregion


        public void MenuAccessRight(int RoleId, int BranchId, int ModuleId, int MenuId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Value = MenuId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Value = ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = BranchId });

            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMenuWiseAccessRight", CommandType.StoredProcedure, dpram);
            IList<Areas.Export.Models.PPG_Menu> lstMenu = new List<Areas.Export.Models.PPG_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new Areas.Export.Models.PPG_Menu
                    {
                        CanAdd = Convert.ToInt32(result["CanAdd"]),
                        CanEdit = Convert.ToInt32(result["CanEdit"]),
                        CanDelete = Convert.ToInt32(result["CanDelete"]),
                        CanView = Convert.ToInt32(result["CanView"])

                    });
                }
                _DBResponse.Data = new { lstMenu };
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";

            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        #region CCIN 
        public void GetPortOfLoading()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoading", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetPortOfLoadingForCCIN(int CountryId)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = CountryId });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoadingForCCIN", CommandType.StoredProcedure,dpram);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBList", CommandType.StoredProcedure);
            List<CCINEntry> LstSB = new List<CCINEntry>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CCINEntry
                    {
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBId = Convert.ToInt32(Result["SBId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSBDetailsBySBId(int SBId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_SBId", MySqlDbType = MySqlDbType.Int32, Value = SBId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetSBDetailsBySBId", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                CCINEntry objCCINEntry = new CCINEntry();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objCCINEntry.SBNo = Convert.ToString(Result.Tables[0].Rows[0]["SB_NO"]);
                        objCCINEntry.SBDate = Convert.ToString(Result.Tables[0].Rows[0]["SB_DATE"]);
                        objCCINEntry.ExporterName = Convert.ToString(Result.Tables[0].Rows[0]["EXP_NAME"]);
                        objCCINEntry.ExporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["EXP_ID"]);
                        objCCINEntry.FOB = Convert.ToDecimal(Result.Tables[0].Rows[0]["FOB"]);
                        objCCINEntry.Package= Convert.ToInt32(Result.Tables[0].Rows[0]["EXP_QTY"]);
                        objCCINEntry.UnitofMeasurement= Convert.ToString(Result.Tables[0].Rows[0]["EXP_UOFMEASUREMENT"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCCINEntry;
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

        public void AddEditCCINEntry(CCINEntry objCCINEntry)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.SBNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.SBDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ExporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.ExporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.GodownName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CHAId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.CHAName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ConsigneeName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ConsigneeName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ConsigneeAdd", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ConsigneeAdd });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.StateId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoadingId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfLoadingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischarge", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PortOfDischarge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Package", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Package });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.Weight });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.FOB });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CommodityId });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = objCCINEntry.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CargoType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRPackageType", MySqlDbType = MySqlDbType.VarString, Value = objCCINEntry.SCMTRPackageType });
            /* lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
             lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.String, Value = "Tax" });
             lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
             lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PartyId });
             lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PartyName });
             lstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.AllTotal });
             lstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalCGST });
             lstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalSGST });
             lstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalIGST });
             lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalAmt });
             lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.RoundUp });
             lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.InvoiceValue });
             lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.Remarks });
             lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });*/
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsSEZ", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.IsSEZ });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditCCINEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "CCIN Entry Saved Successfully" : "CCIN Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Party has insufficient SD Balance to save the Invoice.";
                    _DBResponse.Status = 0;
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

        public void AddEditCCINEntryApproval(int Id, bool IsApproved)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCIN_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsApproved) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditCcinEntryApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Approval Saved Successfully";
                    _DBResponse.Status = 1;
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

        public void GetAllCCINEntry()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntry", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntry> CCINEntryList = new List<CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntry objCCINEntry = new CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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

        public void GetAllCCINEntryForPage(int Page) //, string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntry> CCINEntryList = new List<CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntry objCCINEntry = new CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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

        public void ListOfPackUQCForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UQCCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPackUQCForPage", CommandType.StoredProcedure, Dparam);
            IList<PackUQCForPage> lstPackUQC = new List<PackUQCForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPackUQC.Add(new PackUQCForPage
                    {
                        PackUQCId = Convert.ToInt32(Result["PackId"]),
                        PackUQCDescription = Result["PackName"].ToString(),
                        PackUQCCode = Result["PackCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPackUQC, State };
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

        #region CCIN Search MKS
        public void GetAllCCINEntryForSearch(int Page, string searchtext)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "p_in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "search", MySqlDbType = MySqlDbType.VarChar , Size = 100, Value = searchtext });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForSearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntry> CCINEntryList = new List<CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntry objCCINEntry = new CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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
        #endregion
        public void GetAllCCINEntryApprovalForPage(int Page)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryApprovalForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntry> CCINEntryList = new List<CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntry objCCINEntry = new CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);
                        objCCINEntry.IsApproved = Convert.ToBoolean(dr["IsApproved"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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

        public void GetCCINPartyList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINPartyList", CommandType.StoredProcedure);
            List<Party> LstParty = new List<Party>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new Party
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstParty;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                int OperationType = 2; // For Export
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CcinEntry_Id", MySqlDbType = MySqlDbType.Int32, Value = CCINEntryId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
                LstParam.Add(new MySqlParameter { ParameterName = "In_FOB", MySqlDbType = MySqlDbType.Decimal, Value = FOB });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OperationType", MySqlDbType = MySqlDbType.Int32, Value = OperationType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CargoType", MySqlDbType = MySqlDbType.VarChar, Value = CargoType });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCcinCharges", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                CCINEntry objCCINEntry = new CCINEntry();
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
                                ActualFullCharge = Convert.ToDecimal(dr["ActualFullCharge"]),
                            });
                        }

                        objCCINEntry.IsPartyStateInCompState = Convert.ToBoolean(Result.Tables[0].Rows[0]["IsLocalGST"]);
                        if (lstPostPaymentChrg.Count > 0)
                        {
                            objCCINEntry.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstPostPaymentChrg).ToString();
                        }

                    }

                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        objCCINEntry.PartySDBalance = Convert.ToDecimal(Result.Tables[1].Rows[0]["SDBalance"]);
                    }
                    if (Result.Tables[2].Rows.Count > 0)
                    {
                        //objCCINEntry.PartySDBalance = Convert.ToDecimal(Result.Tables[1].Rows[0]["SDBalance"]);
                        objCCINEntry.PaymentMode = Result.Tables[2].Rows[0]["IN_MODE"].ToString();
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCCINEntry;
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

        public void GetCCINEntryById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINEntryById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntry objCCINEntry = new CCINEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                    // objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    // objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    objCCINEntry.CCINNo = Convert.ToString(Result["CCINNo"]);
                    objCCINEntry.CCINDate = Convert.ToString(Result["CCINDate"]);
                    objCCINEntry.SBNo = Convert.ToString(Result["SBNo"]);
                    objCCINEntry.SBDate = Convert.ToString(Result["SBDate"]);
                    objCCINEntry.SBType = Convert.ToInt32(Result["SBType"]);
                    objCCINEntry.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                    objCCINEntry.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCCINEntry.CHAName = Convert.ToString(Result["CHAName"]);
                    objCCINEntry.ConsigneeName = Convert.ToString(Result["ConsigneeName"]);
                    objCCINEntry.ConsigneeAdd = Convert.ToString(Result["ConsigneeAdd"]);
                    objCCINEntry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    objCCINEntry.StateId = Convert.ToInt32(Result["StateId"]);
                    objCCINEntry.CityId = Convert.ToInt32(Result["CityId"]);
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    //objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                    //objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                    //objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                    objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                }


                if (Status == 1)
                {
                    _DBResponse.Data = objCCINEntry;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetCCINEntryForEdit(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            //lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINDetForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntry objCCINEntry = new CCINEntry();

            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                    // objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    //objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    objCCINEntry.CCINNo = Convert.ToString(Result["CCINNo"]);
                    objCCINEntry.CCINDate = Convert.ToString(Result["CCINDate"]);
                    objCCINEntry.SBNo = Convert.ToString(Result["SBNo"]);
                    objCCINEntry.SBDate = Convert.ToString(Result["SBDate"]);
                    objCCINEntry.SBType = Convert.ToInt32(Result["SBType"]);
                    objCCINEntry.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                    objCCINEntry.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCCINEntry.CHAName = Convert.ToString(Result["CHAName"]);
                    objCCINEntry.ConsigneeName = Convert.ToString(Result["ConsigneeName"]);
                    objCCINEntry.ConsigneeAdd = Convert.ToString(Result["ConsigneeAdd"]);
                    objCCINEntry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    objCCINEntry.StateId = Convert.ToInt32(Result["StateId"]);
                    objCCINEntry.CityId = Convert.ToInt32(Result["CityId"]);
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    //objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                    //objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                    //objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                    objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                    objCCINEntry.SCMTRPackageType = Convert.ToString(Result["SCMTRPackageType"]);

                    objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                    objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]);
                    objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);

                    //objCCINEntry.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                }
                /* if (Result.NextResult())
                 {
                     while (Result.Read())
                     {
                         objCCINEntry.lstPostPaymentChrg.Add(new PostPaymentCharge()
                         {
                             ChargeId = objCCINEntry.lstPostPaymentChrg.Count + 1,
                             OperationId = Convert.ToInt32(Result["OperationId"]),
                             Clause = "",
                             ChargeName = Convert.ToString(Result["ChargeName"]),
                             ChargeType = Convert.ToString(Result["ChargeType"]),
                             SACCode = Convert.ToString(Result["SACCode"]),
                             Quantity = Convert.ToInt32(Result["Quantity"]),
                             Rate = Convert.ToDecimal(Result["Rate"]),
                             Amount = Convert.ToDecimal(Result["Amount"]),
                             Discount = 0,
                             Taxable = 0,
                             CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                             SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                             IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                             CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                             SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                             IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                             Total = Convert.ToDecimal(Result["Total"]),
                             ActualFullCharge = Convert.ToDecimal(Result["ActualFullCharge"]),
                         });
                     }
                     objCCINEntry.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                     if (objCCINEntry.lstPostPaymentChrg.Count > 0)
                     {
                         objCCINEntry.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(objCCINEntry.lstPostPaymentChrg).ToString();
                     }
                 }
                 if (Result.NextResult())
                 {
                     while (Result.Read())
                     {
                         objCCINEntry.PartySDBalance = Convert.ToDecimal(Result["SDBalance"]);
                     }

                 }*/

                if (Status == 1)
                {
                    _DBResponse.Data = objCCINEntry;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetCargoDetBTTById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.Int32, Value = Id });
            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetBTTById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CHNBTTCargoDet objBTTCargo = new CHNBTTCargoDet();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["GrossWeight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["Fob"]);

                }


                if (Status == 1)
                {
                    _DBResponse.Data = objBTTCargo;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetCargoDetShiftById(string Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.VarChar, Value = Id });
            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetShiftById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PPGBTTCargoDet objBTTCargo = new PPGBTTCargoDet();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["Weight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["FobValue"]);
                    objBTTCargo.exporter = Convert.ToString(Result["Exporter"]);

                }


                if (Status == 1)
                {
                    _DBResponse.Data = objBTTCargo;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DeleteCCINEntry(int CCINEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINEntryId", MySqlDbType = MySqlDbType.Int32, Value = CCINEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCCINEntry", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Can't be Deleted as It Is Used In Gate Entry.";
                    _DBResponse.Status = 2;
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Can't be Deleted as It Is Used In CCIN Entry Approval.";
                    _DBResponse.Status = 2;
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
        public void GetAllCCINEntryApprovalForSearch(int Page, string searchtext)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "p_in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "search", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = searchtext });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryApprovalForSearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntry> CCINEntryList = new List<CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntry objCCINEntry = new CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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

        public void GetCcinEntrySlipForPrint(string ccin)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.VarChar, Value = ccin });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoCartedInSlipPrint", CommandType.StoredProcedure, DParam);
            CHN_CCINPrint ObjStuffing = new CHN_CCINPrint();
            // WFLD_ContainerStuffingDtl lstcont = new WFLD_ContainerStuffingDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.CCINNO = Result["CCINNO"].ToString();
                    ObjStuffing.CCINDate = Result["CCINDate"].ToString();
                    ObjStuffing.SBNo = Result["SBNo"].ToString();
                    ObjStuffing.SBDate = (Result["SBDate"] == null ? "" : Result["SBDate"]).ToString();
                    ObjStuffing.InvoiceNo = (Result["InvoiceNo"] == null ? "" : Result["InvoiceNo"]).ToString();
                    ObjStuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjStuffing.Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString();
                    ObjStuffing.GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString();
                    ObjStuffing.CargoType = (Result["CargoType"] == null ? "" : Result["CargoType"]).ToString();
                    ObjStuffing.NoofPkg = Convert.ToDecimal(Result["NoofPkg"] == DBNull.Value ? 0 : Result["NoofPkg"]);
                    ObjStuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjStuffing.FOB = Convert.ToDecimal(Result["FOB"] == DBNull.Value ? 0 : Result["FOB"]);
                 //   ObjStuffing.CargoInvNo = Result["CargoInvNo"].ToString();
                //    ObjStuffing.CargoInvDt = Result["CargoInvDt"].ToString();
                ObjStuffing.PackageType= Result["PackageType"].ToString();
                    ObjStuffing.Country = Result["CountryName"].ToString();
                    ObjStuffing.PortDestName = Result["PortDestName"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.Lstshed.Add(new CHN_ShedEntries
                        {
                            CartingDate = (Result["CartingDate"] == null ? "" : Result["CartingDate"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            SpaceType = Result["SpaceType"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NoOfPkg = Convert.ToDecimal(Result["NoOfPkg"] == DBNull.Value ? 0 : Result["NoOfPkg"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            ShortPkg = Convert.ToDecimal(Result["ShortPkg"] == DBNull.Value ? 0 : Result["ShortPkg"]),
                            ExcessPkg = Convert.ToDecimal(Result["ExcessPkg"] == DBNull.Value ? 0 : Result["ExcessPkg"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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


        public void GetMCIN(string SBNo, string SBDATE)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = SBNo });


            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(SBDATE).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMCIN", CommandType.StoredProcedure, DParam);
            LEOPage leo = new LEOPage();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    leo.Id = Convert.ToInt32(Result["id"]);
                    leo.MCIN = Result["MCIN"].ToString();

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { leo };
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
        public void AddEditLEOEntry(LEOPage objLEOPage)
        {

            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objLEOPage.Id });

            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.String, Value = objLEOPage.ShipBillNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objLEOPage.ShipBillDate).ToString("yyyy-MM-dd") });

            lstParam.Add(new MySqlParameter { ParameterName = "in_MCIN", MySqlDbType = MySqlDbType.String, Value = objLEOPage.MCIN });


            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });


            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditLEOEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {

                    _DBResponse.Message = (Result == 1) ? "LEO Entry Saved Successfully" : "LEO Entry Updated Successfully";
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


        public void GetAllLEOEntryBYID(int id)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = id });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                // List<LEOPage> LEOPageList = new List<LEOPage>();
                LEOPage LEOPageEntry = new LEOPage();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        // LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageEntry;
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

        public void GetAllLEOEntryBYSBMCIN(string SERCHVALUE)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_SERCHVALUE", MySqlDbType = MySqlDbType.String, Size = 11, Value = SERCHVALUE });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOSerch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LEOPage> LEOPageList = new List<LEOPage>();
                //LEOPage LEOPageEntry = new LEOPage();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);
                        LEOPageList.Add(LEOPageEntry);

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageList;
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

        public void GetAllLEOEntryForPage()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LEOPage> LEOPageList = new List<LEOPage>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);

                        LEOPageList.Add(LEOPageEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageList;
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

        #region Stuffing Request
        public void AddEditStuffingRequest(CHN_StuffingRequest ObjStuffing, string StuffingXML, string StuffingContrXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingHdrLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ShippingHdrLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ForwarderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RequestDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.RequestDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.StuffingType });
            //mks
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ForeignLiner });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Via });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Voyage });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = (ObjStuffing.PortId==null?0: ObjStuffing.PortId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = (ObjStuffing.Distance == null ? 0 : ObjStuffing.Distance) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TypeOfTrip", MySqlDbType = MySqlDbType.Int32, Value = (ObjStuffing.TypeOfTrip == null ? 0 : ObjStuffing.TypeOfTrip) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.Int32, Value = (ObjStuffing.Movement == null ? 0 : ObjStuffing.Movement) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = (ObjStuffing.CityId == null ? 0 : ObjStuffing.CityId) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingXML", MySqlDbType = MySqlDbType.Text, Value = StuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingContrXML", MySqlDbType = MySqlDbType.Text, Value = StuffingContrXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditStuffingRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.StuffingReqId == 0 ? "Stuffing Request Details Saved Successfully" : "Stuffing Request Details Updated Successfully");
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Request Details Already Exist";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Stuffing Request Details As It Already Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void DeleteStuffingRequest(int StuffingReqId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteStuffingRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Request Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Stuffing Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void GetAllStuffingRequest(int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequest> LstStuffing = new List<CHN_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequest
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString(),
                        Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetStuffingRequest(int StuffingReqId, int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_StuffingRequest ObjStuffing = new CHN_StuffingRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.StuffingReqNo = Result["StuffingReqNo"].ToString();
                    ObjStuffing.StuffingType = Result["StuffingType"].ToString();
                    ObjStuffing.RequestDate = Result["RequestDate"].ToString();
                    ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjStuffing.CHA = Result["CHA"].ToString();
                    ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                    ObjStuffing.ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString();
                    ObjStuffing.Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString();
                    ObjStuffing.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]);
                    ObjStuffing.Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]);
                    ObjStuffing.StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]);


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new CHN_StuffingRequestDtl
                        {
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"].ToString()),
                            ShippingDate = Result["ShippingDate"].ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            //  ContainerNo = Result["ContainerNo"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            //  Size = Convert.ToString(Result["Size"]),
                            //  ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            RQty = Convert.ToInt32(Result["RQty"]),
                            RWt = Convert.ToDecimal(Result["RWt"]),
                            //  StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            //  StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Exporter = Result["Exporter"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            //  ShippingLine = Result["ShippingLine"].ToString(),
                            // CFSCode = Result["CFSCode"].ToString()
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new CHN_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
        public void GetCartRegNoForStuffingReq(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegNoForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequest> LstStuffing = new List<CHN_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequest
                    {
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetCartRegDetForStuffingReq(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequestDtl> LstStuffing = new List<CHN_StuffingRequestDtl>();
            List<CHN_StuffingReqContainerDtl> LstStuffingContr = new List<CHN_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),

                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHA = Result["CHA"].ToString(),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new CHN_StuffingReqContainerDtl
                        {
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstStuffing = LstStuffing, ContainerDetails = LstStuffingContr };
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
        public void GetAllContainerNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<CHN_StuffingReqContainerDtl> LstStuffing = new List<CHN_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingReqContainerDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetContainerNoDet(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNo", CommandType.StoredProcedure, DParam);
            // StuffingRequestDtl ObjStuffing = new StuffingRequestDtl();
            CHN_StuffingReqContainerDtl ObjStuffing = new CHN_StuffingReqContainerDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjStuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    // ObjStuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjStuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjStuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjStuffing.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjStuffing.CFSCode = Result["CFSCode"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
        public void GetShippingBillNoOfCartApp(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            List<CHN_StuffingRequestDtl> LstBillingNo = new List<CHN_StuffingRequestDtl>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingBillNoOfCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBillingNo.Add(new CHN_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBillingNo;
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


        public void ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ShippinglineDtlAfterEmptyCont", CommandType.StoredProcedure, DParam);
            CHN_StuffingReqContainerDtl ObjSR = new CHN_StuffingReqContainerDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSR.ShippingLineId = Convert.ToInt32(Result["ToShippingId"]);
                    ObjSR.ShippingLine = Result["ShippingLine"].ToString();
                    ObjSR.Size = Result["Size"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSR;
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
        public void GetShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.ShippingLine> LstShippingLine = new List<Areas.Export.Models.ShippingLine>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new Areas.Export.Models.ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstShippingLine;
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

        public void GetForwarder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetForwarder", CommandType.StoredProcedure, DParam);
            List<CHN_ForwarderList> LstForwarder = new List<CHN_ForwarderList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstForwarder.Add(new CHN_ForwarderList
                    {
                        ForwarderId = Convert.ToInt32(Result["ShippingLineId"]),
                        Forwarder = Result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstForwarder;
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
        //public void ListOfExporter()
        //{
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
        //    IList<Exporter> lstExporter = new List<Exporter>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstExporter.Add(new Exporter
        //            {
        //                EXPEximTraderId = Convert.ToInt32(result["EximTraderId"]),
        //                ExporterName = result["EximTraderName"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstExporter;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Status = 1;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Status = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Status = 0;
        //    }
        //    finally
        //    {
        //        result.Dispose();
        //        result.Close();
        //    }
        //}
        //public void ListOfCHA()
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] dparam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
        //    IList<CHA> lstCHA = new List<CHA>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstCHA.Add(new CHA
        //            {
        //                CHAEximTraderId = Convert.ToInt32(result["EximTraderId"]),
        //                CHAName = result["EximTraderName"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstCHA;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Status = 1;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Status = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Status = 0;
        //    }
        //    finally
        //    {
        //        result.Dispose();
        //        result.Close();
        //    }
        //}
        public void CHN_GetStuffingRequest(int StuffingReqId, int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_StuffingRequest ObjStuffing = new CHN_StuffingRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.StuffingReqNo = Result["StuffingReqNo"].ToString();
                    ObjStuffing.StuffingType = Result["StuffingType"].ToString();

                    ObjStuffing.ForeignLiner = Result["ForeignLiner"].ToString();
                    ObjStuffing.Vessel = Result["Vessel"].ToString();
                    ObjStuffing.Via = Result["Via"].ToString();
                    ObjStuffing.Voyage = Result["Voyage"].ToString();

                    ObjStuffing.RequestDate = Result["RequestDate"].ToString();
                    ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjStuffing.ShippingHdrLineId = Convert.ToInt32(Result["ShippingHdrLineId"]);
                    ObjStuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjStuffing.CHA = Result["CHA"].ToString();
                    ObjStuffing.Forwarder = Result["Forwarder"].ToString();
                    ObjStuffing.ShippingHdrLine = Result["ShippingHdrLine"].ToString();
                    //ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                    ObjStuffing.PortId = Convert.ToInt32(Result["PortId"]);
                    ObjStuffing.Distance = Convert.ToDecimal(Result["Distance"]);
                    ObjStuffing.TypeOfTrip = Convert.ToInt32(Result["TypeOfTrip"]);
                    ObjStuffing.Movement = Convert.ToInt32(Result["Movement"]);
                    ObjStuffing.CityId = Convert.ToInt32(Result["CityId"]);
                    ObjStuffing.CityName = Result["CityName"].ToString();
                    ObjStuffing.PortName = Result["PortName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new CHN_StuffingRequestDtl
                        {
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            CommInvNo = (Result["ComInv"] == null ? "" : Result["ComInv"].ToString()),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"].ToString()),
                            ShippingDate = Result["ShippingDate"].ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            //  ContainerNo = Result["ContainerNo"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            //  Size = Convert.ToString(Result["Size"]),
                            //  ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            //  StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            //  StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Exporter = Result["Exporter"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                            CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                            //  ShippingLine = Result["ShippingLine"].ToString(),
                            // CFSCode = Result["CFSCode"].ToString()
                            RQty = Convert.ToInt32(Result["RQty"]),
                            RWt = Convert.ToDecimal(Result["RWt"]),
                            PackUQCCode = Result["PackUQCCode"].ToString(),
                            PackUQCDescription= Result["PackUQCDesc"].ToString(),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new CHN_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
        public void CHN_GetCartRegDetForStuffingReq(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequestDtl> LstStuffing = new List<CHN_StuffingRequestDtl>();
            List<CHN_StuffingReqContainerDtl> LstStuffingContr = new List<CHN_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommInvNo = Result["CommInvNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHA = Result["CHA"].ToString(),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        RQty = Convert.ToInt32(Result["RemainingUnits"]),
                        RWt = Convert.ToDecimal(Result["RemainingWeight"]),
                        PackUQCCode= Result["PackUQCCode"].ToString(),
                        PackUQCDescription= Result["PackUQCDesc"].ToString(),

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new CHN_StuffingReqContainerDtl
                        {
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            ShippingLine = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstStuffing = LstStuffing, ContainerDetails = LstStuffingContr };
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
        public void GetAllStuffingRequest(int RoleId, int Uid, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequest> LstStuffing = new List<CHN_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequest
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString(),
                        Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]),
                        SBNO = Convert.ToString(Result["SBNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        public void SearchStuffingRequest(int RoleId, int Uid, string ContNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_StuffingRequest> LstStuffing = new List<CHN_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_StuffingRequest
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString(),
                        Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]),
                        SBNO = Convert.ToString(Result["SBNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        #region Container Stuffing
        public void AddEditContainerStuffing(Chn_ContainerStuffing ObjStuffing, string ContainerStuffingXML)//, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
                                                                                                                             // string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.FinalDestinationLocationId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjStuffing.FinalDestinationLocation) });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.Text, Value = SCMTRXML });

            /*  LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREPartyCode });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREOperationId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeType });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeName });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRECharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRETotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRETotalAmount });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GRESACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRESACCode });

              LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSPartyId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSPartyCode });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSOperationId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeType });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeName });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSTotalAmount });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSSACCode });

              LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOPartyId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOPartyCode });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOOperationId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeType });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeName });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOTotalAmount });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOSACCode });

              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingPartyId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingPartyCode });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingOperationId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeType });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeName });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingTotalAmount });
              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingSACCode });

              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPPartyId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPPartyCode });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPOperationId });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeType });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeName });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTCharge });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTPer });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPTotalAmount });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPSACCode });

              LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

              LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

              LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

              LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
              LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });*/

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceCSList inv = new PPG_InvoiceCSList();
            try
            {

                if (Result == 1)
                {
                    String[] invobj;
                    invobj = ReturnObj.Split(',');
                    if (invobj.Length > 0)
                        inv.invoicenoGRE = invobj[0]; //InvoiceGRE;

                    if (invobj.Length > 1)
                        inv.invoicenoINS = invobj[1];// InvoiceINS;
                    if (invobj.Length > 2)
                        inv.invoicenoSTO = invobj[2];// InvoiceSTO;
                    if (invobj.Length > 3)
                        inv.invoicenoHND = invobj[3];// InvoiceHND;
                    if (invobj.Length > 4)
                        inv.invoicenoGENSP = invobj[4];
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Container Stuffing Details Saved Successfully" : "Container Stuffing Details Updated Successfully");
                }

                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Stuffing Details Already Exist";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        public void ListOfPOD()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPOD", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListOfCityForStuffingReq()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CityListForStuffReq", CommandType.StoredProcedure);
            List<City> City = new List<City>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    City.Add(new City
                    {
                        CityName = Convert.ToString(Result["CityName"]),
                        CityId = Convert.ToInt32(Result["CityId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = City;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerDetForStuffing(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 35, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetForStuffing", CommandType.StoredProcedure, DParam);
            ContainerStuffingDtl ObjStuffing = new ContainerStuffingDtl();
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            _DBResponse = new DatabaseResponse();
            int PortId = 0;string PortName = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    PortId = Convert.ToInt32(Result["PortId"]);
                    PortName = Result["PortName"].ToString();
                    LstStuffing.Add(new ContainerStuffingDtl
                    {
                        StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        StuffingType = Convert.ToString(Result["StuffingType"] == null ? "" : Result["StuffingType"]),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CustomSeal = (Result["CustomSeal"] == null ? "" : Result["CustomSeal"]).ToString(),
                        ShippingSeal = (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString(),
                        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                        ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                        Consignee = (Result["ConsigneeName"] == null ? "" : Result["ConsigneeName"]).ToString(),
                        SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                        spacetype = Result["spacetype"].ToString(),
                        EquipmentQUC = Result["EquipmentQUC"].ToString(),
                        EquipmentSealType = Result["EquipmentSealType"].ToString(),
                        EquipmentStatus = Result["EquipmentStatus"].ToString(),
                        MCINPCIN = Result["MCINPCIN"].ToString(),
                        SEZ=Convert.ToInt32(Result["SEZ"])

                        //    LstContainer.Add(new ContainerStuffingDtl
                        //    {
                        //        ContainerNo = Result["ContainerNo"].ToString(),
                        //        CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                        //        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                        //        //ObjStuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                        //        StuffingType = Convert.ToString(Result["StuffingType"] == null ? "" : Result["StuffingType"]),
                        //        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        //        CustomSeal = (Result["CustomSeal"] == null ? "" : Result["CustomSeal"]).ToString(),
                        //        ShippingSeal = (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString(),
                        //        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                        //        ShippingDate = (Result["ShippingDate"] == null ? "": Result["ShippingDate"]).ToString(),
                        //        //ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                        //        // ObjStuffing.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                        //        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        //        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        //        StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                        //        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                        //        Exporter = Result["Exporter"].ToString(),
                        //        CHA = Result["CHA"].ToString()
                        //});
                    });
                }
                if (Status == 1)
                {
                    ObjStuffing.LstStuffing = LstStuffing;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data =new { ObjStuffing, PortId, PortName };
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

        public void GetAllContainerStuffing(int Uid, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        ShipBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetContainerStuffing(int ContainerStuffingId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            Chn_ContainerStuffing ObjStuffing = new Chn_ContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingDate = (Result["StuffingDate"] == null ? "" : Result["StuffingDate"]).ToString();
                    ObjStuffing.ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]);
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjStuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"] == null ? "" : Result["StuffingReqNo"]);
                    ObjStuffing.RequestDate = (Result["RequestDate"] == null ? "" : Result["RequestDate"]).ToString();
                    ObjStuffing.DirectStuffing = Convert.ToBoolean(Result["DirectStuffing"]);
                    ObjStuffing.TransportMode = Convert.ToInt32(Result["TransportMode"]);
                    ObjStuffing.ContOrigin = Convert.ToString(Result["Origin"] == null ? "" : Result["Origin"]);
                    ObjStuffing.ContVia = Convert.ToString(Result["Via"] == null ? "" : Result["Via"]);
                    ObjStuffing.ContPOL = Convert.ToString(Result["POL"] == null ? "" : Result["POL"]);
                    ObjStuffing.POD = Convert.ToString(Result["POD"] == null ? "" : Result["POD"]);
                    ObjStuffing.POLName = Convert.ToString(Result["POLName"] == null ? "" : Result["POLName"]);
                    ObjStuffing.FinalDestinationLocationId = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    ObjStuffing.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new ContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
                            Refer = Convert.ToInt32(Result["Refer"] == DBNull.Value ? 0 : Result["Refer"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            ShippingSeal = Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                            MCINPCIN = Result["MCINPCIN"].ToString(),

                            StuffingCargoType = Convert.ToString(Result["StuffingCargoType"]),
                            StuffingMethod = Convert.ToString(Result["StuffingMethod"]),
                        });
                    }
                }


             

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjStuffing.GREOperationId = Convert.ToInt32(Result["OperationId"]);
                        ObjStuffing.GREPartyId = Convert.ToInt32(Result["PartyId"]);
                        ObjStuffing.GREPartyCode = Convert.ToString(Result["PartyCode"]);
                        ObjStuffing.GREChargeType = Convert.ToString(Result["ChargeType"]);
                        ObjStuffing.GREChargeName = Convert.ToString(Result["ChargeName"]);
                        ObjStuffing.GRECharge = Convert.ToDecimal(Result["Charge"]);
                        ObjStuffing.GRECGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        ObjStuffing.GRESGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        ObjStuffing.GREIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        ObjStuffing.GREIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        ObjStuffing.GRECGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        ObjStuffing.GRESGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        //ObjStuffing.GREAmount = Convert.ToDecimal(Result["Amount"]);
                        //ObjStuffing.GRETaxable = Convert.ToDecimal(Result["Taxable"]);
                        ObjStuffing.GRESACCode = Convert.ToString(Result["SACCode"]);
                        ObjStuffing.GRETotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        ObjStuffing.InvoiceNoGRE = Convert.ToString(Result["InvoiceNo"]);
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjStuffing.INSOperationId = Convert.ToInt32(Result["OperationId"]);
                        ObjStuffing.INSPartyId = Convert.ToInt32(Result["PartyId"]);
                        ObjStuffing.INSPartyCode = Convert.ToString(Result["PartyCode"]);
                        ObjStuffing.INSChargeType = Convert.ToString(Result["ChargeType"]);
                        ObjStuffing.INSChargeName = Convert.ToString(Result["ChargeName"]);
                        ObjStuffing.INSCharge = Convert.ToDecimal(Result["Charge"]);
                        ObjStuffing.INSCGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        ObjStuffing.INSSGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        ObjStuffing.INSIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        ObjStuffing.INSIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        ObjStuffing.INSCGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        ObjStuffing.INSSGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        //ObjStuffing.INSAmount = Convert.ToDecimal(Result["Amount"]);
                        //ObjStuffing.INSTaxable = Convert.ToDecimal(Result["Taxable"]);
                        ObjStuffing.INSSACCode = Convert.ToString(Result["SACCode"]);
                        ObjStuffing.INSTotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        ObjStuffing.InvoiceNoINS = Convert.ToString(Result["InvoiceNo"]);
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjStuffing.STOOperationId = Convert.ToInt32(Result["OperationId"]);
                        ObjStuffing.STOPartyId = Convert.ToInt32(Result["PartyId"]);
                        ObjStuffing.STOPartyCode = Convert.ToString(Result["PartyCode"]);
                        ObjStuffing.STOChargeType = Convert.ToString(Result["ChargeType"]);
                        ObjStuffing.STOChargeName = Convert.ToString(Result["ChargeName"]);
                        ObjStuffing.STOCharge = Convert.ToDecimal(Result["Charge"]);
                        ObjStuffing.STOCGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        ObjStuffing.STOSGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        ObjStuffing.STOIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        ObjStuffing.STOIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        ObjStuffing.STOCGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        ObjStuffing.STOSGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        //ObjStuffing.STOAmount = Convert.ToDecimal(Result["Amount"]);
                        //ObjStuffing.STOTaxable = Convert.ToDecimal(Result["Taxable"]);
                        ObjStuffing.STOSACCode = Convert.ToString(Result["SACCode"]);
                        ObjStuffing.STOTotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        ObjStuffing.InvoiceNoSTO = Convert.ToString(Result["InvoiceNo"]);
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjStuffing.HandalingOperationId = Convert.ToInt32(Result["OperationId"]);
                        ObjStuffing.HandalingPartyId = Convert.ToInt32(Result["PartyId"]);
                        ObjStuffing.HandalingPartyCode = Convert.ToString(Result["PartyCode"]);
                        ObjStuffing.HandalingChargeType = Convert.ToString(Result["ChargeType"]);
                        ObjStuffing.HandalingChargeName = Convert.ToString(Result["ChargeName"]);
                        ObjStuffing.HandalingCharge = Convert.ToDecimal(Result["Charge"]);
                        ObjStuffing.HandalingCGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        ObjStuffing.HandalingSGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        ObjStuffing.HandalingIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        ObjStuffing.HandalingIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        ObjStuffing.HandalingCGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        ObjStuffing.HandalingSGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        //ObjStuffing.HandalingAmount = Convert.ToDecimal(Result["Amount"]);
                        //ObjStuffing.HandalingTaxable = Convert.ToDecimal(Result["Taxable"]);
                        ObjStuffing.HandalingSACCode = Convert.ToString(Result["SACCode"]);
                        ObjStuffing.HandalingTotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        ObjStuffing.InvoiceNoHND = Convert.ToString(Result["InvoiceNo"]);
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjStuffing.GENSPOperationId = Convert.ToInt32(Result["OperationId"]);
                        ObjStuffing.GENSPPartyId = Convert.ToInt32(Result["PartyId"]);
                        ObjStuffing.GENSPPartyCode = Convert.ToString(Result["PartyCode"]);
                        ObjStuffing.GENSPChargeType = Convert.ToString(Result["ChargeType"]);
                        ObjStuffing.GENSPChargeName = Convert.ToString(Result["ChargeName"]);
                        ObjStuffing.GENSPCharge = Convert.ToDecimal(Result["Charge"]);
                        ObjStuffing.GENSPCGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        ObjStuffing.GENSPSGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        ObjStuffing.GENSPIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        ObjStuffing.GENSPIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        ObjStuffing.GENSPCGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        ObjStuffing.GENSPSGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        //ObjStuffing.HandalingAmount = Convert.ToDecimal(Result["Amount"]);
                        //ObjStuffing.HandalingTaxable = Convert.ToDecimal(Result["Taxable"]);
                        ObjStuffing.GENSPSACCode = Convert.ToString(Result["SACCode"]);
                        ObjStuffing.GENSPTotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        ObjStuffing.InvoiceNoGENSP = Convert.ToString(Result["InvoiceNo"]);
                    }
                }





                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
        public void GetReqNoForContainerStuffing(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReqNoForContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        RequestDate=Result["RequestDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetContainerNoByStuffingReq(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoByStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new ContainerStuffingDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void DeleteContainerStuffing(int ContainerStuffingId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Container Stuffing Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Container Stuffing Details As It Exist In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Container Stuffing Details As Next Invoice Generated";
                    _DBResponse.Data = null;
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
        }

        public void GetContainerStuffForPrint(int ContainerStuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffForPrint", CommandType.StoredProcedure, DParam);
            Chn_ContainerStuffing ObjStuffing = new Chn_ContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingDate = Result["StuffingDate"].ToString();
                    ObjStuffing.ContVia = Result["Via"].ToString();
                    ObjStuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjStuffing.CargoType = (Result["CargoType"] == null ? "" : Result["CargoType"]).ToString();
                    ObjStuffing.ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString();
                    ObjStuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjStuffing.Vessel = Result["Vessel"].ToString();
                    ObjStuffing.Voyage = Result["Voyage"].ToString();
                    ObjStuffing.Via = Result["Via"].ToString();
                    ObjStuffing.FinalDestinationLocation= Result["CustodianCode"].ToString();
                }
                if (Result.NextResult())
                {
                    ObjStuffing.Size = "";
                    while (Result.Read())
                    {
                        ObjStuffing.Size += Result["Size"].ToString() + ",";

                    }
                    ObjStuffing.Size = ObjStuffing.Size.Remove(ObjStuffing.Size.Length - 1);
                    ObjStuffing.ShippingLineNo = (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgStuffingDtl.Add(new PPG_ContainerStuffingDtl
                        {
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            EntryNo = (Result["EntryNo"] == null ? "" : Result["EntryNo"]).ToString(),
                            InDate = (Result["InDate"] == null ? "" : Result["InDate"]).ToString(),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            ShippingSeal= Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToInt32(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            PortName = (Result["PortName"] == null ? "" : Result["PortName"]).ToString(),
                            PortDestination = (Result["PortDestination"] == null ? "" : Result["PortDestination"]).ToString(),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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

        public void ListOfGREParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfGREParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        GREPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        GREPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        public void CalculateGroundRentEmpty(String StuffingDate, String ContainerStuffingXML, int GREPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GREPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(StuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRentEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                Chn_ContainerStuffing ObjCS = new Chn_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.GREOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.GREChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.GREChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.GRECharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GRECGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.GRESGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.GREIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.GREIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.GRECGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.GRESGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.GREAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GRETaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.GRETotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.GRESACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREOperationCFSCodeWiseAmtLst.Add(new GREOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREContainerWiseAmtLst.Add(new GREContainerWiseAmt
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            GrEmpty = Convert.ToDecimal(result["GrEmpty"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                result.Dispose();
                result.Close();
            }
        }


        internal void ListOfINSParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfINSParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        INSPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        INSPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        internal void CalculateINS(string stuffingDate, String ContainerStuffingXML, int iNSPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(iNSPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateInsurence", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                Chn_ContainerStuffing ObjCS = new Chn_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.INSOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.INSChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.INSChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.INSCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.INSCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.INSSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.INSIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.INSIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.INSCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.INSSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.INSAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.INSTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.INSTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.INSSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSOperationCFSCodeWiseAmtLst.Add(new INSOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSContainerWiseAmtLst.Add(new INSContainerWiseAmt
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            InsuranceCharge = Convert.ToDecimal(result["InsuranceCharge"])
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.LstppgShipDtl.Add(new PPG_ShippingBillNo
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            ShippingBillNo = (result["ShippingBillNo"] == null ? "" : result["ShippingBillNo"]).ToString(),
                            ShippingDate = result["ShippingDate"].ToString(),
                            FOB = Convert.ToDecimal(result["FOB"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                result.Dispose();
                result.Close();
            }
        }

        internal void ListOfSTOParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfSTOParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        STOPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        STOPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        internal void CalculateSTO(string stuffingDate, String ContainerStuffingXML, int StoPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(StoPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "EXPCSSTO" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateStorageCharge", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();

            try
            {
                Chn_ContainerStuffing ObjCS = new Chn_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.STOOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.STOChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.STOChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.STOCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.STOCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.STOSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.STOIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.STOIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.STOCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.STOSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.STOAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.STOTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.STOTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.STOSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtl
                        {
                            BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                            BOEDate = Convert.ToDateTime(result["BOEDate"]),
                            BOLNo = Convert.ToString((result["BOENo"])),
                            BOLDate = Convert.ToDateTime(result["BOLDate"]),
                            CargoDescription = Convert.ToString(result["CargoDescription"]),
                            GodownId = Convert.ToInt32(result["GodownId"]),
                            GodownName = Convert.ToString(result["GodownName"] == null ? "" : result["GodownName"]),
                            GdnWiseLctnIds = Convert.ToString(result["GdnWiseLctnIds"]),
                            GdnWiseLctnNames = Convert.ToString(result["GdnWiseLctnNames"]),
                            CargoType = Convert.ToInt32(result["CargoType"] == null ? "" : result["CargoType"]),
                            DestuffingDate = Convert.ToDateTime(result["DestuffingDate"] == null ? "" : result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(result["CartingDate"] == null ? "" : result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(result["GrossWt"] == null ? "" : result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToDecimal(result["SpaceOccupiedUnit"] == null ? "" : result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            Duty = Convert.ToDecimal(result["Duty"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOOperationCFSCodeWiseAmtLst.Add(new STOOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                result.Dispose();
                result.Close();
            }
        }


        internal void ListOfHandalingParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfHandalingParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        HandalingPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        HandalingPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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


        internal void ListOfGENSPParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfHandalingParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        GENSPPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        GENSPPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        internal void CalculateHandaling(string stuffingDate, String Origin, String Via, string ContainerStuffingXML, int HandalingPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HandalingPartyId) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TMode", MySqlDbType = MySqlDbType.VarChar, Value = TMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = Origin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = Via });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateHandallingCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                Chn_ContainerStuffing ObjCS = new Chn_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.HandalingOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.HandalingChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.HandalingChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.HandalingCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.HandalingCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.HandalingSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.HandalingIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.HandalingIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.HandalingCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.HandalingSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.HandalingAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.HandalingTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.HandalingTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.HandalingSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.HNDOperationCFSCodeWiseAmtLst.Add(new HNDOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                result.Dispose();
                result.Close();
            }
        }

        internal void CalculateGENSP(string stuffingDate, string ContainerStuffingXML, int GENSPPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GENSPPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "EXPCSGEN" });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGENSPCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                Chn_ContainerStuffing ObjCS = new Chn_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.GENSPOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.GENSPChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.GENSPChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.GENSPCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GENSPCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.GENSPSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.GENSPIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.GENSPIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.GENSPCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.GENSPSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.GENSPAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GENSPTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.GENSPTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.GENSPSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtl
                        {
                            BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                            BOEDate = Convert.ToDateTime(result["BOEDate"]),
                            BOLNo = Convert.ToString((result["BOENo"])),
                            BOLDate = Convert.ToDateTime(result["BOLDate"]),
                            CargoDescription = Convert.ToString(result["CargoDescription"]),
                            GodownId = Convert.ToInt32(result["GodownId"]),
                            GodownName = Convert.ToString(result["GodownName"] == null ? "" : result["GodownName"]),
                            GdnWiseLctnIds = Convert.ToString(result["GdnWiseLctnIds"]),
                            GdnWiseLctnNames = Convert.ToString(result["GdnWiseLctnNames"]),
                            CargoType = Convert.ToInt32(result["CargoType"] == null ? "" : result["CargoType"]),
                            DestuffingDate = Convert.ToDateTime(result["DestuffingDate"] == null ? "" : result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(result["CartingDate"] == null ? "" : result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(result["GrossWt"] == null ? "" : result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToDecimal(result["SpaceOccupiedUnit"] == null ? "" : result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            Duty = Convert.ToDecimal(result["Duty"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GENSPOperationCFSCodeWiseAmtLst.Add(new GENSPOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.LstppgShipDtl.Add(new PPG_ShippingBillNo
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            ShippingBillNo = (result["ShippingBillNo"] == null ? "" : result["ShippingBillNo"]).ToString(),
                            ShippingDate = result["ShippingDate"].ToString(),
                            FOB = Convert.ToDecimal(result["AREA"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                result.Dispose();
                result.Close();
            }
        }
        public void SearchContainerStuffing(string ContNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        ShipBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        #region Container Stuffing Amendment

        public void ListOfStuffingNoForAmendment()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfStuffingNoForAmendment", CommandType.StoredProcedure, DParam);
            List<Chn_ContainerStuffing> Lstsr = new List<Chn_ContainerStuffing>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstsr.Add(new Chn_ContainerStuffing
                    {
                        StuffingNo = Convert.ToString(Result["StuffingNo"]),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        StuffingDate = Convert.ToString(Result["StuffingDate"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstsr;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetContainerStuffingDetails(int ContainerStuffingId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            Chn_ContainerStuffing ObjStuffing = new Chn_ContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjStuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"] == null ? "" : Result["StuffingReqNo"]);
                    ObjStuffing.RequestDate = (Result["RequestDate"] == null ? "" : Result["RequestDate"]).ToString();
                    ObjStuffing.DirectStuffing = Convert.ToBoolean(Result["DirectStuffing"]);
                    ObjStuffing.TransportMode = Convert.ToInt32(Result["TransportMode"]);
                    ObjStuffing.ContOrigin = Convert.ToString(Result["Origin"] == null ? "" : Result["Origin"]);
                    ObjStuffing.ContVia = Convert.ToString(Result["Via"] == null ? "" : Result["Via"]);
                    ObjStuffing.ContPOL = Convert.ToString(Result["POL"] == null ? "" : Result["POL"]);
                    ObjStuffing.POD = Convert.ToString(Result["POD"] == null ? "" : Result["POD"]);
                    ObjStuffing.POLName = Convert.ToString(Result["POLName"] == null ? "" : Result["POLName"]);
                    ObjStuffing.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"] == null ? "" : Result["FinalDestinationLocation"]);
                    ObjStuffing.FinalDestinationLocationId = Convert.ToInt32(Result["FinalDestinationLocationID"] == DBNull.Value ? 0 : Result["FinalDestinationLocationID"]);                  
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new ContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
                            Refer = Convert.ToInt32(Result["Refer"] == DBNull.Value ? 0 : Result["Refer"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            ShippingSeal = Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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

        public void AddEditAmendmentContainerStuffing(Chn_ContainerStuffing ObjStuffing, string ContainerStuffingXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.FinalDestinationLocationId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjStuffing.FinalDestinationLocation) });
           

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditAmendmentContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {

                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Amendment Container Stuffing Details Saved Successfully" : " Amendment Container Stuffing Details Updated Successfully");
                }

                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Stuffing Approval Already Done";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetAllAmendmentContainerStuffing(int Page, int Uid, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllAmendmentContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContainerStuffing> LstStuffing = new List<Chn_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        ShipBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendmentDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        #region Carting Application
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<CwcExim.Areas.Export.Models.CHA > lstCHA = new List<CwcExim.Areas.Export.Models.CHA > ();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CwcExim.Areas.Export.Models.CHA
                    {
                        CHAEximTraderId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void ListOfExporter()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
            IList<CwcExim.Areas.Export.Models.Exporter > lstExporter = new List<CwcExim.Areas.Export.Models.Exporter > ();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new CwcExim.Areas.Export.Models.Exporter
                    {
                        EXPEximTraderId = Convert.ToInt32(result["EximTraderId"]),
                        ExporterName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstExporter;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetAllCommodity()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.Commodity> LstCommodity = new List<Areas.Export.Models.Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Export.Models.Commodity
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString()
                        // CommodityAlias = (Result["CommodityAlias"] == null ? "" : Result["CommodityAlias"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCommodity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllCommodityForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType = Result["CommodityType"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCommodity, State };
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
        public void GetAllCartingApp(int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CartingList> LstCartingApp = new List<CartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingApp.Add(new CartingList
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ApplicationDate = Result["ApplicationDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingApp;
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
        public void GetCartingApp(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CartingApplication ObjCartingApp = new CartingApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCartingApp.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCartingApp.CartingNo = Result["CartingNo"].ToString();
                    ObjCartingApp.ApplicationNo = Result["ApplicationNo"].ToString();
                    ObjCartingApp.ApplicationDate = Result["ApplicationDate"].ToString().Split()[0];
                    ObjCartingApp.CHAEximTraderId = Convert.ToInt32(Result["CHAEximTraderId"]);
                    ObjCartingApp.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjCartingApp.CHAName = Result["CHAName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCartingApp.lstShipping.Add(new ShippingDetails
                        {
                            CartingAppDtlId = Convert.ToInt32(Result["CartingAppDtlId"]),
                            CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = Result["ShippingBillDate"].ToString(),
                            CommInvcNo = (Result["CommInvNo"] == null ? "" : Result["CommInvNo"]).ToString(),
                            PackingListNo = (Result["PackingListNo"] == null ? "" : Result["PackingListNo"]).ToString(),
                            Exporter = (Result["EximTraderName"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterEximTraderId"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            MarksAndNo = Result["MarksAndNo"].ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            FoBValue = Convert.ToDecimal(Result["FobValue"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Result["CommodityName"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCartingApp;
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
        public void AddEditCartingApp(CartingApplication objCA, int Uid)
        {
            string Param = "0", ReturnObj = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.CartingNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.ApplicationNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCA.ApplicationDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAEximTraderId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CHAEximTraderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objCA.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = objCA.StringifyData });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output }); ;
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = objCA.CartingAppId, Direction = ParameterDirection.Output });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditCartingApp", CommandType.StoredProcedure, dparam, out Param, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Carting Application Saved Successfully" : "Carting Application Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Carting Application As It Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }
        public void DeleteCartingApp(int CartingAppId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCartingApp", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In Carting Work Order.";
                    _DBResponse.Status = 2;
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In Another Page";
                    _DBResponse.Status = 3;
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
        public void PrintCartingApp(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintCrtngApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PrintCA> lstCA = new List<PrintCA>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCA.Add(new PrintCA
                    {
                        ShipBillNo = Result["ShippingBillNo"].ToString(),
                        ShipBillDate = Result["ShippingBillDate"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        MarksAndNo = Result["MarksAndNo"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstCA[0].CartingNo = Result["CartingNo"].ToString();
                        lstCA[0].CartingDt = Result["ApplicationDate"].ToString();
                        lstCA[0].CHAName = Result["CHAName"].ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCA;
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

        #region  Carting Register
        public void AddEditCartingRegister(Chn_CartingRegister objCR, string XML /*, string LocationXML,string ClearLocation=null*/)
        {
            string OutParam = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RegisterDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.RegisterDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objCR.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCR.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CHAId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objCR.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = objCR.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Value = objCR.SpaceType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_LocationXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = OutParam });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditCartingRegister", CommandType.StoredProcedure, Dparam, out OutParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1 ? "Carting Register Details Saved Successfully" : "Carting Register Details Updated Successfully");
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Data Already Exists";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Location Already Occupied";
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Cannot Update Carting Register Details As It Already Used In Next Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetAllRegisterDetails(int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            List<Chn_CartingRegister> lstCR = new List<Chn_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Chn_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
                        ShippingBill= result["ShippingBill"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCR;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetRegisterDetails(int CartingRegisterId, int Uid, string Purpose = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Purpose });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Uid });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            Chn_CartingRegister objCR = new Chn_CartingRegister();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objCR.CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]);
                    objCR.CartingAppId = Convert.ToInt32(result["CartingAppId"]);
                    objCR.CartingRegisterNo = result["CartingRegisterNo"].ToString();
                    //objCR.CartingAppId = Convert.ToInt32(result["CartingAppId"]);
                    objCR.RegisterDate = result["RegisterDate"].ToString();
                    objCR.ApplicationDate = result["ApplicationDate"].ToString();
                    objCR.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                    objCR.GodownName = result["GodownName"].ToString();
                    objCR.ApplicationNo = result["ApplicationNo"].ToString();
                    objCR.CHAName = result["CHANAME"].ToString();
                    objCR.CartingType = Convert.ToInt32(result["CartingType"]);
                    objCR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objCR.GodownId = Convert.ToInt32(result["GodownId"]);
                    objCR.CHAId = Convert.ToInt32(result["CHAId"]);
                    objCR.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objCR.ShippingLineName = result["ShippingLineName"].ToString();
                    objCR.SpaceType = result["SpaceType"].ToString();
                    objCR.IsShortCargo = Convert.ToInt32(result["IsShortCargo"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstRegisterDtl.Add(new Chn_CartingRegisterDtl
                        {
                            ShippingBillNo = result["ShippingBillNo"].ToString(),
                            ShippingDate = result["ShippingBillDate"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                            CommodityName = result["CommodityName"].ToString(),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            MarksAndNo = (result["MarksAndNo"] == null ? "" : result["MarksAndNo"]).ToString(),
                            NoOfUnits = Convert.ToInt32(result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(result["Weight"]),
                            FoBValue = Convert.ToDecimal(result["FobValue"]),
                            CUM = Convert.ToDecimal(result["CUM"]),
                            SQM = Convert.ToDecimal(result["SQM"]),
                            ActualQty = Convert.ToInt32(result["ActualQty"]),
                            ActualWeight = Convert.ToDecimal(result["ActualWeight"]),
                            Exporter = Convert.ToString(result["Exporter"]),
                            CartingAppDtlId = Convert.ToInt32(result["CartingAppDtlId"]),
                            LocationDetails = (result["LocationDetails"] == null ? "" : result["LocationDetails"]).ToString(),
                            Location = (result["Location"] == null ? "" : result["Location"]).ToString(),
                            ExporterId = Convert.ToInt32(result["ExporterId"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CartingRegisterDtlId = Convert.ToInt32(result["CartingRegisterDtlId"]),
                            SQMReserved= Convert.ToDecimal(result["SQMReserved"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstShortCargoDetails.Add(new ShortCargoDetails
                        {
                            ShortCargoDtlId = Convert.ToInt32(result["ShortCargoDtlId"]),
                            CartingDate = result["CartingDate"].ToString(),
                            Qty = Convert.ToInt32(result["Qty"]),
                            Weight = Convert.ToDecimal(result["Weight"]),
                            SQM = Convert.ToDecimal(result["SQM"]),
                            FOB = Convert.ToDecimal(result["FOB"]),
                        });
                    }
                }
                if (Purpose == "edit")
                {
                    if (result.NextResult())
                    {
                        while (result.Read())
                        {
                            objCR.lstGdnWiseLctn.Add(new Areas.Export.Models.GodownWiseLocation
                            {
                                // Row = result["Row"].ToString(),
                                LocationName = result["LocationName"].ToString(),
                                LocationId = Convert.ToInt32(result["LocationId"]),
                                //IsOccupied=Convert.ToBoolean(result["IsOccupied"])
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objCR;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetAllApplicationNo()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAppNoForCartingRegister", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<ApplicationNoDet> lstApplication = new List<ApplicationNoDet>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new ApplicationNoDet
                    {
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstApplication;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAppDetForCartingRegister(int CartingAppId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppDetForCartingRegister", CommandType.StoredProcedure, DParam);
            Chn_CartingRegister ObjCarting = new Chn_CartingRegister();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCarting.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCarting.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjCarting.GodownName = Result["GodownName"].ToString();
                    ObjCarting.CHAName = Result["EximTraderName"].ToString();
                    ObjCarting.CHAId = Convert.ToInt32(Result["CHAEximTraderId"]);
                    ObjCarting.ShippingLineName = Result["ShippingLineName"].ToString();
                    ObjCarting.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCarting.lstRegisterDtl.Add(new Chn_CartingRegisterDtl
                        {
                            CartingAppDtlId = Convert.ToInt32(Result["CartingAppDtlId"]),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingDate = Result["ShippingBillDate"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            MarksAndNo = Result["MarksAndNo"].ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            FoBValue = Convert.ToDecimal(Result["FobValue"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            Exporter = Result["EximTraderName"].ToString(),
                            ExporterId = Convert.ToInt32(Result["EximTraderId"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            FoBValue1 = Convert.ToDecimal(Result["FobValue"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCarting.lstGdnWiseLctn.Add(new Areas.Export.Models.GodownWiseLocation
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            LocationName = Result["LocationName"].ToString(),
                            // Column = Result["Column"].ToString(),
                            // Row = result["Row"].ToString()
                            // IsOccupied = Convert.ToBoolean(Result["IsOccupied"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCarting;
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
        public void DeleteCartingRegister(int CartingRegisterId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DelCartingRegister", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Carting Register Details Deleted Successfully";
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void GetLocationDetailsByGodownId(int GodownId)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_godownid", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetLocationDetailsByGodownId", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<Areas.Export.Models.GodownWiseLocation> lstApplication = new List<Areas.Export.Models.GodownWiseLocation>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new Areas.Export.Models.GodownWiseLocation
                    {
                        LocationId = Convert.ToInt32(result["LocationId"]),
                        //Row = result["Row"].ToString(),
                        //Column = result["Column"].ToString(),
                        LocationName = result["LocationName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstApplication;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAllGodown(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGodownExp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Godown> LstGodown = new List<Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGodown;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        public void AddShortCargoDetail(string XML, int CartingRegisterId, int CartingRegisterDtlId, string ShippingBillNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterDtlId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterDtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Value = ShippingBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("Addshortcargodtl", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = 1;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Short Cargo Details Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = 1;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Save data as next step already done";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetAllCartingEntryForSearch(int Page, string searchtext)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "p_in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "search", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = searchtext });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCartingForSearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<Chn_CartingRegister> CartingList = new List<Chn_CartingRegister>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Chn_CartingRegister objCarting = new Chn_CartingRegister();
                        objCarting.CartingRegisterId = Convert.ToInt32(dr["CartingRegisterId"]);
                        objCarting.CartingRegisterNo = dr["CartingRegisterNo"].ToString();
                        //  objCarting.CartingAppId = Convert.ToInt32(dr["CartingAppId"]);
                        objCarting.RegisterDate = dr["RegisterDate"].ToString();
                        //  objCarting.Remarks = (dr["Remarks"] == null ? "" : dr["Remarks"]).ToString();
                        objCarting.ApplicationNo = dr["ApplicationNo"].ToString();
                        objCarting.CHAName = dr["CHANAME"].ToString();
                        objCarting.ShippingBill= dr["ShippingBill"].ToString();

                        CartingList.Add(objCarting);
                        Status = 1;

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CartingList;
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
        public void GetAllCartingForPage(int page) //, string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = page });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCartingForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<Chn_CartingRegister> CartingList = new List<Chn_CartingRegister>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Chn_CartingRegister objCarting = new Chn_CartingRegister();
                        objCarting.CartingRegisterId = Convert.ToInt32(dr["CartingRegisterId"]);
                        objCarting.CartingRegisterNo = dr["CartingRegisterNo"].ToString();
                        //  objCarting.CartingAppId = Convert.ToInt32(dr["CartingAppId"]);
                        objCarting.RegisterDate = dr["RegisterDate"].ToString();
                        //  objCarting.Remarks = (dr["Remarks"] == null ? "" : dr["Remarks"]).ToString();
                        objCarting.ApplicationNo = dr["ApplicationNo"].ToString();
                        objCarting.CHAName = dr["CHANAME"].ToString();
                        objCarting.ShippingBill = dr["ShippingBill"].ToString();
                        CartingList.Add(objCarting);
                        Status = 1;

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CartingList;
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

        #endregion

        #region Container Movement
        public void GetAllInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerMovement> LstInternalMovement = new List<PPG_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_ContainerMovement
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        //BOENo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
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


        public void GetInternalPaymentSheetInvoice(int ContainerStuffingDtlId, int ContainerStuffingId, string ContainerNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int pvalue, decimal trv, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ConstuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(MovementDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "In_DestLocationId", MySqlDbType = MySqlDbType.Int32, Value = DestLocationIdiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_partyId", MySqlDbType = MySqlDbType.Int32, Value = Partyid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_charge_type", MySqlDbType = MySqlDbType.VarChar, Value = ctype });
            LstParam.Add(new MySqlParameter { ParameterName = "in_port_value", MySqlDbType = MySqlDbType.Int32, Value = pvalue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trweight", MySqlDbType = MySqlDbType.Decimal, Value = trv });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Constuffing", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //try
            //{
            //    var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDeliveryPaymentSheet", DParam);

            //    var objPostPaymentSheet = new PostPaymentSheet();
            //    objPostPaymentSheet.InvoiceType = InvoiceType;
            //    objPostPaymentSheet.InvoiceDate = InvoiceDate;
            //    objPostPaymentSheet.RequestId = DestuffingAppId;
            //    objPostPaymentSheet.RequestNo = DestuffingAppNo;
            //    objPostPaymentSheet.RequestDate = DestuffingAppDate;
            //    objPostPaymentSheet.PartyId = PartyId;
            //    objPostPaymentSheet.PartyName = PartyName;
            //    objPostPaymentSheet.PartyAddress = PartyAddress;
            //    objPostPaymentSheet.PartyState = PartyState;
            //    objPostPaymentSheet.PartyStateCode = PartyStateCode;
            //    objPostPaymentSheet.PartyGST = PartyGST;
            //    objPostPaymentSheet.PayeeId = PayeeId;
            //    objPostPaymentSheet.PayeeName = PayeeName;
            //    objPostPaymentSheet.DeliveryType = DeliveryType;

            //    objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
            //    objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreInvoiceCargo));


            //    objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
            //    {
            //        if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
            //            objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
            //        if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
            //            objPostPaymentSheet.CHAName += item.CHAName + ", ";
            //        if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
            //            objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
            //        if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
            //            objPostPaymentSheet.BOENo += item.BOENo + ", ";
            //        if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
            //            objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
            //        if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
            //            objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
            //        if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
            //            objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
            //        if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
            //            objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
            //        if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
            //            objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
            //        if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
            //            objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

            //        if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
            //        {
            //            objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
            //            {
            //                CargoType = item.CargoType,
            //                CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
            //                CFSCode = item.CFSCode,
            //                CIFValue = item.CIFValue,
            //                ContainerNo = item.ContainerNo,
            //                ArrivalDate = item.ArrivalDate,
            //                ArrivalTime = item.ArrivalTime,
            //                LineNo = item.LineNo,
            //                BOENo = item.BOENo,
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
            //        objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
            //        {
            //            CargoType = item.CargoType,
            //            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
            //            CFSCode = item.CFSCode,
            //            CIFValue = item.CIFValue,
            //            ContainerNo = item.ContainerNo,
            //            ArrivalDate = item.ArrivalDate,
            //            ArrivalTime = item.ArrivalTime,
            //            LineNo = item.LineNo,
            //            BOENo = item.BOENo,
            //            DeliveryType = item.DeliveryType,
            //            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
            //            Duty = item.Duty,
            //            GrossWt = item.GrossWeight,
            //            Insured = item.Insured,
            //            NoOfPackages = item.NoOfPackages,
            //            Reefer = item.Reefer,
            //            RMS = item.RMS,
            //            Size = item.Size,
            //            SpaceOccupied = item.SpaceOccupied,
            //            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
            //            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
            //            WtPerUnit = item.WtPerPack,
            //            AppraisementPerct = item.AppraisementPerct,
            //            HeavyScrap = item.HeavyScrap,
            //            StuffCUM = item.StuffCUM
            //        });

            //        objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
            //        objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
            //        objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
            //        objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
            //        objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
            //        objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
            //            + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
            //    });

            //    var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

            //    //******************************************************************************************************
            //    //Get Godown Type From Godown Master By GodownId
            //    if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
            //    {
            //        List<MySqlParameter> LstParam2 = new List<MySqlParameter>();
            //        LstParam2.Add(new MySqlParameter
            //        {
            //            ParameterName = "in_godownid",
            //            MySqlDbType = MySqlDbType.Int32,
            //            Value = objPostPaymentSheet.lstPreInvoiceCargo.Count > 0 ? objPostPaymentSheet.lstPreInvoiceCargo[0].GodownId : 0
            //        });

            //        IDataParameter[] DParam2 = { };
            //        DParam2 = LstParam2.ToArray();

            //        var GodowntypeId = Convert.ToInt32(DataAccess.ExecuteScalar("getgodowntypeid", CommandType.StoredProcedure, DParam2));
            //        objPostPaymentSheet.CalculateCharges(5, ChargeName, GodowntypeId);
            //    }
            //    else
            //    {
            //        objPostPaymentSheet.CalculateCharges(5, ChargeName);
            //    }
            //    //*******************************************************************************************************
            //    _DBResponse.Status = 1;
            //    _DBResponse.Message = "Success";
            //    _DBResponse.Data = objPostPaymentSheet;
            //}
            //catch (Exception ex)
            //{
            //    _DBResponse.Status = 0;
            //    _DBResponse.Message = "No Data";
            //    _DBResponse.Data = null;
            //}
            //finally
            //{

            //}



            PPG_MovementInvoice objInvoice = new PPG_MovementInvoice();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerMovementPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["ShippingLineId"]);
                        objInvoice.CHAName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["DestuffingId"]);
                        objInvoice.RequestNo = Result["DestuffingNo"].ToString();
                        objInvoice.RequestDate = Result["DestuffingDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            // BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            // CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            // BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            //  LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            //BOENo = Result["BOENo"].ToString(),
                            GrossWt = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            //  CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.hasSDAvailableBalance = Convert.ToInt32(Result["hasSDAvailableBalance"]);
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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

        internal void getOnlyRightsGodown()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getOnlyRightsGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Godown> LstGodown = new List<Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGodown;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }


        }

        public void GodownWiseLocation(int GodownId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGdwnWiseLctn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownWiseLctn> lstGodownlctn = new List<Areas.Import.Models.GodownWiseLctn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownlctn.Add(new Areas.Import.Models.GodownWiseLctn
                    {
                        LocationId = Convert.ToInt32(Result["LocationId"]),
                        LocationName = Convert.ToString(Result["LocationName"]),
                        // IsOccupied = Convert.ToInt32(Result["IsOccupied"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownlctn;
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

        public void AddEditInvoiceMovement(PPG_Movement_Invoice ObjPostPaymentSheet, PPG_Movement_Invoice ObjPostPaymentSheett, PPG_Movement_Invoice ObjPostPaymentSheettt, string ContainerXML, string ContainerXMLL, string ContainerXMLLL, string ChargesXML, string ChargesXMLL, string ChargesXMLLL, string ContWiseChargeXML, string ContWiseChargeXMLL, string ContWiseChargeXMLLL, string OperationCfsCodeWiseAmtXML, string OperationCfsCodeWiseAmtXMLL, string OperationCfsCodeWiseAmtXMLLL,
      int BranchId, int Uid,
     string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";

            string ReturnObj = "";

            if (ObjPostPaymentSheet.BOEDate == "")
            {
                ObjPostPaymentSheet.BOEDate = "1900-01-01";
            }

            if (ObjPostPaymentSheet.OldLctnNames == "")
            {
                ObjPostPaymentSheet.OldLctnNames = "1900-01-01";
            }
            else
            {
                ObjPostPaymentSheet.OldLctnNames = "1900-01-01";
            }



            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_port", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldGodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.BOEDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.NewGodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.OldLctnNames).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_sid", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_sname", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });

            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });



            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            //  LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIdd", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheett.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNamee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIddd", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheettt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNameee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNoo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddresss", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStatee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyStateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNooo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddressss", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodeee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotall", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.AllTotal });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotalll", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.AllTotal });


            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmtt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscountt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxablee", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmtt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.InvoiceAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscounttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxableee", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.InvoiceAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLL", MySqlDbType = MySqlDbType.Text, Value = ContainerXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLL", MySqlDbType = MySqlDbType.Text, Value = ChargesXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLL", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXMLL });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLL", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXMLL });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLLL", MySqlDbType = MySqlDbType.Text, Value = ContainerXMLLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLLL", MySqlDbType = MySqlDbType.Text, Value = ChargesXMLLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLLL", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXMLLL });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLLL", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXMLLL });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIdd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIdd });

            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIddd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIddd });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TareWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.CargoWeight });


            //   LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjj });

            //  LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjjj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditImpContainerMovement", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceList inv = new PPG_InvoiceList();
            try
            {
                if (Result == 1)
                {
                    String[] invobj;
                    invobj = GeneratedClientId.Split(',');
                    String[] movobj;
                    movobj = ReturnObj.Split(',');
                    if (invobj.Length >= 1)
                        inv.invoiceno = invobj[0];
                    if (invobj.Length >= 2)
                        inv.invoicenoo = invobj[1];
                    if (invobj.Length >= 3)
                        inv.invoicenooo = invobj[2];
                    inv.MovementNo = movobj[0];
                    _DBResponse.Data = inv;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Movement Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Payment Invoice have not generated due to low SD Balance";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        public void GetInvoiceDetailsForMovementPrintByNo(string InvoiceNo, string InvoiceType = "EXPMovement")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForMovementExportPrintByNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_Movement_Invoice objPaymentSheet = new PPG_Movement_Invoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPaymentSheet.CompGST = Result["GstIn"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = Result["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = Result["PartyName"].ToString();
                        objPaymentSheet.PartyState = Result["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = Result["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = Result["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGST = Result["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(Result["InvoiceAmt"]);

                        objPaymentSheet.ShippingLineName = Result["ShippingLinaName"] == System.DBNull.Value ? "" : Result["ShippingLinaName"].ToString();
                        //   objPaymentSheet.BOENo = Result["BOENo"].ToString();
                        //   objPaymentSheet.BOEDate = Result["BOEDate"].ToString();
                        objPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPaymentSheet.RequestNo = Result["StuffingReqNo"].ToString();
                        objPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPaymentSheet.CHAName = Result["CHAName"].ToString();
                        objPaymentSheet.ImporterExporter = Result["ExporterImporterName"].ToString();
                        objPaymentSheet.ArrivalDate = Result["ArrivalDate"].ToString();
                        objPaymentSheet.DeliveryDate = Result["DeliveryDate"].ToString();
                        objPaymentSheet.DestuffingDate = Result["DestuffingDate"].ToString();
                        objPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPaymentSheet.PartyCode = Result["PartyAlias"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.lstPostPaymentCont.Add(new PpgPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            ArrivalDate = Convert.ToString(Result["FromDate"]),
                            DeliveryDate = Convert.ToString(Result["ToDate"]),
                            // GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        if (Convert.ToDecimal(Result["Rate"]) > 0)
                        {
                            objPaymentSheet.lstPostPaymentChrg.Add(new PpgPostPaymentChrg()
                            {
                                Clause = Convert.ToString(Result["OperationSDesc"]),
                                ChargeName = Convert.ToString(Result["OperationDesc"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Taxable = Convert.ToDecimal(Result["Taxable"]),

                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"]),

                            });
                        }
                    }
                }

                //-------------------------------------------------------------------------
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheet;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                //-----------------------------------------------------------------------
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

        public void GetInternalMovement(int MovementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_ContainerMovement ObjInternalMovement = new PPG_ContainerMovement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    //    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    //    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    //    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    //    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    //    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    //    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    //    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    //    ObjInternalMovement.NewLocationIds = Result["NewLocationIds"].ToString();
                    //    ObjInternalMovement.NewLctnNames = Result["NewLctnNames"].ToString();
                    //    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    //    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    //    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                    //    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    //    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    //
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
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
        public void DelInternalMovement(int MovementId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Internal Movement Details Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Internal Movement Details As It Exists In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void AddEditImpInternalMovement(PPG_ContainerMovement ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ObjInternalMovement.Container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.SealDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.CustomSealDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.port });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.sid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.Sline });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Internal Movement  Details Already Exists";
                    _DBResponse.Data = null;
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
        }


        public void GetContainerForMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerMovement> LstStuffing = new List<PPG_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPG_ContainerMovement
                        {
                            Container = Result["ContainerNo"].ToString(),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].Container == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new PPG_ContainerMovement
                            {
                                Container = Result["ContainerNo"].ToString(),
                                ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExport", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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
        public void GetLocationForInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortForContainerMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerMovement> LstInternalMovement = new List<PPG_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_ContainerMovement
                    {
                        LocationName = Result["PortName"].ToString(),
                        LocationId = Convert.ToInt32(Result["PortId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
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

        public void GetConDetForMovement(int ContainerStuffingDtlId, String ContNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ConstuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetConDetForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_ContainerMovement ObjInternalMovement = new PPG_ContainerMovement();
            List<PPG_ShippingBill> LstShippingBillNo = new List<PPG_ShippingBill>();

            try
            {
                if (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.Container = Result["ContainerNo"].ToString();
                    ObjInternalMovement.CFSCode = Result["CFSCode"].ToString();
                    ObjInternalMovement.SealNo = Result["ShippingSeal"].ToString();
                    ObjInternalMovement.CustomSealNo = Result["CustomSeal"].ToString();
                    ObjInternalMovement.ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"].ToString());
                    ObjInternalMovement.ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"].ToString());
                    ObjInternalMovement.TransportMode = Result["TransportMode"] == System.DBNull.Value ? 1 : Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjInternalMovement.CargoWeight = Result["CargoWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CargoWeight"].ToString());
                    ObjInternalMovement.LocationName = Result["POL"].ToString();
                    ObjInternalMovement.LocationId = Convert.ToInt32(Result["PolId"].ToString());
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (LstShippingBillNo.Count <= 0)
                        {
                            LstShippingBillNo.Add(new PPG_ShippingBill
                            {
                                shippingBillNo = Result["ShippingBillNo"].ToString(),
                                CargoWeight = Convert.ToDecimal(Result["CargoWeight"].ToString()),
                            });
                        }
                        else
                        {
                            int flag = 0;

                            for (int i1 = 0; i1 < LstShippingBillNo.Count; i1++)
                            {
                                if (LstShippingBillNo[i1].shippingBillNo == Result["ShippingBillNo"].ToString())
                                {
                                    flag = 1;
                                    break;
                                }
                            }

                            if (flag == 0)
                            {
                                LstShippingBillNo.Add(new PPG_ShippingBill
                                {
                                    shippingBillNo = Result["ShippingBillNo"].ToString(),
                                    CargoWeight = Convert.ToDecimal(Result["CargoWeight"].ToString()),
                                });
                            }
                        }
                    }
                }
                ObjInternalMovement.ShipBill = LstShippingBillNo;
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
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

        #region BTT Cargo Entry
        public void AddEditBTTCargoEntry(BTTCargoEntry ObjBTT, string StuffingXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.BTTId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjBTT.BTTDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.CartingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBTT.CartingNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjBTT.CartingDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjBTT.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "BTTDetailXML", MySqlDbType = MySqlDbType.Text, Value = StuffingXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBTTCargoEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "BTT Cargo Entry Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Details Already Exist";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Edit BTT Cargo Entry Details As It Already Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetBTTCargoEntry()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<BTTCargoEntry> ObjBTT = new List<BTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.Add(new BTTCargoEntry()
                    {
                        BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]),
                        CartingNo = Convert.ToString(Result["CartingAppNo"]),
                        CartingDate = Convert.ToString(Result["CartingDate"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                        BTTNo = Convert.ToString(Result["BTTCargoEntryNo"]),
                        BTTDate = Convert.ToString(Result["BTTCargoEntryDate"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBTT;
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
        public void GetBTTCargoEntryById(int BTTId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BTTId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BTTCargoEntry ObjBTT = new BTTCargoEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]);
                    ObjBTT.BTTNo = Result["BTTCargoEntryNo"].ToString();
                    ObjBTT.BTTDate = Result["BTTCargoEntryDate"].ToString();
                    ObjBTT.CartingId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjBTT.CartingNo = Result["CartingAppNo"].ToString();
                    ObjBTT.CartingDate = Result["CartingDate"].ToString();
                    ObjBTT.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjBTT.CHAName = Result["EximTraderName"].ToString();
                    ObjBTT.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjBTT.lstBTTCargoEntryDtl.Add(new BTTCargoEntryDtl
                        {
                            BTTDetailId = Convert.ToInt32(Result["BTTCargoEntryDetailID"]),
                            CartingDetailId = Convert.ToInt32(Result["CartingDetailId"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Convert.ToString(Result["CommodityName"]),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            BTTQuantity = Convert.ToInt32(Result["BTTQuantity"]),
                            BTTWeight = Convert.ToDecimal(Result["BTTWeight"]),
                            BTTSQM= Convert.ToDecimal(Result["BTTSqm"]),
                            ActualSQM= Convert.ToDecimal(Result["ActualSQM"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBTT;
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
        public void DeleteBTTCargoEntry(int BTTId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Value = BTTId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteBTTCargoEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "BTT Cargo Entry Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Details Not Found";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete BTT Cargo Entry Details As It Exists In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void GetCartingAppList(int BTTCargoEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTCargoEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BTTCargoEntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingAppList", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegListForBTT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<BTTCartingList> lstBTTCartingList = new List<BTTCartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingList.Add(new BTTCartingList()
                    {
                        CartingId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Convert.ToString(Result["CartingNo"]),
                        CartingDate = Convert.ToString(Result["ApplicationDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBTTCartingList;
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
        public void GetCartingDetailList(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingDetailList", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegDetailBTT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<BTTCartingDetailList> lstBTTCartingDetailList = new List<BTTCartingDetailList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingDetailList.Add(new BTTCartingDetailList()
                    {
                        CartingDetailId = Convert.ToInt32(Result["CartingAppDtlId"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        SQM = Convert.ToDecimal(Result["SQM"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBTTCartingDetailList;
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
        public void GetCHAListForBTT()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure);
            IList<CHAList> lstCHA = new List<CHAList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAList
                    {
                        CHAId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetBTTCargoList(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AllBTTCargoList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<BTTCargoEntry> lstCont = new List<BTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCont.Add(new BTTCargoEntry
                    {
                        BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]),
                        CartingNo = Convert.ToString(Result["CartingAppNo"]),
                        CartingDate = Convert.ToString(Result["CartingDate"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                        BTTNo = Convert.ToString(Result["BTTCargoEntryNo"]),
                        BTTDate = Convert.ToString(Result["BTTCargoEntryDate"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCont;
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

        public void BTTCargoSearch(int Page, string searchtext)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "search", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = searchtext });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BTTCargoSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<BTTCargoEntry> lstCont = new List<BTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCont.Add(new BTTCargoEntry
                    {
                        BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]),
                        CartingNo = Convert.ToString(Result["CartingAppNo"]),
                        CartingDate = Convert.ToString(Result["CartingDate"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                        BTTNo = Convert.ToString(Result["BTTCargoEntryNo"]),
                        BTTDate = Convert.ToString(Result["BTTCargoEntryDate"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCont;
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


        #region Job Order
        public void GetAllTrainNo()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetExportTrainNo", CommandType.StoredProcedure, dpram);
            IList<PPG_TrainList> lstTrainNo = new List<PPG_TrainList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainNo.Add(new PPG_TrainList { TrainNo = result["TrainNo"].ToString(), TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainNo;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetImportJODetailsFrPrint(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDetForPrntexpjo", CommandType.StoredProcedure, dpram);
            PPGPrintJOModel objMdl = new PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objMdl.ContainerType = result["ContainerType"].ToString();
                    objMdl.JobOrderNo = result["JobOrderNo"].ToString();
                    objMdl.JobOrderDate = result["JobOrderDate"].ToString();
                    objMdl.ShippingLineName = result["ShippingLineName"].ToString();
                    objMdl.FromLocation = result["FromLocation"].ToString();
                    objMdl.ToLocation = result["ToLocation"].ToString();
                    objMdl.TrainNo = result["TrainNo"].ToString();
                    objMdl.TrainDate = result["TrainDate"].ToString();

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objMdl.lstDet.Add(new PPGPrintJOModelDet
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            OnBehalf = result["OnBehalf"].ToString(),
                            CustomSealNo = result["CustomSealNo"].ToString(),

                            Sline = result["Line_Seal_No"].ToString(),
                            POL = result["POL"].ToString(),
                            POD = result["POD"].ToString(),
                            Ct_Tare = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            Cargo_Wt = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            CFSCode = result["CFSCode"].ToString(),


                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objMdl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetTrainDtl(int TrainSumId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSumId", MySqlDbType = MySqlDbType.Int32, Value = TrainSumId });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetExportTrainDet", CommandType.StoredProcedure, dpram);
            FormOneForImpJO objFormone = new FormOneForImpJO();
            IList<PPG_TrainDtl> lstDtl = new List<PPG_TrainDtl>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    // objFormone.FormOneNo = result["FormOneNo"].ToString();
                    lstDtl.Add(new PPG_TrainDtl
                    {
                        TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                        TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = Convert.ToDateTime(result["TrainDate"]).ToString("dd/MM/yyyy"),
                        PortId = (result["PortId"]) == null ? 0 : Convert.ToInt32(result["PortId"]),
                        Wagon = (result["Wagon"] == null ? "" : result["Wagon"]).ToString(),
                        ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                        SZ = (result["SZ"] == null ? "" : result["SZ"]).ToString(),
                        LineSeal = (result["LineSeal"] == null ? "" : result["LineSeal"]).ToString(),
                        Commodity = (result["Commodity"] == null ? "" : result["Commodity"]).ToString(),
                        SLine = (result["SLine"] == null ? "" : result["SLine"]).ToString(),
                        TW = (result["TW"] == null ? 0 : Convert.ToDecimal(result["TW"])),
                        CW = (result["CW"] == null ? 0 : Convert.ToDecimal(result["CW"])),
                        GW = (result["GW"] == null ? 0 : Convert.ToDecimal(result["GW"])),
                        Ct_Status = (result["Status"] == null ? "" : result["Status"]).ToString(),
                        PKGS = (result["PKGS"] == null ? 0 : Convert.ToDecimal(result["PKGS"])),
                        CustomSeal = (result["CustomSeal"] == null ? "" : result["CustomSeal"]).ToString(),
                        Shipper = (result["Shipper"] == null ? "" : result["Shipper"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        CRRSBillingParty = (result["CRRSBillingParty"] == null ? "" : result["CRRSBillingParty"]).ToString(),
                        BillingParty = (result["BillingParty"] == null ? "" : result["BillingParty"]).ToString(),
                        StuffingMode = (result["StuffingMode"] == null ? "" : result["StuffingMode"]).ToString(),
                        SBillNo = (result["SBillNo"] == null ? "" : result["SBillNo"]).ToString(),
                        Date = (result["Date"] == null ? "" : result["Date"]).ToString(),
                        Origin = (result["Origin"] == null ? "" : result["Origin"]).ToString(),
                        POL = (result["POL"] == null ? "" : result["POL"]).ToString(),
                        POD = (result["POD"] == null ? "" : result["POD"]).ToString(),
                        DepDate = (result["DepDate"] == null ? "" : result["DepDate"]).ToString(),


                        TransportName = result["PortName"].ToString(),

                        ShippingLineName = result["S_Linee"] == DBNull.Value ? "" : result["S_Linee"].ToString(),
                        ShippingLineId = result["S_LineId"] == DBNull.Value ? 0 : Convert.ToInt32(result["S_LineId"].ToString())

                    });
                }
                if (Status == 1)
                {

                    _DBResponse.Data = lstDtl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetTrainDetailsOnEditMode(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfexpJobOrd", CommandType.StoredProcedure, dpram);
            PPGExportJobOrder objImpJO = new PPGExportJobOrder();
            IList<PPG_TrainDtl> lstDtl = new List<PPG_TrainDtl>();
            IList<PPG_ImportJobDel> lstdel = new List<PPG_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new PPG_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    //objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    //objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    //objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    //objImpJO.JobOrderDate = Convert.ToDateTime(result["JobOrderDate"]);
                    //objImpJO.TrainNo = result["TrainNo"].ToString();
                    //objImpJO.TrainDate = Convert.ToDateTime(result["TrainDate"]);

                    //  objImpJO.Remarks = result["Remarks"].ToString();

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new PPG_TrainDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            SZ = result["ContainerSize"].ToString(),
                            LineSeal = result["Line_Seal_No"].ToString(),
                            Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            PKGS = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon = result["Wagon_No"].ToString(),
                            SLine = result["S_Line"].ToString(),
                            TransportName = result["Transportfrom"].ToString(),
                            TW = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            CW = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            GW = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                            // Destination = result["Destination"].ToString(),
                            //  Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Data = lstDtl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void AddEditImpJO(PPGExportJobOrder objJO, string XML = null)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PickUpLocation", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PickUpLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSummaryID", MySqlDbType = MySqlDbType.Int32, Value = objJO.TrainSummaryID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderFor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objJO.JobOrderFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.JobOrderDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_transportfor", MySqlDbType = MySqlDbType.Int32, Value = objJO.TransportBy });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_no", MySqlDbType = MySqlDbType.VarChar, Value = objJO.TrainNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.TrainDate) });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objJO.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.LongText, Value = XML });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditExportJobOrder", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Export Job Order Saved Successfully" : "Export Job Order Saved Successfully";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }


        public void GetAllImpJO(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExpJobOrdForPage", CommandType.StoredProcedure, dpram);
            IList<PPG_ImportJobOrderList> lstImpJO = new List<PPG_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new PPG_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        public void GetAllImpJO(string ContainerNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("SearchListOfExportJobOrder", CommandType.StoredProcedure, dpram);
            IList<PPG_ImportJobOrderList> lstImpJO = new List<PPG_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new PPG_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetImpJODetails(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfexpJobOrd", CommandType.StoredProcedure, dpram);
            PPGExportJobOrder objImpJO = new PPGExportJobOrder();
            IList<PPG_TrainDtl> lstDtl = new List<PPG_TrainDtl>();
            IList<PPG_ImportJobDel> lstdel = new List<PPG_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new PPG_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    objImpJO.JobOrderDate = Convert.ToDateTime(result["JobOrderDate"]);
                    objImpJO.TrainNo = result["TrainNo"].ToString();
                    objImpJO.TrainDate = Convert.ToDateTime(result["TrainDate"]);
                    objImpJO.TrainSummaryID = result["TrainSummaryId"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new PPG_TrainDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            SZ = result["ContainerSize"].ToString(),
                            LineSeal = result["Line_Seal_No"].ToString(),
                            Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            PKGS = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon = result["Wagon_No"].ToString(),
                            SLine = result["S_Line"].ToString(),
                            TransportForm = result["Transportfrom"].ToString(),
                            TW = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            CW = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            GW = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                            // Destination = result["Destination"].ToString(),
                            // Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }
                if (lstDtl.Count > 0)
                {
                    objImpJO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstDtl);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        public void DeleteImpJO(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteExpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Export Job Order Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<ShippingLineForPage> lstShippingLine = new List<ShippingLineForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLineForPage
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShippingLine, State };
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

        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            IList<ShippingLine> lstShippingLine = new List<ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        ShippingLineName = result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShippingLine;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAllPickupLocation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPickupLocation", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGPickupModel> LstPickUp = new List<PPGPickupModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPickUp.Add(new PPGPickupModel
                    {
                        pickup_location = Result["PickUpLctn"].ToString(),
                        alias = (Result["LctnAlias"] == null ? "" : Result["LctnAlias"]).ToString(),
                        id = Convert.ToInt32(Result["Id"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPickUp;
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
        public void GetAllPortForJobOrderTrasport()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<TransformList> LstPort = new List<TransformList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new TransformList
                    {
                        PortName = Result["PortName"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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

        #region Loaded Container Payment Sheet
        public void GetLoadedContainerRequestForPaymentSheet(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedContainerRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PPG_PaySheetStuffingRequest> objPaySheetStuffing = new List<PPG_PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PPG_PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        GateEntryDateTime = Convert.ToString(Result["EntryDateTime"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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

        public void GetLoadedPaymentSheetInvoice(int ContainerStuffingId, string InvoiceDate,string InvoiceType,String SEZ, string contxml, int PayeeId,int PartyId,int IntercartingApplicable,int ICDDestuffing, int InvoiceId = 0,int SealCharge=0,int WithSeal=0,int Directloading=0,int Weighment=0,decimal DiscountPer=0, int InsuranceSurcharge = 0, int EnergySurcharge = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = contxml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.VarChar, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IntercartingApplicable", MySqlDbType = MySqlDbType.Int32, Value = IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = SealCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WithSeal", MySqlDbType = MySqlDbType.Int32, Value = WithSeal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Directloading", MySqlDbType = MySqlDbType.Int32, Value = Directloading });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceSurcharge", MySqlDbType = MySqlDbType.Int32, Value = InsuranceSurcharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EnergySurcharge", MySqlDbType = MySqlDbType.Int32, Value = EnergySurcharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DiscountPer", MySqlDbType = MySqlDbType.Decimal, Value = DiscountPer });
         
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            CHN_ExpPaymentSheet objInvoice = new CHN_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("getLoadedContainerPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Result["PayeeName"].ToString();
                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new ChnExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            CargoType =  Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate=Result["BOEDate"].ToString(),
                            GrossWt = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            DestuffingDate ="",
                            StuffingDate = "",
                            CartingDate="",
                            ShippingLineName=Result["ShippingLineName"].ToString(),
                            CHAName=Result["CHAName"].ToString(),
                            ImporterExporter=Result["ImporterExporter"].ToString(),
                            SpaceOccupiedUnit=Result["SpaceOccupiedUnit"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new ChnExpInvoiceChargeBase
                        {
                            ChargeId = Result["ChargeId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"] == System.DBNull.Value ? "" : Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString()
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new CHN_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new CHN_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentNo=Result["SBNo"].ToString(),
                            DocumentDate=Result["SBDate"].ToString(),
                            DocumentType= (Result["SBNo"].ToString()==""?"":"SB"),
                        });
                    }
                }
                if(Result.NextResult())
                {
                    while(Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsLocalGST=Convert.ToInt32(Result["IsLocalGST"].ToString());
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);
               

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

        public void GetLoadedPaymentSheetInvoice(int ContainerStuffingId, string InvoiceDate, string InvoiceType, string contxml, int PayeeId, int PartyId, int IntercartingApplicable, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = contxml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.VarChar, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IntercartingApplicable", MySqlDbType = MySqlDbType.Int32, Value = IntercartingApplicable });


            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            CHN_ExpPaymentSheet objInvoice = new CHN_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("getLoadedContainerPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Result["PayeeName"].ToString();
                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new ChnExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            GrossWt = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            DestuffingDate = "",
                            StuffingDate = "",
                            CartingDate = "",
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new ChnExpInvoiceChargeBase
                        {
                            ChargeId = Result["ChargeId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"] == System.DBNull.Value ? "" : Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString()
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new CHN_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new CHN_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentNo = Result["SBNo"].ToString(),
                            DocumentDate = Result["SBDate"].ToString(),
                            DocumentType = (Result["SBNo"].ToString() == "" ? "" : "SB"),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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

        public void GetContainerForLoadedContainerPaymentSheet(int LoadContReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedContainerRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PPG_PaymentSheetContainer> objPaymentSheetContainer = new List<PPG_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PPG_PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        Size = Result["Size"].ToString(),
                        ArrivalDt = Result["ArrivalDt"].ToString(),
                        IsHaz = Result["IsHaz"].ToString(),
                         IsOnWheel=Convert.ToString(Result["IsOnWheel"].ToString())
                         
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
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


        public void AddEditInvoiceContLoaded(CHN_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid, string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Intercarting", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.SealCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WithSeal", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.WithSeal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Directloading", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Directloading });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Discount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.DiscountPer==null?0: ObjPostPaymentSheet.DiscountPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceSurcharge", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InsuranceSurcharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EnergySurcharge", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.EnergySurcharge });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContLoadedInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Load Container Request
        public void ListOfLoadCont()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ListLoadContReq> LstCont = new List<ListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new ListLoadContReq
                    {
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Result["LoadContReqNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        LoadContReqDate = Result["LoadContReqDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
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
        public void GetLoadContDetails(int LoadContReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Role.RoleId) });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_LoadContReq objDet = new PPG_LoadContReq();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDet.LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]);
                    objDet.LoadContReqNo = Result["LoadContReqNo"].ToString();
                    objDet.CHAName = Result["CHAName"].ToString();
                    objDet.LoadContReqDate = Result["LoadContReqDate"].ToString();
                    objDet.Remarks = Result["Remarks"].ToString();

                    objDet.ForeignLiner = Result["ForeignLiner"].ToString();
                    objDet.Vessel = Result["Vessel"].ToString();
                    objDet.Via = Result["Via"].ToString();
                    objDet.Voyage = Result["Voyage"].ToString();
                    objDet.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objDet.FinalDestinationLocationID = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    objDet.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                    objDet.CustomExaminationType = Convert.ToString(Result["ExamType"]);                    
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objDet.lstContDtl.Add(new  Chn_LoadContReqDtl
                        {
                            LoadContReqDetlId = Convert.ToInt32(Result["LoadContReqDetlId"]),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            FobValue = Convert.ToDecimal(Result["FobValue"]),
                            ExporterName = Result["ExporterName"].ToString(),
                            ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString(),
                            CommodityName = Result["CommodityName"].ToString(),
                            EquipmentSealType = Result["EquipmentSealType"].ToString(),
                            EquipmentStatus = Result["EquipmentStatus"].ToString(),
                            EquipmentQUC = Result["EquipmentQUC"].ToString(),
                            PackageType = Result["PackageType"].ToString(),
                            ContLoadType = Result["ContLoadType"].ToString(),
                            CustomSeal = Result["CustomSeal"].ToString(),
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                            IsSEZ = Convert.ToBoolean(Result["SEZ"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDet;
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
        public void AddEditLoadContDetails(PPG_LoadContReq objLoadContReq, string XML)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objLoadContReq.LoadContReqDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objLoadContReq.Remarks });

            //mks
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objLoadContReq.ForeignLiner });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objLoadContReq.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objLoadContReq.Via });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objLoadContReq.Voyage });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objLoadContReq.Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationID", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.FinalDestinationLocationID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.FinalDestinationLocation });

            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExamType", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.CustomExaminationType });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Loaded Container Request Saved Successfully" : "Loaded Container Request Updated Successfully");
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Loaded Container Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void DelLoadContReqhdr(int LoadContReqId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelLoadContReqhdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Loaded Container Request DetailsDeleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Loaded Container Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void ListOfCommodity()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            IList<Areas.Export.Models.Commodity> lstCommodity = new List<Areas.Export.Models.Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new Areas.Export.Models.Commodity
                    {
                        CommodityId = Convert.ToInt32(result["CommodityId"]),
                        CommodityName = result["CommodityName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCommodity;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetAllLoadedContainerRq(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AllLoadedContRqData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ListLoadContReq> LstEIR = new List<ListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEIR.Add(new ListLoadContReq
                    {
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        LoadContReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CHAName = Convert.ToString(Result["CHAName"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEIR;
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

        public void LoadedContReqSr(int Page, string searchtext)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                //lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "p_in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "search", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = searchtext });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("loadedcontreqsearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<ListLoadContReq> LoadedCnRqLst = new List<ListLoadContReq>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        ListLoadContReq objLoadedCnRqEntry = new ListLoadContReq();
                        // objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objLoadedCnRqEntry.LoadContReqId = Convert.ToInt32(dr["LoadContReqId"]);
                        objLoadedCnRqEntry.LoadContReqNo = Convert.ToString(dr["LoadContReqNo"]);
                        //objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objLoadedCnRqEntry.LoadContReqDate = Convert.ToString(dr["LoadContReqDate"]);
                        objLoadedCnRqEntry.CHAName = Convert.ToString(dr["CHAName"]);
                        objLoadedCnRqEntry.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        LoadedCnRqLst.Add(objLoadedCnRqEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LoadedCnRqLst;
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


        public void GetAllLoadedCntRqData(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LoadedContReqSearchLst", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ListLoadContReq> LstLoadedCntReq = new List<ListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstLoadedCntReq.Add(new ListLoadContReq
                    {
                        //objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        //objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        LoadContReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        CHAName = Convert.ToString(Result["CHAName"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstLoadedCntReq;
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
        #region BTT Payment Sheet
        public void GetCartingApplicationForPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);

            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegisterForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["CartingAppId"]),
                        StuffingReqNo = Convert.ToString(Result["CartingAppNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
        public void GetShipBillForPaymentSheet(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegisterForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        Size = Result["Size"].ToString(),
                        ArrivalDt = Result["ArrivalDt"].ToString(),
                        IsHaz = Result["IsHaz"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
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

        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType,string SEZ, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            ChnInvoiceBTT objInvoice = new ChnInvoiceBTT();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new ChnPreInvoiceContainerBTT
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new ChnPostPaymentContainerBTT
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new ChnPostPaymentChrgBTT
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new ChnContainerWiseAmountBTT
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            CargoHandlingCharge = Convert.ToDecimal(Result["CargoHandlingCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new ChnOperationCFSCodeWiseAmountBTT
                        {
                            InvoiceId = InvoiceId,
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
                        objInvoice.lstPreInvoiceCargo.Add(new ChnPreInvoiceCargoBTT
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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

        public void AddEditBTTInvoice(ChnInvoiceBTT ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
         String SEZ,  int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceBTT", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Export Destuffing
        public void GetContainersForExpDestuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerListForExportDestuffing", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<ContainersExpDestuffing> objPaySheetStuffing = new List<ContainersExpDestuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ContainersExpDestuffing()
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Result["CHAName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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

        public void GetChargesForExpDestuffing(int ContainerStuffingId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ChargesForExportDestuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ExportDestuffingCharges> objPaySheetStuffing = new List<ExportDestuffingCharges>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ExportDestuffingCharges()
                    {
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        Charge = Convert.ToDecimal(Result["Charge"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        CGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        SGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Amount = Convert.ToDecimal(Result["Charge"]),
                        Taxable = Convert.ToDecimal(Result["Charge"]),
                        TotalAmount = Convert.ToDecimal(Result["TotalAmount"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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


        public void GetExportDestuffingPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingDtlId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PpgInvoiceExpDestuf objInvoice = new PpgInvoiceExpDestuf();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExportDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainerExpDestuf
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainerExpDestuf
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrgExpDestuf
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmountExpDestuf
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmountExpDestuf
                        {
                            InvoiceId = InvoiceId,
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
                        objInvoice.lstPreInvoiceCargo.Add(new PpgPreInvoiceCargoExpDestuf
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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

        public void AddEditExpDestufInvoice(PpgInvoiceExpDestuf ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceExpDestuf", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Destuffing Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region cargo shifting
        public void GetShippingLineForInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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

        public void GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShiftingType", MySqlDbType = MySqlDbType.Int32, Value = ShiftingType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillsForCargoShifting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CargoShiftingShipBillDetails> objShipBills = new List<CargoShiftingShipBillDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objShipBills.Add(new CargoShiftingShipBillDetails()
                    {
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                        RegisterDate = Convert.ToString(Result["RegisterDate"]),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        ActualQty = Convert.ToDecimal(Result["ActualQty"]),
                        ActualWeight = Convert.ToDecimal(Result["ActualWeight"]),
                        IsChecked = Convert.ToInt32(Result["IsChecked"]) == 0 ? false : true,
                        SQM = Convert.ToDecimal(Result["SQM"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objShipBills;
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
        public void GetCargoShiftingPaymentSheet(string InvoiceDate, int PartyId, string ShipBillsXML, int InvoiceId, string TaxType, int PayeeId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipBillsXML", MySqlDbType = MySqlDbType.Text, Value = ShipBillsXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = TaxType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PpgInvoiceCargoShifting objInvoice = new PpgInvoiceCargoShifting();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoShiftingPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Result["PayeeName"].ToString();

                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrgShifting
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmountCS
                        {
                            InvoiceId = InvoiceId,
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

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Taxable);
                objInvoice.TotalDiscount = objInvoice.lstPostPaymentChrg.Sum(o => o.Discount);
                objInvoice.TotalTaxable = objInvoice.lstPostPaymentChrg.Sum(o => o.Taxable);
                objInvoice.TotalCGST = objInvoice.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
                objInvoice.TotalSGST = objInvoice.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
                objInvoice.TotalIGST = objInvoice.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
                objInvoice.CWCAmtTotal = objInvoice.TotalTaxable + objInvoice.TotalCGST + objInvoice.TotalSGST + objInvoice.TotalIGST;
                objInvoice.HTAmtTotal = 0;
                objInvoice.CWCTDSPer = 0;
                objInvoice.HTTDSPer = 0;
                objInvoice.TDS = 0;
                objInvoice.TDSCol = 0;
                objInvoice.AllTotal = objInvoice.CWCAmtTotal;
                objInvoice.RoundUp = 0;
                objInvoice.InvoiceAmt = objInvoice.AllTotal;
                objInvoice.ShippingLineName = objInvoice.PartyName;
                objInvoice.CHAName = "";
                objInvoice.ImporterExporter = "";
                objInvoice.Module = "EXPCRGSHFT";
                objInvoice.InvoiceId = InvoiceId;

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
        public void AddEditCargoShiftInvoice(PpgInvoiceCargoShifting ObjPostPaymentSheet, string ChargesXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid, string CartingRgisterDtlXML, int FromGodownId, int ToGodownId, int ToShippingId, int ShiftingType, int FromShippingLineId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = FromShippingLineId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_FromShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.FromShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToShippingId", MySqlDbType = MySqlDbType.Int32, Value = ToShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Value = FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Value = ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipBillsXML", MySqlDbType = MySqlDbType.Text, Value = CartingRgisterDtlXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShiftingType", MySqlDbType = MySqlDbType.Int32, Value = ShiftingType });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceCargoShift", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Export Container Credit Debit RR
        public void GetInvoiceNoForCreditDebitRR()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getInvoiceForRRCreditDebit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstInv = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInv.Add(new
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(Result["InvoiceDate"]).ToString("dd/MM/yyyy"),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInv;
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
        public void GetContainerDetForExpRR(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getInvoiceForRRCreditDebit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgRRCreditDebitInvoiceDetails obj = new PpgRRCreditDebitInvoiceDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    obj.PartyId = Convert.ToInt32(Result["PartyId"]);
                    obj.PartyName = Convert.ToString(Result["PartyName"]);
                    obj.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    obj.PayeeName = Convert.ToString(Result["PayeeName"]);
                    obj.Address = Convert.ToString(Result["Address"]);
                    obj.StateCode = Convert.ToString(Result["StateCode"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        obj.lstContDetailsRRCD.Add(new PpgPostPaymentContainerRRCD
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Reefer = Convert.ToInt32(Result["IsReefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToInt32(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        obj.lstChrgDetailsRRCD.Add(new PpgPostPaymentChrgRRCD
                        {
                            OperationId = Convert.ToInt32(Result["OperationId"]),
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
                            Total = Convert.ToDecimal(Result["Total"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = obj;
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
        public void GetEXPRRCDSheetInvoice(int InvoiceId, int ShippingLineId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRRCreditDebitPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgInvoiceRRCreditDebit objInvoice = new PpgInvoiceRRCreditDebit();
            try
            {
                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentContRRCD.Add(new PpgPostPaymentContainerRRCD
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgRRCD.Add(new PpgPostPaymentChrgRRCD
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrgRRCD.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
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
        public void AddEditExportRRCreditDebitModule(PpgInvoiceRRCreditDebit ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = (ObjPostPaymentSheet.InvoiceType == null ? "Tax" : ObjPostPaymentSheet.InvoiceType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceIdCRNote", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceIdCRNote });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceRRCreditDebit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Train Summary




        public void AddUpdateTrainSummaryUpload(ppgExportTrainSummaryUpload Obj, string TrainSummaryUploadXML = "")
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TrainSummaryUploadId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.TrainDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PortId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PortId });
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddUpdateExportTrainSummaryUpload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                _DBResponse.Status = 1;
                _DBResponse.Message = "";
                _DBResponse.Data = Result;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void CheckTrainSummaryUpload(string TrainNo, string TrainSummaryUploadXML)
        {

            DataSet Result = new DataSet();
            try
            {

                int RetValue = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo }); ;
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("checkexporttrainsummaryupload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<ppgExportTrainSummaryUpload> TrainSummaryUploadList = new List<ppgExportTrainSummaryUpload>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    ppgExportTrainSummaryUpload objTrainSummaryUpload = new ppgExportTrainSummaryUpload();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon = Convert.ToString(dr["Wagon"]);
                    objTrainSummaryUpload.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    objTrainSummaryUpload.SZ = Convert.ToString(dr["SZ"]);
                    objTrainSummaryUpload.Status = Convert.ToString(dr["Status"]);
                    objTrainSummaryUpload.SLine = Convert.ToString(dr["SLine"]);
                    objTrainSummaryUpload.TW = Convert.ToDecimal(dr["TW"]);
                    objTrainSummaryUpload.CW = Convert.ToDecimal(dr["CW"]);
                    objTrainSummaryUpload.GW = Convert.ToDecimal(dr["GW"]);
                    objTrainSummaryUpload.PKGS = Convert.ToInt32(dr["PKGS"]);
                    objTrainSummaryUpload.Commodity = Convert.ToString(dr["Commodity"]);
                    objTrainSummaryUpload.LineSeal = Convert.ToString(dr["LineSeal"]);
                    objTrainSummaryUpload.CustomSeal = Convert.ToString(dr["CustomSeal"]);
                    objTrainSummaryUpload.Shipper = Convert.ToString(dr["Shipper"]);
                    objTrainSummaryUpload.CHA = Convert.ToString(dr["CHA"]);
                    objTrainSummaryUpload.CRRSBillingParty = Convert.ToString(dr["CRRSBillingParty"]);
                    objTrainSummaryUpload.BillingParty = Convert.ToString(dr["BillingParty"]);
                    objTrainSummaryUpload.StuffingMode = Convert.ToString(dr["StuffingMode"]);
                    objTrainSummaryUpload.SBillNo = Convert.ToString(dr["SBillNo"]);
                    objTrainSummaryUpload.Date = Convert.ToDateTime(dr["Date"]);
                    objTrainSummaryUpload.Origin = Convert.ToString(dr["Origin"]);
                    objTrainSummaryUpload.POL = Convert.ToString(dr["POL"]);
                    objTrainSummaryUpload.POD = Convert.ToString(dr["POD"]);
                    objTrainSummaryUpload.DepDate = Convert.ToDateTime(dr["DepDate"]);

                    objTrainSummaryUpload.StatusValue = Convert.ToInt32(dr["StatusValue"]);

                    if (objTrainSummaryUpload.StatusValue == 0)
                    {
                        objTrainSummaryUpload.StatusDesc = "OK";
                    }
                    else if (objTrainSummaryUpload.StatusValue == 1)
                    {
                        objTrainSummaryUpload.StatusDesc = "Already Exist.";
                    }
                    else if (objTrainSummaryUpload.StatusValue == 2)
                    {
                        objTrainSummaryUpload.StatusDesc = "Cannot Save";
                    }


                    TrainSummaryUploadList.Add(objTrainSummaryUpload);
                }

                _DBResponse.Status = RetValue;
                _DBResponse.Message = "";
                _DBResponse.Data = TrainSummaryUploadList;
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



        public void GetAllTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<ppgExportTrainSummaryUpload> TrainSummaryUploadList = new List<ppgExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        ppgExportTrainSummaryUpload objTrainSummaryUpload = new ppgExportTrainSummaryUpload();
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        //objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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


        public void GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryUploadId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<ppgExportTrainSummaryUpload> TrainSummaryUploadList = new List<ppgExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        ppgExportTrainSummaryUpload objTrainSummaryUpload = new ppgExportTrainSummaryUpload();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        // objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                        objTrainSummaryUpload.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objTrainSummaryUpload.SZ = Convert.ToString(dr["SZ"]);
                        objTrainSummaryUpload.LineSeal = Convert.ToString(dr["LineSeal"]);
                        objTrainSummaryUpload.Commodity = Convert.ToString(dr["Commodity"]);
                        objTrainSummaryUpload.SLine = Convert.ToString(dr["SLine"]);
                        //objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                        //objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                        //objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                        //objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                        //objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                        //objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                        //objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                        //objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                        objTrainSummaryUpload.StatusDesc = "";
                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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

        public void ListOfTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<ppgExportTrainSummaryUpload> TrainSummaryUploadList = new List<ppgExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        ppgExportTrainSummaryUpload objTrainSummaryUpload = new ppgExportTrainSummaryUpload();
                        objTrainSummaryUpload.TrainSummaryUploadId = Convert.ToInt32(dr["TrainSummaryId"]);
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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







        #endregion

        #region Export Payment Sheet
        public void GetContStuffingForPaymentSheet(int StuffingReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHN_ContainerStuffingPSReq> objPaySheetStuffing = new List<CHN_ContainerStuffingPSReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new CHN_ContainerStuffingPSReq()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
        public void GetContDetForPaymentSheet(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHN_ContainerDetails> objPaySheetStuffing = new List<CHN_ContainerDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new CHN_ContainerDetails()
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Result["Size"].ToString(),
                        ArrivalDate = Convert.ToString(Result["ArrivalDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
        public void GetExportPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML,

            int InvoiceId, int PartyId, int PayeeId,String SEZ,int IntercartingApplicable,int ICDDestuffing, int SealCharge,int EIRPurpose,int CFSCharges,int Weighment, string MovementType,int EnergySurcharges,decimal DiscountPer)

        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IntercartingApplicable", MySqlDbType = MySqlDbType.Int32, Value = IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = SealCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EIRPurpose", MySqlDbType = MySqlDbType.Int32, Value = EIRPurpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCharges", MySqlDbType = MySqlDbType.Int32, Value = CFSCharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = MovementType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EnergySurcharges", MySqlDbType = MySqlDbType.VarChar, Value = EnergySurcharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DiscountPer", MySqlDbType = MySqlDbType.Decimal, Value = DiscountPer });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            CHN_ExpPaymentSheet objInvoice = new CHN_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExportPS", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Convert.ToString(Result["PayeeName"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new ChnExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            //ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            //LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = "",
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter= Result["ImporterExporter"].ToString(),
                            PltBox = Convert.ToInt32(Result["PLtBox"]),
                            ParkDays = Convert.ToInt32(Result["ParkDays"]),
                            LockDays = Convert.ToInt32(Result["LockDays"]),

                        });
                        objInvoice.DeliveryType =Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new ChnExpInvoiceChargeBase
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString(),
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new CHN_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new CHN_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            DocumentNo=Convert.ToString(Result["SBNo"]),
                            DocumentDate = Convert.ToString(Result["SBDate"]),
                            Clause= Result["Clause"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstADDPostPaymentChrg.Add(new ChnExpADDCWCInvoiceChargeBase
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString(),
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])
                        });
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

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
        public void ListOfExpInvoice(string Module,string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHNListOfExpInvoice> lstExpInvoice = new List<CHNListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new CHNListOfExpInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExpInvoice;
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
        public void AddEditExpInvoice(CHN_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
            int BranchId, int Uid, string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.SealCharge });

            LstParam.Add(new MySqlParameter { ParameterName = "in_EIRPurpose", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.EIRPurpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCharges", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.CFSCharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DiscountPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.DiscountPer });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion


        #region Shippingbill Amendment
        public void GetAmenSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBList", CommandType.StoredProcedure);
            List<CHN_Amendment> LstSB = new List<CHN_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Result["SBDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAmenSBDetailsBySbNoDate(string SbNo, string SbDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SbNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.Date, Size = 11, Value = Convert.ToDateTime(SbDate) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBDetailsBySbNoDate", CommandType.StoredProcedure, DParam);
            List<CHN_Amendment> LstSB = new List<CHN_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Convert.ToString(Result["SBDate"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        FOBValue = Convert.ToString(Result["FOB"]),
                        Pkg = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CCINId = Convert.ToString(Result["CCINID"]),
                        Cargo = Convert.ToString(Result["Cargo"]),
                        CommodityId = Convert.ToInt32((Result["CommodityId"]) ?? 0),
                        ExporterId = Convert.ToInt32((Result["ExporterId"]) ?? 0),
                        Area = Convert.ToString((Result["SQM"]) ?? 0),
                        IsApprove = Convert.ToInt32((Result["IsApproved"]) ?? 0),
                        Cutting = Convert.ToInt32((Result["isCutting"]) ?? 0),
                        ShortCargo = Result["ShortCargo"].ToString(),
                        CCINNO = Result["CCINNo"].ToString(),
                        SBType = Result["SBType"].ToString(),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        PostOfDest = Result["PostOfDest"].ToString(),
                        ShippingLineId = Convert.ToInt32((Result["ShippingLineId"]) ?? 0),
                        PortOfLoadingId = Convert.ToInt32((Result["PortOfLoadingId"]) ?? 0),
                        PortofDestId = Convert.ToInt32((Result["PortofDestId"]) ?? 0),
                        CCINInvoiceNo = Convert.ToString(Result["CCINInvoiceNo"]),
                        CCINInvoiceDate = Convert.ToString(Result["CCINInvoiceDate"]),
                        VehicleNo = Convert.ToInt32((Result["VehicleNo"]) ?? 0),
                        NoofGround = Convert.ToInt32((Result["NoofGround"]) ?? 0),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditAmendment(string OldSBNoValue, string NewSBNoValue, string Date, int InvoiceId, string InvoiceNo, string InvoiceDate, string FlagMerger)
        {
            InvoiceDate = null;//DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentId", MySqlDbType = MySqlDbType.String, Value = AmendmentNO });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldSBNoXML", MySqlDbType = MySqlDbType.Text, Value = OldSBNoValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewSBNoXML", MySqlDbType = MySqlDbType.Text, Value = NewSBNoValue });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = FlagMerger });


            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditAmendment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "Amendment Entry Saved Successfully" : "Amendment Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = "Data with this invoice no already exists in Amendment";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Shipbill should be same stage  ";
                    _DBResponse.Status = 0;
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


        public void AddEditShipAmendment(CHN_AmendmentViewModel vm)
        {
            vm.InvoiceDate = null;// DateTime.ParseExact(vm.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.String, Value = vm.ID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShipBillNo", MySqlDbType = MySqlDbType.String, Value = vm.NewShipBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldShipBillNo", MySqlDbType = MySqlDbType.String, Value = vm.ShipBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldShipBillDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.OldShipBillDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShipBillDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.ShipbillDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.Date) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_exporterId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.ExporterID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Commodityid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.CargoID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Weight) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_pkg", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Pkg) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.FOB) });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = vm.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = vm.InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.VarChar, Value = vm.InvoiceDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SbType", MySqlDbType = MySqlDbType.Int32, Value = vm.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = vm.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = vm.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortLoadingId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfLoadingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortDestId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditShipBillAmendment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "Amendment Saved Successfully" : "Amendment Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = "Data with this invoice no already exists in Amendment";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "";
                    _DBResponse.Status = 0;
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



        public void GetAllCommodityForPageAmendment(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType = Result["CommodityType"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCommodity, State };
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

        public void GetAmenSBDetailsByAmendNO(string AmendNO)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentDetailsByAmendNo", CommandType.StoredProcedure, DParam);
            List<CHN_AmendmentViewModel> LstSB = new List<CHN_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_AmendmentViewModel
                    {
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["OldShipBillDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }



        public void GetAmenDetailsByAmendNO(string AmendNO)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentDetailsByAmendNo", CommandType.StoredProcedure, DParam);
            List<CHN_Amendment> LstSB = new List<CHN_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_Amendment
                    {
                        AmendmentNo = Convert.ToString(Result["AmendNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendDate"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        FOBValue = Convert.ToString(Result["FOB"]),
                        Pkg = Convert.ToString(Result["Pkg"]),
                        ShipBillDate = Convert.ToString(Result["SBDate"]),
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        Cargo = Convert.ToString(Result["Cargo"]),
                        Type = Convert.ToString(Result["Type"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        Area = Result["Area"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }



        public void lstAmendDate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmenDataAll", CommandType.StoredProcedure, DParam);
            List<CHN_Amendment> LstSB = new List<CHN_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_Amendment
                    {
                        AmendmentNo = Convert.ToString(Result["ShipbillNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendDate"])


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetInvoiceListForShipbillAmend()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListForShipbillAmend", CommandType.StoredProcedure);
            List<dynamic> LstSB = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new
                    {
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListForShipbillAmend()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_id", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfShipBillAmned", CommandType.StoredProcedure, Dparam);
            List<CHN_AmendmentViewModel> LstSB = new List<CHN_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_AmendmentViewModel
                    {
                        ID = Convert.ToInt32(Result["id"]),
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["OldShipBillDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetShipbillAmendDet(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_id", MySqlDbType = MySqlDbType.Int32, Value = id });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfShipBillAmned", CommandType.StoredProcedure, Dparam);
            CHN_AmendmentViewModel LstSB = new CHN_AmendmentViewModel();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB = (new CHN_AmendmentViewModel
                    {
                        ID = Convert.ToInt32(Result["id"]),
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ExporterID = Convert.ToString(Result["ExporterID"]),
                        ExporterName = Result["ExporterName"].ToString(),
                        CargoID = Convert.ToInt32(Result["CommodityID"]),
                        Cargo = Result["CommodityName"].ToString(),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Pkg = Convert.ToInt32(Result["Pkg"]),
                        FOB = Convert.ToString(Result["FOB"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["AmendDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        ShippingLineName= Convert.ToString(Result["ShippingLineName"]),
                        PortOfLoading= Convert.ToString(Result["PortOfLoading"]),
                        PortOfDest= Convert.ToString(Result["PortOfDest"]),
                        SBTypeName= Convert.ToString(Result["SBTypeName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListOfShippingLineNameAmend()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfShippingLine", CommandType.StoredProcedure);
            IList<CHN_ShipingLine> lstShipping = new List<CHN_ShipingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShipping.Add(new CHN_ShipingLine
                    {
                        Id = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShipping;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion
        #region SCMRT
        public void ListOfFinalDestination(string CustodianName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianName", MySqlDbType = MySqlDbType.VarChar, Value = CustodianName });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFinalDestination", CommandType.StoredProcedure, DParam);


            List<CHN_FinalDestination> LstCustodian = new List<CHN_FinalDestination>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCustodian.Add(new CHN_FinalDestination
                    {
                        CustodianCode = Convert.ToString(Result["CustodianCode"]),
                        CustodianName = Convert.ToString(Result["CustodianName"]),
                        CustodianId = Convert.ToInt32(Result["CustodianId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCustodian;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion

        #region Container Stuffing Approval
        public void GetContStuffingForApproval(int StuffingReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerStuffingApproval> objPaySheetStuffing = new List<ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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

        public void GetPortOfCall()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfCall", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PortOfCall> objPortOfCallList = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPortOfCallList.Add(new PortOfCall()
                    {

                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPortOfCallList;
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

        public void GetPortOfCallForPage(string PortCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PortCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfCallForPage", CommandType.StoredProcedure, Dparam);
            IList<PortOfCall> lstPortOfCall = new List<PortOfCall>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePortOfCall = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPortOfCall.Add(new PortOfCall
                    {
                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePortOfCall = Convert.ToBoolean(Result["StateParty"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPortOfCall, StatePortOfCall };
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

        public void GetNextPortOfCallForPage(string PortCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PortCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetNextPortOfCallForPage", CommandType.StoredProcedure, Dparam);
            IList<PortOfCall> lstNextPortOfCall = new List<PortOfCall>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StateNextPortOFCall = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstNextPortOfCall.Add(new PortOfCall
                    {
                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StateNextPortOFCall = Convert.ToBoolean(Result["StatePayer"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstNextPortOfCall, StateNextPortOFCall };
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


        public void AddEditContainerStuffingApproval(PortOfCall objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPortOfCall.ApprovalDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ModeOfTransport", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ModeOfTransport });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditContainerStuffingApproval", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Stuffing Approved Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Data Already Exists";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofContainerStuffingApproval(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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

        public void GetContainerStuffingApprovalById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PortOfCall objDestuffing = new PortOfCall();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.ApprovalDate = Convert.ToString(Result["ApprovalDate"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.PortOfCallName = Convert.ToString(Result["PortOfCallName"]);
                    objDestuffing.PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objDestuffing.NextPortOfCallName = Convert.ToString(Result["NextPortOfCallName"]);
                    objDestuffing.NextPortOfCallCode = Convert.ToString(Result["NextPortOfCallCode"]);
                    objDestuffing.ModeOfTransportName = Convert.ToString(Result["ModeOfTransport"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllContainerStuffingApprovalSearch(string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofContainerStuffingApprovalSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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

        #region Get CIM-SF Details

        public void GetCIMSFDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRStuffingDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();




            try
            {

                int count = Result.Tables.Count;
                if (count == 1)
                {
                    if (Convert.ToInt32(Result.Tables[0].Rows[0]["Result"]) == 1)
                    {
                        _DBResponse.Status = 2;
                        _DBResponse.Message = "CIM SF Message Already Send.";
                        _DBResponse.Data = Result;
                    }
                    else
                    {
                        _DBResponse.Status = 3;
                        _DBResponse.Message = "CIM SF Acknowledgement Received Successfully, Please Do Amendment";
                        _DBResponse.Data = Result;
                    }

                }
                else
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetCIMSFDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateCIMSFStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion

        #region Loaded Container Stuffing Approval
        public void GetLoadedContainerStuffingForApproval(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadContainerRequestForApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerStuffingApproval> objPaySheetStuffing = new List<ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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


        public void AddEditLoadContainerStuffingApproval(PortOfCall objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ApprovalId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPortOfCall.ApprovalDate).ToString("yyyy-MM-dd HH:mm:ss") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ModeOfTransport", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ModeOfTransport });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditLoadContainerStuffingApproval", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Loaded Container Stuffing Approved Successfully" : "Loaded Container Stuffing Approval Updated Successfully";

                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can't Update CIM ASR File Already Send.";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM ASR Acknowledgement Received.";
                }
                else if (result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Loaded Container Stuffing Approval Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofLoadContainerStuffingApproval(int Page, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofLoadContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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

        public void GetLoadContainerStuffingApprovalById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewLoadContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PortOfCall objDestuffing = new PortOfCall();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.ApprovalDate = Convert.ToString(Result["ApprovalDate"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.PortOfCallName = Convert.ToString(Result["PortOfCallName"]);
                    objDestuffing.PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objDestuffing.NextPortOfCallName = Convert.ToString(Result["NextPortOfCallName"]);
                    objDestuffing.NextPortOfCallCode = Convert.ToString(Result["NextPortOfCallCode"]);
                    objDestuffing.ModeOfTransportName = Convert.ToString(Result["ModeOfTransport"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }




        #endregion

        #region Get Loaded CIM-ASR Details

        public void GetLoadedContainerCIMASRDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetLoadContSCMTRCIMASRDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetLoadContCIMASRDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateLoadContCIMASRStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion
        #region Get CIM-ASR Details


        public void GetCIMASRDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRCIMASRDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetCIMASRDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateCIMASRStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion

        #region ACTUAL ARRIVAL DATE AND TIME 

        public void GetContainerNoForActualArrival(string ContainerBoxSearch, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerBoxSearch });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForActualArrival", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerNoForActualArrival> objContainerNoForActualArrival = new List<ContainerNoForActualArrival>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    objContainerNoForActualArrival.Add(new ContainerNoForActualArrival()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { ContainerList = objContainerNoForActualArrival, State }; ;
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
        public void AddEditActualArrivalDatetime(Chn_ActualArrivalDatetime objActualArrivalDatetime)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objActualArrivalDatetime.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.GatePassNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objActualArrivalDatetime.ArrivalDateTime).ToString("yyyy-MM-dd HH:mm:ss") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditActualArrivalDatetime", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Arrival Datetime Saved Successfully" : "Arrival Datetime Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "CFSCode already exist";
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
        public void GetListOfArrivalDatetime(int Uid, int Id)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfArrivalDatetime", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Chn_ActualArrivalDatetime> objArrivalDatetimeList = new List<Chn_ActualArrivalDatetime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objArrivalDatetimeList.Add(new Chn_ActualArrivalDatetime()
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        ArrivalDateTime = Convert.ToString(Result["ArrivalDateTime"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objArrivalDatetimeList;
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


        #region Loaded Container Stuffing SF
        public void GetLoadedContainerStuffingForSF(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadContainerRequestForSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerStuffingApproval> objPaySheetStuffing = new List<ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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


        public void AddEditLoadContainerStuffingSF(Chn_LoadContSF objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ApprovalId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditLoadContainerStuffingSF", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Loaded Container For SF Saved Successfully";

                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can't Update CIM ASR File Already Send.";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM ASR Acknowledgement Received.";
                }
                else if (result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Loaded Container Stuffing Approval Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofLoadContainerStuffingSF(int Page, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofLoadContainerStuffingSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_LoadContSF> LstStuffingApproval = new List<Chn_LoadContSF>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new Chn_LoadContSF
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        StuffingReqNo = Result["LoadReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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

        public void GetLoadContainerStuffingSFById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewLoadContainerStuffingSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Chn_LoadContSF objDestuffing = new Chn_LoadContSF();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }




        #endregion

        #region Get CIM-AT Details

        public void GetATDetails(string CFSCode, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = Convert.ToString(CFSCode) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetScmtrATDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        //public void GetCIMARDetailsUpdateStatus(int HeaderId)
        //{
        //    int Status = 0;
        //    DataSet Result = new DataSet();
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = HeaderId });
        //    //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
        //    // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    Result = DataAccess.ExecuteDataSet("GetCIMARDetailsUpdateStatus", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {

        //        _DBResponse.Status = 1;
        //        _DBResponse.Message = "Success";
        //        _DBResponse.Data = Result;


        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        //Result.Dispose();
        //        //Result.Close();
        //    }
        //}
        #endregion

        #region Pallatisation

        public void GetSBListPallatisation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBNoForPallatisation", CommandType.StoredProcedure, DParam);
            List<CHN_Pallatisation> LstSB = new List<CHN_Pallatisation>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_Pallatisation
                    {

                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"])


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSBDetailsPallatisation(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBStockForPalet", CommandType.StoredProcedure, DParam);
            List<CHN_PallatisationSBDetails> LstSBDetails = new List<CHN_PallatisationSBDetails>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSBDetails.Add(new CHN_PallatisationSBDetails
                    {
                        GodownID = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        Qty = Convert.ToInt32(Result["Units"]),
                        RefID = Convert.ToInt32(Result["RefId"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        SQM = Convert.ToDecimal(Result["Area"]),
                        LocationId = Convert.ToString(Result["LocationId"]),
                        LocationName = Convert.ToString(Result["LocationName"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        ComodityId = Convert.ToString(Result["ComodityId"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        Vol = Convert.ToDecimal(Result["Vol"]),
                        Item = Convert.ToString(Result["Item"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        PkgType = Convert.ToString(Result["PkgType"]),
                        FOB = 0M,
                         SBNo=Convert.ToString(Result["ShippingBillNo"]),
                          SBDate = Convert.ToString(Result["ShippingBillDate"]),
                        //GeneralCBM = Convert.ToDecimal(Result["CUM"]),
                        // ReserveCBM = Convert.ToDecimal(Result["ReservedCBM"]),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSBDetails;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetPartyNameForPallatisation(int Page, string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyNamesForPallatization", CommandType.StoredProcedure, DParam);
            List<PartyDet> LstPartyDetails = new List<PartyDet>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePayer = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyDetails.Add(new PartyDet
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePayer = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstPartyDetails, StatePayer };
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetChargeForPallatisation(int PartyID, int Pallet, string SEZ)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pallet", MySqlDbType = MySqlDbType.Int32, Value = Pallet });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPallatisationCharge", CommandType.StoredProcedure, DParam);
            List<CHN_PallasationChargeBase> LstChargeDetails = new List<CHN_PallasationChargeBase>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstChargeDetails.Add(new CHN_PallasationChargeBase
                    {

                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        ChargeId = Convert.ToInt32(Result["OperationId"]),
                        ChargeName = Convert.ToString(Result["ChargeName"]),
                        ChargeType = Convert.ToString(Result["ChargeType"]),
                        Clause = Convert.ToString(Result["Clause"]),
                        Discount = Convert.ToDecimal(Result["Discount"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        OperationId = Convert.ToString(Result["OperationId"]),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        SACCode = Convert.ToString(Result["SACCode"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"]),
                        Total = Convert.ToDecimal(Result["Total"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChargeDetails;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditPallatisation(CHN_Pallatisation vm, string StockXml, string StockNewXml)
        {
            string GeneratedClientId = "";
            string RetunID = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.String, Value = vm.GodownNewName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.GodownNewID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_locationID", MySqlDbType = MySqlDbType.String, Value = vm.LocationId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_locationName", MySqlDbType = MySqlDbType.String, Value = vm.LocationName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pallet", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.NoOfPallet) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UOM", MySqlDbType = MySqlDbType.String, Value = vm.PkgType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.PartyID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.String, Value = vm.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.PayeeID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.String, Value = vm.PayeeName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Total) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.TotalTaxable) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.TotalCGST) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.TotalSGST) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.TotalIGST) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.CartingRegisterId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XMLOldStockDetails", MySqlDbType = MySqlDbType.Text, Value = StockXml });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XMLNewStockDetails", MySqlDbType = MySqlDbType.Text, Value = StockNewXml });

            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNO", MySqlDbType = MySqlDbType.VarChar, Value = vm.SBNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDATE", MySqlDbType = MySqlDbType.Datetime, Value = Convert.ToDateTime(vm.SBDate) });
            //lstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargeXml });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.String, Value = "Tax" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.String, Value = vm.Invoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = vm.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReservedCBM", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.ReserveCBM) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UnReservedCBM", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.GeneralCBM) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Decimal, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = 7 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = vm.SEZ });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditPallatisation", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RetunID);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = RetunID;
                    _DBResponse.Message = (Result == 1) ? "Pallatisation Saved Successfully " : "Pallatisation Updated Successfully";
                    _DBResponse.Status = Result;

                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }


                else if (Result == 33)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
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

        public void PrintPallatisationInvoice(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceID", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetPallatisationPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                // Result.Dispose();
                // Result.Close();
            }

        }


        public void GetAllPallatisationList(int EditFlag = 0, string InvoiceNo = null, string InvoiceDate = null, string ShippingBillNo = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EditFlag", MySqlDbType = MySqlDbType.Int32, Value = EditFlag });
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Value = ShippingBillNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPallatisationList", CommandType.StoredProcedure, DParam);
            List<CHN_Pallatisation> LstPallatisationDetails = new List<CHN_Pallatisation>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPallatisationDetails.Add(new CHN_Pallatisation
                    {

                        Invoice = Convert.ToString(Result["Pallets"]),
                        InvoiceDate = Convert.ToString(Result["GodownName"]),
                       
                        SBNo = Convert.ToString(Result["SBNO"]),
                        SBDate = Convert.ToString(Result["SBDATE"]),
                         GeneralCBM = Convert.ToDecimal(Result["PKG"]),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPallatisationDetails;
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        #endregion
    }
}