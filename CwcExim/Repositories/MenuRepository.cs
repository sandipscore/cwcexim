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
    public class MenuRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }


        public void GetMenu(int RoleId)
        {
           List<Menu> ObjMenuLst = null;
            Menu ObjMenu = null;

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "@RoleId", MySqlDbType = MySqlDbType.Int32,Size=4, Value = RoleId });
           
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMenues", CommandType.StoredProcedure, DParam);

            ObjMenuLst = new List<Menu>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjMenu = new Menu();
                    ObjMenu.MenuId = Convert.ToInt32(Result["MenuId"]);
                    ObjMenu.MenuName = Convert.ToString(Result["MenuName"]);
                    ObjMenu.ActionUrl = Convert.ToString(Result["ActionUrl"]);
                    ObjMenuLst.Add(ObjMenu);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjMenuLst;
                }
                else
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 2;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddEditMenu(Menu ObjMenu)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.MenuId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuName", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = ObjMenu.MenuName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ParentMenuId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.ParentMenuId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ActionUrl", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjMenu.ActionUrl });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DisplayPosition", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.DisplayPosition });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.CreatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjMenu.UpdatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction=ParameterDirection.Output,Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam= { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMenu", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    // _DBResponse.Message = "Success";
                    _DBResponse.Message = ObjMenu.MenuId == 0 ? "Menu Details saved successfully" : "Menu Details updated successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Menu Name already exists.";
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

        public void GetAllMenu()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMenu", CommandType.StoredProcedure, DParam);
            Menu ObjMenu = null;
            List<Menu> lstMenu = new List<Menu>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjMenu = new Menu();
                    ObjMenu.MenuId = Convert.ToInt32(Result["MenuId"]);
                    ObjMenu.MenuName = Convert.ToString(Result["MenuName"]);
                    ObjMenu.ParentMenuId = Convert.ToInt32(Result["ParentMenuId"]==DBNull.Value?0 : Result["ParentMenuId"]);
                    ObjMenu.ParentMenuName = Convert.ToString(Result["ParentMenuName"]);
                    ObjMenu.ModuleName = Convert.ToString(Result["ModuleName"]);
                    ObjMenu.ModuleId = Convert.ToInt32(Result["ModuleId"] == DBNull.Value ? 0 : Result["ModuleId"]);
                    ObjMenu.DisplayPosition = Convert.ToInt32(Result["DisplayPosition"]==DBNull.Value ? 0 : Result["DisplayPosition"]);
                    ObjMenu.ActionUrl = Convert.ToString(Result["ActionUrl"]);
                    ObjMenu.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjMenu.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjMenu.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjMenu.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjMenu.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjMenu.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    lstMenu.Add(ObjMenu);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstMenu;
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

        public void GetMenuByMenuID(int MenuID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MenuID });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMenu", CommandType.StoredProcedure, DParam);
            Menu ObjMenu = null;
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjMenu = new Menu();
                    ObjMenu.MenuId = Convert.ToInt32(Result["MenuId"]);
                    ObjMenu.MenuName = Convert.ToString(Result["MenuName"]);
                    ObjMenu.ParentMenuId = Convert.ToInt32(Result["ParentMenuId"] == DBNull.Value ? 0 : Result["ParentMenuId"]);
                    ObjMenu.ParentMenuName = Convert.ToString(Result["ParentMenuName"]);
                    ObjMenu.ModuleName = Convert.ToString(Result["ModuleName"]);
                    ObjMenu.ModuleId = Convert.ToInt32(Result["ModuleId"] == DBNull.Value ? 0 : Result["ModuleId"]);
                    ObjMenu.DisplayPosition = Convert.ToInt32(Result["DisplayPosition"] == DBNull.Value ? 0 : Result["DisplayPosition"]);
                    ObjMenu.ActionUrl = Convert.ToString(Result["ActionUrl"]);
                    ObjMenu.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjMenu.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjMenu.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjMenu.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjMenu.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjMenu.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjMenu;
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

        public void GetMenuByRole(int ModuleID, int RoleId,int PartyType)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ModuleID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = RoleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyType });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMenuByRole", CommandType.StoredProcedure, DParam);
            Menu ObjMenu = null;
            List<Menu> lstMenu = new List<Menu>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjMenu = new Menu();
                    ObjMenu.MenuId = Convert.ToInt32(Result["MenuId"]);
                    ObjMenu.MenuName = Convert.ToString(Result["MenuName"]);
                    ObjMenu.ActionUrl = Convert.ToString(Result["ActionUrl"] == null ? "" : Result["ActionUrl"]);
                    ObjMenu.ParentMenuId =Convert.ToInt32(Result["ParentMenuId"]==DBNull.Value?0:Result["ParentMenuId"]);
                    lstMenu.Add(ObjMenu);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstMenu;
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