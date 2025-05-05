using CwcExim.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using CwcExim.Areas.GateOperation.Models;
using CwcExim.Models;
using System.Data;

namespace CwcExim.Repositories
{
    public class WeighmentRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddWeighment(Weighment ObjWeighment)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_WeightmentId", MySqlDbType = MySqlDbType.Int32, Value = ObjWeighment.WeightmentId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjWeighment.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleMasterId", MySqlDbType = MySqlDbType.Int32, Value = ObjWeighment.VehicleMasterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WeightmentDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjWeighment.WeightmentDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = ObjWeighment.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmptyWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjWeighment.EmptyWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjWeighment.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddWeightment", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Weightment Details Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Weightment Details Already Exist";
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
        public void GetWeighment()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetWeighment",CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            List<Weighment> LstWeighment = new List<Weighment>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWeighment.Add(new Weighment
                    {
                        WeightmentId=Convert.ToInt32(Result["WeightmentId"]),
                       // CFSCode=Result["CFSCode"].ToString(),
                       // VehicleMasterId=Convert.ToInt32(Result["VehicleMasterId"]),
                        WeightmentDate=Convert.ToString(Result["WeightmentDate"]),
                        Weight=Convert.ToDecimal(Result["Weight"]),
                        EmptyWeight=Convert.ToDecimal(Result["EmptyWeight"]),
                        ContainerNo=Result["ContainerNo"].ToString(),
                        VehicleNumber=Result["VehicleNumber"].ToString(),
                        OperationType =Result["OperationType"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWeighment;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerNoForWeighment()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForWeighment", CommandType.StoredProcedure,DParam);
            List<Weighment> LstWeighment = new List<Weighment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstWeighment.Add(new Weighment
                    {
                       ContainerNo= Result["ContainerNo"].ToString(),
                       CFSCode=Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWeighment;
                }
                else 
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
    }
}