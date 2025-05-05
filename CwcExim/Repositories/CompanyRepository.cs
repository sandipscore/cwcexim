using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace CwcExim.Repositories
{
    public class CompanyRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddCompany(Company ObjCompany)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName= "in_CompanyId", MySqlDbType=MySqlDbType.Int32,Value= ObjCompany.CompanyId }); ;
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompanyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.CompanyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompanyShortName", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.CompanyShortName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompanyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.CompanyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PhoneNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.PhoneNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FaxNumber", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.FaxNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmailAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.EmailAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = ObjCompany.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = ObjCompany.CityId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GstIn", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.GstIn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pan", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.Pan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.StateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjCompany.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSFormat", MySqlDbType = MySqlDbType.VarChar, Value = ObjCompany.CFSFormat });
            LstParam.Add(new MySqlParameter { ParameterName= "in_BranchId", MySqlDbType=MySqlDbType.Int32,Value=Convert.ToInt32(HttpContext.Current.Session["BranchId"])});
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMstCompany", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Company Creation Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Company Name Already Exist";
                    _DBResponse.Data = null;
                }
                else if(Result==3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Company Short Name Already Exist";
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
        public void GetAllCompany()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllCompany", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Company> LstCompany = new List<Company>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCompany.Add(new Company
                    {
                        CompanyName=Result["CompanyName"].ToString(),
                        CompanyAddress=(Result["CompanyAddress"]==null?"":Result["CompanyAddress"]).ToString(),
                        PhoneNo=(Result["PhoneNo"]==null?"":Result["PhoneNo"]).ToString(),
                        FaxNumber=(Result["FaxNumber"]==null?"":Result["FaxNumber"]).ToString(),
                        EmailAddress=(Result["EmailAddress"]==null?"":Result["EmailAddress"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCompany;
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