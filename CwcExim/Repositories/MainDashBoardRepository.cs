using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class MainDashBoardRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GetCompanyInfo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCompanyInfo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Company Company = new Company();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    Company.CompanyName = Result["CompanyName"].ToString();
                    Company.ROAddress = (Result["ROAddress"] == null ? "" : Result["ROAddress"]).ToString();

                    Company.CompanyAddress = (Result["CompanyAddress"] == null ? "" : Result["CompanyAddress"]).ToString();
                    Company.PhoneNo = (Result["PhoneNo"] == null ? "" : Result["PhoneNo"]).ToString();
                    Company.FaxNumber = (Result["FaxNumber"] == null ? "" : Result["FaxNumber"]).ToString();
                    Company.EmailAddress = (Result["EmailAddress"] == null ? "" : Result["EmailAddress"]).ToString();
                    Company.LocationUrl = (Result["LocationUrl"] == null ? "" : Result["LocationUrl"]).ToString();
                    Company.ContactAddress = (Result["ContactAddress"] == null ? "" : Result["ContactAddress"]).ToString();
                    Company.ContactPhone = (Result["ContactPhone"] == null ? "" : Result["ContactPhone"]).ToString();
                    Company.BranchType = (Result["BranchType"] == null ? "" : Result["BranchType"]).ToString();
                    Company.BranchName = (Result["BranchName"] == null ? "" : Result["BranchName"]).ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Company;
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
                log.Error(ex.StackTrace);
            }
            finally
            {
                Result.Dispose();
                Result.Close();

            }
        }

    }
}