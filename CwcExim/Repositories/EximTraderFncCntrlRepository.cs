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
    public class EximTraderFncCntrlRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        // for search modal pop up

        public void GetEximTraderNew(int EximTraderId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = EximTraderId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderBalance", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<EximTraderFinanceControl> LstEximTrader = new List<EximTraderFinanceControl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.Add(new EximTraderFinanceControl
                    {
                       
                        PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"]),
                        CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetEximTrader()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForEximFinc", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<EximTraderFinanceControl> LstEximTrader = new List<EximTraderFinanceControl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.Add(new EximTraderFinanceControl
                    {
                        EximTraderName=Result["EximTraderName"].ToString(),
                        EximTraderId=Convert.ToInt32(Result["EximTraderId"]),
                        Address=Result["Address"].ToString(),
                        GSTNo=Result["GSTNo"].ToString(),
                        Tan=Result["Tan"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetAllEximFinanceControl()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximFinanceControl", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            List<EximTraderFinanceControl> LstEximTrader = new List<EximTraderFinanceControl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.Add(new EximTraderFinanceControl
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        FinanceControlId = Convert.ToInt32(Result["FinanceControlId"]),
                        //PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]),
                       // CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"] == DBNull.Value ? 0 : Result["CurrentBalance"]),
                       // CreditLimit = Convert.ToDecimal(Result["CreditLimit"] == DBNull.Value ? 0 : Result["CreditLimit"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        Address = (Result["Address"] == null ? "" : Result["Address"]).ToString(),
                       // GSTNo = (Result["GSTNo"] == null ? "" : Result["GSTNo"]).ToString(),
                       // Tan = (Result["Tan"]==null?"":Result["Tan"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetEximFinanceControl(int FinanceControlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FinanceControlId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximFinanceControl", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            EximTraderFinanceControl ObjEximTrader = new EximTraderFinanceControl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEximTrader.FinanceControlId = Convert.ToInt32(Result["FinanceControlId"]);
                    ObjEximTrader.EximTraderId = Convert.ToInt32(Result["EximTraderId"]);
                    ObjEximTrader.CreditPeriod = Convert.ToInt32(Result["CreditPeriod"]==DBNull.Value?0:Result["CreditPeriod"]);
                    ObjEximTrader.PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]);
                    ObjEximTrader.CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"] == DBNull.Value ? 0 : Result["CurrentBalance"]);
                    ObjEximTrader.CreditLimit = Convert.ToDecimal(Result["CreditLimit"] == DBNull.Value ? 0 : Result["CreditLimit"]);
                    ObjEximTrader.EximTraderName = Result["EximTraderName"].ToString();
                    ObjEximTrader.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjEximTrader.GSTNo = (Result["GSTNo"]==null?"":Result["GSTNo"]).ToString();
                    ObjEximTrader.Tan = (Result["Tan"]==null?"":Result["Tan"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEximTrader;
                }
                else
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch(Exception ex)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditEximFinanceControl(EximTraderFinanceControl ObjEximTrader)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32,Size=11,Value = ObjEximTrader.FinanceControlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tan", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Tan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PreviousBalance", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.PreviousBalance});
            LstParam.Add(new MySqlParameter { ParameterName = "in_CurrentBalance", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.CurrentBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreditLimit", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.CreditLimit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreditPeriod", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.CreditPeriod });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output,Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstEximFinanceControl",CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjEximTrader.FinanceControlId ==0 ? "Exim Trader Finance Control Details Saved Successfully": "Exim Trader Finance Control Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Exim Trader Finance Control Details Already Exist";
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
        public void DeleteEximFinanceControl(int FinanceControlId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FinanceControlId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0,Direction=ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId",Direction=ParameterDirection.Output,MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstEximFinanceControl", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Exim Trader Finance Control Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
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

    }
}