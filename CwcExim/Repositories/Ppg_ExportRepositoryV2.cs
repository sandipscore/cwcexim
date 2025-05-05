using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Export.Models;
using System.Configuration;
using CwcExim.Models;
using System.Web.Mvc;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using System.Globalization;

namespace CwcExim.Repositories
{
    public class Ppg_ExportRepositoryV2
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get 
            {
                return _DBResponse;
            }
        }

        #region CCIN 

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
        public void GetAllCountry()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Country> LstCountry = new List<Country>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCountry.Add(new Country
                    {
                        CountryName = Result["CountryName"].ToString(),
                        CountryAlias = (Result["CountryAlias"] == null ? "" : Result["CountryAlias"]).ToString(),
                        CountryId = Convert.ToInt32(Result["CountryId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCountry;
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

        public void GetCountry(int CountryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CountryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Country ObjCountry = new Country();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCountry.CountryName = Result["CountryName"].ToString();
                    ObjCountry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjCountry.CountryAlias = (Result["CountryAlias"] == null ? "" : Result["CountryAlias"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCountry;
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
        public void ListOfExporter()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
            IList<Exporter> lstExporter = new List<Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Exporter
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
            // ShippingLine LstShippingLine = new ShippingLine();
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
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<Areas.Export.Models.CHA> lstCHA = new List<Areas.Export.Models.CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Areas.Export.Models.CHA
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
        public void GetSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBListV2", CommandType.StoredProcedure);
            List<CCINEntryV2> LstSB = new List<CCINEntryV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CCINEntryV2
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
                Result = DataAccess.ExecuteDataSet("GetSBDetailsBySBIdV2", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                CCINEntryV2 objCCINEntry = new CCINEntryV2();

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

        public void AddEditCCINEntry(CCINEntryV2 objCCINEntry)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoadingName", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PortOfLoadingName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischarge", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PortOfDischarge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Package", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Package });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.Weight });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.FOB });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CommodityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = objCCINEntry.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.CargoTypeID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IP", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.IP });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PackageType });

           

            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.Int32, Value = (objCCINEntry.IsSEZ==true?1:0) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("addeditccinentryV2", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Message = "Can not update it is already Approved.";
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

        public void AddEditCCINEntryApproval(CCINEntryV2 objCCINEntry)
        {

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExiamAppID", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.EximappID });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = objCCINEntry.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.CargoType) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.Approved) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PackageType });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsSEZ", MySqlDbType = MySqlDbType.Int32, Value = (objCCINEntry.IsSEZ == true ? 1 : 0) });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("addeditccinentryapporvalV2", CommandType.StoredProcedure, DParam);
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

        public void GetAllCCINEntry(string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
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

        public void GetAllCCINEntryForPage(int Page, string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForPageV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
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
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryApprovalForPageV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINPartyListV2", CommandType.StoredProcedure);
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

                CCINEntryV2 objCCINEntry = new CCINEntryV2();
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
            IDataReader Result = DA.ExecuteDataReader("getccinentrybyidV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            try
            {

                while (Result.Read())
                {
                    objCCINEntry.nonApproval = Convert.ToString(Result["nonApproval"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                        objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
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
                        objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                        objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                        objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                        objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                        objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                        objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                        objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                        objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                        objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                        objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                        objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                        objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
                        objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                        objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDescription"]);
                        objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);
                    }
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


        public void GetCCINEntryForEdit(int Id, int PartyId)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINDetForEditV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                    objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
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
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.CargoTypeID = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                    objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                    objCCINEntry.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);

                    objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                    objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDescription"]);
                    objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);
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
            PPGBTTCargoDetV2 objBTTCargo = new PPGBTTCargoDetV2();
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
            // /   lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetShiftById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<PPGBTTCargoDetV2> SbNoList = new List<PPGBTTCargoDetV2>();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    PPGBTTCargoDetV2 objBTTCargo = new PPGBTTCargoDetV2();

                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["Weight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["FobValue"]);
                    objBTTCargo.exporter = Convert.ToString(Result["Exporter"]);
                    SbNoList.Add(objBTTCargo);
                }


                if (Status == 1)
                {
                    _DBResponse.Data = SbNoList;
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
            int result = DA.ExecuteNonQuery("DeleteCCINEntryV2", CommandType.StoredProcedure, Dparam);
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
        public void GetCCINShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINShippingLineV2", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.ShippingLine> LstShippingLine = new List<Areas.Export.Models.ShippingLine>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new Areas.Export.Models.ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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



        public void GetMstGodownList()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["uid"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownV2", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.GodownList> LstGodown = new List<Areas.Export.Models.GodownList>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Export.Models.GodownList
                    {

                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void PrintCCINEntry(int Id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("PrintCCINV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
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
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CountryName = Result["CountryName"].ToString();
                    objCCINEntry.PackageType = Result["PackageType"].ToString();
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
        public void PrintCCINEntryApproval(int Id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("PrintCCINEntryApprovalV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
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
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CountryName = Result["CountryName"].ToString();
                    objCCINEntry.ApprovedBy = Result["ApprovedBy"].ToString();
                    objCCINEntry.ApprovedOn = Result["ApprovedOn"].ToString();
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
            List<PPG_ContainerMovementV2> LstInternalMovement = new List<PPG_ContainerMovementV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_ContainerMovementV2
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
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int pvalue, decimal trv, string cargotype, int PayeeId,string SEZ, int InvoiceId = 0)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargotype", MySqlDbType = MySqlDbType.VarChar, Value = cargotype });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Constuffing", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();



            PPG_MovementInvoiceV2 objInvoice = new PPG_MovementInvoiceV2();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerMovementPaymentSheetV2", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainerV2
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
                        objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainerV2
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
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrgV2
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
                        objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmountV2
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmountV2
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

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new ppgCMPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
                        });
                    }


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstShipbillwiseAmountV2.Add(new PpgShipbillwiseAmountV2
                        {
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingDate = Convert.ToString(Result["ShippingDate"]),
                            GateInDate = Convert.ToString(Result["GateInDate"]),
                            CCINDate=Convert.ToString(Result["CCINDate"]),
                            CargoType = Convert.ToInt32(Result["CargoType"])
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
            List<Areas.Import.Models.Godown> LstGodown = new List<Areas.Import.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Import.Models.Godown
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

        public void AddEditInvoiceMovement(PPG_Movement_InvoiceV2 ObjPostPaymentSheet, PPG_Movement_InvoiceV2 ObjPostPaymentSheett, PPG_Movement_InvoiceV2 ObjPostPaymentSheettt, string ContainerXML, string ContainerXMLL, string ContainerXMLLL, string ChargesXML, string ChargesXMLL, string ChargesXMLLL, string ContWiseChargeXML, string ContWiseChargeXMLL, string ContWiseChargeXMLLL, string OperationCfsCodeWiseAmtXML, string OperationCfsCodeWiseAmtXMLL, string OperationCfsCodeWiseAmtXMLLL, string ChargesBreakupXML,
           string ChargesBreakupXMLL, string ChargesBreakupXMLLL,
      string SbXML, int BranchId, int Uid,
     string Module, decimal TareWeight, decimal CargoWeight, decimal ElwbCargoWeight, decimal ElwbTareWeight, string SEZ, string CargoXML = "")
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargo", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Cargo });


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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIdd", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNamee", MySqlDbType = MySqlDbType.VarChar, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIddd", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNameee", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNoo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddresss", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStatee", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodee", MySqlDbType = MySqlDbType.VarChar, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNooo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddressss", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateee", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodeee", MySqlDbType = MySqlDbType.VarChar, Value = "" });
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotall", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotalll", MySqlDbType = MySqlDbType.Decimal, Value = 0 });


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
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmtt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscountt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxablee", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmtt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmttt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscounttt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxableee", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmttt", MySqlDbType = MySqlDbType.Decimal, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLL", MySqlDbType = MySqlDbType.Text, Value = "" });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXMLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXMLLL", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SbXML", MySqlDbType = MySqlDbType.Text, Value = SbXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIdd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIdd });

            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIddd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIddd });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = TareWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWeight", MySqlDbType = MySqlDbType.Decimal, Value = CargoWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ElwbCargoWeight", MySqlDbType = MySqlDbType.Decimal, Value = ElwbCargoWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ElwbTareWeight", MySqlDbType = MySqlDbType.Decimal, Value = ElwbTareWeight });


            //   LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjj });

            //  LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjjj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditImpContainerMovementV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceListV2 inv = new PPG_InvoiceListV2();
            try
            {
                if (Result == 1)
                {
                    /*String[] invobj;
                    invobj = GeneratedClientId.Split(',');
                    String[] movobj;
                    movobj = ReturnObj.Split(',');
                    if (invobj.Length >= 1)
                        inv.invoiceno = invobj[0];
                    
                    inv.MovementNo = movobj[0];*/
                    _DBResponse.Data = ReturnObj;
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
            PPG_Movement_InvoiceV2 objPaymentSheet = new PPG_Movement_InvoiceV2();
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
                        objPaymentSheet.lstPostPaymentCont.Add(new PpgPostPaymentContainerV2()
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
                            objPaymentSheet.lstPostPaymentChrg.Add(new PpgPostPaymentChrgV2()
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
            PPG_ContainerMovementV2 ObjInternalMovement = new PPG_ContainerMovementV2();
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
        public void AddEditImpInternalMovement(PPG_ContainerMovementV2 ObjInternalMovement)
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerMovementV2> LstStuffing = new List<PPG_ContainerMovementV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPG_ContainerMovementV2
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
                            LstStuffing.Add(new PPG_ContainerMovementV2
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
        public void GetPaymentParty(int Page, string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Page) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(PartyCode) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExportV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.PaymentPartyNameV2> objPaymentPartyName = new List<Areas.Export.Models.PaymentPartyNameV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Areas.Export.Models.PaymentPartyNameV2()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
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
            List<PPG_ContainerMovementV2> LstInternalMovement = new List<PPG_ContainerMovementV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new PPG_ContainerMovementV2
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetConDetForInternalMovementV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_ContainerMovementV2 ObjInternalMovement = new PPG_ContainerMovementV2();
            List<PPG_ShippingBillV2> LstShippingBillNo = new List<PPG_ShippingBillV2>();

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
                            LstShippingBillNo.Add(new PPG_ShippingBillV2
                            {
                                shippingBillNo = Result["ShippingBillNo"].ToString(),
                                CargoWeight = Convert.ToDecimal(Result["CargoWeight"].ToString()),
                                CargoType = Convert.ToInt32(Result["CargoType"].ToString()),
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
                                LstShippingBillNo.Add(new PPG_ShippingBillV2
                                {
                                    shippingBillNo = Result["ShippingBillNo"].ToString(),
                                    CargoWeight = Convert.ToDecimal(Result["CargoWeight"].ToString()),
                                    CargoType = Convert.ToInt32(Result["CargoType"].ToString()),
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
        public void ListOfMovementInv(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ContMovementListV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContMovementList> LstMovement = new List<Ppg_ContMovementList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstMovement.Add(new Ppg_ContMovementList
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMovement;
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
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion


        #region Concor Ledger Sheet
        public void AddEditConcorledgersheet(PpgConcorLedgerSheetViewModelV2 ObjConcorSheet)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjConcorSheet.ID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SlNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = Convert.ToString(ObjConcorSheet.SlNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ConcorInvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.ConcorInvoiceNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjConcorSheet.InvoiceID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjConcorSheet.InvoiceDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjConcorSheet.InvoiceDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PODPOL", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.POLPOD) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.OperationType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.TrainNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjConcorSheet.TrainDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjConcorSheet.Size) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tareweight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NetWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.NetWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjConcorSheet.ContainerType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Grossweight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IRR", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.IRR) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_THC", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.THC) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Doc", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.DOC) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OtherChg", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.OtherChg) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreditAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.CreditAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Balance", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjConcorSheet.Balance) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditConcorLedgerSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Concor Ledger Sheet Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Concor Ledger Sheet Updated Successfully";
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

        public void GetContainerDetails(string ContainerNo, string TrainNo, string OprationType)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(TrainNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(OprationType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ContainerNo) });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTrainSummeryInfoConcor", CommandType.StoredProcedure, DParam);
            List<PpgConcorLedgerSheetViewModelV2> LstConcarLedgerSheet = new List<PpgConcorLedgerSheetViewModelV2>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstConcarLedgerSheet.Add(new PpgConcorLedgerSheetViewModelV2
                    {

                        Size = Convert.ToInt32(Result["Size"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        NetWeight = Convert.ToDecimal(Result["NetWeight"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        POLPOD = Convert.ToString(Result["PolPod"]),
                        ContainerType = Convert.ToString(Result["ContainerType"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstConcarLedgerSheet;
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

        public void GetContainerList(string TrainNo, string OprationType)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(TrainNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(OprationType) });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerConcor", CommandType.StoredProcedure, DParam);
            List<PpgConcorLedgerSheetViewModelV2> LstConcarLedgerSheet = new List<PpgConcorLedgerSheetViewModelV2>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstConcarLedgerSheet.Add(new PpgConcorLedgerSheetViewModelV2
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstConcarLedgerSheet;
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

        public void GetAllConcorLedgerSheet(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(id) });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLstConcorDetails", CommandType.StoredProcedure, DParam);
            List<PpgConcorLedgerSheetViewModelV2> LstConcarLedgerSheet = new List<PpgConcorLedgerSheetViewModelV2>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstConcarLedgerSheet.Add(new PpgConcorLedgerSheetViewModelV2
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Balance = Convert.ToDecimal(Result["Balance"]),
                        ID = Convert.ToInt32(Result["ID"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        THC = Convert.ToDecimal(Result["THC"]),
                        DOC = Convert.ToDecimal(Result["Doc"]),
                        OtherChg = Convert.ToDecimal(Result["OtherChg"]),
                        CreditAmount = Convert.ToDecimal(Result["CreditAmount"]),
                        GrossWeight = Convert.ToDecimal(Result["Grossweight"]),
                        TareWeight = Convert.ToDecimal(Result["Tareweight"]),
                        NetWeight = Convert.ToDecimal(Result["NetWeight"]),
                        ConcorInvoiceNo = Convert.ToString(Result["ConcorInvoiceNo"]),
                        ContainerType = Convert.ToString(Result["ContainerType"]),
                        Date = Convert.ToString(Result["Date"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        Size = Convert.ToInt32(Result["Size"]),

                        OperationType = Convert.ToString(Result["OperationType"]),
                        POLPOD = Convert.ToString(Result["PODPOL"]),
                        SlNo = Convert.ToString(Result["SlNo"]),
                        TrainNo = Convert.ToString(Result["TrainNo"]),
                        TrainDate = Convert.ToString(Result["TrainDate"])



                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstConcarLedgerSheet;
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

        public void GetAllConcorLedgerSheetEdit(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(id) });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLstConcorDetails", CommandType.StoredProcedure, DParam);
            PpgConcorLedgerSheetViewModelV2 LstConcarLedgerSheet = new PpgConcorLedgerSheetViewModelV2();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstConcarLedgerSheet.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    LstConcarLedgerSheet.Balance = Convert.ToDecimal(Result["Balance"]);
                    LstConcarLedgerSheet.ID = Convert.ToInt32(Result["ID"]);
                    LstConcarLedgerSheet.IRR = Convert.ToDecimal(Result["IRR"]);
                    LstConcarLedgerSheet.THC = Convert.ToDecimal(Result["THC"]);
                    LstConcarLedgerSheet.DOC = Convert.ToDecimal(Result["Doc"]);
                    LstConcarLedgerSheet.OtherChg = Convert.ToDecimal(Result["OtherChg"]);
                    LstConcarLedgerSheet.CreditAmount = Convert.ToDecimal(Result["CreditAmount"]);
                    LstConcarLedgerSheet.GrossWeight = Convert.ToDecimal(Result["Grossweight"]);
                    LstConcarLedgerSheet.TareWeight = Convert.ToDecimal(Result["Tareweight"]);
                    LstConcarLedgerSheet.NetWeight = Convert.ToDecimal(Result["NetWeight"]);
                    LstConcarLedgerSheet.ConcorInvoiceNo = Convert.ToString(Result["ConcorInvoiceNo"]);
                    LstConcarLedgerSheet.ContainerType = Convert.ToString(Result["ContainerType"]);
                    LstConcarLedgerSheet.Date = Convert.ToString(Result["Date"]);
                    LstConcarLedgerSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                    LstConcarLedgerSheet.Size = Convert.ToInt32(Result["Size"]);

                    LstConcarLedgerSheet.OperationType = Convert.ToString(Result["OperationType"]);
                    LstConcarLedgerSheet.POLPOD = Convert.ToString(Result["PODPOL"]);
                    LstConcarLedgerSheet.SlNo = Convert.ToString(Result["SlNo"]);
                    LstConcarLedgerSheet.TrainNo = Convert.ToString(Result["TrainNo"]);
                    LstConcarLedgerSheet.TrainDate = Convert.ToString(Result["TrainDate"]);




                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstConcarLedgerSheet;
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

        public void DeleteConcorledgersheet(int id)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(id) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteConcorLedgerSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Concor Ledger Sheet Delete Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Concor Ledger Sheet Updated Successfully";
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

        #endregion;



        #region Container Stuffing



        public void ListOfPortOfDestinationStuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPortOfDestinationForStuffing", CommandType.StoredProcedure);
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

        public void ListOfStuffingReqForAmendment(string sra)
        {
             
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_sra", MySqlDbType = MySqlDbType.VarChar, Value = sra });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfStuffingReqForAmendment", CommandType.StoredProcedure,DParam);
            List<ContainerStuffingV2> Lstsr = new List<ContainerStuffingV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstsr.Add(new ContainerStuffingV2
                    {
                        AmendmentStuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
                        AmendmentStuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
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


        public void ListOfPortOfDestinationStuffingSearch( string pod)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pod", MySqlDbType = MySqlDbType.VarChar, Value = pod });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPortOfDestinationForStuffing", CommandType.StoredProcedure,DParam);
           

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

        public void ListOfFinalDestination(string CustodianName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianName", MySqlDbType = MySqlDbType.VarChar, Value = CustodianName });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFinalDestination", CommandType.StoredProcedure, DParam);

            
            List<PPG_FinalDestination> LstCustodian = new List<PPG_FinalDestination>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCustodian.Add(new PPG_FinalDestination
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

        //with SCMTR
        // public void AddEditContainerStuffing(PPG_ContainerStuffingV2 ObjStuffing, string ContainerStuffingXML, string SCMTRXML, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
        //string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML, string ChargesXML, string BreakUpdateXML)
        //{
        //    string GeneratedClientId = "0";
        //    string ReturnObj = "0";


        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd") });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = SCMTRXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRETotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRETotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRESACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRECFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOSACCode });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CargoTypeId });

        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.PayeeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BreakUpdateXML", MySqlDbType = MySqlDbType.Text, Value = BreakUpdateXML });
        //    IDataParameter[] DParam = LstParam.ToArray();

        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DA.ExecuteNonQuery("AddEditContainerStuffingV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
        //    _DBResponse = new DatabaseResponse();
        //    PPG_InvoiceCSListV2 inv = new PPG_InvoiceCSListV2();
        //    try
        //    {

        //        if (Result == 1)
        //        {
        //            String[] invobj;
        //            invobj = ReturnObj.Split(',');
        //            if (invobj.Length > 0)
        //                inv.invoicenoGRE = invobj[0]; //InvoiceGRE;

        //            if (invobj.Length > 1)
        //                inv.invoicenoINS = invobj[1];// InvoiceINS;
        //            if (invobj.Length > 2)
        //                inv.invoicenoSTO = invobj[2];// InvoiceSTO;
        //            if (invobj.Length > 3)
        //                inv.invoicenoHND = invobj[3];// InvoiceHND;
        //            if (invobj.Length > 4)
        //                inv.invoicenoGENSP = invobj[4];
        //            _DBResponse.Data = inv;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Container Stuffing Details Saved Successfully" : "Container Stuffing Details Updated Successfully");
        //        }

        //        else if (Result == 2)
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Container Stuffing Details Already Exist";
        //        }
        //        else if (Result == 3)
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = ReturnObj;
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

        //without SCMTR
        public void AddEditContainerStuffing(PPG_ContainerStuffingV2 ObjStuffing, string ContainerStuffingXML, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
      string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML, string ChargesXML, string BreakUpdateXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";
            var SCMTRStuffingDate = (dynamic)null;
            SCMTRStuffingDate = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd")});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
         //   LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = SCMTRXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GRECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRECFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOCFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CargoTypeId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BreakUpdateXML", MySqlDbType = MySqlDbType.Text, Value = BreakUpdateXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.CustodianId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRStuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = SCMTRStuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });
            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffingV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceCSListV2 inv = new PPG_InvoiceCSListV2();
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
                    _DBResponse.Data = inv;
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
                    _DBResponse.Message = "Can't Update CIM SF File Already Send.";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM SF Acknowledgement Received.";
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status =5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Stuffing Approval Done.";
                }
                else if (Result == 7)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 7;
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

        public void AmndContainerStuffing(PPG_ContainerStuffingV2 ObjStuffing, string ContainerStuffingXML, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
    string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML, string ChargesXML, string BreakUpdateXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";
            var SCMTRStuffingDate = (dynamic)null;
            SCMTRStuffingDate = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:MM:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = SCMTRXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GRECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRECFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOCFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CargoTypeId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BreakUpdateXML", MySqlDbType = MySqlDbType.Text, Value = BreakUpdateXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.CustodianId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRStuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = SCMTRStuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });
            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AmndContainerStuffingV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceCSListV2 inv = new PPG_InvoiceCSListV2();
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
                    _DBResponse.Data = inv;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Amendment Container Stuffing Details Saved Successfully" : "Amendment Container Stuffing Details Updated Successfully");
                }

                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Amendment Container Stuffing Details Already Exist";
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
            ContainerStuffingDtlV2 ObjStuffing = new ContainerStuffingDtlV2();
            List<ContainerStuffingDtlV2> LstStuffing = new List<ContainerStuffingDtlV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new ContainerStuffingDtlV2
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
                        CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                        EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                        EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                        EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                        MCINPCIN = Convert.ToString(Result["MCINPCIN"]),
                        SEZ = Convert.ToInt32(Result["SEZ"])
                    });
                }
                if (Status == 1)
                {
                    ObjStuffing.LstStuffing = LstStuffing;
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
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
        public void GetAllContainerStuffingPage(int Uid,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
           
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingPageV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerNo= Result["ContainerNo"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingV2", CommandType.StoredProcedure, DParam);
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
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
                    ObjStuffing.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    ObjStuffing.PayeeName = Convert.ToString(Result["PayeeName"]);
                    ObjStuffing.InvoiceNoGRE= Convert.ToString(Result["InvoiceNoGRE"]);
                    ObjStuffing.GREPartyId= Convert.ToInt32(Result["PartyId"]);
                    ObjStuffing.GREPartyCode = Convert.ToString(Result["PartyName"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"]);
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new ContainerStuffingDtlV2
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
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
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                            MCINPCIN = Convert.ToString(Result["MCINPCIN"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.lstChargs.Add(new Ppg_ContStuffChargesV2
                        {
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            Taxable = Convert.ToDecimal(Result["Charge"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTCharge"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTCharge"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTCharge"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            Total = Convert.ToDecimal(Result["TotalAmount"])


                        });
                        ObjStuffing.InvoiceNoGRE = Result["InvoiceNo"].ToString();
                        ObjStuffing.GRETotalAmount = ObjStuffing.lstChargs.Sum(x => x.Taxable) + ObjStuffing.lstChargs.Sum(x => x.CGSTAmt) + ObjStuffing.lstChargs.Sum(x => x.SGSTAmt) + ObjStuffing.lstChargs.Sum(x => x.IGSTAmt);
                        //ObjStuffing.GREOperationId = Convert.ToInt32(Result["OperationId"]);
                        //ObjStuffing.GREPartyId = Convert.ToInt32(Result["PartyId"]);
                        //ObjStuffing.GREPartyCode = Convert.ToString(Result["PartyCode"]);
                        //ObjStuffing.GREChargeType = Convert.ToString(Result["ChargeType"]);
                        //ObjStuffing.GREChargeName = Convert.ToString(Result["ChargeName"]);
                        //ObjStuffing.GRECharge = Convert.ToDecimal(Result["Charge"]);
                        //ObjStuffing.GRECGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        //ObjStuffing.GRESGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        //ObjStuffing.GREIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        //ObjStuffing.GREIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        //ObjStuffing.GRECGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        //ObjStuffing.GRESGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        ////ObjStuffing.GREAmount = Convert.ToDecimal(Result["Amount"]);
                        ////ObjStuffing.GRETaxable = Convert.ToDecimal(Result["Taxable"]);
                        //ObjStuffing.GRESACCode = Convert.ToString(Result["SACCode"]);
                        //ObjStuffing.GRETotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        //ObjStuffing.InvoiceNoGRE = Convert.ToString(Result["InvoiceNo"]);
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjStuffing.LstSCMTRDtl.Add(new ContainerStuffingV2SCMTR
                //        {
                //            Id = Convert.ToInt32(Result["Id"]),
                //            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                //            ReportingpartyCode = (Result["ReportingpartyCode"] == null ? "" : Result["ReportingpartyCode"]).ToString(),
                //            Equipmenttype = (Result["Equipmenttype"] == null ? "" : Result["Equipmenttype"]).ToString(),
                //            ContainerID = (Result["ContainerID"] == null ? "" : Result["ContainerID"]).ToString(),
                //            EquipmentSize = !string.IsNullOrEmpty(Result["EquipmentSize"].ToString()) ? Result["EquipmentSize"].ToString() : "",
                //            EquipmentLoadStatus = Result["EquipmentLoadStatus"].ToString(),
                //            FinalDestinationLocation = Result["FinalDestinationLocation"].ToString(),
                //            EventDate = (Result["EventDate"] == null ? "" : Result["EventDate"]).ToString(),
                //            EquipmentSealType = (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString(),
                //            EquipmentSealNo = Result["EquipmentSealNo"].ToString(),
                //            EquipmentStatus = (Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]).ToString(),
                //            CargoSequenceNo = Convert.ToInt32(Result["CargoSequenceNo"]),
                //            DocumentType = (Result["DocumentType"] == null ? "" : Result["DocumentType"]).ToString(),
                //            ShipmentLoadStatus = (Result["ShipmentLoadStatus"] == null ? "" : Result["ShipmentLoadStatus"]).ToString(),
                //            PackageType = Convert.ToString(Result["PackageType"] == null ? "" : Result["PackageType"]),
                //            EquipmentSerialNo = Convert.ToInt32(Result["EquipmentSerialNo"]),
                //            DocumentTypeCode = Convert.ToString(Result["DocumentTypeCode"] == null ? "" : Result["DocumentTypeCode"]),
                //            DocumentReferenceNo = Convert.ToString(Result["DocumentReferenceNo"] == null ? "" : Result["DocumentReferenceNo"]),
                //            DocumentSerialNo = Convert.ToInt32(Result["DocumentSerialNo"]),

                //        });
                //    }
                //}

                /* if (Result.NextResult())
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
                 }*/





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
          
        public void GetContainerStuffingAmendment(int ContainerStuffingId, int Uid)
        { 
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingAmendmentV2", CommandType.StoredProcedure, DParam);
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
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
                    ObjStuffing.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    ObjStuffing.PayeeName = Convert.ToString(Result["PayeeName"]);
                    ObjStuffing.InvoiceNoGRE = Convert.ToString(Result["InvoiceNoGRE"]);
                    ObjStuffing.GREPartyId = Convert.ToInt32(Result["PartyId"]);
                    ObjStuffing.GREPartyCode = Convert.ToString(Result["PartyName"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"]);
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new ContainerStuffingDtlV2
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
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
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.lstChargs.Add(new Ppg_ContStuffChargesV2
                        {
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            Taxable = Convert.ToDecimal(Result["Charge"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTCharge"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTCharge"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTCharge"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            Total = Convert.ToDecimal(Result["TotalAmount"])


                        });
                        ObjStuffing.InvoiceNoGRE = Result["InvoiceNo"].ToString();
                        ObjStuffing.GRETotalAmount = ObjStuffing.lstChargs.Sum(x => x.Taxable) + ObjStuffing.lstChargs.Sum(x => x.CGSTAmt) + ObjStuffing.lstChargs.Sum(x => x.SGSTAmt) + ObjStuffing.lstChargs.Sum(x => x.IGSTAmt);
                        //ObjStuffing.GREOperationId = Convert.ToInt32(Result["OperationId"]);
                        //ObjStuffing.GREPartyId = Convert.ToInt32(Result["PartyId"]);
                        //ObjStuffing.GREPartyCode = Convert.ToString(Result["PartyCode"]);
                        //ObjStuffing.GREChargeType = Convert.ToString(Result["ChargeType"]);
                        //ObjStuffing.GREChargeName = Convert.ToString(Result["ChargeName"]);
                        //ObjStuffing.GRECharge = Convert.ToDecimal(Result["Charge"]);
                        //ObjStuffing.GRECGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]);
                        //ObjStuffing.GRESGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]);
                        //ObjStuffing.GREIGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]);
                        //ObjStuffing.GREIGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                        //ObjStuffing.GRECGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                        //ObjStuffing.GRESGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                        ////ObjStuffing.GREAmount = Convert.ToDecimal(Result["Amount"]);
                        ////ObjStuffing.GRETaxable = Convert.ToDecimal(Result["Taxable"]);
                        //ObjStuffing.GRESACCode = Convert.ToString(Result["SACCode"]);
                        //ObjStuffing.GRETotalAmount = Convert.ToDecimal(Result["TotalAmount"]);
                        //ObjStuffing.InvoiceNoGRE = Convert.ToString(Result["InvoiceNo"]);
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjStuffing.LstSCMTRDtl.Add(new ContainerStuffingV2SCMTR
                //        {
                //            Id = Convert.ToInt32(Result["Id"]),
                //            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                //            ReportingpartyCode = (Result["ReportingpartyCode"] == null ? "" : Result["ReportingpartyCode"]).ToString(),
                //            Equipmenttype = (Result["Equipmenttype"] == null ? "" : Result["Equipmenttype"]).ToString(),
                //            ContainerID = (Result["ContainerID"] == null ? "" : Result["ContainerID"]).ToString(),
                //            EquipmentSize = !string.IsNullOrEmpty(Result["EquipmentSize"].ToString()) ? Result["EquipmentSize"].ToString() : "",
                //            EquipmentLoadStatus = Result["EquipmentLoadStatus"].ToString(),
                //            FinalDestinationLocation = Result["FinalDestinationLocation"].ToString(),
                //            EventDate = (Result["EventDate"] == null ? "" : Result["EventDate"]).ToString(),
                //            EquipmentSealType = (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString(),
                //            EquipmentSealNo = Result["EquipmentSealNo"].ToString(),
                //            EquipmentStatus = (Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]).ToString(),
                //            CargoSequenceNo = Convert.ToInt32(Result["CargoSequenceNo"]),
                //            DocumentType = (Result["DocumentType"] == null ? "" : Result["DocumentType"]).ToString(),
                //            ShipmentLoadStatus = (Result["ShipmentLoadStatus"] == null ? "" : Result["ShipmentLoadStatus"]).ToString(),
                //            PackageType = Convert.ToString(Result["PackageType"] == null ? "" : Result["PackageType"]),
                //            EquipmentSerialNo = Convert.ToInt32(Result["EquipmentSerialNo"]),
                //            DocumentTypeCode = Convert.ToString(Result["DocumentTypeCode"] == null ? "" : Result["DocumentTypeCode"]),
                //            DocumentReferenceNo = Convert.ToString(Result["DocumentReferenceNo"] == null ? "" : Result["DocumentReferenceNo"]),
                //            DocumentSerialNo = Convert.ToInt32(Result["DocumentSerialNo"]),

                //        });
                //    }
                //}

                /* if (Result.NextResult())
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
                 }*/





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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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
            List<ContainerStuffingDtlV2> LstStuffing = new List<ContainerStuffingDtlV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new ContainerStuffingDtlV2
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
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Cannot Delete Cash Receipt Done.";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Cannot Delete CIM SF File Already Send";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Cannot Delete CIM SF Acknowledgement  Received.";
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffForPrintV2", CommandType.StoredProcedure, DParam);
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            PPG_ContainerStuffingDtlV2 lstcont = new PPG_ContainerStuffingDtlV2();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingDate = Result["StuffingDate"].ToString();
                    ObjStuffing.ContVia = Result["Via"].ToString();
                    ObjStuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjStuffing.CargoType = (Result["CargoType"] == null ? "" : Result["CargoType"]).ToString();
                    ObjStuffing.ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString();
                    ObjStuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjStuffing.CustodianCode= (Result["CustodianCode"] == null ? "" : Result["CustodianCode"]).ToString();
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
                        ObjStuffing.LstppgStuffingDtl.Add(new PPG_ContainerStuffingDtlV2
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
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToInt32(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            PortName = (Result["PortName"] == null ? "" : Result["PortName"]).ToString(),
                            PortDestination = (Result["PortDestination"] == null ? "" : Result["PortDestination"]).ToString(),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            EquipmentSealType= (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgCharge.Add(new PPG_ContainerStuffingChargeV2
                        {
                            Invoiceno = Convert.ToString(Result["invoiceno"] == DBNull.Value ? "" : Result["invoiceno"]),
                            InvoiceDate = Convert.ToString(Result["invoicedate"] == DBNull.Value ? "" : Result["invoicedate"]),
                            chargetype = Convert.ToString(Result["chargetype"] == DBNull.Value ? "" : Result["chargetype"]),
                            total = Convert.ToString(Result["total"] == DBNull.Value ? 0 : Result["total"]),
                            eximtraderalias = Convert.ToString(Result["eximtraderalias"] == DBNull.Value ? "" : Result["eximtraderalias"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgShipDtl.Add(new PPG_ShippingBillNoV2
                        {
                            ContainerNo = Convert.ToString(Result["ContainerNo"] == DBNull.Value ? "" : Result["ContainerNo"]),
                            CFSCode = Convert.ToString(Result["CFSCode"] == DBNull.Value ? "" : Result["CFSCode"]),
                            CargoType = Convert.ToString(Result["CargoType"] == DBNull.Value ? "" : Result["CargoType"]),
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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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

        public void CalculateGroundRentEmpty(String StuffingDate, String ContainerStuffingXML, int GREPartyId, int StuffingReqId)
        {
            int Status = 0;
           
            StuffingDate = Convert.ToDateTime(StuffingDate).ToString("dd-MM-yyyy");
            StuffingDate = DateTime.ParseExact(StuffingDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GREPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRentEmptyV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.lstChargs.Add(new Ppg_ContStuffChargesV2
                    {
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        ChargeType = Convert.ToString(result["ChargeType"]),
                        ChargeName = Convert.ToString(result["ChargeName"]),
                        CGSTAmt = Convert.ToDecimal(result["CGSTAmt"]),
                        SGSTAmt = Convert.ToDecimal(result["SGSTAmt"]),
                        IGSTAmt = Convert.ToDecimal(result["IGSTAmt"]),
                        IGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString()),
                        CGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString()),
                        SGSTPer = Convert.ToDecimal(result["SGSTPer"]),
                        Taxable = Convert.ToDecimal(result["Taxable"]),
                        Total = Convert.ToDecimal(result["Total"]),
                        SACCode = Convert.ToString(result["SACCode"])
                    });
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREOperationCFSCodeWiseAmtLst.Add(new GREOperationCFSCodeWiseAmtV2
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
                        ObjCS.GREContainerWiseAmtLst.Add(new GREContainerWiseAmtV2
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            GrEmpty = Convert.ToDecimal(result["GrEmpty"]),
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.lstPostPaymentChrgBreakup.Add(new ppgGRLPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),
                        });
                    }
                }
                ObjCS.GRECharge = ObjCS.lstChargs.Sum(x => x.Taxable);
                ObjCS.GRECGSTCharge = ObjCS.lstChargs.Sum(x => x.CGSTAmt);
                ObjCS.GRESGSTCharge = ObjCS.lstChargs.Sum(x => x.SGSTAmt);
                ObjCS.GREIGSTCharge = ObjCS.lstChargs.Sum(x => x.IGSTAmt);
                ObjCS.GRETotalAmount = ObjCS.GRECharge + ObjCS.GRECGSTCharge + ObjCS.GRESGSTCharge + ObjCS.GREIGSTCharge;

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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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
                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
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
                        ObjCS.INSOperationCFSCodeWiseAmtLst.Add(new INSOperationCFSCodeWiseAmtV2
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
                        ObjCS.INSContainerWiseAmtLst.Add(new INSContainerWiseAmtV2
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
                        ObjCS.LstppgShipDtl.Add(new PPG_ShippingBillNoV2
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


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSStartdate = result["StartDate"].ToString();
                        ObjCS.INSEndDate = result["EndDate"].ToString();
                        ObjCS.INSCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new ppgGRLPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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
                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
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
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtlV2
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
                        ObjCS.STOOperationCFSCodeWiseAmtLst.Add(new STOOperationCFSCodeWiseAmtV2
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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_ContainerStuffingV2
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
        internal void CalculateHandaling(string stuffingDate, String Origin, String Via, string ContainerStuffingXML, int HandalingPartyId, String CargoType)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.VarChar, Value = CargoType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateHandallingCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
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
                        ObjCS.HNDOperationCFSCodeWiseAmtLst.Add(new HNDOperationCFSCodeWiseAmtV2
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
                        ObjCS.HNDStartdate = result["StartDate"].ToString();
                        ObjCS.HNDEndDate = result["EndDate"].ToString();
                        ObjCS.HNDCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new ppgGRLPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.LongText, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "EXPCSGEN" });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGENSPCharge", CommandType.StoredProcedure, DParam);
            // DataSet Result = new DataSet();
            //Result = DataAccess.ExecuteDataSet("CalculateGENSPCharge", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();

            try
            {

                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
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
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtlV2
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
                        ObjCS.GENSPOperationCFSCodeWiseAmtLst.Add(new GENSPOperationCFSCodeWiseAmtV2
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
                        ObjCS.LstppgShipDtl.Add(new PPG_ShippingBillNoV2
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

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOStartdate = result["StartDate"].ToString();
                        ObjCS.STOEndDate = result["EndDate"].ToString();
                        ObjCS.STOCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new ppgGRLPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

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
        public void LoadPayeeList(int Page, string PartyCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
                IDataParameter[] DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("ContStuffPayeeListV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<dynamic> lstShiping = new List<dynamic>();
                bool State = false;
                Status = 1;
                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        lstShiping.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                    }
                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShiping, State };
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
        public void LoadPartyList(int Page, string PartyCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
                LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
                IDataParameter[] DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("ContStuffPartyListV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<dynamic> lstShiping = new List<dynamic>();
                bool State = false;
                Status = 1;
                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        lstShiping.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                    }
                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShiping, State };
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

        #region cargo shifting

        public void GetShippingLineForInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.PaymentPartyName> objPaymentPartyName = new List<Areas.Export.Models.PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Areas.Export.Models.PaymentPartyName()
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillsForCargoShiftingV2", CommandType.StoredProcedure, DParam);
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
        /*public void GetCargoShiftingPaymentSheet(string InvoiceDate, int PartyId, string ShipBillsXML, int InvoiceId, string TaxType, int PayeeId)
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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new ppgCargoPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
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
        }*/
        public void AddEditCargoShiftInvoice(int BranchId, int Uid, string CartingRgisterDtlXML, int FromGodownId, int ToGodownId, int ToShippingId, int ShiftingType, int FromShippingLineId, string ShiftingDate, string Remarks, int CargoShiftingId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            ShiftingDate = DateTime.ParseExact(ShiftingDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoShiftingId", MySqlDbType = MySqlDbType.Int32, Value = CargoShiftingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = ShiftingDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Module });

            //LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            //LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakXML });

            //LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            //LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            //LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
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
            int Result = DA.ExecuteNonQuery("AddEditInvoiceCargoShiftV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cargo Shifting Saved Successfully.";

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
        public void ListOfCargoshifting(int Page)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofCargoShiftV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Ppg_ListOfCargoShiftV2> objCargo = new List<Ppg_ListOfCargoShiftV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCargo.Add(new Ppg_ListOfCargoShiftV2()
                    {
                        CargoShiftingId=Convert.ToInt32(Result["CargoShiftingId"]),
                        ShiftingNo = Convert.ToString(Result["ShiftingNo"]),
                        ShiftingDt = Convert.ToString(Result["ShiftingDt"]),
                        FromShipping = Convert.ToString(Result["FromShipping"]),
                        ToShipping = Convert.ToString(Result["ToShipping"]),
                        FromGodown = Convert.ToString(Result["FromGodown"]),
                        ToGodown = Convert.ToString(Result["ToGodown"]),
                        ShiftingType = Convert.ToString(Result["ShiftingType"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCargo;
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
        public void CargoShiftingDet(int CargoShiftingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoShiftingId", MySqlDbType = MySqlDbType.Int32, Value = CargoShiftingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoShiftingDetV2", CommandType.StoredProcedure, Dparam);
            Ppg_CargoShifting objAppr = new Ppg_CargoShifting();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objAppr.CargoShiftingId = Convert.ToInt32(Result["CargoShiftingId"]);
                    objAppr.ShiftingNo = Convert.ToString(Result["ShiftingNo"]);
                    objAppr.ShiftingDt = Convert.ToString(Result["ShiftingDt"]);
                    objAppr.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objAppr.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    objAppr.GSTNo = Convert.ToString(Result["GSTNo"]);
                    objAppr.FromShippingId = Convert.ToInt32(Result["FromShippingId"]);
                    objAppr.FromShippingName = Convert.ToString(Result["FromShippingName"]);
                    objAppr.ToShippingId = Convert.ToInt32(Result["ToShippingId"]);
                    objAppr.ToShippingName = Convert.ToString(Result["ToShippingName"]);
                    objAppr.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    objAppr.FromGodownName = Convert.ToString(Result["FromGodownName"]);
                    objAppr.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    objAppr.ToGodownName = Convert.ToString(Result["ToGodownName"]);
                    objAppr.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objAppr.PayeeName = Convert.ToString(Result["PayeeName"]);
                    objAppr.ShiftingType = Convert.ToString(Result["ShiftingType"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objAppr.lstCartingRegisterDtl.Add(new Ppg_CartingRegisterDetail
                        {
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                            RegisterDate = Convert.ToString(Result["RegisterDate"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                            SQM = Convert.ToDecimal(Result["SQM"]),
                            ActualQty = Convert.ToInt32(Result["ActualQty"]),
                            ActualWeight = Convert.ToDecimal(Result["ActualWeight"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objAppr;
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

        #region Shipping line,CHA,exporter,Forwarder list and Party code wise search
        public void EximtraderlistPopulation(int Page, string PartyCode, int Exporter, int ShippingLine, int CHA, int ForwarderConsolidator)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.Int32, Value = Exporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.Int32, Value = ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHA", MySqlDbType = MySqlDbType.Int32, Value = CHA });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderConsolidator", MySqlDbType = MySqlDbType.Int32, Value = ForwarderConsolidator });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            DataSet Result = new DataSet();
            Result = DataAccess.ExecuteDataSet("GetEximtraderlistV2", CommandType.StoredProcedure, Dparam);

            List<dynamic> LstParty = new List<dynamic>();
            bool State = false;
            _DBResponse = new DatabaseResponse();
            try
            {
                Status = 1;
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    LstParty.Add(new { EximTraderId = Convert.ToInt32(dr["EximTraderId"]), EximTraderName = dr["EximTraderName"].ToString(), EximTraderAlias = dr["EximTraderAlias"].ToString() });
                }
                if (Result.Tables[1].Rows.Count > 0)
                {
                    State = Convert.ToBoolean(Result.Tables[1].Rows[0][0]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstParty, State };
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
            }
        }
        public void ShppingLineReservedGodown(int EximTraderId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = EximTraderId });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader result = DataAccess.ExecuteDataReader("GetReservDetOfShippinglineV2", CommandType.StoredProcedure, Dparam);

            int GodownId = 0;
            string GodownName = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    GodownId = Convert.ToInt32(result["GodownId"]);
                    GodownName = result["GodownName"].ToString();
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Sucess";
                _DBResponse.Data = new { GodownId, GodownName };

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        #region Stuffing Request
        public void AddEditStuffingRequest(PPG_StuffingRequest ObjStuffing, string StuffingXML, string StuffingContrXML)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingPlanId });
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
            List<PPG_StuffingRequest> LstStuffing = new List<PPG_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequest
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
            List<PPG_StuffingRequest> LstStuffing = new List<PPG_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequest
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
            List<PPG_StuffingReqContainerDtl> LstStuffing = new List<PPG_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingReqContainerDtl
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
            PPG_StuffingReqContainerDtl ObjStuffing = new PPG_StuffingReqContainerDtl();
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
            List<PPG_StuffingRequestDtl> LstBillingNo = new List<PPG_StuffingRequestDtl>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingBillNoOfCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBillingNo.Add(new PPG_StuffingRequestDtl
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
            PPG_StuffingReqContainerDtl ObjSR = new PPG_StuffingReqContainerDtl();
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
            PPG_StuffingRequest ObjStuffing = new PPG_StuffingRequest();
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
                    ObjStuffing.ShippingHdrLineId = Convert.ToInt32(Result["ShippingHdrLineId"]);
                    ObjStuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjStuffing.CHA = Result["CHA"].ToString();
                    ObjStuffing.Forwarder = Result["Forwarder"].ToString();
                    ObjStuffing.ShippingHdrLine = Result["ShippingHdrLine"].ToString();
                    //ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                    ObjStuffing.StuffingPlanNo = Result["PlanNo"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new PPG_StuffingRequestDtl
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
                            CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
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
                            CommodityName = Result["CommodityName"].ToString(),
                            OperationType= Result["OperationType"].ToString(),
                            PackUQCCode = Result["PackUQCCode"].ToString(),
                            PackUQCDescription = Result["PackUQCDesc"].ToString(),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new PPG_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            Fob=Convert.ToDecimal(Result["Fob"])
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
        public void GetCartRegDetForStuffingReq(int CartingRegisterId,string flag)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.VarChar, Value = flag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_StuffingRequestDtl> LstStuffing = new List<PPG_StuffingRequestDtl>();
            List<PPG_StuffingReqContainerDtl> LstStuffingContr = new List<PPG_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommInvNo = Result["CommInvNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
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
                        
                          PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                        PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        OperationType=Convert.ToString(Result["OperationType"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new PPG_StuffingReqContainerDtl
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
        public void ListofStuffingRequest(int RoleId, int Uid, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofStuffingRequestV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_StuffingRequest> LstStuffing = new List<PPG_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequest
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


        public void GetPackUQCForStuffingReq()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPackUQCForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PackUQCForPage> Lstpackuqc = new List<PackUQCForPage>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstpackuqc.Add(new PackUQCForPage
                    {
                        PackUQCCode = Result["PackUQCCode"].ToString(),
                        PackUQCDescription = Result["PackUQCDescription"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstpackuqc;
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
            IList<CwcExim.Areas.Export.Models.PaySheetStuffingRequest> objPaySheetStuffing = new List<CwcExim.Areas.Export.Models.PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new CwcExim.Areas.Export.Models.PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["CartingAppId"]),
                        StuffingReqNo = Convert.ToString(Result["CartingAppNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        CartShip = Convert.ToString(Result["CartShip"])
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
            IList<CwcExim.Areas.Export.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<CwcExim.Areas.Export.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new CwcExim.Areas.Export.Models.PaymentSheetContainer()
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

        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, int PartyId,string ContainerXML, int InvoiceId,string SEZ)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PpgInvoiceBTT objInvoice = new PpgInvoiceBTT();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTPaymentSheetV2", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new PpgPreInvoiceContainerBTT
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
                        objInvoice.lstPostPaymentCont.Add(new PpgPostPaymentContainerBTT
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
                        objInvoice.lstPostPaymentChrg.Add(new PpgPostPaymentChrgBTT
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
                        objInvoice.lstContWiseAmount.Add(new PpgContainerWiseAmountBTT
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmountBTT
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
                        objInvoice.lstPreInvoiceCargo.Add(new PpgPreInvoiceCargoBTT
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

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new ppgBTTPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
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

        public void AddEditBTTInvoice(PpgInvoiceBTT ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML
           , int BranchId, int Uid,
          string Module, string CargoXML = "",string SEZ="")
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

        #region Edit Container Movement

        public void GetPaymentPartyforEditMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
          //  LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Page) });
          //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(PartyCode) });
         //   IDataParameter[] DParam = { };
          //  DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExportV2EditMovement", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.PaymentPartyNameV2> objPaymentPartyName = new List<Areas.Export.Models.PaymentPartyNameV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Areas.Export.Models.PaymentPartyNameV2()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
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




        public void EditContainerMovementInvoice(Areas.Export.Models.PPG_MovementInvoice ObjPostPaymentSheet,
           string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
          int BranchId, int Uid,
         string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy-MM-dd HH:mm:ss") });
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
            int Result = DA.ExecuteNonQuery("EditInvoiceContainerMovement", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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


        public void GetContainerMovementInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerMovementInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = new Areas.Export.Models.PPG_MovementInvoice();
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
                        objPostPaymentSheet.ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]);
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
                        objPostPaymentSheet.ShippingId = Convert.ToInt32(Result["ShippingId"]);
                        
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                        objPostPaymentSheet.LocationId = Convert.ToInt32(Result["LocationId"]);
                        objPostPaymentSheet.MoveToId = Convert.ToInt32(Result["MoveToId"]);
                        objPostPaymentSheet.MoveTo = Convert.ToString(Result["MoveTo"]);
                        objPostPaymentSheet.TransportMode = Convert.ToString(Result["TransportMode"]);
                        objPostPaymentSheet.TareWeight = Convert.ToDecimal(Result["TareWeight"]);
                        objPostPaymentSheet.CargoWeight = Convert.ToDecimal(Result["CargoWeight"]);
                        objPostPaymentSheet.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                        objPostPaymentSheet.GatewayPortId = Convert.ToInt32(Result["GatewayPortId"]);
                        objPostPaymentSheet.CargoType = Convert.ToInt32(Result["CargoType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Export.Models.PpgPostPaymentChrg()
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
                        objPostPaymentSheet.lstPostPaymentCont.Add(new Areas.Export.Models.PpgPostPaymentContainer
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
                            LCLFCL = Convert.ToString(Result["LCLFCL"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new Areas.Export.Models.PpgContainerWiseAmount
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
                            LineNo = Result["LineNo"].ToString() 
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Export.Models.PpgOperationCFSCodeWiseAmount
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

        #region Edit Container Stuffing

        public void GetPaymentPartyforEditStuffing()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Page) });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(PartyCode) });
            //   IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExportV2EditStuffing", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.PaymentPartyNameV2> objPaymentPartyName = new List<Areas.Export.Models.PaymentPartyNameV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Areas.Export.Models.PaymentPartyNameV2()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
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





        public void GetContainerStuffingInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = new Areas.Export.Models.PPG_MovementInvoice();
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
                        objPostPaymentSheet.ContainerStuffingId = Convert.ToInt32(Result["StuffReqId"]);
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
                     //   objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                      //  objPostPaymentSheet.ShippingId = Convert.ToInt32(Result["ShippingId"]);

                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                      //  objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                      //  objPostPaymentSheet.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                       // objPostPaymentSheet.LocationId = Convert.ToInt32(Result["LocationId"]);
                       // objPostPaymentSheet.MoveToId = Convert.ToInt32(Result["MoveToId"]);
                       // objPostPaymentSheet.MoveTo = Convert.ToString(Result["MoveTo"]);
                       // objPostPaymentSheet.TransportMode = Convert.ToString(Result["TransportMode"]);
                       // objPostPaymentSheet.TareWeight = Convert.ToDecimal(Result["TareWeight"]);
                      //  objPostPaymentSheet.CargoWeight = Convert.ToDecimal(Result["CargoWeight"]);
                        objPostPaymentSheet.PaymentMode = Convert.ToString(Result["PaymentMode"]);
// objPostPaymentSheet.GatewayPortId = Convert.ToInt32(Result["GatewayPortId"]);
// objPostPaymentSheet.CargoType = Convert.ToInt32(Result["CargoType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Export.Models.PpgPostPaymentChrg()
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
                        objPostPaymentSheet.lstPostPaymentCont.Add(new Areas.Export.Models.PpgPostPaymentContainer
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
                            LCLFCL = Convert.ToString(Result["LCLFCL"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new Areas.Export.Models.PpgContainerWiseAmount
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
                            LineNo = Result["LineNo"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Export.Models.PpgOperationCFSCodeWiseAmount
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
                        objPostPaymentSheet.LstStuffingDtl.Add(new ContainerStuffingDtlV2
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
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
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"])
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


        public void CalculateGroundRentEmptyForEdit(String StuffingDate, String ContainerStuffingXML, int GREPartyId, int StuffingReqId,int Invoiceid)
        {
            int Status = 0;
            StuffingDate = DateTime.ParseExact(StuffingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GREPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = Invoiceid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRentEmptyV2ForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.lstChargs.Add(new Ppg_ContStuffChargesV2
                    {
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        ChargeType = Convert.ToString(result["ChargeType"]),
                        ChargeName = Convert.ToString(result["ChargeName"]),
                        CGSTAmt = Convert.ToDecimal(result["CGSTAmt"]),
                        SGSTAmt = Convert.ToDecimal(result["SGSTAmt"]),
                        IGSTAmt = Convert.ToDecimal(result["IGSTAmt"]),
                        IGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString()),
                        CGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString()),
                        SGSTPer = Convert.ToDecimal(result["SGSTPer"]),
                        Taxable = Convert.ToDecimal(result["Taxable"]),
                        Total = Convert.ToDecimal(result["Total"]),
                        SACCode = Convert.ToString(result["SACCode"])
                    });
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREOperationCFSCodeWiseAmtLst.Add(new GREOperationCFSCodeWiseAmtV2
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
                        ObjCS.GREContainerWiseAmtLst.Add(new GREContainerWiseAmtV2
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            GrEmpty = Convert.ToDecimal(result["GrEmpty"]),
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.lstPostPaymentChrgBreakup.Add(new ppgGRLPostPaymentChargebreakupdateV2
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),
                        });
                    }
                }
                ObjCS.GRECharge = ObjCS.lstChargs.Sum(x => x.Taxable);
                ObjCS.GRECGSTCharge = ObjCS.lstChargs.Sum(x => x.CGSTAmt);
                ObjCS.GRESGSTCharge = ObjCS.lstChargs.Sum(x => x.SGSTAmt);
                ObjCS.GREIGSTCharge = ObjCS.lstChargs.Sum(x => x.IGSTAmt);
                ObjCS.GRETotalAmount = ObjCS.GRECharge + ObjCS.GRECGSTCharge + ObjCS.GRESGSTCharge + ObjCS.GREIGSTCharge;

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


        public void AddEditContainerStuffingForEdit(PPG_ContainerStuffingV2 ObjStuffing, string ContainerStuffingXML, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
    string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML, string ChargesXML, string BreakUpdateXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = SCMTRXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GRECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRECFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDCFSCode });
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOStartdate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOCFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CargoTypeId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BreakUpdateXML", MySqlDbType = MySqlDbType.Text, Value = BreakUpdateXML });
            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffingV2ForEdit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            PPG_InvoiceCSListV2 inv = new PPG_InvoiceCSListV2();
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
                    _DBResponse.Data = inv;
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

        public void EditContainerStuffInvoice(Areas.Export.Models.PPG_MovementInvoice ObjPostPaymentSheet,
          string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
         int BranchId, int Uid,
        string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy-MM-dd HH:mm:ss") });
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
            int Result = DA.ExecuteNonQuery("EditInvoiceContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        #region BTT Payment Sheet Edit 3.2
        public void GetBTTInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal hours = 0;
            Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = new Areas.Import.Models.PpgInvoiceYard();
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
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
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
                        objPostPaymentSheet.DirectDestuffing = Convert.ToString(Result["IsDirect"]);
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
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Import.Models.PpgPostPaymentChrg()
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
                        objPostPaymentSheet.lstPostPaymentCont.Add(new Areas.Import.Models.PpgPostPaymentContainer()
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
                            Duty = Convert.ToDecimal(Result["Duty"])

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new Areas.Import.Models.PpgContainerWiseAmount()
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
                            LineNo = Convert.ToString(Result["LineNo"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Import.Models.PpgOperationCFSCodeWiseAmount
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

         
        #region Export Destuffing




        public void GetContainersForExpDestuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportDestuffing", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Ppg_ExportDestuffingContainer> objPaySheetStuffing = new List<Ppg_ExportDestuffingContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ExportDestuffingContainer()
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerStuffingId=Convert.ToInt32(Result["ContainerStuffingId"]),
                        Size= Convert.ToString(Result["Size"]),
                         GodownId= Convert.ToInt32(Result["GodownId"]),
                         GodownName= Convert.ToString(Result["GodownName"]),
                          ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        OperationType= Convert.ToString(Result["OperationType"]),




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

        public void GetContainersForExpDestuffingSpcl()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportDestuffingSpcl", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Ppg_ExportDestuffingContainer> objPaySheetStuffing = new List<Ppg_ExportDestuffingContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ExportDestuffingContainer()
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Convert.ToString(Result["Size"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLine = Convert.ToString(Result["ShippingLine"]),
                        RefNo = Convert.ToString(Result["RefNo"]),
                        RefDate = Convert.ToString(Result["RefDate"]),
                        SType = Convert.ToString(Result["SType"]),
                        GodownId = Result["GodownId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"] == System.DBNull.Value ? "" : Convert.ToString(Result["GodownName"])

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

        public void GetSBDetForExpDestuffing(string CFSCode,string OperationType)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBDetForExportDestuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Ppg_ExportDestuffDetails> objPaySheetStuffing = new List<Ppg_ExportDestuffDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ExportDestuffDetails()
                    {
                        SBNo = Convert.ToString(Result["ShippingBillNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        EXPId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        cargoDesc = Convert.ToString(Result["CargoDesc"]),
                        CargoType = Convert.ToString(Result["CargoType"]),
                        GrWt = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                        CUM = Result["CUM"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CUM"]),
                        Unit = Result["Unit"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Unit"]),
                        FOB = Result["FOB"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["FOB"]),
                        UnReservedSQM = Result["SQM"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SQM"]),
                        CommodityId = Result["CommodityId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CommodityId"]),
                        Commodity = Convert.ToString(Result["CommodityName"]),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        CHAId= Convert.ToInt32(Result["CHAId"]),


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

        public void GetSBDetForExpDestuffingSpcl(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBDetForExportDestuffingSpcl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Ppg_ExportDestuffDetails> objPaySheetStuffing = new List<Ppg_ExportDestuffDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ExportDestuffDetails()
                    {
                        SBNo = Convert.ToString(Result["ShippingBillNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        EXPId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        cargoDesc = Convert.ToString(Result["CargoDesc"]),
                        CargoType = Convert.ToString(Result["CargoType"]),
                        GrWt = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                        CUM = Result["CUM"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CUM"]),
                        Unit = Result["Unit"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Unit"]),
                        FOB = Result["FOB"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["FOB"]),
                        UnReservedSQM = Result["SQM"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SQM"]),
                        CommodityId = Result["CommodityId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CommodityId"]),
                        Commodity = Convert.ToString(Result["CommodityName"])
                      


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

        public void AddEditExpDestuffingEntry(Ppg_ExportDestuffing ObjDestuffing, string DestuffingEntryXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.Destuffingdate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo == null ? null : ObjDestuffing.ContainerNo.ToUpper() });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SpaceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.RefNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditExportDestuffingEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingId == 0 ? "Destuffing Entry Details Saved Successfully" : "Destuffing Entry Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
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

        public void AddEditExpDestuffingEntrySpl(Ppg_ExportDestuffing ObjDestuffing, string DestuffingEntryXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.Destuffingdate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo == null ? null : ObjDestuffing.ContainerNo.ToUpper() });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SpaceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.RefNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditExportDestuffingEntrySpl", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingId == 0 ? "Destuffing Entry Details Saved Successfully" : "Destuffing Entry Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
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

        public void GetAllDestuffingEntry(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfExpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Ppg_ExportDestuffingList
                    {
                        DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString()
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



        public void DestuffingEntrySr(string searchtext)
        {
            // DataSet Result = new DataSet();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = searchtext });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfExpDestuffingEntrySerch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Ppg_ExportDestuffingList
                    {
                        DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString()
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

        public void GetAllDestuffingEntrySplserch(string searchtext)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = searchtext });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfExpDestuffingEntrySplSerch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Ppg_ExportDestuffingList
                    {
                        DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString()
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


        public void GetAllDestuffingEntrySpl(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfExpDestuffingEntrySpl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new Ppg_ExportDestuffingList
                    {
                        DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString()
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
        public void GetDestuffingEntryDetailsById(int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("EditViewExportDestuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Ppg_ExportDestuffing objDestuffing = new Ppg_ExportDestuffing();
            IList<Ppg_ExportDestuffDetails> lstDestuffing = new List<Ppg_ExportDestuffDetails>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToInt32(Result["Size"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.DestuffingNo = Convert.ToString(Result["DestuffingEntryNo"]);
                    objDestuffing.Destuffingdate = Convert.ToString(Result["DestuffingEntryDate"]);
                    objDestuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objDestuffing.GodownName = Convert.ToString(Result["GodownName"]);
                    objDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippinglineId"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLine"].ToString());
                    objDestuffing.Remarks = Convert.ToString(Result["Remarks"]);
                    objDestuffing.SpaceType = Convert.ToString(Result["Spacetype"].ToString());
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstDestuffing.Add(new Ppg_ExportDestuffDetails
                        {
                            DestuffingDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            SBNo = Convert.ToString(Result["SBNo"]),
                            SBDate = Convert.ToString(Result["SBDate"]),
                            EXPId = Convert.ToInt32(Result["ExpId"]),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            cargoDesc = Convert.ToString(Result["CargoDescription"]),
                            Unit = Convert.ToInt32(Result["NoOfPackages"]),
                            GrWt = Convert.ToDecimal(Result["GrossWeight"]),
                            CUM = Convert.ToDecimal(Result["CUM"]),
                            FOB = Convert.ToDecimal(Result["FOB"]),
                            ReservedSQM = Convert.ToDecimal(Result["ReservedArea"]),
                            CargoType = Convert.ToString(Result["CargoType"]),
                            UnReservedSQM = Convert.ToInt32(Result["UnReservedArea"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["CommodityName"]),
                            LocationId = Result["GodownWiseLocationIds"].ToString(),
                            Location = Convert.ToString(Result["GodownWiseLctnNames"]),
                            CHAId = Convert.ToInt32(Result["CHAId"])
                        });
                    }
                }
                if (lstDestuffing.Count > 0)
                {
                    objDestuffing.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstDestuffing);
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


        public void GetDestuffingEntryDetailsByIdSpl(int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("EditViewExportDestuffingSpl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Ppg_ExportDestuffing objDestuffing = new Ppg_ExportDestuffing();
            IList<Ppg_ExportDestuffDetails> lstDestuffing = new List<Ppg_ExportDestuffDetails>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.DestuffingId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToInt32(Result["Size"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.DestuffingNo = Convert.ToString(Result["DestuffingEntryNo"]);
                    objDestuffing.Destuffingdate = Convert.ToString(Result["DestuffingEntryDate"]);
                    objDestuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objDestuffing.GodownName = Convert.ToString(Result["GodownName"]);
                    objDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippinglineId"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLine"].ToString());
                    objDestuffing.Remarks = Convert.ToString(Result["Remarks"]);
                    objDestuffing.SpaceType = Convert.ToString(Result["Spacetype"].ToString());
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstDestuffing.Add(new Ppg_ExportDestuffDetails
                        {
                            DestuffingDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            SBNo = Convert.ToString(Result["OldSBNo"]),
                            NewSpCCIN = Convert.ToString(Result["SBNo"]),
                            SBDate = Convert.ToString(Result["SBDate"]),
                            EXPId = Convert.ToInt32(Result["ExpId"]),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            cargoDesc = Convert.ToString(Result["CargoDescription"]),
                            Unit = Convert.ToInt32(Result["NoOfPackages"]),
                            GrWt = Convert.ToDecimal(Result["GrossWeight"]),
                            CUM = Convert.ToDecimal(Result["CUM"]),
                            FOB = Convert.ToDecimal(Result["FOB"]),
                            ReservedSQM = Convert.ToDecimal(Result["ReservedArea"]),
                            CargoType = Convert.ToString(Result["CargoType"]),
                            UnReservedSQM = Convert.ToInt32(Result["UnReservedArea"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["CommodityName"]),
                            LocationId = Result["GodownWiseLocationIds"].ToString(),
                            Location = Convert.ToString(Result["GodownWiseLctnNames"])
                        });
                    }
                }
                if (lstDestuffing.Count > 0)
                {
                    objDestuffing.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstDestuffing);
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

        public void DelDestuffingEntry(int DestuffingEntryId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelExportDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Entry Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Destuffing Entry Details As It Exist In Another Page";
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

        public void GetExportDestuffingForPrint(int DestuffingEntryId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("PrintExportDestuffing", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
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

        public void GetExportDestuffingForPrintSpl(int DestuffingEntryId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("PrintExportDestuffingSpl", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
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
        #region Container Delivery
        public void GetContainerNoforContDelivery()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_PortCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PortCode });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetcontainerforDeliveryInformation", CommandType.StoredProcedure);
            IList<Ppg_containerList> lstContainer = new List<Ppg_containerList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                // bool StatePortOfCall = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new Ppg_containerList
                    {

                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        GatePassId = Convert.ToInt32(Result["GatePassId"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstContainer };
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
        public void GetContainerNoforContDeliveryDetails(string GatePassId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = GatePassId });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerdelivertdetails", CommandType.StoredProcedure, Dparam);
            Ppg_ContainerDeliverySystem Container = new Ppg_ContainerDeliverySystem();
            _DBResponse = new DatabaseResponse();
            try
            {
                // bool StatePortOfCall = false;
                while (Result.Read())
                {
                    Status = 1;


                    Container.GatePassNo = Convert.ToString(Result["GatePassNo"]);
                    Container.GatePassDate = Convert.ToString(Result["GatePassDate"]);
                    Container.GatePassId = Convert.ToInt32(Result["GatePassId"]);
                    Container.PartyName = Convert.ToString(Result["PartyName"]);
                    Container.PartyId = Convert.ToString(Result["PartyId"]);

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { Container };
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
        public void AddEditContainerDelivery(Ppg_ContainerDeliverySystem objPortOfCall, int Uid)
        {
            string dtASON = Convert.ToDateTime(objPortOfCall.ActualTimeOfArrival).ToString("yyyy-MM-dd HH:mm:ss");
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.GatePassId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.GatePassNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPortOfCall.GatePassDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.PartyId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PartyName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_DateofArrival", MySqlDbType = MySqlDbType.DateTime, Value = dtASON });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.ContainerNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.CFSCode });

            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditContainerDelivery", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Container Delivery Save Successfully" : "Stuffing Approval Updated Successfully";

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
        public void ListofContainerDelivery(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContainerDeliverySystem> LstContainerDelivery = new List<Ppg_ContainerDeliverySystem>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerDelivery.Add(new Ppg_ContainerDeliverySystem
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ActualTimeOfArrival = Result["DateofArrival"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerDelivery;
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
        public void GetAllContainerContainerDeliverySearch(string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofContainerDeliverySearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_ContainerDeliverySystem> LstContainerDelivery = new List<Ppg_ContainerDeliverySystem>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerDelivery.Add(new Ppg_ContainerDeliverySystem
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ActualTimeOfArrival = Result["DateofArrival"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerDelivery;
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
        public void GetContainerDeliveryId(int Id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewContainerDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Ppg_ContainerDeliverySystem objDestuffing = new Ppg_ContainerDeliverySystem();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Id = Convert.ToInt32(Result["Id"]);
                    objDestuffing.GatePassId = Convert.ToInt32(Result["GatePassId"]);
                    objDestuffing.GatePassDate = Result["GatePassDate"].ToString();
                    objDestuffing.GatePassNo = Result["GatePassNo"].ToString();
                    objDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    objDestuffing.CFSCode = Result["CFSCode"].ToString();
                    objDestuffing.PartyName = Result["PartyName"].ToString();
                    objDestuffing.ActualTimeOfArrival = Result["DateofArrival"].ToString();
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
            IList<Ppg_ContainerStuffingApproval> objPaySheetStuffing = new List<Ppg_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ContainerStuffingApproval()
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
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value =Convert.ToInt32(objPortOfCall.ApprovalId) });
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
                if (result == 1|| result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Stuffing Approved Successfully" : "Stuffing Approval Updated Successfully";

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
                        CFSCode = Result["CFSCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["ContainerStuffingId"])
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
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]); 

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


        public void GetAmendmentContainerStuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentContainerStuffingV2", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                     
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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

        public void GetCIMSFDetails(int ContainerStuffingId,string MsgType)
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
             Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRStuffingDetailsV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

          


            try
            {

                int count = Result.Tables.Count;
                if (count == 1)
                {
                    if(Convert.ToInt32(Result.Tables[0].Rows[0]["Result"])==1)
                    {
                        _DBResponse.Status = 2;
                        _DBResponse.Message = "CIM SF Message Already Send.";
                        _DBResponse.Data = Result;
                    }
                    else
                    {
                        _DBResponse.Status = 3;
                        _DBResponse.Message = "CIM SF Acknowledgement Received Successfully Please Do Amendment";
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

        public void GetAmenCIMSFDetailsUpdateStatus(int ContainerStuffingId)
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
            IList<Ppg_ContainerStuffingApproval> objPaySheetStuffing = new List<Ppg_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ContainerStuffingApproval()
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

        public void ListofLoadContainerStuffingApproval(int Page, string SearchValue="")
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

        #region Get CIM-ASR Details

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
        public void AddEditActualArrivalDatetime(Ppg_ActualArrivalDatetime objActualArrivalDatetime)
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
            IList<Ppg_ActualArrivalDatetime> objArrivalDatetimeList = new List<Ppg_ActualArrivalDatetime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objArrivalDatetimeList.Add(new Ppg_ActualArrivalDatetime()
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

        #region Get Loaded CIM-SF Details

        public void GetLoadedCIMSFDetails(int ContainerStuffingId, string MsgType)
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
            Result = DataAccess.ExecuteDataSet("ICES_GetLoadedSCMTRStuffingDetailsV2", CommandType.StoredProcedure, DParam);
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
                        _DBResponse.Message = "CIM SF Acknowledgement Received Successfully Please Do Amendment";
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

        public void GetLoadedCIMSFDetailsUpdateStatus(int ContainerStuffingId)
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
            Result = DataAccess.ExecuteDataSet("UpdateLoadedCIMSFStatus", CommandType.StoredProcedure, DParam);
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
            IList<Ppg_ContainerStuffingApproval> objPaySheetStuffing = new List<Ppg_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Ppg_ContainerStuffingApproval()
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


        public void AddEditLoadContainerStuffingSF(Ppg_LoadContSF objPortOfCall, int Uid)
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
            List<Ppg_LoadContSF> LstStuffingApproval = new List<Ppg_LoadContSF>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new Ppg_LoadContSF
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
            Ppg_LoadContSF objDestuffing = new Ppg_LoadContSF();
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

        #region Stuffing Plan

        public void GetCartRegNoForStuffingPlan(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();           
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegNoForStuffingPlan", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_StuffingPlanRequest> LstStuffing = new List<PPG_StuffingPlanRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingPlanRequest
                    {
                        CartingRegisterNo = Result["ShippingBillNo"].ToString(),
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

        public void GetCartRegDetForStuffingPlan(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.VarChar, Value = flag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingPlan", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_StuffingRequestDtl> LstStuffing = new List<PPG_StuffingRequestDtl>();
            List<PPG_StuffingReqContainerDtl> LstStuffingContr = new List<PPG_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommInvNo = Result["CommInvNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
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

                        PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                        PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        OperationType = Convert.ToString(Result["OperationType"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new PPG_StuffingReqContainerDtl
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

        public void AddEditStuffingPlan(Ppg_StuffingPlan ObjStuffing, string StuffingXMLL)
        {
            var stuffingdate = (dynamic)null;

            if (ObjStuffing.StuffingPlanDate != null && ObjStuffing.StuffingPlanDate != "")
            {
                stuffingdate = DateTime.ParseExact(ObjStuffing.StuffingPlanDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingPlanId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingdate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.LongText, Value = StuffingXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();

            //log.Info("Before SP AddEditStuffingPlan call");

            int Result = DA.ExecuteNonQuery("AddEditStuffingPlan", CommandType.StoredProcedure, DParam, out GeneratedClientId);


            //log.Info("After SP AddEditStuffingPlan call");
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.StuffingPlanId == 0 ? "Stuffing Plan Details Saved Successfully" : "Stuffing Plan Details Updated Successfully");
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Plan Details Already Exist";
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

                //log.Error("Err :" + ex.Message + "\r\n" + ex.StackTrace);
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }


        public void GetAllStuffingPlan(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
           
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
           
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingPlanList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_StuffingPlan> LstStuffing = new List<Ppg_StuffingPlan>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Ppg_StuffingPlan
                    {
                        StuffingPlanNo = Convert.ToString(Result["PlanNo"]),
                        StuffingPlanDate = Convert.ToString(Result["PlanDate"]),
                        StuffingPlanId = Convert.ToInt32(Result["PlanID"]),
                        SBNo = Convert.ToString(Result["ShippingBillNo"]),
                        SBDate = Convert.ToString(Result["ShippingDate"]),
                        isSubmit = Convert.ToInt32(Result["IsSubmit"]),
                        ExporterName = Convert.ToString(Result["Exporter"]),
                        CHAName = Convert.ToString(Result["CHA"]),
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

        public void AddEditStuffingPlanSubmit(int StuffingPlanId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = StuffingPlanId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditStuffingPlanSubmit", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Plan Details Submit Successfully";
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

        public void EditStuffingPlan(int StuffingPlanId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = StuffingPlanId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingPlanDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_StuffingPlan LstStuffing = new Ppg_StuffingPlan();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstStuffing.StuffingPlanNo = Convert.ToString(Result["PlanNo"]);
                    LstStuffing.StuffingPlanDate = Convert.ToString(Result["PlanDate"]);
                    LstStuffing.StuffingPlanId = Convert.ToInt32(Result["id"]);

                }
                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        LstStuffing.lstStuffingPlan.Add(new Ppg_StuffingPlanDtl
                        {
                            PlanID = Convert.ToInt32(Result["PlanID"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingDate = Convert.ToString(Result["ShippingDate"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHA = Convert.ToString(Result["CHA"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                            GrossWeight = Convert.ToInt32(Result["GrossWeight"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),                           

                        });
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


        public void DeleteStuffingPlanSubmit(int StuffingPlanId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = StuffingPlanId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("DeleteStuffingPlanSubmit", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Plan Details Delete Successfully";
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

        public void GetAllStuffingPlanForRequest()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingPlanForRequest", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<Ppg_StuffingPlan> LstStuffing = new List<Ppg_StuffingPlan>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Ppg_StuffingPlan
                    {
                        StuffingPlanNo = Convert.ToString(Result["PlanNo"]),
                        StuffingPlanId = Convert.ToInt32(Result["PlanID"]),


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

        public void GetAllStuffingPlanDetailsForRequest(int StuffingPlanId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingPlanId", MySqlDbType = MySqlDbType.Int32, Value = StuffingPlanId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetailsStuffingPlan", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPG_StuffingRequestDtl> LstStuffing = new List<PPG_StuffingRequestDtl>();
            List<PPG_StuffingReqContainerDtl> LstStuffingContr = new List<PPG_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommInvNo = Result["CommInvNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
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
                        PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                        PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        OperationType = Convert.ToString(Result["OperationType"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new PPG_StuffingReqContainerDtl
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
        #endregion

        #region CCIN For Secondary User

        public void ListOfPackUQCForPageForSecondaryUser(string PartyCode, int Page = 0)
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
        public void GetAllCountryForSecondaryUser()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Country> LstCountry = new List<Country>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCountry.Add(new Country
                    {
                        CountryName = Result["CountryName"].ToString(),
                        CountryAlias = (Result["CountryAlias"] == null ? "" : Result["CountryAlias"]).ToString(),
                        CountryId = Convert.ToInt32(Result["CountryId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCountry;
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

        public void GetCountryForSecondaryUser(int CountryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CountryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Country ObjCountry = new Country();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCountry.CountryName = Result["CountryName"].ToString();
                    ObjCountry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjCountry.CountryAlias = (Result["CountryAlias"] == null ? "" : Result["CountryAlias"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCountry;
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
        public void ListOfExporterForSecondaryUser()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
            IList<Exporter> lstExporter = new List<Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Exporter
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
        public void GetShippingLineForSecondaryUser()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.ShippingLine> LstShippingLine = new List<Areas.Export.Models.ShippingLine>();
            // ShippingLine LstShippingLine = new ShippingLine();
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
        public void GetAllCommodityForSecondaryUser()
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
        public void GetAllCommodityForPageForSecondaryUser(string PartyCode, int Page = 0)
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
        public void ListOfCHAForSecondaryUser()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<Areas.Export.Models.CHA> lstCHA = new List<Areas.Export.Models.CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Areas.Export.Models.CHA
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
        public void GetPortOfLoadingForSecondaryUser()
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
        public void GetSBListForSecondaryUser()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBListV2", CommandType.StoredProcedure);
            List<CCINEntryV2> LstSB = new List<CCINEntryV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CCINEntryV2
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

        public void GetSBDetailsBySBIdForSecondaryUser(int SBId)
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
                Result = DataAccess.ExecuteDataSet("GetSBDetailsBySBIdV2", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                CCINEntryV2 objCCINEntry = new CCINEntryV2();

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

        public void AddEditCCINEntryForSecondaryUser(CCINEntryV2 objCCINEntry)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoadingName", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PortOfLoadingName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischarge", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PortOfDischarge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Package", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Package });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.Weight });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.FOB });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CommodityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = objCCINEntry.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.CargoTypeID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IP", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.IP });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PackageType });



            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.Int32, Value = (objCCINEntry.IsSEZ == true ? 1 : 0) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("addeditccinentryV2", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Message = "Can not update it is already Approved.";
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

        public void AddEditCCINEntryApprovalForSecondaryUser(CCINEntryV2 objCCINEntry)
        {

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExiamAppID", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.EximappID });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = objCCINEntry.PaymentSheetModelJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.CargoType) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.Approved) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PackageType });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsSEZ", MySqlDbType = MySqlDbType.Int32, Value = (objCCINEntry.IsSEZ == true ? 1 : 0) });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("addeditccinentryapporvalV2", CommandType.StoredProcedure, DParam);
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

        public void GetAllCCINEntryForSecondaryUser(string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
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

        public void GetAllCCINEntryForPageForSecondaryUser(int Page, int UID, string SearchValue = "", string SearchCCIN = "", string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();

                lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
                lstParam.Add(new MySqlParameter { ParameterName = "in_SearchCCIN", MySqlDbType = MySqlDbType.VarChar, Value = SearchCCIN });
                lstParam.Add(new MySqlParameter { ParameterName = "in_UID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UID });

                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntrySecondaryUserForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                //GetAllCCINEntryForPageV2
                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
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

        public void GetAllCCINEntryApprovalForPageForSecondaryUser(int Page)
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
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryApprovalForPageV2", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<CCINEntryV2> CCINEntryList = new List<CCINEntryV2>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        CCINEntryV2 objCCINEntry = new CCINEntryV2();
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

        public void GetCCINPartyListForSecondaryUser()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINPartyListV2", CommandType.StoredProcedure);
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


        public void GetCCINChargesForSecondaryUser(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
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

                CCINEntryV2 objCCINEntry = new CCINEntryV2();
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

        public void GetCCINEntryByIdForSecondaryUser(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("getccinentrybyidV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            try
            {

                while (Result.Read())
                {
                    objCCINEntry.nonApproval = Convert.ToString(Result["nonApproval"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                        objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
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
                        objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                        objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                        objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                        objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                        objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                        objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                        objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                        objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                        objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                        objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                        objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                        objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
                        objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                        objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDescription"]);
                        objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);
                    }
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


        public void GetCCINEntryForEditForSecondaryUser(int Id, int PartyId)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINDetForEditV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                    objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
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
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.CargoTypeID = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                    objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                    objCCINEntry.PaymentMode = Convert.ToString(Result["PaymentMode"]);
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);

                    objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                    objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDescription"]);
                    objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);
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

        public void GetCargoDetBTTByIdForSecondaryUser(int Id)
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
            PPGBTTCargoDetV2 objBTTCargo = new PPGBTTCargoDetV2();
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

        public void GetCargoDetShiftByIdForSecondaryUser(string Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.VarChar, Value = Id });
            // /   lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetShiftById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<PPGBTTCargoDetV2> SbNoList = new List<PPGBTTCargoDetV2>();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    PPGBTTCargoDetV2 objBTTCargo = new PPGBTTCargoDetV2();

                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["Weight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["FobValue"]);
                    objBTTCargo.exporter = Convert.ToString(Result["Exporter"]);
                    SbNoList.Add(objBTTCargo);
                }


                if (Status == 1)
                {
                    _DBResponse.Data = SbNoList;
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
        public void DeleteCCINEntryForSecondaryUser(int CCINEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINEntryId", MySqlDbType = MySqlDbType.Int32, Value = CCINEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCCINEntryV2", CommandType.StoredProcedure, Dparam);
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
        public void GetCCINShippingLineForSecondaryUser()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINShippingLineV2", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.ShippingLine> LstShippingLine = new List<Areas.Export.Models.ShippingLine>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new Areas.Export.Models.ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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



        public void GetMstGodownListForSecondaryUser()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["uid"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownV2", CommandType.StoredProcedure, DParam);
            List<Areas.Export.Models.GodownList> LstGodown = new List<Areas.Export.Models.GodownList>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Export.Models.GodownList
                    {

                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void PrintCCINEntryForSecondaryUser(int Id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("PrintCCINV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
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
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CountryName = Result["CountryName"].ToString();
                    objCCINEntry.PackageType = Result["PackageType"].ToString();
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
        public void PrintCCINEntryApprovalForSecondaryUser(int Id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("PrintCCINEntryApprovalV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
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
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CountryName = Result["CountryName"].ToString();
                    objCCINEntry.ApprovedBy = Result["ApprovedBy"].ToString();
                    objCCINEntry.ApprovedOn = Result["ApprovedOn"].ToString();
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
        #endregion
    }
}