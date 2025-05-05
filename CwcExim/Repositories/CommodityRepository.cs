using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CwcExim.Repositories
{
    public class CommodityRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void AddEditCommodity(Commodity ObjCommodity)
        {
            /* Commodity Type:1.HAZ 2.Non HAZ */
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCommodity.CommodityId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityName", MySqlDbType = MySqlDbType.VarChar, Value = ObjCommodity.CommodityName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = ObjCommodity.CommodityType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityAlias", MySqlDbType = MySqlDbType.VarChar, Value = (ObjCommodity.CommodityAlias==null?null:ObjCommodity.CommodityAlias.Trim()) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TaxExempted", MySqlDbType = MySqlDbType.Bit, Value = ObjCommodity.TaxExempted });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCommodity.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjCommodity.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32,Direction=ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstCommodity", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjCommodity.CommodityId == 0 ? "Commodity Details Saved Successfully" : "Commodity Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Commodity Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if(Result==3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Commodity Alias Already Exists";
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

        public void DeleteCommodity(int CommodityId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CommodityId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstCommodity", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Commodity Details Deleted Successfully";
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

        public void GetAllCommodity(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllCommodity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Commodity> LstCommodity = new List<Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Commodity{
                        CommodityId=Convert.ToInt32(Result["CommodityId"]),
                        CommodityName=Result["CommodityName"].ToString(),
                        CommodityAlias=(Result["CommodityAlias"]==null?"":Result["CommodityAlias"]).ToString()
                    });
                }
                if(Status==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCommodity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
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

        public void GetCommodity(int CommodityId)
        {
            int Status = 0;
            List <MySqlParameter> LstParam= new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CommodityId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Commodity ObjCommodity = new Commodity();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCommodity.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    ObjCommodity.CommodityName = Result["CommodityName"].ToString();
                    ObjCommodity.CommodityAlias = (Result["CommodityAlias"]==null?"":Result["CommodityAlias"]).ToString();
                    ObjCommodity.TaxExempted = Convert.ToBoolean(Result["TaxExempted"] == DBNull.Value ? 0 : Result["TaxExempted"]);                    
                    ObjCommodity.CommodityType = Convert.ToInt32(Result["CommodityType"] == DBNull.Value ? 0 : Result["CommodityType"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCommodity;
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