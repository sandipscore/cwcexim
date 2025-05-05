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
    public class SacRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void AddSac(SAC ObjSac)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Value = ObjSac.SACId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjSac.SACCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.VarChar, Value = ObjSac.Description });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Gst", MySqlDbType = MySqlDbType.Decimal, Value = ObjSac.GST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Cess", MySqlDbType = MySqlDbType.Decimal, Value = ObjSac.CESS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSac.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstSac", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "GST Against SAC Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "SAC Code Already Exist";
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
        public void GetAllSac()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSac", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
            List<SAC> LstSac = new List<SAC>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSac.Add(new SAC
                    {
                        SACId=Convert.ToInt32(Result["SacId"]),
                        SACCode = (Result["SACCode"]==null?"":Result["SACCode"]).ToString(),
                        GST = Convert.ToDecimal(Result["Gst"]==DBNull.Value?0:Result["Gst"]),
                        //CESS=Convert.ToDecimal(Result["CESS"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSac;
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
        public void GetSac(int SacId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Value = SacId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSac", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            SAC ObjSac = new SAC();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSac.SACId = Convert.ToInt32(Result["SacId"]);
                    ObjSac.SACCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjSac.Description = (Result["Description"] == null ? "" : Result["Description"]).ToString();
                    ObjSac.GST = Convert.ToDecimal(Result["Gst"]==DBNull.Value?0:Result["Gst"]);
                    //ObjSac.CESS = Convert.ToDecimal(Result["Cess"]==DBNull.Value?0:Result["Cess"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSac;
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
    }
}