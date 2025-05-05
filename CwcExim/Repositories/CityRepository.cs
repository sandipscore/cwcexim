using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CwcExim.Repositories
{
    public class CityRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;           
            }
        }

        public void AddEditCity(City ObjCity)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCity.CityId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCity.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCity.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityName", MySqlDbType = MySqlDbType.VarChar,  Value = ObjCity.CityName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityAlias", MySqlDbType = MySqlDbType.VarChar,  Value = ObjCity.CityAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjCity.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstCity",CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjCity.CityId == 0 ? "City Details Saved Successfully" : "City Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "City Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "City Alias Already Exists";
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
        public void DeleteCity(int CityId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CityId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstCity", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "City Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                //else if (Result == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Cannot Delete City Details As It Exist In Port Master";
                //    _DBResponse.Data = null;
                //}
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete City Details As It Exist In Contractor Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Delete City Details As It Exist In Exim Trader Master";
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
        public void GetAllCity()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<City> LstCity = new List<City>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCity.Add(new City
                    {
                        CityId = Convert.ToInt32(Result["CityId"]),
                        CountryId = Convert.ToInt32(Result["CountryId"]),
                        StateId = Convert.ToInt32(Result["StateId"]),
                        CityName = Result["CityName"].ToString(),
                        CityAlias = (Result["CityAlias"] == null ? "" : Result["CityAlias"]).ToString(),
                        CountryName = (Result["CountryName"] == null ? "" : Result["CountryName"]).ToString(),
                        StateName = (Result["StateName"] == null ? "" : Result["StateName"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCity;
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
        public void GetCity(int CityId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = CityId, Size = 11 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            City ObjCity = new City();
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    ObjCity.CityId = Convert.ToInt32(Result["CityId"]);
                    ObjCity.CityName = Result["CityName"].ToString();
                    ObjCity.CityAlias = Result["CityAlias"].ToString();
                    ObjCity.StateId = Convert.ToInt32(Result["StateId"]);
                    ObjCity.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjCity.CountryName = Result["CountryName"].ToString();
                    ObjCity.StateName = Result["StateName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCity;
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
       public void GetCityByStateId(int StateId)
       {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32,Size=11, Value = StateId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCityByStateId",CommandType.StoredProcedure,DParam);
            List<City> LstCity = new List<City>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCity.Add(new City
                    {
                       CityId= Convert.ToInt32(Result["CityId"]),
                       CityName=Result["CityName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCity;
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