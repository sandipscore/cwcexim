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
    public class CountryRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditCountry(Country ObjCountry)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCountry.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryName", MySqlDbType = MySqlDbType.VarChar, Value = ObjCountry.CountryName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjCountry.CountryAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjCountry.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCountry.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstCountry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjCountry.CountryId == 0 ? "Country Details Saved Successfully" : "Country Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status =2;
                    _DBResponse.Message = "Country Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Coutry Alias Already Exists";
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

        public void DeleteCountry(int CountryId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32,Size=11, Value = CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstCountry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if(Result==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Country Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Country Details Does Not Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In State Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In City Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Port Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Contractor Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status =7;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Exim Trader Master";
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

        public void GetAllCountry()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Country> LstCountry = new List<Country>();
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    LstCountry.Add(new Country {
                        CountryName = Result["CountryName"].ToString(),
                        CountryAlias = (Result["CountryAlias"] == null ? "" : Result["CountryAlias"]).ToString(),
                        CountryId = Convert.ToInt32(Result["CountryId"])
                    });
                }
                if(Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCountry;
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

        public void GetCountry(int CountryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName= "in_CountryId",MySqlDbType=MySqlDbType.Int32,Size=11,Value=CountryId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCountry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Country ObjCountry = new Country();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCountry.CountryName = Result["CountryName"].ToString();
                    ObjCountry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjCountry.CountryAlias = (Result["CountryAlias"]==null?"":Result["CountryAlias"]).ToString();
                }
                if(Status==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCountry;
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