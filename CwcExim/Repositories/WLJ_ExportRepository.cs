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
using System.Collections.Generic;
using System;
using System.Linq;

namespace CwcExim.Repositories
{
    public class WLJ_ExportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void ListOfExporterForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExporterForPage", CommandType.StoredProcedure, Dparam);
            IList<ExporterForPage> lstExp = new List<ExporterForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstExp.Add(new ExporterForPage
                    {
                        ExporterId = Convert.ToInt32(Result["ExpId"]),
                        ExporterName = Result["ExporterName"].ToString(),
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
                    _DBResponse.Data = new { lstExp, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
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

        public void ListOfChaForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForPage", CommandType.StoredProcedure, Dparam);
            IList<CwcExim.Areas.Import.Models.CHAForPage> lstCHA = new List<CwcExim.Areas.Import.Models.CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CwcExim.Areas.Import.Models.CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
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
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<CwcExim.Areas.Export.Models.CHA> lstCHA = new List<CwcExim.Areas.Export.Models.CHA>();
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
            IList<CwcExim.Areas.Export.Models.Exporter> lstExporter = new List<CwcExim.Areas.Export.Models.Exporter>();
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

        #region CCIN Entry

        public void GetCCINShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINShippingLine", CommandType.StoredProcedure, DParam);
            List<ShippingLine> LstShippingLine = new List<ShippingLine>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new ShippingLine
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
        public void GetCCINShippingLineForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page }); IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINShippingLineForPage", CommandType.StoredProcedure, DParam);
            List<Wlj_ShippingLineForPage> LstShippingLine = new List<Wlj_ShippingLineForPage>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new Wlj_ShippingLineForPage
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        PartyCode = Convert.ToString(Result["PartyCode"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),

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
                    _DBResponse.Data = new { LstShippingLine, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetVehicleForCCIN()
        {

            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetVehicleForCCIN", CommandType.StoredProcedure);
            List<Wlj_CCINEntry> LstSB = new List<Wlj_CCINEntry>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Wlj_CCINEntry
                    {
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        VehicleNo = Convert.ToString(Result["VehicleNo"]),
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

        public void GetSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBList", CommandType.StoredProcedure);
            List<Wlj_CCINEntry> LstSB = new List<Wlj_CCINEntry>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Wlj_CCINEntry
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
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();

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
                    objCCINEntry.Others = Convert.ToString(Result["Other"]);
                    //  objCCINEntry.PKType = Convert.ToInt32(Result["PackageType"]);
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
                    objCCINEntry.PrevEntryId = Convert.ToInt32(Result["EntryId"]);
                    objCCINEntry.EntryId = Convert.ToInt32(Result["EntryId"]);
                    objCCINEntry.VehicleNo = Convert.ToString(Result["VehicleNo"]);
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
                    objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]);
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
        public void GetPortOfLoadingForCCIN(int CountryId)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = CountryId });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoadingForCCIN", CommandType.StoredProcedure, dpram);
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

                Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();

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
        public void GetCCINByGateEntryId(int EntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINByGateEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLine"]);
                    objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCCINEntry.CHAName = Convert.ToString(Result["CHA"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["NoOfPkg"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["GrWt"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);


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

                Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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
        public void AddEditCCINEntry(Wlj_CCINEntry objCCINEntry)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.SBNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.SBDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Others", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.Others });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_PKType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PKType });
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_PrevEntryId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PrevEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Vehicle", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.VehicleNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PackageType });
            /* lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
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
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Update CCIN ENTRY As Next Step Done";
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
        public void GetCCINEntry(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            // lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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
                    objCCINEntry.Others = Convert.ToString(Result["Other"]);
                    //  objCCINEntry.PKType = Convert.ToInt32(Result["PackageType"]);
                    objCCINEntry.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                    objCCINEntry.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCCINEntry.CHAName = Convert.ToString(Result["CHAName"]);
                    objCCINEntry.ConsigneeName = Convert.ToString(Result["ConsigneeName"]);
                    objCCINEntry.ConsigneeAdd = Convert.ToString(Result["ConsigneeAdd"]);
                    objCCINEntry.Country = Convert.ToString(Result["Country"]);
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
                    // objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    // objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.VehicleNo = Convert.ToString(Result["VehicleNo"]);
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
                    objCCINEntry.PackUQCDescription = Result["PackUQCDesc"].ToString();
                    // objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
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

                List<Wlj_CCINEntry> CCINEntryList = new List<Wlj_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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

        public void GetAllCCINEntryForSearch(int Page, string searchtext)
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
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForSearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<Wlj_CCINEntry> CCINEntryList = new List<Wlj_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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
        public void GetCcinEntrySlipForPrint(string ccin)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.VarChar, Value = ccin });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoCartedInSlipPrint", CommandType.StoredProcedure, DParam);
            WLJ_CCINPrint ObjStuffing = new WLJ_CCINPrint();
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
                    ObjStuffing.PackageType = Result["PackageType"].ToString();
                    ObjStuffing.Country = Result["CountryName"].ToString();
                    ObjStuffing.PortDestName = Result["PortDestName"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.Lstshed.Add(new WLJ_ShedEntries
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

        #region CCIN Entry Approval

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

                List<Wlj_CCINEntry> CCINEntryList = new List<Wlj_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
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

                List<Wlj_CCINEntry> CCINEntryList = new List<Wlj_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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

        #endregion

        #region  Carting Register
        public void AddEditCartingRegister(Wlj_CartingRegister objCR, string XML /*, string LocationXML,string ClearLocation=null*/)
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
            List<Wlj_CartingRegister> lstCR = new List<Wlj_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Wlj_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
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
            Wlj_CartingRegister objCR = new Wlj_CartingRegister();
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
                        objCR.lstRegisterDtl.Add(new Wlj_CartingRegisterDtl
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
                            StorageType = (result["StorageType"]).ToString(),
                            Exporter = Convert.ToString(result["Exporter"]),
                            CartingAppDtlId = Convert.ToInt32(result["CartingAppDtlId"]),
                            LocationDetails = (result["LocationDetails"] == null ? "" : result["LocationDetails"]).ToString(),
                            Location = (result["Location"] == null ? "" : result["Location"]).ToString(),
                            ExporterId = Convert.ToInt32(result["ExporterId"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CartingRegisterDtlId = Convert.ToInt32(result["CartingRegisterDtlId"]),
                            SQMReserved = Convert.ToDecimal(result["SQMReserved"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstShortCargoDetails.Add(new Wlj_ShortCargoDetails
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
            Wlj_CartingRegister ObjCarting = new Wlj_CartingRegister();
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
                        ObjCarting.lstRegisterDtl.Add(new Wlj_CartingRegisterDtl
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

                List<Wlj_CartingRegister> CartingList = new List<Wlj_CartingRegister>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Wlj_CartingRegister objCarting = new Wlj_CartingRegister();
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

                List<Wlj_CartingRegister> CartingList = new List<Wlj_CartingRegister>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Wlj_CartingRegister objCarting = new Wlj_CartingRegister();
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

        #region Stuffing Request

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
        public void AddEditStuffingRequest(Wlj_StuffingRequest ObjStuffing, string StuffingXML, string StuffingContrXML)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = (ObjStuffing.PortId == null ? 0 : ObjStuffing.PortId) });
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
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingRequest
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
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingRequest
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
            Wlj_StuffingRequest ObjStuffing = new Wlj_StuffingRequest();
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
                        ObjStuffing.LstStuffing.Add(new Wlj_StuffingRequestDtl
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
                        ObjStuffing.LstStuffingContainer.Add(new Wlj_StuffingReqContainerDtl
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
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingRequest
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
            List<Wlj_StuffingRequestDtl> LstStuffing = new List<Wlj_StuffingRequestDtl>();
            List<Wlj_StuffingReqContainerDtl> LstStuffingContr = new List<Wlj_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingRequestDtl
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
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        RQty=Convert.ToInt32(Result["RemainingUnits"]),
                        RWt=Convert.ToDecimal(Result["RemainingWeight"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new Wlj_StuffingReqContainerDtl
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
            List<Wlj_StuffingReqContainerDtl> LstStuffing = new List<Wlj_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingReqContainerDtl
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
            Wlj_StuffingReqContainerDtl ObjStuffing = new Wlj_StuffingReqContainerDtl();
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
            List<Wlj_StuffingRequestDtl> LstBillingNo = new List<Wlj_StuffingRequestDtl>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingBillNoOfCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBillingNo.Add(new Wlj_StuffingRequestDtl
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
            Wlj_StuffingReqContainerDtl ObjSR = new Wlj_StuffingReqContainerDtl();
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
            List<ForwarderList> LstForwarder = new List<ForwarderList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstForwarder.Add(new ForwarderList
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

        public void Kdl_GetStuffingRequest(int StuffingReqId, int RoleId, int Uid)
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
            Wlj_StuffingRequest ObjStuffing = new Wlj_StuffingRequest();
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
                        ObjStuffing.LstStuffing.Add(new Wlj_StuffingRequestDtl
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
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new Wlj_StuffingReqContainerDtl
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
                            Fob = Convert.ToDecimal(Result["Fob"]),
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
        public void Kdl_GetCartRegDetForStuffingReq(int CartingRegisterId)
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
            List<Wlj_StuffingRequestDtl> LstStuffing = new List<Wlj_StuffingRequestDtl>();
            List<Wlj_StuffingReqContainerDtl> LstStuffingContr = new List<Wlj_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_StuffingRequestDtl
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
                        PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                        PackUQCCode = Convert.ToString(Result["PackUQCCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new Wlj_StuffingReqContainerDtl
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

        #endregion

        #region Container Stuffing
        public void AddEditContainerStuffing(Wlj_ContainerStuffing ObjStuffing, string ContainerStuffingXML)//, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
       // public void AddEditContainerStuffing(Wlj_ContainerStuffing ObjStuffing, string ContainerStuffingXML)                                                                                                  // string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML)
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

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SCMTRXML", MySqlDbType = MySqlDbType.LongText, Value = SCMTRXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CustodianId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {

                if (Result == 1)
                {
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
            Wlj_ContainerStuffingDtl ObjStuffing = new Wlj_ContainerStuffingDtl();
            List<Wlj_ContainerStuffingDtl> LstStuffing = new List<Wlj_ContainerStuffingDtl>();
            _DBResponse = new DatabaseResponse();
            int PortId = 0; string PortName = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    PortId = Convert.ToInt32(Result["PortId"]);
                    PortName = Result["PortName"].ToString();
                    LstStuffing.Add(new Wlj_ContainerStuffingDtl
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
                        EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                        EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                        EquipmentStatus = Convert.ToString(Result["EquipmentStatus"])
                    });
                }
                if (Status == 1)
                {
                    ObjStuffing.LstStuffing = LstStuffing;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { ObjStuffing, PortId, PortName };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContainerStuffing
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
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
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"] == null ? "" : Result["CustodianCode"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"] == DBNull.Value ? 0 : Result["CustodianId"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new Wlj_ContainerStuffingDtl
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
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"] == null ? "" : Result["EquipmentQUC"])
                        });
                    }
                }

                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjStuffing.LstSCMTRDtl.Add(new WLJContainerStuffingSCMTR
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

        public void ListOfFinalDestination(string CustodianName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianName", MySqlDbType = MySqlDbType.VarChar, Value = CustodianName });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFinalDestination", CommandType.StoredProcedure, DParam);


            List<WLJ_FinalDestination> LstCustodian = new List<WLJ_FinalDestination>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCustodian.Add(new WLJ_FinalDestination
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
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContainerStuffing
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        RequestDate = Result["RequestDate"].ToString()
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
            List<Wlj_ContainerStuffingDtl> LstStuffing = new List<Wlj_ContainerStuffingDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContainerStuffingDtl
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
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
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
                    ObjStuffing.CustodianCode= Result["CustodianCode"].ToString();
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
                        ObjStuffing.LstStuffingDtl.Add(new Wlj_ContainerStuffingDtl
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
                            EquipmentSealType= (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString(),
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
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContainerStuffing
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
            List<Wlj_ContainerStuffing> Lstsr = new List<Wlj_ContainerStuffing>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstsr.Add(new Wlj_ContainerStuffing
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
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
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
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"] == null ? "" : Result["CustodianCode"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"] == DBNull.Value ? 0 : Result["CustodianId"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new Wlj_ContainerStuffingDtl
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
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"] == null ? "" : Result["EquipmentQUC"])
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

        public void AddEditAmendmentContainerStuffing(Wlj_ContainerStuffing ObjStuffing, string ContainerStuffingXML)
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
            
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CustodianId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });

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
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Wlj_ContainerStuffing
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
            IList<WLJ_ContainerStuffingPSReq> objPaySheetStuffing = new List<WLJ_ContainerStuffingPSReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new WLJ_ContainerStuffingPSReq()
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
            IList<WLJ_ContainerDetails> objPaySheetStuffing = new List<WLJ_ContainerDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new WLJ_ContainerDetails()
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
        public void GetExportPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType,string SEZ, string ContainerXML,
            int InvoiceId, int PartyId, int PayeeId, int OTHour)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHour", MySqlDbType = MySqlDbType.Int32, Value = OTHour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WLJ_ExpPaymentSheet objInvoice = new WLJ_ExpPaymentSheet();
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
                        objInvoice.lstPostPaymentCont.Add(new WLJExpInvoiceContainerBase
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
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                        });
                        objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new WLJExpInvoiceChargeBase
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
                        objInvoice.lstContwiseAmt.Add(new WLJ_ExpContWiseAmount
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
                        objInvoice.lstOperationContwiseAmt.Add(new WLJ_ExpOperationContWiseCharge
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
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<WLJListOfExpInvoice> lstExpInvoice = new List<WLJListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new WLJListOfExpInvoice()
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
        public void AddEditExpInvoice(WLJ_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });
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
            Wlj_LoadContReq objDet = new Wlj_LoadContReq();
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
                    objDet.IsEMGDocument = Convert.ToBoolean(Result["IsEgmDocument"]);
                    objDet.ExaminationType = Convert.ToString(Result["EximType"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objDet.lstContDtl.Add(new Wlj_LoadContReqDtl
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
                            CommodityType = Result["CommodityType"].ToString()
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
        public void AddEditLoadContDetails(Wlj_LoadContReq objLoadContReq, string XML)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objLoadContReq.ExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsEgmDocument", MySqlDbType = MySqlDbType.Int32,  Value =Convert.ToInt32(objLoadContReq.IsEMGDocument) });

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
        #endregion

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
        //mks
        public void GetLoadedPaymentSheetInvoice(int ContainerStuffingId, string InvoiceDate, string InvoiceType, string SEZ,string contxml, int PayeeId, int PartyId, int InvoiceId = 0 , int OTHour=0,int OnWheelHours=0,string ExportUnder="")
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHour", MySqlDbType = MySqlDbType.Int32, Value = OTHour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnWheelHours", MySqlDbType = MySqlDbType.Int32, Value = OnWheelHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WLJ_ExpPaymentSheet objInvoice = new WLJ_ExpPaymentSheet();
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
                        objInvoice.lstPostPaymentCont.Add(new WLJExpInvoiceContainerBase
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
                        objInvoice.lstPostPaymentChrg.Add(new WLJExpInvoiceChargeBase
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
                        objInvoice.lstContwiseAmt.Add(new WLJ_ExpContWiseAmount
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
                        objInvoice.lstOperationContwiseAmt.Add(new WLJ_ExpOperationContWiseCharge
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


        public void AddEditInvoiceContLoaded(WLJ_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });
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

        #region BTT Cargo Entry
        public void AddEditBTTCargoEntry(Wlj_BTTCargoEntry ObjBTT, string StuffingXML, int BranchId, int Uid)
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
            IList<Wlj_BTTCargoEntry> ObjBTT = new List<Wlj_BTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.Add(new Wlj_BTTCargoEntry()
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
            Wlj_BTTCargoEntry ObjBTT = new Wlj_BTTCargoEntry();
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
                        ObjBTT.lstBTTCargoEntryDtl.Add(new Wlj_BTTCargoEntryDtl
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
                            BTTWeight = Convert.ToInt32(Result["BTTWeight"])
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
            IList<Wlj_BTTCartingList> lstBTTCartingList = new List<Wlj_BTTCartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingList.Add(new Wlj_BTTCartingList()
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
            IList<Wlj_BTTCartingDetailList> lstBTTCartingDetailList = new List<Wlj_BTTCartingDetailList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingDetailList.Add(new Wlj_BTTCartingDetailList()
                    {
                        CartingDetailId = Convert.ToInt32(Result["CartingAppDtlId"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"])
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
        #endregion
        #region CCIN Entry Approval Search
        public void GetAllCCINEntryForSearchApproval(int Page, string searchtext)
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
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForSearchApproval", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<Wlj_CCINEntry> CCINEntryList = new List<Wlj_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
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

    }
}