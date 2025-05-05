using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace CwcExim.Repositories
{
    public class LoniCWCChargesRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
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

        #region Storage Charge
        public void AddEditStorageCharge(StorageCharge ObjStorage)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjStorage.StorageChargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WarehouseType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.WarehouseType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.ChargeType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMeterPerMonth", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerMonth });

            LstParam.Add(new MySqlParameter { ParameterName = "in_RateMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateMeterPerDay });

            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerMonth", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerMonth });

            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStorage.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeFrom", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeTo", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.CommodityType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjStorage.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstStorageCharge", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstStorageCharge", CommandType.StoredProcedure, DParam);
            List<StorageCharge> LstStorageCharge = new List<StorageCharge>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStorageCharge.Add(new StorageCharge
                    {
                        StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]),
                        WarehouseTypeName = Convert.ToString(Result["WarehouseTypeName"]),
                        //ChargeTypeName = Convert.ToString(Result["ChargeTypeName"]),
                        RateMeterPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]),
                        RateSqMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]),
                        RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]),
                        RateCubMeterPerDay = Convert.ToDecimal(Result["RateCubMeterPerDay"]),
                        RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]),
                        RateCubMeterPerMonth = Convert.ToDecimal(Result["RateCubMeterPerMonth"]),
                        EffectiveDate = Convert.ToString(Result["EffectiveDate"]),
                        DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                        DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"])
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstStorageCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            StorageCharge ObjStorage = new StorageCharge();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStorage.StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]);
                    ObjStorage.RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                    ObjStorage.RateSqMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                    ObjStorage.RateMeterPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]);
                    ObjStorage.RateCubMeterPerDay = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
                    ObjStorage.RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]);
                    ObjStorage.RateCubMeterPerMonth = Convert.ToDecimal(Result["RateCubMeterPerMonth"]);
                    ObjStorage.EffectiveDate = Convert.ToString(Result["EffectiveDate"]);
                    ObjStorage.WarehouseType = Convert.ToInt32(Result["WarehouseType"]);
                    ObjStorage.ChargeType = Convert.ToInt32(Result["ChargeType"] == DBNull.Value ? 0 : Result["ChargeType"]);
                    ObjStorage.DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"] == DBNull.Value ? 0 : Result["DaysRangeFrom"]);
                    ObjStorage.DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"] == DBNull.Value ? 0 : Result["DaysRangeTo"]);
                    ObjStorage.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjStorage.CommodityType = Convert.ToInt32(Result["CommodityType"]);
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

        #region Ground Rent
        public void AddEditMstGroundRent(CWCChargesGroundRent objCR, int Uid)
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
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objCR.OperationType });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCR.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditMstGroundRent", CommandType.StoredProcedure, dpram);
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
            IDataReader result = DA.ExecuteDataReader("GetAllGroundRentDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CWCChargesGroundRent> objList = new List<CWCChargesGroundRent>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CWCChargesGroundRent
                    {
                        GroundRentId = Convert.ToInt32(result["GroundRentId"]),
                        ContainerType = Convert.ToInt32(result["ContainerType"]),
                        RentAmount = Convert.ToDecimal(result["RentAmount"]),
                        Size = result["Size"].ToString(),
                        DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]),
                        DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]),
                        OperationType = Convert.ToInt32(result["OperationType"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        CommodityType = Convert.ToInt32(result["CommodityType"])
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
            IDataReader result = DA.ExecuteDataReader("GetAllGroundRentDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CWCChargesGroundRent objGR = null;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objGR = new CWCChargesGroundRent();
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

        #region Weighment
        public void AddEditMstWeighment(CWCWeighment objCW, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeighmentId", MySqlDbType = MySqlDbType.Int32, Value = objCW.WeighmentId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerRate", MySqlDbType = MySqlDbType.Decimal, Value = objCW.ContainerRate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TruckRate", MySqlDbType = MySqlDbType.Decimal, Value = objCW.TruckRate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objCW.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCW.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCW.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dpram = lstParam.ToArray();
            int result = DA.ExecuteNonQuery("AddEditMstWeighment", CommandType.StoredProcedure, dpram);
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
            IDataReader result = DA.ExecuteDataReader("GetAllWeighmentDet", CommandType.StoredProcedure, Dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            IList<CWCWeighment> lstWeighment = null;
            CWCWeighment objCWC = null;
            try
            {
                if (WeighmentId == 0)
                {
                    lstWeighment = new List<CWCWeighment>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstWeighment.Add(new CWCWeighment
                        {
                            WeighmentId = Convert.ToInt32(result["WeighmentId"]),
                            ContainerRate = Convert.ToDecimal(result["ContainerRate"]),
                            ContainerSize = result["ContainerSize"].ToString(),
                            EffectiveDate = Convert.ToString(result["EffectiveDate"]),
                            SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString(),
                            TruckRate = Convert.ToDecimal(result["TruckRate"])
                        });
                    }
                }
                else
                {
                    objCWC = new CWCWeighment();
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
        public void AddEditMstEntryFees(CWCEntryFees objEF, int Uid)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstEntryFees", CommandType.StoredProcedure, dparam);
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
            IDataReader result = DA.ExecuteDataReader("GetMstEntryFees", CommandType.StoredProcedure, dparam);
            IList<CWCEntryFees> lstEntryFees = null;
            CWCEntryFees objEF = null;
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                if (EntryFeeId == 0)
                {
                    lstEntryFees = new List<CWCEntryFees>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstEntryFees.Add(new CWCEntryFees
                        {
                            EntryFeeId = Convert.ToInt32(result["EntryFeeId"]),
                            ContainerType = Convert.ToInt32(result["ContainerType"]),
                            CommodityType = Convert.ToInt32(result["CommodityType"]),
                            OperationType = Convert.ToInt32(result["OperationType"]),
                            Reefer = Convert.ToBoolean(result["Reefer"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            EffectiveDate = result["EffectiveDate"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString()
                        });
                    }
                }
                else
                {
                    objEF = new CWCEntryFees();
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

        #region Miscellaneous
        public void AddEditMiscellaneous(Miscellaneous ObjMiscellaneous)
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
            List<Miscellaneous> LstMiscellaneous = new List<Miscellaneous>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstMiscellaneous.Add(new Miscellaneous
                    {
                        MiscellaneousId = Convert.ToInt32(Result["MiscellaneousId"]),
                        Fumigation = Convert.ToDecimal(Result["Fumigation"]),
                        Washing = Convert.ToDecimal(Result["Washing"]),
                        Reworking = Convert.ToDecimal(Result["Reworking"]),
                        Bagging = Convert.ToDecimal(Result["Bagging"]),
                        Palletizing = Convert.ToDecimal(Result["Palletizing"]),
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
            Miscellaneous ObjMiscellaneous = new Miscellaneous();
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

        #region Insurance
        public void AddEditInsurance(Insurance ObjInsurance)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInsurance.InsuranceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Charge", MySqlDbType = MySqlDbType.Decimal, Value = ObjInsurance.Charge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjInsurance.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjInsurance.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjInsurance.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstInsurance", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstInsurance", CommandType.StoredProcedure, DParam);
            List<Insurance> LstInsurance = new List<Insurance>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInsurance.Add(new Insurance
                    {
                        InsuranceId = Convert.ToInt32(Result["InsuranceId"]),
                        Charge = Convert.ToDecimal(Result["Charge"]),
                        EffectiveDate = Convert.ToString(Result["EffectiveDate"]),
                        SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString()
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
            Insurance ObjInsurance = new Insurance();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstInsurance", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInsurance.InsuranceId = Convert.ToInt32(Result["InsuranceId"]);
                    ObjInsurance.Charge = Convert.ToDecimal(Result["Charge"]);
                    ObjInsurance.EffectiveDate = Convert.ToString(Result["EffectiveDate"]);
                    ObjInsurance.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
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

        #region Reefer
        public void AddEditMstReefer(CWCReefer objRef)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = objRef.ReeferChrgId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRef.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objRef.ElectricityCharge });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objRef.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objRef.ContainerSize });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditMstReefer", CommandType.StoredProcedure, dpram);
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
            IDataReader result = DA.ExecuteDataReader("ListOfMstReefer", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CWCReefer> objList = new List<CWCReefer>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CWCReefer
                    {
                        ReeferChrgId = Convert.ToInt32(result["ReeferChrgId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ElectricityCharge = Convert.ToDecimal(result["ElectricityCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString()
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
            IDataReader result = DA.ExecuteDataReader("ListOfMstReefer", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CWCReefer objRef = new CWCReefer();
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

        #region TDS
        public void AddEditMstTds(CWCTds objTds)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_TdsId", MySqlDbType = MySqlDbType.Int32, Value = objTds.TdsId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objTds.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_CWCTdsPrcnt", MySqlDbType = MySqlDbType.Decimal, Value = objTds.CWCTdsPrcnt });
            lstparam.Add(new MySqlParameter { ParameterName = "in_HTTdsPrcnt", MySqlDbType = MySqlDbType.Decimal, Value = objTds.HTTdsPrcnt });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objTds.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditMsttds", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1 ? "TDS Details Saved Successfully" : "TDS Details Updated Successfully");
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
        public void GetAllTDS()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_TdsId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ListOfMstTds", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<CWCTds> objList = new List<CWCTds>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new CWCTds
                    {
                        TdsId = Convert.ToInt32(result["TdsId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        CWCTdsPrcnt = Convert.ToDecimal(result["CWCTdsPrcnt"]),
                        HTTdsPrcnt = Convert.ToDecimal(result["HTTdsPrcnt"]),
                        SacCode = result["SacCode"].ToString()
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
        public void GetTDSDet(int TdsId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_TdsId", MySqlDbType = MySqlDbType.Int32, Value = TdsId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ListOfMstTds", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CWCTds objTds = new CWCTds();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objTds.TdsId = Convert.ToInt32(result["TdsId"]);
                    objTds.EffectiveDate = result["EffectiveDate"].ToString();
                    objTds.CWCTdsPrcnt = Convert.ToDecimal(result["CWCTdsPrcnt"]);
                    objTds.HTTdsPrcnt = Convert.ToDecimal(result["HTTdsPrcnt"]);
                    objTds.SacCode = result["SacCode"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objTds;
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
    }
}