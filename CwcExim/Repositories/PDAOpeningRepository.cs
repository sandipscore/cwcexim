using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;

namespace CwcExim.Repositories
{
    public class PDAOpeningRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddPDAOpening(PDAOpening ObjPDA)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PDAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPDA.PDAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPDA.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FolioNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPDA.FolioNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectDebit", MySqlDbType = MySqlDbType.Int32, Value = ObjPDA.DirectDebit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NegativeBalance", MySqlDbType = MySqlDbType.Int32, Value = ObjPDA.NegativeBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPDA.Date) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPDA.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjPDA.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMstPDAOpening", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "PDA Opening Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "PDA Opening Details Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result==3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Folio No Already Exist";
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
        public void GetEximTrader()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstPDA", CommandType.StoredProcedure);
            List<PDAOpening> LstPDA = new List<PDAOpening>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDA.Add(new PDAOpening
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDA;
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
        public void GetPDAOpening(int PDAId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PDAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PDAId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPDAOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PDAOpening ObjPDA = new PDAOpening();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPDA.PDAId = Convert.ToInt32(Result["PDAId"]);
                    ObjPDA.EximTraderId = Convert.ToInt32(Result["EximTraderId"]);
                    ObjPDA.FolioNo = (Result["FolioNo"] == null ? "" : Result["FolioNo"]).ToString();
                    ObjPDA.Date = Convert.ToString(Result["Date"] == null ? "" : Result["Date"]);
                    ObjPDA.Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? 0 : Result["Amount"]);
                    ObjPDA.DirectDebit = Convert.ToBoolean(Result["DirectDebit"] == DBNull.Value ? 0 : Result["DirectDebit"]);
                    ObjPDA.NegativeBalance = Convert.ToBoolean(Result["NegativeBalance"] == DBNull.Value ? 0 : Result["NegativeBalance"]);
                    ObjPDA.EximTraderName = (Result["EximTraderName"] == null ? "" : Result["EximTraderName"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPDA;
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
        public void GetAllPDAOpening()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PDAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPDAOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PDAOpening> LstPDAOpening = new List<PDAOpening>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDAOpening.Add(new PDAOpening
                    {
                        PDAId = Convert.ToInt32(Result["PDAId"]),
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        FolioNo = (Result["FolioNo"] == null ? "" : Result["FolioNo"]).ToString(),
                        Date = Convert.ToString(Result["Date"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        DirectDebit = Convert.ToBoolean(Result["DirectDebit"]),
                        NegativeBalance = Convert.ToBoolean(Result["NegativeBalance"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDAOpening;
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