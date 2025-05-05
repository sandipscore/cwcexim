using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CwcExim.UtilityClasses;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;

namespace CwcExim.Repositories
{
    public class RoleMasterRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditRoles(RoleMaster objRoleMaster)
        {
            string Status = "0";
            int RetValue = 0;
            int GeneratedClientId = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = objRoleMaster.RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleName", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objRoleMaster.RoleName });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = objRoleMaster.CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = objRoleMaster.UpdatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4,Direction=ParameterDirection.Output, Value = RetValue });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            DParam = LstParam.ToArray();
           int  Result = DataAccess.ExecuteNonQuery("AddEditRoleMaster", CommandType.StoredProcedure, DParam,out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else if(Result == 0)
                {
                    DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Role Name already exists.";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
       }

        public void GetAllRoles()
        {
            List<RoleMaster> ObjRoleLst = null;
            RoleMaster ObjRoleMaster = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleID", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = 0 });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllRoles", CommandType.StoredProcedure, DParam);

            ObjRoleLst = new List<RoleMaster>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRoleMaster = new RoleMaster();
                    ObjRoleMaster.RoleId = Convert.ToInt32(Result["RoleId"]);
                    ObjRoleMaster.RoleName = Convert.ToString(Result["RoleName"]);
                    ObjRoleMaster.CreatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.CreatedBy = Convert.ToInt32(Result["CreatedBy"]);
                    ObjRoleMaster.CreatedOn =  Convert.ToString(Result["CreatedOn"]==null?"": Result["CreatedOn"]);
                    ObjRoleMaster.UpdatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"]);
                    ObjRoleMaster.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjRoleLst.Add(ObjRoleMaster);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjRoleMaster.RoleId == 0 ? "Role saved successfully" : "Role updated successfully";
                    _DBResponse.Data = ObjRoleLst;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
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

        public void GetAllRoles(int RoleID)
        {
            List<RoleMaster> ObjRoleLst = null;
            RoleMaster ObjRoleMaster = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleID", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = RoleID });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllRoles", CommandType.StoredProcedure, DParam);

            ObjRoleLst = new List<RoleMaster>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRoleMaster = new RoleMaster();
                    ObjRoleMaster.RoleId = Convert.ToInt32(Result["RoleId"]);
                    ObjRoleMaster.RoleName = Convert.ToString(Result["RoleName"]);
                    ObjRoleMaster.CreatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.CreatedBy = Convert.ToInt32(Result["CreatedBy"]);
                    ObjRoleMaster.CreatedOn = Convert.ToString(Result["CreatedOn"]);
                    ObjRoleMaster.UpdatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"]);
                    ObjRoleMaster.UpdatedOn = Convert.ToString(Result["UpdatedOn"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjRoleMaster;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
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

        // GET Roles FOr Assignments
        public void GetAllRolesForAssignments(int RoleID)
        {
            List<RoleMaster> ObjRoleLst = null;
            RoleMaster ObjRoleMaster = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_RoleID", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = RoleID });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllRoles", CommandType.StoredProcedure, DParam);

            ObjRoleLst = new List<RoleMaster>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRoleMaster = new RoleMaster();
                    ObjRoleMaster.RoleId = Convert.ToInt32(Result["RoleId"]);
                    ObjRoleMaster.RoleName = Convert.ToString(Result["RoleName"]);
                    ObjRoleMaster.CreatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.CreatedBy = Convert.ToInt32(Result["CreatedBy"]);
                    ObjRoleMaster.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjRoleMaster.UpdatedByName = Convert.ToString(Result["strCreatedBy"]);
                    ObjRoleMaster.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"]);
                    ObjRoleMaster.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjRoleLst.Add(ObjRoleMaster);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjRoleMaster.RoleId == 0 ? "Role saved successfully" : "Role updated successfully";
                    _DBResponse.Data = ObjRoleLst;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
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
    }
}