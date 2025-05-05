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
    public class BankRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GetAllBank()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32,Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBank", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            List<Bank> LstBank = new List<Bank>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBank.Add(new Bank
                    {
                        AccountNo = (Result["AccountNo"] == null ? "" : Result["AccountNo"]).ToString(),
                        BankId = Convert.ToInt32(Result["BankId"]),
                        BankName = (Result["BankName"] == null ? "" : Result["BankName"]).ToString(),
                        Email = Result["Email"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBank;
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
        public void GetBank(int BankId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BankId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBank", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Bank ObjBank = new Bank();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBank.BankId = Convert.ToInt32(Result["BankId"]);
                    ObjBank.BankName = (Result["BankName"] == null ? "" : Result["BankName"]).ToString();
                    ObjBank.AccountNo = (Result["AccountNo"] == null ? "" : Result["AccountNo"]).ToString();
                    ObjBank.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjBank.IFSC = (Result["IFSC"] == null ? "" : Result["IFSC"]).ToString();
                    ObjBank.Branch = (Result["Branch"] == null ? "" : Result["Branch"]).ToString();
                    ObjBank.MobileNo = (Result["MobileNo"] == null ? "" : Result["MobileNo"]).ToString();
                    ObjBank.FaxNo = (Result["FaxNo"] == null ? "" : Result["FaxNo"]).ToString();
                    ObjBank.Email = (Result["Email"]==null? "" : Result["Email"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBank;
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
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddBank(Bank ObjBank)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjBank.BankId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankName", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.BankName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AccountNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.AccountNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IFSC", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.IFSC });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Branch });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.MobileNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FaxNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.FaxNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjBank.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = GeneratedClientId });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMstBank", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Bank And Branch Combination Already Exist";
                    _DBResponse.Data = null;
                }
                //else if (Result == 4)
                //{
                //    _DBResponse.Status = 4;
                //    _DBResponse.Message = "Duplicate Fax No.";
                //    _DBResponse.Data = null;
                //}
                //else if (Result == 5)
                //{
                //    _DBResponse.Status = 5;
                //    _DBResponse.Message = "Duplicate Email Id";
                //    _DBResponse.Data = null;
                //}
                //else if (Result == 6)
                //{
                //    _DBResponse.Status = 6;
                //    _DBResponse.Message = "Duplicate Account No.";
                //    _DBResponse.Data = null;
                //}
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
    }
}