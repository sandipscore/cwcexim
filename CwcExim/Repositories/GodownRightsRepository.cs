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
    
    public class GodownRightsRepository
    {

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void AddEditGodownRights(string GodownRightsXML,int UId)
        {
            
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "GodownRightsXML", MySqlDbType = MySqlDbType.Text, Value = GodownRightsXML });
            LstParam.Add(new MySqlParameter { ParameterName = "UserId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Value = PartyType });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditGodownRights", CommandType.StoredProcedure, DParam, out Status);
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

        public void GetGodownRights(int UserId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
           // lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Value = PartyType });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownRights", CommandType.StoredProcedure, DParam);
            GodownRights ObjAccessRights = null;
            List<GodownRights> lstAccessRight = new List<GodownRights>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAccessRights = new GodownRights();
                    ObjAccessRights.SrlNo = Convert.ToInt32(Result["SrlNo"]);
                    ObjAccessRights.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjAccessRights.GodownName = Convert.ToString(Result["GodownName"]);
                    //ObjAccessRights.ModuleId = Convert.ToInt32(Result["ModuleId"]);
                    ObjAccessRights.IsAllowed = Convert.ToBoolean(Result["IsAllowed"] == DBNull.Value ? false : Result["IsAllowed"]);
                    ObjAccessRights.Godown = Convert.ToString(Result["Godown"]);
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
        public void GetAllUser(int RoleID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleID });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUser", CommandType.StoredProcedure, DParam);
            Models.User ObjUser = null;
            List<Models.User> lstUser = new List<Models.User>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUser = new Models.User();
                    ObjUser.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjUser.Name = Convert.ToString(Result["Name"]);
                    ObjUser.Password = Convert.ToString(Result["Password"]);
                    ObjUser.RoleId = Convert.ToInt32(Result["RoleId"] == DBNull.Value ? 0 : Result["RoleId"]);
                    //  ObjUser.DesignationId = Convert.ToInt32(Result["DesignationId"] == DBNull.Value ? 0 : Result["DesignationId"]);
                    //  ObjUser.Designation = Convert.ToString(Result["Designation"] == null ? "" : Result["Designation"]);
                    ObjUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);
                    ObjUser.PanNo = Convert.ToString(Result["PanNo"] == null ? "" : Result["PanNo"]);
                    ObjUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    //  ObjUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);
                    ObjUser.LicenseNo = Convert.ToString(Result["LicenseNo"]);
                    ObjUser.RoleName = Convert.ToString(Result["RoleName"]);
                    //  ObjUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                    // ObjUser.Designation = Convert.ToString(Result["Designation"]);
                    ObjUser.EmailVerified = Convert.ToBoolean(Result["EmailVerified"] == DBNull.Value ? false : Result["EmailVerified"]);
                    ObjUser.MobileVerified = Convert.ToBoolean(Result["MobileVerified"] == DBNull.Value ? false : Result["MobileVerified"]);
                    ObjUser.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjUser.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjUser.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjUser.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjUser.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjUser.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjUser.IsBlocked = Convert.ToBoolean(Result["IsBlocked"] == DBNull.Value ? false : Result["IsBlocked"]);
                    ObjUser.IsSelected = false;
                    lstUser.Add(ObjUser);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstUser;
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
        public void GetMenuWiseAccessRight(int menuid, int roleid, int moduleid, int branchid)
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