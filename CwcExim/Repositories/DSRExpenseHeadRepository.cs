using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;

using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using CwcExim.Areas.Master.Models;

namespace CwcExim.Repositories
{
    public class DSRExpenseHeadRepository
    {
        private DatabaseResponse _DBResponse;

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditExpenseHead(DSRExpenseHead ObjExpenseHead)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseHeadId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjExpenseHead.ExpenseHeadId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjExpenseHead.ExpenseCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseHead", MySqlDbType = MySqlDbType.VarChar, Value = ObjExpenseHead.ExpHead });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjExpenseHead.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstExpenseHead", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjExpenseHead.ExpenseHeadId == 0 ? "Expense Head Details Saved Successfully" : "Expense Head Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Expense Code Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Expense Head Already Exist";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }

        }
        public void DeleteExpenseHead(int ExpenseHeadId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseHeadId", MySqlDbType = MySqlDbType.Int32, Value = ExpenseHeadId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstExpenseHead", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Expense Head Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = ReturnObj;
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
        public void GetExpenseHead(int ExpenseHeadId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseHeadId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ExpenseHeadId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstExpenseHead", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            DSRExpenseHead ObjExpense = new DSRExpenseHead();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjExpense.ExpenseHeadId = Convert.ToInt32(Result["ExpenseHeadId"]);
                    ObjExpense.ExpHead = (Result["ExpenseHead"] == null ? "" : Result["ExpenseHead"]).ToString();
                    ObjExpense.ExpenseCode = (Result["ExpenseCode"]==null?"":Result["ExpenseCode"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjExpense;
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
        public void GetAllExpenseHead()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName= "in_ExpenseHeadId", MySqlDbType=MySqlDbType.Int32,Value=0});
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstExpenseHead", CommandType.StoredProcedure,DParam);
            List<DSRExpenseHead> LstExpenseHead = new List<DSRExpenseHead>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExpenseHead.Add(new DSRExpenseHead
                    {
                        ExpenseCode=Result["ExpenseCode"].ToString(),
                        ExpHead=Result["ExpenseHead"].ToString(),
                        ExpenseHeadId=Convert.ToInt32(Result["ExpenseHeadId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExpenseHead;
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
        }
        public void GetAllHSNCode()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstHSN", CommandType.StoredProcedure, DParam);
            List<DSRExpenseCodeWiseHSN> LstExpenseHead = new List<DSRExpenseCodeWiseHSN>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExpenseHead.Add(new DSRExpenseCodeWiseHSN
                    {
                        HSNCode = Result["HSNCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExpenseHead;
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
        }
        public void AddExpenseCodeWiseHSN(DSRExpenseCodeWiseHSN ObjExpense)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpHSNId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjExpense.ExpHSNId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpenseCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjExpense.ExpenseCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HSNCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjExpense.HSNCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjExpense.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddExpenseCodeWiseHSN", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjExpense.ExpHSNId == 0 ? "Expense Code Wise HSN Details Saved Successfully" : "Expense Code Wise HSN Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Combination Of Expense Code And HSN Code Already Exist";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllExpCodeWiseHSN()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExpenseCodeWiseHSN", CommandType.StoredProcedure, DParam);
            List<DSRExpenseCodeWiseHSN> LstExpenseHead = new List<DSRExpenseCodeWiseHSN>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExpenseHead.Add(new DSRExpenseCodeWiseHSN
                    {
                        HSNCode = Result["HSNCode"].ToString(),
                        ExpenseCode=Result["ExpenseCode"].ToString(),
                        ExpHSNId=Convert.ToInt32(Result["ExpHSNId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExpenseHead;
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
        }

            
        public void GetExpHSNCode(int ExpHSNId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpHSNId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ExpHSNId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstExpHSN", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRExpenseCodeWiseHSN ObjExpense = new DSRExpenseCodeWiseHSN();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjExpense.ExpHSNId = Convert.ToInt32(Result["ExpHSNId"]);
                    ObjExpense.HSNCode = (Result["HSNCode"] == null ? "" : Result["HSNCode"]).ToString();
                    ObjExpense.ExpenseCode = (Result["ExpenseCode"] == null ? "" : Result["ExpenseCode"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjExpense;
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
    }
}