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
using CwcExim.Areas.Import.Models;
using System.Globalization;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{
    public class Hdb_ExportRepository
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
        #region Carting Application
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<Hdb_CHA> lstCHA = new List<Hdb_CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Hdb_CHA
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
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ListOfExpForCartingApp", CommandType.StoredProcedure, DParam);
            IList<Hdb_Exporter> lstExporter = new List<Hdb_Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Hdb_Exporter
                    {
                        EXPEximTraderId = Convert.ToInt32(result["EximTraderId"]),
                        ExporterName = result["NameAddress"].ToString()
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

        public void GetGstNoForCartApp(int ExpEximTraderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = ExpEximTraderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGstNoForCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CartingApplication ObjHdbCart = new Hdb_CartingApplication();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjHdbCart.GSTNo = Result["GSTNo"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjHdbCart;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
            IList<Areas.Export.Models.Hdb_Commodity> LstCommodity = new List<Areas.Export.Models.Hdb_Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Export.Models.Hdb_Commodity
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
        public void GetAllCartingApp(int RoleId, int Uid,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingList> LstCartingApp = new List<Hdb_CartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingApp.Add(new Hdb_CartingList
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ApplicationDate = Result["ApplicationDate"].ToString(),
                        SBNo=Convert.ToString(Result["SBNo"]),
                        SBDate=Convert.ToString(Result["SBDate"])
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

        public void GetAllCartingAppforSearch(String ShippingCha,int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Shipping", MySqlDbType = MySqlDbType.VarChar,Size=200, Value = ShippingCha });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingAppByShipping", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingList> LstCartingApp = new List<Hdb_CartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingApp.Add(new Hdb_CartingList
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ApplicationDate = Result["ApplicationDate"].ToString(),
                         SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"])
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

        public void GetAllCartingAppforSearchDate(String AppDate, int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if ((AppDate != null) || AppDate!="") AppDate = Convert.ToDateTime(AppDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = AppDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingAppByDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingList> LstCartingApp = new List<Hdb_CartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingApp.Add(new Hdb_CartingList
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ApplicationDate = Result["ApplicationDate"].ToString(),
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"])
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CartingApplication ObjCartingApp = new Hdb_CartingApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCartingApp.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCartingApp.CartingNo = Result["CartingNo"].ToString();
                    ObjCartingApp.ApplicationNo = Result["CartingNo"].ToString();
                    ObjCartingApp.ApplicationDate = Result["ApplicationDate"].ToString().Split()[0];
                    ObjCartingApp.CHAEximTraderId = Convert.ToInt32(Result["CHAEximTraderId"]);
                    ObjCartingApp.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
                    ObjCartingApp.ForwarderName = Result["ForwarderName"].ToString();
                    ObjCartingApp.VehicleNo = Result["VehicleNo"].ToString();
                    ObjCartingApp.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjCartingApp.CHAName = Result["CHAName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCartingApp.lstShipping.Add(new Hdb_ShippingDetails
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
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            CustomHouseCode = Result["CustomHouseCode"].ToString(),
                            IECCode = Result["IECCode"].ToString(),
                            GSTNo = Result["GSTNo"].ToString(),
                            PortOfDestination = Result["PortOfDestination"].ToString(),
                            PackageType = Result["PackageType"].ToString(),
                            OtherPkgType = Result["OtherPkgType"].ToString(),
                            CommInvDate=Result["CommInvDate"].ToString(),
                            PackingListDate=Result["PackingListDate"].ToString(),
                            SCMTRPackageType=Convert.ToString(Result["SCMTRPackageType"]),
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                            IsSEZ = Convert.ToBoolean(Result["SEZ"]),
                        }

                        );
                    }
                    ////while(Result["PackageType"].ToString()=="Others")
                    ////{
                    ////    ObjCartingApp.lstShipping.Add(new Hdb_ShippingDetails
                    ////    {
                    ////        OtherPkgType = Result["OtherPkgType"].ToString()

                    ////    });
                    ////}
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
        public void AddEditCartingApp(Hdb_CartingApplication objCA, int Uid)
        {
            string Param = "0", ReturnObj = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.CartingNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.ApplicationNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCA.ApplicationDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAEximTraderId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CHAEximTraderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderId", MySqlDbType = MySqlDbType.Int32, Value = objCA.ForwarderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderName", MySqlDbType = MySqlDbType.VarChar, Value = objCA.ForwarderName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Value = objCA.VehicleNo });
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
                else if (result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can not Save due to insufficient Party Bond ";
                    _DBResponse.Data = null;
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
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In GateEntry.";
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
            List<Hdb_PrintCA> lstCA = new List<Hdb_PrintCA>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCA.Add(new Hdb_PrintCA
                    {
                        ShipBillNo = Result["ShippingBillNo"].ToString(),
                        ShipBillDate = Result["ShippingBillDate"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        MarksAndNo = Result["MarksAndNo"].ToString(),
                        FoBValue = Convert.ToDecimal(Result["FoBValue"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        PkgType = Result["PkgType"].ToString(),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        SCMTRPackageType= Result["SCMTRPackageType"].ToString()
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

        public void IcegateSBList()
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIcegateSBDetForCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_SBList> LstSB = new List<Hdb_SBList>();
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_SBList
                    {
                        Id = Convert.ToInt32(Result["ID"]),
                        SBNo = Convert.ToString(Result["SB_NO"])
                    });
                }
                if(Status==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Data = LstSB;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
            catch(Exception ex)
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


        public void GetIceGateSBDet(string SBNo,int Id)
        {
            int Status = 0;
            List<MySqlParameter> Lstparam = new List<MySqlParameter>();
            Lstparam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            Lstparam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = SBNo });
            IDataParameter[] Dparam = { };
            Dparam = Lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIcegateSBDetForCartingApp", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            Hdb_ShippingDetails ObjSB = new Hdb_ShippingDetails();
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    ObjSB.CustomHouseCode = Convert.ToString(Result["CustomCD"]);
                    ObjSB.ShippingBillNo = Convert.ToString(Result["SB_NO"]);
                    ObjSB.ShippingDate = Convert.ToString(Result["SB_DATE"]);
                    ObjSB.IECCode = Convert.ToString(Result["IECCD"]);
                    ObjSB.FoBValue = Convert.ToDecimal(Result["FOB"]);
                    ObjSB.PortOfDestination = Convert.ToString(Result["POD"]);

                }
                if (Status==1)
                {
                    _DBResponse.Data = ObjSB;
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
            catch(Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
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
        #endregion


        #region PackingApplication

        public void GetPacking(int Packingapplicationid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_Packingapplicationid", MySqlDbType = MySqlDbType.Int32, Value = Packingapplicationid });
            lstparam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetPackingApp", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_PackingApplication objFcs = new Hdb_PackingApplication();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFcs.Packingapplicationid = Convert.ToInt32(result["Packingapplicationid"]);
                    objFcs.PackingNo = result["PackingNo"].ToString();
                    objFcs.VehicleNo = result["VehicleNo"].ToString();
                    objFcs.EntryDate = result["EntryDate"].ToString();
                    objFcs.PartyName = result["PartyName"].ToString();
                    objFcs.NoofItems = Convert.ToInt32(result["NoOfItems"]);
                    objFcs.MaterialType = result["MaterialType"].ToString();
                    if (objFcs.MaterialType == "Others")
                    {
                        //  objFcs.MaterialType = result["MaterialType"].ToString();
                        objFcs.Others = result["Others"].ToString();
                    }
                    else
                    {
                        objFcs.MaterialType = result["MaterialType"].ToString();
                    }
                    objFcs.Remarks = result["Remarks"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFcs;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void AddEditPackingApplication(Hdb_PackingApplication objFC)
        {

            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            lstParam.Add(new MySqlParameter { ParameterName = "in_Packingapplicationid", MySqlDbType = MySqlDbType.Int32, Value = objFC.Packingapplicationid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackingNo", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.PackingNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.VehicleNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFC.EntryDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = objFC.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NoOfItems", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objFC.NoofItems) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Others", MySqlDbType = MySqlDbType.VarChar, Value = objFC.Others });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MaterialType", MySqlDbType = MySqlDbType.VarChar, Value = objFC.MaterialType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objFC.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objFC.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditPackingApplication", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Packing Application Saved Successfully" : "Packing Application Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
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
        public void GetAllPackingApp(int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Packingapplicationid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPackingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_PackingApplication> lstPackingApp = new List<Hdb_PackingApplication>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPackingApp.Add(new Hdb_PackingApplication
                    {
                        Packingapplicationid = Convert.ToInt32(Result["Packingapplicationid"]),
                        PackingNo = Result["PackingNo"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        EntryDate = Result["EntryDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        NoofItems = Convert.ToInt32(Result["NoofItems"]),
                        MaterialType = Result["MaterialType"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPackingApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void DeletePacking(int Packingapplicationid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Packingapplicationid", MySqlDbType = MySqlDbType.Int32, Value = Packingapplicationid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeletePackingApp", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Packing Application Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In GateEntry.";
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


        #endregion
        #region Carting Register
        public void AddEditCartingRegister(Hdb_CartingRegister objCR, string XML /*, string LocationXML,string ClearLocation=null*/)
        {
            string OutParam = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCR.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RegisterDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.RegisterDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CHAId});
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objCR.Uid });
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
                    _DBResponse.Message = "Cannot Update Carting Register Details As It Already Exist In Another Page";
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
        public void GetAllRegisterDetails(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            List<Hdb_CartingRegister> lstCR = new List<Hdb_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Hdb_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
                        RequestDate = result["RequestDate"].ToString(),
                        SBNo=Convert.ToString(result["SBNo"]),
                        SBDate=Convert.ToString(result["SBDate"])
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

        public void GetAllRegisterDetailsByDate(String CartingRegDate)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            if ((CartingRegDate != null) || CartingRegDate != "") CartingRegDate = Convert.ToDateTime(CartingRegDate).ToString("yyyy-MM-dd");
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegDate", MySqlDbType = MySqlDbType.Date, Value = CartingRegDate });

            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegisterByDate", CommandType.StoredProcedure, Dparam);
            List<Hdb_CartingRegister> lstCR = new List<Hdb_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Hdb_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
                        RequestDate = result["RequestDate"].ToString(),
                        SBNo = Convert.ToString(result["SBNo"]),
                        SBDate = Convert.ToString(result["SBDate"])
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

        public void GetAllRegisterDetailsByCHA(String ShippingCha)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingCha", MySqlDbType = MySqlDbType.VarChar,Size=200, Value = ShippingCha });

            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegisterByCHA", CommandType.StoredProcedure, Dparam);
            List<Hdb_CartingRegister> lstCR = new List<Hdb_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Hdb_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
                        RequestDate = result["RequestDate"].ToString(),
                        SBNo = Convert.ToString(result["SBNo"]),
                        SBDate = Convert.ToString(result["SBDate"])
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
        public void GetRegisterDetails(int CartingRegisterId, string Purpose = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Purpose });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 10, Value = 0 });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            Hdb_CartingRegister objCR = new Hdb_CartingRegister();
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
                    objCR.RequestDate = result["RequestDate"].ToString();
                    objCR.ApplicationDate = result["ApplicationDate"].ToString();
                    objCR.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                    objCR.GodownName = (result["GodownName"] == null ? "" : result["GodownName"]).ToString();
                    objCR.GodownId = Convert.ToInt32(result["GodownId"] == DBNull.Value ? 0 : result["GodownId"]);
                    objCR.ApplicationNo = result["ApplicationNo"].ToString();
                    objCR.CHAId = Convert.ToInt32(result["CHAId"]);
                    objCR.CHAName = result["CHANAME"].ToString();
                    objCR.CartingType = Convert.ToInt32(result["CartingType"]);
                    objCR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstRegisterDtl.Add(new Hdb_CartingRegisterDtl
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
                            Storage = (result["StorageArea"] == null ? "" : result["StorageArea"]).ToString(),
                            ForwarderId = Convert.ToInt32(result["ForwarderId"]),
                            ForwarderName = (result["ForwarderName"] == null ? "" : result["ForwarderName"]).ToString(),
                        });
                    }
                }
                if (Purpose == "edit")
                {
                    if (result.NextResult())
                    {
                        while (result.Read())
                        {
                            objCR.lstGdnWiseLctn.Add(new Areas.Export.Models.Hdb_GodownWiseLocation
                            {
                                Row = result["Row"].ToString(),
                                Column = result["Column"].ToString(),
                                LocationId =Convert.ToInt32(result["LocationId"]),
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
            List<Hdb_ApplicationNoDet> lstApplication = new List<Hdb_ApplicationNoDet>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new Hdb_ApplicationNoDet
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
            Hdb_CartingRegister ObjCarting = new Hdb_CartingRegister();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCarting.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCarting.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjCarting.GodownName = Result["GodownName"].ToString();
                    ObjCarting.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjCarting.CHAName = Result["EximTraderName"].ToString();
                    ObjCarting.CHAId = Convert.ToInt32(Result["CHAId"]);
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCarting.lstRegisterDtl.Add(new Hdb_CartingRegisterDtl
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
                            Exporter = Result["EximTraderName"].ToString()
                        });
                    }
                }
                /* if (Result.NextResult())
                 {
                     while (Result.Read())
                     {
                         ObjCarting.lstGdnWiseLctn.Add(new Areas.Export.Models.GodownWiseLocation
                         {
                             LocationId = Convert.ToInt32(Result["LocationId"]),
                             Row = Result["Row"].ToString(),
                             Column = Convert.ToInt32(Result["Column"]),
                             // IsOccupied = Convert.ToBoolean(Result["IsOccupied"])
                         });
                     }
                 } */

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

        public void PrintCartingReg(int CartingRegisterId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("PrintCartingReg", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<PrintCartingReg> LstCartingReg = new List<PrintCartingReg>();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingReg.Add(new PrintCartingReg
                    {
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        RegisterDate = Convert.ToString(Result["RegisterDate"]),
                        ShipBillNo = Result["ShipBillNo"].ToString(),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        CHA = Result["CHA"].ToString(),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        FobValue = Convert.ToDecimal(Result["FobValue"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        SBUnits = Convert.ToInt32(Result["SBUnits"]),
                        BalanceUnits = Convert.ToInt32(Result["BalanceUnits"]),
                        ActualUnits = Convert.ToInt32(Result["ActualUnits"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstCartingReg;
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


        #region Container Stuffing
        public void AddEditContainerStuffing(HDBContainerStuffing ObjStuffing, string ContainerStuffingXML)
        {
            var StuffingDate = (dynamic)null;
            StuffingDate = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss");
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPrinting", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.IsPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrint", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.NoOfPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.FinalDestinationLocationId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjStuffing.FinalDestinationLocation) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                    _DBResponse.Message = "Cannot Edit Container Stuffing Details As It Already Exist In Another Page";
                }


                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can not Save due to insufficient Party Bond ";
                    _DBResponse.Data = null;
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
            List<HDBContainerStuffingDtl> LstStuffing = new List<HDBContainerStuffingDtl>();
            HDBContainerStuffingDtl ObjStuffing = new HDBContainerStuffingDtl();
            // List<ContainerStuffingDtl> LstContainer = new List<ContainerStuffingDtl>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new HDBContainerStuffingDtl
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
                    IsReefer = Convert.ToInt32(Result["Reefer"]),
                    IsODC = Convert.ToInt32(Result["IsODC"]),
                    Forwarder = Result["Forwarder"].ToString(),
                    ForwarderId = Convert.ToInt32(Result["ForwarderId"]),
                    Insured = 0,
                    EquipmentQUC = Result["EquipmentQUC"].ToString(),
                    EquipmentSealType = Result["EquipmentSealType"].ToString(),
                    EquipmentStatus = Result["EquipmentStatus"].ToString(),
                    MCINPCIN = Convert.ToString(Result["MCINPCIN"]),
                    SEZ = Convert.ToInt32(Result["SEZ"])
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
        public void GetAllContainerStuffing(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new HDBContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        Forwader= Result["ForwarderName"].ToString()
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
        public void GetContainerStuffing(int ContainerStuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
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
                    ObjStuffing.IsPrinting = Convert.ToString(Result["IsPrinting"]);
                    ObjStuffing.NoOfPrinting = Convert.ToInt32(Result["NoOfPrinting"]);
                    ObjStuffing.FinalDestinationLocationId = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    ObjStuffing.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new HDBContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            ConsigneeLocation = (Result["ConsigneeLocation"] == null ? "" : Result["ConsigneeLocation"]).ToString(),
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
                            IsODC = Convert.ToInt32(Result["IsODC"]),
                            IsReefer = Convert.ToInt32(Result["IsReefer"]),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"]),
                            Forwarder = Result["Forwarder"].ToString(),
                            CBM= Convert.ToDecimal(Result["CUM"]),
                            EquipmentQUC=Convert.ToString(Result["EquipmentQUC"]),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                            MCINPCIN = Convert.ToString(Result["MCINPCIN"]),
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
        public void GetReqNoForContainerStuffing()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReqNoForContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new HDBContainerStuffing
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"])
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
            List<HDBContainerStuffingDtl> LstStuffing = new List<HDBContainerStuffingDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new HDBContainerStuffingDtl
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
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjStuffing.FinalDestinationLocation = Result["FinalDestinationLocation"].ToString(); 
                }
                if (Result.NextResult())
                {
                    ObjStuffing.Size = "";
                    while (Result.Read())
                    {
                        ObjStuffing.Size += Result["Size"].ToString() + ",";
                    }
                    ObjStuffing.Size = ObjStuffing.Size.Remove(ObjStuffing.Size.Length - 1);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new HDBContainerStuffingDtl
                        {
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToInt32(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            CartingRegNo = (Result["CartingRegisterNo"] == null ? "" : Result["CartingRegisterNo"]).ToString(),
                            EquipmentSealType = (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString(),
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
     
        #region Container Stuffing Amendment

        public void ListOfStuffingNoForAmendment()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfStuffingNoForAmendment", CommandType.StoredProcedure, DParam);
            List<HDBContainerStuffing> Lstsr = new List<HDBContainerStuffing>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstsr.Add(new HDBContainerStuffing
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
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
                    ObjStuffing.IsPrinting = Convert.ToString(Result["IsPrinting"]);
                    ObjStuffing.NoOfPrinting = Convert.ToInt32(Result["NoOfPrinting"]);
                    ObjStuffing.FinalDestinationLocationId = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    ObjStuffing.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingDtl.Add(new HDBContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            ConsigneeLocation = (Result["ConsigneeLocation"] == null ? "" : Result["ConsigneeLocation"]).ToString(),
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
                            IsODC = Convert.ToInt32(Result["IsODC"]),
                            IsReefer = Convert.ToInt32(Result["IsReefer"]),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"]),
                            Forwarder = Result["Forwarder"].ToString(),
                            CBM = Convert.ToDecimal(Result["CUM"]),
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

        public void AddEditAmendmentContainerStuffing(HDBContainerStuffing ObjStuffing, string ContainerStuffingXML)
        {

            var StuffingDate = (dynamic)null;
            StuffingDate = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss");
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.DateTime, Value = StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPrinting", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.IsPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrint", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.NoOfPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjStuffing.FinalDestinationLocationId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(ObjStuffing.FinalDestinationLocation) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditAmendmentContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new HDBContainerStuffing
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

        #region Stuffing Request
        public void AddEditStuffingRequest(Hdb_StuffingRequest ObjStuffing, string StuffingXML, string StuffingContrXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TypeOfTrip", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TypeOfTrip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Destination", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Destination });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RequestDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.RequestDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.StuffingType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingXML", MySqlDbType = MySqlDbType.Text, Value = StuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingContrXML", MySqlDbType = MySqlDbType.Text, Value = StuffingContrXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalQuantity", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ttlQuantity });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.ttlWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalFob", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.ttlValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Confirm", MySqlDbType = MySqlDbType.Text, Value = string.IsNullOrEmpty(ObjStuffing.IsConfirm)?"":ObjStuffing.IsConfirm });

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
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can not Save due to insufficient Party Bond ";
                    _DBResponse.Data = null;
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
                else if (Result == 2)
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
        public void GetAllStuffingRequest(int RoleId, int Uid,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_StuffingRequest> LstStuffing = new List<Hdb_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Hdb_StuffingRequest
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"])
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
        public void GetCartRegNoForStuffingReq()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegNoForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_StuffingRequest> LstStuffing = new List<Hdb_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Hdb_StuffingRequest
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
            List<Hdb_StuffingReqContainerDtl> LstStuffing = new List<Hdb_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Hdb_StuffingReqContainerDtl
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
            Hdb_StuffingReqContainerDtl ObjStuffing = new Hdb_StuffingReqContainerDtl();
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
                    ObjStuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
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
            List<Hdb_StuffingRequestDtl> LstBillingNo = new List<Hdb_StuffingRequestDtl>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingBillNoOfCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBillingNo.Add(new Hdb_StuffingRequestDtl
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
        //public void GetShippingLine()
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
        //    IDataParameter[] DParam = { };
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
        //    List<Areas.Export.Models.ShippingLine> LstShippingLine = new List<Areas.Export.Models.ShippingLine>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstShippingLine.Add(new Areas.Export.Models.ShippingLine
        //            {
        //                ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
        //                ShippingLineName = Result["ShippingLine"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = LstShippingLine;
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
        public void Hdb_GetStuffingRequest(int StuffingReqId, int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_StuffingRequest ObjStuffing = new Hdb_StuffingRequest();
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
                    ObjStuffing.Destination = (Result["Destination"] == null ? "" : Result["Destination"]).ToString();
                    ObjStuffing.Movement = Convert.ToInt32(Result["Movement"]);
                    ObjStuffing.TypeOfTrip = Convert.ToInt32(Result["TypeOfTrip"]);
                    //ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new Hdb_StuffingRequestDtl
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
                            ContComeFrom = Convert.ToInt32(Result["ContComeFrom"]),
                            PortId = Convert.ToInt32(Result["PortId"]),
                            PortName = (Result["PortName"] == null ? "" : Result["PortName"]).ToString(),
                            Distance = Convert.ToDecimal(Result["Distance"]),
                            ContainerType = Convert.ToInt32(Result["ContainerType"]),
                            Cum = Convert.ToDecimal(Result["Cum"]),
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                            //  ShippingLine = Result["ShippingLine"].ToString(),
                            // CFSCode = Result["CFSCode"].ToString()
                            // RQty = Convert.ToInt32(Result["RQty"]),
                            // RWt = Convert.ToDecimal(Result["RWt"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new Hdb_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            StuffFOBValue = Convert.ToDecimal(Result["StuffFOBValue"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            SealNo = Result["SealNo"].ToString(),
                            ForwarderName = Result["ForwarderName"].ToString(),
                            ForwarderId = Convert.ToInt32(Result["ForwarderId"]),
                            CartRegDtlId = Convert.ToInt32(Result["CartRegDtlId"]),
                            ContComeFrom = Convert.ToInt32(Result["ContComeFrom"]),
                            PortId = Convert.ToInt32(Result["PortId"]),
                            PortName = (Result["PortName"] == null ? "" : Result["PortName"]).ToString(),
                            Distance = Convert.ToDecimal(Result["Distance"]),
                            ContainerType = Convert.ToInt32(Result["ContainerType"]),
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
        public void Hdb_GetCartRegDetForStuffingReq(int CartingRegisterId)
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
            List<Hdb_StuffingRequestDtl> LstStuffing = new List<Hdb_StuffingRequestDtl>();
            List<Hdb_StuffingReqContainerDtl> LstStuffingContr = new List<Hdb_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Hdb_StuffingRequestDtl
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
                        Cum = Convert.ToDecimal(Result["Cum"] == DBNull.Value ? 0 : Result["Cum"]),
                        ForwarderId = Convert.ToInt32(Result["ForwarderId"] == DBNull.Value ? 0 : Result["ForwarderId"]),
                        ForwarderName = Result["ForwarderName"].ToString(),
                        PackUQCCode = Result["PackUQCCode"].ToString(),
                        PackUQCDescription = Result["PackUQCDesc"].ToString(),
                        //  RQty = Convert.ToInt32(Result["RemainingUnits"]),
                        //  RWt = Convert.ToDecimal(Result["RemainingWeight"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new Hdb_StuffingReqContainerDtl
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
        #endregion

        #region Load Container Request
        public void ListOfLoadCont(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDBListLoadContReq> LstCont = new List<HDBListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new HDBListLoadContReq
                    {
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Result["LoadContReqNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        LoadContReqDate = Result["LoadContReqDate"].ToString(),
                        groupContainer = (Result["ContainerNO"]).ToString(),
                        groupSize = Result["Size"].ToString(),
                        groupShipBill = Result["ShippingBillNo"].ToString(),
                        PortName = Result["PortName"].ToString(),
                        ShippingBillDate = Result["ShippingBillDate"].ToString()
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDBLoadContReq objDet = new HDBLoadContReq();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDet.LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]);
                    objDet.LoadContReqNo = Result["LoadContReqNo"].ToString();
                    objDet.CHAName = Result["CHAName"].ToString();
                    objDet.IsODC = Convert.ToBoolean(Result["IsODC"]);
                    objDet.LoadContReqDate = Result["LoadContReqDate"].ToString();
                    objDet.Remarks = Result["Remarks"].ToString();
                    objDet.IsPrinting = Result["IsPrinting"].ToString();
                    objDet.NoOfPrint = Convert.ToInt32(Result["NoOfPrint"]);
                    objDet.PortId = Convert.ToInt32(Result["PortId"]);
                    objDet.PortName = (Result["PortName"]).ToString();
                    objDet.Distance = Convert.ToDecimal(Result["Distance"]);
                    objDet.Transportation = Result["Transportation"].ToString();
                    objDet.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objDet.CustomExaminationType = Convert.ToString(Result["ExamType"]);
                    objDet.FinalDestinationLocationID = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    objDet.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objDet.lstContDtl.Add(new HDBLoadContReqDtl
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
                            PackageType = Result["PackageType"].ToString(),
                            OtherPkgType = Result["OtherPkgType"].ToString(),
                            SCMTRPackageType = Convert.ToString(Result["SCMTRPackageType"]),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                            ContLoadType = Convert.ToString(Result["ContLoadType"]),
                            CustomSeal = Convert.ToString(Result["CustomSeal"]),
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
        public void AddEditLoadContDetails(HDBLoadContReq objLoadContReq, string XML)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objLoadContReq.LoadContReqDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsODC", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objLoadContReq.IsODC) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objLoadContReq.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPrinting", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = objLoadContReq.IsPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrint", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.NoOfPrint });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Transportation", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.Transportation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = objLoadContReq.Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExamType", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.CustomExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Confirm", MySqlDbType = MySqlDbType.Text, Value = string.IsNullOrEmpty(objLoadContReq.IsConfirm) ? "" : objLoadContReq.IsConfirm });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationID", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.FinalDestinationLocationID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.FinalDestinationLocation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
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

                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can not Save due to insufficient Party Bond ";
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

        #region BTT Payment Sheet
        public void GetCartingApplicationForPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);
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
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["CartingAppId"]),
                        StuffingReqNo = Convert.ToString(Result["CartingAppNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        AppNo = Convert.ToString(Result["AppNo"]),
                        
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

        public void GetPaymentPayee()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeForExportInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_PaymentPayeeName> objPaymentPartyName = new List<Hdb_PaymentPayeeName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Hdb_PaymentPayeeName()
                    {
                        PayeeId = Convert.ToInt32(Result["CHAId"]),
                        PayeeName = Convert.ToString(Result["CHAName"]),
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
        public void GetShipBillForPaymentSheet(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);
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
        public void GetBTTPaymentSheet(string InvoiceDate, int StuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, int PartyId, int CasualLabour,string ExportUnder)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            HDBInvoiceBTT objInvoice = new HDBInvoiceBTT();
            try
            {


                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetBTTPaymentSheet", CommandType.StoredProcedure, DParam);
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
                    objInvoice.lstPrePaymentCont.Add(new HDBPreInvoiceContainerBTT
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
                    objInvoice.lstPostPaymentCont.Add(new HDBPostPaymentContainerBTT
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
                    objInvoice.lstPostPaymentChrg.Add(new HDBPostPaymentChrgBTT
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
                    objInvoice.lstContWiseAmount.Add(new HDBContainerWiseAmountBTT
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new HDBOperationCFSCodeWiseAmountBTT
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
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                // {
                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.lstPreInvoiceCargo.Add(new HDBPreInvoiceCargoBTT
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
                //}
                //}
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

            }
        }
        public void AddEditBTTInvoice(HDBInvoiceBTT ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

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
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = ReturnObj;
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

        #region Export Payment Sheet


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
                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
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
        public void GetPaymentPartyForExportnvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForExportInvoice", CommandType.StoredProcedure);
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

        public void GetPayeeForExportnvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeForExportInvoice", CommandType.StoredProcedure);
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


        public void GetStuffingRequestForPaymentSheet(int StuffingReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
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
        //public void GetPaymentParty()
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentParty", CommandType.StoredProcedure);
        //    _DBResponse = new DatabaseResponse();
        //    IList<Hdb_PaymentPartyName> objPaymentPartyName = new List<Hdb_PaymentPartyName>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentPartyName.Add(new Hdb_PaymentPartyName()
        //            {
        //                PartyId = Convert.ToInt32(Result["CHAId"]),
        //                PartyName = Convert.ToString(Result["CHAName"]),
        //                Address = Convert.ToString(Result["Address"]),
        //                State = Convert.ToString(Result["StateName"]),
        //                StateCode = Convert.ToString(Result["StateCode"]),
        //                GSTNo = Convert.ToString(Result["GSTNo"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaymentPartyName;
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

        public void GetCFSCodeForEdit(int StuffingReqId, string Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCFSCodeForEditExp", CommandType.StoredProcedure, DParam);
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
        public void GetContainerForPaymentSheet(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
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

        //Not Required
        //public void GetPaymentSheet(string InvoiceDate, int StuffingReqId, string ContainerXML)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    Hdb_PaySheetChargeDetails objPaymentSheet = new Hdb_PaySheetChargeDetails();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentSheet.lstPSContainer.Add(new Hdb_PSContainer()
        //            {
        //                CFSCode = Convert.ToString(Result["CFSCode"]),
        //                ContainerNo = Convert.ToString(Result["ContainerNo"]),
        //                Size = Convert.ToString(Result["Size"]),
        //                IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //                Insured = Convert.ToString(Result["Insured"])
        //            });
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstEmptyGR.Add(new Hdb_PSEmptyGroudRent()
        //                {
        //                    ContainerType = Convert.ToString(Result["ContainerType"]),
        //                    CommodityType = Convert.ToString(Result["CommodityType"]),
        //                    IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //                    Size = Convert.ToString(Result["Size"]),
        //                    DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //                    DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //                    RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //                    ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //                    GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    FOBValue = Convert.ToDecimal(Result["Fob"]),
        //                    IsInsured = Convert.ToInt32(Result["Insured"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstLoadedGR.Add(new Hdb_PSLoadedGroudRent()
        //                {
        //                    ContainerType = Convert.ToString(Result["ContainerType"]),
        //                    CommodityType = Convert.ToString(Result["CommodityType"]),
        //                    IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //                    Size = Convert.ToString(Result["Size"]),
        //                    DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //                    DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //                    RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //                    ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //                    GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    FOBValue = Convert.ToDecimal(Result["Fob"]),
        //                    IsInsured = Convert.ToInt32(Result["Insured"])
        //                });
        //            }
        //        }
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
        //                objPaymentSheet.lstStorageRent.Add(new Areas.Export.Models.StorageRent()
        //                {
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    ActualCUM = Convert.ToDecimal(Result["CUM"]),
        //                    ActualSQM = Convert.ToDecimal(Result["SQM"]),
        //                    ActualWeight = Convert.ToDecimal(Result["ActualWeight"]),
        //                    StuffCUM = Convert.ToDecimal(Result["StuffCUM"]),
        //                    StuffSQM = Convert.ToDecimal(Result["StuffSQM"]),
        //                    StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
        //                    StorageDays = Convert.ToInt32(Result["StorageDays"]),
        //                    StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
        //                    StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
        //                    StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"]),
        //                    Fob = Convert.ToDecimal(Result["Fob"]),
        //                    WtPerPackage = Convert.ToDecimal(Result["WtPerPackage"])
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

        public void GetExportPaymentSheet(string InvoiceDate, int StuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, int PartyId, int CasualLabour,int Distance, string ExportUnder)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            HDBInvoiceExp objInvoice = new HDBInvoiceExp();
            try
            {


                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
                DataSet ds = DataAccess.ExecuteDataSet("GetExportPaymentSheet", CommandType.StoredProcedure, DParam);
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
                    objInvoice.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                    objInvoice.RequestNo = Result["StuffingReqNo"].ToString();
                    objInvoice.RequestDate = Result["RequestDate"].ToString();
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();

                }
                //}

                // if (Result.NextResult())
                //{

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new HDBPreInvoiceContainer
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
                    objInvoice.lstPostPaymentCont.Add(new HDBPostPaymentContainer
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
                    objInvoice.lstPostPaymentChrg.Add(new HDBPostPaymentChrg
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
                    objInvoice.lstContWiseAmount.Add(new HDBContainerWiseAmount
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new HDBOperationCFSCodeWiseAmount
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
                         SBNo= Convert.ToString(Result["SB"]),
                          SBDate=Convert.ToString(Result["SBDate"]),
                           CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Weight = Convert.ToDecimal(Result["WT"]),

                    });
                }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                // {
                //foreach (DataRow Result in ds.Tables[6].Rows)
                //{
                //    objInvoice.lstPreInvoiceCargo.Add(new HDBPreInvoiceCargo
                //    {
                //        BOENo = Result["BOENo"].ToString(),
                //        BOEDate = Result["BOEDate"].ToString(),
                //        BOLDate = Result["BOLDate"].ToString(),
                //        BOLNo = Result["BOLNo"].ToString(),
                //        CargoDescription = Result["CargoDescription"].ToString(),
                //        CargoType = Convert.ToInt32(Result["CargoType"]),
                //        CartingDate = Result["CartingDate"].ToString(),
                //        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                //        DestuffingDate = Result["DestuffingDate"].ToString(),
                //        Duty = Convert.ToDecimal(Result["Duty"]),
                //        GodownId = Convert.ToInt32(Result["GodownId"]),
                //        GodownName = Result["GodownName"].ToString(),
                //        GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                //        GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                //        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                //        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                //        SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                //        SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                //        StuffingDate = Result["StuffingDate"].ToString(),
                //        WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                //    });
                //}
                //}
                //}
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
        public void AddEditExpInvoice(HDBInvoiceExp ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,string ForeignPortCode, int BranchId, int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "Exp" });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FrnDestPortCd", MySqlDbType = MySqlDbType.VarChar, Value = ForeignPortCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            //int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
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
        //
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

        #region loaded container payment sheet
        public void GetLoadedPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
        string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
        string InvoiceType, string LineXML, int CasualLabour,int Distance,string ExportUnder,string Under, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Under", MySqlDbType = MySqlDbType.VarChar, Value = Under });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_LoadedContainerPayment objInvoice = new Hdb_LoadedContainerPayment();
            List<String> Clause = new List<String>();

            try
            {
                DataSet Result = new DataSet();
               // IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedPaymentSheet", CommandType.StoredProcedure, DParam);
            //    DataSet ds = DataAccess.ExecuteDataSet("GetLoadedPaymentSheet", CommandType.StoredProcedure, DParam);
                Result = DataAccess.ExecuteDataSet("GetLoadedPaymentSheet", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();



                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objInvoice.ROAddress = Convert.ToString(Result.Tables[0].Rows[0]["ROAddress"]);
                        objInvoice.CompanyName = Convert.ToString(Result.Tables[0].Rows[0]["CompanyName"]);
                        objInvoice.CompanyShortName = Convert.ToString(Result.Tables[0].Rows[0]["CompanyShortName"]);
                        objInvoice.CompanyAddress = Convert.ToString(Result.Tables[0].Rows[0]["CompanyAddress"]);
                        objInvoice.PhoneNo = Convert.ToString(Result.Tables[0].Rows[0]["PhoneNo"]);
                        objInvoice.FaxNumber = Convert.ToString(Result.Tables[0].Rows[0]["FaxNumber"]);
                        objInvoice.EmailAddress = Convert.ToString(Result.Tables[0].Rows[0]["EmailAddress"]);
                        objInvoice.StateId = Convert.ToInt32(Result.Tables[0].Rows[0]["StateId"]);
                        objInvoice.StateCode = Convert.ToString(Result.Tables[0].Rows[0]["StateCode"]);
                        objInvoice.CityId = Convert.ToInt32(Result.Tables[0].Rows[0]["CityId"]);
                        objInvoice.GstIn = Convert.ToString(Result.Tables[0].Rows[0]["GstIn"]);
                        objInvoice.Pan = Convert.ToString(Result.Tables[0].Rows[0]["Pan"]);
                      //  objInvoice.CompGST = Convert.ToString(Result.Tables[0].Rows[0]["CompGST"]);
                      //  objInvoice.CompPAN = Convert.ToString(Result.Tables[0].Rows[0]["CompPAN"]);
                     //   objInvoice.CompStateCode = Convert.ToString(Result.Tables[0].Rows[0]["CompStateCode"]);
                    }
                }

                if (Result.Tables[1].Rows.Count > 0)
                {
                    objInvoice.PartyId = Convert.ToInt32(Result.Tables[1].Rows[0]["CHAId"]);
                    objInvoice.CHAName = Convert.ToString(Result.Tables[1].Rows[0]["CHAName"]);
                    objInvoice.PartyName = Convert.ToString(Result.Tables[1].Rows[0]["CHAName"]);
                    objInvoice.PartyGST = Convert.ToString(Result.Tables[1].Rows[0]["GSTNo"]);
                    objInvoice.RequestId = Convert.ToInt32(Result.Tables[1].Rows[0]["LoadContReqId"]);
                    objInvoice.RequestNo = Convert.ToString(Result.Tables[1].Rows[0]["LoadContReqNo"]);
                    objInvoice.RequestDate = Convert.ToString(Result.Tables[1].Rows[0]["RequestDate"]);
                    objInvoice.PartyAddress = Convert.ToString(Result.Tables[1].Rows[0]["Address"]);
                    objInvoice.PartyStateCode = Convert.ToString(Result.Tables[1].Rows[0]["StateCode"]);

                }


                if (Result.Tables[2].Rows.Count > 0)
                {

                    foreach (DataRow dr in Result.Tables[2].Rows)
                    {
                        objInvoice.lstPrePaymentCont.Add(new Hdb_PreInvoiceContainer
                        {
                            ContainerNo = dr["ContainerNo"].ToString(),
                            CFSCode = dr["CFSCode"].ToString(),
                            ArrivalDate = dr["ArrivalDate"].ToString(),
                            ArrivalTime = dr["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(dr["CargoType"]),
                            BOENo = dr["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(dr["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(dr["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(dr["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(dr["CIFValue"]),
                            Duty = Convert.ToDecimal(dr["Duty"]),
                            Reefer = Convert.ToInt32(dr["Reefer"]),
                            RMS = Convert.ToInt32(dr["RMS"]),
                            DeliveryType = Convert.ToInt32(dr["DeliveryType"]),
                            Insured = Convert.ToInt32(dr["Insured"]),
                            Size = dr["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(dr["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(dr["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(dr["AppraisementPerct"]),
                            LCLFCL = dr["LCLFCL"].ToString(),
                            CartingDate = dr["CartingDate"].ToString(),
                            DestuffingDate = dr["DestuffingDate"].ToString(),
                            StuffingDate = dr["StuffingDate"].ToString(),
                            // ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = dr["BOEDate"].ToString(),
                            CHAName = dr["CHAName"].ToString(),
                            ImporterExporter = dr["ImporterExporter"].ToString(),
                            LineNo = dr["LineNo"].ToString(),
                            // OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = dr["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = dr["SpaceOccupiedUnit"].ToString(),
                            ISODC=Convert.ToInt32(dr["ISODC"]),

                        });
                        objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
                        {
                            ContainerNo = dr["ContainerNo"].ToString(),
                            CFSCode = dr["CFSCode"].ToString(),
                            ArrivalDate = dr["ArrivalDate"].ToString(),
                            ArrivalTime = dr["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(dr["CargoType"]),
                            BOENo = dr["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(dr["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(dr["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(dr["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(dr["CIFValue"]),
                            Duty = Convert.ToDecimal(dr["Duty"]),
                            Reefer = Convert.ToInt32(dr["Reefer"]),
                            RMS = Convert.ToInt32(dr["RMS"]),
                            DeliveryType = Convert.ToInt32(dr["DeliveryType"]),
                            Insured = Convert.ToInt32(dr["Insured"]),
                            Size = dr["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(dr["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(dr["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(dr["AppraisementPerct"]),
                            LCLFCL = dr["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(dr["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(dr["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(dr["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(dr["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(dr["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(dr["StuffingDate"].ToString()),
                            ISODC=Convert.ToInt32(dr["ISODC"]),

                        });
                    }
                }
                if (Result.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[3].Rows)
                    {
                        objInvoice.lstPostPaymentChrg.Add(new Hdb_PostPaymentChrg
                        {
                            ChargeId = Convert.ToInt32(dr["Srno"]),
                            Clause = dr["Clause"].ToString(),
                            ChargeType = dr["ChargeType"].ToString(),
                            ChargeName = dr["ChargeName"].ToString(),
                            SACCode = dr["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(dr["Quantity"]),
                            Rate = Convert.ToDecimal(dr["Rate"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            Discount = Convert.ToDecimal(dr["Discount"]),
                            Taxable = Convert.ToDecimal(dr["Taxable"]),
                            IGSTPer = Convert.ToDecimal(dr["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(dr["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(dr["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                            Total = Convert.ToDecimal(dr["Total"]),
                            OperationId = Convert.ToInt32(dr["OperationId"]),
                        });
                    }


                }

                if (Result.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[4].Rows)
                    {


                        objInvoice.lstContWiseAmount.Add(new Hdb_ContainerWiseAmount
                        {
                            CFSCode = dr["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(dr["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(dr["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(dr["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(dr["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(dr["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(dr["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(dr["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(dr["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(dr["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[5].Rows)
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new Hdb_OperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = dr["CFSCode"].ToString(),
                            ContainerNo = dr["ContainerNo"].ToString(),
                            Size = dr["Size"].ToString(),
                            OperationId = Convert.ToInt32(dr["OperationId"]),
                            ChargeType = dr["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Rate = Convert.ToDecimal(dr["Rate"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            Clause = dr["Clause"].ToString()
                        });
                    }
                }



                if (Result.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[5].Rows)
                    {
                        Clause.Add(dr["Clause"].ToString());

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

            }
        }

        public void AddEditInvoice(Hdb_LoadedContainerPayment ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string CfsChargeXML,string ForeignPortCode, int BranchId, int Uid,string Distance,
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = CfsChargeXML });


            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LEODate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LEODate});
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FrnDestPortCd", MySqlDbType = MySqlDbType.VarChar, Value = ForeignPortCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Under", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Under });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.VarChar, Value = Distance });

            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditLoadedContainerInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId,out ReturnObj);
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
                    _DBResponse.Status = 4;
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


        #region Party Bond
        public void AddEditPartyBond(PartyBond ObjPartyBond)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyBondId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyBond.PartyBondId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyBond.TrNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TrDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyBond.TrDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyBond.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjPartyBond.Value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ValidUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyBond.ValidUpto).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditPartyBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjPartyBond.PartyBondId == 0 ? "Party Bond Details Saved Successfully" : "Party Bond Details Updated Successfully");
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Party Bond Details Does Not Exist";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Error";
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
        public void GetPartyBond(int PartyBondId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyBondId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyBondId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PartyBond ObjPartyBond = new PartyBond();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPartyBond.PartyBondId = Convert.ToInt32(Result["PartyBondId"]);
                    ObjPartyBond.TrNo = (Result["TrNo"] == null ? "" : Result["TrNo"]).ToString();
                    ObjPartyBond.TrDate = (Result["TrDate"] == null ? "" : Result["TrDate"]).ToString();
                    ObjPartyBond.PartyName = (Result["PartyName"] == null ? "" : Result["PartyName"]).ToString();
                    ObjPartyBond.PartyId = Convert.ToInt32(Result["PartyId"] == DBNull.Value ? 0 : Result["PartyId"]);
                    ObjPartyBond.Value = Convert.ToDecimal(Result["Value"] == DBNull.Value ? 0 : Result["Value"]);
                    ObjPartyBond.ValidUpto = (Result["ValidUpto"] == null ? "" : Result["ValidUpto"]).ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPartyBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllPartyBond()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyBondId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyBondList> LstPartyBond = new List<PartyBondList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyBond.Add(new PartyBondList
                    {
                        PartyBondId = Convert.ToInt32(Result["PartyBondId"]),
                        TrNo = (Result["TrNo"] == null ? "" : Result["TrNo"]).ToString(),
                        TrDate = (Result["TrDate"] == null ? "" : Result["TrDate"]).ToString(),
                        PartyName = (Result["PartyName"] == null ? "" : Result["PartyName"]).ToString(),
                        Value = Convert.ToDecimal(Result["Value"] == DBNull.Value ? 0 : Result["Value"]),
                        ValidUpto = (Result["ValidUpto"] == null ? "" : Result["ValidUpto"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPartyBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Reefer Plugin Request
        public void GetReeferPluginRequest(int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReeferPluginRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> lstContainer = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Selected = false,
                        PlugInDatetime = "",
                        PlugOutDatetime = ""
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetInvoiceNo()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForReefer", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_ReeferPluginRequest> objReeferPlugin = new List<Hdb_ReeferPluginRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objReeferPlugin.Add(new Hdb_ReeferPluginRequest()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        Date = Result["Date"].ToString(),
                        CHA=Result["CHA"].ToString(),
                        Exporter=Result["Exporter"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeId = Convert.ToInt32(Result["PayeeId"]),
                        PayeeName = Result["PayeeName"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        //ContainerNo = Result["ContainerNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objReeferPlugin;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetReeferPaymentSheet(string InvoiceDate, string InvoiceType, int PayeeId, string ContainerXML, int InvoiceId, int PartyId,int Distance,string ExportUnder)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PrevInvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_ReeferInv objInvoice = new Hdb_ReeferInv();
            try
            {
                DataSet ds = DataAccess.ExecuteDataSet("GetReeferPaymentSheetExp", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
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
                foreach (DataRow Result in ds.Tables[1].Rows)
                {
                    objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                    objInvoice.CHAName = Result["CHAName"].ToString();
                    objInvoice.PartyName = Result["CHAName"].ToString();
                    objInvoice.PartyGST = Result["GSTNo"].ToString();
                    objInvoice.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                    objInvoice.RequestNo = Result["StuffingReqNo"].ToString();
                    objInvoice.RequestDate = Result["RequestDate"].ToString();
                    objInvoice.PartyAddress = Result["Address"].ToString();
                    objInvoice.PartyStateCode = Result["StateCode"].ToString();
                    objInvoice.PartyState = Result["PartyState"].ToString();
                }
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

                    });
                    objInvoice.lstPostPaymentCont.Add(new Hdb_PostPaymentContainerRef
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
                        PlugInDatetime = Convert.ToDateTime(Result["PlugInDatetime"]).ToString("dd/MM/yyyy HH:mm"),
                        PlugOutDatetime = Convert.ToDateTime(Result["PlugOutDatetime"]).ToString("dd/MM/yyyy HH:mm"),
                        Shift = Convert.ToInt32(Result["Shift"])
                    });
                }
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
        public void AddEditReeferInvoice(Hdb_ReeferInv ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid)
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

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Distance });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditReeferPluginRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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

        #region BTT Cargo Entry
        public void AddEditBTTCargoEntry(Hdb_BTTCargoEntry ObjBTT, string StuffingXML, int BranchId, int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPrinting", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = ObjBTT.IsPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrint", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.NoOfPrint });
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
        public void GetBTTCargoEntry(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_BTTCargoEntry> ObjBTT = new List<Hdb_BTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.Add(new Hdb_BTTCargoEntry()
                    {
                        BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]),
                        CartingNo = Convert.ToString(Result["CartingAppNo"]),
                        CartingDate = Convert.ToString(Result["CartingDate"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"])

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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_BTTCargoEntry ObjBTT = new Hdb_BTTCargoEntry();
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
                    ObjBTT.IsPrinting = (Result["IsPrinting"] == null ? "" : Result["IsPrinting"]).ToString();
                    ObjBTT.NoOfPrint = Convert.ToInt32(Result["NoOfPrint"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjBTT.lstBTTCargoEntryDtl.Add(new Hdb_BTTCargoEntryDtl
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
                            BTTWeight = Convert.ToInt32(Result["BTTWeight"]),
                            Fob = Convert.ToDecimal(Result["PreviousFob"]),
                            InFob = Convert.ToDecimal(Result["Fob"]),
                            Sqm = Convert.ToDecimal(Result["BTTSqm"]),
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingAppList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_BTTCartingList> lstBTTCartingList = new List<Hdb_BTTCartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingList.Add(new Hdb_BTTCartingList()
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingDetailList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_BTTCartingDetailList> lstBTTCartingDetailList = new List<Hdb_BTTCartingDetailList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingDetailList.Add(new Hdb_BTTCartingDetailList()
                    {
                        CartingDetailId = Convert.ToInt32(Result["CartingAppDtlId"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        Fob = Convert.ToDecimal(Result["FobValue"])
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
            IList<Hdb_CHAList> lstCHA = new List<Hdb_CHAList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Hdb_CHAList
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

        #region BTT Cargo Print
        public void BTTCargoPrint(int BTTId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BTTId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoPrint", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();
            Hdb_BTTCargoEntry ObjBTT = new Hdb_BTTCargoEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjBTT.ExporterImporterName = Result["ExporterImporterName"].ToString();
                    ObjBTT.StorageAmt = Convert.ToDecimal(Result["StorageAmt"]);
                    ObjBTT.HTTotalAmt = Convert.ToDecimal(Result["HTTotalAmt"]);
                    ObjBTT.OthersAmt = Convert.ToDecimal(Result["OthersAmt"]);
                    ObjBTT.Total = (ObjBTT.StorageAmt + ObjBTT.HTTotalAmt + ObjBTT.OthersAmt);

                    ObjBTT.CrNo = Result["ReceiptNo"].ToString();
                    ObjBTT.CrDate = Result["ReceiptDate"].ToString();
                    //ObjBTT.CartingDate = Result["CartingDate"].ToString();
                    //ObjBTT.CHAId = Convert.ToInt32(Result["CHAId"]);
                    //ObjBTT.CHAName = Result["EximTraderName"].ToString();
                    //ObjBTT.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjBTT.lstBTTCargoEntryDtl.Add(new Hdb_BTTCargoEntryDtl
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
                            BTTWeight = Convert.ToInt32(Result["BTTWeight"]),
                            CartingAppNo = Convert.ToString(Result["CartingAppNo"]),
                            CartingDate = Convert.ToString(Result["CartingDate"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
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
        #endregion
        #region Carting Work Order
        public void AddEditCartingWorkOrder(Hdb_CartingWorkOrder ObjWorkOrder)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjWorkOrder.CartingWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjWorkOrder.WorkOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjWorkOrder.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCartingWorkOrder", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjWorkOrder.CartingWorkOrderId == 0 ? "Carting Work Order Details Saved Successfully" : "Carting Work Order Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Carting Work Order Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Carting Work Order Details As It Exist In Carting Register";
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
        public void DeleteCartingWorkOrder(int CartingWorkOrderId, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteCartingWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Carting Work Order Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Page";
                    _DBResponse.Data = GeneratedClientId;
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
        public void GetCartingWorkOrder(int CartingWorkOrderId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_CartingWorkOrder ObjWorkOrder = new Hdb_CartingWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjWorkOrder.CartingWorkOrderId = Convert.ToInt32(Result["CartingWorkOrderId"]);
                    ObjWorkOrder.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    ObjWorkOrder.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjWorkOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjWorkOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjWorkOrder.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjWorkOrder.GodownName = Result["GodownName"].ToString();
                    ObjWorkOrder.CartingNo = Result["CartingNo"].ToString();
                    ObjWorkOrder.CartingDate = Result["CartingDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjWorkOrder.LstCarting.Add(new Hdb_CartingWorkOrder
                        {
                            //CommodityId=Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(Result["Weight"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjWorkOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllCartingWorkOrder(int BranchId,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWorkOrder.Add(new Hdb_CartingWorkOrder
                    {
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CartingNo = Result["CartingNo"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        CartingWorkOrderId = Convert.ToInt32(Result["CartingWorkOrderId"]),
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        CHA = Convert.ToString(Result["CHAName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWorkOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllCartingWorkOrderByDate(String CartingDate,int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if ((CartingDate != null) || CartingDate != "") CartingDate = Convert.ToDateTime(CartingDate).ToString("yyyy-MM-dd");

            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.Date, Value = CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingWorkOrderByDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWorkOrder.Add(new Hdb_CartingWorkOrder
                    {
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CartingNo = Result["CartingNo"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        CartingWorkOrderId = Convert.ToInt32(Result["CartingWorkOrderId"]),
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        CHA = Convert.ToString(Result["CHAName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWorkOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllCartingWorkOrderByCHA(string ShiipingChA,int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShiipingChA", MySqlDbType = MySqlDbType.VarChar,Size=200 ,Value = ShiipingChA });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingWorkOrderByShipping", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWorkOrder.Add(new Hdb_CartingWorkOrder
                    {
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CartingNo = Result["CartingNo"].ToString(),
                        CartingDate = Result["CartingDate"].ToString(),
                        CartingWorkOrderId = Convert.ToInt32(Result["CartingWorkOrderId"]),
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBDate = Convert.ToString(Result["SBDate"]),
                        CHA = Convert.ToString(Result["CHAName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWorkOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCartingNoForWorkOrder(int BranchId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingNoForWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_CartingWorkOrder> LstCartingNo = new List<Hdb_CartingWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingNo.Add(new Hdb_CartingWorkOrder
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingNo;
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
        public void GetCartingDetailForWorkOrder(int CartingAppId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingDetailForWorkOrder", CommandType.StoredProcedure, DParam);
            List<Hdb_CartingWorkOrder> LstCarting = new List<Hdb_CartingWorkOrder>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCarting.Add(new Hdb_CartingWorkOrder
                    {
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        CartingDate = Result["CartingDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCarting;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetDetailsForPrint(int CartingWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CartingWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CartingWOPrint", CommandType.StoredProcedure, DParam);
            List<Hdb_CartingWOPrint> LstCartingDet = new List<Hdb_CartingWOPrint>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingDet.Add(new Hdb_CartingWOPrint
                    {
                        CartingNo = Result["CartingNo"].ToString(),
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ExpName = Result["ExpName"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        GodownName = Result["GodownName"].ToString(),
                        TruckNo = Result["VehicleNo"].ToString(),


                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstCartingDet[0].CompanyAddress = (Result["CompanyAddress"] == null ? "" : Result["CompanyAddress"]).ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingDet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
        #region Stuffing Work Order

        public void AddEditStuffingWorkOrder(Hdb_StuffingWorkOrder ObjStuffing,

string StuffingXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderId",

                MySqlDbType = MySqlDbType.Int32,
                Value = ObjStuffing.WorkOrderId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderDate",

                MySqlDbType = MySqlDbType.Date,
                Value = Convert.ToDateTime

(ObjStuffing.WorkOrderDate).ToString("yyyy-MM-dd")
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName =

"in_StuffingRequestId",
                MySqlDbType = MySqlDbType.Int32,
                Value =

ObjStuffing.StuffingRequestId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName =

"in_StuffingRequestNo",
                MySqlDbType = MySqlDbType.VarChar,
                Value =

ObjStuffing.StuffingRequestNo
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_StuffingDate",

                MySqlDbType = MySqlDbType.Date,
                Value = Convert.ToDateTime

(ObjStuffing.StuffingRequestDate).ToString("yyyy-MM-dd")
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_GodownId",

                MySqlDbType = MySqlDbType.Int32,
                Value = ObjStuffing.GodownId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_Remarks",

                MySqlDbType = MySqlDbType.VarChar,
                Value = ObjStuffing.Remarks
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_BranchId",

                MySqlDbType = MySqlDbType.Int32,
                Value = BranchId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_Uid",
                MySqlDbType

= MySqlDbType.Int32,
                Value = Uid
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "WorkOrderDtlXML",

                MySqlDbType = MySqlDbType.Text,
                Value = StuffingXML
            });

            LstParam.Add(new MySqlParameter
            {
                ParameterName = "RetValue",

                MySqlDbType = MySqlDbType.Int32,
                Direction = ParameterDirection.Output,
                Value = 0

            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "GeneratedClientId",

                MySqlDbType = MySqlDbType.Int32,
                Direction = ParameterDirection.Output,
                Value =

GeneratedClientId
            });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer

();
            int Result = DA.ExecuteNonQuery("AddEditStuffingWorkOrder",

CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Work Order Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Work Order Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Work Order Details Already Exist";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Edit Stuffing Work Order Details As It Exist In Container Stuffing";
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
        public void GetStuffingWorkOrder(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderId",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = 0
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_Page",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = Page
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_StuffingWorkOrder> ObjStuffing = new List<Hdb_StuffingWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.Add(new Hdb_StuffingWorkOrder()
                    {
                        WorkOrderId = Convert.ToInt32(Result["WorkOrderId"]),
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        StuffingRequestNo = Result["StuffingNo"].ToString(),
                        StuffingRequestDate = Result["StuffingDate"].ToString(),
                        OrderType = Result["OrderType"].ToString(),
                        Forwarder = Result["ForwarderName"].ToString(),


                    });
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
        public void GetStuffingWorkOrderById(int StuffingWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderId",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = StuffingWorkOrderId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_Page",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = 0
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =

DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader

("GetStuffingWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Hdb_StuffingWorkOrder ObjStuffing = new Hdb_StuffingWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.WorkOrderId = Convert.ToInt32(Result["WorkOrderId"]);
                    ObjStuffing.StuffingRequestId = Convert.ToInt32(Result["StuffingRequestId"]);
                    ObjStuffing.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    ObjStuffing.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjStuffing.StuffingRequestNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingRequestDate = Result["StuffingDate"].ToString();
                    ObjStuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjStuffing.GodownName = Result["GodownName"].ToString();
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.lstStuffingWorkOrderDtl.Add(new Hdb_StuffingWorkOrderDtl
                        {
                            WorkOrderDetailId = Convert.ToInt32(Result["WorkOrderDtlId"]),
                            WorkOrderId = Convert.ToInt32(Result["WorkOrderId"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            CommodityId = Convert.ToInt32(Result["CommodityID"]),
                            CommodityName = Convert.ToString(Result["CommodityName"]),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnit"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            WeightPerUnit = Convert.ToDecimal(Result["WeightPerUnit"])
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
        public void DeleteStuffingWorkOrder(int StuffingWorkOrderId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderId",

                MySqlDbType = MySqlDbType.Int32,
                Value = StuffingWorkOrderId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "RetValue",

                MySqlDbType = MySqlDbType.Int32,
                Value = 0,
                Direction = ParameterDirection.Output

            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "GeneratedClientId",

                MySqlDbType = MySqlDbType.Int32,
                Value = GeneratedClientId,
                Direction = ParameterDirection.Output
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteStuffingWorkOrder", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Work Order Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Work Order Details Not Found";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Stuffing Work Order Details As It Exists In Container Stuffing";
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
        public void GetStuffingRequestList(int StuffingWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_WorkOrderId",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = StuffingWorkOrderId
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =

DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader

("GetStuffingRequestList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_StuffingNoList> lstStuffingNoList = new

List<Hdb_StuffingNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstStuffingNoList.Add(new Hdb_StuffingNoList()
                    {
                        StuffingRequestId = Convert.ToInt32(Result

["StuffingReqId"]),
                        StuffingRequestNo = Convert.ToString(Result

["StuffingReqNo"]),
                        StuffingRequestDate = Convert.ToString(Result

["RequestDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStuffingNoList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerListByStuffingReqId(int StuffingRequestId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName =

"in_StuffingRequestId",
                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value =

StuffingRequestId
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =

DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader

("GetContainerListByStuffingReqId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_ContainerList> lstContainer = new List<Hdb_ContainerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new Hdb_ContainerList()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        StuffingRequestDetailId = Convert.ToInt32(Result

["StuffingReqDtlId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CargoDescription = Convert.ToString(Result

["CargoDescription"] == null ? "" : Result["CargoDescription"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] ==

DBNull.Value ? 0 : Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value

? 0 : Result["Weight"]),
                        WeightPerUnit = Convert.ToDecimal(Result["WeightPerUnit"] ==

DBNull.Value ? 0 : Result["WeightPerUnit"]),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        StuffingRequestDate = Convert.ToString(Result["RequestDate"]),
                        GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_GodownId",

                MySqlDbType = MySqlDbType.Int32,
                Size = 11,
                Value = 0
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_BranchId",

                MySqlDbType = MySqlDbType.Int32,
                Value = Convert.ToInt32

(HttpContext.Current.Session["BranchId"])
            });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =

DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodown",

CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Hdb_GodownList> lstGodownList = new List<Hdb_GodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Hdb_GodownList()
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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


        public void GetDetailsStufffingWOForPrint(int StuffingWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter
            {
                ParameterName =

"in_StuffingWorkOrderId",
                MySqlDbType = MySqlDbType.Int32,
                Value =

StuffingWorkOrderId
            });
            LstParam.Add(new MySqlParameter
            {
                ParameterName = "in_BranchId",

                MySqlDbType = MySqlDbType.Int32,
                Value = Convert.ToInt32

(HttpContext.Current.Session["BranchId"])
            });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess =

DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("StuffingWOPrint",

CommandType.StoredProcedure, DParam);
            List<StuffingWOPrint> LstStuffingWOPrint = new List<StuffingWOPrint>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingWOPrint.Add(new StuffingWOPrint
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ExpName = Result["ExpName"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingWOPrint[0].CompanyAddress = (Result

["CompanyAddress"] == null ? "" : Result["CompanyAddress"]).ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingWOPrint;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region BTT Carting Register
        public void ContainerNoForBTTCartingRegister()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = "" });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetContNoForCrtReg", CommandType.StoredProcedure, Dparam);
            List<dynamic> lstCont = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    dynamic obj = new System.Dynamic.ExpandoObject();
                    /*lstCont.Add(new
                    {*/
                    obj.CFSCode = result["CFSCode"].ToString();
                    obj.ContainerNo = result["ContainerNo"].ToString();
                    obj.OldCFSCode = result["OldCFSCode"].ToString();
                    lstCont.Add(obj);
                    /*});*/
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
        public void ContainerDtlForBTTCartingRegister(string OldCFSCode)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = OldCFSCode });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetContNoForCrtReg", CommandType.StoredProcedure, Dparam);
            List<dynamic> lstCont = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCont.Add(new
                    {
                        ShippingBillNo = result["ShippingBillNo"].ToString(),
                        ShippingDate = result["ShippingDate"].ToString(),
                        ExporterId = Convert.ToInt32(result["ExporterId"]),
                        Exporter = result["ExporterName"].ToString(),
                        CargoDescription = result["CargoDescription"].ToString(),
                        CommodityId = Convert.ToInt32(result["CommodityId"]),
                        CommodityName = result["CommodityName"].ToString(),
                        CargoType = Convert.ToInt32(result["CargoType"]),
                        MarksAndNo = result["MarksNo"].ToString(),
                        NoOfUnits = Convert.ToInt32(result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(result["Weight"]),
                        CUM = Convert.ToDecimal(result["CUM"]),
                        SQM = Convert.ToDecimal(result["SQM"]),
                        ActualQty = Convert.ToInt32(result["ReceivedPackage"]),
                        ActualWeight = Convert.ToDecimal(result["ActualWeight"]),
                        FoBValue = Convert.ToDecimal(result["Fob"]),
                        CartingAppDtlId=0,
                        LocationDetails="",
                        Location="",
                        Storage=""
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
        public void GetAllBTTRegisterDetails()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllBTTCartingRegister", CommandType.StoredProcedure, Dparam);
            List<Hdb_CartingRegister> lstCR = new List<Hdb_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new Hdb_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        RegisterDate = result["RegisterDate"].ToString(),
                        CHAName = result["CHANAME"].ToString()
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
        public void AddEditBTTCartingRegister(Hdb_BTTCartingRegister objCR, string XML)
        {
            string OutParam = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = objCR.ContainerNo});
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = objCR.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCR.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RegisterDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.RegisterDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objCR.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CHAId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = OutParam });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBTTCartingRegister", CommandType.StoredProcedure, Dparam, out OutParam);
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
                    _DBResponse.Message = "Cannot Update Carting Register Details As It Already Exist In Another Page";
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
        public void GetBTTRegisterDetails(int CartingRegisterId, string Purpose = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Purpose });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllBTTCartingRegister", CommandType.StoredProcedure, Dparam);
            Hdb_BTTCartingRegister objCR = new Hdb_BTTCartingRegister();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objCR.CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]);
                    objCR.CartingRegisterNo = result["CartingRegisterNo"].ToString();
                    objCR.RegisterDate = result["RegisterDate"].ToString();
                    objCR.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                    objCR.GodownName = (result["GodownName"] == null ? "" : result["GodownName"]).ToString();
                    objCR.GodownId = Convert.ToInt32(result["GodownId"] == DBNull.Value ? 0 : result["GodownId"]);
                    objCR.CHAId = Convert.ToInt32(result["CHAId"]);
                    objCR.CHAName = result["CHANAME"].ToString();
                    objCR.CartingType = Convert.ToInt32(result["CartingType"]);
                    objCR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objCR.ContainerNo = result["ContainerNo"].ToString();
                    objCR.CFSCode = result["CFSCode"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstRegisterDtl.Add(new Hdb_BTTCartingRegisterDtl
                        {
                            ShippingBillNo = result["ShippingBillNo"].ToString(),
                            ShippingDate = result["ShippingBillDate"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                            CommodityId=Convert.ToInt32(result["CommodityId"]),
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
                            ExporterId= Convert.ToInt32(result["ExporterId"]),
                            CartingRegisterDtlId =Convert.ToInt32(result["CartingRegisterDtlId"]),
                            CartingAppDtlId = Convert.ToInt32(result["CartingAppDtlId"]),
                            LocationDetails = (result["LocationDetails"] == null ? "" : result["LocationDetails"]).ToString(),
                            Location = (result["Location"] == null ? "" : result["Location"]).ToString(),
                            Storage = (result["StorageArea"] == null ? "" : result["StorageArea"]).ToString(),
                        });
                    }
                }
                if (Purpose == "edit")
                {
                    if (result.NextResult())
                    {
                        while (result.Read())
                        {
                            objCR.lstGdnWiseLctn.Add(new Areas.Export.Models.Hdb_GodownWiseLocation
                            {
                                Row = result["Row"].ToString(),
                                Column = result["Column"].ToString(),
                                LocationId = Convert.ToInt32(result["LocationId"])
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
        public void DeleteBTTCartingRegister(int CartingRegisterId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DelBTTCartingRegister", CommandType.StoredProcedure, Dparam);
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
        public void PrintBTTCartingReg(int CartingRegisterId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("PrintBTTCartingReg", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<PrintCartingReg> LstCartingReg = new List<PrintCartingReg>();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingReg.Add(new PrintCartingReg
                    {
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        RegisterDate = Convert.ToString(Result["RegisterDate"]),
                        ShipBillNo = Result["ShipBillNo"].ToString(),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        CHA = Result["CHA"].ToString(),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        FobValue = Convert.ToDecimal(Result["FobValue"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        SBUnits = Convert.ToInt32(Result["SBUnits"]),
                        BalanceUnits = Convert.ToInt32(Result["BalanceUnits"]),
                        ActualUnits = Convert.ToInt32(Result["ActualUnits"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = LstCartingReg;
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

        #region BTT Container Payment Sheet
        public void GetContainerListForBTTContPS()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetContListForBTTPS", CommandType.StoredProcedure);
            List<dynamic> lstCont = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCont.Add(new
                    {
                        OldCfsCode = result["OldCfsCode"].ToString(),
                        StuffType = result["StuffType"].ToString(),
                        CFSCode = result["CFSCode"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        ArrivalDt = result["ArrivalDt"].ToString(),
                        IsHaz = result["IsHaz"].ToString(),
                        Selected = false
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
        public void GetBTTContPaymentSheet(string InvoiceDate,string ConatinerXML,string InvoiceType,int PartyId,int PayeeId,int CasualLabour,int Distance, int InvoiceId,
            string ExportUnder)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ConatinerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_BttContPS objInvoice = new Hdb_BttContPS();
            List<String> Clause = new List<String>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTContPaymentSheet", CommandType.StoredProcedure, DParam);

            try
            {
                _DBResponse = new DatabaseResponse();
                while (Result.Read())
                {
                    Status = 1;
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
                        objInvoice.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objInvoice.RequestNo = Result["StuffingReqNo"].ToString();
                        objInvoice.RequestDate = Result["RequestDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.IsLocalGST = Convert.ToInt32(Result["IsLocalGST"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstConatiner.Add(new Hdb_BttContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
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
                            AppraisementPerct = Convert.ToInt32(Result["ApprisementPerc"]),
                            LCLFCL = Result["FCLLCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),
                            ISODC=Convert.ToInt32(Result["ISODC"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContCharges.Add(new Hdb_BttContCharges
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
                            Selected=false
                        });
                        if(Result["ChargeType"].ToString()=="HT")
                        {
                            objInvoice.lstContHTCharges.Add(new Hdb_BttContCharges
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
                                Selected = false
                            });
                        }
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmt.Add(new Hdb_BttContwiseCharges
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
                        objInvoice.lstOperationCode.Add(new Hdb_BttContOperationCFSCodeWiseAmt
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
                            Clause = Result["Clause"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContCharges.Where(x => x.Clause == Result["Clause"].ToString()).ToList().ForEach(item => {
                            item.Selected = true;
                        });
                        Clause.Add(Result["Clause"].ToString());
                    }
                }
                objInvoice.ActualApplicable = Clause;
                objInvoice.TotalAmt = objInvoice.lstContCharges.Sum(o => o.Total);                

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
        public void AddEditBTTContInvoice(Hdb_BttContPS ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
          int BranchId, int Uid,string Module)
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
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.Distance });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBTTContInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        #endregion

        public void GetPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<HDB_PartyForPage> lstParty = new List<HDB_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new HDB_PartyForPage
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

        public void GetPaymentPayerForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPayerForPage", CommandType.StoredProcedure, Dparam);
            IList<HDB_PartyForPage> lstPayer = new List<HDB_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePayer = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPayer.Add(new HDB_PartyForPage
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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePayer = Convert.ToBoolean(Result["StatePayer"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPayer, StatePayer };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }


        #region BTT container - Factory stuffed and direct return to factory
        public void GetContainerListForBTTContForFactoryStuffed()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetContainerForFactoryStuffed", CommandType.StoredProcedure);
            List<dynamic> lstCont = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCont.Add(new
                    {
                        OldCfsCode = result["OldCfsCode"].ToString(),
                        StuffType = result["StuffType"].ToString(),
                        CFSCode = result["CFSCode"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        ArrivalDt = result["ArrivalDt"].ToString(),
                        IsHaz = result["IsHaz"].ToString(),
                        Selected = false
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
        public void GetBTTContPaymentSheetForFactoryStuffed(string InvoiceDate, string ConatinerXML, string InvoiceType, int PartyId, int PayeeId, int CasualLabour, int InvoiceId,
            string ExportUnder,string Printing,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ConatinerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Printing", MySqlDbType = MySqlDbType.VarChar, Value = Printing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Hdb_BttContPS objInvoice = new Hdb_BttContPS();
            List<String> Clause = new List<String>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTContPaymentSheetForStuffContainer", CommandType.StoredProcedure, DParam);

            try
            {
                _DBResponse = new DatabaseResponse();
                while (Result.Read())
                {
                    Status = 1;
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
                        objInvoice.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objInvoice.RequestNo = Result["StuffingReqNo"].ToString();
                        objInvoice.RequestDate = Result["RequestDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.IsLocalGST = Convert.ToInt32(Result["IsLocalGST"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstConatiner.Add(new Hdb_BttContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
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
                            AppraisementPerct = Convert.ToInt32(Result["ApprisementPerc"]),
                            LCLFCL = Result["FCLLCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),
                            ISODC = Convert.ToInt32(Result["ISODC"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContCharges.Add(new Hdb_BttContCharges
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
                            Selected = false
                        });
                        if (Result["ChargeType"].ToString() == "HT")
                        {
                            objInvoice.lstContHTCharges.Add(new Hdb_BttContCharges
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
                                Selected = false
                            });
                        }
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmt.Add(new Hdb_BttContwiseCharges
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
                        objInvoice.lstOperationCode.Add(new Hdb_BttContOperationCFSCodeWiseAmt
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
                            Clause = Result["Clause"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContCharges.Where(x => x.Clause == Result["Clause"].ToString()).ToList().ForEach(item => {
                            item.Selected = true;
                        });
                        Clause.Add(Result["Clause"].ToString());
                    }
                }
                objInvoice.ActualApplicable = Clause;
                objInvoice.TotalAmt = objInvoice.lstContCharges.Sum(o => o.Total);

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
        public void AddEditBTTContInvoiceForFactoryStuffed(Hdb_BttContPS ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
          int BranchId, int Uid, string Module)
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
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBTTContFactoryStuffInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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


        public void GetFactoryStuffinvoiceList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFactoryStuffContainerInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<LoadedContainerStuffList> objPaymentPartyName = new List<LoadedContainerStuffList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new LoadedContainerStuffList()
                    {
                       
                        PartyName = Convert.ToString(Result["PartyName"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceID = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
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
        #endregion


        #region Shippingbill Amendment
        public void GetAmenSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBList", CommandType.StoredProcedure);
            List<Hdb_Amendment> LstSB = new List<Hdb_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_Amendment
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
     

        public void GetAmenSBListSearch(string SbNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SbNo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBListSearch", CommandType.StoredProcedure, DParam);
            List<Hdb_Amendment> LstSB = new List<Hdb_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_Amendment
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
            List<Hdb_Amendment> LstSB = new List<Hdb_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Convert.ToString(Result["SBDate"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        FOBValue = Convert.ToString(Result["FOB"]),
                        Pkg = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CartingAppId = Convert.ToString(Result["CartingAppId"]),
                        Cargo = Convert.ToString(Result["Cargo"]),
                        CommodityId = Convert.ToInt32((Result["CommodityId"]) ?? 0),
                        ExporterId = Convert.ToInt32((Result["ExporterId"]) ?? 0),
                        Area = Convert.ToString((Result["SQM"]) ?? 0),

                        IsApprove = Convert.ToInt32((Result["IsApproved"]) ?? 0),
                        Cutting = Convert.ToInt32((Result["isCutting"]) ?? 0),
                        ShortCargo = Result["ShortCargo"].ToString(),
                        CartingNo = Result["CartingNo"].ToString(),
                        SBType = Result["SBType"].ToString(),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        GodownName = Result["GodownName"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        PostOfDest = Result["PostOfDest"].ToString(),
                        ShippingLineId = Convert.ToInt32((Result["ShippingLineId"]) ?? 0),

                        GodownId = Convert.ToInt32((Result["GodownId"]) ?? 0),
                        PortOfLoadingId = Convert.ToInt32((Result["PortOfLoadingId"]) ?? 0),
                        PortofDestId = Convert.ToInt32((Result["PortofDestId"]) ?? 0),

                        PackUQCCode = Result["PackUQCCode"].ToString(),
                        PackUQCDescription = Result["PackUQCDesc"].ToString(),

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

        public void AddEditAmendment(string OldSBNoValue, string NewSBNoValue, string Date, string FlagMerger)
        {
         //   InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentId", MySqlDbType = MySqlDbType.String, Value = AmendmentNO });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldSBNoXML", MySqlDbType = MySqlDbType.Text, Value = OldSBNoValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewSBNoXML", MySqlDbType = MySqlDbType.Text, Value = NewSBNoValue });

           
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


        public void AddEditShipAmendment(Hdb_AmendmentViewModel vm)
        {
         //   vm.InvoiceDate = DateTime.ParseExact(vm.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
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

      //      lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = vm.InvoiceId });
       //     lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = vm.InvoiceNo });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.VarChar, Value = vm.InvoiceDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SbType", MySqlDbType = MySqlDbType.Int32, Value = vm.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = vm.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortLoadingId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfLoadingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = vm.GodownId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.VarChar, Value = vm.GodownName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = vm.ShippingLineName });

            lstParam.Add(new MySqlParameter { ParameterName = "in_PortDestId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfDestId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = vm.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = vm.PackUQCDescription });

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
            List<Hdb_AmendmentViewModel> LstSB = new List<Hdb_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_AmendmentViewModel
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
            List<Hdb_Amendment> LstSB = new List<Hdb_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_Amendment
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
            List<Ppg_Amendment> LstSB = new List<Ppg_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Ppg_Amendment
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
            List<Hdb_AmendmentViewModel> LstSB = new List<Hdb_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Hdb_AmendmentViewModel
                    {
                        ID = Convert.ToInt32(Result["id"]),
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        /*OldShipBillDate = Convert.ToString(Result["AmendDate"])*/
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
            Hdb_AmendmentViewModel LstSB = new Hdb_AmendmentViewModel();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB = (new Hdb_AmendmentViewModel
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

                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        PortOfDest = Result["PortOfDest"].ToString(),
                        GodownName = Result["GodownName"].ToString(),

                        SBTypeName = Result["SBTypeName"].ToString(),
                        PackUQCDescription = Result["PackUQCDesc"].ToString(),

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


        public void ListOfShippingLineName()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfShippingLine", CommandType.StoredProcedure);
            IList<ShipingLine> lstShipping = new List<ShipingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShipping.Add(new ShipingLine
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
            List<CwcExim.Models.Godown> LstGodown = new List<CwcExim.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new CwcExim.Models.Godown
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


        public void ListOfExporterForSBAmend()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporterForSBAmend", CommandType.StoredProcedure);
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
        #endregion

        public void GetIRNForExport(String InvoiceNo)
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
                            PrdDesc =null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null,//Result["Unit"].ToString(),
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
        public void GetIRNB2CForExport(String InvoiceNo)
        {
            Areas.Export.Models.Hdb_IrnB2CDetails Obj = new Areas.Export.Models.Hdb_IrnB2CDetails();

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
                    log.Info("After Calling Stored Procedure Error" + " Invoice No " + InvoiceNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }


        public void GetHeaderIRNForExport()
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
            IList<Hdb_ListOfExpInvoice> lstExpInvoice = new List<Hdb_ListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new Hdb_ListOfExpInvoice()
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


        #region SCMRT
        public void ListOfFinalDestination(string CustodianName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianName", MySqlDbType = MySqlDbType.VarChar, Value = CustodianName });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFinalDestination", CommandType.StoredProcedure, DParam);


            List<HDB_FinalDestination> LstCustodian = new List<HDB_FinalDestination>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCustodian.Add(new HDB_FinalDestination
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
            IList<Hdb_ContainerStuffingApproval> objPaySheetStuffing = new List<Hdb_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Hdb_ContainerStuffingApproval()
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
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objPortOfCall.ApprovalId) });
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
                    _DBResponse.Message = result==1?"Stuffing Approved Successfully": "Stuffing Approved Update Successfully";
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
                    objDestuffing.ModeOfTransport= Convert.ToInt32(Result["ModeOfTransportId"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["ContainerStuffingId"]);
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
            Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRStuffingDetailsV2", CommandType.StoredProcedure, DParam);
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
            IList<Hdb_ContainerStuffingApproval> objPaySheetStuffing = new List<Hdb_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Hdb_ContainerStuffingApproval()
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
            IList<Hdb_ContainerStuffingApproval> objPaySheetStuffing = new List<Hdb_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Hdb_ContainerStuffingApproval()
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


        public void AddEditLoadContainerStuffingSF(Hdb_LoadContSF objPortOfCall, int Uid)
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
                    _DBResponse.Message = "Loaded Container For SF Saved Successfully" ;

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
            List<Hdb_LoadContSF> LstStuffingApproval = new List<Hdb_LoadContSF>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new Hdb_LoadContSF
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
            Hdb_LoadContSF objDestuffing = new Hdb_LoadContSF();
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

        #region Get Load Container CIM-ASR Details

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
                    _DBResponse.Data = new {ContainerList= objContainerNoForActualArrival, State }; ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        public void AddEditActualArrivalDatetime(HDB_ActualArrivalDatetime objActualArrivalDatetime)
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
        public void GetListOfArrivalDatetime(int Uid,int Id)
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
            IList<HDB_ActualArrivalDatetime> objArrivalDatetimeList = new List<HDB_ActualArrivalDatetime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objArrivalDatetimeList.Add(new HDB_ActualArrivalDatetime()
                    {
                        Id= Convert.ToInt32(Result["Id"]),
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

        #region LoaderContUpdate
        public void GetContainerNoORLoadContReefer(string ContainerBoxSearch, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerBoxSearch });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoORLoadContReefer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerNoForActualArrival> objContainerLoad = new List<ContainerNoForActualArrival>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    objContainerLoad.Add(new ContainerNoForActualArrival()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                      // State = Convert.ToBoolean(Result["State"])

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
                    _DBResponse.Data = new { ContainerList = objContainerLoad, State }; ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        public void SaveUpdateContainerLoaderReefer(HDB_ActualArrivalDatetime objActualArrivalDatetime)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = objActualArrivalDatetime.Reefer });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("UpdateContainerLoaderReefer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "UpdateReefer Saved Successfully" : "UpdateReefer Updated Successfully";
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
