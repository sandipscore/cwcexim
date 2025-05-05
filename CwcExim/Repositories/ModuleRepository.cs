using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class ModuleRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditModule(Module ObjModule)
        {
            string Status = "0";
            int RetValue = 0;
            int GeneratedClientId = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ModuleId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.ModuleId });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ModuleName", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjModule.ModuleName });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ModulePrefix", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjModule.ModulePrefix });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_ModuleFees", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.ModuleFees });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_FinePerct", MySqlDbType = MySqlDbType.Decimal, Value = ObjModule.FinePerct });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_RebatePerct", MySqlDbType = MySqlDbType.Decimal, Value = ObjModule.RebatePerct });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_ReviewFees", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.ReviewFees });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_RevisionFees", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.RevisionFees });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_AppealFees", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.AppealFees });
            //LstParam.Add(new MySqlParameter { ParameterName = "@in_HighestApprovalAuthority", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.HighestApprovalAuthority });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjModule.UpdatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = RetValue });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditModule", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjModule.ModuleId == 0 ? "Module Details Saved Successfully" : "Module Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Module Name Already Exists.";
                    _DBResponse.Data = null;
                }
                else if(Result==3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Module Prefix Already Exists.";
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

        public void GetAllModule()
        {
            List<Module> lstModule = null;
            Module ObjModule = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ModuleId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = 0 });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetModule", CommandType.StoredProcedure, DParam);

            lstModule = new List<Module>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjModule = new Module();
                    ObjModule.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjModule.ModuleName = Convert.ToString(Result["ModuleName"]);
                    ObjModule.ModulePrefix= Convert.ToString(Result["ModulePrefix"]);
                    //ObjModule.ModuleFees = Convert.ToInt32(Result["ModuleFees"] == DBNull.Value ? 0 : Result["ModuleFees"]);
                    //ObjModule.FinePerct = Convert.ToDecimal(Result["FinePerct"]);
                    //ObjModule.RebatePerct = Convert.ToDecimal(Result["RebatePerct"]);
                    //ObjModule.ReviewFees = Convert.ToInt32(Result["ReviewFees"]);
                    //ObjModule.RevisionFees = Convert.ToInt32(Result["RevisionFees"]);
                    //ObjModule.AppealFees = Convert.ToInt32(Result["AppealFees"]);
                    //ObjModule.HighestApprovalAuthority = Convert.ToInt32(Result["HighestApprovalAuthority"] == DBNull.Value ? 0 : Result["HighestApprovalAuthority"]);
                    //ObjModule.HighestAppAuthName = Convert.ToString(Result["HighestAppAuthName"]);
                    //ObjModule.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    //ObjModule.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    //ObjModule.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    //ObjModule.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    //ObjModule.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    //ObjModule.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    lstModule.Add(ObjModule);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstModule;
                }
                else
                {
                    _DBResponse.Status = 2;
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

        public void GetModule(int ModuleId)
        {
            Module ObjModule = null;
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ModuleId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ModuleId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetModule", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjModule = new Module();
                    ObjModule.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjModule.ModuleName = Convert.ToString(Result["ModuleName"]);
                    ObjModule.ModulePrefix = Convert.ToString(Result["ModulePrefix"]);
                    //ObjModule.ModuleFees = Convert.ToInt32(Result["ModuleFees"] == DBNull.Value ? 0 : Result["ModuleFees"]);
                    //ObjModule.FinePerct = Convert.ToDecimal(Result["FinePerct"]);
                    //ObjModule.RebatePerct = Convert.ToDecimal(Result["RebatePerct"]);
                    //ObjModule.ReviewFees = Convert.ToInt32(Result["ReviewFees"]);
                    //ObjModule.RevisionFees = Convert.ToInt32(Result["RevisionFees"]);
                    //ObjModule.AppealFees = Convert.ToInt32(Result["AppealFees"]);
                    //ObjModule.HighestApprovalAuthority = Convert.ToInt32(Result["HighestApprovalAuthority"] == DBNull.Value ? 0 : Result["HighestApprovalAuthority"]);
                    //ObjModule.HighestAppAuthName = Convert.ToString(Result["HighestAppAuthName"]);
                    //ObjModule.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    //ObjModule.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    //ObjModule.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    //ObjModule.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    //ObjModule.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    //ObjModule.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjModule;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 2;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetModuleForAccessRights()
        {
            List<Module> lstModule = null;
            Module ObjModule = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetModuleForAccessRights", CommandType.StoredProcedure, DParam);

            lstModule = new List<Module>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjModule = new Module();
                    ObjModule.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjModule.ModuleName = Convert.ToString(Result["ModuleName"]);
                    lstModule.Add(ObjModule);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstModule;
                }
                else
                {
                    _DBResponse.Status = 2;
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

        public void GetModuleByRole(int RoleID,int PartyType)
        {
            List<Module> lstModule = null;
            Module ObjModule = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleID", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = RoleID });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_PartyType", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = PartyType });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetModuleByRole", CommandType.StoredProcedure, DParam);

            lstModule = new List<Module>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjModule = new Module();
                    ObjModule.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjModule.ModuleName = Convert.ToString(Result["ModuleName"]);
                    ObjModule.IconFile = Convert.ToString(Result["IconFile"]);
                    lstModule.Add(ObjModule);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstModule;
                }
                else
                {
                    _DBResponse.Status = 2;
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

        public void DeleteModule(int ModuleId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Value = ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("DeleteModule", CommandType.StoredProcedure,dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if(Result==1)
                {
                    _DBResponse.Data = Result;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Module Deleted Successfully";
                }
                else if(Result==2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Module Details As It Exists In Menu Master";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }
    }
}