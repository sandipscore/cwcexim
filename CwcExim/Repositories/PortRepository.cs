using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;

namespace CwcExim.Repositories
{
    public class PortRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddEditPort(Port ObjPort)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32,Size=11, Value = ObjPort.PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPort.PortName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjPort.PortAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstPort", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjPort.PortId==0?"Port Details Saved Successfully": "Port Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message =  "Port Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Port Alias Already Exists";
                    _DBResponse.Data = null;
                }
                else 
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
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
        public void DeletePort(int PortId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstPort", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Port Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch(Exception ex)
            {
                DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void GetAllPort()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure,DParam);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName=Result["PortName"].ToString(),
                        PortAlias=Result["PortAlias"].ToString(),
                        PortId=Convert.ToInt32(Result["PortId"]),
                        CountryId=Convert.ToInt32(Result["CountryId"]),
                        StateId=Convert.ToInt32(Result["StateId"]),
                        POD=Convert.ToBoolean(Result["POD"]),
                        CountryName=Result["CountryName"].ToString(),
                        StateName=Result["StateName"].ToString()
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
        public void GetPort(int PortId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PortId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            Port ObjPort = new Port();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPort.PortId = Convert.ToInt32(Result["PortId"]);
                    ObjPort.PortName = Result["PortName"].ToString();
                    ObjPort.PortAlias = Result["PortAlias"].ToString();
                    ObjPort.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjPort.StateId = Convert.ToInt32(Result["StateId"]);
                    ObjPort.POD = Convert.ToBoolean(Result["POD"]);
                    ObjPort.CountryName = Result["CountryName"].ToString();
                    ObjPort.StateName = Result["StateName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPort;
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
    }
}