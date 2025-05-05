using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.DAL;
using CwcExim.UtilityClasses;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Bond.Models;
namespace CwcExim.Repositories
{
    public class BondRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
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
        public void ListOfImporter()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImporter", CommandType.StoredProcedure, dparam);
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

        #region Application for availability of space
        public void ListOfSpaceAvailability()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ListOfBondApp> lstApp = new List<ListOfBondApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstApp.Add(new ListOfBondApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        ImporterLicenseNo = Result["ImporterLicenseNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        BOENo=Result["BOENo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void SpaceAvailabilityDetails(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BondApp objBA = new BondApp();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBA.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    objBA.ApplicationNo = Result["ApplicationNo"].ToString();
                    objBA.ApplicationDate = Result["ApplicationDate"].ToString();
                    objBA.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objBA.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    objBA.ImporterLicenseNo = Result["ImporterLicenseNo"].ToString();
                    objBA.BOLAWBNo = Result["BOLAWBNo"].ToString();
                    objBA.BOENo = Result["BOENo"].ToString();
                    objBA.BOEDate = Result["BOEDate"].ToString();
                    objBA.CargoDescription = Result["CargoDescription"].ToString();
                    objBA.NatureOfPackages = Result["NatureOfPackages"].ToString();
                    objBA.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBA.NatureOfMaterial = Convert.ToInt32(Result["NatureOfMaterial"]);
                    objBA.DimensionPerUnit = Convert.ToDecimal(Result["DimensionPerUnit"]);
                    objBA.Weight = Convert.ToDecimal(Result["Weight"]);
                    objBA.SpaceReq = Convert.ToDecimal(Result["SpaceReq"]);
                    objBA.AssessCIFvalue = Convert.ToDecimal(Result["AssessCIFvalue"]);
                    objBA.DutyAmt = Convert.ToDecimal(Result["DutyAmt"]);
                    objBA.ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString();
                    objBA.CHAName = Result["CHAName"].ToString();
                    objBA.ImporterName = Result["ImporterName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBA;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditBondApp(BondApp objBA)
        {
            string ApplicationNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationNo", MySqlDbType = MySqlDbType.VarChar, Value = objBA.ApplicationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objBA.ApplicationDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterLicenseNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.ImporterLicenseNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOLAWBNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.BOLAWBNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objBA.BOEDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objBA.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NatureOfPackages", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objBA.NatureOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfUnits", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.NoOfUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NatureOfMaterial", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.NatureOfMaterial });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DimensionPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = objBA.DimensionPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objBA.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceReq", MySqlDbType = MySqlDbType.Decimal, Value = objBA.SpaceReq });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AssessCIFvalue", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.AssessCIFvalue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DutyAmt", MySqlDbType = MySqlDbType.Decimal, Value = objBA.DutyAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpDateofWarehouse", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objBA.ExpDateofWarehouse) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Value = 0, Size = 30, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSpcAvlApp", CommandType.StoredProcedure, DParam, out ApplicationNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Bond Application Saved Successfully" : "Bond Application Updated Successfully";
                    _DBResponse.Data = ApplicationNo;
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
        public void DeleteBondApp(int SpaceappId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteSpcAvailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Bond Application Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Delete Application";
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

        #region Space Availability Certificate
        public void AddEditBondSpaceAvailCert(SpaceAvailableCert ObjSpaceAvail)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjSpaceAvail.SacNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.SacDate).ToString("yyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaReserved", MySqlDbType = MySqlDbType.Decimal, Value = ObjSpaceAvail.AreaReserved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ValidUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.ValidUpto).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSubmitted", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.IsSubmitted });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApprovedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.ApprovedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBondSpaceAvailCert", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate Details Saved Successfully";
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
        public void GetSpaceAvailCert(int Status, int Skip)
        {
            int status = 0;
            bool State = false;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.Int32, Value = Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCert", CommandType.StoredProcedure, DParam);
            List<ListOfBondApp> LstSpaceAvail = new List<ListOfBondApp>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstSpaceAvail.Add(new ListOfBondApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstSpaceAvail, State = State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSpaceAvailCertById(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SpaceAvailableCert ObjSpaceAvail = new SpaceAvailableCert();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ApplicationNo = Result["ApplicationNo"].ToString();
                    ObjSpaceAvail.ApplicationDate = Result["ApplicationDate"].ToString();
                    ObjSpaceAvail.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjSpaceAvail.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    ObjSpaceAvail.ImporterLicenseNo = Result["ImporterLicenseNo"].ToString();
                    ObjSpaceAvail.BOLAWBNo = Result["BOLAWBNo"].ToString();
                    ObjSpaceAvail.BOENo = Result["BOENo"].ToString();
                    ObjSpaceAvail.BOEDate = Result["BOEDate"].ToString();
                    ObjSpaceAvail.CargoDescription = Result["CargoDescription"].ToString();
                    ObjSpaceAvail.NatureOfPackages = Result["NatureOfPackages"].ToString();
                    ObjSpaceAvail.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    ObjSpaceAvail.NatureOfMaterial = Convert.ToInt32(Result["NatureOfMaterial"]);
                    ObjSpaceAvail.DimensionPerUnit = Convert.ToDecimal(Result["DimensionPerUnit"]);
                    ObjSpaceAvail.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjSpaceAvail.SpaceReq = Convert.ToDecimal(Result["SpaceReq"]);
                    ObjSpaceAvail.AssessCIFvalue = Convert.ToDecimal(Result["AssessCIFvalue"]);
                    ObjSpaceAvail.DutyAmt = Convert.ToDecimal(Result["DutyAmt"]);
                    ObjSpaceAvail.ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString();
                    ObjSpaceAvail.CHAName = Result["CHAName"].ToString();
                    ObjSpaceAvail.ImporterName = Result["ImporterName"].ToString();
                    ObjSpaceAvail.SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString();
                    ObjSpaceAvail.SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString();
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"] == DBNull.Value ? 0 : Result["AreaReserved"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"] == null ? "" : Result["ValidUpto"]);
                    ObjSpaceAvail.IsApproved = Convert.ToInt32(Result["IsApproved"] == DBNull.Value ? 0 : Result["IsApproved"]);
                    ObjSpaceAvail.IsSubmitted = Convert.ToInt32(Result["IsSubmitted"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSpaceAvailCertForPrint(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SpaceAvailCertPdf ObjSpaceAvailCert = new SpaceAvailCertPdf();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvailCert.Importer = Result["Importer"].ToString();
                    ObjSpaceAvailCert.BOLAWBNo = (Result["BOLAWBNo"] == null ? "" : Result["BOLAWBNo"]).ToString();
                    ObjSpaceAvailCert.BOENoDate = (Result["BOENoDate"] == null ? "" : Result["BOENoDate"]).ToString();
                    ObjSpaceAvailCert.SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString();
                    ObjSpaceAvailCert.SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString();
                    ObjSpaceAvailCert.AreaReserved = Convert.ToDecimal(Result["AreaReserved"] == DBNull.Value ? 0 : Result["AreaReserved"]);
                    ObjSpaceAvailCert.ValidUpto = Convert.ToString(Result["ValidUpto"] == null ? "" : Result["ValidUpto"]);
                    ObjSpaceAvailCert.CHAName = Result["CHAName"].ToString(); 
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvailCert;
                }
                else
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

        #region Space Availability Certificate (Extend)
        public void AddEditBondSpaceAvailCertExt(SpaceAvailCertExtend ObjSpaceAvail)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaReserved", MySqlDbType = MySqlDbType.Decimal, Value = ObjSpaceAvail.AreaReserved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExtendOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.ExtendOn).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExtendUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.ExtendUpto).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("AddEditBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjSpaceAvail.SpaceAvailCertExtId == 0 ? "Space Availability Certificate (Extend) Details Saved Successfully" : "Space Availability Certificate (Extend) Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Space Availability Certificate (Extend) Details Already Exist";
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
        public void DelBondSpaceAvailCertExt(int SpaceAvailCertExtId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("DelBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate (Extend) Details Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Space Availability Certificate (Extend) Details As It Already Exist In Another Page";
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
        public void GetAllSpaceAvailCertExt()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<SpaceAvailCertExtend> LstSpaceAvail = new List<SpaceAvailCertExtend>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSpaceAvail.Add(new SpaceAvailCertExtend
                    {
                        SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]),
                        SacNo = Convert.ToString(Result["SacNo"]),
                        ApplicationNo = Result["ApplicationNo"].ToString(),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString()
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSpaceAvailCertExt(int SpaceAvailCertExtId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SpaceAvailCertExtend ObjSpaceAvail = new SpaceAvailCertExtend();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ExtendOn = Convert.ToString(Result["ExtendOn"]);
                    ObjSpaceAvail.ExtendUpto = Convert.ToString(Result["ExtendUpto"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSacNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<SpaceAvailableCert> LstSpaceAvail = new List<SpaceAvailableCert>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSpaceAvail.Add(new SpaceAvailableCert
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        SacNo = Result["SacNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSacNoDet(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNoDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SpaceAvailableCert ObjSpaceAvail = new SpaceAvailableCert();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"] == null ? "" : Result["ImporterName"]);
                    ObjSpaceAvail.BOENo = Convert.ToString(Result["BOENo"] == null ? "" : Result["BOENo"]);
                    ObjSpaceAvail.BOEDate = Convert.ToString(Result["BOEDate"] == null ? "" : Result["BOEDate"]);
                    ObjSpaceAvail.ImporterId = Convert.ToInt32(Result["ImporterId"]==DBNull.Value?0:Result["ImporterId"]);
                    ObjSpaceAvail.CargoDescription = Convert.ToString(Result["CargoDescription"] == null ? "" : Result["CargoDescription"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Approval for Space Availability Criteria (Extend)
        public void ListOfSpaceAvailabilityExt(string Status, int Skip)
        {
            int status = 0;
            bool State = false;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("bondAppExtendList", CommandType.StoredProcedure, DParam);
            List<BondAppExtendList> LstDtl = new List<BondAppExtendList>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstDtl.Add(new BondAppExtendList
                    {
                        SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]),
                        SacNo = Convert.ToString(Result["SacNo"]),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstDtl, State = State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSpaceAvailabilityExt(int SpaceAvailCertExtId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BondAppExtendDetails ObjSpaceAvail = new BondAppExtendDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ExtendOn = Convert.ToString(Result["ExtendOn"]);
                    ObjSpaceAvail.ExtendUpto = Convert.ToString(Result["ExtendUpto"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
                    ObjSpaceAvail.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void UpdateAppExtend(int SpaceAvailCertExtId, int IsApproved)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("bondUpdateAppExtend", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate (Extend) Details Saved Successfully";
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
        public void PrintSACExt(int SpaceAvailCertExtId,int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintSACExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PrintSACExt ObjSpaceAvail = new PrintSACExt();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.BOLAWBNo = Convert.ToString(Result["BOLAWBNo"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.BOE = Convert.ToString(Result["BOE"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.ExtendUpto = Convert.ToString(Result["ExtendUpto"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region  Deposit Application
        public void AddEditBondDepositApp(DepositApp ObjDepositApp)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.DepositDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondBOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.BondBOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondBOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.BondBOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.WRDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Units", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Units });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Duty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDepositApp.IsInsured) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("AddEditBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDepositApp.DepositAppId == 0 ? "Deposit Application Details Saved Successfully" : "Deposit Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Deposit Application Details Already Exist";
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
        public void GetAllDepositApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DepositApp> LstDepositApp = new List<DepositApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositApp.Add(new DepositApp
                    {
                        DepositNo = Convert.ToString(Result["DepositNo"]),
                        DepositDate = Convert.ToString(Result["DepositDate"]),
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString(),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetDepositApp(int DepositAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DepositApp ObjDepositApp = new DepositApp();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDepositApp.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    ObjDepositApp.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjDepositApp.DepositNo = Convert.ToString(Result["DepositNo"]);
                    ObjDepositApp.DepositDate = Convert.ToString(Result["DepositDate"]);
                    ObjDepositApp.BondBOENo = Convert.ToString(Result["BondBOENo"] == null ? "" : Result["BondBOENo"]);
                    ObjDepositApp.BondBOEDate = Convert.ToString(Result["BondBOEDate"] == null ? "" : Result["BondBOEDate"]);
                    ObjDepositApp.WRNo = Convert.ToString(Result["WRNo"] == null ? "" : Result["WRNo"]);
                    ObjDepositApp.WRDate = Convert.ToString(Result["WRDate"] == null ? "" : Result["WRDate"]);
                    ObjDepositApp.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDepositApp.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDepositApp.CargoDescription = Convert.ToString(Result["CargoDescription"] == null ? "" : Result["CargoDescription"]);
                    ObjDepositApp.Units = Convert.ToInt32(Result["Units"]);
                    ObjDepositApp.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjDepositApp.Value = Convert.ToDecimal(Result["Value"]);
                    ObjDepositApp.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDepositApp.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjDepositApp.IsInsured = Convert.ToBoolean(Result["IsInsured"]);
                    ObjDepositApp.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjDepositApp.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjDepositApp.CHA = Convert.ToString(Result["CHA"]);
                    ObjDepositApp.Importer = Convert.ToString(Result["Importer"]);
                    ObjDepositApp.GodownName = Convert.ToString(Result["GodownName"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSacNoForDepositApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSacNoForDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DepositApp> LstDepositApp = new List<DepositApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositApp.Add(new DepositApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        SacNo = Result["SacNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void PrintDA(int DepositeAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositeAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintDepositeApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PrintDA ObjDepositApp = new PrintDA();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDepositApp.DepositeNo = Convert.ToString(Result["DepositNo"]);
                    ObjDepositApp.DepositeDate = Convert.ToString(Result["DepositDate"]);
                    ObjDepositApp.BondNo = Result["BondNo"].ToString();
                    ObjDepositApp.BondDt = Convert.ToString(Result["BondDt"] == null ? "" : Result["BondDt"]);
                    ObjDepositApp.WRNo = Convert.ToString(Result["WRNo"] == null ? "" : Result["WRNo"]);
                    ObjDepositApp.WRDt = Convert.ToString(Result["WRDate"] == null ? "" : Result["WRDate"]);
                    ObjDepositApp.BondBOENo = Result["BondBOENo"].ToString();
                    ObjDepositApp.BondBOEDate = Convert.ToString(Result["BondBOEDate"] == null ? "" : Result["BondBOEDate"]);
                    ObjDepositApp.GodownName = Convert.ToString(Result["GodownName"]);
                }
                if(Result.NextResult())
                {
                    while(Result.Read())
                    {
                        ObjDepositApp.lstSAC.Add(new SACDet
                        {
                            SacNo=Result["SacNo"].ToString(),
                            SacDate=Result["SacDate"].ToString(),
                            ImporterName=Result["ImporterName"].ToString(),
                            CHAName=Result["CHAName"].ToString(),
                            CargoDescription=Result["CargoDescription"].ToString(),
                            NoOfUnits=Convert.ToInt32(Result["NoOfUnits"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositApp;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        //public void GetSacNoDetForDepositApp(int DepositAppId)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNoDet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    SpaceAvailableCert ObjSpaceAvail = new SpaceAvailableCert();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
        //            ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
        //            ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
        //            ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
        //            ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
        //            ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
        //            ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
        //            ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = ObjSpaceAvail;
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

        #region Work Order For Unloading & Delivery
        public void GetBondNoForWODelivery()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondForWO", CommandType.StoredProcedure, DParam);
            List<ListOfBondNo> LstBond = new List<ListOfBondNo>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfBondNo
                    {
                        BondBOENo = Convert.ToString(Result["BondBOENo"]),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetDetailsforBondNo(int DepositAppId)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondForWO", CommandType.StoredProcedure, DParam);
            BondWODeli objWO = new BondWODeli();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objWO.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    objWO.BondDate = Convert.ToString(Result["BondBOEDate"]);
                    objWO.WRNo = Result["WRNo"].ToString();
                    objWO.WRDate = Result["WRDate"].ToString();
                    objWO.GodownName = Result["GodownName"].ToString();
                    objWO.CargoDescription = Result["CargoDescription"].ToString();
                    objWO.SacNo = Result["SacNo"].ToString();
                    objWO.SacDate = Result["SacDate"].ToString();
                    objWO.Units = Convert.ToInt32(Result["Units"]);
                    objWO.Weight = Convert.ToDecimal(Result["Weight"]);
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objWO;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListOfWODeli()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondWOId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfBondWO", CommandType.StoredProcedure, DParam);
            List<ListOfBondWODeli> LstBond = new List<ListOfBondWODeli>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfBondWODeli
                    {
                        BondWOId = Convert.ToInt32(Result["BondWOId"]),
                        BondBOENo = Result["BondBOENo"].ToString(),
                        BondBOEDate = Result["BondBOEDate"].ToString(),
                        WorkOrderNo = Convert.ToString(Result["WorkOrderNo"]),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        SacNo = Result["SacNo"].ToString()
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListOfWODeliDetails(int BondWOId)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondWOId", MySqlDbType = MySqlDbType.Int32, Value = BondWOId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfBondWO", CommandType.StoredProcedure, DParam);
            BondWODeli objWO = new BondWODeli();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objWO.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    objWO.WorkOrderFor = Result["WorkOrderFor"].ToString();
                    objWO.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    objWO.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    objWO.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    objWO.BondNo = Convert.ToString(Result["BondBOENo"]);
                    objWO.BondDate = Result["BondBOEDate"].ToString();
                    objWO.WRNo = Convert.ToString(Result["WRNo"]);
                    objWO.WRDate = Result["WRDate"].ToString();
                    objWO.DeliveryNo = Result["DeliveryNo"].ToString();
                    objWO.GodownName = Result["GodownName"].ToString();
                    objWO.CargoDescription = Result["CargoDescription"].ToString();
                    objWO.SacNo = Result["SacNo"].ToString();
                    objWO.SacDate = Result["SacDate"].ToString();
                    objWO.Units = Convert.ToInt32(Result["Units"]);
                    objWO.Weight = Convert.ToDecimal(Result["Weight"]);
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objWO;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditWODeliDetails(int BondWOId, int DepositAppId, string WorkOrderFor, string WorkOrderDate, string DeliveryNo,int CargoUnits,decimal CargoWeight)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondWOId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BondWOId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(WorkOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = DeliveryNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderFor", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = WorkOrderFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoUnits", MySqlDbType = MySqlDbType.Int32, Value = CargoUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWeight", MySqlDbType = MySqlDbType.Int32, Value = CargoWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBondWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Bond Work Order Saved Successfully" : "Bond Work Order Updated Successfully";
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

        #region Unloading at Bonded Warehouse
        public void GetCFSCodeForWOUnloading()
        {
            int status = 0;
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCFSCodeforBondUnloading", CommandType.StoredProcedure);
            List<BondUnloadingCFSCode> LstBond = new List<BondUnloadingCFSCode>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new BondUnloadingCFSCode
                    {
                        //WorkOrderNo = Convert.ToString(Result["WorkOrderNo"]),
                        //BondWOId = Convert.ToInt32(Result["BondWOId"])
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetBondNoForWOUnloading()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondNoUnloading", CommandType.StoredProcedure, DParam);
            List<ListOfBOENo> LstBond = new List<ListOfBOENo>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfBOENo
                    {
                        //WorkOrderNo = Convert.ToString(Result["WorkOrderNo"]),
                        //BondWOId = Convert.ToInt32(Result["BondWOId"])
                        DepositNo = Convert.ToString(Result["DepositNo"]),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetBondNoForWOUnloadingDet(int DepositAppId)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondNoUnloading", CommandType.StoredProcedure, DParam);
            BOENoDetails objBOE = new BOENoDetails();
            _DBResponse = new DatabaseResponse();
            List<GodownDetails> lstGodown = new List<GodownDetails>();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objBOE.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    objBOE.DepositDate = Result["DepositDate"].ToString();
                    //  objBOE.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    objBOE.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    objBOE.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    objBOE.WRNo = Result["WRNo"].ToString();
                    objBOE.WRDate = Result["WRDate"].ToString();
                    objBOE.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objBOE.GodownName = Result["GodownName"].ToString();
                    objBOE.CargoDescription = Result["CargoDescription"].ToString();
                    objBOE.SacNo = Result["SacNo"].ToString();
                    objBOE.SacDate = Result["SacDate"].ToString();
                    objBOE.Units = Convert.ToInt32(Result["Units"]);
                    objBOE.Weight = Convert.ToDecimal(Result["Weight"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstGodown.Add(new GodownDetails { LocationId = Convert.ToInt32(Result["LocationId"]), LocationName = Result["LocationName"].ToString() });
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { BOE = objBOE, lstGodown };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void ListForWOUnloading()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = "" });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfWOUnloading", CommandType.StoredProcedure, DParam);
            List<ListOfWOunloading> LstBond = new List<ListOfWOunloading>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfWOunloading
                    {
                        UnloadingId = Convert.ToInt32(Result["UnloadingId"]),
                        BondBOENo = Convert.ToString(Result["BondBOENo"]),
                        BondBOEDate = Convert.ToString(Result["BondBOEDate"]),
                        DepositNo = Result["DepositNo"].ToString(),
                        DepositDate = Result["DepositDate"].ToString(),
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void WOUnloadingDetails(int UnloadingId, string Action = "")
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = UnloadingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = Action });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfWOUnloading", CommandType.StoredProcedure, DParam);
            BondWOUnloading objBond = new BondWOUnloading();
            List<GodownDetails> lstGodown = new List<GodownDetails>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objBond.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    objBond.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    objBond.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objBond.UnloadingId = Convert.ToInt32(Result["UnloadingId"]);
                    objBond.DepositNo = Result["DepositNo"].ToString();
                    objBond.DepositDate = Result["DepositDate"].ToString();
                    objBond.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    objBond.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    objBond.WRNo = Result["WRNo"].ToString();
                    objBond.WRDate = Result["WRDate"].ToString();
                    objBond.GodownName = Result["GodownName"].ToString();
                    objBond.SacNo = Result["SacNo"].ToString();
                    objBond.SacDate = Result["SacDate"].ToString();
                    objBond.UnloadedDate = Result["UnloadedDate"].ToString();
                    objBond.LocationName = Result["LocationName"].ToString();
                    objBond.LocationId = Result["LocationId"].ToString();
                    objBond.AreaOccupied = Convert.ToDecimal(Result["AreaOccupied"]);
                    objBond.CargoDescription = Result["CargoDescription"].ToString();
                    objBond.UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]);
                    objBond.UnloadedWeights = Convert.ToDecimal(Result["UnloadedWeights"]);
                    objBond.BalancedUnits = Convert.ToInt32(Result["BalancedUnits"]);
                    objBond.BalancedWeights = Convert.ToDecimal(Result["BalancedWeights"]);
                    objBond.PackageCondition = Result["PackageCondition"].ToString();
                    objBond.Remarks = Result["Remarks"].ToString();
                    objBond.Units = Convert.ToInt32(Result["Units"]);
                    objBond.Weight = Convert.ToDecimal(Result["Weight"]);
                }
                if (Result.NextResult())
                {
                    objBond.CfsCodeList = new List<BondUnloadingCFSCode>();
                    while (Result.Read())
                    {
                        objBond.CfsCodeList.Add(new BondUnloadingCFSCode {
                            Id = Convert.ToInt32(Result["Id"]),
                            UnloadingId = Convert.ToInt32(Result["UnloadingId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo= Result["ContainerNo"].ToString(),
                            Selected=true
                        });
                    }
                    objBond.CfsCodes = Newtonsoft.Json.JsonConvert.SerializeObject(objBond.CfsCodeList);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstGodown.Add(new GodownDetails { LocationId = Convert.ToInt32(Result["LocationId"]), LocationName = Result["LocationName"].ToString() });
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (Action == "") _DBResponse.Data = objBond;
                    else _DBResponse.Data = new { objBond, lstGodown };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditWOUnloading(BondWOUnloading obj,string ContainerXML="")
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = obj.UnloadingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = obj.DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = obj.SpaceAppId });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(obj.WRDate).ToString("yyyy-MM-dd") });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = obj.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(obj.UnloadedDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = obj.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = obj.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaOccupied", MySqlDbType = MySqlDbType.Decimal, Value = obj.AreaOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedUnits", MySqlDbType = MySqlDbType.Int32, Value = obj.UnloadedUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedWeights", MySqlDbType = MySqlDbType.Decimal, Value = obj.UnloadedWeights });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalancedUnits", MySqlDbType = MySqlDbType.Int32, Value = obj.BalancedUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalancedWeights", MySqlDbType = MySqlDbType.Decimal, Value = obj.BalancedWeights });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PackageCondition", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = obj.PackageCondition });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = obj.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditWOUnloading", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Unloading Work Order Saved Successfully" : "Unloading Work Order Updated Successfully");
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
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

        #region Bond Advance Payment Sheet
        public void GetSACNoForAdvBondPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACNoForBondAdvPaymentSheet");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }



        public void GetPrevioudStorage(String SaCNo)
        {
            int Status = 0;
           
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            IDataReader Result=null;
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_SACNO", MySqlDbType = MySqlDbType.VarChar, Value = SaCNo });
                //var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACForBondUnloading");
                IDataParameter[] DParam = LstParam.ToArray();
                 Result = DataAccess.ExecuteDataReader("GetPrevStorageCharges", CommandType.StoredProcedure, DParam);
                Decimal StorageCharge = 0;
               
                while (Result.Read())
                {
                    
                        StorageCharge = Convert.ToDecimal(Result["PrevStorageCharge"]);
                   
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = StorageCharge;
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
        public void AddEditInvoice(BondPostPaymentSheet ObjPostPaymentSheet, string ChargesXML, int BranchId, int Uid, string Module,string ExportUnder)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.UptoDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalTaxable });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpoterUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBondInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
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
        public void AddEditInvoice(BondPostPaymentSheet ObjPostPaymentSheet, string ChargesXML, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.UptoDate) });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalTaxable });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_ExpoterUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBondInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
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
        #endregion

        #region Delivery Order

        public void GetWOForDeliveryOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondWOForDeliveryOrder", CommandType.StoredProcedure, DParam);
            List<ListOfWorkOrderNo> LstWONo = new List<ListOfWorkOrderNo>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWONo.Add(new ListOfWorkOrderNo
                    {
                        SacNo = Convert.ToString(Result["SacNo"]),
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWONo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetWODetForDeliveryOrder(int SpaceAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondWOForDeliveryOrder", CommandType.StoredProcedure, DParam);
            WorkOrderDetails ObjWorkOrder = new WorkOrderDetails();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                   // ObjWorkOrder.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                  //  ObjWorkOrder.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                  //  ObjWorkOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjWorkOrder.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    ObjWorkOrder.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                  //  ObjWorkOrder.WRNo = Result["WRNo"].ToString();
                  //  ObjWorkOrder.WRDate = Result["WRDate"].ToString();
                 //   ObjWorkOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                  //  ObjWorkOrder.SacNo = Result["SacNo"].ToString();
                    ObjWorkOrder.SacDate = Result["SacDate"].ToString();
                    ObjWorkOrder.Importer = Result["Importer"].ToString();
                    ObjWorkOrder.CHAId = Convert.ToInt32(Result["CHAId"]);
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjWorkOrder.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                //        ObjWorkOrder.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]==DBNull.Value?0:Result["ClosingWeight"]);
                //    }
               // }
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
        public void GetAllDeliveryOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            List<ListOfDeliveryOrder> LstBond = new List<ListOfDeliveryOrder>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBond.Add(new ListOfDeliveryOrder
                    {
                        DeliveryOrderId = Convert.ToInt32(Result["DeliveryOrderId"]),
                        DeliveryOrderNo = Convert.ToString(Result["DeliveryOrderNo"]),
                        DeliveryOrderDate = Convert.ToString(Result["DeliveryOrderDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetDeliveryOrder(int DeliveryOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryOrderId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            DeliveryOrder ObjDeliveryOrder = new DeliveryOrder();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDeliveryOrder.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    ObjDeliveryOrder.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    ObjDeliveryOrder.DeliveryOrderId = Convert.ToInt32(Result["DeliveryOrderId"]);
                    ObjDeliveryOrder.DeliveryOrderNo = Result["DeliveryOrderNo"].ToString();
                    ObjDeliveryOrder.DeliveryOrderDate = Result["DeliveryOrderDate"].ToString();
                  //  ObjDeliveryOrder.WorkOrderNo = Result["WorkOrderNo"].ToString();
                  //  ObjDeliveryOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjDeliveryOrder.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    ObjDeliveryOrder.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    //ObjDeliveryOrder.WRNo = Result["WRNo"].ToString();
                    //ObjDeliveryOrder.WRDate = Result["WRDate"].ToString();
                    //ObjDeliveryOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDeliveryOrder.SacNo = Result["SacNo"].ToString();
                    ObjDeliveryOrder.SacDate = Result["SacDate"].ToString();
                    //ObjDeliveryOrder.Units = Convert.ToInt32(Result["Units"]);
                    //ObjDeliveryOrder.Weight = Convert.ToDecimal(Result["Weight"]);
                    //ObjDeliveryOrder.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                    //ObjDeliveryOrder.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]);
                    //ObjDeliveryOrder.Value = Convert.ToDecimal(Result["Value"]);
                    //ObjDeliveryOrder.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDeliveryOrder.Remarks = Result["Remarks"].ToString();
                    ObjDeliveryOrder.Importer = Result["Importer"].ToString();
                    ObjDeliveryOrder.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryOrder.CargoDescription = (Result["CargoDescription"]==null?"":Result["CargoDescription"]).ToString();
                }
                if(Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstDeliveryOrder.Add(new DeliveryOrderDtl
                        {
                            WRNo = Result["WRNo"].ToString(),
                            WRDate = Result["WRDate"].ToString(),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]),
                            ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            DeliveryOrderDtlId = Convert.ToInt32(Result["DeliveryOrderDtlId"]),
                            DepositAppId= Convert.ToInt32(Result["DepositAppId"]),
                            DepositNo = Result["DepositNo"].ToString(),
                            DepositDate = Result["DepositDate"].ToString(),
                            Size = Result["Size"].ToString(),
                        });
                    }
                }
               
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditDeliveryOrder(DeliveryOrder ObjDeliveryOrder,string DeliveryOrderXml)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.DeliveryOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryOrderXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.SpaceAppId });
         //   LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.GodownId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDeliveryOrder.WRDate).ToString("yyyy-MM-dd") });
         //   LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderDate", MySqlDbType = MySqlDbType.Date, Value =Convert.ToDateTime(ObjDeliveryOrder.DeliveryOrderDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Units", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.Units });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Weight });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_ClosingUnits", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.ClosingUnits });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_ClosingWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.ClosingWeight });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Value });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Duty });
          //  LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Delivery Order Details Saved Successfully" : "Delivery Order Details Updated Successfully");
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
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
        public void GetDepositNoForDelvOrder(int SpaceAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDepositDetForBondDelvOrder", CommandType.StoredProcedure, DParam);
            List<DeliveryOrderDtl> LstDepositNo= new List<DeliveryOrderDtl>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositNo.Add(new DeliveryOrderDtl
                    {
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"]),
                        DepositNo = Convert.ToString(Result["DepositNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetDepositDetForDelvOrder(int DepositAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDepositDetForBondDelvOrder", CommandType.StoredProcedure, DParam);
            DeliveryOrderDtl ObjDepositNo = new DeliveryOrderDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                        ObjDepositNo.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                        ObjDepositNo.GodownId = Convert.ToInt32(Result["GodownId"]);
                        ObjDepositNo.WRNo = Convert.ToString(Result["WRNo"]);
                        ObjDepositNo.WRDate = Convert.ToString(Result["WRDate"]);
                        ObjDepositNo.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]);
                        ObjDepositNo.Duty = Convert.ToDecimal(Result["Duty"]);
                        ObjDepositNo.Value = Convert.ToDecimal(Result["Value"]);
                        ObjDepositNo.DepositDate = Convert.ToString(Result["DepositDate"]);
                        ObjDepositNo.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Bond Delivery Payment

        public void GetSACNoForDelBondPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (List<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACForBondDelivery");
                
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        #endregion

        #region Bond Unloading Payment

        public void GetSACNoForUnlodBondPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACForBondUnloading");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }


        public void GetContainersByDepositAppId(int DepositAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            //var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACForBondUnloading");
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainersByDepositAppId", CommandType.StoredProcedure, DParam);

            try
            {
              
                List<dynamic> lst = new List<dynamic>();
                while (Result.Read())
                {
                    lst.Add(new { CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        ArrivalDate= Result["ArrivalDate"].ToString(),
                        Reefer =Convert.ToInt32(Result["Reefer"]),
                    });
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lst;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region BOND EDIT
        public void GetSACNoForAdvBondPaymentSheetById(int SpaceappId)
        {
           
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSACNoForBondAdvPaymentSheetById", CommandType.StoredProcedure, DParam);

            try
            {
                //var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACNoForBondAdvPaymentSheet");
                BondSacDetails SacObj = new BondSacDetails();
                while (Result.Read())
                {
                    SacObj.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    SacObj.DepositNo = Result["DepositNo"].ToString();
                    SacObj.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    SacObj.Address = Result["Address"].ToString();
                    SacObj.StateCode = Result["StateCode"].ToString();
                    SacObj.GSTNo = Result["GSTNo"].ToString();
                    SacObj.State = Result["State"].ToString();
                    SacObj.SacDate = Result["SacDate"].ToString();
                    SacObj.ValidUpto = Result["ValidUpto"].ToString();
                    SacObj.ShippingLineName= Result["ShippingLineName"].ToString();
                    SacObj.CHAName= Result["CHAName"].ToString();
                    SacObj.AreaReserved= Convert.ToDecimal(Result["AreaReserved"]);
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = SacObj;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSACForBondUnloadingById(int SpaceappId)
        {

            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSACForBondUnloadingById", CommandType.StoredProcedure, DParam);
            //var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACNoForBondAdvPaymentSheet");
            BondSacDetails SacObj = new BondSacDetails();

            try
            {
                while (Result.Read())
                {
                    SacObj.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    SacObj.DepositNo = Result["DepositNo"].ToString();
                    SacObj.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    SacObj.ImporterName = Convert.ToString(Result["ImporterName"]);
                    SacObj.Address = Result["Address"].ToString();
                    SacObj.StateCode = Result["StateCode"].ToString();
                    SacObj.GSTNo = Result["GSTNo"].ToString();
                    SacObj.State = Result["State"].ToString();
                    SacObj.SacDate = Result["SacDate"].ToString();
                    SacObj.ValidUpto = Result["ValidUpto"].ToString();
                    SacObj.ShippingLineName = Result["ShippingLineName"].ToString();
                    SacObj.CHAName = Result["CHAName"].ToString();
                    SacObj.AreaReserved= Convert.ToDecimal(Result["AreaReserved"]);
                    SacObj.Weight = Convert.ToDecimal(Result["Weight"]);
                    SacObj.CIFValue = Convert.ToDecimal(Result["CIFValue"]);
                    SacObj.Duty = Convert.ToDecimal(Result["Duty"]);
                    SacObj.Units = Convert.ToInt32(Result["Units"]);
                    SacObj.IsInsured = Convert.ToInt32(Result["IsInsured"]);
                    SacObj.DepositDate = Result["DepositDate"].ToString();
                    SacObj.BondBOENo = Result["BondBOENo"].ToString();
                    SacObj.BondBOEDate = Result["BondBOEDate"].ToString();
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = SacObj;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSACForBondDeliveryById(int SpaceappId)
        {

            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSACForBondDeliveryById", CommandType.StoredProcedure, DParam);
            //var objBondSacDetails = (IList<BondSacDetails>)DataAccess.ExecuteDynamicSet<BondSacDetails>("GetSACNoForBondAdvPaymentSheet");
            BondSacDetails SacObj = new BondSacDetails();
            try
            {
               
                while (Result.Read())
                {
                    SacObj.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    SacObj.DepositNo = Result["DepositNo"].ToString();
                    SacObj.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    SacObj.ImporterName = Convert.ToString(Result["ImporterName"]);
                    SacObj.Address = Result["Address"].ToString();
                    SacObj.StateCode = Result["StateCode"].ToString();
                    SacObj.GSTNo = Result["GSTNo"].ToString();
                    SacObj.State = Result["State"].ToString();
                    SacObj.SacDate = Result["SacDate"].ToString();
                    SacObj.ValidUpto = Result["ValidUpto"].ToString();
                    SacObj.ShippingLineName = Result["ShippingLineName"].ToString();
                    SacObj.CHAName = Result["CHAName"].ToString();
                    SacObj.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    SacObj.Weight = Convert.ToDecimal(Result["Weight"]);
                    SacObj.CIFValue = Convert.ToDecimal(Result["CIFValue"]);
                    SacObj.Duty = Convert.ToDecimal(Result["Duty"]);
                    SacObj.Units = Convert.ToInt32(Result["Units"]);
                    SacObj.IsInsured = Convert.ToInt32(Result["IsInsured"]);
                    SacObj.DepositDate = Result["DepositDate"].ToString();
                    SacObj.BondBOENo = Result["BondBOENo"].ToString();
                    SacObj.BondBOEDate = Result["BondBOEDate"].ToString();
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = SacObj;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
    }
}