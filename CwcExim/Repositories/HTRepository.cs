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
    public class HTRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddEditHTCharges(HTChargesVM objHT, int Uid)
        {
            /*
            Container Type: 1.Empty Container 2.Loaded Container 3.Cargo 4.RMS
            Type: 1.General 2.Heavy
            Product Type: 1.HAZ 2.Non HAZ
            */
            string id = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = objHT.HTChargesId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = objHT.OperationId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = objHT.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = objHT.Type });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objHT.Description });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.Int32, Value = objHT.Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MaxDistance", MySqlDbType = MySqlDbType.Decimal, Value = objHT.MaxDistance });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = objHT.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RateCWC", MySqlDbType = MySqlDbType.Decimal, Value = objHT.RateCWC });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContractorRate", MySqlDbType = MySqlDbType.Decimal, Value = objHT.ContractorRate});
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objHT.EffectiveDate).ToString("yyyy/MM/dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = "0" });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstHTCharges", CommandType.StoredProcedure, DParam, out id);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = id;
                    _DBResponse.Message = ((result == 1) ? "H&T Charges Saved Successfully" : "H&T Charges Updated Successfully");
                    _DBResponse.Status = result;
                }
                else if (result == 3)
                {
                    _DBResponse.Data = 0;
                    _DBResponse.Message = "Data Already Exists";
                    _DBResponse.Status = 0;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }
        public void GetAllHTCharges()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTCharges", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<HTCharges> lstCharges = new List<HTCharges>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCharges.Add(new HTCharges
                    {
                        OperationDesc = result["OperationDesc"].ToString(),
                        HTChargesId = Convert.ToInt32(result["HTChargesID"]),
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        OperationCode = result["OperationCode"].ToString(),
                        RateCWC=Convert.ToDecimal(result["RateCWC"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCharges;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetHTChargesDetails(int HTChargesId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = HTChargesId });
            IDataParameter[] dparm = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTCharges", CommandType.StoredProcedure, dparm);
            HTChargesVM objHt = new HTChargesVM();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objHt.HTChargesId = Convert.ToInt32(result["HTChargesID"]);
                    objHt.OperationId = Convert.ToInt32(result["OperationId"]);
                    objHt.ContainerType = Convert.ToInt32(result["ContainerType"]==DBNull.Value?0:result["ContainerType"]);
                    objHt.Type = Convert.ToInt32(result["Type"]==DBNull.Value?0: result["Type"]);
                    //objHt.Description = result["Description"].ToString();
                    objHt.Size = Convert.ToInt32(result["Size"]);
                    objHt.MaxDistance = Convert.ToDecimal(result["MaxDistance"]);
                    objHt.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objHt.RateCWC = Convert.ToDecimal(result["RateCWC"]);
                    objHt.ContractorRate= Convert.ToDecimal(result["ContractorRate"]);
                    objHt.EffectiveDate = (result["EffectiveDate"]).ToString();
                    objHt.OperationCode = result["OperationCode"].ToString();
                    objHt.OperationType = Convert.ToInt32(result["OperationType"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objHt;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
    }
}