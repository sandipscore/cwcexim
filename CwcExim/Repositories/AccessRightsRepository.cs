using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.DAL;
using CwcExim.UtilityClasses;
using System.Data;
using MySql.Data.MySqlClient;

namespace CwcExim.Repositories
{
    public class AccessRightsRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }

        //public void AddEditAccessRights(string AccessRightsXML,int ModuleId, int RoleId, int Uid)
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "AccessRightsXML", MySqlDbType = MySqlDbType.Text, Value = AccessRightsXML });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Text, Value = ModuleId });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Text, Value = RoleId });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Text, Value = Uid });
        //    lstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
        //    lstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
        //    IDataParameter[] DParam = { };
        //    DParam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DataAccess.ExecuteNonQuery("AddEditAccessRights", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = null;
        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //}

        public void AddEditAccessRights(string AccessRightsXML, int ModuleId, int RoleId,/*int PartyType,*/ int Uid)
        {
            
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "AccessRightsXML", MySqlDbType = MySqlDbType.Text, Value = AccessRightsXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Text, Value = ModuleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Text, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Text, Value = Uid });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Value = PartyType });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditAccessRights", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else if (Result == 0)
                {
                    DBResponse.Status = 0;
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

        public void GetAccessRights(int ModuleId, int RoleId /*,int PartyType*/)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32,Value = ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Value = PartyType });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAccessRights", CommandType.StoredProcedure, DParam);
            AccessRights ObjAccessRights = null;
            List<AccessRights> lstAccessRight = new List<AccessRights>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAccessRights = new AccessRights();
                    ObjAccessRights.SrlNo = Convert.ToInt32(Result["SrlNo"]);
                    ObjAccessRights.MenuId = Convert.ToInt32(Result["MenuId"]);
                    ObjAccessRights.MenuName = Convert.ToString(Result["MenuName"]);
                    //ObjAccessRights.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjAccessRights.CanAdd = Convert.ToBoolean(Result["CanAdd"] == DBNull.Value ? false : Result["CanAdd"]);
                    ObjAccessRights.CanView = Convert.ToBoolean(Result["CanView"] == DBNull.Value ? false : Result["CanView"]);
                    ObjAccessRights.CanEdit = Convert.ToBoolean(Result["CanEdit"] == DBNull.Value ? false : Result["CanEdit"]);
                    ObjAccessRights.CanDelete = Convert.ToBoolean(Result["CanDelete"]==DBNull.Value?false:Result["CanDelete"]);
                    lstAccessRight.Add(ObjAccessRights);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAccessRight;
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

        public void GetMenuWiseAccessRight(int menuid, int roleid, int moduleid,int branchid)
        {
            int Status = 0;


            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Value = menuid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = roleid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Value = moduleid }); 
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = branchid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMenuWiseAccessRight", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            dynamic objRights = new { CanAdd = 0, CanEdit = 0, CanDelete = 0, CanView = 0 };

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objRights = new
                    {
                        CanAdd = Convert.ToInt32(Result["CanAdd"]),
                        CanEdit = Convert.ToInt32(Result["CanEdit"]),
                        CanDelete = Convert.ToInt32(Result["CanDelete"]),
                        CanView = Convert.ToInt32(Result["CanView"])
                    };

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objRights;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = objRights;
                }

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = objRights;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
    }
}