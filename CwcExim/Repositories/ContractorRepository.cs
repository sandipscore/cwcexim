using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class ContractorRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditContractor(Contractor ObjContractor)
        {
            string GeneratedClientId = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContractor.ContractorId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorName", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.ContractorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.ContractorAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContractor.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = ObjContractor.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = ObjContractor.CityId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PinCode", MySqlDbType = MySqlDbType.Int32, Value = ObjContractor.PinCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PhoneNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.PhoneNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FaxNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.FaxNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.Email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContactPerson", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.ContactPerson });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.MobileNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pan", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.Pan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AadhaarNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.AadhaarNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjContractor.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjContractor.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstContractor", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjContractor.ContractorId == 0 ? "Contractor Details Saved Successfully" : "Contractor Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Contractor Name Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Contractor Alias Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Duplicate Email";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Duplicate PAN";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Duplicate Aadhaar No";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Duplicate GST No";
                    _DBResponse.Data = null;
                }
                else if (Result ==8)
                {
                    _DBResponse.Status = 8;
                    _DBResponse.Message = "Duplicate Phone No";
                    _DBResponse.Data = null;
                }
                else if (Result == 9)
                {
                    _DBResponse.Status = 9;
                    _DBResponse.Message = "Duplicate Mobile No";
                    _DBResponse.Data = null;
                }
                else if (Result == 10)
                {
                    _DBResponse.Status = 10;
                    _DBResponse.Message = "Duplicate Fax No";
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

        public void DeleteContractor(int ContractorId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContractorId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstContractor", CommandType.StoredProcedure,DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Contractor Details Deleted Successfully";
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

        public void GetAllContractor()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstContractor", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Contractor> LstContractor = new List<Contractor>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContractor.Add(new Contractor
                    {
                        ContractorId = Convert.ToInt32(Result["ContractorId"]),
                        ContractorName = Result["ContractorName"].ToString(),
                        // ContractorAlias=(Result["ContractorAlias"]==null?"":Result["ContractorAlias"]).ToString(),
                        // Address=Result["Address"].ToString(),
                        // CountryId=Convert.ToInt32(Result["CountryId"]),
                        //StateId = Convert.ToInt32(Result["StateId"]),
                        // CityId=Convert.ToInt32(Result["CityId"]),
                        // PinCode= Convert.ToInt32(Result["PinCode"]),
                        // PhoneNo=Result["PhoneNo"].ToString(),
                        // FaxNo=Result["FaxNo"].ToString(),
                        Email = (Result["Email"]==null?"":Result["Email"]).ToString(),
                        ContactPerson = (Result["ContactPerson"]==null?"":Result["ContactPerson"]).ToString(),
                        // MobileNo=Result["MobileNo"].ToString(),
                        // Pan=Result["Pan"].ToString(),
                        // AadhaarNo=Result["AadhaarNo"].ToString(),
                        // GSTNo=Result["GSTNo"].ToString(),
                        // CountryName=Result["CountryName"].ToString(),
                        //StateName=Result["StateName"].ToString(),
                        // CityName=Result["CityName"].ToString()
                    });
                 }
                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = LstContractor;
                    }
                    else
                    {
                        _DBResponse.Status = 0;
                        _DBResponse.Message = "No Data";
                        _DBResponse.Data = null;
                    }
            }
            catch(Exception ex )
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

        public void GetContractor(int ContractorId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContractorId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContractorId });
            IDataParameter [] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstContractor", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            Contractor ObjContractor = new Contractor();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjContractor.ContractorId = Convert.ToInt32(Result["ContractorId"]);
                    ObjContractor.ContractorName = Result["ContractorName"].ToString();
                    ObjContractor.ContractorAlias = (Result["ContractorAlias"] == null ? "" : Result["ContractorAlias"]).ToString();
                    ObjContractor.Address = (Result["Address"]==null?"":Result["Address"]).ToString();
                    ObjContractor.CountryId = Convert.ToInt32(Result["CountryId"]==DBNull.Value?0:Result["CountryId"]);
                    ObjContractor.StateId = Convert.ToInt32(Result["StateId"]==DBNull.Value?0:Result["StateId"]);
                    ObjContractor.CityId = Convert.ToInt32(Result["CityId"]==DBNull.Value?0:Result["CityId"]);
                    ObjContractor.PinCode = Convert.ToInt32(Result["PinCode"]==DBNull.Value?0:Result["PinCode"]);
                    ObjContractor.PhoneNo = (Result["PhoneNo"]==null?"":Result["PhoneNo"]).ToString();
                    ObjContractor.FaxNo = (Result["FaxNo"]==null?"":Result["FaxNo"]).ToString();
                    ObjContractor.Email = (Result["Email"]==null?"":Result["Email"]).ToString();
                    ObjContractor.ContactPerson = (Result["ContactPerson"]==null?"":Result["ContactPerson"]).ToString();
                    ObjContractor.MobileNo = (Result["MobileNo"] == null?"":Result["MobileNo"]).ToString();
                    ObjContractor.Pan = (Result["Pan"]==null?"":Result["Pan"]).ToString();
                    ObjContractor.AadhaarNo = (Result["AadhaarNo"]==null?"":Result["AadhaarNo"]).ToString();
                    ObjContractor.GSTNo = (Result["GSTNo"]==null?"":Result["GSTNo"]).ToString();
                    ObjContractor.CountryName = (Result["CountryName"]==null?"":Result["CountryName"]).ToString();
                    ObjContractor.StateName = (Result["StateName"]==null?"":Result["StateName"]).ToString();
                    ObjContractor.CityName = (Result["CityName"]==null?"":Result["CityName"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Succes";
                    _DBResponse.Data = ObjContractor;
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
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }


    }
}