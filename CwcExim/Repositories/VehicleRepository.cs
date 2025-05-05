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
    public class VehicleRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditVehicle(Vehicle ObjVehicle)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleMasterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjVehicle.VehicleMasterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNumber", MySqlDbType = MySqlDbType.VarChar, Value = ObjVehicle.VehicleNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleWeight", MySqlDbType = MySqlDbType.VarChar, Value = ObjVehicle.VehicleWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjVehicle.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstVehicle", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjVehicle.VehicleMasterId == 0 ? "Vehicle Master Saved Successfully" : "Vehicle Master Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status =2;
                    _DBResponse.Message = "Vehicle Number Already Exists";
                    _DBResponse.Data = null;
                }
                //else if (Result == 3)
                //{
                //    _DBResponse.Status = 3;
                //    _DBResponse.Message = "Coutry Alias Already Exists";
                //    _DBResponse.Data = null;
                //}
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

        public void DeleteVehicle(int VehicleMasterId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleMasterId", MySqlDbType = MySqlDbType.Int32,Size=11, Value = VehicleMasterId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstVehicle", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if(Result==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Vehicle Number Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Vehicle Number Does Not Exist";
                    _DBResponse.Data = null;
                }
                //else if (Result == 3)
                //{
                //    _DBResponse.Status = 3;
                //    _DBResponse.Message = "Cannot Delete Country Details As It Exist In State Master";
                //    _DBResponse.Data = null;
                //}
               
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

        public void GetAllVehicleMaster()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstVehicle", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Vehicle> LstVehicle = new List<Vehicle>();
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    LstVehicle.Add(new Vehicle {
                        VehicleNumber = Result["VehicleNumber"].ToString(),
                        VehicleWeight = (Result["VehicleWeight"] == null ? "" : Result["VehicleWeight"]).ToString(),
                        VehicleMasterId = Convert.ToInt32(Result["VehicleMasterId"])
                    });
                }
                if(Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVehicle;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetVehicle(int VehicleId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName= "in_VehicleId", MySqlDbType=MySqlDbType.Int32,Size=11,Value= VehicleId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstVehicle", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Vehicle ObjVehicle = new Vehicle();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjVehicle.VehicleNumber = Result["VehicleNumber"].ToString();
                    ObjVehicle.VehicleWeight = (Result["VehicleWeight"] == null ? "" : Result["VehicleWeight"]).ToString();
                    ObjVehicle.VehicleMasterId = Convert.ToInt32(Result["VehicleMasterId"]);
                }
                if(Status==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjVehicle;
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
    }
}