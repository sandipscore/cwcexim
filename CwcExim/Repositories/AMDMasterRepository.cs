using CwcExim.Areas.Master.Models;
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
    public class AMDMasterRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }

        #region Franchise Charge
        public void GetFranchiseCharge(int franchisechargeid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = franchisechargeid });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            AMDFranchiseCharges objFcs = new AMDFranchiseCharges();
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
                    objFcs.ContainerRangeFrom = Convert.ToInt32(result["ContainerCountFrom"]);
                    objFcs.ContainerRangeTo = Convert.ToInt32(result["ContainerCountTo"]);

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
        public void AddEditMstFranchiseCharges(AMDFranchiseCharges objFC, int Uid)
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerCountFrom", MySqlDbType = MySqlDbType.Int32, Value = objFC.ContainerRangeFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerCountTo", MySqlDbType = MySqlDbType.Int32, Value = objFC.ContainerRangeTo });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstFranchise", CommandType.StoredProcedure, dparam);
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
            IDataReader result = DA.ExecuteDataReader("GetMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<AMDFranchiseCharges> objList = new List<AMDFranchiseCharges>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new AMDFranchiseCharges
                    {
                        franchisechargeid = Convert.ToInt32(result["franchisechargeid"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        //Chargesfor = result["Chargesfor"].ToString(),
                        //ODC = Convert.ToBoolean(result["ODC"]),
                        RoaltyCharge = Convert.ToDecimal(result["RoaltyCharge"]),
                        FranchiseCharge = Convert.ToDecimal(result["FranchiseCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        ContainerRangeFrom =Convert.ToInt32(result["ContainerCountFrom"]),
                        ContainerRangeTo = Convert.ToInt32(result["ContainerCountTo"]),
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
        #endregion

        #region Reefer
        public void AddEditMstReefer(AMDReefer objRef)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = objRef.ReeferChrgId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRef.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objRef.ElectricityCharge });
            lstparam.Add(new MySqlParameter { ParameterName = "in_MonitoringCharge", MySqlDbType = MySqlDbType.Decimal, Value = objRef.MonitoringCharge });
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
            IList<AMDReefer> objList = new List<AMDReefer>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new AMDReefer
                    {
                        ReeferChrgId = Convert.ToInt32(result["ReeferChrgId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ElectricityCharge = Convert.ToDecimal(result["ElectricityCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        MonitoringCharge=Convert.ToDecimal(result["MonitoringCharge"]),
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
            AMDReefer objRef = new AMDReefer();
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
                    objRef.MonitoringCharge = Convert.ToDecimal(result["MonitoringCharge"]);
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

        #region Party Wise HT Charges
        public void AddEditHTCharges(HTChargesPtyWise objHT, int Uid, String ChargeListXML)
        {
            
            string id = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = objHT.HTChargesId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = objHT.OperationId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = objHT.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = objHT.Type });            
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = objHT.PartyName });

            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstHTChargesPtyWise", CommandType.StoredProcedure, DParam, out id);
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

            HTChargesPtyWise Obj = new HTChargesPtyWise();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    Obj.LstWeightSlab.Add(new WeightSlabPtyWise
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
                        Obj.LstDistanceSlab.Add(new DistanceSlabPtyWise
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
            DataSet result = DA.ExecuteDataSet("GetAllMstHTChargesPtyWise", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            List<ChargeListPtyWise> LstCharge = new List<ChargeListPtyWise>();
            int Status = 0;
            try
            {
                foreach (DataRow dr in result.Tables[1].Rows)
                {
                    Status = 1;
                    LstCharge.Add(new ChargeListPtyWise
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
                        PortName = Convert.ToString(dr["PortName"]),
                        CustomExam = Convert.ToString(dr["CustomExam"]),
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

        public void GetAllHTChargesPtyWise()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTChargesPtyWise", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            HTChargesPtyWise lstCharges = new HTChargesPtyWise();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCharges.LstviewList.Add(new ViewListPtyWise
                    {
                        OperationDesc = result["OperationDesc"].ToString(),
                        HTChargesId = Convert.ToInt32(result["HTChargesID"]),
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        OperationCode = result["OperationCode"].ToString(),
                        RateCWC = Convert.ToDecimal(result["RateCWC"]),
                        ChargesFor = result["ChargesFor"].ToString(),
                    });
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        lstCharges.LstWeightSlab.Add(new WeightSlabPtyWise
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
                        lstCharges.LstDistanceSlab.Add(new DistanceSlabPtyWise
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
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTChargesPtyWise", CommandType.StoredProcedure, dparm);
            HTChargesPtyWise objHt = new HTChargesPtyWise();
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
                    objHt.PartyName = result["PartyName"].ToString();
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

        public void GetPartyNameForHTCharges(int Page, string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.String, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyNameForCWCCharges", CommandType.StoredProcedure, DParam);
            List<Areas.CashManagement.Models.PartyDet> LstPartyDetails = new List<Areas.CashManagement.Models.PartyDet>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePayer = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyDetails.Add(new Areas.CashManagement.Models.PartyDet
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePayer = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstPartyDetails, StatePayer };
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
        #endregion
    }
}