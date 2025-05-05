using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Master.Models;

namespace CwcExim.Repositories
{
    public class CHN_RateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddEditMstRateFees(CHNStorageCharge objStorage, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Value = objStorage.StorageChargeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeId", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objStorage.ChargeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objStorage.SACId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar,  Value = objStorage.OperationType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ContainerLoadType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsOdc", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objStorage.IsOdc) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Rate", MySqlDbType = MySqlDbType.Decimal, Value = objStorage.Rate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objStorage.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationPer", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ExaminationPer });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstRateFees", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Rate Saved Successfully" : "Rate Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Data Already Exists";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListOfSACCode()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfChennaiSac", CommandType.StoredProcedure);
            IList<CHNSac> lstSac = new List<CHNSac>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSac.Add(new CHNSac
                    {
                        SACId = Convert.ToInt32(Result["SacId"].ToString()),
                        SACCode = Result["SacCode"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSac;
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

        public void ListOfChargeName()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfChargeName", CommandType.StoredProcedure);
            IList<CHNMiscCharge> LstOfChargeName = new List<CHNMiscCharge>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstOfChargeName.Add(new CHNMiscCharge
                    {
                        ChargeId = Convert.ToInt32(Result["ChargeId"].ToString()),
                        ChargesName = Result["ChargesName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOfChargeName;
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

        public void GetAllMiscRateFees(int StorageChargeId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Value = StorageChargeId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstMiscRateFees", CommandType.StoredProcedure, dparam);
            IList<CHNStorageCharge> lstRateFees = null;
            CHNStorageCharge objSC = null;
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                if (StorageChargeId == 0)
                {
                    lstRateFees = new List<CHNStorageCharge>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstRateFees.Add(new CHNStorageCharge
                        {
                            StorageChargeId = Convert.ToInt32(result["StorageChargeId"]),
                            ChargeName = Convert.ToString(result["ChargeName"]),
                            SacCode = Convert.ToString(result["SacCode"]),
                            OperationType = Convert.ToString(result["OperationType"]),
                            ContainerType = Convert.ToString(result["ContainerType"]),
                            ContainerLoadType = Convert.ToString(result["ContainerLoadType"]),
                            Size = Convert.ToString(result["Size"]),
                            IsOdc= Convert.ToBoolean(result["IsOdc"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            EffectiveDate = result["EffectiveDate"].ToString(),
                            CartingType=result["CartingType"].ToString()                            
                        });
                    }
                }
                else
                {
                    objSC = new CHNStorageCharge();
                    while (result.Read())
                    {
                        Status = 2;
                        objSC.StorageChargeId = Convert.ToInt32(result["StorageChargeId"]);
                        objSC.ChargeId = Convert.ToInt32(result["ChargeId"]);
                        objSC.ChargeName = Convert.ToString(result["ChargeName"]);
                        objSC.SacCode = Convert.ToString(result["SacCode"]);
                        objSC.SACId = Convert.ToInt32(result["SacId"]);
                        objSC.OperationType = Convert.ToString(result["OperationType"]);
                        objSC.ContainerType = Convert.ToString(result["ContainerType"]);                        
                        objSC.ContainerLoadType = Convert.ToString(result["ContainerLoadType"]);
                        objSC.Size = Convert.ToString(result["Size"]);
                        objSC.IsOdc = Convert.ToBoolean(result["IsOdc"]);
                        objSC.Rate = Convert.ToDecimal(result["Rate"]);
                        objSC.EffectiveDate = result["EffectiveDate"].ToString();
                        objSC.CartingType = result["CartingType"].ToString();
                        objSC.ExaminationPer = result["ExaminationPer"].ToString();
                    }
                }
                if (Status == 1 || Status == 2)
                {
                    if (Status == 1) _DBResponse.Data = lstRateFees;
                    else _DBResponse.Data = objSC;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = Status;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
    }
}