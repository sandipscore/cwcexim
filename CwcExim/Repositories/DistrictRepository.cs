using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class DistrictRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void AddEditDistrict(DistrictBase ObjDistrict)
        {
            string Status = "0";
            int Result = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDistrict.DistrictId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictName", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjDistrict.DistrictName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ForKolkata", MySqlDbType = MySqlDbType.Bit, Value = ObjDistrict.ForKolkata });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDistrict.CreatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDistrict.UpdatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            if (ObjDistrict.ForKolkata == true)
            {
                DistrictForKol ObjDistForKol = (DistrictForKol)ObjDistrict;
                lstParam.Add(new MySqlParameter { ParameterName = "in_RegionCode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjDistForKol.RegionCode });
                lstParam.Add(new MySqlParameter { ParameterName = "in_PinCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjDistForKol.PinCode });
                lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDistForKol.BranchId });
                DParam = lstParam.ToArray();
                Result = DataAccess.ExecuteNonQuery("AddEditKolDistrict", CommandType.StoredProcedure, DParam, out Status);
            }
            else if(ObjDistrict.ForKolkata==false)
            {
                DistrictForNonKol ObjDistForNonKol = (DistrictForNonKol)ObjDistrict;
                lstParam.Add(new MySqlParameter { ParameterName = "in_CESCDistrictCode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjDistForNonKol.CESCDistrictCode });
                lstParam.Add(new MySqlParameter { ParameterName = "in_NonCESCDistrictCode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjDistForNonKol.NonCESCDistrictCode });
                lstParam.Add(new MySqlParameter { ParameterName = "in_DmOfficeAddress", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjDistForNonKol.DmOfficeAddress });
                lstParam.Add(new MySqlParameter { ParameterName = "in_DmOfficeEmail", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjDistForNonKol.DmOfficeEmail });
                lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDistForNonKol.BranchId });
                DParam = lstParam.ToArray();
                Result = DataAccess.ExecuteNonQuery("AddEditNonKolDistrict", CommandType.StoredProcedure, DParam, out Status);
            }

            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    // _DBResponse.Message = "Success";
                    _DBResponse.Message = ObjDistrict.DistrictId == 0 ? "District Details saved successfully" : "District Details updated successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Pin Code.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate District Name.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Duplicate CESC Dstrict Code.";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Duplicate Non CESC District Code.";
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

        public void GetAllDistrict(bool ForKolkata)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ForKolkata", MySqlDbType = MySqlDbType.Bit, Size = 11, Value = ForKolkata });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDistrict", CommandType.StoredProcedure, DParam);
            DistrictForKol ObjDistForKol = null;
            DistrictForNonKol ObjDistForNonKol = null;
            List<DistrictBase> lstDist = new List<DistrictBase>();
          
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                  
                    if (ForKolkata == true)
                    {
                        ObjDistForKol = new DistrictForKol();
                        ObjDistForKol.DistrictId = Convert.ToInt32(Result["DistrictId"]);
                        ObjDistForKol.DistrictName = Convert.ToString(Result["DistrictName"]);
                        ObjDistForKol.ForKolkata = Convert.ToBoolean(Result["ForKolkata"]);
                        ObjDistForKol.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                        ObjDistForKol.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                        ObjDistForKol.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                        ObjDistForKol.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                        ObjDistForKol.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                        ObjDistForKol.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                        ObjDistForKol.RegionCode = Convert.ToString(Result["RegionCode"] == null ? "" : Result["RegionCode"]);
                        ObjDistForKol.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                        ObjDistForKol.BranchId = Convert.ToInt32(Result["BranchId"] == DBNull.Value ? 0 : Result["BranchId"]);
                        ObjDistForKol.BranchName = Convert.ToString(Result["BranchName"] == null ? "" : Result["BranchName"]);
                        lstDist.Add(ObjDistForKol);
                    }
                    else
                    {
                        ObjDistForNonKol = new DistrictForNonKol();
                        ObjDistForNonKol.DistrictId = Convert.ToInt32(Result["DistrictId"]);
                        ObjDistForNonKol.DistrictName = Convert.ToString(Result["DistrictName"]);
                        ObjDistForNonKol.ForKolkata = Convert.ToBoolean(Result["ForKolkata"]);
                        ObjDistForNonKol.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                        ObjDistForNonKol.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                        ObjDistForNonKol.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                        ObjDistForNonKol.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                        ObjDistForNonKol.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                        ObjDistForNonKol.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                        ObjDistForNonKol.CESCDistrictCode = Convert.ToString(Result["CESCDistrictCode"]);
                        ObjDistForNonKol. NonCESCDistrictCode = Convert.ToString(Result["NonCESCDistrictCode"]);
                        ObjDistForNonKol.DmOfficeAddress = Convert.ToString(Result["DmOfficeAddress"] == null ? "" : Result["DmOfficeAddress"]);
                        ObjDistForNonKol.DmOfficeEmail = Convert.ToString(Result["DmOfficeEmail"] == null ? "" : Result["DmOfficeEmail"]);
                        ObjDistForNonKol.BranchId = Convert.ToInt32(Result["BranchId"] == DBNull.Value ? 0 : Result["BranchId"]);
                        ObjDistForNonKol.BranchName = Convert.ToString(Result["BranchName"] == null ? "" : Result["BranchName"]);
                        lstDist.Add(ObjDistForNonKol);
                    }
                   
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstDist;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetDistrict(int DistrictId,bool ForKolkata)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DistrictId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ForKolkata", MySqlDbType = MySqlDbType.Bit, Size = 11, Value = ForKolkata });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDistrict", CommandType.StoredProcedure, DParam);
            DistrictForKol ObjDistForKol = null;
            DistrictForNonKol ObjDistForNonKol = null;

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ForKolkata == true)
                    {
                        ObjDistForKol = new DistrictForKol();
                        ObjDistForKol.DistrictId = Convert.ToInt32(Result["DistrictId"]);
                        ObjDistForKol.DistrictName = Convert.ToString(Result["DistrictName"]);
                        ObjDistForKol.ForKolkata = Convert.ToBoolean(Result["ForKolkata"]);
                        ObjDistForKol.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                        ObjDistForKol.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                        ObjDistForKol.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                        ObjDistForKol.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                        ObjDistForKol.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                        ObjDistForKol.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                        ObjDistForKol.RegionCode = Convert.ToString(Result["RegionCode"] == null ? "" : Result["RegionCode"]);
                        ObjDistForKol.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                        ObjDistForKol.BranchId = Convert.ToInt32(Result["BranchId"] == DBNull.Value ? 0 : Result["BranchId"]);
                        ObjDistForKol.BranchName = Convert.ToString(Result["BranchName"] == null ? "" : Result["BranchName"]);
                    }
                    else
                    {
                        ObjDistForNonKol = new DistrictForNonKol();
                        ObjDistForNonKol.DistrictId = Convert.ToInt32(Result["DistrictId"]);
                        ObjDistForNonKol.DistrictName = Convert.ToString(Result["DistrictName"]);
                        ObjDistForNonKol.ForKolkata = Convert.ToBoolean(Result["ForKolkata"]);
                        ObjDistForNonKol.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                        ObjDistForNonKol.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                        ObjDistForNonKol.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                        ObjDistForNonKol.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                        ObjDistForNonKol.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                        ObjDistForNonKol.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                        ObjDistForNonKol.CESCDistrictCode = Convert.ToString(Result["CESCDistrictCode"]);
                        ObjDistForNonKol.NonCESCDistrictCode = Convert.ToString(Result["NonCESCDistrictCode"]);
                        ObjDistForNonKol.DmOfficeAddress = Convert.ToString(Result["DmOfficeAddress"] == null ? "" : Result["DmOfficeAddress"]);
                        ObjDistForNonKol.DmOfficeEmail = Convert.ToString(Result["DmOfficeEmail"] == null ? "" : Result["DmOfficeEmail"]);
                    }

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if(ForKolkata==true)
                    _DBResponse.Data = ObjDistForKol;
                    else
                     _DBResponse.Data = ObjDistForNonKol;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetDistrictAll()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDistrict", CommandType.StoredProcedure, DParam);
            District ObjDistrict = null;
            List<District> lstDistrict = new List<District>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDistrict = new District();
                    ObjDistrict.DistrictId = Convert.ToInt32(Result["DistrictId"]);
                    ObjDistrict.DistrictName = Convert.ToString(Result["DistrictName"] == DBNull.Value ? 0 : Result["DistrictName"]);                   
                    lstDistrict.Add(ObjDistrict);

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstDistrict;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


    }
}