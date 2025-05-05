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
    public class TTUN_RateRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void ListOfSACCodeRate()
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



        public void AddEditMstRateFees(CHNStorageCharge objStorage, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Value = objStorage.StorageChargeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeId", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objStorage.ChargeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objStorage.SACId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.OperationType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ContainerLoadType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsOdc", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objStorage.IsOdc) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Rate", MySqlDbType = MySqlDbType.Decimal, Value = objStorage.Rate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objStorage.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationPer", MySqlDbType = MySqlDbType.VarChar, Value = objStorage.ExaminationPer });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objStorage.PartyId) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditPartyWiseMstRateFees", CommandType.StoredProcedure, dparam);
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
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfSac", CommandType.StoredProcedure);
            List<string> lstSac = new List<string>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSac.Add(Result["SacCode"].ToString());
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
            IDataReader result = DA.ExecuteDataReader("GetMstPartyWiseMiscRateFees", CommandType.StoredProcedure, dparam);
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
                            IsOdc = Convert.ToBoolean(result["IsOdc"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            EffectiveDate = result["EffectiveDate"].ToString(),
                            CartingType = result["CartingType"].ToString(),
                             PartyId = Convert.ToInt32(result["PartyId"]),
                             PartyName = Convert.ToString(result["EximTraderName"]),
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

                        objSC.PartyId = Convert.ToInt32(result["PartyId"]);
                        objSC.PartyName = Convert.ToString(result["EximTraderName"]);
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
        #region HT Charges
        public void AddEditHTCharges(CHNHTCharges objHT, int Uid, String ChargeListXML)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = objHT.Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargesFor", MySqlDbType = MySqlDbType.VarChar, Value = objHT.ChargesFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MaxDistance", MySqlDbType = MySqlDbType.Decimal, Value = objHT.MaxDistance });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = objHT.CommodityType });

            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = objHT.ContainerLoadType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = objHT.TransportFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EximType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = objHT.EximType });

            lstParam.Add(new MySqlParameter { ParameterName = "in_RateCWC", MySqlDbType = MySqlDbType.Decimal, Value = objHT.RateCWC });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContractorRate", MySqlDbType = MySqlDbType.Decimal, Value = objHT.ContractorRate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objHT.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = "0" });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Text, Direction = ParameterDirection.Output, Value = "0" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeListXML", MySqlDbType = MySqlDbType.Text, Value = ChargeListXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SlabType", MySqlDbType = MySqlDbType.Int32, Value = objHT.SlabType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeightSlab", MySqlDbType = MySqlDbType.Int32, Value = objHT.WeightSlab });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistanceSlab", MySqlDbType = MySqlDbType.Int32, Value = objHT.DistanceSlab });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsODC", MySqlDbType = MySqlDbType.Int32, Value = objHT.IsODC });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objHT.PartyId });

            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditPartywiseMstHTCharges", CommandType.StoredProcedure, DParam, out id);
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

        public void GetSlabData(string Size, string ChargesFor, string OperationCode)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Size });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargesFor", MySqlDbType = MySqlDbType.VarChar, Value = ChargesFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationCode", MySqlDbType = MySqlDbType.VarChar, Value = OperationCode });
            IDataParameter[] Dparam = lstParam.ToArray();

            IDataReader result = DA.ExecuteDataReader("GetSlabData", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();

            CHNHTCharges Obj = new CHNHTCharges();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    Obj.LstWeightSlab.Add(new CHNWeightSlab
                    {
                        WeightSlabId = Convert.ToInt32(result["WeightSlabId"].ToString()),
                        FromWeightSlab = Convert.ToInt32(result["FromWeightSlab"].ToString()),
                        ToWeightSlab = Convert.ToInt32(result["ToWeightSlab"]),
                        chkWeightSlab = false,
                        Size = Convert.ToString(result["Size"]),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        Obj.LstDistanceSlab.Add(new CHNDistanceSlab
                        {
                            DistanceSlabId = Convert.ToInt32(result["DistanceSlabId"].ToString()),
                            FromDistanceSlab = Convert.ToInt32(result["FromDistanceSlab"].ToString()),
                            ToDistanceSlab = Convert.ToInt32(result["ToDistanceSlab"]),
                            chkDistanceSlab = false,
                            Size = Convert.ToString(result["Size"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = Obj;
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

        public void GetHTSlabChargesDtl(int HTChargesID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = HTChargesID });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = { };
            Dparam = lstParam.ToArray();
            DataSet result = DA.ExecuteDataSet("GetAllPartywiseMstHTCharges", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            List<CHNChargeList> LstCharge = new List<CHNChargeList>();
            int Status = 0;
            try
            {
                foreach (DataRow dr in result.Tables[1].Rows)
                {
                    Status = 1;
                    LstCharge.Add(new CHNChargeList
                    {
                        WtSlabId = Convert.ToInt32(dr["WtSlabId"].ToString()),
                        FromWtSlabCharge = Convert.ToInt32(dr["FromWtSlabCharge"].ToString()),
                        ToWtSlabCharge = Convert.ToInt32(dr["ToWtSlabCharge"]),
                        DisSlabId = Convert.ToInt32(dr["DisSlabId"].ToString()),
                        FromDisSlabCharge = Convert.ToInt32(dr["FromDisSlabCharge"].ToString()),
                        ToDisSlabCharge = Convert.ToInt32(dr["ToDisSlabCharge"]),
                        CwcRate = Convert.ToDecimal(dr["RateCwc"]),
                        ContractorRate = Convert.ToDecimal(dr["ContractorRate"]),
                        RoundTripRate = Convert.ToDecimal(dr["RoundTripRate"]),
                        EmptyRate = Convert.ToDecimal(dr["EmptyRate"]),
                        SlabType = Convert.ToInt32(dr["SlabType"]),
                        WeightSlab = Convert.ToInt32(dr["WeightSlab"]),
                        DistanceSlab = Convert.ToInt32(dr["DistanceSlab"]),
                        AddlWtCharges = Convert.ToDecimal(dr["AddlWtCharges"]),
                        AddlDisCharges = Convert.ToDecimal(dr["AddlDisCharges"]),
                        PortId = Convert.ToInt32(dr["PortId"]),
                        PortName = Convert.ToString(dr["PortName"])
                      

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Data = LstCharge;
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
                //result.Close();
            }
        }

        public void GetAllHTCharges()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllPartywiseMstHTCharges", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            CHNHTCharges lstCharges = new CHNHTCharges();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCharges.LstviewList.Add(new CHNViewList
                    {
                        OperationDesc = result["OperationDesc"].ToString(),
                        HTChargesId = Convert.ToInt32(result["HTChargesID"]),
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        OperationCode = result["OperationCode"].ToString(),
                        RateCWC = Convert.ToDecimal(result["RateCWC"]),
                        ChargesFor = result["ChargesFor"].ToString(),
                         PartyName = result["PartyName"].ToString(),

                    });
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        lstCharges.LstWeightSlab.Add(new CHNWeightSlab
                        {
                            WeightSlabId = Convert.ToInt32(result["WeightSlabId"].ToString()),
                            FromWeightSlab = Convert.ToInt32(result["FromWeightSlab"].ToString()),
                            ToWeightSlab = Convert.ToInt32(result["ToWeightSlab"]),
                            chkWeightSlab = false,
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        lstCharges.LstDistanceSlab.Add(new CHNDistanceSlab
                        {
                            DistanceSlabId = Convert.ToInt32(result["DistanceSlabId"].ToString()),
                            FromDistanceSlab = Convert.ToInt32(result["FromDistanceSlab"].ToString()),
                            ToDistanceSlab = Convert.ToInt32(result["ToDistanceSlab"]),
                            chkDistanceSlab = false,
                        });
                    }
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
            IDataReader result = DA.ExecuteDataReader("GetAllPartyWiseMstHTCharges", CommandType.StoredProcedure, dparm);
            CHNHTCharges objHt = new CHNHTCharges();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objHt.HTChargesId = Convert.ToInt32(result["HTChargesID"]);
                    objHt.OperationId = Convert.ToInt32(result["OperationId"]);
                    objHt.ContainerType = Convert.ToInt32(result["ContainerType"] == DBNull.Value ? 0 : result["ContainerType"]);
                    objHt.Type = Convert.ToInt32(result["Type"] == DBNull.Value ? 0 : result["Type"]);
                    //objHt.Description = result["Description"].ToString();
                    objHt.Size = Convert.ToString(result["Size"]);
                    objHt.MaxDistance = Convert.ToDecimal(result["MaxDistance"]);
                    objHt.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objHt.RateCWC = Convert.ToDecimal(result["RateCWC"]);
                    objHt.ContractorRate = Convert.ToDecimal(result["ContractorRate"]);
                    objHt.EffectiveDate = (result["EffectiveDate"]).ToString();
                    objHt.OperationCode = result["OperationCode"].ToString();
                    objHt.OperationType = Convert.ToInt32(result["OperationType"]);
                    objHt.ContainerLoadType = (result["ContainerLoadType"]).ToString();
                    objHt.TransportFrom = result["TransportFrom"].ToString();
                    objHt.EximType = result["EximType"].ToString();
                    objHt.SlabType = Convert.ToInt32(result["SlabType"]);
                    objHt.WeightSlab = Convert.ToInt32(result["WeightSlab"]);
                    objHt.DistanceSlab = Convert.ToInt32(result["DistanceSlab"]);
                    objHt.ChargesFor = Convert.ToString(result["ChargesFor"]);
                    objHt.IsODC = Convert.ToInt32(result["IsODC"]);
                    objHt.PartyId = Convert.ToInt32(result["PartyId"]);
                    objHt.PartyName= Convert.ToString(result["PartyName"]);
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
        #endregion

        #region Miscellaneous
        public void AddEditMiscellaneous(CHNMiscellaneous ObjMiscellaneous)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MiscellaneousId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMiscellaneous.MiscellaneousId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fumigation", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.Fumigation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Washing", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.Washing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reworking", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.Reworking });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Bagging", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.Bagging });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjMiscellaneous.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Palletizing", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.Palletizing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PrintingCharges", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.PrintingCharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjMiscellaneous.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjMiscellaneous.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstMiscellaneous", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjMiscellaneous.MiscellaneousId == 0 ? "Miscellaneous Details Saved Successfully" : "Miscellaneous Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Fumigation Detail Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Washing Detail Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Reworking Detail Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Bagging Detail Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Palletizing Detail Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Printing Charges Already Exist";
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
        public void GetAllMiscellaneous()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MiscellaneousId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstMiscellaneous", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHNMiscellaneous> LstMiscellaneous = new List<CHNMiscellaneous>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstMiscellaneous.Add(new CHNMiscellaneous
                    {
                        MiscellaneousId = Convert.ToInt32(Result["MiscellaneousId"]),
                        Fumigation = Convert.ToDecimal(Result["Fumigation"]),
                        Washing = Convert.ToDecimal(Result["Washing"]),
                        Reworking = Convert.ToDecimal(Result["Reworking"]),
                        Bagging = Convert.ToDecimal(Result["Bagging"]),
                        Palletizing = Convert.ToDecimal(Result["Palletizing"]),
                        PrintingCharges = Convert.ToDecimal(Result["PrintingCharges"]),
                        EffectiveDate = Convert.ToString(Result["EffectiveDate"]),
                        SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMiscellaneous;
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
        public void GetMiscellaneous(int MiscellaneousId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MiscellaneousId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MiscellaneousId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstMiscellaneous", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHNMiscellaneous ObjMiscellaneous = new CHNMiscellaneous();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjMiscellaneous.MiscellaneousId = Convert.ToInt32(Result["MiscellaneousId"]);
                    ObjMiscellaneous.Fumigation = Convert.ToDecimal(Result["Fumigation"]);
                    ObjMiscellaneous.Washing = Convert.ToDecimal(Result["Washing"]);
                    ObjMiscellaneous.Reworking = Convert.ToDecimal(Result["Reworking"]);
                    ObjMiscellaneous.Bagging = Convert.ToDecimal(Result["Bagging"]);
                    ObjMiscellaneous.Palletizing = Convert.ToDecimal(Result["Palletizing"]);
                    ObjMiscellaneous.PrintingCharges = Convert.ToDecimal(Result["PrintingCharges"]);
                    ObjMiscellaneous.EffectiveDate = Convert.ToString(Result["EffectiveDate"]);
                    ObjMiscellaneous.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjMiscellaneous;
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

        #endregion

        #region Storage Charge
        public void AddEditStorageCharge(CHNCWCStorageCharge ObjStorage)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjStorage.StorageChargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WarehouseType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.WarehouseType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.ChargeType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RsrvRateSqMeterPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMeterPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerDay });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ShutOutCargoRatePerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.ShutOutCargoRateSqMeterPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMeterPerMonth", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMeterPerMonth });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjStorage.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStorage.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeFrom", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeTo", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.CommodityType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjStorage.ContainerLoadType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageType", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = ObjStorage.StorageType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaType", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = ObjStorage.AreaType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjStorage.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditPartyWiseMstStorageCharge", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjStorage.StorageChargeId == 0 ? "Storage Charge Details Saved Successfully" : "Storage Charge Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Combination Of Warehouse Type And Charges Type Already Exist";
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
        public void GetAllStorageCharge()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartywiseMstStorageCharge", CommandType.StoredProcedure, DParam);
            List<CHNCWCStorageCharge> LstStorageCharge = new List<CHNCWCStorageCharge>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStorageCharge.Add(new CHNCWCStorageCharge
                    {
                        StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]),
                        WarehouseTypeName = Convert.ToString(Result["WarehouseTypeName"]),
                        //ChargeTypeName = Convert.ToString(Result["ChargeTypeName"]),
                        //RateMeterPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]),
                        //RateSqMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]),
                        RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]),
                        //RateCubMeterPerDay = Convert.ToDecimal(Result["RateCubMeterPerDay"]),
                        RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]),
                        RateSqMeterPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]),
                        EffectiveDate = (Result["EffectiveDate"]).ToString(),
                        DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                        Size = Convert.ToString(Result["Size"]),
                        DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                         PartyId = Convert.ToInt32(Result["PartyId"]),

                        PartyName=Convert.ToString(Result["EximTraderName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStorageCharge;
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
        public void GetStorageCharge(int StorageChargeId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Value = StorageChargeId, Size = 11 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartywiseMstStorageCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHNCWCStorageCharge ObjStorage = new CHNCWCStorageCharge();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStorage.StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]);
                    ObjStorage.RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                    ObjStorage.RateSqMPerDay = Convert.ToDecimal(Result["RateSqMeterPerDay"]);
                    ObjStorage.RateSqMeterPerWeek = Convert.ToDecimal(Result["RsrvRateSqMeterPerWeek"]);
                    //ObjStorage.RateCubMeterPerDay = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
                    ObjStorage.RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]);
                    //ObjStorage.RateCubMeterPerMonth = Convert.ToDecimal(Result["RateCubMeterPerMonth"]);
                    ObjStorage.RateSqMeterPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                    ObjStorage.ContainerLoadType = (Result["ContainerLoadType"]).ToString();
                    ObjStorage.StorageType = (Result["StorageType"]).ToString();
                    ObjStorage.AreaType = (Result["AreaType"]).ToString();
                    ObjStorage.EffectiveDate = (Result["EffectiveDate"]).ToString();
                    ObjStorage.WarehouseType = Convert.ToInt32(Result["WarehouseType"]);
                    ObjStorage.ChargeType = Convert.ToInt32(Result["ChargeType"] == DBNull.Value ? 0 : Result["ChargeType"]);
                    ObjStorage.DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"] == DBNull.Value ? 0 : Result["DaysRangeFrom"]);
                    ObjStorage.DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"] == DBNull.Value ? 0 : Result["DaysRangeTo"]);
                    ObjStorage.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjStorage.CommodityType = Convert.ToInt32(Result["CommodityType"]);
                    ObjStorage.Size = (Result["Size"]).ToString();
                    ObjStorage.PartyId = Convert.ToInt32(Result["PartyId"]);

                    ObjStorage.PartyName = Convert.ToString(Result["EximTraderName"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStorage;
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
        #endregion
        #region Weighment
        public void AddEditMstWeighment(CHNCWCWeighment objCW, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = objCW.WeighmentId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerRate", MySqlDbType = MySqlDbType.Decimal, Value = objCW.ContainerRate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TruckRate", MySqlDbType = MySqlDbType.Decimal, Value = objCW.TruckRate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objCW.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCW.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCW.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCW.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dpram = lstParam.ToArray();
            int result = DA.ExecuteNonQuery("AddEditPartyWiseMstWeighment", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = ((result == 1) ? "Weighment Details Saved Successfully" : "Weighment Details Updated Successfully");
                }
                else if (result == 3 || result == 4 || result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = ((result == 3) ? "Combination of Container Rate and Truck Rate Already Exists" : ((result == 4) ? "Container Rate Already Exists" : "Truck Rate Already Exists"));
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetWeighmentDet(int WeighmentId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = WeighmentId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllPartywiseWeighmentDet", CommandType.StoredProcedure, Dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            IList<CHNCWCWeighment> lstWeighment = null;
            CHNCWCWeighment objCWC = null;
            try
            {
                if (WeighmentId == 0)
                {
                    lstWeighment = new List<CHNCWCWeighment>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstWeighment.Add(new CHNCWCWeighment
                        {
                            WeighmentId = Convert.ToInt32(result["WeighmentId"]),
                            ContainerRate = Convert.ToDecimal(result["ContainerRate"]),
                            ContainerSize = result["ContainerSize"].ToString(),
                            EffectiveDate = Convert.ToString(result["EffectiveDate"]),
                            SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString(),
                            TruckRate = Convert.ToDecimal(result["TruckRate"]),
                             PartyName = Convert.ToString(result["EximTraderName"]),
                            
                        });
                    }
                }
                else
                {
                    objCWC = new CHNCWCWeighment();
                    while (result.Read())
                    {
                        Status = 2;
                        objCWC.WeighmentId = Convert.ToInt32(result["WeighmentId"]);
                        objCWC.ContainerRate = Convert.ToDecimal(result["ContainerRate"]);
                        objCWC.ContainerSize = result["ContainerSize"].ToString();
                        objCWC.EffectiveDate = Convert.ToString(result["EffectiveDate"]);
                        objCWC.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();
                        objCWC.TruckRate = Convert.ToDecimal(result["TruckRate"]);
                    }

                }
                if (Status == 1 || Status == 2)
                {
                    if (Status == 1) _DBResponse.Data = lstWeighment;
                    else _DBResponse.Data = objCWC;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
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
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        #region Entry Fees
        public void AddEditMstEntryFees(CHNCWCEntryFees objEF, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryFeeId", MySqlDbType = MySqlDbType.Int32, Value = objEF.EntryFeeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.OperationType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objEF.Reefer) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Rate", MySqlDbType = MySqlDbType.Decimal, Value = objEF.Rate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objEF.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objEF.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objEF.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 7, Value =Convert.ToInt32(objEF.PartyId) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditPartywiseMstEntryFees", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Entery Fees Saved Successfully" : "Entry Fees Updated Successfully";
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
        public void GetAllEntryFees(int EntryFeeId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_EntryFeeId", MySqlDbType = MySqlDbType.Int32, Value = EntryFeeId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetPartyWiseMstEntryFees", CommandType.StoredProcedure, dparam);
            IList<CHNCWCEntryFees> lstEntryFees = null;
            CHNCWCEntryFees objEF = null;
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                if (EntryFeeId == 0)
                {
                    lstEntryFees = new List<CHNCWCEntryFees>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstEntryFees.Add(new CHNCWCEntryFees
                        {
                            EntryFeeId = Convert.ToInt32(result["EntryFeeId"]),
                            ContainerType = Convert.ToInt32(result["ContainerType"]),
                            CommodityType = Convert.ToInt32(result["CommodityType"]),
                            OperationType = Convert.ToInt32(result["OperationType"]),
                            Reefer = Convert.ToBoolean(result["Reefer"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            EffectiveDate = result["EffectiveDate"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                             PartyId= Convert.ToInt32(result["PartyId"]),
                             PartyName = result["EximTraderName"].ToString(),
                        });
                    }
                }
                else
                {
                    objEF = new CHNCWCEntryFees();
                    while (result.Read())
                    {
                        Status = 2;
                        objEF.EntryFeeId = Convert.ToInt32(result["EntryFeeId"]);
                        objEF.ContainerType = Convert.ToInt32(result["ContainerType"]);
                        objEF.CommodityType = Convert.ToInt32(result["CommodityType"]);
                        objEF.OperationType = Convert.ToInt32(result["OperationType"]);
                        objEF.Reefer = Convert.ToBoolean(result["Reefer"]);
                        objEF.Rate = Convert.ToDecimal(result["Rate"]);
                        objEF.EffectiveDate = result["EffectiveDate"].ToString();
                        objEF.ContainerSize = result["ContainerSize"].ToString();
                        objEF.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();
                        objEF.PartyId = Convert.ToInt32(result["PartyId"]);
                        objEF.PartyName = result["EximTraderName"].ToString();
                    }
                }
                if (Status == 1 || Status == 2)
                {
                    if (Status == 1) _DBResponse.Data = lstEntryFees;
                    else _DBResponse.Data = objEF;
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
        #endregion

        #region Ground Rent
        public void AddEditMstGroundRent(CHNCWCChargesGroundRent objCR, int Uid)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_GroundRentId", MySqlDbType = MySqlDbType.Int32, Value = objCR.GroundRentId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = objCR.ContainerType, Size = 2 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = objCR.CommodityType, Size = 2 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_DaysRangeFrom", MySqlDbType = MySqlDbType.Int32, Value = objCR.DaysRangeFrom });
            lstparam.Add(new MySqlParameter { ParameterName = "in_DaysRangeTo", MySqlDbType = MySqlDbType.Int32, Value = objCR.DaysRangeTo });
            //lstparam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Int32, Value = objCR.Reefer, Size = 2 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_RentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objCR.RentAmount });
            //lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objCR.ElectricityCharge });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objCR.Size });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ODC", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objCR.IsODC) });
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objCR.OperationType });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCR.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCR.PartyId });
            lstparam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditPartyWiseMstGroundRent", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1 ? "Ground Rent Details Saved Successfully" : "Ground Rent Details Updated Successfully");
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
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
        public void GetAllGroundRentDet()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_GroundRentId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetPartWiseAllGroundRentDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CHNCWCChargesGroundRent> objList = new List<CHNCWCChargesGroundRent>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CHNCWCChargesGroundRent
                    {
                        GroundRentId = Convert.ToInt32(result["GroundRentId"]),
                        ContainerType = Convert.ToInt32(result["ContainerType"]),
                        RentAmount = Convert.ToDecimal(result["RentAmount"]),
                        Size = result["Size"].ToString(),
                        DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]),
                        DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]),
                        OperationType = Convert.ToInt32(result["OperationType"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        CommodityType = Convert.ToInt32(result["CommodityType"]),
                        PartyName=Convert.ToString(result["EximTraderName"]),
                         PartyId= Convert.ToInt32(result["PartyId"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objList;
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
        public void GetGroundRentDet(int GroundRentId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_GroundRentId", MySqlDbType = MySqlDbType.Int32, Value = GroundRentId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetPartWiseAllGroundRentDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CHNCWCChargesGroundRent objGR = null;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objGR = new CHNCWCChargesGroundRent();
                    objGR.GroundRentId = Convert.ToInt32(result["GroundRentId"]);
                    objGR.ContainerType = Convert.ToInt32(result["ContainerType"]);
                    objGR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objGR.DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]);
                    objGR.DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]);
                    //objGR.Reefer = Convert.ToBoolean(result["Reefer"]);
                    objGR.RentAmount = Convert.ToDecimal(result["RentAmount"]);
                    //objGR.ElectricityCharge = Convert.ToDecimal(result["ElectricityCharge"]);
                    objGR.Size = result["Size"].ToString();
                    objGR.OperationType = Convert.ToInt32(result["OperationType"]);
                    objGR.EffectiveDate = result["EffectiveDate"].ToString();
                    objGR.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();
                    objGR.IsODC = Convert.ToBoolean(result["IsODC"]);
                    objGR.PartyName = Convert.ToString(result["EximTraderName"]);
                    objGR.PartyId = Convert.ToInt32(result["PartyId"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objGR;
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
        #endregion

        #region Reefer
        public void AddEditMstReefer(CHNCWCReefer objRef)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = objRef.ReeferChrgId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRef.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objRef.ElectricityCharge });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objRef.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objRef.ContainerSize });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objRef.PartyId) });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditPartyWiseMstReefer", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1 ? "Reefer Details Saved Successfully" : "Reefer Details Updated Successfully");
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
        public void GetAllReefer()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ListOfPartywiseMstReefer", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CHNCWCReefer> objList = new List<CHNCWCReefer>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CHNCWCReefer
                    {
                        ReeferChrgId = Convert.ToInt32(result["ReeferChrgId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ElectricityCharge = Convert.ToDecimal(result["ElectricityCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        PartyId = Convert.ToInt32(result["PartyId"].ToString()),
                        PartyName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objList;
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
        public void GetReeferDet(int ReeferChrgId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = ReeferChrgId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ListOfPartywiseMstReefer", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CHNCWCReefer objRef = new CHNCWCReefer();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objRef.ReeferChrgId = Convert.ToInt32(result["ReeferChrgId"]);
                    objRef.EffectiveDate = result["EffectiveDate"].ToString();
                    objRef.ElectricityCharge = Convert.ToDecimal(result["ElectricityCharge"]);
                    objRef.SacCode = result["SacCode"].ToString();
                    objRef.ContainerSize = result["ContainerSize"].ToString();
                    objRef.PartyId =Convert.ToInt32(result["PartyId"].ToString());
                    objRef.PartyName = result["EximTraderName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objRef;
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
        #endregion

        #region Insurance
        public void AddEditInsurance(CHNInsurance ObjInsurance)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInsurance.InsuranceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Charge", MySqlDbType = MySqlDbType.Decimal, Value = ObjInsurance.Charge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjInsurance.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjInsurance.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjInsurance.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInsurance.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditPartyWiseMstInsurance", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInsurance.InsuranceId == 0 ? "Insurance Details Saved Successfully" : "Insurance Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Charge Detail Already Exist";
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
        public void GetAllInsurance()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartywiseMstInsurance", CommandType.StoredProcedure, DParam);
            List<CHNInsurance> LstInsurance = new List<CHNInsurance>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInsurance.Add(new CHNInsurance
                    {
                        InsuranceId = Convert.ToInt32(Result["InsuranceId"]),
                        Charge = Convert.ToDecimal(Result["Charge"]),
                        EffectiveDate = Convert.ToString(Result["EffectiveDate"]),
                        SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["EximTraderName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInsurance;
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
        public void GetInsurance(int InsuranceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceId", MySqlDbType = MySqlDbType.Int32, Value = InsuranceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            CHNInsurance ObjInsurance = new CHNInsurance();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartywiseMstInsurance", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInsurance.InsuranceId = Convert.ToInt32(Result["InsuranceId"]);
                    ObjInsurance.Charge = Convert.ToDecimal(Result["Charge"]);
                    ObjInsurance.EffectiveDate = Convert.ToString(Result["EffectiveDate"]);
                    ObjInsurance.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjInsurance.PartyId = Convert.ToInt32(Result["PartyId"]);
                    ObjInsurance.PartyName = Convert.ToString(Result["EximTraderName"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInsurance;
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
        #endregion

        #region Franchise Charges
        public void GetFranchiseCharge(int franchisechargeid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = franchisechargeid });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetPartyWiseMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CHNCWCFranchiseCharges objFcs = new CHNCWCFranchiseCharges();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFcs.franchisechargeid = Convert.ToInt32(result["franchisechargeid"]);
                    objFcs.EffectiveDate = result["EffectiveDate"].ToString();
                    objFcs.ContainerSize = result["ContainerSize"].ToString();
                    objFcs.ChargesFor = result["Chargesfor"].ToString();
                    objFcs.ODC = Convert.ToBoolean(result["ODC"]);
                    objFcs.RoaltyCharge = Convert.ToDecimal(result["RoaltyCharge"]);
                    objFcs.FranchiseCharge = Convert.ToDecimal(result["FranchiseCharge"]);
                    objFcs.SacCode = result["SacCode"].ToString();
                    objFcs.PartyName = result["EximTraderName"].ToString();
                    objFcs.PartyId =Convert.ToInt32(result["PartyId"].ToString());

                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFcs;
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
        public void AddEditMstFranchiseCharges(CHNCWCFranchiseCharges objFC, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = objFC.franchisechargeid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFC.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Chargesfor", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.ChargesFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ODC", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objFC.ODC) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objFC.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_roaltycharge", MySqlDbType = MySqlDbType.Decimal, Value = objFC.RoaltyCharge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_franchisecharge", MySqlDbType = MySqlDbType.Decimal, Value = objFC.FranchiseCharge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objFC.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditPartyWiseMstFranchise", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Franchise Charge Saved Successfully" : "Franchise Charge Updated Successfully";
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
        public void GetAllFranchiseCharges()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_franchisechargeid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetPartyWiseMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CHNCWCFranchiseCharges> objList = new List<CHNCWCFranchiseCharges>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CHNCWCFranchiseCharges
                    {
                        franchisechargeid = Convert.ToInt32(result["franchisechargeid"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        //Chargesfor = result["Chargesfor"].ToString(),
                        //ODC = Convert.ToBoolean(result["ODC"]),
                        RoaltyCharge = Convert.ToDecimal(result["RoaltyCharge"]),
                        FranchiseCharge = Convert.ToDecimal(result["FranchiseCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        PartyName = result["EximTraderName"].ToString(),
                    PartyId = Convert.ToInt32(result["PartyId"].ToString())
                });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objList;
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
        #endregion

        #region PORT
        public void AddEditPort(CHNPort ObjPort)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPort.PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPort.PortName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjPort.PortAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjPort.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstPort", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjPort.PortId == 0 ? "Port Details Saved Successfully" : "Port Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Port Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Port Alias Already Exists";
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
        public void DeletePort(int PortId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstPort", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Port Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot delete as it exists in another page";
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
                DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void GetAllPort()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure, DParam);
            List<CHNPort> LstPort = new List<CHNPort>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new CHNPort
                    {
                        PortName = Result["PortName"].ToString(),
                        PortAlias = Result["PortAlias"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"]),
                        CountryId = Convert.ToInt32(Result["CountryId"]),
                        StateId = Convert.ToInt32(Result["StateId"]),
                        POD = Convert.ToBoolean(Result["POD"]),
                        CountryName = Result["CountryName"].ToString(),
                        StateName = Result["StateName"].ToString(),
                        //Distance =Convert.ToDecimal(Result["Distance"].ToString())
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
        public void GetPort(int PortId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PortId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            CHNPort ObjPort = new CHNPort();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPort.PortId = Convert.ToInt32(Result["PortId"]);
                    ObjPort.PortName = Result["PortName"].ToString();
                    ObjPort.PortAlias = Result["PortAlias"].ToString();
                    ObjPort.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjPort.StateId = Convert.ToInt32(Result["StateId"]);
                    ObjPort.POD = Convert.ToBoolean(Result["POD"]);
                    ObjPort.CountryName = Result["CountryName"].ToString();
                    ObjPort.StateName = Result["StateName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPort;
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

        public string GetPortname(int PortId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PortId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            PPGPost ObjPort = new PPGPost();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPort", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPort.PortId = Convert.ToInt32(Result["PortId"]);
                    ObjPort.PortName = Result["PortName"].ToString();
                    ObjPort.PortAlias = Result["PortAlias"].ToString();
                    ObjPort.CountryId = Convert.ToInt32(Result["CountryId"]);
                    ObjPort.StateId = Convert.ToInt32(Result["StateId"]);
                    ObjPort.POD = Convert.ToBoolean(Result["POD"]);
                    ObjPort.CountryName = Result["CountryName"].ToString();
                    ObjPort.StateName = Result["StateName"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPort;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                return (ObjPort.PortName);
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
            return (ObjPort.PortName);
        }
        #endregion
    }
}