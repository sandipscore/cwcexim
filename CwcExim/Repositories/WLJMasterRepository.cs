using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Master.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class WLJMasterRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }

        public void GetFranchiseCharge(int franchisechargeid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = franchisechargeid });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            WLJCWCFranchiseCharges objFcs = new WLJCWCFranchiseCharges();
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
        public void AddEditMstFranchiseCharges(WLJCWCFranchiseCharges objFC, int Uid)
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
            IList<WLJCWCFranchiseCharges> objList = new List<WLJCWCFranchiseCharges>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new WLJCWCFranchiseCharges
                    {
                        franchisechargeid = Convert.ToInt32(result["franchisechargeid"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        //Chargesfor = result["Chargesfor"].ToString(),
                        //ODC = Convert.ToBoolean(result["ODC"]),
                        RoaltyCharge = Convert.ToDecimal(result["RoaltyCharge"]),
                        FranchiseCharge = Convert.ToDecimal(result["FranchiseCharge"]),
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
    }
}