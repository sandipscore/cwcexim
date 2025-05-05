using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.DAL;
using CwcExim.UtilityClasses;
using System.Data;
//using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using CwcExim.Areas.GateOperation.Models;

namespace CwcExim.Repositories
{
    public class UserRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void Login(string LoginId, string Password)
        {
            Login ObjLogin = null;
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType =MySqlDbType.VarChar, Size = 50, Value = LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = Password });
            DParam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("UserLogin", CommandType.StoredProcedure, DParam);
            //Utility.Encrypt(Password)
            ObjLogin = new Login();
            ObjLogin.Role = new Role();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    
                    ObjLogin.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjLogin.LoginId = Convert.ToString(Result["LoginId"]);
                   // ObjLogin.Password = Convert.ToString(Result["Password"]);
                    ObjLogin.Name= Convert.ToString(Result["Name"]);
                    ObjLogin.Role=new Role { RoleId = Convert.ToInt32(Result["RoleId"]), RoleName = Convert.ToString(Result["RoleName"]) };
                    ObjLogin.Locked = Convert.ToBoolean(Result["Locked"] == DBNull.Value ? false : Result["Locked"]);
                    ObjLogin.AttemptCount = Convert.ToInt32(Result["AttemptCount"] == DBNull.Value ? 0 : Result["AttemptCount"]);
                    ObjLogin.IsBlocked= Convert.ToBoolean(Result["IsBlocked"] == DBNull.Value ? false : Result["IsBlocked"]);
                    //ObjLogin.PartyType = Convert.ToInt32(Result["PartyType"] == DBNull.Value ? 0 : Result["PartyType"]);
                    ObjLogin.Importer = Convert.ToBoolean(Result["Importer"] == DBNull.Value ? 0 : Result["Importer"]);
                    ObjLogin.Exporter = Convert.ToBoolean(Result["Exporter"]==DBNull.Value?0:Result["Exporter"]);
                    ObjLogin.ShippingLine = Convert.ToBoolean(Result["ShippingLine"]==DBNull.Value?0:Result["ShippingLine"]);
                    ObjLogin.CHA = Convert.ToBoolean(Result["CHA"]==DBNull.Value?0:Result["CHA"]);
                    ObjLogin.EximTraderId = Convert.ToInt32(Result["EximTraderId"] == DBNull.Value ? 0 : Result["EximTraderId"]);
                    ObjLogin.FirstLogin= Convert.ToInt32(Result["CountLogin"] == DBNull.Value ? 0 : Result["CountLogin"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjLogin;

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

        // Create CwcExim Extenal User

        public void AddEditExtarnalUser(SecondaryUser ObjUser)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjUser.LoginId });
           lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjUser.Name });
            
             lstParam.Add(new MySqlParameter { ParameterName = "in_EximtraderId", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.EximTraderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjUser.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjUser.Email });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.CreatedBy });
           
           
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSecondaryUser", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjUser.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "User ID is already taken by another user. Please try another User ID";
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

        // Create CwcExim User
        public void AddEditUser(UserBase ObjUser)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size=50, Value = ObjUser.LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjUser.Name });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjUser.Password });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.RoleId });
           // lstParam.Add(new MySqlParameter { ParameterName = "in_DesignationId", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.DesignationId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjUser.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjUser.Email });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.CreatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.UpdatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsBlocked", MySqlDbType = MySqlDbType.Int32, Value = ObjUser.IsBlocked });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditUser", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjUser.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "User ID is already taken by another user. Please try another User ID";
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

        // Create USer With ID
        public void AddEditUserWithId(SignupWithId ObjSignUp)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSignUp.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjSignUp.LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjSignUp.Name });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjSignUp.Password });           
            lstParam.Add(new MySqlParameter { ParameterName = "in_PanNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSignUp.PanNo});            
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSignUp.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjSignUp.Email });           
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditUserWithId", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjSignUp.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "User ID is already taken by another user. Please try another User ID";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Pan No is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "User Already Exists.";
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

        //Create User WithOut ID
        public void AddEditUserWithoutId(SignUpWithoutId ObjSignUpWithoutId)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSignUpWithoutId.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjSignUpWithoutId.LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjSignUpWithoutId.Name });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjSignUpWithoutId.Password });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PanNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSignUpWithoutId.PanNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSignUpWithoutId.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjSignUpWithoutId.Email });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjSignUpWithoutId.Address });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictId", MySqlDbType = MySqlDbType.Int32, Value = ObjSignUpWithoutId.DistrictId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pincode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSignUpWithoutId.PinCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EmailVerified", MySqlDbType = MySqlDbType.Bit,Value = 1 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileVerified", MySqlDbType = MySqlDbType.Bit,Value = 1 });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditUserWithoutId", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjSignUpWithoutId.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "User ID is already taken by another user. Please try another User ID";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Pan No is already registered for another user.";
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

        //Create CwcExim Forced User 

        public void AddEditWbdedForcedUser(WbdedForcedUser ObjWbdedForcedUser)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjWbdedForcedUser.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjWbdedForcedUser.LoginId !=null? ObjWbdedForcedUser.LoginId.Trim():"" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjWbdedForcedUser.Name !=null? ObjWbdedForcedUser.Name.Trim():""});           
            lstParam.Add(new MySqlParameter { ParameterName = "in_RegistrationNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjWbdedForcedUser.RegistrationNo !=null? ObjWbdedForcedUser.RegistrationNo.Trim():""});
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjWbdedForcedUser.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjWbdedForcedUser.Email });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjWbdedForcedUser.Address !=null?ObjWbdedForcedUser.Address.Trim():"" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistrictId", MySqlDbType = MySqlDbType.Int32, Value = ObjWbdedForcedUser.DistrictId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pincode", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjWbdedForcedUser.PinCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjWbdedForcedUser.CreatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjWbdedForcedUser.UpdatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditWbdedForcedUser", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjWbdedForcedUser.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "User ID is already taken by another user. Please try another User ID";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Registration No. already exixts.";
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

        // Check User When Click on New user withid
        public void CheckUser(string LoginId)
        {
            //string Status = "0";
           /// Login ObjLogin = null;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Value = LoginId });
            //lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Object Result = DataAccess.ExecuteScalar("CheckUser",CommandType.StoredProcedure, DParam);                              
            _DBResponse = new DatabaseResponse();                              
            try
            {               
                if (Convert.ToInt32(Result) == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;

                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "User Id Does Not Exist.";
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

        // Fill SignUp with ID view
        public void FillSignupWithId(string Loginid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Value = Loginid });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetWbdedForcedUserForEdit", CommandType.StoredProcedure, DParam);            
            SignupWithId ObjSignupWithId = null;
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSignupWithId = new SignupWithId();                  
                    ObjSignupWithId.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjSignupWithId.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjSignupWithId.Name = Convert.ToString(Result["Name"]);                   
                    ObjSignupWithId.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjSignupWithId.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);                  
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSignupWithId;
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

        // Get CwcExim forced user list

        public void GetWbdedForcedUserList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetWbdedForcedUser", CommandType.StoredProcedure, DParam);
            WbdedForcedUser ObjWbdedForcedUser = null;
            List<WbdedForcedUser> lstWbdedForcedUser = new List<WbdedForcedUser>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjWbdedForcedUser = new WbdedForcedUser();
                    ObjWbdedForcedUser.GeneratedSerialNo = Convert.ToInt32(Result["GeneratedSerialNo"]);
                    ObjWbdedForcedUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjWbdedForcedUser.Name = Convert.ToString(Result["Name"]);
                    ObjWbdedForcedUser.RegistrationNo = Convert.ToString(Result["RegistrationNo"]==null ? "" : Result["RegistrationNo"]);
                    ObjWbdedForcedUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjWbdedForcedUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);                  
                    ObjWbdedForcedUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjWbdedForcedUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    ObjWbdedForcedUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);                  
                    ObjWbdedForcedUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                    lstWbdedForcedUser.Add(ObjWbdedForcedUser);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstWbdedForcedUser;
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

        // CwcExim Forced User For EDIT
        public void GetWbdedForcedUserForEdit(string Loginid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Value = Loginid });
           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetWbdedForcedUserForEdit", CommandType.StoredProcedure, DParam);
            WbdedForcedUser ObjWbdedForcedUser = null;            
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjWbdedForcedUser = new WbdedForcedUser();
                    //ObjWbdedForcedUser.GeneratedSerialNo = Convert.ToInt32(Result["GeneratedSerialNo"]);
                    ObjWbdedForcedUser.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjWbdedForcedUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjWbdedForcedUser.Name = Convert.ToString(Result["Name"]);
                    ObjWbdedForcedUser.RegistrationNo = Convert.ToString(Result["RegistrationNo"] == null ? "" : Result["RegistrationNo"]);
                    ObjWbdedForcedUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjWbdedForcedUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);
                    ObjWbdedForcedUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjWbdedForcedUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    ObjWbdedForcedUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);
                    ObjWbdedForcedUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                   
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjWbdedForcedUser;
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

        public void GetAllseconsaryUser(int UID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UID", MySqlDbType = MySqlDbType.Int32, Value = UID });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSecondaryUser", CommandType.StoredProcedure,DParam);
            Models.SecondaryUser ObjUser = null;
            List<Models.SecondaryUser> lstUser = new List<Models.SecondaryUser>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUser = new Models.SecondaryUser();
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
                    ObjUser.IsBlocked = Convert.ToBoolean(Result["IsBlocked"]);
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

        public void GetSecondaryUserEDIT(int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSecondaryUserEDIT", CommandType.StoredProcedure, DParam);
            Models.SecondaryUser ObjUser = null;
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUser = new Models.SecondaryUser();
                    ObjUser.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjUser.Name = Convert.ToString(Result["Name"]);
                    ObjUser.Password = Convert.ToString(Result["Password"]);
                    ObjUser.RoleId = Convert.ToInt32(Result["RoleId"] == DBNull.Value ? 0 : Result["RoleId"]);
                    //ObjUser.DesignationId = Convert.ToInt32(Result["DesignationId"] == DBNull.Value ? 0 : Result["DesignationId"]);
                    //ObjUser.Designation = Convert.ToString(Result["Designation"] == null ? "" : Result["Designation"]);
                    ObjUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);
                    ObjUser.PanNo = Convert.ToString(Result["PanNo"] == null ? "" : Result["PanNo"]);
                    ObjUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    //ObjUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);
                    ObjUser.LicenseNo = Convert.ToString(Result["LicenseNo"]);
                    ObjUser.RoleName = Convert.ToString(Result["RoleName"]);
                    //ObjUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                    //ObjUser.Designation = Convert.ToString(Result["Designation"]);
                    ObjUser.EmailVerified = Convert.ToBoolean(Result["EmailVerified"] == DBNull.Value ? false : Result["EmailVerified"]);
                    ObjUser.MobileVerified = Convert.ToBoolean(Result["MobileVerified"] == DBNull.Value ? false : Result["MobileVerified"]);
                    ObjUser.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjUser.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjUser.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjUser.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjUser.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjUser.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjUser.IsBlocked = Convert.ToBoolean(Result["IsBlocked"] == DBNull.Value ? false : Result["IsBlocked"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjUser;
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
                    ObjUser.IsBlocked= Convert.ToBoolean(Result["IsBlocked"]);
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
        public void GetUser(int Uid,int RoleID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleID });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUser", CommandType.StoredProcedure, DParam);
            Models.User ObjUser = null;
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
                    ObjUser.DesignationId = Convert.ToInt32(Result["DesignationId"] == DBNull.Value ? 0 : Result["DesignationId"]);
                    ObjUser.Designation = Convert.ToString(Result["Designation"] == null ? "" : Result["Designation"]);
                    ObjUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);
                    ObjUser.PanNo = Convert.ToString(Result["PanNo"] == null ? "" : Result["PanNo"]);
                    ObjUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    ObjUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);
                    ObjUser.LicenseNo = Convert.ToString(Result["LicenseNo"]);
                    ObjUser.RoleName = Convert.ToString(Result["RoleName"]);
                    ObjUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                    ObjUser.Designation = Convert.ToString(Result["Designation"]);
                    ObjUser.EmailVerified = Convert.ToBoolean(Result["EmailVerified"] == DBNull.Value ? false : Result["EmailVerified"]);
                    ObjUser.MobileVerified = Convert.ToBoolean(Result["MobileVerified"] == DBNull.Value ? false : Result["MobileVerified"]);
                    ObjUser.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjUser.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjUser.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjUser.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjUser.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjUser.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    //ObjUser.IsActive = Convert.ToBoolean(Result["IsActive"] == DBNull.Value ? false : Result["IsActive"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjUser;
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

        public void GetUser(int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = -1 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUser", CommandType.StoredProcedure, DParam);
            Models.EditWBDEDUser ObjUser = null;
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUser = new Models.EditWBDEDUser();
                    ObjUser.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjUser.Name = Convert.ToString(Result["Name"]);
                    ObjUser.Password = Convert.ToString(Result["Password"]);
                    ObjUser.RoleId = Convert.ToInt32(Result["RoleId"] == DBNull.Value ? 0 : Result["RoleId"]);
                    //ObjUser.DesignationId = Convert.ToInt32(Result["DesignationId"] == DBNull.Value ? 0 : Result["DesignationId"]);
                    //ObjUser.Designation = Convert.ToString(Result["Designation"] == null ? "" : Result["Designation"]);
                    ObjUser.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjUser.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);
                    ObjUser.PanNo = Convert.ToString(Result["PanNo"] == null ? "" : Result["PanNo"]);
                    ObjUser.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);
                    ObjUser.PinCode = Convert.ToString(Result["PinCode"] == null ? "" : Result["PinCode"]);
                    //ObjUser.DistrictId = Convert.ToInt32(Result["DistrictId"] == DBNull.Value ? 0 : Result["DistrictId"]);
                    ObjUser.LicenseNo = Convert.ToString(Result["LicenseNo"]);
                    ObjUser.RoleName = Convert.ToString(Result["RoleName"]);
                    //ObjUser.DistrictName = Convert.ToString(Result["DistrictName"]);
                    //ObjUser.Designation = Convert.ToString(Result["Designation"]);
                    ObjUser.EmailVerified = Convert.ToBoolean(Result["EmailVerified"] == DBNull.Value ? false : Result["EmailVerified"]);
                    ObjUser.MobileVerified = Convert.ToBoolean(Result["MobileVerified"] == DBNull.Value ? false : Result["MobileVerified"]);
                    ObjUser.CreatedByName = Convert.ToString(Result["CreatedByName"]);
                    ObjUser.CreatedBy = Convert.ToInt32(Result["CreatedBy"] == DBNull.Value ? 0 : Result["CreatedBy"]);
                    ObjUser.CreatedOn = Convert.ToString(Result["CreatedOn"] == null ? "" : Result["CreatedOn"]);
                    ObjUser.UpdatedByName = Convert.ToString(Result["UpdatedByName"]);
                    ObjUser.UpdatedBy = Convert.ToInt32(Result["UpdatedBy"] == DBNull.Value ? 0 : Result["UpdatedBy"]);
                    ObjUser.UpdatedOn = Convert.ToString(Result["UpdatedOn"] == null ? "" : Result["UpdatedOn"]);
                    ObjUser.IsBlocked = Convert.ToBoolean(Result["IsBlocked"] == DBNull.Value ? false : Result["IsBlocked"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjUser;
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

        public void SaveLoginAuditTrail(LoginAuditTrail ObjLoginAuditTrail)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrailId", MySqlDbType = MySqlDbType.Int32, Value = ObjLoginAuditTrail.TrailId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjLoginAuditTrail.LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SessionId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjLoginAuditTrail.SessionId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.Int32, Value = ObjLoginAuditTrail.Status });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IPAddress", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjLoginAuditTrail.IPAddress });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("SaveLoginAuditTrail", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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
        public void SaveAccountLock(AccountLock ObjAccountLock)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LockId", MySqlDbType = MySqlDbType.Int32, Value = ObjAccountLock.LockId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjAccountLock.LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SessionId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjAccountLock.SessionId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IPAddress", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjAccountLock.IPAddress });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LockMinutes", MySqlDbType = MySqlDbType.Int32, Value = ObjAccountLock.LockMinutes });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("SaveAccountLock", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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

        public void SaveUserFailureAttempt(string LoginId)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoginId", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = LoginId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("SaveUserFailureAttempt", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "You have only 2 Attempts left. After that your account will be locked";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    _DBResponse.Status = -2;
                    _DBResponse.Message = "You have only 1 Attempt left. After that your account will be locked";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    _DBResponse.Status = -3;
                    _DBResponse.Message = "Your account is temporarily locked.";
                    _DBResponse.Data = null;
                }
                else if (Result == -4)
                {
                    _DBResponse.Status = -4;
                    _DBResponse.Message = "Your account is temporarily locked now. Come back later.";
                    _DBResponse.Data = null;
                }
                else if(Result==-6)
                {
                    _DBResponse.Status = -6;
                    _DBResponse.Message = "";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "";
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

        public void GetUserForDistAssignment(int RoleID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleID });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUser", CommandType.StoredProcedure, DParam);
            Models.UserForDistAssignment ObjUser = null;
            List<Models.UserForDistAssignment> lstUser = new List<Models.UserForDistAssignment>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUser = new Models.UserForDistAssignment();
                    ObjUser.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjUser.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjUser.Name = Convert.ToString(Result["Name"]);
                    ObjUser.Designation = Convert.ToString(Result["Designation"] == null ? "" : Result["Designation"]);
                    ObjUser.RoleName = Convert.ToString(Result["RoleName"]);
                    ObjUser.Designation = Convert.ToString(Result["Designation"]);
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

        // Forgot Password
        public void ForogtPassword(string Param,string Choice, string NewPassword)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_param", MySqlDbType = MySqlDbType.VarChar, Value = Param });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Choice", MySqlDbType = MySqlDbType.VarChar, Value = Choice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Value = NewPassword });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            string Status = "0";
            string ReturnObj = "";
         
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("ForgotPassword", CommandType.StoredProcedure, DParam, out Status, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ReturnObj;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Invalid Credentials";
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

        //Change Password
        public void ChangePassword(ChangePassword ObjChangePassword)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjChangePassword.Uid });          
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewPassword", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjChangePassword.NewPassword });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldPassword", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjChangePassword.OldPassword });          
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //int Result = DataAccess.ExecuteNonQuery("ChangePassword", CommandType.StoredProcedure, DParam, out Status);
            int Result = DataAccess.ExecuteNonQuery("ChangePassword", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Password Changed Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Invalid Old Password.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Password Used in 90 days Can't allowed.";
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

        // Edit Profile
        public void GetUserDetail(int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUserDetail", CommandType.StoredProcedure, DParam);
            EditProfile ObjEditProfile = null;           
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEditProfile = new EditProfile();
                    ObjEditProfile.Uid = Convert.ToInt32(Result["Uid"]);
                    ObjEditProfile.LoginId = Convert.ToString(Result["LoginId"]);
                    ObjEditProfile.Name = Convert.ToString(Result["Name"]);                                                         
                    ObjEditProfile.MobileNo = Convert.ToString(Result["MobileNo"] == null ? "" : Result["MobileNo"]);
                    ObjEditProfile.Email = Convert.ToString(Result["Email"] == null ? "" : Result["Email"]);                  
                    ObjEditProfile.Address = Convert.ToString(Result["Address"] == null ? "" : Result["Address"]);                                      
                   
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEditProfile;
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

        // Update USer Profile
        public void UpdateUserProfile(EditProfile ObjEditProfile)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEditProfile.Uid });          
            lstParam.Add(new MySqlParameter { ParameterName = "in_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEditProfile.Name });           
            lstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjEditProfile.MobileNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjEditProfile.Email });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = ObjEditProfile.Address });         
            lstParam.Add(new MySqlParameter { ParameterName = "in_UpdatedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjEditProfile.UpdatedBy });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateUserProfile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjEditProfile.Uid == 0 ? "User Created Successfully" : "User Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Mobile Number is already registered for another user.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Email Address is already registered for another user.";
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

        // Deleting User
        public void DeleteUser(int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            int Result=DataAccess.ExecuteNonQuery("DeleteUser", CommandType.StoredProcedure,DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "User Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }


        }

        public void UnlockUser(string UserName)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
           
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserName", MySqlDbType = MySqlDbType.VarChar, Value = UserName });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            string Status = "0";
            string ReturnObj = "";

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UnlockUser", CommandType.StoredProcedure, DParam, out Status, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Status;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Invalid Credentials";
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

        #region User id status
        public void GetUserIdList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("MasterUserId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<MasterUserIdList> Lstuserid = new List<MasterUserIdList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstuserid.Add(new MasterUserIdList
                    {
                        Uid = Convert.ToString(Result["Uid"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstuserid;
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

        public void UpdateUserId(UserStatus ObjUpdateUserID, int Uid)
        {
            //    DateTime GatePassDate = DateTime.ParseExact(ObjExitThroughGateHeader.GatePassDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //  DateTime GatePassDate = Convert.ToDateTime(ObjExitThroughGateHeader.GatePassDate).ToString("yyyy-MM-dd");

            //  var Exitdt = DateTime.ParseExact(ObjExitThroughGateHeader.GateExitDateTime, "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
            //if(ObjExitThroughGateHeader.GatePassId=="")
            //{
            //    ObjExitThroughGateHeader.GatePassId = ""
            //}

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = ObjUpdateUserID.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateUserIdstatus", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "User status Saved Successfully" : "User status Updated Successfully";
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

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetAllUserIdstatus()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("MasterUserId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<UserStatus> Lstuserid = new List<UserStatus>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstuserid.Add(new UserStatus
                    {
                        Uid = Convert.ToString(Result["Uid"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstuserid;
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

    }
}