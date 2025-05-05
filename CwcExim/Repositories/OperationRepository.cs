using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using CwcExim.DAL;
using System.Data;
namespace CwcExim.Repositories
{
    public class OperationRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddMstOperation(Operation objOper)
        {
            string id = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objOper.Type });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationCode", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objOper.Code });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32,  Value = objOper.SacId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationSDesc", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objOper.ShortDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationDesc", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objOper.Description });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objOper.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dpram = lstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddMstOperation", CommandType.StoredProcedure, dpram, out id);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = id;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Operation Details Saved Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Operation Code Already Exists";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Combination Of Operation Type And SAC Code Already Exists";
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
        public void GetAllMstOperation()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ViewMstOperation", CommandType.StoredProcedure, dparam);
            IList<Operation> lstOP = new List<Operation>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstOP.Add(new Operation
                    {
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        Type = Convert.ToInt32(result["OperationType"]),
                        Code = (result["OperationCode"] == null ? "" : result["OperationCode"]).ToString(),
                        SacCode=(result["SacCode"]==null?"": result["SacCode"]).ToString(),
                        ShortDescription = (result["OperationSDesc"] == null ? "" : result["OperationSDesc"]).ToString(),
                        Description = (result["OperationDesc"] == null ? "" : result["OperationDesc"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstOP;
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
        public void ViewMstOperation(int OperationId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = OperationId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ViewMstOperation", CommandType.StoredProcedure, dparam);
            Operation objOP = new Operation();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objOP.OperationId = Convert.ToInt32(result["OperationId"]);
                    objOP.Type = Convert.ToInt32(result["OperationType"]);
                    objOP.Code = (result["OperationCode"] == null ? "" : result["OperationCode"]).ToString();
                    objOP.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();
                    objOP.ShortDescription = (result["OperationSDesc"] == null ? "" : result["OperationSDesc"]).ToString();
                    objOP.Description = (result["OperationDesc"] == null ? "" : result["OperationDesc"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOP;
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
    }
}