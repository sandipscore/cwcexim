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
    public class DesignationRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditDesignation(DesignationMaster ObjDesignation)
        {
            string Status = "0";
            int RetValue = 0;
            int GeneratedClientId = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_DesignationId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjDesignation.DesignationId });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_Designation", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjDesignation.Designation });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_HigherAuthority", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjDesignation.HigherAuthority });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_ApprovalLevel", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjDesignation.ApprovalLevel });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjDesignation.CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = ObjDesignation.UpdatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = RetValue });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditDesignation", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Designation Created Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 0)
                {
                    DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Designation Name already exists.";
                    _DBResponse.Data = null;
                }
            }
            catch(Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllDesignation()
        {
            List<DesignationMaster> ObjDesignationLst = null;
            DesignationMaster ObjDesignation = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_DesignationId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = 0 });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDesignation", CommandType.StoredProcedure, DParam);

            ObjDesignationLst = new List<DesignationMaster>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDesignation = new DesignationMaster();
                    ObjDesignation.GeneratedSerialNo = Convert.ToInt32(Result["GeneratedSerialNo"]);
                    ObjDesignation.DesignationId = Convert.ToInt32(Result["DesignationId"]);
                    ObjDesignation.Designation = Convert.ToString(Result["Designation"]);
                    ObjDesignation.HigherAuthority= Convert.ToInt32(Result["HigherAuthority"] == DBNull.Value ? 0 : Result["HigherAuthority"]);
                    ObjDesignation.HigherAuthorityName= Convert.ToString(Result["HigherAuthorityName"]);
                    ObjDesignation.ApprovalLevel = Convert.ToInt32(Result["ApprovalLevel"] == DBNull.Value ? 0 : Result["ApprovalLevel"]);
                    ObjDesignation.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjDesignation.CreatedBy = Convert.ToInt32(Result["CreatedBy"]== DBNull.Value ? 0: Result["CreatedBy"]);
                    ObjDesignation.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjDesignation.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjDesignation.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjDesignation.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjDesignationLst.Add(ObjDesignation);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDesignationLst;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch(Exception ex)
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

        public void GetDesignation(int DesignationId)
        {
            DesignationMaster ObjDesignation = null;
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@in_DesignationId", MySqlDbType = MySqlDbType.Int32, Size = 4, Value = DesignationId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDesignation", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDesignation = new DesignationMaster();
                    ObjDesignation.DesignationId = Convert.ToInt32(Result["DesignationId"]);
                    ObjDesignation.Designation = Convert.ToString(Result["Designation"]);
                    ObjDesignation.HigherAuthority = Convert.ToInt32(Result["HigherAuthority"] == DBNull.Value ? 0 : Result["HigherAuthority"]);
                    ObjDesignation.HigherAuthorityName = Convert.ToString(Result["HigherAuthorityName"]);
                    ObjDesignation.ApprovalLevel = Convert.ToInt32(Result["ApprovalLevel"] == DBNull.Value ? 0 : Result["ApprovalLevel"]);
                    ObjDesignation.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjDesignation.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjDesignation.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjDesignation.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjDesignation.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjDesignation.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDesignation.DesignationId == 0 ? "Designation saved successfully" : "Designation updated successfully";
                    _DBResponse.Data = ObjDesignation;
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

    }
}