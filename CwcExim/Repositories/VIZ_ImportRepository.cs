using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;
using CwcExim.Areas.Import.Models;
using System.Globalization;
using Newtonsoft.Json;
using EinvoiceLibrary;
using CwcExim.Areas.Export.Models;

namespace CwcExim.Repositories
{
    public class VIZ_ImportRepository
    {

        private DatabaseResponse _DBResponse;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Import Yard Payment Sheet
        public void GetAppraismentRequestForPaymentSheet(int AppraisementAppId = 0, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VIZPaySheetStuffingRequest> objPaySheetStuffing = new List<VIZPaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new VIZPaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),

                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

                        Billtoparty = Result["BilltoParty"].ToString(),
                        IsInsured = Result["IsInsured"].ToString(),
                        Transporter = Result["transporter"].ToString(),
                        InsuredFrom = Result["InsuredDateFrm"].ToString(),
                        InsuredTo = Result["InsuredDateTo"].ToString(),

                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
                        HBLNo = Convert.ToString(Result["ContainerNo"]),
                        Importer = Convert.ToString(Result["Importer"]),
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ImpCount = Convert.ToInt32(Result["ImpCount"])

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
        public void GetContainerForPaymentSheet(int AppraisementAppId, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"]),
                        OBLNo = Convert.ToString(Result["OBLNo"]),
                        SealCutDate = Convert.ToString(Result["SealCutDate"]),
                        NoOfPkg = Convert.ToInt32(Result["NoOfPackages"]),
                        GrWait = Convert.ToDecimal(Result["GrossWeight"]),
                        IsBond = Convert.ToBoolean(Result["IsBond"]),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer.GroupBy(o => o.CFSCode).Select(o => o.First()).ToList();
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
        public void ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofYardInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VIZListOfExpInvoice> lstExpInvoice = new List<VIZListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new VIZListOfExpInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"]),
                        AppNo = Convert.ToString(Result["AppNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])

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

        public void ListOfChaForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForPage", CommandType.StoredProcedure, Dparam);
            IList<VIZCHAForPage> lstCHA = new List<VIZCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new VIZCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        BillToParty = Convert.ToBoolean(Convert.ToInt32(Result["BilltoParty"].ToString())),
                        IsInsured = Convert.ToBoolean(Convert.ToInt32(Result["IsInsured"].ToString())),
                        IsTransporter = Convert.ToBoolean(Convert.ToInt32(Result["transporter"].ToString())),
                        InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        InsuredTodate = Result["InsuredDateTo"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
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
        #region Yard Invoice
        public void GetYardPaymentSheet(string InvoiceDate, string DeliveryDate, int AppraisementId, string InvoiceType,
            string ContainerXML, int InvoiceId, int OTHours, int PartyId, int PayeeId, int isdirect, int NoOfVehicles, int Distance, int OwnMovement, int InsuredParty, int YardToBond, string SEZ)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.Int32, Value = NoOfVehicles });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Transporter", MySqlDbType = MySqlDbType.Int32, Value = OwnMovement });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Insured", MySqlDbType = MySqlDbType.Int32, Value = InsuredParty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = InsuredParty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardToBond", MySqlDbType = MySqlDbType.Int32, Value = YardToBond });

            LstParam.Add(new MySqlParameter { ParameterName = "in_isdirect", MySqlDbType = MySqlDbType.Int32, Value = isdirect });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            VIZInvoiceYard objInvoice = new VIZInvoiceYard();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
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

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new VIZPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
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
                        objInvoice.lstPostPaymentCont.Add(new VIZPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
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
                        objInvoice.lstPostPaymentChrg.Add(new VIZPostPaymentChrg
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
                        objInvoice.lstContWiseAmount.Add(new VIZContainerWiseAmount
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new VIZOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentNo = Result["SBNo"] == System.DBNull.Value ? "" : Result["SBNo"].ToString(),
                            DocumentType = "Yard",
                            DocumentDate = Result["SBDate"] == System.DBNull.Value ? System.DateTime.Now.ToString("yyyy/MM/dd") : Result["SBDate"].ToString(),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.NDays = Result["NDays"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VIZPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

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
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
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


        public void AddEditInvoice(VIZInvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Vehicle, int Uid, int Direct, int IsYardtoBond, string SEZ, int BillToParty, int Distance, int OTHours, int IsTransporter, int IsInsured, string Module, string CargoXML = "")

        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDirect", MySqlDbType = MySqlDbType.Int32, Value = Direct });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vehicle", MySqlDbType = MySqlDbType.Int32, Value = Vehicle });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsYardtoBond", MySqlDbType = MySqlDbType.Int32, Value = IsYardtoBond });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = BillToParty });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsTransporter", MySqlDbType = MySqlDbType.Int32, Value = IsTransporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = IsInsured });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceYard", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        public void GetImpPaymentPartyForFCLPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForFCLPage", CommandType.StoredProcedure, Dparam);
            IList<VIZImpPartyForpage> lstParty = new List<VIZImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {

                    Status = 1;
                    lstParty.Add(new VIZImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        InsuredToDate = Convert.ToString(Result["InsuredDateTo"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredDateFrm"]),
                        IsTransporter = Convert.ToBoolean(Result["transporter"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
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
                    _DBResponse.Data = new { lstParty, State };
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetails", CommandType.StoredProcedure, DParam);
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
                            PrdDesc = null,//Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null, // Result["Unit"].ToString(),
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
            VIZ_IrnB2CDetails Obj = new VIZ_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetails", CommandType.StoredProcedure, DParam);
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
            int Result = DA.ExecuteNonQuery("Addeditirnresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
                    _DBResponse.Status = Result;
                }

                else
                {
                    log.Info("After Calling Stored Procedure Error" + " Invoice No " + InvoiceNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);
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
        public void GetImpPaymentPayeeForFCLPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPayeeForFCLPage", CommandType.StoredProcedure, Dparam);
            IList<VIZImpPartyForpage> lstParty = new List<VIZImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {

                    Status = 1;
                    lstParty.Add(new VIZImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        InsuredToDate = Convert.ToString(Result["InsuredDateTo"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredDateFrm"]),
                        IsTransporter = Convert.ToBoolean(Result["transporter"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
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
                    _DBResponse.Data = new { lstParty, State };
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

        //public void GetBillToPartyDetails(string PartyId)
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });            
        //    IDataParameter[] Dparam = lstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetBillToPartyDetails", CommandType.StoredProcedure, Dparam);
        //    IList<WFLDImpPartyForpage> lstParty = new List<WFLDImpPartyForpage>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        bool State = false;
        //        while (Result.Read())
        //        {

        //            Status = 1;
        //            lstParty.Add(new WFLDImpPartyForpage
        //            {
        //                PartyId = Convert.ToInt32(Result["ImporterId"]),
        //                PartyName = Convert.ToString(Result["ImporterName"]),
        //                Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
        //                State = Convert.ToString(Result["StateName"]),
        //                StateCode = Convert.ToString(Result["StateCode"]),
        //                GSTNo = Convert.ToString(Result["GSTNo"]),
        //                InsuredToDate = Convert.ToString(Result["InsuredDateTo"]),
        //                InsuredFrmDate = Convert.ToString(Result["InsuredDateFrm"]),
        //                IsTransporter = Convert.ToBoolean(Result["transporter"]),
        //                IsInsured = Convert.ToBoolean(Result["IsInsured"]),
        //                PartyCode = Convert.ToString(Result["PartyCode"])
        //            });
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                State = Convert.ToBoolean(Result["State"]);
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = new { lstParty, State };
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Dispose();
        //        Result.Close();
        //    }

        //}


        #endregion
        #region Destuffing Payment Sheet


        public void GetDeStuffingRequestForImpPaymentSheet(int DestuffingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDestuffingRequestForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VIZ_DestuffingPaymentSheet> objPaySheetStuffing = new List<VIZ_DestuffingPaymentSheet>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new VIZ_DestuffingPaymentSheet()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        StuffingReqId = Convert.ToInt32(Result["DestuffingId"]),
                        StuffingReqNo = Convert.ToString(Result["DestuffingNo"]),
                        StuffingReqDate = Convert.ToString(Result["DestuffingDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"])
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
        public void GetDestuffingContForPaymentSheet(int DestuffingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDestuffingRequestForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.VIZ_PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.VIZ_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.VIZ_PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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

        #region Godown Destuffing Payment Sheet 
        public void GetDestuffingPaymentSheet(string InvoiceDate, int DestuffingAppId, string SEZ, int PartyId, int PayeeId,
            string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyID", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                VIZ_ImpPaymentSheet objInvoice = new VIZ_ImpPaymentSheet();
                IDataReader Result = DataAccess.ExecuteDataReader("GetAfterDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);

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
                        objInvoice.lstPostPaymentCont.Add(new VIZExpInvoiceContainerBase
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
                            StuffingDate = "",
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            PltBox = Convert.ToInt32(Result["PLtBox"]),
                            ParkDays = Convert.ToInt32(Result["ParkDays"]),
                            LockDays = Convert.ToInt32(Result["LockDays"]),

                        });
                        objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new VIZExpInvoiceChargeBase
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
                            OperationId = Result["OperationId"].ToString()
                         
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new Areas.Import.Models.VIZ_ExpContWiseAmount
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
                        objInvoice.lstOperationContwiseAmt.Add(new Areas.Import.Models.VIZ_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            DocumentNo = Convert.ToString(Result["SBNo"]),
                            DocumentDate = Convert.ToString(Result["SBDate"]),
                            Clause = Result["Clause"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["hasSDAvailableBalance"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.NDays = Result["NDays"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VIZPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

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
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
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

            }
        }


        public void AddEditDestuffingInvoice(VIZ_ImpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditDestuffingInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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


        public void GetAllDestuffingPaymentSheet()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VIZListOfExpInvoice> objPaymentSheetContainer = new List<VIZListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new VIZListOfExpInvoice()
                    {
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

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



        #endregion

        // Not required
        //public void GetCargoPaymentSheet(string InvoiceDate, int StuffingAppId, string ContainerXML)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    PaySheetChargeDetails objPaymentSheet = new PaySheetChargeDetails();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentSheet.lstPSContainer.Add(new PSContainer()
        //            {
        //                CFSCode = Convert.ToString(Result["CFSCode"]),
        //                ContainerNo = Convert.ToString(Result["ContainerNo"]),
        //                Size = Convert.ToString(Result["Size"]),
        //                IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //                Insured = Convert.ToString(Result["Insured"])
        //            });
        //        }
        //        //if (Result.NextResult())
        //        //{
        //        //    while (Result.Read())
        //        //    {
        //        //        objPaymentSheet.lstEmptyGR.Add(new PSEmptyGroudRent()
        //        //        {
        //        //            ContainerType = Convert.ToString(Result["ContainerType"]),
        //        //            CommodityType = Convert.ToString(Result["CommodityType"]),
        //        //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //        //            Size = Convert.ToString(Result["Size"]),
        //        //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //        //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //        //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //        //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //        //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //        //            CFSCode = Convert.ToString(Result["CFSCode"]),
        //        //            FOBValue = Convert.ToDecimal(Result["Fob"]),
        //        //            IsInsured = Convert.ToInt32(Result["Insured"])
        //        //        });
        //        //    }
        //        //}
        //        //if (Result.NextResult())
        //        //{
        //        //    while (Result.Read())
        //        //    {
        //        //        objPaymentSheet.lstLoadedGR.Add(new PSLoadedGroudRent()
        //        //        {
        //        //            ContainerType = Convert.ToString(Result["ContainerType"]),
        //        //            CommodityType = Convert.ToString(Result["CommodityType"]),
        //        //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //        //            Size = Convert.ToString(Result["Size"]),
        //        //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //        //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //        //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //        //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //        //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //        //            CFSCode = Convert.ToString(Result["CFSCode"]),
        //        //            FOBValue = Convert.ToDecimal(Result["Fob"]),
        //        //            IsInsured = Convert.ToInt32(Result["Insured"])
        //        //        });
        //        //    }
        //        //}
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.InsuranceRate = Convert.ToDecimal(Result["InsuranceCharge"]) / 100 / 1000;
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstStorageRent.Add(new Areas.Import.Models.StorageRent()
        //                {
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    ActualCUM = Convert.ToDecimal(Result["CUM"]),
        //                    ActualSQM = Convert.ToDecimal(Result["SQM"]),
        //                    ActualWeight = Convert.ToDecimal(Result["GrossWeight"]),
        //                    StuffCUM = Convert.ToDecimal(Result["DeStuffCUM"]),
        //                    StuffSQM = Convert.ToDecimal(Result["DeStuffSQM"]),
        //                    StuffWeight = Convert.ToDecimal(Result["DestuffingWeight"]),
        //                    StorageDays = Convert.ToInt32(Result["StorageDays"]),
        //                    StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
        //                    StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
        //                    StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.RateSQMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
        //                objPaymentSheet.RateSQMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
        //                objPaymentSheet.RateCUMPerWeek = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
        //                objPaymentSheet.RateMTPerDay = Convert.ToDecimal(Result["RateMetricTonPerDay"]);
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstInsuranceCharges.Add(new InsuranceCharge()
        //                {
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
        //                    IsInsured = Convert.ToInt16(Result["Insured"]),
        //                    FOB = Convert.ToDecimal(Result["Fob"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstPSHTCharge.Add(new PSHTCharges()
        //                {
        //                    ChargeId = Convert.ToInt32(Result["HTChargesID"]),
        //                    ChargeName = Convert.ToString(Result["Description"]),
        //                    Charge = Convert.ToDecimal(Result["RateCWC"])
        //                });
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaymentSheet;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Dispose();
        //        Result.Close();
        //    }
        //}
        //public void AddEditCargoInvoice(PaymentSheetFinalModel ObjPSFinalModel, string ContainerXML, string ChargesXML, int BranchId, int Uid)
        //{
        //    string GeneratedClientId = "0";
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.InvoiceId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.StuffingReqNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.StuffingDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PartyName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PayeeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.GSTNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.CWCTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.HTTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.AllTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.RoundUp });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.Invoice });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.Remarks });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "IMPCA" });

        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    IDataParameter[] DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Payment Invoice Saved Successfully";
        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Payment Invoice Updated Successfully";
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Status = -1;
        //        _DBResponse.Message = "Error";
        //    }
        //}
        //
        #endregion

        #region Godown Payment Sheet
        public void GetDestuffEntryNo(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryNoList> LstDestuffEntryNo = new List<DestuffingEntryNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffEntryNo.Add(new DestuffingEntryNoList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterId = Convert.ToInt32(Result["ImporterId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffEntryNo;
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
        public void ListOfImporterForMerge()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImporterMergePage", CommandType.StoredProcedure);
            IList<Importer> lstImporterName = new List<Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporterName.Add(new Importer
                    {
                        ImporterId = Convert.ToInt32(result["ImporterId"]),
                        ImporterName = result["ImporterName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImporterName;
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
        public void ListOfChaForMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForMergePage", CommandType.StoredProcedure, Dparam);
            IList<VIZCHAForPage> lstCHA = new List<VIZCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new VIZCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        BillToParty = Convert.ToBoolean(Result["BillToParty"]),
                        InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        InsuredTodate = Result["InsuredDateTo"].ToString(),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        State = Convert.ToBoolean(Result["State"]);
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
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

        public void GetImpPaymentPartyForMergePage(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForMergePage", CommandType.StoredProcedure, Dparam);
            IList<ImpPartyForpage> lstParty = new List<ImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new ImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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

        public void GetAllDeliveryMergeApplication(int Page, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMergeDelivaryAppForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZMergeDeliveryApplicationList> LstDeliveryApp = new List<VIZMergeDeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new VIZMergeDeliveryApplicationList
                    {
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"].ToString()),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        DeliveryDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeliveryApp;
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

        public void GetGSTValue(int PartyId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGSTforSClick", CommandType.StoredProcedure, Dparam);
            String GSTNo = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;


                    GSTNo = Result["GSTNO"].ToString() + "-" + Result["PartyState"].ToString();

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = GSTNo;
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

        public void GetDeliveryPaymentSheetSingle(string InvoiceType, int DestuffingAppId, string DeliveryDate, string InvoiceDate,
           int InvoiceId, string XMLText, int PartyId, int PayeeId, int vhecleNo, int vhecleNoUn, bool IsInsured, bool IsBilltoparty, bool Transporter, bool ImportToBond, int Cargo, string XMLGodown, int OTHours = 0, int OblFlag = 0, string SEZ = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = XMLText });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = InvoiceDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.Int32, Value = vhecleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNoUn", MySqlDbType = MySqlDbType.Int32, Value = vhecleNoUn });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsBilltoparty == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargo", MySqlDbType = MySqlDbType.Int32, Value = Cargo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsInsured == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsTransporter", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Transporter == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImportToBond", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ImportToBond == false ? 0 : 1) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_OblFlag", MySqlDbType = MySqlDbType.Int32, Value = OblFlag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XMLGodown", MySqlDbType = MySqlDbType.Text, Value = XMLGodown });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();



            //getsingleclickdeliverypaymentsheet_new1

            //getsingleclickdeliverypaymentsheet_v2
            VIZInvoiceGodown objInvoice = new VIZInvoiceGodown();
            IDataReader Result = DataAccess.ExecuteDataReader("getsingleclickdeliverypaymentsheet", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new VIZPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["DelDuty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new VIZPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
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
                        objInvoice.lstContWiseAmount.Add(new VIZContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            EntryFee = Result["EntryFee"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Result["CstmRevenue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Result["GrEmpty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Result["GrLoaded"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Result["ReeferCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Result["StorageCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Result["InsuranceCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Result["PortCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Result["WeighmentCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WeighmentCharge"]),
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
                        objInvoice.lstInvoiceCargo.Add(new VIZInvoiceCargo
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
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            LineNo = Result["LineNo"].ToString() == null ? "" : Result["LineNo"].ToString()
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new VIZPostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
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
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),



                        });
                    }


                }

                if (Result.NextResult())
                {
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new VIZOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            Clause = Result["Clause"] == System.DBNull.Value ? "" : Result["Clause"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            DocumentNo = Result["SBNo"] == System.DBNull.Value ? "" : Result["SBNo"].ToString(),
                            DocumentType = "Godown",
                            DocumentDate = Result["SBDate"] == System.DBNull.Value ? System.DateTime.Now.ToString("yyyy/MM/dd") : Result["SBDate"].ToString(),

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
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new VIZDeliPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

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
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
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

        public void AddEditSingleMergeDeliveryApplication(VIZMergeDeliveryIssueViewModel ObjDeliveryApp,
         string DeliveryAppDtlXml, string DeliveryOrdDtlXml,
         string lstPrePaymentContXML, string lstPostPaymentContXML,
               string lstPostPaymentChrgXML, string lstContWiseAmountXML,
               string lstOperationCFSCodeWiseAmountXML, string lstPostPaymentChrgBreakupXML, string lstInvoiceCargoXML, string DeliveryGodownDtlXml)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });


            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryAppDtlXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryOrdDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstContWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstContWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryGodownDtlXml", MySqlDbType = MySqlDbType.Text, Value = DeliveryGodownDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstOperationCFSCodeWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstOperationCFSCodeWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgBreakupXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgBreakupXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.DeliveryDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNos", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNos });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNoUn", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNoUn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjDeliveryApp.DeliApp.SEZ) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Insured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.IsInsured) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OwnMovement", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.IsTransporter) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.BillToParty) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargo", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.Cargo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpToBondConversion", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.ImportToBondCon) });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            VIZmergedet pdet = new VIZmergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeDelPaymentIssue", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();



            try
            {



                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details with Invoice and Issue Slip Saved Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice cannot be generated due to  balance is low";
                    _DBResponse.Data = null;
                }


                else if (Result == 33)
                {
                    _DBResponse.Status = 33;
                    _DBResponse.Message = "Invoice can not be generated,Payer has not SD account ";
                    _DBResponse.Data = null;
                }

                else if (Result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "0 Amount Invoice can not be generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 5)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice already generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Invoice Date should be Greater than or equal to already generated Invoice Date";
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

        public void GetBOELineNoForDelivery(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZBOELineNoList> LstBOELineNo = new List<VIZBOELineNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBOELineNo.Add(new VIZBOELineNoList
                    {

                        BOELineNo = Result["BOELineNo"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        CHAId = Result["CHAId"].ToString(),
                        Billtoparty = Result["BilltoParty"].ToString(),
                        IsInsured = Result["IsInsured"].ToString(),
                        Transporter = Result["transporter"].ToString(),
                        InsuredFrom = Result["InsuredDateFrm"].ToString(),
                        InsuredTo = Result["InsuredDateTo"].ToString(),
                        Oblcount = Result["Oblcount"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PartyId = Result["PartyId"].ToString(),
                        PartyInsured = Result["PartyInsured"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBOELineNo;
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
        public void GetBOELineNoDetForDelivery(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoDetForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZDeliveryApplicationDtl ObjDeliveryApp = new VIZDeliveryApplicationDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDeliveryApp.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjDeliveryApp.CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]);
                    ObjDeliveryApp.SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]);
                    ObjDeliveryApp.GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]);
                    ObjDeliveryApp.ResvGrossWt = Convert.ToDecimal(Result["ResvGrossWt"] == DBNull.Value ? 0 : Result["ResvGrossWt"]);
                    ObjDeliveryApp.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDeliveryApp.CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]);
                    ObjDeliveryApp.OOC_BOENo = Result["BOENo"].ToString();
                    ObjDeliveryApp.OOC_BOEDATE = Result["BOEDate"] == DBNull.Value ? "" : Convert.ToDateTime(Result["BOEDate"]).ToString("dd/MM/yyyy");
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDeliveryApp.Importer = Convert.ToString(Result["Importer"] == DBNull.Value ? "" : Result["Importer"]);
                    ObjDeliveryApp.ShippingLine = Convert.ToString(Result["ShippingLineName"] == DBNull.Value ? "" : Result["ShippingLineName"]);
                    ObjDeliveryApp.TSA = Convert.ToString(Result["TSANo"] == DBNull.Value ? "" : Result["TSANo"]);
                    ObjDeliveryApp.CommodityId = Convert.ToInt32(Result["Cargo"] == DBNull.Value ? 0 : Result["Cargo"]);

                    // ObjDeliveryApp.Commodity = Result["Commodity"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.OblFreeFlag = Convert.ToInt32(Result["Flag"].ToString());
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryApp;
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

        public void ListOfGodownByOBL(string OBL)
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Value = OBL });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownAccessRightsByOBL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.WFLDGodownListWithDestiffDetails> lstGodownList = new List<Areas.Import.Models.WFLDGodownListWithDestiffDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.WFLDGodownListWithDestiffDetails
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),
                        DestuffWeight = Convert.ToDecimal(Result["DestuffWeight"]),
                        DstuffReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                        DstuffCBM = Convert.ToDecimal(Result["CBM"]),
                        DstuffSQM = Convert.ToDecimal(Result["SQM"]),
                        DstuffCIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        DstuffGrossDuty = Convert.ToDecimal(Result["GrossDuty"]),
                        DstuffRevSQM = Convert.ToDecimal(Result["RevSqm"]),

                        DestuffResWeight = Convert.ToDecimal(Result["RevWgt"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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

        public void ListOfChaForSingleMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSingleMergePage", CommandType.StoredProcedure, Dparam);
            IList<VIZCHAForPage> lstCHA = new List<VIZCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new VIZCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        State = Convert.ToBoolean(Result["State"]);
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
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

        public void ListOfChaForPageforSingleClick(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSinglePage", CommandType.StoredProcedure, Dparam);
            IList<VIZCHAForPage> lstCHA = new List<VIZCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new VIZCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        BillToParty = Convert.ToBoolean(Result["BillToParty"]),
                        InsuredTodate = Result["InsuredDateTo"].ToString(),
                        InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        IsTransporter = Convert.ToBoolean(Result["transporter"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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
                    _DBResponse.Data = new { lstCHA, State };
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

        public void ListOfPayeeForPageforSingleClick(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPayeeForSinglePage", CommandType.StoredProcedure, Dparam);
            IList<VIZCHAForPage> lstCHA = new List<VIZCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new VIZCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        BillToParty = Convert.ToBoolean(Result["BillToParty"]),
                        InsuredTodate = Result["InsuredDateTo"].ToString(),
                        InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        IsTransporter = Convert.ToBoolean(Result["transporter"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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
                    _DBResponse.Data = new { lstCHA, State };
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

        public void GetCIFFromOOCDelivery(String BOE, String BOEDT)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOE });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDT).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CIFFromOOC CIFFromOOCOBJ = new CIFFromOOC();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CIFFromOOCOBJ.CIF = Result["CIF_VALUE"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIF_VALUE"]);
                    CIFFromOOCOBJ.Duty = Result["DUTY"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DUTY"]);
                    CIFFromOOCOBJ.BOE_DATE = Convert.ToString(Result["BOE_DATE"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CIFFromOOCOBJ;
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

        public void GetIssueSlip(int IssueSlipId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_Issueslip ObjIssueSlip = new VIZ_Issueslip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]);
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjIssueSlip.IssueSlipNo = Result["IssueSlipNo"].ToString();
                    ObjIssueSlip.IssueSlipDate = Result["IssueSlipDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                    ObjIssueSlip.InvoiceNo = Result["InvoiceNo"].ToString();
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VIZ_IssueSlipContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VIZ_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
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
        public void GetAllIssueSlip()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_Issueslip> LstIssueSlip = new List<VIZ_Issueslip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new VIZ_Issueslip
                    {
                        IssueSlipNo = Result["IssueSlipNo"].ToString(),
                        IssueSlipDate = Result["IssueSlipDate"].ToString(),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
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
        public void GetInvoiceNoForIssueSlip()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_Issueslip> LstIssueSlip = new List<VIZ_Issueslip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new VIZ_Issueslip
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
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
        public void GetInvoiceDetForIssueSlip(int InvoiceId)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_Issueslip ObjIssueSlip = new VIZ_Issueslip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VIZ_IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VIZ_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
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


        public void GetInvoiceDetForMergeIssueSlip(String InvoiceNo)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = InvoiceNo });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForMergeIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_Issueslip ObjIssueSlip = new VIZ_Issueslip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"].ToString());
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new VIZ_IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new VIZ_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
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
        public void GetIssueSlipForPreview(int IssueSlipId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIssueSlipForPreview", CommandType.StoredProcedure, DParam);
            VIZ_Issueslip ObjIssueSlip = new VIZ_Issueslip();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.TotalCWCDues = Convert.ToDecimal(Result["TotalCWCDues"] == DBNull.Value ? 0 : Result["TotalCWCDues"]);
                    ObjIssueSlip.CRNoDate = Convert.ToString(Result["CRNoDate"] == null ? "" : Result["CRNoDate"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstIssueSlipRpt.Add(new VIZ_IssueSlipReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            MarksNo = Convert.ToString(Result["MarksNo"] == null ? "" : Result["MarksNo"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            Weight = Convert.ToString(Result["Weight"] == null ? "" : Result["Weight"]),
                            ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
                            DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                            Location = Result["Location"].ToString(),
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjIssueSlip;
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
                Result.Dispose();
                Result.Close();
            }
        }

        public void ListOfShippingLineForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<Areas.Import.Models.ShippingLine> lstShippingLine = new List<Areas.Import.Models.ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Areas.Import.Models.ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineName = result["NameAddress"].ToString()
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
        public void ListOfCHAForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<Areas.ExpSealCheking.Models.CHA> lstCHA = new List<Areas.ExpSealCheking.Models.CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Areas.ExpSealCheking.Models.CHA
                    {
                        CHAId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["NameAddress"].ToString()
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

        public void ListOfImporterCustForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfImporterForCustPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.ImporterForPage> lstImporter = new List<Areas.Import.Models.ImporterForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Areas.Import.Models.ImporterForPage
                    {
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ImporterName = Result["ImporterName"].ToString(),
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
                    _DBResponse.Data = new { lstImporter, State };
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
        public void ListOfImporterForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Importer" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<Importer> lstImporter = new List<Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Importer
                    {
                        ImporterId = Convert.ToInt32(result["EximTraderId"]),
                        ImporterName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImporter;
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

        public void ListOfPODForm()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfPOD", CommandType.StoredProcedure);
            IList<PortOfDischarge> lstPOD = new List<PortOfDischarge>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPOD.Add(new PortOfDischarge
                    {
                        PODId = Convert.ToInt32(result["PortId"]),
                        PODName = result["PortName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPOD;
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

        public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<VIZImpPartyForpage> lstParty = new List<VIZImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new VIZImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),


                        IsTransporter = Convert.ToBoolean(Result["IsTransporter"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredFrmDate"]),
                        InsuredToDate = Convert.ToString(Result["InsuredToDate"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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
                    _DBResponse.Data = new { lstParty, State };
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

        public void GetImpPaymentPayeeForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPayerForPage", CommandType.StoredProcedure, Dparam);
            IList<VIZImpPartyForpage> lstParty = new List<VIZImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new VIZImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),


                        IsTransporter = Convert.ToBoolean(Result["IsTransporter"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredFrmDate"]),
                        InsuredToDate = Convert.ToString(Result["InsuredToDate"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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
                    _DBResponse.Data = new { lstParty, State };
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

        #region Empty Container Payment Sheet

        public void AddEditEmptyContPaymentSheet(VIZInvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Uid,
           string Module, string SEZ, string CargoXML = "")
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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });


            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditEmptyContPaymentSheet", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        public void GetApplicationForEmptyContainer(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaySheetStuffingRequest> objPaySheetStuffing = new List<Areas.Import.Models.PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ApplicationFor == "YARD")
                    {
                        objPaySheetStuffing.Add(new Areas.Import.Models.PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                            StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
                    else
                    {
                        objPaySheetStuffing.Add(new Areas.Import.Models.PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["DestuffingId"]),
                            StuffingReqNo = Convert.ToString(Result["DestuffingNo"]),
                            StuffingReqDate = Convert.ToString(Result["DestuffingDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
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

        public void GetEmptyContainerListForInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<EmptyContainerListForInvoice> objListForInvoice = new List<EmptyContainerListForInvoice>();
            try
            {
                //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
                while (Result.Read())
                {
                    Status = 1;

                    objListForInvoice.Add(new EmptyContainerListForInvoice()
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        EmptyDate = Convert.ToString(Result["EmptyDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objListForInvoice;
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
        public void GetEmptyContForPaymentSheet(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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

        public void GetEmptyContByEntryId(int EntryId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = true,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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
        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, string InvoiceFor, int PartyId, string SEZ,string MovementType)
        {


            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 20, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = SEZ });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = MovementType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            VIZInvoiceYard objInvoice = new VIZInvoiceYard();
            DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);

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
                    objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                    objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();

                }
                //}

                // if (Result.NextResult())
                //{

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new VIZPreInvoiceContainer
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
                    objInvoice.lstPostPaymentCont.Add(new VIZPostPaymentContainer
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
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new VIZPostPaymentChrg
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
                    objInvoice.lstContWiseAmount.Add(new VIZContainerWiseAmount
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
                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[5].Rows)
                {
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new VIZOperationCFSCodeWiseAmount
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
                        DocumentNo = Result["SBNo"].ToString(),
                        DocumentType = "BOE",
                        DocumentDate = Result["SBDate"].ToString(),
                        Clause = Convert.ToString(Result["Clause"]),
                    });
                }

                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.lstPostPaymentChrgBreakup.Add(new VIZPostPaymentChargebreakupdate
                    {
                        ChargeId = Convert.ToInt32(Result["OperationId"]),
                        Clause = Result["Clause"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        CFSCode = Result["CFSCode"].ToString(),
                        Startdate = Result["StartDate"].ToString(),
                        EndDate = Result["EndDate"].ToString(),
                        //Clause = Convert.ToString(Result["Clause"]),
                    });
                }


                //}
                //if (Result.NextResult())
                //{
                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[7].Rows)
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
                ds.Dispose();
            }
        }

        public void GetPaymentPartyForImportInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImportInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentPartyName> objPaymentPartyName = new List<Areas.Import.Models.PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Areas.Import.Models.PaymentPartyName()
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
        public void GetBOLForEmptyCont(string InvoiceFor, int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLForEmpCont", CommandType.StoredProcedure, dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            var BOL = "";
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    BOL = result["BOLNo"].ToString() + ":" + result["BOLDate"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = BOL;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = BOL;
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

        #region Destuffing Entry
        public void AddEditDestuffingEntry(VIZDestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId, int Uid)
        {

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.StartDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.TallySheetId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.ContainerId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.CHAId == 0 ? null : ObjDestuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjDestuffing.CustomSealNo });
            /*LstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.FOB) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.GrossDuty) });*/

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DeliveryType) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DOType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DOType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.GodownId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            int Result = DataAccess.ExecuteNonQuery("AddEditImpDestuffingEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingEntryId == 0 ? "Destuffing Entry Details Saved Successfully" : "Destuffing Entry Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Destuffing Entry  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Edit Destuffing Entry Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Data already exists";
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
        public void DelDestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Entry Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Destuffing Entry Application Details As It Exist In Another Page";
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
        public void GetDestuffingEntry(int DestuffingEntryId, int BranchId, string Action)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = Action });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZDestuffingEntry ObjDestuffing = new VIZDestuffingEntry();
            try
            { 
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.TallySheetId = Result["TallySheetId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetId"]);
                    ObjDestuffing.ContainerId = Convert.ToInt32(Result["ContainerId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    //ObjDestuffing.DOType = Convert.ToInt32(Result["DOType"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.CHA = (Result["CHA"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDestuffing.TallySheetDate = Result["TallySheetDate"].ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.lstDtl.Add(new VIZ_DestuffingEntryDtl
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            TallySheetDtlId = Result["TallySheetDtlId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetDtlId"]),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"] == DBNull.Value ? 0 : Result["ReceivedPackages"]),
                            UOM = Result["UOM"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            RevArea = Convert.ToDecimal(Result["RevArea"] == DBNull.Value ? 0 : Result["RevArea"]),

                            AreaCbm = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffWeight"] == DBNull.Value ? 0 : Result["DestuffWeight"]),
                            ReservedWeight = Convert.ToDecimal(Result["ReservedWeight"] == DBNull.Value ? 0 : Result["ReservedWeight"]),

                            GodownWiseLocationIds = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLocationNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                            TallySheetArea = Result["AreaTallySheet"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["AreaTallySheet"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            OBLWiseDestuffingDate = Result["OBLWiseDestuffingDate"].ToString(),
                            IsEditable = Convert.ToInt32(Result["IsEditable"] == DBNull.Value ? 0 : Result["IsEditable"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            IsHoldRelease = Convert.ToInt32(Result["IsHoldRelease"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Convert.ToString(Result["GodownName"]),
                            GdTransId = Convert.ToInt32(Result["GdTransId"]),
                            StockDetailsId = Convert.ToInt32(Result["StockDetailsId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            //ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString(),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            IsReserved = Convert.ToInt32(Result["IsReserved"]),
                            StorageType=Convert.ToString(Result["StorageType"])

                        });
                    }
                }
                //if (Action == "Edit")
                //{
                //    if (Result.NextResult())
                //    {
                //        while (Result.Read())
                //        {
                //            ObjDestuffing.lstLocation.Add(new GodownLocation
                //            {
                //                LocationId = Convert.ToInt32(Result["LocationId"]),
                //                LocationName = Result["LocationName"].ToString()
                //            });
                //        }
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
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
        public void GetAllDestuffingEntry(int Page, int UId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntryForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_DestuffingList> LstDestuffing = new List<VIZ_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new VIZ_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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

        public void GetAllDestuffingEntry(int UId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntrySearchByContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_DestuffingList> LstDestuffing = new List<VIZ_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new VIZ_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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
        public void GetContrNoForDestuffingEntry(int BranchId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstDestuffing = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        TallySheetId = Convert.ToInt32(Result["TallySheetId"]),
                        Id = Convert.ToInt32(Result["Id"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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
        public void GetOBLforDestuffingEntry(int TallySheetId, int BranchId, String CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLforDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //List<dynamic> lstOBL = new List<dynamic>();
            List<VIZ_DestuffingEntryDtl> lstOBL = new List<VIZ_DestuffingEntryDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //lstOBL.Add(new
                    //{
                    //    TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                    //    BOLNo = Result["OblNo"].ToString()
                    //});
                    lstOBL.Add(new VIZ_DestuffingEntryDtl
                    {
                        TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                        BOLNo = Result["OblNo"].ToString(),
                        BOLDate = (Result["OBLDATE"] == null ? "" : Result["OblDate"]).ToString(),
                        LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        CargoDescription = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPkg"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        UOM = Result["UOM"].ToString(),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        Commodity = Result["Commodity"].ToString(),
                        TallySheetArea = Convert.ToDecimal(Result["AreaTallysheet"]),
                        //GodownId = Convert.ToInt32(Result["GodownId"]),
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        RemWeight = Convert.ToDecimal(Result["RemWeight"]),
                        RemPackages = Convert.ToInt32(Result["RemPackages"]),
                        IsHoldRelease = Convert.ToInt32(Result["IsHoldRelease"]),
                        BOENo = Result["BOENo"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        ShippingLine = Result["ShippingLineName"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),

                        OBLWiseDestuffingDate = Result["OblWiseDestuffingDate"].ToString(),

                    });

                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBL;
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

        public void GetReservationValidate(int ImporterId, string DestuffingDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DestuffingDate).ToString("yyyy-MM-dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationValidate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_ReasvetionArea ObjResArea = new VIZ_ReasvetionArea();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    ObjResArea.IsReserved = Convert.ToInt32(Result["IsReserved"]);

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjResArea;
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
        public void GetContrDetForDestuffingEntry(int TallySheetId, int BranchId, String CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_DestuffingEntryDtl> lstdynamic = new List<VIZ_DestuffingEntryDtl>();
            List<dynamic> lstdynamicGodown = new List<dynamic>();
            List<dynamic> lstdynamicloc = new List<dynamic>();
            List<dynamic> lstdynamichdr = new List<dynamic>();
            List<VIZ_DestufGodownDetails> lstgodowndynamic = new List<VIZ_DestufGodownDetails>();
            string ShippingLine = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstdynamic.Add(new VIZ_DestuffingEntryDtl
                    {
                        //TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                        //BOLNo = Result["OblNo"].ToString(),
                        //BOLDate = (Result["OBLDATE"] == null ? "" : Result["OblDate"]).ToString(),
                        //LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        //CargoDescription = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                        //NoOfPackages = Convert.ToInt32(Result["NoOfPkg"]),
                        //GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        //UOM = Result["UOM"].ToString(),
                        //CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        //Commodity = Result["Commodity"].ToString(),
                        //TallySheetArea = Convert.ToDecimal(Result["AreaTallysheet"]),
                        //GodownId = Convert.ToInt32(Result["GodownId"]),
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        //CargoType = Convert.ToInt32(Result["CargoType"]),
                        // ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                        //DestuffingWeight = Convert.ToInt32(Result["DestuffingWeight"]),
                        // OBLWiseDestuffingDate = "",
                    });

                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamicloc.Add(new
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            LocationName = Result["LocationName"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ShippingLine = Convert.ToInt32(Result["ShippingLineId"]) + "," + Result["ShippingLineName"].ToString() + "," + Result["TallySheetDate"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamichdr.Add(new
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Result["CHAName"].ToString(),
                            RotationNo = Result["RotationNo"].ToString(),
                            CustomSealNo = Result["CustomSealNo"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamicGodown.Add(new
                        {
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString()
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        lstgodowndynamic.Add(new WFLD_DestufGodownDetails
                //        {

                //            TallySheetId = Convert.ToInt32(Result["TallySheetId"]),
                //            GodownId = Convert.ToInt32(Result["GodownId"]),
                //            GodownName = Convert.ToString(Result["GodownName"]),
                //            GodownWiseLocationIds = Convert.ToString(Result["LocationIds"]),
                //            GodownWiseLocationNames = Convert.ToString(Result["LocationNames"]),
                //            ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                //            DestuffingWeight = Convert.ToInt32(Result["DestuffingWeight"]),
                //            Area = Convert.ToDecimal(Result["SQM"]),
                //            AreaCbm = Convert.ToDecimal(Result["SQM"]),
                //            EntryDate = Convert.ToString(Result["EntryDate"])

                //        });
                //    }
                //}

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstdynamic, lstdynamicloc, ShippingLine, lstdynamichdr };
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
        public void GetDestuffEntryForPrint(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_DestuffingSheet ObjDestuffing = new VIZ_DestuffingSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDateTime = Result["DestuffingEntryDateTime"].ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.GateInDate = Result["GateInDate"].ToString();
                    ObjDestuffing.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjDestuffing.SlaSealNo = Result["SlaSealNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.IGMNo = Result["IGMNo"].ToString();
                    ObjDestuffing.MovementType = Result["MovementType"].ToString();
                    ObjDestuffing.ShippingLine = Result["ShippingLine"].ToString();
                    ObjDestuffing.POL = (Result["POL"] == null ? "" : Result["POL"]).ToString();
                    ObjDestuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        decimal tArea = 0;
                        decimal RevArea = 0;
                        // if (Convert.ToDecimal(Result["Area"]) > 0)
                        //{
                        tArea = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]);
                        RevArea = Convert.ToDecimal(Result["RevArea"] == DBNull.Value ? 0 : Result["RevArea"]);

                        //}
                        //else
                        //{
                        //    tArea = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]);
                        //}
                        ObjDestuffing.lstDtl.Add(new VIZ_DestuffingSheetDtl
                        {
                            SMTPNo = (Result["SMTPNo"] == null ? "" : Result["SMTPNo"]).ToString(),
                            OblNo = (Result["OblNo"] == null ? "" : Result["OblNo"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Cargo = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                            Type = (Result["Type"] == null ? "" : Result["Type"]).ToString(),
                            NoOfPkg = Convert.ToInt32(Result["NoOfPkg"] == DBNull.Value ? 0 : Result["NoOfPkg"]),
                            PkgRec = Convert.ToInt32(Result["PkgRec"] == DBNull.Value ? 0 : Result["PkgRec"]),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            RWeight = Convert.ToDecimal(Result["RWeight"] == DBNull.Value ? 0 : Result["RWeight"]),

                            Area = tArea,
                            revArea = RevArea,
                            //Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            //AreaCbm = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]),

                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                            CBM = Convert.ToDecimal(Result["Cbm"]),
                            GeneralHazardous = Convert.ToString(Result["CargoType"]),
                            LineNo = Convert.ToString(Result["LineNo"]),
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            TSANO = Convert.ToString(Result["TSANo"]),

                        });
                    }
                }
                /*if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.objCom.Name = Result["Name"].ToString();
                        ObjDestuffing.objCom.RoleName = Result["RoleName"].ToString();
                        ObjDestuffing.objCom.CompanyShortName = Result["CompanyShortName"].ToString();
                        ObjDestuffing.objCom.CompanyAddress = Result["CompanyAddress"].ToString();
                    }
                }*/
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
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
        public void GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(BOEDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            dynamic obj = new { CIFValue = "", GrossDuty = "" };
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj = new
                    {
                        CIFValue = Result["CIF_VALUE"].ToString(),
                        GrossDuty = Result["DUTY"].ToString()
                    };
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

        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<Areas.Import.Models.ShippingLineForPage> lstShippingLine = new List<Areas.Import.Models.ShippingLineForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Areas.Import.Models.ShippingLineForPage
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

        public void ListOfGodown()
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownAccessRights", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.VIZGodownList> lstGodownList = new List<Areas.Import.Models.VIZGodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.VIZGodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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

        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<Areas.Import.Models.CHA> lstCHA = new List<Areas.Import.Models.CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Areas.Import.Models.CHA
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

        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            IList<Areas.Import.Models.ShippingLine> lstShippingLine = new List<Areas.Import.Models.ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Areas.Import.Models.ShippingLine
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

        public void GetGodownLocationById(int GodownId)
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownLocationById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> lstdynamicloc = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstdynamicloc.Add(new
                    {
                        LocationId = Convert.ToInt32(Result["LocationId"]),
                        LocationName = Result["LocationName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstdynamicloc };
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
        public void ListOfGodownRights(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_UserId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownRights", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.VIZGodownList> lstGodownList = new List<Areas.Import.Models.VIZGodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.VIZGodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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
        #region Custom Appraisement Application
      
   
        public void GetContnrDetForCustomAppraise(string CFSCode, string LineNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            VIZ_CustomAppraisement ObjAppraisement = new VIZ_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    ObjAppraisement.CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString();
                    ObjAppraisement.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjAppraisement.ContainerLoadType = (Result["ContainerLoadType"] == null ? "" : Result["ContainerLoadType"]).ToString();
                    ObjAppraisement.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjAppraisement.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjAppraisement.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjAppraisement.CHAName = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjAppraisement.Vessel = (Result["VesselName"] == null ? "" : Result["VesselName"]).ToString();
                    ObjAppraisement.Voyage = (Result["VoyageNo"] == null ? "" : Result["VoyageNo"]).ToString();
                    ObjAppraisement.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjAppraisement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjAppraisement.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjAppraisement.Rotation = (Result["RotationNo"] == null ? "" : Result["RotationNo"]).ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    // ObjAppraisement.ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]);
                    ObjAppraisement.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    //  ObjAppraisement.Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);
                    // ObjAppraisement.RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]);
                    ObjAppraisement.GateEntryDate = Convert.ToDateTime(Result["EntryDateTime"]).ToString("MM/dd/yyyy");


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
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
        public void GetAllCustomAppraisementApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_CustomAppraisement> LstAppraisement = new List<VIZ_CustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new VIZ_CustomAppraisement
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
                        AppraisementDate = Result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
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
        public void GetCustomAppraisement(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
           VIZ_CustomAppraisement ObjAppraisement = new VIZ_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjAppraisement.CHAName = Result["CHA"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.IsDO = Convert.ToInt32(Result["IsDO"]);
                    ObjAppraisement.ApplicationForApp = Convert.ToInt32(Result["Application_For"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementVIZ.Add(new VIZ_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),

                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            OBLDate = (Result["OBLDate"] == null ? "" : Result["OBLDate"]).ToString(),
                            // Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            //  Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            Importer = Result["Importer"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString(),
                            ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            RMSValue = Convert.ToInt32(Result["RMS"]),

                            //  Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]),
                            //  RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]),
                            //  HeavyScrap = Convert.ToInt32(Result["HeavyScrap"] == DBNull.Value ? 0 : Result["HeavyScrap"]),
                            //  AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"] == DBNull.Value ? 0 : Result["AppraisementPerct"]),
                            //  IsInsured = Convert.ToInt32(Result["IsInsured"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstCustomAppraisementOrdDtl.Add(new VIZ_CustomAppraisementOrdDtl
                        {
                            OrderId = Convert.ToInt32(Result["OrderId"]),
                            CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            IssuedBy = (Result["DoIssueBy"] == null ? "" : Result["DoIssueBy"]).ToString(),
                            DeliveredTo = (Result["CargsDelivered"] == null ? "" : Result["CargsDelivered"]).ToString(),
                            ValidType = (Result["ValidType"] == null ? "" : Result["ValidType"]).ToString(),
                            ValidDate = (Result["ValidDate"] == null ? "" : Result["ValidDate"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
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
        public void AddEditCustomAppraisement(VIZ_CustomAppraisement ObjAppraisement, string AppraisementXML, string CAOrdXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAppraisement.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_application_For", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ApplicationForApp });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjAppraisement.AppraisementDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CAOrdXML", MySqlDbType = MySqlDbType.Text, Value = CAOrdXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCustomAppraisementApp", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjAppraisement.CustomAppraisementId == 0 ? "Custom Appraisement Application Details Saved Successfully" : "Custom Appraisement Application Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Application Details As It Already Exists In Another Page";
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
        public void DelCustomAppraisement(int CustomAppraisementId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Application Details As It Exist In Another Page";
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

        //public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
        //    IDataParameter[] Dparam = lstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
        //    IList<ShippingLineForPage> lstShippingLine = new List<ShippingLineForPage>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        bool State = false;
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstShippingLine.Add(new ShippingLineForPage
        //            {
        //                ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
        //                ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
        //                PartyCode = Result["PartyCode"].ToString()
        //            });
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                State = Convert.ToBoolean(Result["State"]);
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = new { lstShippingLine, State };
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Dispose();
        //        Result.Close();
        //    }

        //}
        public void GetContnrNoForCustomAppraise()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrNoForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VIZ_CustomAppraisementDtl> LstAppraisement = new List<VIZ_CustomAppraisementDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new VIZ_CustomAppraisementDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
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
        public void GetOBLContainer(String OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VIZ_CustomAppraisement objOBLEntry = new VIZ_CustomAppraisement();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        VIZ_CustomAppraisementOBLCont objOBLCont = new VIZ_CustomAppraisementOBLCont();
                        objOBLCont.CFSCode = Convert.ToString(dr["CFSCode"]);
                        objOBLCont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.VIZCustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {
                    //if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    //}
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.VIZCustomAppraisementCont;
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

        public void GetBOEDetail(String BOENo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_NO", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBOEDetail", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VIZ_CustomAppraisementBOECont objOBLCont = new VIZ_CustomAppraisementBOECont();
                if (Result != null && Result.Tables.Count > 0)
                {

                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        objOBLCont.BOENo = Convert.ToString(dr["BOE_NO"]);
                        objOBLCont.BOEDate = Convert.ToString(dr["BOE_Date"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIF_VALUE"]);
                        objOBLCont.Duty = Convert.ToDecimal(dr["DUTY"]);
                    }
                }
                if (Status == 1)
                {

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLCont;
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

        public void GetContainerOBL(String CFSCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetContainerOBL", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                VIZ_CustomAppraisement objOBLEntry = new VIZ_CustomAppraisement();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        VIZ_CustomAppraisementOBLCont objOBLCont = new VIZ_CustomAppraisementOBLCont();
                        objOBLCont.OBLNo = Convert.ToString(dr["OBLNo"]);
                        objOBLCont.OBLDate = Convert.ToString(dr["OBLDate"]);
                        objOBLCont.NoOfPackages = Convert.ToInt32(dr["NO_PKG"]);
                        objOBLCont.GrossWeight = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIFValue"]);
                        objOBLCont.ImporterId = Convert.ToInt32(dr["ImporterId"]);
                        objOBLCont.Importer = Convert.ToString(dr["Importer"]);
                        objOBLCont.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLCont.ShippingLine = Convert.ToString(dr["ShippingLine"]);
                        objOBLCont.CargoDescription = Convert.ToString(dr["CargoDescription"]).Replace(",", "_c_").Replace("<br/>", "_br_");
                        objOBLCont.CargoType = Convert.ToInt32(dr["CargoType"]);
                        objOBLEntry.VIZCustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {
                    //if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    //}
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.VIZCustomAppraisementCont;
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
            IList<VIZ_Menu> lstMenu = new List<VIZ_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new VIZ_Menu
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
        #endregion
    }
}