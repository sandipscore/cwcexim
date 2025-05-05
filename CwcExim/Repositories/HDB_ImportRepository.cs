using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Import.Models;
using CwcExim.DAL;
using System.Data;
using CwcExim.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Areas.GateOperation.Models;
//using CwcExim.Areas.Export.Models;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{
    public class HDB_ImportRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

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
            IList<CwcExim.Areas.Import.Models.Hdb_DestuffingEntry> lstGodownlctn = new List<CwcExim.Areas.Import.Models.Hdb_DestuffingEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownlctn.Add(new CwcExim.Areas.Import.Models.Hdb_DestuffingEntry
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

        #region Form One LCL
        public void SaveImportIgmFile(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string CargoXML, string ContainerXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIgmFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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




        public void SaveImportIALFile(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string Kdl_FormOneModelListXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            //LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = Kdl_FormOneModelListXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIALFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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

        internal void GetYardPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML, int InvoiceId, int PartyId, int CasualLabour,string ExportUnder,string Printing,int Distance)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Printing", MySqlDbType = MySqlDbType.VarChar, Value = Printing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int16, Value = Distance });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_InvoiceYard objInvoice = new Hdb_InvoiceYard();
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
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
                    objInvoice.Printing = Result["Printing"].ToString();
                }
                //}

                // if (Result.NextResult())
                //{

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
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
                        ISODC=Convert.ToInt32(Result["ISODC"]),

                    });
                    objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        ISODC=Convert.ToInt32(Result["ISODC"]),
                    });
                }
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
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
                    objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
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

        internal void GetHTChargesYard(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML, int InvoiceId, string ClauseXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ClauseXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ClauseXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_InvoiceYard objInvoice = new Hdb_InvoiceYard();
            IDataReader Result = DataAccess.ExecuteDataReader("GetHTChargesYard", CommandType.StoredProcedure, DParam);

            try
            {
                _DBResponse = new DatabaseResponse();

                //while (Result.Read())
                //{
                //    objInvoice.ROAddress = Result["ROAddress"].ToString();
                //    objInvoice.CompanyName = Result["CompanyName"].ToString();
                //    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                //    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                //    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                //    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                //    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                //    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                //    objInvoice.StateCode = Result["StateCode"].ToString();

                //    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                //    objInvoice.GstIn = Result["GstIn"].ToString();
                //    objInvoice.Pan = Result["Pan"].ToString();

                //    objInvoice.CompGST = Result["GstIn"].ToString();
                //    objInvoice.CompPAN = Result["Pan"].ToString();
                //    objInvoice.CompStateCode = Result["StateCode"].ToString();

                //}
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                //        objInvoice.CHAName = Result["CHAName"].ToString();
                //        objInvoice.PartyName = Result["CHAName"].ToString();
                //        objInvoice.PartyGST = Result["GSTNo"].ToString();
                //        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                //        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                //        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                //        objInvoice.PartyAddress = Result["Address"].ToString();
                //        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                //    }
                //}

                //if (Result.NextResult())
                //{

                //    while (Result.Read())
                //    {
                //        objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
                //        {
                //            ContainerNo = Result["ContainerNo"].ToString(),
                //            CFSCode = Result["CFSCode"].ToString(),
                //            ArrivalDate = Result["ArrivalDate"].ToString(),
                //            ArrivalTime = Result["ArrivalTime"].ToString(),
                //            CargoType = Convert.ToInt32(Result["CargoType"]),
                //            BOENo = Result["BOENo"].ToString(),
                //            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                //            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                //            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                //            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                //            Duty = Convert.ToDecimal(Result["Duty"]),
                //            Reefer = Convert.ToInt32(Result["Reefer"]),
                //            RMS = Convert.ToInt32(Result["RMS"]),
                //            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                //            Insured = Convert.ToInt32(Result["Insured"]),
                //            Size = Result["Size"].ToString(),
                //            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                //            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                //            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                //            LCLFCL = Result["LCLFCL"].ToString(),
                //            CartingDate = Result["CartingDate"].ToString(),
                //            DestuffingDate = Result["DestuffingDate"].ToString(),
                //            StuffingDate = Result["StuffingDate"].ToString(),
                //            ApproveOn = Result["ApproveOn"].ToString(),
                //            BOEDate = Result["BOEDate"].ToString(),
                //            CHAName = Result["CHAName"].ToString(),
                //            ImporterExporter = Result["ImporterExporter"].ToString(),
                //            LineNo = Result["LineNo"].ToString(),
                //            OperationType = Convert.ToInt32(Result["OperationType"]),
                //            ShippingLineName = Result["ShippingLineName"].ToString(),
                //            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                //        });
                //        objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
                //        {
                //            ContainerNo = Result["ContainerNo"].ToString(),
                //            CFSCode = Result["CFSCode"].ToString(),
                //            ArrivalDate = Result["ArrivalDate"].ToString(),
                //            ArrivalTime = Result["ArrivalTime"].ToString(),
                //            CargoType = Convert.ToInt32(Result["CargoType"]),
                //            BOENo = Result["BOENo"].ToString(),
                //            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                //            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                //            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                //            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                //            Duty = Convert.ToDecimal(Result["Duty"]),
                //            Reefer = Convert.ToInt32(Result["Reefer"]),
                //            RMS = Convert.ToInt32(Result["RMS"]),
                //            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                //            Insured = Convert.ToInt32(Result["Insured"]),
                //            Size = Result["Size"].ToString(),
                //            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                //            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                //            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                //            LCLFCL = Result["LCLFCL"].ToString(),
                //            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                //            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                //            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                //        });
                //    }
                //}
                //if (Result.NextResult())
                //{
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
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


                //}

                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
                //        {
                //            CFSCode = Result["CFSCode"].ToString(),
                //            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                //            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                //            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                //            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                //            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                //            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                //            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                //            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                //            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                //            ContainerId = 0,
                //            InvoiceId = 0,
                //            LineNo = ""
                //        });
                //    }
                //}

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
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

        public void AddEditContPaymentSheet(Hdb_InvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Printing", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Printing });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(String.IsNullOrEmpty(ObjPostPaymentSheet.Distance)) });

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
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = ReturnObj;
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
        public void ListOfExpInvoice(string Module,string ModuleNext, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Modulenext", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ModuleNext });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoiceforEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice> lstExpInvoice = new List<CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice()
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
        public void AddEditEmptyContPaymentSheet(Hdb_InvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(String.IsNullOrEmpty(ObjPostPaymentSheet.Distance)?"0": ObjPostPaymentSheet.Distance) });

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
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = ReturnObj;
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
            IList<PPG_Menu> lstMenu = new List<PPG_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new PPG_Menu
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

        public void AddEditFormOne(Hdb_FormOneLclModel objFormOne, int BranchId, string FormOneDetailXML, int CreatedBy)
        {
            DateTime dt = DateTime.ParseExact(objFormOne.FormOneDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string convertFromOneDate = dt.ToString("yyyy-MM-dd");
           
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormoneDate", MySqlDbType = MySqlDbType.VarChar, Value = convertFromOneDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VoyageNo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischargeId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.PortOfDischargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortCharge", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortChargeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortChargeAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContCBT", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.CBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IceGateContSize", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.IcegateContSize });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IceGateContNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.IceGateContNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTForm", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.CBTFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.GateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });

            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditFormOneLcl", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
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
        public void DeleteFormOne(int FormOneId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteFormOne", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Form-1 Details As It Already Exists In Another Page";
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

        public void GetICEGateData(string containerNo, string size)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = containerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = size });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetICEGateData", CommandType.StoredProcedure, DParam);
            List<Hdb_FormoneLclDetailModel> formonelist = new List<Hdb_FormoneLclDetailModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    formonelist.Add(new Hdb_FormoneLclDetailModel
                    {
                        ContainerNo = Convert.ToString(result["CONTAINER_NO"]),
                        ContainerSize = Convert.ToString(result["CONT_SIZE"]),
                        Noofpkg = Convert.ToInt32(result["NO_PKG"]),
                        PackageType = Convert.ToString(result["PKG_TYPE"]),
                        SealNo = Convert.ToString(result["CONT_SEAL_NO"].ToString()),
                        GrossWeight = Convert.ToDecimal(result["GR_WT"].ToString()),
                        LineNo = Convert.ToString(result["LINE_NO"]),
                        LCLFCL = Convert.ToString(result["CONT_STATUS"]),
                        OBLNO = Convert.ToString(result["OBL_NO"]),
                        OBLDate=Convert.ToString(result["OBL_DATE"]),
                        IGMNo=Convert.ToString(result["IGM_NO"]),
                        IGMDate = Convert.ToString(result["IGMDate"]),
                        CargoDesc = Convert.ToString(result["CARGO_DESC"]),
                        MarksNo = Convert.ToString(result["MARKS_OF_NUMBER"]),
                        TPNo=Convert.ToString(result["TPNo"]),
                        TPDate=Convert.ToString(result["TPDate"]),
                        PortofLoading = Convert.ToInt32(result["PortId"]),
                        IgmImporter=Convert.ToString(result["IgmImp"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = formonelist;
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

        public void ListOfShippingLines()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<Hdb_FormOneDetailModel> lstShippingLine = new List<Hdb_FormOneDetailModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Hdb_FormOneDetailModel
                    {
                        ShippingLineId_dtl = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineNameDet = result["NameAddress"].ToString()
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
        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
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
        public void ListOfCHA()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<CHA> lstCHA = new List<CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHA
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


        public void ListOfCHAName()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTraderCHA", CommandType.StoredProcedure, DParam);
            IList<CHA> lstCHA = new List<CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHA
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
        public void ListOfImporter()
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

        public void ListOfForwarder()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Forwarder" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<TSAForwarder> lstForwarder = new List<TSAForwarder>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstForwarder.Add(new TSAForwarder
                    {
                        TSAForwarderId = Convert.ToInt32(result["EximTraderId"]),
                        TSAForwarderName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstForwarder;
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
        public void ListOfPOD()
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



        public void GetAutoPopulateData(string ContainerNumber, int BranchId)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetailsWithoutReference", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGate ObjEntryThroughGate = new HDB_EntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.LCLFCL = Result["LCLFCL"].ToString();
                    //
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    //
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    ObjEntryThroughGate.CBT = Convert.ToBoolean(Result["CBT"]);
                    ObjEntryThroughGate.IsODC = Convert.ToBoolean(Result["IsODC"]);
                    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoofPackages"] == DBNull.Value ? 0 : Result["NoofPackages"]);
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    string strDate = ObjEntryThroughGate.ReferenceDate;
                    string[] arrayDate = strDate.Split(' ');
                    ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;
                    ObjEntryThroughGate.Remarks = Convert.ToString(Result["Remarks"]);
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
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



        public void GetContainer(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerWithoutReference", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new ContainerRefGateEntry
                    {
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        ContainerNo = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
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
        public void ListOfCommodity()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            IList<Areas.Import.Models.Commodity> lstCommodity = new List<Areas.Import.Models.Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new Areas.Import.Models.Commodity
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
        public void GetFormOne(int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "LCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            IList<Hdb_FormOneLclModel> lstFormOne = new List<Hdb_FormOneLclModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Hdb_FormOneLclModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"])
                        //,
                        //TrBondNo= Convert.ToString(result["TrBondNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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


        public void GetFormOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormBYContainer", CommandType.StoredProcedure, DParam);
            IList<Hdb_FormOneLclModel> lstFormOne = new List<Hdb_FormOneLclModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Hdb_FormOneLclModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetFormOneById(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            Hdb_FormOneLclModel objFormOne = new Hdb_FormOneLclModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.IceGateContNo = Convert.ToString(result["IceGateContNo"]);
                    objFormOne.IcegateContSize = Convert.ToString(result["IceGateContSize"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.BLNo = Convert.ToString(result["BLNo"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    //objFormOne.VesselName = Convert.ToString(result["VesselName"]);
                    //objFormOne.VoyageNo = Convert.ToString(result["VoyageNo"]);
                    //objFormOne.RotationNo = Convert.ToString(result["RotationNo"]);
                    objFormOne.PortOfDischargeId = Convert.ToInt32(result["PortOfDischargeId"]);
                    objFormOne.PortName = Convert.ToString(result["PortName"]);
                    objFormOne.PortCharge = Convert.ToDecimal(result["PortCharge"]);
                    objFormOne.PortChargeAmt = Convert.ToDecimal(result["PortChargeAmount"]);
                    objFormOne.CargoType = Convert.ToInt32(result["CargoType"]);
                    objFormOne.CBT = Convert.ToString(result["Container_CBT"]);
                    objFormOne.CBTFrom = Convert.ToString(result["CbtForm"]);

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetails.Add(new Hdb_FormoneLclDetailModel()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            Reefer = Convert.ToInt32(result["Reefer"]),
                            FlatReck = Convert.ToInt32(result["FlatReck"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            MarksNo = Convert.ToString(result["MarksNo"]),
                            CHAId = Convert.ToInt32(result["CHAId"]),
                            CHAName = Convert.ToString(result["CHAName"]),
                            ImporterId = Convert.ToInt32(result["ImporterId"]),
                            ImporterName = Convert.ToString(result["ImporterName"]),
                            ForwarderId = Convert.ToInt32(result["ForwarderId"]),
                            ForwarderName = Convert.ToString(result["ForwarderName"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CommodityName = Convert.ToString(result["CommodityName"]),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            Remarks = Convert.ToString(result["Remarks"]),
                            LCLFCL = Convert.ToString(result["LCLFCL"]),
                            VehicleNo = Convert.ToString(result["VehicleNo"]),
                            Noofpkg = Convert.ToInt32(result["Noofpkg"]),
                            PackageType = Convert.ToString(result["PackageType"]),
                            Others = Convert.ToString(result["Others"]),
                            GrossWeight = Convert.ToDecimal(result["GrossWeight"].ToString()),
                            OBLNO = Convert.ToString(result["OBLNO"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            GrossDuty = Convert.ToDecimal(result["GrossDuty"]),
                            IsODC = Convert.ToInt32(result["IsODC"]),
                            OBLDate = Convert.ToString(result["OBLDate"]),
                            ShippingLineId_dtl = Convert.ToInt32(result["ShippingLineId"]),
                            ShippingLineNameDet = Convert.ToString(result["ShippingLineName"]),
                            BLNo= Convert.ToString(result["BLNo"]),
                            BLDate = Convert.ToString(result["BLDate"]),
                            TSANo = Convert.ToString(result["TSANo"]),
                            TSADate = Convert.ToString(result["TSADate"]),
                            IGMNo = Convert.ToString(result["IGMNo"]),
                            IGMDate = Convert.ToString(result["IGMDate"]),
                            VesselName = Convert.ToString(result["VesselName"]),
                            VoyageNo = Convert.ToString(result["VoyageNo"]),
                            RotationNo = Convert.ToString(result["RotationNo"]),
                            TPNo=Convert.ToString(result["TPNo"]),
                            TPDate=Convert.ToString(result["TPDate"]),
                            IgmImporter=Convert.ToString(result["IgmImporter"]),
                            IceGateContNo = Convert.ToString(result["IceGateContNo"]),
                            IceGateContSize = Convert.ToString(result["IceGateContSize"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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
        public void FormOnePrint(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("FormOnePrint", CommandType.StoredProcedure, DParam);
            Hdb_FormOnePrintModel objFormOne = new Hdb_FormOnePrintModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.ShippingLineNo = Convert.ToString(result["ShippingLine"]);
                    objFormOne.FormOneNo = Convert.ToString(result["CustomSlNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.CHAName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CHAAddress = Convert.ToString(result["Address"]);
                    objFormOne.CHAPhoneNo = Convert.ToString(result["PhoneNo"]);
                    objFormOne.ShippingLineNo = result["ShippingLine"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOnePrintDetail.Add(new Hdb_FormOnePrintDetailModel()
                        {
                            VesselName = Convert.ToString(result["VesselName"]),
                            VoyageNo = Convert.ToString(result["VoyageNo"]),
                            RotationNo = Convert.ToString(result["RotationNo"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            ImpName = Convert.ToString(result["EximTraderName"]),
                            ImpAddress = Convert.ToString(result["Address"]),
                            ImpName2 = Convert.ToString(result["ImporterParty2"]),
                            ImpAddress2 = Convert.ToString(result["ImporterPartyAddress2"]),
                            Type = Convert.ToString(result["TypeLoadEmpty"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            HazType = Convert.ToInt32(result["HazType"]) == 2 ? "NON-HAZ" : "HAZ",
                            ReferType = Convert.ToInt32(result["ReferType"]) == 0 ? "" : "/ REEFER",
                            FormOneDate = Convert.ToString(result["FormOneDate"]),
                            ShippingLineName = Convert.ToString(result["ShippingLineName"]),
                            ForwarderName= Convert.ToString(result["Forwarder"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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

        #region Import Job Order

        public void GetForm1Details(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetForm1GetData", CommandType.StoredProcedure, DParam);
            List<Hdb_ImportJobOrderDtl> joborderlist = new List<Hdb_ImportJobOrderDtl>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    joborderlist.Add(new Hdb_ImportJobOrderDtl
                    {
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        //CFSCode = Convert.ToString(result["CfsCode"]),
                        Size = Convert.ToString(result["ContainerSize"]),
                        CustomSealNo = Convert.ToString(result["SealNo"]),
                        CargoType = Convert.ToInt32(result["CargoType"]),
                        ShippingLineNo = Convert.ToString(result["LineNo"]),
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        ShippingLineName = result["ShippingLineName"].ToString(),
                        CHAId = Convert.ToInt32(result["CHAId"]),
                        CHAName = result["CHAName"].ToString(),
                        //ShippingLineName = Convert.ToString(result["ShippingLineName"]),
                        //ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                        //ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                        //CHAName = Convert.ToString(result["CHAName"]),
                        DisplayCargoType = Convert.ToString(result["DisplayCargoType"]),
                        Commodity = Convert.ToString(result["Commodity"]),
                        CommodityId = Convert.ToInt32(result["CommodityId"]),
                        ContainerLoadType = Convert.ToString(result["ContainerLoadType"]),
                        NoofPackages = Convert.ToInt32(result["NoofPackages"]),
                        GrossWeight = Convert.ToDecimal(result["GrossWeight"]),
                        SealNo = Convert.ToString(result["SealNo"]),
                        CargoDescription = Convert.ToString(result["CargoDesc"]),
                        IsODC = Convert.ToInt32(result["IsODC"]),
                        LineNo = Convert.ToString(result["LineNo"]),
                        OBLNO = Convert.ToString(result["OBLNO"]),
                        PackageType = Convert.ToString(result["PackageType"]),
                        Others = Convert.ToString(result["Others"]),
                        FromLocation = Convert.ToString(result["FromLocation"]),
                        ToLocation = Convert.ToString(result["ToLocation"] == null ? "" : result["ToLocation"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = joborderlist;
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
        public void GetAllBlNofromFormOne(string Container_CBT)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Value = Container_CBT });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBlnoFromform1", CommandType.StoredProcedure, dpram);
            IList<Hdb_BlNoList> lstBlNo = new List<Hdb_BlNoList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstBlNo.Add(new Hdb_BlNoList { BlNo = result["BLNo"].ToString(), FormOneId = Convert.ToInt32(result["FormOneId"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstBlNo;
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
        public void GetBlNoDtl(int FormOneId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = FormOneId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBlnoFromform1", CommandType.StoredProcedure, dpram);
            Hdb_FormOneForImpJO objFormone = new Hdb_FormOneForImpJO();
            IList<Hdb_FormOneDtlForImpJO> lstDtl = new List<Hdb_FormOneDtlForImpJO>();
            IList<HDBYard> lstYard = new List<HDBYard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    // objFormone.FormOneNo = result["FormOneNo"].ToString();
                    objFormone.FormOneDate = result["FormOneDate"].ToString();
                    objFormone.ShippingLineId = Convert.ToInt32(result["ShippingLineId"] == DBNull.Value ? 0 : result["ShippingLineId"]);
                    objFormone.ShippingLineName = (result["ShippingLineName"] == null ? "" : result["ShippingLineName"]).ToString();
                    objFormone.CHAId = Convert.ToInt32(result["CHAId"]);
                    objFormone.CHAName = (result["CHAName"]).ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new Hdb_FormOneDtlForImpJO
                        {
                            FormOneDetailId = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            Reefer = (Convert.ToInt32(result["Reefer"]) == 1 ? "Reefer" : "Non Reefer").ToString(),
                            SealNo = (result["SealNo"] == null ? "" : result["SealNo"]).ToString()
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstYard.Add(new HDBYard { YardId = Convert.ToInt32(result["YardId"]), YardName = result["YardName"].ToString() });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { FormOne = objFormone, FormOneDtl = lstDtl, YardList = lstYard };
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
        public void GetAllImpJO(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            IList<Hdb_ImportJobOrderList> lstImpJO = new List<Hdb_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new Hdb_ImportJobOrderList
                    {
                        JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                        JobOrderFor = Convert.ToInt32(result["JobOrderFor"]),
                        Purpose = result["Purpose"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
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

        public void GetJobOrderOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetIJobOrderByContainer", CommandType.StoredProcedure, DParam);
            IList<Hdb_ImportJobOrderList> lstImpJO = new List<Hdb_ImportJobOrderList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new Hdb_ImportJobOrderList
                    {
                        JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                        JobOrderFor = Convert.ToInt32(result["JobOrderFor"]),
                        Purpose = result["Purpose"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
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
        public void GetImpJODetails(int JobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = JobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            Hdb_ImportJobOrder objImpJO = new Hdb_ImportJobOrder();
            IList<Hdb_ImportJobOrderDtl> lstDtl = new List<Hdb_ImportJobOrderDtl>();
            IList<Hdb_ImportClauseDtl> lstCDtl = new List<Hdb_ImportClauseDtl>();

            IList<HDBYard> lstYard = new List<HDBYard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objImpJO.ImpJobOrderId = Convert.ToInt32(result["JobOrderId"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    objImpJO.JobOrderDate = result["JobOrderDate"].ToString();
                    objImpJO.FormOneNo = result["FormOneNo"].ToString();
                    objImpJO.FormOneDate = result["FormOneDate"].ToString();
                    //  objImpJO.OperationId = Convert.ToInt32(result["Clause"].ToString());
                    objImpJO.RoundTrip = Convert.ToBoolean(result["RoundTrip"]);
                    objImpJO.JobOrderFor = Convert.ToInt32(result["JobOrderFor"].ToString());
                    objImpJO.FromContainer = Convert.ToInt32(result["ContComeFrom"].ToString());
                    objImpJO.PortName = Convert.ToString(result["PortName"].ToString());
                    objImpJO.PortId = Convert.ToInt32(result["PortId"]);
                    objImpJO.Distance = Convert.ToDecimal(result["Distance"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new Hdb_ImportJobOrderDtl
                        {
                            JobOrderDetailId = Convert.ToInt32(result["JobOrderDtlId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            CustomSealNo = (result["CustomSealNo"] == null ? "" : result["CustomSealNo"]).ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CHAId = Convert.ToInt32(result["CHAId"]),
                            CHAName = result["CHAName"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            FromLocation = result["FromLocation"].ToString(),
                            ToLocation = result["ToLocation"].ToString(),
                            ToYardId = Convert.ToInt32(result["YardId"]),
                            YardName = result["YardName"].ToString(),
                            YardWiseLocationIds = Convert.ToString(result["YardWiseLocationIds"]),
                            YardWiseLctnNames = result["YardWiseLctnNames"].ToString(),
                            NoofPackages = Convert.ToInt32(result["Noofpackages"]),
                            GrossWeight = Convert.ToDecimal(result["GrossWeight"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            Commodity = result["Commodity"].ToString(),
                            DisplayCargoType = Convert.ToString(result["DisplayCargoType"]),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            IsODC = Convert.ToInt32(result["IsODC"]),
                            LineNo = result["LineNo"].ToString(),
                            OBLNO = result["OBLNO"].ToString(),
                            PackageType = result["PackageType"].ToString(),
                            Others = result["Others"].ToString(),
                            OperationName = result["OperationName"].ToString(),
                            OtherOperation = result["OtherOperation"].ToString(),
                            GodownId = Convert.ToInt32((result["GodownId"] == DBNull.Value ? 0 : result["GodownId"])),
                            GodownName = (result["GodownName"] == null ? "" : result["GodownName"]).ToString(),
                            GodownWiseLocationIds = (result["GodownWiseLocationIds"] == null ? "" : result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (result["GodownWiseLctnNames"] == null ? "" : result["GodownWiseLctnNames"]).ToString(),
                            Purpose = (result["Purpose"] == null ? "" : result["Purpose"]).ToString(),

                        });
                    }
                }
                if (lstDtl.Count > 0)
                {
                    objImpJO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstDtl);
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstCDtl.Add(new Hdb_ImportClauseDtl
                        {
                            OperationId = Convert.ToInt32(result["Clause"]),
                            OperationCode = result["OperationCode"].ToString(),
                        });
                    }
                }
                if (lstCDtl.Count > 0)
                {
                    objImpJO.StringifyClauseXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstCDtl);
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
            int result = DA.ExecuteNonQuery("DeleteImpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Import Job Order Deleted Successfully";
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
        public void AddEditImpJO(Hdb_ImportJobOrder objJO, string XML = null, string LocationXML = null, string ClauseXML = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objJO.ImpJobOrderId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortName", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PortName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Size = 6, Value = Convert.ToInt32(objJO.Distance) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundTrip", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objJO.RoundTrip) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderfor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objJO.JobOrderFor) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneNo", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.FormOneNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromContainer", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objJO.FromContainer) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderNo", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = Convert.ToString(objJO.JobOrderNo) });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objJO.OperationId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.FormOneDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CXml", MySqlDbType = MySqlDbType.Text, Value = ClauseXML });

            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditImpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Import Job Order Saved Successfully" : "Import Job Order Saved Successfully";
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
        public void GetYardWiseLocation(int YardId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = YardId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetYardWiseLocation", CommandType.StoredProcedure, dpram);
            IList<CwcExim.Areas.Master.Models.HDBYardWiseLocation> lstDtl = new List<CwcExim.Areas.Master.Models.HDBYardWiseLocation>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstDtl.Add(new CwcExim.Areas.Master.Models.HDBYardWiseLocation
                    {
                        LocationId = Convert.ToInt32(result["LocationId"]),
                        LocationName = result["Location"].ToString(),
                        //IsOccupied = Convert.ToBoolean(result["IsOccupied"])
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

        #region Import Job Order Print
        public void GetImportJODetailsFrPrint(int ImpJobOrderId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetDetForPrntimpjo", CommandType.StoredProcedure, Dpram);
            List<PrintJobOrder> LstJobOrder = new List<PrintJobOrder>();
           // List<OperationList> LstOperationName = new List<OperationList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                //while (Result.Read())
                //{
                //    LstOperationName.Add(new OperationList
                //    {
                //        OperationName = (Result["variable"] == null ? "" : Result["variable"]).ToString()
                //    });
                //}
                //if (Result.NextResult())
                //{
                    while (Result.Read())
                    {
                        Status = 1;
                    LstJobOrder.Add(new PrintJobOrder
                    {
                        PartyName = Result["PartyName"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        NatureOfOperation = Result["NatureOfOperation"].ToString(),
                        Location = Result["Location"].ToString(),
                        Weight = Result["Weight"].ToString(),
                    });
                    }
                

               
                /*if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstJobOrder.Add(new PrintJobOrder
                        {
                            NoOfUnits = Result["NoOfUnits"].ToString(),
                            Location = Result["Location"].ToString(),
                            PartyName = Result["PartyName"].ToString(),
                            NatureOfOperation = Result["NatureOfOperation"].ToString(),
                            BOENo = Result["BOENo"].ToString()
                        });
                    }                   
                } */
                if (Status == 1)
                {
                    _DBResponse.Data = new { LstJobOrder };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "No Data Since Operation For Job Order Has Not Been Selected";
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
        #endregion

        #endregion

        #region Destuffing Entry LCL

        public void AddEditDestuffingEntry_Hdb(Hdb_DestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWODtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DeStuffingWODtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DODate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.ArrivalDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Vessel });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Voyage });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "LCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownXML", MySqlDbType = MySqlDbType.Text, Value = GodownXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLcoationXML });
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
        public void GetDestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    //ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    //ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    //ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            //DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString()
                            //IsInsured = Convert.ToInt32(Result["IsInsured"])
                            //RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            //RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            //RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            //RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        public void GetAllDestuffingEntry(int BranchId,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "LCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntryList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                        FormOneNo = (Result["FormOneNo"] == null ? "" : Result["FormOneNo"]).ToString(),
                        ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
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
        public void GetContrNoForDestuffingEntry(string type, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "LCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DestuffingEntry> LstDestuffing = new List<Hdb_DestuffingEntry>();
            try
            {
                while (Result.Read())
                {
                    var flag = 0;
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntry
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        Vessel = Result["VesselName"].ToString(),
                        Voyage = Result["VoyageNo"].ToString(),
                        CBT = Result["Container_CBT"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"].ToString()),
                        ShippingLine = Convert.ToString(Result["ShippingLineName"].ToString()),
                        Size = Convert.ToString(Result["ContainerSize"].ToString()),
                        Rotation = Convert.ToString(Result["RotationNo"].ToString()),
                        ArrivalDate = Convert.ToString(Result["ArrivalDate"].ToString()),
                        CustomSealNo = Convert.ToString(Result["CustomSealNo"].ToString()),
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

        public void GetContrDetForDestuffingEntry_Hdb(string CFSCode, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();


            List<Hdb_DestuffingEntry> ObjDestuffingLst = new List<Hdb_DestuffingEntry>();

            try
            {
                while (Result.Read())
                {
                    Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
                    Status = 1;
                    Status = 1;
                    //ObjDestuffing.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    // ObjDestuffing.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    // ObjDestuffing.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjDestuffing.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDestuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjDestuffing.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDestuffing.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjDestuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    //ObjDestuffing.DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]);
                    ObjDestuffing.CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]);
                    ObjDestuffing.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjDestuffing.LocationDetails = (Result["LocationDetails"] == null ? "" : Result["LocationDetails"]).ToString();
                    ObjDestuffing.GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString();
                    //ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString();
                    ObjDestuffing.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    //ObjDestuffing.CargoType = Result["CargoType"].ToString();
                    //ObjDestuffing.StorageType = (Result["StorageType"] == null ? "" : Result["StorageType"]).ToString();
                    ObjDestuffing.CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]);
                    ObjDestuffing.Commodity = Result["Commodity"].ToString();
                    // ObjDestuffing.AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]);
                    ObjDestuffing.BOLNo = Result["BLNo"].ToString();
                    //  ObjDestuffing.IsInsured= Convert.ToBoolean(Result["IsInsured"] == DBNull.Value ? 0 : Result
                    ObjDestuffing.ForwarderName = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString();
                    ObjDestuffing.BOLDate = (Result["BLDate"] == null ? "" : Result["BLDate"]).ToString();
                    ObjDestuffing.TSANo = (Result["TSANo"] == null ? "" : Result["TSANo"]).ToString();
                    ObjDestuffing.TSADate = (Result["TSADate"] == null ? "" : Result["TSADate"]).ToString();
                    ObjDestuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"] == DBNull.Value ? 0 : Result["ForwarderId"]);
                    ObjDestuffingLst.Add(ObjDestuffing);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffingLst;
                }
                //else if (Status == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = ObjDestuffingDtl;
                //}
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

        public void GetDestuffEntryForPrint_Hdb(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    // ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.CBT = Result["ContType"].ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    // ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    // ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    // ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Cargo = Convert.ToString(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            // Storage = Convert.ToString(Result["StorageType"] == DBNull.Value ? 0 : Result["StorageType"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"])
                        });
                    }
                }


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
        public void GetHdb_DestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "LCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    //ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    //ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    //ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.CBT = (Result["Container_CBT"] == null ? "" : Result["Container_CBT"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            //DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                            StorageType = (Result["StorageType"] == null ? "" : Result["StorageType"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            TSANo = (Result["TSANo"] == null ? "" : Result["TSANo"]).ToString(),
                            TSADate = (Result["TSADate"] == null ? "" : Result["TSADate"]).ToString(),
                            ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString(),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"]),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            Remarksobl = (Result["Remarksobl"] == null ? "" : Result["Remarksobl"]).ToString()
                            //RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            //RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            //RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            //RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        #endregion

        #region Destuffing Entry FCL

        public void AddEditDestuffingEntry_HdbFCL(Hdb_DestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWODtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DeStuffingWODtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DODate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.ArrivalDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "FCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownXML", MySqlDbType = MySqlDbType.Text, Value = GodownXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLcoationXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpDestuffingEntryFCL", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
        public void DelDestuffingEntryFCL(int DestuffingEntryId, int BranchId)
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
        public void GetDestuffingEntryFCL(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntryFCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            //DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            //IsInsured = Convert.ToInt32(Result["IsInsured"])
                            //RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            //RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            //RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            //RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        public void GetAllDestuffingEntryFCL(int BranchId,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "FCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntryList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                        Size = Result["Size"].ToString(),
                        ArrivalDate = Result["ArrivalDate"].ToString(),
                        FormOneNo = Result["FormOneNo"].ToString(),
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



        public void GetDestuffingFCLByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetImpDestuffingEntryforcontainer", CommandType.StoredProcedure, DParam);
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntryList
                    {
                        DestuffingEntryNo = result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = result["DestuffingEntryDate"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(result["DestuffingEntryId"]),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        ShippingLine = (result["ShippingLine"] == null ? "" : result["ShippingLine"]).ToString(),
                        LCLFCL = (result["LCLFCL"] == null ? "" : result["LCLFCL"]).ToString(),
                        Size = result["Size"].ToString(),
                        ArrivalDate = result["ArrivalDate"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstDestuffing;
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


        public void GetDestuffingLCLByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetImpDestuffingEntryLCLforcontainer", CommandType.StoredProcedure, DParam);
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntryList
                    {
                        DestuffingEntryNo = result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = result["DestuffingEntryDate"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(result["DestuffingEntryId"]),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        ShippingLine = (result["ShippingLine"] == null ? "" : result["ShippingLine"]).ToString(),
                        LCLFCL = (result["LCLFCL"] == null ? "" : result["LCLFCL"]).ToString(),
                        Size = result["Size"].ToString(),
                        ArrivalDate = result["ArrivalDate"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstDestuffing;
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

        public void GetContrNoForDestuffingEntryFCL(string type, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container_CBT", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntryFCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DestuffingEntry> LstDestuffing = new List<Hdb_DestuffingEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Hdb_DestuffingEntry
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        Vessel = Result["VesselName"].ToString(),
                        Voyage = Result["VoyageNo"].ToString(),
                        CBT = Result["Container_CBT"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"].ToString()),
                        ShippingLine = Convert.ToString(Result["ShippingLineName"].ToString()),
                        Size = Convert.ToString(Result["ContainerSize"].ToString()),
                        Rotation = Convert.ToString(Result["RotationNo"].ToString()),
                        ArrivalDate = Convert.ToString(Result["ArrivalDate"].ToString()),
                        CustomSealNo = Convert.ToString(Result["CustomSealNo"].ToString()),
                     
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

        public void GetContrDetForDestuffingEntry_HdbFCL(string CFSCode, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();


            List<Hdb_DestuffingEntry> ObjDestuffingLst = new List<Hdb_DestuffingEntry>();

            try
            {
                while (Result.Read())
                {
                    Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
                    Status = 1;
                    Status = 1;
                    //ObjDestuffing.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    // ObjDestuffing.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    // ObjDestuffing.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjDestuffing.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDestuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjDestuffing.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDestuffing.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjDestuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    //ObjDestuffing.DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]);
                    ObjDestuffing.CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]);
                    ObjDestuffing.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    //ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString();
                    ObjDestuffing.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    //ObjDestuffing.CargoType = Result["CargoType"].ToString();
                    //ObjDestuffing.StorageType = (Result["StorageType"] == null ? "" : Result["StorageType"]).ToString();
                    ObjDestuffing.CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]);
                    ObjDestuffing.Commodity = Result["Commodity"].ToString();
                    // ObjDestuffing.AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]);
                    ObjDestuffing.BOLNo = Result["BLNo"].ToString();
                    ObjDestuffing.BOLDate = (Result["BLDate"] == null ? "" : Result["BLDate"]).ToString();
                    //  ObjDestuffing.IsInsured= Convert.ToBoolean(Result["IsInsured"] == DBNull.Value ? 0 : Result
                    ObjDestuffing.ForwarderName = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString();
                    ObjDestuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"] == DBNull.Value ? 0 : Result["ForwarderId"]);
                    ObjDestuffingLst.Add(ObjDestuffing);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffingLst;
                }
                //else if (Status == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = ObjDestuffingDtl;
                //}
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

        public void GetDestuffEntryForPrint_HdbFCL(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    // ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    // ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    // ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    // ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Cargo = Convert.ToString(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            // Storage = Convert.ToString(Result["StorageType"] == DBNull.Value ? 0 : Result["StorageType"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"])
                        });
                    }
                }


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
        public void GetHdb_DestuffingEntryFCL(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntryFCL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.CBT = (Result["Container_CBT"] == null ? "" : Result["Container_CBT"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new Hdb_DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            //DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                            StorageType = (Result["StorageType"] == null ? "" : Result["StorageType"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            TSANo = (Result["TSANo"] == null ? "" : Result["TSANo"]).ToString(),
                            TSADate = (Result["TSADate"] == null ? "" : Result["TSADate"]).ToString(),
                            ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString(),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"])
                            //RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            //RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            //RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            //RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        #endregion

        #region Custom Appraisement Application
        public void AddEditCustomAppraisement(Hdb_CustomAppraisement ObjAppraisement, string AppraisementXML)
        {
            string GeneratedClientId = "0";

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAppraisement.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjAppraisement.AppraisementDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.CHAId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
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
        public void GetCustomAppraisement(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CustomAppraisement ObjAppraisement = new Hdb_CustomAppraisement();
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
                    //ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    //ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.IsDO = Convert.ToInt32(Result["IsDO"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisement.Add(new Hdb_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            IGMNo = (Result["IGMNo"] == null ? "" : Result["IGMNo"]).ToString(),
                            TSANo = (Result["TSANo"] == null ? "" : Result["TSANo"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
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
                            Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"] == DBNull.Value ? 0 : Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"] == DBNull.Value ? 0 : Result["AppraisementPerct"]),
                            IsInsured = Convert.ToInt32(Result["IsInsured"]),
                            StorageType = Convert.ToString(Result["StorageType"] == DBNull.Value ? "" : Result["StorageType"]),
                            AreaInSqm = Convert.ToDecimal(Result["AreaSqm"] == DBNull.Value ? 0.00 : Result["AreaSqm"]),
                            TransportMode=Convert.ToString(Result["TransportMode"]),
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
        public void GetAllCustomAppraisementApp(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CustomAppraisement> LstAppraisement = new List<Hdb_CustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new Hdb_CustomAppraisement
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
                        AppraisementDate = Result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        FormOneNo = Result["FormOneNo"].ToString(),
                        BL = Result["BL"].ToString(),
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



        public void GetApprisementFCLByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetImpApprisementforcontainer", CommandType.StoredProcedure, DParam);
            List<Hdb_CustomAppraisement> LstAppraisement = new List<Hdb_CustomAppraisement>();

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new Hdb_CustomAppraisement
                    {
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        AppraisementDate = result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(result["CustomAppraisementId"]),
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                        BL = result["BL"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstAppraisement;
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
            List<Hdb_CustomAppraisementDtl> LstAppraisement = new List<Hdb_CustomAppraisementDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new Hdb_CustomAppraisementDtl
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
        public void GetContnrDetForCustomAppraise(string CFSCode, string LineNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CustomAppraisementDtl ObjAppraisement = new Hdb_CustomAppraisementDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    ObjAppraisement.CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString();
                    ObjAppraisement.IGMNo = (Result["IGMNo"] == null ? "" : Result["IGMNo"]).ToString();
                    ObjAppraisement.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjAppraisement.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    // ObjAppraisement.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjAppraisement.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjAppraisement.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjAppraisement.CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjAppraisement.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjAppraisement.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjAppraisement.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjAppraisement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjAppraisement.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjAppraisement.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjAppraisement.ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]);
                    ObjAppraisement.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjAppraisement.Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);
                    // ObjAppraisement.RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]);
                    ObjAppraisement.CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]);
                    ObjAppraisement.Duty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);
                    ObjAppraisement.EntryDateTime = Result["EntryDateTime"].ToString();
                    ObjAppraisement.TransportMode = Result["TransportMode"].ToString();

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

        public void GetCustomAppraisementForPrint(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpAppraisementForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CustomAppraisement ObjAppId = new Hdb_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppId.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppId.AppraisementDate = (Result["AppraisementDate"] == null ? "" : Result["AppraisementDate"]).ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppId.LstAppraisement.Add(new Hdb_CustomAppraisementDtl
                        {
                            BOEDate = (Result["Date"] == null ? "" : Result["Date"]).ToString(),
                            ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                            // ContainerNo_Print = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            IGMNo = (Result["IGMNo"] == null ? "" : Result["IGMNo"]).ToString(),
                            SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString(),
                            // ITEMNo = (Result["ITEMNo"] == null ? "" : Result["ITEMNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                             CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                        // Importer= (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                        // Importer= (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                        // SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPkg"] == DBNull.Value ? 0 : Result["NoOfPkg"]),
                            // ExamDate = Convert.ToString(Result["ExamDate"] == DBNull.Value ? 0 : Result["ExamDate"])
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
                    _DBResponse.Data = ObjAppId;
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

        #region Custom Appraisement Work Order

        public void AddEditCstmAppraiseWorkOrdr(Hdb_CstmAppraiseWorkOrder ObjWorkOrder, string AppraisementXML /* , string YardXML*/)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjWorkOrder.CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjWorkOrder.WorkOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjWorkOrder.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.YardId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLocationIds", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjWorkOrder.YardWiseLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLctnNames", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjWorkOrder.YardWiseLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = ObjWorkOrder.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_YardXML", MySqlDbType = MySqlDbType.Text, Value = YardXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpCstmAppraiseWorkOrdr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjWorkOrder.CstmAppraiseWorkOrderId == 0 ? "Custom Appraisement Work Order Details Saved Successfully" : "Custom Appraisement Work Order Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Work Order Details As It Already Exists In Another Page";
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


        public void GetAppraisementNoForWorkOrdr()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraisementNoForWorkOrdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CstmAppraiseWorkOrder> LstAppraisement = new List<Hdb_CstmAppraiseWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new Hdb_CstmAppraiseWorkOrder
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
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
        public void GetCstmAppraiseDtlForWorkOrdr(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseDtlForWorkOrdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CustomAppraisement ObjAppraisement = new Hdb_CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisement.Add(new Hdb_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString()
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
        public void GetCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CstmAppraiseWorkOrder ObjAppraisement = new Hdb_CstmAppraiseWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjAppraisement.CstmAppraiseWorkOrderId = Convert.ToInt32(Result["CstmAppraiseWorkOrderId"]);
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    //ObjAppraisement.CstmAppraiseWorkOrderNo = Result["CstmAppraiseWorkOrderNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.Remarks = Result["Remarks"].ToString();
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    //ObjAppraisement.YardId = Convert.ToInt32(Result["YardId"]);
                    //ObjAppraisement.YardName = Convert.ToString(Result["YardName"] == null ? "" : Result["YardName"]);
                    //ObjAppraisement.YardWiseLocationIds = Convert.ToString(Result["YardWiseLocationIds"] == null ? "" : Result["YardWiseLocationIds"]);
                    //ObjAppraisement.YardWiseLctnNames = Convert.ToString(Result["YardWiseLctnNames"] == null ? "" : Result["YardWiseLctnNames"]);
                    ObjAppraisement.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementDtl.Add(new Hdb_CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            // LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            // CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            //ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            Importer = Result["Importer"].ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            // WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString()
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
        public void GetAllCstmAppraiseWorkOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CstmAppraiseWorkOrder> LstAppraisement = new List<Hdb_CstmAppraiseWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new Hdb_CstmAppraiseWorkOrder
                    {
                        CstmAppraiseWorkOrderNo = Result["CstmAppraiseWorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CstmAppraiseWorkOrderId = Convert.ToInt32(Result["CstmAppraiseWorkOrderId"])
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
        public void DelCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Work Order Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Work Order Details As It Exist In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Work Order Details As It Exist In Destuffing Application";
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
        public void GetCstmAppraiseWOForPreview(int CstmAppraiseWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWOForPreview", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CstmAppraiseWorkOrder ObjAppraisement = new Hdb_CstmAppraiseWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.LstAppraisementDtl.Add(new Hdb_CustomAppraisementDtl
                    {
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                        GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                        Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                    });
                    ObjAppraisement.AppraisementNo = Result["CstmAppraiseWorkOrderNo"].ToString();
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.YardWiseLctnNames = (Result["YardWiseLctnNames"] == null ? "" : Result["YardWiseLctnNames"]).ToString();
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
        public void GetAllYard()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDBYard> LstYard = new List<HDBYard>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstYard.Add(new HDBYard
                    {
                        YardId = Convert.ToInt32(Result["YardId"]),
                        YardName = Result["YardName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstYard;
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

        #region Custom Appraisement Approval
        public void NewCustomeAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfNewAppfrCustApp", CommandType.StoredProcedure, dpram);
            IList<Hdb_ListOfCustAppraismentAppr> lstApproval = new List<Hdb_ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new Hdb_ListOfCustAppraismentAppr
                    {
                        CstmAppraiseWorkOrderId = Convert.ToInt32(result["CstmAppraiseWorkOrderId"]),
                        CstmAppraiseWorkOrderNo = result["CstmAppraiseWorkOrderNo"].ToString(),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                        CustomAppraisementId = Convert.ToInt32(result["CustomAppraisementId"]),
                        ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                        FormOneNo = (result["FormOneNo"] == null ? "" : result["FormOneNo"]).ToString(),
                        BL = (result["BL"] == null ? "" : result["BL"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval = lstApproval, State = State };
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
        public void ApprovalHoldCustomAppraisement(int Skip, string status)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Value = status });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfAppHoldCustAppForRemoval", CommandType.StoredProcedure, dpram);
            IList<Hdb_ListOfCustAppraismentAppr> lstApproval = new List<Hdb_ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new Hdb_ListOfCustAppraismentAppr
                    {
                        CstmAppraiseWorkOrderId = Convert.ToInt32(result["CstmAppraiseWorkOrderId"]),
                        CstmAppraiseWorkOrderNo = result["CstmAppraiseWorkOrderNo"].ToString(),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                        CustomAppraisementId = Convert.ToInt32(result["CustomAppraisementId"]),
                        ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                        FormOneNo = (result["FormOneNo"] == null ? "" : result["FormOneNo"]).ToString(),
                        BL = (result["BL"] == null ? "" : result["BL"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval, State };
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
        public void UpdateCustomApproval(int CstmAppraiseWorkOrderId, int IsApproved, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Approved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("UpdateCustomApp", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Approval Details Saved Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Approval Details As It Already Exist In Another Page";
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

        #endregion

        #region Import Yard Payment Sheet
        public void GetAppraismentRequestForPaymentSheet(int AppraisementAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_PaySheetStuffingRequest> objPaySheetStuffing = new List<Hdb_PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Hdb_PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"])
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
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_PaymentPartyName> objPaymentPartyName = new List<Hdb_PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Hdb_PaymentPartyName()
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
        public void GetPaymentPartyForImportInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImportInvoice", CommandType.StoredProcedure);
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



        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public void GetCFSCodeForEdit(int AppraisementAppId, string Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCFSCodeForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_PaymentSheetContainer> objPaymentSheetContainer = new List<Hdb_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Hdb_PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = HasColumn(Result, "ContainerNo") ? Convert.ToString(Result["ContainerNo"]) : "",
                        Selected = false,
                        ArrivalDt = HasColumn(Result, "ArrivalDt") ? Convert.ToString(Result["ArrivalDt"]) : "",
                        IsHaz = HasColumn(Result, "IsHaz") ? Convert.ToString(Result["IsHaz"]) : "",
                        Size = HasColumn(Result, "Size") ? Convert.ToString(Result["Size"]) : "",
                        LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : "",
                        LineNo = HasColumn(Result, "LineNo") ? Convert.ToString(Result["LineNo"]) : "",
                        BOENo = HasColumn(Result, "BOENo") ? Convert.ToString(Result["BOENo"]) : "",
                        BOEDate = HasColumn(Result, "BOEDate") ? Convert.ToString(Result["BOEDate"]) : ""

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
        public void GetContainerForPaymentSheet(int AppraisementAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_PaymentSheetContainer> objPaymentSheetContainer = new List<Hdb_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Hdb_PaymentSheetContainer()
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

        // Not required
        public void GetContainerPaymentSheet(string InvoiceDate, int StuffingAppId, string ContainerXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_PaySheetChargeDetails objPaymentSheet = new Hdb_PaySheetChargeDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.lstPSContainer.Add(new Hdb_PSContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        IsReefer = Convert.ToBoolean(Result["Reefer"]),
                        Insured = Convert.ToString(Result["Insured"])
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstEmptyGR.Add(new PSEmptyGroudRent()
                //        {
                //            ContainerType = Convert.ToString(Result["ContainerType"]),
                //            CommodityType = Convert.ToString(Result["CommodityType"]),
                //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                //            Size = Convert.ToString(Result["Size"]),
                //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            FOBValue = Convert.ToDecimal(Result["Fob"]),
                //            IsInsured = Convert.ToInt32(Result["Insured"])
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstLoadedGR.Add(new Hdb_PSLoadedGroudRent()
                        {
                            ContainerType = Convert.ToString(Result["ContainerType"]),
                            CommodityType = Convert.ToString(Result["CommodityType"]),
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            Size = Convert.ToString(Result["Size"]),
                            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            FOBValue = Convert.ToDecimal(Result["Fob"]),
                            IsInsured = Convert.ToInt32(Result["Insured"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.InsuranceRate = Convert.ToDecimal(Result["InsuranceCharge"]) / 100 / 1000;
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstStorageRent.Add(new StorageRent()
                //        {
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            ActualCUM = Convert.ToDecimal(Result["CUM"]),
                //            ActualSQM = Convert.ToDecimal(Result["SQM"]),
                //            ActualWeight = Convert.ToDecimal(Result["GrossWeight"]),
                //            StuffCUM = Convert.ToDecimal(Result["DeStuffCUM"]),
                //            StuffSQM = Convert.ToDecimal(Result["DeStuffSQM"]),
                //            StuffWeight = Convert.ToDecimal(Result["DestuffingWeight"]),
                //            StorageDays = Convert.ToInt32(Result["StorageDays"]),
                //            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                //            StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
                //            StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"])
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.RateSQMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                        objPaymentSheet.RateSQMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                        objPaymentSheet.RateCUMPerWeek = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
                        objPaymentSheet.RateMTPerDay = Convert.ToDecimal(Result["RateMetricTonPerDay"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstInsuranceCharges.Add(new Hdb_InsuranceCharge()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                            IsInsured = Convert.ToInt16(Result["Insured"]),
                            FOB = Convert.ToDecimal(Result["Fob"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstPSHTCharge.Add(new Hdb_PSHTCharges()
                        {
                            ChargeId = Convert.ToInt32(Result["HTChargesID"]),
                            ChargeName = Convert.ToString(Result["Description"]),
                            Charge = Convert.ToDecimal(Result["RateCWC"])
                        });
                    }
                }
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
        public void AddEditContainerInvoice(PaymentSheetFinalModel ObjPSFinalModel, string ContainerXML, string ChargesXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.StuffingReqNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.StuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.Invoice });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "IMP" });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
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
        //
       
        public void GetIRNForInvoice(String InvoiceNo, String TypeOfInv)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });

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
                        seller.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
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
                        buyer.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
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
                        ship.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
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
                            PrdDesc = Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = Result["Unit"].ToString(),
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
            Hdb_IrnB2CDetails Obj = new Hdb_IrnB2CDetails();

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
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Saved Successfully" : "OBL Entry Updated Successfully";
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
        public void AddEditIRNB2C(String IRN, String QrCode, string InvoiceNo)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = IRN });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = QrCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnB2Cresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Saved Successfully" : "OBL Entry Updated Successfully";
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

        #endregion

        #region Delivery Application
        public void GetAllDeliveryApplication(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DeliveryApplicationList> LstDeliveryApp = new List<Hdb_DeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new Hdb_DeliveryApplicationList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        DeliveryAppDate = Result["DeliveryAppDate"].ToString(),
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"]),
                        OBL = Result["OBL"].ToString(),
                        FormOneNo = Result["FormOneNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingDate"].ToString(),
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


        public void GetDeliveryFCLByOBL(string OBL)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = OBL });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetImpDelverybyOBL", CommandType.StoredProcedure, DParam);
            List<Hdb_DeliveryApplicationList> LstDeliveryApp = new List<Hdb_DeliveryApplicationList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new Hdb_DeliveryApplicationList
                    {
                        DestuffingEntryNo = result["DestuffingEntryNo"].ToString(),
                        DeliveryNo = result["DeliveryNo"].ToString(),
                        DeliveryAppDate = result["DeliveryAppDate"].ToString(),
                        DeliveryId = Convert.ToInt32(result["DeliveryId"]),
                        OBL = result["OBL"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString(),
                        DestuffingEntryDate = result["DestuffingDate"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstDeliveryApp;
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

        public void GetDeliveryApplication(int DeliveryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_DeliveryApplication ObjDeliveryApp = new Hdb_DeliveryApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.DeliveryId = Convert.ToInt32(Result["DeliveryId"]);
                    ObjDeliveryApp.DeliveryNo = Result["DeliveryNo"].ToString();
                    ObjDeliveryApp.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDeliveryApp.DestuffingId = Convert.ToInt32(Result["DestuffingId"]);
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    ObjDeliveryApp.Importer = Result["Importer"].ToString();
                    ObjDeliveryApp.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryApp.IsPrinting = Result["IsPrinting"].ToString();
                    ObjDeliveryApp.NoOfPrint = Convert.ToInt32(Result["NoOfPrint"]);
                    ObjDeliveryApp.ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString();
                    ObjDeliveryApp.CHA = Result["CHA"].ToString();
                    ObjDeliveryApp.TransportMode = Result["TransportMode"].ToString();


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.LstDeliveryAppDtl.Add(new Hdb_DeliveryApplicationDtl
                        {
                            DeliveryDtlId = Convert.ToInt32(Result["DeliveryDtlId"]),
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOELineNo = (Result["BOELineNo"] == null ? "" : Result["BOELineNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]),
                            CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            DelCUM = Convert.ToDecimal(Result["DelCUM"] == DBNull.Value ? 0 : Result["DelCUM"]),
                            DelSQM = Convert.ToDecimal(Result["DelSQM"] == DBNull.Value ? 0 : Result["DelSQM"]),
                            DelGrossWt = Convert.ToDecimal(Result["DelGrossWt"] == DBNull.Value ? 0 : Result["DelGrossWt"]),
                            DelCIF = Convert.ToDecimal(Result["DelCIF"] == DBNull.Value ? 0 : Result["DelCIF"]),
                            DelDuty = Convert.ToDecimal(Result["DelDuty"] == DBNull.Value ? 0 : Result["DelDuty"]),
                            DelNoOfPackages = Convert.ToInt32(Result["DelNoOfPackages"] == DBNull.Value ? 0 : Result["DelNoOfPackages"]),
                            IsBndConversion = Convert.ToInt32(Result["IsBndConversion"] == DBNull.Value ? 0 : Result["IsBndConversion"]),
                            ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString(),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"] == DBNull.Value ? 0 : Result["ForwarderId"]),
                            // ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            // Importer = Result["Importer"].ToString()
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                        });
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
        public void AddEditDeliveryApplication(Hdb_DeliveryApplication ObjDeliveryApp, string DeliveryXml)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Printing", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjDeliveryApp.IsPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrint", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjDeliveryApp.NoOfPrint });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjDeliveryApp.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDeliveryApplication", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
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
                    _DBResponse.Message = "Cannot Edit Delivery Application Details As It Already Exist In Another Page";
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
            List<Hdb_BOELineNoList> LstBOELineNo = new List<Hdb_BOELineNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBOELineNo.Add(new Hdb_BOELineNoList
                    {
                        BOELineNo = Result["BOELineNo"].ToString(),
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
            Hdb_DeliveryApplicationDtl ObjDeliveryApp = new Hdb_DeliveryApplicationDtl();
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
                    ObjDeliveryApp.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDeliveryApp.CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]);
                    ObjDeliveryApp.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    ObjDeliveryApp.Commodity = Result["Commodity"].ToString();
                    ObjDeliveryApp.Importer = Result["Importer"].ToString();
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    ObjDeliveryApp.ForwarderName = Result["ForwarderName"].ToString();
                    ObjDeliveryApp.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
                    ObjDeliveryApp.CHA = Result["CHA"].ToString();
                    ObjDeliveryApp.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryApp.TransportMode = Convert.ToString(Result["TransportMode"]);

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
        public void GetDestuffEntryNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_DestuffingEntryNoList> LstDestuffEntryNo = new List<Hdb_DestuffingEntryNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffEntryNo.Add(new Hdb_DestuffingEntryNoList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
                        HBLNo = (Result["HBLNo"] == null ? "" : Result["HBLNo"]).ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
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

        #endregion

        #region Godown Payment Sheet



        public void GetGodownPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
         string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour,
         string InvoiceType, string LineXML,string ExportUnder, int Distance, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_PostPaymentSheet objInvoice = new Hdb_PostPaymentSheet();
            List<String> Clause = new List<String>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownDeliveryPaymentSheet", CommandType.StoredProcedure, DParam);

            try
            {
                //DataSet Result = DataAccess.ExecuteDataSet("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
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
                            //AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            // ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            // OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            ISODC = Convert.ToInt32(Result["ISODC"]),

                        });
                        objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                           // AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),
                            ISODC = Convert.ToInt32(Result["ISODC"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
                        {
                            ChargeId = Convert.ToInt32(Result["Srno"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
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
                        objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
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
                            Clause = Result["Clause"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                        });
                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Clause.Add(Result["Clause"].ToString());

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPreInvoiceCargo.Add(new PreInvoiceCargo {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Result["CargoType"].ToString() == "" ? 0 : Convert.ToInt32(Result["CargoType"]),
                            //CartingDate = Result["CartingDate"].ToString(),
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
                            //StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }
                objInvoice.ActualApplicable = Clause;
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

        public void AddEditInvoice(Hdb_PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string CfsChargeXML, int BranchId, int Uid,
         string Module, string CargoXML = "")
        {
            //  var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];
            //  string cfsCodeWiseHtRateXML = UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ForwarderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Forwarder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Forwarder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOLNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOLDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOLDate });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = CfsChargeXML });


            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(String.IsNullOrEmpty(ObjPostPaymentSheet.Distance)?"0": ObjPostPaymentSheet.Distance) });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();

            

            


            int Result = DA.ExecuteNonQuery("AddEditGodownInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId,out ReturnObj);

            

            _DBResponse = new DatabaseResponse();
            if (GeneratedClientId == "")
            {
                Result = 3;
            }
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";

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
                else if (Result == 4)
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
        public void GetInvoiceDetailsForGodownPrintByNo(string InvoiceNo, string InvoiceType = "IMPDeli")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForGodownPrintByNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgInvoiceYard objPaymentSheet = new PpgInvoiceYard();
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

                        objPaymentSheet.ShippingLineName = Result["ShippingLinaName"].ToString();
                        objPaymentSheet.BOENo = Result["BOENo"].ToString();
                        objPaymentSheet.BOEDate = Result["BOEDate"].ToString();
                        objPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPaymentSheet.RequestNo = Result["StuffingReqNo"].ToString();
                        objPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
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
        public void GetBOLForDeliverApp(int DeliveryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLFrDeli", CommandType.StoredProcedure, dparam);
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

        #region Empty Container Payment Sheet
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
            IList<Hdb_PaySheetStuffingRequest> objPaySheetStuffing = new List<Hdb_PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ApplicationFor == "YARD")
                    {
                        objPaySheetStuffing.Add(new Hdb_PaySheetStuffingRequest()
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
                        objPaySheetStuffing.Add(new Hdb_PaySheetStuffingRequest()
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
                            StateCode = Convert.ToString(Result["StateCode"]),
                            AppNo = Convert.ToString(Result["AppNo"])
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
            IList<Hdb_PaymentSheetContainer> objPaymentSheetContainer = new List<Hdb_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Hdb_PaymentSheetContainer()
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

        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, string InvoiceFor, int CasualLabour, string ExportUnder,int Distance)
        {
            //int Status = 0;
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            //LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            //IDataParameter[] DParam = { };
            //DParam = LstParam.ToArray();
            //DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //_DBResponse = new DatabaseResponse();
            //try
            //{
            //    var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContPaymentSheet", DParam);
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
            //    objPostPaymentSheet.CalculateCharges(6, ChargeName);

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

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 20, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_InvoiceYard objInvoice = new Hdb_InvoiceYard();
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContPaymentSheet", CommandType.StoredProcedure, DParam);
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
                    objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
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
                        ISODC=Convert.ToInt32(Result["ISODC"]),

                    });
                    objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        ISODC=Convert.ToInt32(Result["ISODC"]),

                    });
                }
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
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
                    objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
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

        #region Issue Slip
        public void AddEditIssueSlip(HdbIssueSlip ObjIssueSlip)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjIssueSlip.IssueSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjIssueSlip.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjIssueSlip.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpIssueSlip", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjIssueSlip.IssueSlipId == 0 ? "Issue Slip Details Saved Successfully" : "Issue Slip Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Issue Slip  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Issue Slip  Details As It Already Exists In Another Page";
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
        public void DelIssueSlip(int IssueSlipId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Issue Slip Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Issue Slip Details As It Exists In Another Page";
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
        public void GetIssueSlip(int IssueSlipId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
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
                        ObjIssueSlip.LstContainer.Add(new HdbIssueSlipContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new HdbIssueSlipCargo
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
        public void GetAllIssueSlip(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new HdbIssueSlip
                    {
                        IssueSlipNo = Result["IssueSlipNo"].ToString(),
                        IssueSlipDate = Result["IssueSlipDate"].ToString(),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"])
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
        public void GetAllIssueSlipsearch(string PartyCode)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlipinvsrch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new HdbIssueSlip
                    {
                        IssueSlipNo = Result["IssueSlipNo"].ToString(),
                        IssueSlipDate = Result["IssueSlipDate"].ToString(),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"])
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
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new HdbIssueSlip
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
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
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
                        ObjIssueSlip.LstContainer.Add(new HdbIssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new HdbIssueSlipCargo
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
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
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
                        ObjIssueSlip.LstIssueSlipRpt.Add(new HdbIssueSlipReport
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

        #endregion

        #region  Import Delivery Payment Sheet
        public void GetDeliveryApplicationForImpPaymentSheet(int DestuffingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplicationForImpPaymentSheet", CommandType.StoredProcedure, DParam);
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
                        StuffingReqId = Convert.ToInt32(Result["DeliveryId"]),
                        StuffingReqNo = Convert.ToString(Result["DeliveryNo"]),
                        StuffingReqDate = Convert.ToString(Result["DeliveryAppDate"]),
                        DestuffingEntryDate = Convert.ToString(Result["DestuffingEntryDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        HBLNo = Convert.ToString(Result["HBLNo"]),
                        Importer=Convert.ToString(Result["Importer"]),
                        Forwarder = Convert.ToString(Result["Forwarder"]),
                        ForwarderId=Convert.ToInt32(Result["ForwarderId"])
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

        #endregion

        #region Form One FCL
        public void SaveImportIgmFileFCL(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string CargoXML, string ContainerXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIgmFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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
        public void SaveImportIALFileFCL(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string Kdl_FormOneModelListXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            //LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = Kdl_FormOneModelListXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIALFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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
        public void AddEditFormOneFCL(Hdb_FormOneModel objFormOne, int BranchId, string FormOneDetailXML, int CreatedBy)
        {
            string Status = "0";
            string TPDate = objFormOne.TPDate;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFormOne.FormOneDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.BLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BLDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFormOne.BLDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGMNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.IGMNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGMDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFormOne.IGMDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.TPNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.Date, Value =(TPDate==null? null : Convert.ToDateTime(TPDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TSANo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.TSANo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TSADate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFormOne.TSADate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VesselName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischargeId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.PortOfDischargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortCharge", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortChargeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortChargeAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContCBT", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.CBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CBTForm", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.CBTFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.GateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });

            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditFormOne", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
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
        public void DeleteFormOneFCL(int FormOneId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteFormOne", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Form-1 Details As It Already Exists In Another Page";
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
        public void GetICEGateDataFCL(string containerNo, string size)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = containerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = size });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetICEGateDataFCL", CommandType.StoredProcedure, DParam);
            List<Hdb_FormOneDetailModel> formonelist = new List<Hdb_FormOneDetailModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    formonelist.Add(new Hdb_FormOneDetailModel
                    {
                        ContainerNo = Convert.ToString(result["CONTAINER_NO"]),
                        ContainerSize = Convert.ToString(result["CONT_SIZE"]),
                        Noofpkg = Convert.ToInt32(result["NO_PKG"]),
                        PackageType = Convert.ToString(result["PKG_TYPE"]),
                        SealNo = Convert.ToString(result["CONT_SEAL_NO"].ToString()),
                        GrossWeight = Convert.ToDecimal(result["GR_WT"].ToString()),
                        LineNo = Convert.ToString(result["LINE_NO"]),
                        LCLFCL = Convert.ToString(result["CONT_STATUS"]),
                        OBLNO = Convert.ToString(result["OBL_NO"]),
                        OBLDate=Convert.ToString(result["OBL_DATE"]),
                        IGMNo= Convert.ToString(result["IGM_NO"]),
                        IGMdate= Convert.ToString(result["IGMDate"]),
                        TPNo= Convert.ToString(result["TPNo"]),
                        TPdate= Convert.ToString(result["TPDate"]),
                        CargoDesc= Convert.ToString(result["CARGO_DESC"]),
                        MarksNo= Convert.ToString(result["MARKS_OF_NUMBER"]),
                        PortOfLoading= Convert.ToInt32(result["PortId"]),
                        IgmImporter=Convert.ToString(result["IgmImp"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = formonelist;
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
        public void ListOfShippingLineFCL()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
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
        public void GetAutoPopulateDataFCL(string ContainerNumber, int BranchId)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetailsWithoutReference", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_EntryThroughGate ObjEntryThroughGate = new HDB_EntryThroughGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.LCLFCL = Result["LCLFCL"].ToString();
                    //
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    //
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    ObjEntryThroughGate.CBT = Convert.ToBoolean(Result["CBT"]);
                    ObjEntryThroughGate.IsODC = Convert.ToBoolean(Result["IsODC"]);
                    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoofPackages"] == DBNull.Value ? 0 : Result["NoofPackages"]);
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    string strDate = ObjEntryThroughGate.ReferenceDate;
                    string[] arrayDate = strDate.Split(' ');
                    ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;
                    ObjEntryThroughGate.Remarks = Convert.ToString(Result["Remarks"]);
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
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
        public void GetContainerFCL(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerWithoutReference", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new ContainerRefGateEntry
                    {
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        ContainerNo = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
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
        public void GetFormOneFCL(int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "FCL" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOneFcl", CommandType.StoredProcedure, DParam);
            IList<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Hdb_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        ContainerSize = Convert.ToString(result["ContainerSize"])
                        //,
                        //TrBondNo= Convert.ToString(result["TrBondNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetFormOneByContainerFCL(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormBYContainer", CommandType.StoredProcedure, DParam);
            IList<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Hdb_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                         ContainerSize = Convert.ToString(result["ContainerSize"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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



        public void GetIndentOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetIndentContainer", CommandType.StoredProcedure, DParam);
            IList<Hdb_ListContainerIndent> lstdet = new List<Hdb_ListContainerIndent>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstdet.Add(new Hdb_ListContainerIndent
                    {
                        IndentId = Convert.ToInt32(result["IndentId"]),
                        IndentNo = Convert.ToString(result["IndentNo"]),
                        IndentDate = Convert.ToString(result["IndentDate"]),
                        TrailerNo = Convert.ToString(result["TrailerNo"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        ContainerSize = Convert.ToString(result["ContainerSize"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstdet;
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
        public void GetFormOneByIdFCL(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOneFcl", CommandType.StoredProcedure, DParam);
            Hdb_FormOneModel objFormOne = new Hdb_FormOneModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.BLNo = Convert.ToString(result["BLNo"]);
                    objFormOne.BLDate = Convert.ToString(result["BLDate"]);
                    objFormOne.IGMNo = Convert.ToString(result["IGMNo"]);
                    objFormOne.IGMDate = Convert.ToString(result["IGMDate"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.VesselName = Convert.ToString(result["VesselName"]);
                    objFormOne.VoyageNo = Convert.ToString(result["VoyageNo"]);
                    objFormOne.RotationNo = Convert.ToString(result["RotationNo"]);
                    objFormOne.PortOfDischargeId = Convert.ToInt32(result["PortOfDischargeId"]);
                    objFormOne.PortName = Convert.ToString(result["PortName"]);
                    objFormOne.PortCharge = Convert.ToDecimal(result["PortCharge"]);
                    objFormOne.PortChargeAmt = Convert.ToDecimal(result["PortChargeAmount"]);
                    objFormOne.CargoType = Convert.ToInt32(result["CargoType"]);
                    objFormOne.CBT = Convert.ToString(result["Container_CBT"]);
                    objFormOne.CBTFrom = Convert.ToString(result["CbtForm"]);
                    objFormOne.TPNo = Convert.ToString(result["TPNo"]);
                    objFormOne.TPDate = Convert.ToString(result["TPDate"]);

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetail.Add(new Hdb_FormOneDetailModel()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            Reefer = Convert.ToInt32(result["Reefer"]),
                            FlatReck = Convert.ToInt32(result["FlatReck"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            MarksNo = Convert.ToString(result["MarksNo"]),
                            CHAId = Convert.ToInt32(result["CHAId"]),
                            CHAName = Convert.ToString(result["CHAName"]),
                            ImporterId = Convert.ToInt32(result["ImporterId"]),
                            ImporterName = Convert.ToString(result["ImporterName"]),
                            ForwarderId = Convert.ToInt32(result["ForwarderId"]),
                            ForwarderName = Convert.ToString(result["ForwarderName"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CommodityName = Convert.ToString(result["CommodityName"]),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            Remarks = Convert.ToString(result["Remarks"]),
                            LCLFCL = Convert.ToString(result["LCLFCL"]),
                            VehicleNo = Convert.ToString(result["VehicleNo"]),
                            Noofpkg = Convert.ToInt32(result["Noofpkg"]),
                            PackageType = Convert.ToString(result["PackageType"]),
                            Others = Convert.ToString(result["Others"]),
                            GrossWeight = Convert.ToDecimal(result["GrossWeight"].ToString()),
                            OBLNO = Convert.ToString(result["OBLNO"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            GrossDuty = Convert.ToDecimal(result["GrossDuty"]),
                            IsODC = Convert.ToInt32(result["IsODC"]),
                            OBLDate = Convert.ToString(result["OBLDate"]),
                            ShippingLineNameDet = Convert.ToString(result["ShippingLineName"]),
                            IgmImporter=Convert.ToString(result["IgmImporter"])

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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
        public void FormOnePrintFCL(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("FormOnePrintFCL", CommandType.StoredProcedure, DParam);
            Hdb_FormOnePrintModel objFormOne = new Hdb_FormOnePrintModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.ShippingLineNo = Convert.ToString(result["ShippingLine"]);
                    objFormOne.FormOneNo = Convert.ToString(result["CustomSlNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.CHAName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CHAAddress = Convert.ToString(result["Address"]);
                    objFormOne.CHAPhoneNo = Convert.ToString(result["PhoneNo"]);
                    objFormOne.ShippingLineNo = result["ShippingLine"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOnePrintDetail.Add(new Hdb_FormOnePrintDetailModel()
                        {
                            VesselName = Convert.ToString(result["VesselName"]),
                            VoyageNo = Convert.ToString(result["VoyageNo"]),
                            RotationNo = Convert.ToString(result["RotationNo"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            ImpName = Convert.ToString(result["EximTraderName"]),
                            ImpAddress = Convert.ToString(result["Address"]),
                            ImpName2 = Convert.ToString(result["ImporterParty2"]),
                            ImpAddress2 = Convert.ToString(result["ImporterPartyAddress2"]),
                            Type = Convert.ToString(result["TypeLoadEmpty"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            HazType = Convert.ToInt32(result["HazType"]) == 2 ? "NON-HAZ" : "HAZ",
                            ReferType = Convert.ToInt32(result["ReferType"]) == 0 ? "" : "/ REEFER"
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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

        #region Container Indent
        public void ListofForm1()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Form1Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfForm1forContIndent", CommandType.StoredProcedure, DParam);
            IList<Hdb_Form1Det> lstFormOne = new List<Hdb_Form1Det>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Hdb_Form1Det
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetContainerDetails(int Form1Id)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Form1Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Form1Id });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfForm1forContIndent", CommandType.StoredProcedure, DParam);
            IList<Hdb_ContainerDetails> lstCont = new List<Hdb_ContainerDetails>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCont.Add(new Hdb_ContainerDetails
                    {
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        ContainerSize = Convert.ToString(result["ContainerSize"]),
                        CHAName = Convert.ToString(result["CHAName"]),
                        IMPName = Convert.ToString(result["IMPName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCont;
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
        public void ListofContainerIndent(int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("Listofcontindent", CommandType.StoredProcedure, DParam);
            IList<Hdb_ListContainerIndent> lstdet = new List<Hdb_ListContainerIndent>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstdet.Add(new Hdb_ListContainerIndent
                    {
                        IndentId = Convert.ToInt32(result["IndentId"]),
                        IndentNo = Convert.ToString(result["IndentNo"]),
                        IndentDate = Convert.ToString(result["IndentDate"]),
                        TrailerNo = Convert.ToString(result["TrailerNo"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        ContainerSize = Convert.ToString(result["ContainerSize"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstdet;
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
        public void GetContainerIndentDet(int IndentId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IndentId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("Listofcontindent", CommandType.StoredProcedure, DParam);
            Hdb_ContainerIndent objIndent = new Hdb_ContainerIndent();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    objIndent.IndentId = Convert.ToInt32(result["IndentId"]);
                    objIndent.IndentNo = result["IndentNo"].ToString();
                    objIndent.IndentDate = result["IndentDate"].ToString();
                    objIndent.TrailerNo = result["TrailerNo"].ToString();
                    objIndent.ICDIn = result["ICDIn"].ToString();
                    objIndent.ICDOut = result["ICDOut"].ToString();
                    objIndent.Form1Id = Convert.ToInt32(result["Form1Id"]);
                    objIndent.Form1No = result["FormOneNo"].ToString();
                    objIndent.Remarks = result["Remarks"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        objIndent.lstContainerDetails.Add(new Hdb_ContainerDetails
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            CHAName = result["CHAName"].ToString(),
                            IMPName = result["IMPName"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objIndent;
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
        public void AddEditContainerIndent(Hdb_ContainerIndent objCont, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objCont.IndentId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCont.IndentDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrailerNo", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objCont.TrailerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDIn", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCont.ICDIn) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDOut", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCont.ICDOut) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Form1Id", MySqlDbType = MySqlDbType.Int32, Value = objCont.Form1Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objCont.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar,Size=100, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditimpcontindent", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Container Indent Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Indent Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Container Indent Details As It Already Exists In Another Page";
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
        public void GetContainerIndentForPrint(int IndentId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IndentId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("PrintContIndent", CommandType.StoredProcedure, DParam);
            Hdb_ContainerIndent objIndent = new Hdb_ContainerIndent();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    objIndent.IndentId = Convert.ToInt32(result["IndentId"]);
                    objIndent.IndentNo = result["IndentNo"].ToString();
                    objIndent.IndentDate = result["IndentDate"].ToString();
                    objIndent.TrailerNo = result["TrailerNo"].ToString();
                    objIndent.ICDIn = result["ICDIn"].ToString();
                    objIndent.ICDOut = result["ICDOut"].ToString();
                    objIndent.Form1Id = Convert.ToInt32(result["Form1Id"]);
                    //objIndent.Form1No = result["FormOneNo"].ToString();
                    objIndent.Remarks = result["Remarks"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        objIndent.lstContainerDetails.Add(new Hdb_ContainerDetails
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            CHAName = result["CHAName"].ToString(),
                            IMPName = result["IMPName"].ToString(),
                            CargoDesc=result["CargoDesc"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objIndent;
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
        public void DeleteContainerIndent(int IndentId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IndentId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IndentId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteContainerIndent", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Container Indent Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Indent Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Container Indent Details As It Already Exists In Another Page";
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
        #endregion

        #region Empty Container Gate Out

        public void GetEmptyContainerListForInvoiceGateOut()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoiceGateOut", CommandType.StoredProcedure, DParam);
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

        public void GetPaymentPartyForImportInvoiceGateOut()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImportInvoice", CommandType.StoredProcedure);
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

        public void GetEmptyContByEntryIdGateOut(int EntryId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoiceGateOut", CommandType.StoredProcedure, DParam);
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

        public void GetEmptyContPaymentSheetGateOut(string InvoiceDate, int DestuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, string InvoiceFor, int PartyId, string ExportUnder)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_InvoiceYard objInvoice = new Hdb_InvoiceYard();
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContPaymentSheetGateOut", CommandType.StoredProcedure, DParam);
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
                    objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
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
                        ISODC = Convert.ToInt32(Result["ISODC"]),

                    });
                    objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        ISODC=Convert.ToInt32(Result["ISODC"]),

                    });
                }
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
                    {
                        ChargeId = Convert.ToInt32(Result["ChargeId"]),
                        Clause = Result["Clause"].ToString(),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"] == DBNull.Value ? 0 : Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? 0 : Result["Amount"]),
                        Discount = Convert.ToDecimal(Result["Discount"] == DBNull.Value ? 0 : Result["Discount"]),
                        Taxable = Convert.ToDecimal(Result["Taxable"] == DBNull.Value ? 0 : Result["Taxable"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"] == DBNull.Value ? 0 : Result["IGSTPer"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"] == DBNull.Value ? 0 : Result["IGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"] == DBNull.Value ? 0 : Result["SGSTPer"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"] == DBNull.Value ? 0 : Result["SGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"] == DBNull.Value ? 0 : Result["CGSTPer"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"] == DBNull.Value ? 0 : Result["CGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"]),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                    });
                }


                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[4].Rows)
                {
                    objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
                    {
                        InvoiceId = InvoiceId,
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        Quantity = Convert.ToDecimal(Result["Quantity"] == DBNull.Value ? 0 : Result["Quantity"]),
                        Rate = Convert.ToDecimal(Result["Rate"] == DBNull.Value ? 0 : Result["Rate"]),
                        Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? 0 : Result["Amount"]),
                        Clause = Convert.ToString(Result["Clause"]),
                    });
                }
                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.ActualApplicable.Add(Convert.ToString(Result["Clause"]));
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

        public void AddEditEmptyContPaymentSheetGateOut(Hdb_InvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
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
            int Result = DA.ExecuteNonQuery("AddEditEmptyContPaymentSheetGateOut", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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

        #region Party and Payee Popup

        public void ListOfParty()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();            
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfParty", CommandType.StoredProcedure);
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
                        ImporterId = Convert.ToInt32(result["PartyId"]),
                        ImporterName = result["PartyName"].ToString()
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



        public void ListOfPayee(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });           
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForMergePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
               
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

        #endregion

        #region OBL Amendment
        public void OBLAmendmentDetail(string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {                
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLAmendmentDetail", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();
                List<Hdb_OblAmendment> lstOBLList = new List<Hdb_OblAmendment>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Result.Tables[0].Rows.Count; i++)
                        {
                            lstOBLList.Add(new Hdb_OblAmendment
                            {
                                OOBLNo = Convert.ToString(Result.Tables[0].Rows[i]["OOBLNo"]),
                                OBLDate = Convert.ToString(Result.Tables[0].Rows[i]["OBLDate"]),    
                                IGMNo = Convert.ToString(Result.Tables[0].Rows[i]["IGMNo"]),
                                IGMDate = Convert.ToString(Result.Tables[0].Rows[i]["IGMDate"]),
                                CFSCode = Convert.ToString(Result.Tables[0].Rows[i]["CFSCode"]),
                                ContainerNo = Convert.ToString(Result.Tables[0].Rows[i]["ContainerNo"]),
                                ONoOfPkg = Convert.ToInt32(Result.Tables[0].Rows[i]["ONoOfPkg"]),
                                OGRWT = Convert.ToDecimal(Result.Tables[0].Rows[i]["OGRWT"]),
                                OCIFValue = Convert.ToDecimal(Result.Tables[0].Rows[i]["OCIFValue"]),
                                OGRDuty = Convert.ToDecimal(Result.Tables[0].Rows[i]["OGRDuty"]),
                                OImporterId= Convert.ToInt32(Result.Tables[0].Rows[i]["ImporterId"]),
                                OImporterName= Convert.ToString(Result.Tables[0].Rows[i]["ImporterName"])
                            });
                        }                  
                                                
                    
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBLList;
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

        public void GetOBLAmendmentList(string SearchValue, int Page)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLAmendmentList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_OblAmendment> lstOBLAmendment = new List<Hdb_OblAmendment>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstOBLAmendment.Add(new Hdb_OblAmendment
                    {
                        Date=Convert.ToString(Result["CreatedOn"]),
                        OOBLNo = Result["OOBLNo"].ToString(),                        
                        NOBLNo = Convert.ToString(Result["NOBLNo"]),
                        ONoOfPkg = Convert.ToInt32(Result["ONoPKG"]),
                        NNoOfPkg = Convert.ToInt32(Result["NNoPKG"]),
                        OGRWT= Convert.ToDecimal(Result["OGrossWt"]),
                        NGRWT= Convert.ToDecimal(Result["NGrossWt"]),
                        OCIFValue = Convert.ToDecimal(Result["OCIFValue"]),
                        OGRDuty = Convert.ToDecimal(Result["OGRDuty"]),
                        NCIFValue = Convert.ToDecimal(Result["NCIFValue"]),
                        NGRDuty = Convert.ToDecimal(Result["NGRDuty"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBLAmendment;
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

        public void GetImporterForAmendment(int Page, string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImporterForAmendment", CommandType.StoredProcedure, DParam);
            List<Hdb_Importer> LstPartyDetails = new List<Hdb_Importer>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyDetails.Add(new Hdb_Importer
                    {
                        ImporterId = Convert.ToInt32(Result["PartyId"]),
                        ImporterCode = Convert.ToString(Result["PartyCode"]),
                        ImporterName = Convert.ToString(Result["PartyName"]),

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
                    _DBResponse.Data = new { LstPartyDetails, State };
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

        public void AddEditOBLAmendment(Hdb_OblAmendment objOBL)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OOBLNo", MySqlDbType = MySqlDbType.String, Value = objOBL.OOBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.String, Value = objOBL.OBLDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objOBL.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ONoPKG", MySqlDbType = MySqlDbType.String, Value = objOBL.ONoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(objOBL.OGRWT) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OCIFValue", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.OCIFValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OGRDuty", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.OGRDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NOBLNo", MySqlDbType = MySqlDbType.String, Value = objOBL.NOBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NNoPKG", MySqlDbType = MySqlDbType.String, Value = objOBL.NNoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.NGRWT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NCIFValue", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.NCIFValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NGRDuty", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.NGRDuty });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.OImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OImporterName", MySqlDbType = MySqlDbType.String, Value = objOBL.OImporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.NImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NImporterName", MySqlDbType = MySqlDbType.String, Value = objOBL.NImporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
           
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLAmendment", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "OBL No Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update delivery application already done";
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
        #endregion
    }
}