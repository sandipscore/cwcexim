using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Areas.Master.Models;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System.Data;
using CwcExim.Areas.CashManagement.Models;
using System.Web.Mvc;

namespace CwcExim.Repositories
{
    public class DSR_MasterRepository
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


        #region Exim Trader Master
        public void AddEditEximTrader(DSREximTrader ObjEximTrader)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEximTrader.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderName", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.EximTraderName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.EximTraderAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Password });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Importer", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Importer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Exporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHA", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.CHA });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Forwarder", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Forwarder });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Franchise", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Franchise });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BillToPay", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.BillToPay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Transporter", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Transporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.BilltoParty });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Bidder", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Bidder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Rent", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Rent });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.StateId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.CityId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PinCode", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.PinCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PhoneNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.PhoneNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FaxNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.FaxNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContactPerson", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.ContactPerson });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.MobileNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pan", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Pan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AadhaarNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.AadhaarNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tan", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Tan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstEximTrader", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjEximTrader.EximTraderId == 0 ? "Exim Trader Details Saved Successfully" : "Exim Trader Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Exim Trader Name Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Exim Trader Alias Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Duplicate Email Id";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Duplicate PAN";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Duplicate Aadhaar No.";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Duplicate GST No.";
                    _DBResponse.Data = null;
                }
                else if (Result == 8)
                {
                    _DBResponse.Status = 8;
                    _DBResponse.Message = "Duplicate Phone No.";
                    _DBResponse.Data = null;
                }
                else if (Result == 9)
                {
                    _DBResponse.Status = 9;
                    _DBResponse.Message = "Duplicate Mobile No.";
                    _DBResponse.Data = null;
                }
                else if (Result == 10)
                {
                    _DBResponse.Status = 10;
                    _DBResponse.Message = "Duplicate Fax No.";
                    _DBResponse.Data = null;
                }
                else if (Result == 11)
                {
                    _DBResponse.Status = 11;
                    _DBResponse.Message = "Duplicate TAN";
                    _DBResponse.Data = null;
                }
                else if (Result == 12)
                {
                    _DBResponse.Status = 12;
                    _DBResponse.Message = "User Id Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 13)
                {
                    _DBResponse.Status = 12;
                    _DBResponse.Message = "User Id Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 14)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "State Code and GST State Code is not matched";
                    _DBResponse.Data = null;
                }
                else if (Result == 15)
                {
                    _DBResponse.Status = 15;
                    _DBResponse.Message = "PinCode Doesn't belongs to the Selected State ";
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
        public void DeleteEximTrader(int EximTraderId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstEximTrader", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Exim Trader Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Exim Trader Finance Control";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Pda Opening";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Export-Carting Application";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Another Page";
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
        public void GetEximTrader(int EximTraderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EximTraderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTrader", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSREximTrader ObjEximTrader = new DSREximTrader();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEximTrader.EximTraderId = Convert.ToInt32(Result["EximTraderId"]);
                    ObjEximTrader.EximTraderName = Result["EximTraderName"].ToString();
                    ObjEximTrader.EximTraderAlias = (Result["EximTraderAlias"] == null ? "" : Result["EximTraderAlias"]).ToString();
                    ObjEximTrader.UserId = (Result["UserId"] == null ? "" : Result["UserId"]).ToString();
                    ObjEximTrader.Uid = Convert.ToInt32(Result["Uid"] == DBNull.Value ? 0 : Result["Uid"]);
                    ObjEximTrader.Password = (Result["Password"] == null ? "" : Result["Password"]).ToString();
                    ObjEximTrader.Importer = Convert.ToBoolean(Result["Importer"] == DBNull.Value ? 0 : Result["Importer"]);
                    ObjEximTrader.Exporter = Convert.ToBoolean(Result["Exporter"] == DBNull.Value ? 0 : Result["Exporter"]);
                    ObjEximTrader.ShippingLine = Convert.ToBoolean(Result["ShippingLine"] == DBNull.Value ? 0 : Result["ShippingLine"]);
                    ObjEximTrader.CHA = Convert.ToBoolean(Result["CHA"] == DBNull.Value ? 0 : Result["CHA"]);
                    ObjEximTrader.Forwarder = Convert.ToBoolean(Result["Forwarder"] == DBNull.Value ? 0 : Result["Forwarder"]);
                    ObjEximTrader.Rent = Convert.ToBoolean(Result["Rent"] == DBNull.Value ? 0 : Result["Rent"]);
                  
                    // ObjEximTrader.Franchise = Convert.ToBoolean(Result["Franchise"] == DBNull.Value ? 0 : Result["Franchise"]);



                    ObjEximTrader.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjEximTrader.CountryId = Convert.ToInt32(Result["CountryId"] == DBNull.Value ? 0 : Result["CountryId"]);
                    ObjEximTrader.StateId = Convert.ToInt32(Result["StateId"] == DBNull.Value ? 0 : Result["StateId"]);
                    ObjEximTrader.CityId = Convert.ToInt32(Result["CityId"] == DBNull.Value ? 0 : Result["CityId"]);
                    if (Result["PinCode"] == DBNull.Value)
                    {
                        ObjEximTrader.PinCode = null;
                    }
                    else
                    {
                        ObjEximTrader.PinCode = Convert.ToInt32(Result["PinCode"]);
                    }
                    // ObjEximTrader.PinCode = Convert.ToInt32(Result["PinCode"]==DBNull.Value? null : Result["PinCode"]);
                    ObjEximTrader.PhoneNo = (Result["PhoneNo"] == null ? "" : Result["PhoneNo"]).ToString();
                    ObjEximTrader.FaxNo = (Result["FaxNo"] == null ? "" : Result["FaxNo"]).ToString();
                    ObjEximTrader.Email = (Result["Email"] == null ? "" : Result["Email"]).ToString();
                    ObjEximTrader.ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString();
                    ObjEximTrader.MobileNo = (Result["MobileNo"] == null ? "" : Result["MobileNo"]).ToString();
                    ObjEximTrader.Pan = (Result["Pan"] == null ? "" : Result["Pan"]).ToString();
                    ObjEximTrader.AadhaarNo = (Result["AadhaarNo"] == null ? "" : Result["AadhaarNo"]).ToString();
                    ObjEximTrader.GSTNo = (Result["GSTNo"] == null ? "" : Result["GSTNo"]).ToString();
                    ObjEximTrader.Tan = (Result["Tan"] == null ? "" : Result["Tan"]).ToString();
                    ObjEximTrader.CountryName = (Result["CountryName"] == null ? "" : Result["CountryName"]).ToString();
                    ObjEximTrader.StateName = (Result["StateName"] == null ? "" : Result["StateName"]).ToString();
                    ObjEximTrader.CityName = (Result["CityName"] == null ? "" : Result["CityName"]).ToString();
                    // Two new field added on 28.05.2019
                    ObjEximTrader.Transporter = Convert.ToBoolean(Result["Transporter"] == DBNull.Value ? 0 : Result["Transporter"]);
                    ObjEximTrader.BilltoParty = Convert.ToBoolean(Result["BilltoParty"] == DBNull.Value ? 0 : Result["BilltoParty"]);
                    ObjEximTrader.Bidder = Convert.ToBoolean(Result["Bidder"] == DBNull.Value ? 0 : Result["Bidder"]);


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEximTrader;
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


        public void GetBidderTrader(int EximTraderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EximTraderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBidder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRBidder ObjEximTrader = new DSRBidder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEximTrader.BidderId = Convert.ToInt32(Result["BidderId"]);
                    ObjEximTrader.BidderName = Result["BidderName"].ToString();
                    ObjEximTrader.BidderAlias = (Result["BidderAlias"] == null ? "" : Result["BidderAlias"]).ToString();
                    ObjEximTrader.UserId = (Result["UserId"] == null ? "" : Result["UserId"]).ToString();
                    ObjEximTrader.Password = (Result["Password"] == null ? "" : Result["Password"]).ToString();
                    // ObjEximTrader.Importer = Convert.ToBoolean(Result["Importer"] == DBNull.Value ? 0 : Result["Importer"]);
                    // ObjEximTrader.Exporter = Convert.ToBoolean(Result["Exporter"] == DBNull.Value ? 0 : Result["Exporter"]);
                    ////ObjEximTrader.ShippingLine = Convert.ToBoolean(Result["ShippingLine"] == DBNull.Value ? 0 : Result["ShippingLine"]);
                    // ObjEximTrader.CHA = Convert.ToBoolean(Result["CHA"] == DBNull.Value ? 0 : Result["CHA"]);
                    ObjEximTrader.Forwarder = Convert.ToBoolean(Result["Forwarder"] == DBNull.Value ? 0 : Result["Forwarder"]);
                    // ObjEximTrader.Rent = Convert.ToBoolean(Result["Rent"] == DBNull.Value ? 0 : Result["Rent"]);
                    ObjEximTrader.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjEximTrader.CountryId = Convert.ToInt32(Result["CountryId"] == DBNull.Value ? 0 : Result["CountryId"]);
                    ObjEximTrader.StateId = Convert.ToInt32(Result["StateId"] == DBNull.Value ? 0 : Result["StateId"]);
                    ObjEximTrader.CityId = Convert.ToInt32(Result["CityId"] == DBNull.Value ? 0 : Result["CityId"]);
                    if (Result["PinCode"] == DBNull.Value)
                    {
                        ObjEximTrader.PinCode = null;
                    }
                    else
                    {
                        ObjEximTrader.PinCode = Convert.ToInt32(Result["PinCode"]);
                    }
                    // ObjEximTrader.PinCode = Convert.ToInt32(Result["PinCode"]==DBNull.Value? null : Result["PinCode"]);
                    ObjEximTrader.PhoneNo = (Result["PhoneNo"] == null ? "" : Result["PhoneNo"]).ToString();
                    ObjEximTrader.FaxNo = (Result["FaxNo"] == null ? "" : Result["FaxNo"]).ToString();
                    ObjEximTrader.Email = (Result["Email"] == null ? "" : Result["Email"]).ToString();
                    ObjEximTrader.ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString();
                    ObjEximTrader.MobileNo = (Result["MobileNo"] == null ? "" : Result["MobileNo"]).ToString();
                    ObjEximTrader.Pan = (Result["Pan"] == null ? "" : Result["Pan"]).ToString();
                    ObjEximTrader.AadhaarNo = (Result["AadhaarNo"] == null ? "" : Result["AadhaarNo"]).ToString();
                    ObjEximTrader.GSTNo = (Result["GSTNo"] == null ? "" : Result["GSTNo"]).ToString();
                    ObjEximTrader.Tan = (Result["Tan"] == null ? "" : Result["Tan"]).ToString();
                    ObjEximTrader.CountryName = (Result["CountryName"] == null ? "" : Result["CountryName"]).ToString();
                    ObjEximTrader.StateName = (Result["StateName"] == null ? "" : Result["StateName"]).ToString();
                    ObjEximTrader.CityName = (Result["CityName"] == null ? "" : Result["CityName"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEximTrader;
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
        public void GetAllEximTrader()
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTrader", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREximTrader> LstEximTrader = new List<DSREximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                    
                    LstEximTrader.Add(new DSREximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                       
                        Email = string.Join("\n", EmailSplit),
                        
                        ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString(),
                        Type = Result["Type"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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

        public void GetGetAllEximTraderPartyCode(string PartyCode)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderPartyCode", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREximTrader> LstEximTrader = new List<DSREximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                   
                    LstEximTrader.Add(new DSREximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                       
                        Email = string.Join("\n", EmailSplit),
                        // Email = (Result["Email"] == null ? "" : Result["Email"]).ToString(),
                        ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString(),
                        Type = Result["Type"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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



        public void GetGetAllPortByPortData(string PData)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PData", MySqlDbType = MySqlDbType.VarChar, Value = PData });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPortByPortData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRPort> LstPort = new List<DSRPort>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstPort.Add(new DSRPort
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


        public void GetAllEximTraderListPageWise(int Page)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEximTraderListPageWise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREximTrader> LstEximTrader = new List<DSREximTrader>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                   
                    LstEximTrader.Add(new DSREximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        // Email=string.Join("\r\n ", EmailSplit),
                        Email = string.Join("\n", EmailSplit),
                        // Email = (Result["Email"] == null ? "" : Result["Email"]).ToString(),
                        ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString(),
                        Type = Result["Type"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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

        #region SD Opening
        public void GetEximTrader(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstPDA", CommandType.StoredProcedure, Dparam);
            DSRSearchEximTraderData objSD = new DSRSearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new DSRListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objSD.State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void SearchByPartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstPDA", CommandType.StoredProcedure, Dparam);
            DSRSearchEximTraderData objSD = new DSRSearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new DSRListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void AddSDOpening(DSRSDOpening ObjSD, string xml)
        {
            //string GeneratedClientId = "0";
            string Param = "0", ReturnObj = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSD.SDId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSD.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FolioNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjSD.FolioNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjSD.Date) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjSD.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjSD.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSD.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });            
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = ObjSD.SDId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSDOpening", CommandType.StoredProcedure, DParam, out Param, out ReturnObj);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "SD Opening Details Saved Successfully";
                    //_DBResponse.Status = 1;
                    //_DBResponse.Message = "SD Opening Details Saved Successfully";
                    // _DBResponse.Data = GeneratedClientId;
                }
                /*else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "SD Opening Details Already Exist";
                    _DBResponse.Data = null;
                }*/
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Folio No Already Exist";
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

        public void GetAllSDOpening()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSDOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRSDOpening> LstPDAOpening = new List<DSRSDOpening>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDAOpening.Add(new DSRSDOpening
                    {
                        SDId = Convert.ToInt32(Result["SDId"]),
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        FolioNo = (Result["FolioNo"] == null ? "" : Result["FolioNo"]).ToString(),
                        Date = Convert.ToString(Result["Date"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Convert.ToString(Result["EximTraderAlias"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDAOpening;
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

        public void GetSDListPartyCode(string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSDOpeningByParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRSDOpening> LstPDAOpening = new List<DSRSDOpening>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDAOpening.Add(new DSRSDOpening
                    {
                        SDId = Convert.ToInt32(Result["SDId"]),
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        FolioNo = (Result["FolioNo"] == null ? "" : Result["FolioNo"]).ToString(),
                        Date = Convert.ToString(Result["Date"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Convert.ToString(Result["EximTraderAlias"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDAOpening;
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

        public void GetSDOpening(int SDID)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SDID });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSDOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRSDOpening LstSDOpening = new DSRSDOpening();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstSDOpening.SDId = Convert.ToInt32(Result["SDId"]);
                    LstSDOpening.EximTraderId = Convert.ToInt32(Result["EximTraderId"]);
                    LstSDOpening.FolioNo = (Result["FolioNo"] == null ? "" : Result["FolioNo"]).ToString();
                    LstSDOpening.Date = Convert.ToString(Result["Date"]);
                    LstSDOpening.Amount = Convert.ToDecimal(Result["Amount"]);
                    LstSDOpening.EximTraderName = Convert.ToString(Result["EximTraderName"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSDOpening;
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

        #region On Account Opening
        public void OAGetEximTrader(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstOnAc", CommandType.StoredProcedure, Dparam);
            DSRSearchEximTraderData objSD = new DSRSearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new DSRListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objSD.State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void OASearchByPartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstOnAc", CommandType.StoredProcedure, Dparam);
            DSRSearchEximTraderData objSD = new DSRSearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new DSRListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void AddOAOpening(DSROAOpening ObjSD, string xml)
        {
            string ReturnObj="";
            string Param = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnAcId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSD.OnAcId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSD.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FolioNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjSD.FolioNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjSD.Date) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjSD.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSD.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = ObjSD.OnAcId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOAOpening", CommandType.StoredProcedure, DParam, out Param, out ReturnObj);

            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "On Account Opening Details Saved Successfully";
 
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "On Account Opening Details Already Exist";
                    _DBResponse.Data = null;
                }
                //else if (Result == 3)
                //{
                //    _DBResponse.Status = 3;
                //    _DBResponse.Message = "Folio No Already Exist";
                //    _DBResponse.Data = null;
                //}
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

        public void GetAllOAOpening()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnAcId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstOAOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSROAOpening> LstPDAOpening = new List<DSROAOpening>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDAOpening.Add(new DSROAOpening
                    {
                        OnAcId = Convert.ToInt32(Result["OnAcId"]),
                        EximTraderId = Convert.ToInt32(Result["PartyId"]),
                        FolioNo = (Result["ReceiptNo"] == null ? "" : Result["ReceiptNo"]).ToString(),
                        Date = Convert.ToString(Result["ReceiptDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Convert.ToString(Result["EximTraderAlias"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDAOpening;
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

        public void GetOAListPartyCode(string PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstOAOpeningByParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSROAOpening> LstPDAOpening = new List<DSROAOpening>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPDAOpening.Add(new DSROAOpening
                    {
                        OnAcId = Convert.ToInt32(Result["OnAcId"]),
                        EximTraderId = Convert.ToInt32(Result["PartyId"]),
                        FolioNo = (Result["ReceiptNo"] == null ? "" : Result["ReceiptNo"]).ToString(),
                        Date = Convert.ToString(Result["ReceiptDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Convert.ToString(Result["EximTraderAlias"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPDAOpening;
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

        public void GetOAOpening(int OnAcId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnAcId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = OnAcId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstOAOpening", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSROAOpening LstSDOpening = new DSROAOpening();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstSDOpening.OnAcId = Convert.ToInt32(Result["OnAcId"]);
                    LstSDOpening.EximTraderId = Convert.ToInt32(Result["PartyId"]);
                    LstSDOpening.FolioNo = (Result["ReceiptNo"] == null ? "" : Result["ReceiptNo"]).ToString();
                    LstSDOpening.Date = Convert.ToString(Result["ReceiptDate"]);
                    LstSDOpening.Amount = Convert.ToDecimal(Result["Amount"]);
                    LstSDOpening.EximTraderName = Convert.ToString(Result["EximTraderName"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSDOpening;
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

        #region PORT
        public void AddEditPort(DSRPort ObjPort)
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
            List<DSRPort> LstPort = new List<DSRPort>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new DSRPort
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
            DSRPort ObjPort = new DSRPort();
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
            DSRPost ObjPort = new DSRPost();
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

        #region Bank/Cash
        public void GetAllBank()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBank", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRBank> LstBank = new List<DSRBank>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBank.Add(new DSRBank
                    {
                        AccountNo = (Result["AccountNo"] == null ? "" : Result["AccountNo"]).ToString(),
                        BankId = Convert.ToInt32(Result["BankId"]),
                        LedgerName = (Result["BankName"] == null ? "" : Result["BankName"]).ToString(),
                        Email = Result["Email"].ToString(),
                        IFSC= Result["IFSC"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBank;
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
        public void GetBank(int BankId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BankId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBank", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRBank ObjBank = new DSRBank();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBank.BankId = Convert.ToInt32(Result["BankId"]);
                    ObjBank.LedgerName = (Result["BankName"] == null ? "" : Result["BankName"]).ToString();
                    ObjBank.LedgerNo = Result["LedgerNo"] == null ? "" : Result["LedgerNo"].ToString();
                    ObjBank.AccountNo = (Result["AccountNo"] == null ? "" : Result["AccountNo"]).ToString();
                    ObjBank.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjBank.IFSC = (Result["IFSC"] == null ? "" : Result["IFSC"]).ToString();
                    ObjBank.Branch = (Result["Branch"] == null ? "" : Result["Branch"]).ToString();
                    ObjBank.MobileNo = (Result["MobileNo"] == null ? "" : Result["MobileNo"]).ToString();
                    ObjBank.FaxNo = (Result["FaxNo"] == null ? "" : Result["FaxNo"]).ToString();
                    ObjBank.Email = (Result["Email"] == null ? "" : Result["Email"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBank;
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
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddBank(DSRBank ObjBank)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjBank.BankId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankName", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.LedgerName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LedgerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.LedgerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AccountNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.AccountNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IFSC", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.IFSC });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Branch });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MobileNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.MobileNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FaxNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.FaxNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Email", MySqlDbType = MySqlDbType.VarChar, Value = ObjBank.Email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjBank.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjBank.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMstBank", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Account Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                //else if (Result == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Bank And Branch Combination Already Exist";
                //    _DBResponse.Data = null;
                //}
                //else if (Result == 4)
                //{
                //    _DBResponse.Status = 4;
                //    _DBResponse.Message = "Duplicate Fax No.";
                //    _DBResponse.Data = null;
                //}
                //else if (Result == 5)
                //{
                //    _DBResponse.Status = 5;
                //    _DBResponse.Message = "Duplicate Email Id";
                //    _DBResponse.Data = null;
                //}
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Account No.";
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

        public void GetLedger()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLedger", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<DSRLedgerNameModel> lstBankLedger = new List<DSRLedgerNameModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBankLedger.Add(new DSRLedgerNameModel()
                    {
                        BankId = Convert.ToInt32(Result["BankId"]),
                        LedgerId = Convert.ToInt32(Result["BankId"]),
                        LedgerNo=Convert.ToString(Result["LedgerNo"]),
                        LedgerNm = Convert.ToString(Result["LedgerName"]),
                        AccountNo = Convert.ToString(Result["AccountNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        Ifsc = Convert.ToString(Result["IFSC"]),
                        Branch = Convert.ToString(Result["Branch"]),
                        Mobile = Convert.ToString(Result["Mobile"]),
                        Fax = Convert.ToString(Result["Fax"]),
                        Email = Convert.ToString(Result["EmailId"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBankLedger;
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

        #region Rail Freight
        public void AddEditMstRailFreight(DSRRailFreight objEF, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_RailFreightId", MySqlDbType = MySqlDbType.Int32, Value = objEF.RailFreightId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.OperationType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Port", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.Port });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Location_Id", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objEF.LocationId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_From_Metric", MySqlDbType = MySqlDbType.Decimal, Size = 2, Value = objEF.FromMetric });
            lstParam.Add(new MySqlParameter { ParameterName = "in_to_Metric", MySqlDbType = MySqlDbType.Decimal, Size = 2, Value = objEF.ToMetric });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objEF.Reefer) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Rate", MySqlDbType = MySqlDbType.Decimal, Value = objEF.Rate });
            //    lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objEF.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objEF.ContainerSize });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objEF.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditRailFreightFees", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Rail Freight Fees Saved Successfully" : "Rail Freight Fees Updated Successfully";
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
        public void GetAllRailFreight(int RailFreightId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_RailFreightId", MySqlDbType = MySqlDbType.Int32, Value = RailFreightId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstRailFreightFees", CommandType.StoredProcedure, dparam);
            IList<DSRRailFreight> lstRailFreightFees = null;
            DSRRailFreight objEF = null;
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                if (RailFreightId == 0)
                {
                    lstRailFreightFees = new List<DSRRailFreight>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstRailFreightFees.Add(new DSRRailFreight
                        {
                            RailFreightId = Convert.ToInt32(result["RailFreightId"]),
                            ContainerType = Convert.ToInt32(result["ContainerType"]),
                            CommodityType = Convert.ToInt32(result["CommodityType"]),
                            OperationType = Convert.ToInt32(result["OperationType"]),
                            Reefer = Convert.ToBoolean(result["Reefer"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Port = Convert.ToInt32(result["Port"]),
                            ContainerSize = result["ContainerSize"].ToString(),
                            LocationId = Convert.ToInt32(result["LocationId"]),
                            FromMetric = Convert.ToDecimal(result["FromMetric"]),
                            ToMetric = Convert.ToDecimal(result["ToMetric"]),
                        });

                    }
                }
                else
                {
                    objEF = new DSRRailFreight();
                    while (result.Read())
                    {
                        Status = 2;
                        objEF.RailFreightId = Convert.ToInt32(result["RailFreightId"]);
                        objEF.ContainerType = Convert.ToInt32(result["ContainerType"]);
                        objEF.CommodityType = Convert.ToInt32(result["CommodityType"]);
                        objEF.OperationType = Convert.ToInt32(result["OperationType"]);
                        objEF.Reefer = Convert.ToBoolean(result["Reefer"]);
                        objEF.Rate = Convert.ToDecimal(result["Rate"]);

                        objEF.ContainerSize = result["ContainerSize"].ToString();
                        objEF.Port = Convert.ToInt32(result["Port"]);
                        objEF.LocationId = Convert.ToInt32(result["LocationId"]);
                        objEF.FromMetric = Convert.ToDecimal(result["FromMetric"]);
                        objEF.ToMetric = Convert.ToDecimal(result["ToMetric"]);
                    }


                }
                if (Status == 1 || Status == 2)
                {
                    if (Status == 1) _DBResponse.Data = lstRailFreightFees;
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

        #region Gst Against SAC
        public void AddSac(DSRSac ObjSac)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Value = ObjSac.SACId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjSac.SACCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.VarChar, Value = ObjSac.Description });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Gst", MySqlDbType = MySqlDbType.Decimal, Value = ObjSac.GST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cess", MySqlDbType = MySqlDbType.Decimal, Value = ObjSac.CESS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSac.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjSac.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstSac", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "GST Against SAC Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstSac", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRSac> LstSac = new List<DSRSac>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSac.Add(new DSRSac
                    {
                        SACId = Convert.ToInt32(Result["SacId"]),
                        SACCode = (Result["SACCode"] == null ? "" : Result["SACCode"]).ToString(),
                        GST = Convert.ToDecimal(Result["Gst"] == DBNull.Value ? 0 : Result["Gst"]),
                        CESS = Convert.ToDecimal(Result["CESS"] == DBNull.Value ? 0 : Result["CESS"])
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
            DSRSac ObjSac = new DSRSac();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSac.SACId = Convert.ToInt32(Result["SacId"]);
                    ObjSac.SACCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjSac.Description = (Result["Description"] == null ? "" : Result["Description"]).ToString();
                    ObjSac.GST = Convert.ToDecimal(Result["Gst"] == DBNull.Value ? 0 : Result["Gst"]);
                    ObjSac.CESS = Convert.ToDecimal(Result["Cess"] == DBNull.Value ? 0 : Result["Cess"]);
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

        #region Location
        public void GetAllLocation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstLocation", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRLocation> LstLocation = new List<DSRLocation>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstLocation.Add(new DSRLocation
                    {
                        LocationName = Result["LocationName"].ToString(),
                        LocationAlias = (Result["LocationAlias"] == null ? "" : Result["LocationAlias"]).ToString(),
                        LocationId = Convert.ToInt32(Result["LocationId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstLocation;
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

        public void GetLocation(int LocationId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LocationId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstLocation", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRLocation ObjLocation = new DSRLocation();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjLocation.LocationName = Result["LocationName"].ToString();
                    ObjLocation.LocationId = Convert.ToInt32(Result["LocationId"]);
                    ObjLocation.LocationAlias = (Result["LocationAlias"] == null ? "" : Result["LocationAlias"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjLocation;
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

        public void AddEditLocation(DSRLocation ObjLocation)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjLocation.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationName", MySqlDbType = MySqlDbType.VarChar, Value = ObjLocation.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjLocation.LocationAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjLocation.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjLocation.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstLocation", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjLocation.LocationId == 0 ? "Location Details Saved Successfully" : "Location Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Location Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "LOcation Alias Already Exists";
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

        public void DeleteLocation(int LocationId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstLocation", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Location Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Location Details Does Not Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Location Details As It Exist In Rail Freight Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Delete Location Details As It Exist In Rail Freight";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Port Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Contractor Master";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Cannot Delete Country Details As It Exist In Exim Trader Master";
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
        #endregion

        #region HT Charges
        public void AddEditHTCharges(DSRHTCharges objHT, int Uid, String ChargeListXML)
        {
            /*
            Container Type: 1.Empty Container 2.Loaded Container 3.Cargo 4.RMS
            Type: 1.General 2.Heavy
            Product Type: 1.HAZ 2.Non HAZ
            */
            string id = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = objHT.HTChargesId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = objHT.OperationId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = objHT.ContainerType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = objHT.Type });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objHT.Description });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.Int32, Value = objHT.Size });
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
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = "0" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeListXML", MySqlDbType = MySqlDbType.Text, Value = ChargeListXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SlabType", MySqlDbType = MySqlDbType.Int32, Value = objHT.SlabType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_WeightSlab", MySqlDbType = MySqlDbType.Int32, Value = objHT.WeightSlab });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DistanceSlab", MySqlDbType = MySqlDbType.Int32, Value = objHT.DistanceSlab });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CbmSlab", MySqlDbType = MySqlDbType.Int32, Value = objHT.CbmSlab });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsODC", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objHT.IsODC == true ? 1 : 0)});
            lstParam.Add(new MySqlParameter { ParameterName = "in_AdditionalCharge", MySqlDbType = MySqlDbType.Decimal, Value = objHT.AdditionalCharge });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditMstHTCharges", CommandType.StoredProcedure, DParam, out id);
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

            DSRHTCharges Obj = new DSRHTCharges();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    Obj.LstWeightSlab.Add(new DSRWeightSlab
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
                        Obj.LstDistanceSlab.Add(new DSRDistanceSlab
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
            DataSet result = DA.ExecuteDataSet("GetAllMstHTCharges", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            List<DSRChargeList> LstCharge = new List<DSRChargeList>();
            int Status = 0;
            try
            {
                foreach (DataRow dr in result.Tables[1].Rows)
                {
                    Status = 1;
                    LstCharge.Add(new DSRChargeList
                    {
                        WtSlabId = Convert.ToInt32(dr["WtSlabId"].ToString()),
                        FromWtSlabCharge = Convert.ToInt32(dr["FromWtSlabCharge"].ToString()),
                        ToWtSlabCharge = Convert.ToInt32(dr["ToWtSlabCharge"]),
                        DisSlabId = Convert.ToInt32(dr["DisSlabId"].ToString()),
                        FromDisSlabCharge = Convert.ToInt32(dr["FromDisSlabCharge"].ToString()),
                        ToDisSlabCharge = Convert.ToInt32(dr["ToDisSlabCharge"]),
                        FromCbmSlabCharge = Convert.ToInt32(dr["FromCbmSlabCharge"].ToString()),
                        ToCbmSlabCharge = Convert.ToInt32(dr["ToCbmSlabCharge"]),
                        CwcRate = Convert.ToDecimal(dr["RateCwc"]),
                        ContractorRate = Convert.ToDecimal(dr["ContractorRate"]),
                        RoundTripRate = Convert.ToDecimal(dr["RoundTripRate"]),
                        EmptyRate = Convert.ToDecimal(dr["EmptyRate"]),
                        SlabType = Convert.ToInt32(dr["SlabType"]),
                        WeightSlab = Convert.ToInt32(dr["WeightSlab"]),
                        DistanceSlab = Convert.ToInt32(dr["DistanceSlab"]),
                        CbmSlab = Convert.ToInt32(dr["CbmSlab"]),
                        AddlWtCharges = Convert.ToDecimal(dr["AddlWtCharges"]),
                        AddlDisCharges = Convert.ToDecimal(dr["AddlDisCharges"]),
                        AddlCbmCharges = Convert.ToDecimal(dr["AddlCbmCharges"]),
                        PortId = Convert.ToInt32(dr["PortId"]),
                        PortName = Convert.ToString(dr["PortName"]),

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

        public void GetAllHTCharges()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_HTChargesID", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTCharges", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            DSRHTCharges lstCharges = new DSRHTCharges();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCharges.LstviewList.Add(new DSRViewList
                    {
                        OperationDesc = result["OperationDesc"].ToString(),
                        HTChargesId = Convert.ToInt32(result["HTChargesID"]),
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        OperationCode = result["OperationCode"].ToString(),
                        RateCWC = Convert.ToDecimal(result["RateCWC"]),
                        ChargesFor = result["ChargesFor"].ToString(),
                        ContainerSize = result["size"].ToString(),
                        CommodityType = result["CommodityType"].ToString(),
                        ContainerType = result["ContainerType"].ToString(),
                    });
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        lstCharges.LstWeightSlab.Add(new DSRWeightSlab
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
                        lstCharges.LstDistanceSlab.Add(new DSRDistanceSlab
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
            IDataReader result = DA.ExecuteDataReader("GetAllMstHTCharges", CommandType.StoredProcedure, dparm);
            DSRHTCharges objHt = new DSRHTCharges();
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
                    objHt.Size = Convert.ToInt32(result["Size"] == DBNull.Value ? 0 : result["Size"]);
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
                    objHt.CbmSlab = Convert.ToInt32(result["CbmSlab"]);
                    objHt.ChargesFor = Convert.ToString(result["ChargesFor"]);
                    objHt.IsODC = Convert.ToBoolean(result["IsODC"]);// Convert.ToInt32(result[""]);
                    objHt.AdditionalCharge = Convert.ToDecimal(result["AdditionalCharge"]);
                   
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
                //result.Dispose();
                //result.Close();
            }
        }
        #endregion

        #region Miscellaneous
        public void AddEditMiscellaneous(DSRMiscellaneous ObjMiscellaneous)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PrintingCharges", MySqlDbType = MySqlDbType.Decimal, Value = ObjMiscellaneous.PrintingCharges });
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
                else if (Result == 7)
                {
                    _DBResponse.Status = 7;
                    _DBResponse.Message = "Printing Charges Already Exist";
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
            List<DSRMiscellaneous> LstMiscellaneous = new List<DSRMiscellaneous>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstMiscellaneous.Add(new DSRMiscellaneous
                    {
                        MiscellaneousId = Convert.ToInt32(Result["MiscellaneousId"]),
                        Fumigation = Convert.ToDecimal(Result["Fumigation"]),
                        Washing = Convert.ToDecimal(Result["Washing"]),
                        Reworking = Convert.ToDecimal(Result["Reworking"]),
                        Bagging = Convert.ToDecimal(Result["Bagging"]),
                        Palletizing = Convert.ToDecimal(Result["Palletizing"]),
                        PrintingCharges = Convert.ToDecimal(Result["PrintingCharges"]),
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
            DSRMiscellaneous ObjMiscellaneous = new DSRMiscellaneous();
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
                    ObjMiscellaneous.PrintingCharges = Convert.ToDecimal(Result["PrintingCharges"]);
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

        #region Storage Charge
        public void AddEditStorageCharge(DSRCWCStorageCharge ObjStorage)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageChargeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjStorage.StorageChargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WarehouseType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.WarehouseType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.ChargeType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerWeek });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RsrvRateSqMeterPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = "0.00" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerDay", MySqlDbType = MySqlDbType.Decimal, Value ="0" });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCubMeterPerWeek", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCubMeterPerWeek });//Grid/Week
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSqMeterPerMonth", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSqMeterPerMonth });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjStorage.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStorage.EffectiveDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeFrom", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DaysRangeTo", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.DaysRangeTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.CommodityType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjStorage.ContainerLoadType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageType", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = ObjStorage.StorageType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaType", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = ObjStorage.AreaType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = ObjStorage.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStorage.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            //newly add

            LstParam.Add(new MySqlParameter { ParameterName = "in_RateTsaPerBe", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateTsaPerBe });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateMtPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateMtPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateCbmPerDay", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateCbmPerDay });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RateSurCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.RateSurcharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SurCrgDuration", MySqlDbType = MySqlDbType.Int32,Size=4, Value = Convert.ToInt32(ObjStorage.SurchargeDuration) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ResvType", MySqlDbType = MySqlDbType.Int32, Size=1, Value = Convert.ToInt32(ObjStorage.ReservationType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsRmGarments", MySqlDbType = MySqlDbType.Int32, Size=1, Value =Convert.ToInt32(ObjStorage.IsReadyMadeGarments) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOdc", MySqlDbType = MySqlDbType.Int32, Size=1, Value = Convert.ToInt32(ObjStorage.IsOdc) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsHighSecSpace", MySqlDbType = MySqlDbType.Int32, Size=1, Value =Convert.ToInt32(ObjStorage.IsHighSecuredSpace)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsAirConSpace", MySqlDbType = MySqlDbType.Int32, Size=1, Value =Convert.ToInt32(ObjStorage.IsAirConditionSpace)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 2, Value = Convert.ToInt32(ObjStorage.Size) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AdditionalCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStorage.AdditionalCharge });


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
            List<DSRCWCStorageCharge> LstStorageCharge = new List<DSRCWCStorageCharge>();
            _DBResponse = new DatabaseResponse();



            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStorageCharge.Add(new DSRCWCStorageCharge
                    {
                        StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]),
                        WarehouseTypeName = Convert.ToString(Result["WarehouseTypeName"]),
                        //ChargeTypeName = Convert.ToString(Result["ChargeTypeName"]),
                        //RateMeterPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]),
                        //RateSqMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]),
                        RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]),
                        //RateCubMeterPerDay = Convert.ToDecimal(Result["RateCubMeterPerDay"]),
                        RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]),
                        RateSqMeterPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]),
                        EffectiveDate = (Result["EffectiveDate"]).ToString(),
                        DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                        Size =Convert.ToString(Result["Size"]),
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
            DSRCWCStorageCharge ObjStorage = new DSRCWCStorageCharge();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStorage.StorageChargeId = Convert.ToInt32(Result["StorageChargeId"]);
                    ObjStorage.RateSqMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                    ObjStorage.RateSqMeterPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                   // ObjStorage.RateMeterPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]);
                    ObjStorage.RateSqMPerDay = Convert.ToDecimal(Result["RateMeterPerDay"]);
                    ObjStorage.RateCubMeterPerWeek = Convert.ToDecimal(Result["RateCubMeterPerWeek"]); //Grid/Week
                    ObjStorage.RateTsaPerBe = Convert.ToDecimal(Result["RateTsaPerBe"]);
                    ObjStorage.RateMtPerDay = Convert.ToDecimal(Result["RateMtPerDay"]);
                    ObjStorage.RateCbmPerDay = Convert.ToDecimal(Result["RateCbmPerDay"]);
                    ObjStorage.ContainerLoadType = (Result["ContainerLoadType"]).ToString();
                    ObjStorage.StorageType = (Result["StorageType"]).ToString();
                    ObjStorage.AreaType = (Result["AreaType"]).ToString();
                    ObjStorage.EffectiveDate = (Result["EffectiveDate"]).ToString();
                    ObjStorage.WarehouseType = Convert.ToInt32(Result["WarehouseType"]);
                    ObjStorage.ChargeType = Convert.ToInt32(Result["ChargeType"] == DBNull.Value ? 0 : Result["ChargeType"]);
                    ObjStorage.DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"] == DBNull.Value ? 0 : Result["DaysRangeFrom"]);
                    ObjStorage.DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"] == DBNull.Value ? 0 : Result["DaysRangeTo"]);
                    ObjStorage.SacCode = (Result["SacCode"] == null ? "" : Result["SacCode"]).ToString();
                    ObjStorage.CommodityType = Convert.ToInt32(Result["CommodityType"]);
                    ObjStorage.Size = (Result["Size"]).ToString();
                    ObjStorage.ReservationType= Convert.ToInt32(Result["ResvType"]);
                    ObjStorage.IsReadyMadeGarments = Convert.ToBoolean(Result["IsRmGarments"]);
                    ObjStorage.IsOdc = Convert.ToBoolean(Result["IsOdc"]);
                    ObjStorage.IsAirConditionSpace = Convert.ToBoolean(Result["IsAirConSpace"]);
                    ObjStorage.IsHighSecuredSpace = Convert.ToBoolean(Result["IsHighSecSpace"]);

                    ObjStorage.RateSurcharge = Convert.ToDecimal(Result["SurCharge"]);
                    ObjStorage.SurchargeDuration = Convert.ToInt32(Result["SurCrgDuration"]);
                    ObjStorage.AdditionalCharge= Convert.ToDecimal(Result["AdditionalCharge"]);
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

        #region Yard
        public void AddEditYard(DSRYardVM ObjYard, string LocationXML, string DelLocationXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjYard.MstYard.YardId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardName", MySqlDbType = MySqlDbType.VarChar, Value = ObjYard.MstYard.YardName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjYard.MstYard.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DelLocationXML", MySqlDbType = MySqlDbType.Text, Value = DelLocationXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstYard", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjYard.MstYard.YardId == 0 ? "Yard Details Saved Successfully" : "Yard Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Yard Name Already Exist";
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
        public void DeleteYard(int YardId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = YardId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstYard", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Yard Details Deleted Successfully";
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
        public void GetYard(int YardId)
        {
            int Status = 0;
            List<MySqlParameter> LstParm = new List<MySqlParameter>();
            LstParm.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = YardId });
            IDataParameter[] DParam = { };
            DParam = LstParm.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRYardVM ObjYard = new DSRYardVM();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjYard.MstYard.YardId = Convert.ToInt32(Result["YardId"]);
                    ObjYard.MstYard.YardName = Result["YardName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjYard.LstYard.Add(new DSRYardWiseLocation
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            YardId = Convert.ToInt32(Result["YardId"]),
                            LocationName = Result["LocationName"].ToString(),
                            Row = Result["Row"].ToString(),
                            Column = Result["Column"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjYard;
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
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllYard()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRYard> LstYard = new List<DSRYard>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstYard.Add(new DSRYard
                    {
                        YardId = Convert.ToInt32(Result["YardId"]),
                        YardName = Result["YardName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstYard;
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

        #region Operation
        public void AddMstOperation(DSROperation objOper)
        {
            string id = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objOper.Type });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationCode", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objOper.Code });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.Int32, Value = objOper.SacId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationSDesc", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objOper.ShortDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OperationDesc", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objOper.Description });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objOper.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dpram = lstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddMstOperation", CommandType.StoredProcedure, dpram, out id);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = id;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Operation Details Saved Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Operation Code Already Exists";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Combination Of Operation Type And SAC Code Already Exists";
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
        public void GetAllMstOperation()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ViewMstOperation", CommandType.StoredProcedure, dparam);
            IList<DSROperation> lstOP = new List<DSROperation>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstOP.Add(new DSROperation
                    {
                        OperationId = Convert.ToInt32(result["OperationId"]),
                        Type = Convert.ToInt32(result["OperationType"]),
                        Code = (result["OperationCode"] == null ? "" : result["OperationCode"]).ToString(),
                        SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString(),
                        ShortDescription = (result["OperationSDesc"] == null ? "" : result["OperationSDesc"]).ToString(),
                        Description = (result["OperationDesc"] == null ? "" : result["OperationDesc"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstOP;
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
        public void ViewMstOperation(int OperationId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationId", MySqlDbType = MySqlDbType.Int32, Value = OperationId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("ViewMstOperation", CommandType.StoredProcedure, dparam);
            DSROperation objOP = new DSROperation();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objOP.OperationId = Convert.ToInt32(result["OperationId"]);
                    objOP.Type = Convert.ToInt32(result["OperationType"]);
                    objOP.Code = (result["OperationCode"] == null ? "" : result["OperationCode"]).ToString();
                    objOP.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();
                    objOP.ShortDescription = (result["OperationSDesc"] == null ? "" : result["OperationSDesc"]).ToString();
                    objOP.Description = (result["OperationDesc"] == null ? "" : result["OperationDesc"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOP;
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
        public void AddEditMstWeighment(DSRCWCWeighment objCW, int Uid)
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
            IList<DSRCWCWeighment> lstWeighment = null;
            DSRCWCWeighment objCWC = null;
            try
            {
                if (WeighmentId == 0)
                {
                    lstWeighment = new List<DSRCWCWeighment>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstWeighment.Add(new DSRCWCWeighment
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
                    objCWC = new DSRCWCWeighment();
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
        public void AddEditMstEntryFees(DSRCWCEntryFees objEF, int Uid)
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
            IList<DSRCWCEntryFees> lstEntryFees = null;
            DSRCWCEntryFees objEF = null;
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                if (EntryFeeId == 0)
                {
                    lstEntryFees = new List<DSRCWCEntryFees>();
                    while (result.Read())
                    {
                        Status = 1;
                        lstEntryFees.Add(new DSRCWCEntryFees
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
                    objEF = new DSRCWCEntryFees();
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

        #region Ground Rent
        public void AddEditMstGroundRent(DSRCWCChargesGroundRent objCR, int Uid)
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
            lstparam.Add(new MySqlParameter { ParameterName = "in_Odc", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objCR.IsODC) });
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objCR.OperationType });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCR.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            //added
            lstparam.Add(new MySqlParameter { ParameterName = "in_FclLcl", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = "" });
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
            IList<DSRCWCChargesGroundRent> objList = new List<DSRCWCChargesGroundRent>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new DSRCWCChargesGroundRent
                    {
                        GroundRentId = Convert.ToInt32(result["GroundRentId"]),
                        ContainerType = Convert.ToInt32(result["ContainerType"]),
                        RentAmount = Convert.ToDecimal(result["RentAmount"]),
                        Size = result["Size"].ToString(),                     
                      //  IsODC = Convert.ToBoolean(result["IsODC"] == DBNull.Value ? 0 : result["IsODC"]),
                        DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]),
                        DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]),
                        OperationType = Convert.ToInt32(result["OperationType"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        CommodityType = Convert.ToInt32(result["CommodityType"]),

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
            DSRCWCChargesGroundRent objGR = null;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objGR = new DSRCWCChargesGroundRent();
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
                    //objGR.IsODC = Convert.ToBoolean(result["IsODC"]);
                    objGR.IsODC = Convert.ToBoolean(result["IsODC"] == DBNull.Value ? 0 : result["IsODC"]);
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
        #region Incentive
        public void AddEditMstVolumeBaseIncentive(DSRVolumeBaseIncentive objCR, int Uid)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_IncentiveId", MySqlDbType = MySqlDbType.Int32, Value = objCR.IncentiveId });
          //  lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = objCR.ContainerType, Size = 2 });
          //  lstparam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Value = objCR.CommodityType, Size = 2 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_DaysRangeFrom", MySqlDbType = MySqlDbType.Int32, Value = objCR.DaysRangeFrom });
            lstparam.Add(new MySqlParameter { ParameterName = "in_DaysRangeTo", MySqlDbType = MySqlDbType.Int32, Value = objCR.DaysRangeTo });
            //lstparam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Int32, Value = objCR.Reefer, Size = 2 });
            lstparam.Add(new MySqlParameter { ParameterName = "in_Freedays", MySqlDbType = MySqlDbType.Decimal, Value = objCR.FreeDays });
            //lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objCR.ElectricityCharge });
         //   lstparam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objCR.Size });
           // lstparam.Add(new MySqlParameter { ParameterName = "in_OdcType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objCR.OdcType) });
           // lstparam.Add(new MySqlParameter { ParameterName = "in_ExamType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objCR.ExaminationType) });
            lstparam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = objCR.OperationType });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objCR.SacCode });
            lstparam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            //added
          //  lstparam.Add(new MySqlParameter { ParameterName = "in_FclLcl", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = "" });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditMstIncentive", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1 ? "Incentive Details Saved Successfully" : "Incentive Details Updated Successfully");
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
        public void GetAllVolumeBaseIncentiveDet()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_IncentiveId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllIncentiveDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<DSRVolumeBaseIncentive> objList = new List<DSRVolumeBaseIncentive>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new DSRVolumeBaseIncentive
                    {
                        IncentiveId = Convert.ToInt32(result["IncentiveId"]),
                      //  ContainerType = Convert.ToInt32(result["ContainerType"]),
                        FreeDays = Convert.ToInt32(result["FreeDays"]),
                       // Size = result["Size"].ToString(),
                      //  OdcType = Convert.ToString(result["OdcType"] == DBNull.Value ? "0" : result["OdcType"]),
                      //  ExaminationType = Convert.ToString(result["ExaminationType"] == DBNull.Value ? "0" : result["ExaminationType"]),
                        DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]),
                        DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]),
                        OperationType = Convert.ToInt32(result["OperationType"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                      //  CommodityType = Convert.ToInt32(result["CommodityType"]),

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
        public void GetVolumeBaseIncentiveDet(int IncentiveId)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_IncentiveId", MySqlDbType = MySqlDbType.Int32, Value = IncentiveId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllIncentiveDet", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSRVolumeBaseIncentive objGR = null;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objGR = new DSRVolumeBaseIncentive();
                    objGR.IncentiveId = Convert.ToInt32(result["IncentiveId"]);
                  //  objGR.ContainerType = Convert.ToInt32(result["ContainerType"]);
                  //  objGR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    objGR.DaysRangeFrom = Convert.ToInt32(result["DaysRangeFrom"]);
                    objGR.DaysRangeTo = Convert.ToInt32(result["DaysRangeTo"]);                 
                    objGR.FreeDays = Convert.ToInt32(result["FreeDays"]);                  
                  //  objGR.Size = result["Size"].ToString();
                    objGR.OperationType = Convert.ToInt32(result["OperationType"]);
                    objGR.EffectiveDate = result["EffectiveDate"].ToString();
                    objGR.SacCode = (result["SacCode"] == null ? "" : result["SacCode"]).ToString();                  
                   // objGR.OdcType = Convert.ToString(result["OdcType"] == DBNull.Value ? "0" : result["OdcType"]);
                  //  objGR.ExaminationType = Convert.ToString(result["ExaminationType"] == DBNull.Value ? "0" : result["ExaminationType"]);
                    
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
        #region Reefer
        public void AddEditMstReefer(DSRCWCReefer objRef)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ReeferChrgId", MySqlDbType = MySqlDbType.Int32, Value = objRef.ReeferChrgId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRef.EffectiveDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ElectricityCharge", MySqlDbType = MySqlDbType.Decimal, Value = objRef.ElectricityCharge });
            //not used
            lstparam.Add(new MySqlParameter { ParameterName = "in_MonitoringCharge", MySqlDbType = MySqlDbType.Decimal, Value = 0.00 });
            
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
            IList<DSRCWCReefer> objList = new List<DSRCWCReefer>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new DSRCWCReefer
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
            DSRCWCReefer objRef = new DSRCWCReefer();
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

        #region Insurance
        public void AddEditInsurance(DSRInsurance ObjInsurance)
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
            List<DSRInsurance> LstInsurance = new List<DSRInsurance>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInsurance.Add(new DSRInsurance
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
            DSRInsurance ObjInsurance = new DSRInsurance();
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

        #region TDS
        public void AddEditMstTds(DSRCWCTds objTds)
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
            IList<DSRCWCTds> objList = new List<DSRCWCTds>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new DSRCWCTds
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
            DSRCWCTds objTds = new DSRCWCTds();
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

        #region godown
        public void AddEditGodown(DSRGodownVM ObjGodown, string LocationXML, string DelLocationXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjGodown.MstGodwon.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.VarChar, Value = ObjGodown.MstGodwon.GodownName });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjGodown.MstGodwon.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DelLocationXML", MySqlDbType = MySqlDbType.Text, Value = DelLocationXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTypeImport", MySqlDbType = MySqlDbType.Int32, Value = (ObjGodown.IsImport==true ? 1 : 0)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTypeExport", MySqlDbType = MySqlDbType.Int32, Value = (ObjGodown.IsExport==true ? 1: 0)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTypeBond", MySqlDbType = MySqlDbType.Int32, Value = (ObjGodown.IsBond==true ? 1: 0)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsOpen", MySqlDbType = MySqlDbType.Int32, Value = (ObjGodown.IsOpen==true ? 1: 0)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjGodown.MstGodwon.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstGodown", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = ObjGodown.MstGodwon.GodownId == 0 ? "Godown Details Saved Successfully" : "Godown Details Updated Successfully";
                    _DBResponse.Status = 1;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Godown Name Already Exist";
                    _DBResponse.Status = 2;
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

        public void DeleteGodown(int GodownId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstGodown", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Godown Details Deleted Successfully";
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

        public void GetAllGodown()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRGodown> LstGodown = new List<DSRGodown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new DSRGodown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString(),
                        OperationType = Result["OperationType"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGodown;
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

        public void GetGodown(int GodownId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRGodownVM ObjGodown = new DSRGodownVM();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjGodown.MstGodwon.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjGodown.MstGodwon.GodownName = Result["GodownName"].ToString();
                    //ObjGodown.MstGodwon.OperationType = Result["OperationType"].ToString();
                    ObjGodown.IsImport = Convert.ToBoolean(Result["OperationTypeImport"]);
                    ObjGodown.IsExport = Convert.ToBoolean(Result["OperationTypeExport"]);
                    ObjGodown.IsBond = Convert.ToBoolean(Result["OperationTypeBond"]);
                    ObjGodown.IsOpen = Convert.ToBoolean(Result["IsOpen"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjGodown.LstLocation.Add(new DSRGodownWiseLocation
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            LocationName = Result["LocationName"].ToString(),
                            Row = Result["Row"].ToString(),
                            Column = Result["Column"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjGodown;
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

        #region Franchise Charges
        public void GetFranchiseCharge(int franchisechargeid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = franchisechargeid });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] dparam = lstparam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetMstFranchiseCharge", CommandType.StoredProcedure, dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSRCWCFranchiseCharges objFcs = new DSRCWCFranchiseCharges();
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
                    objFcs.PortId =Convert.ToInt32(result["PortId"]);
                    objFcs.PortName = result["PortName"].ToString();

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
        public void AddEditMstFranchiseCharges(DSRCWCFranchiseCharges objFC, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_franchisechargeId", MySqlDbType = MySqlDbType.Int32, Value = objFC.franchisechargeid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objFC.EffectiveDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.ContainerSize });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Chargesfor", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = objFC.ChargesFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ODC", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(objFC.ODC) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Size = 7, Value = objFC.SacCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objFC.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortName", MySqlDbType = MySqlDbType.VarChar, Value = objFC.PortName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_roaltycharge", MySqlDbType = MySqlDbType.Decimal, Value = objFC.RoaltyCharge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_franchisecharge", MySqlDbType = MySqlDbType.Decimal, Value = objFC.FranchiseCharge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
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
            IList<DSRCWCFranchiseCharges> objList = new List<DSRCWCFranchiseCharges>();
            try
            {

               

                while (result.Read())
                {
                    Status = 1;
                    objList.Add(new DSRCWCFranchiseCharges
                    {
                        franchisechargeid = Convert.ToInt32(result["franchisechargeid"]),
                        EffectiveDate = result["EffectiveDate"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        //Chargesfor = result["Chargesfor"].ToString(),
                        //ODC = Convert.ToBoolean(result["ODC"]),
                        RoaltyCharge = Convert.ToDecimal(result["RoaltyCharge"]),
                        FranchiseCharge = Convert.ToDecimal(result["FranchiseCharge"]),
                        SacCode = result["SacCode"].ToString(),
                        PortId= Convert.ToInt32(result["PortId"]),
                        PortName = result["PortName"].ToString(),
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
        #endregion

        #region Chemical
        public void AddEditChemical(DSRChemical ObjChem)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.ChemicalId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalName", MySqlDbType = MySqlDbType.VarChar, Value = ObjChem.ChemicalName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstChemical", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjChem.ChemicalId == 0 ? "Chemical Details Saved Successfully" : "Chemical Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Chemical Name Already Exist";
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
        public void DeleteChemical(int YardId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = YardId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstYard", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Yard Details Deleted Successfully";
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
        public void GetChemical(int ChemicalId)
        {
            int Status = 0;
            List<MySqlParameter> LstParm = new List<MySqlParameter>();
            LstParm.Add(new MySqlParameter { ParameterName = "in_ChemicalId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ChemicalId });
            IDataParameter[] DParam = { };
            DParam = LstParm.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstChemical", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRChemical ObjYard = new DSRChemical();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjYard.ChemicalId = Convert.ToInt32(Result["ChemicalId"]);
                    ObjYard.ChemicalName = Result["ChemicalName"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjYard;
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
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllChemical()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstChemical", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRChemical> LstYard = new List<DSRChemical>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstYard.Add(new DSRChemical
                    {
                        ChemicalId = Convert.ToInt32(Result["ChemicalId"]),
                        ChemicalName = Result["ChemicalName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstYard;
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

        #region Exim Trader Finance Control

        public void GetEximTraderNew(int EximTraderId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = EximTraderId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderBalance", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREximTraderFinanceControl> LstEximTrader = new List<DSREximTraderFinanceControl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.Add(new DSREximTraderFinanceControl
                    {
                        PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]),
                        CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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

        public void GetEximTraderFinanceControl(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForEximFinc", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            DSRSearchEximTraderDataFinanceControl LstEximTrader = new DSRSearchEximTraderDataFinanceControl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstExim.Add(new DSRListOfEximTraderFinanceControl
                    {
                        EximTraderName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        Address = Result["Address"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        Tan = Result["Tan"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstEximTrader.State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetAllEximFinanceControl()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximFinanceControl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSREximTraderFinanceControl> LstEximTrader = new List<DSREximTraderFinanceControl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.Add(new DSREximTraderFinanceControl
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        FinanceControlId = Convert.ToInt32(Result["FinanceControlId"]),
                        //PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]),
                        // CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"] == DBNull.Value ? 0 : Result["CurrentBalance"]),
                        // CreditLimit = Convert.ToDecimal(Result["CreditLimit"] == DBNull.Value ? 0 : Result["CreditLimit"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        Address = (Result["Address"] == null ? "" : Result["Address"]).ToString(),
                        // GSTNo = (Result["GSTNo"] == null ? "" : Result["GSTNo"]).ToString(),
                        // Tan = (Result["Tan"]==null?"":Result["Tan"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetEximFinanceControl(int FinanceControlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FinanceControlId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximFinanceControl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSREximTraderFinanceControl ObjEximTrader = new DSREximTraderFinanceControl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEximTrader.FinanceControlId = Convert.ToInt32(Result["FinanceControlId"]);
                    ObjEximTrader.EximTraderId = Convert.ToInt32(Result["EximTraderId"]);
                    ObjEximTrader.CreditPeriod = Convert.ToInt32(Result["CreditPeriod"] == DBNull.Value ? 0 : Result["CreditPeriod"]);
                    ObjEximTrader.PreviousBalance = Convert.ToDecimal(Result["PreviousBalance"] == DBNull.Value ? 0 : Result["PreviousBalance"]);
                    ObjEximTrader.CurrentBalance = Convert.ToDecimal(Result["CurrentBalance"] == DBNull.Value ? 0 : Result["CurrentBalance"]);
                    ObjEximTrader.CreditLimit = Convert.ToDecimal(Result["CreditLimit"] == DBNull.Value ? 0 : Result["CreditLimit"]);
                    ObjEximTrader.EximTraderName = Result["EximTraderName"].ToString();
                    ObjEximTrader.Address = (Result["Address"] == null ? "" : Result["Address"]).ToString();
                    ObjEximTrader.GSTNo = (Result["GSTNo"] == null ? "" : Result["GSTNo"]).ToString();
                    ObjEximTrader.Tan = (Result["Tan"] == null ? "" : Result["Tan"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEximTrader;
                }
                else
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditEximFinanceControl(DSREximTraderFinanceControl ObjEximTrader)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEximTrader.FinanceControlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tan", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Tan });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PreviousBalance", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.PreviousBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CurrentBalance", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.CurrentBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreditLimit", MySqlDbType = MySqlDbType.Decimal, Value = ObjEximTrader.CreditLimit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreditPeriod", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.CreditPeriod });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEximTrader.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstEximFinanceControl", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjEximTrader.FinanceControlId == 0 ? "Exim Trader Finance Control Details Saved Successfully" : "Exim Trader Finance Control Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Exim Trader Finance Control Details Already Exist";
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
        public void DeleteEximFinanceControl(int FinanceControlId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinanceControlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FinanceControlId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstEximFinanceControl", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Exim Trader Finance Control Details Saved Successfully";
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

        #endregion

        #region Party Wise TDS Deposit
        public void GetAllEximTraderFilterWise(string FilterText)
        {
            // Type of Text for the parameter
            // 'All' For No Filter
            // 'Importer' For Importer=1
            // 'Exporter' For Exporter=1
            // 'ShippingLine' For ShippingLine=1
            // 'CHA' For CHA=1

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FilterText", MySqlDbType = MySqlDbType.String, Value = FilterText });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEximTrader", CommandType.StoredProcedure, DParam);
            List<Party> model = new List<Party>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
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

        public void AddEditPartyWiseTDSDeposit(DSRPartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            string GeneratedClientId = "0";
            if(objPartyWiseTDSDeposit.ReceiptNo=="")
            {
                objPartyWiseTDSDeposit.ReceiptNo = "0";
            }
            string RetReceiptNo = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = objPartyWiseTDSDeposit.ReceiptNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CirtificateNo", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.CirtificateNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CirtificateDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPartyWiseTDSDeposit.CirtificateDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearFrom", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYear });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearTo", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYearNext });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsQuarter", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.TdsQuarter });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = objPartyWiseTDSDeposit.Amount });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditPartyWiseTDSDeposit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RetReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "Party Wise TDS Deposit Saved Successfully" : "Party Wise TDS Deposit Updated Successfully";
                    _DBResponse.Status = Result;

                    var data = new { ReceiptNo = RetReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Data = data;


                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    //_DBResponse.Message = "On Account Balance will be negative for this Party. You cannot save this.";
                    _DBResponse.Message = "SD Balance will be negative for this Party. You cannot save this.";
                    _DBResponse.Status = -1;
                }

                else if (Result == -2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Please select SD Party. You can not deposit TDS in Cash Party";
                    _DBResponse.Status = -2;
                }
                else if (Result == -4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Certificate No Already Exist";
                    _DBResponse.Status = -4;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "You can not Update ! Its already deleted";
                    _DBResponse.Status = 3;
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

        public void GetAllPartyWiseTDSDeposit()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllPartyWiseTDSDeposit", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<DSRPartyWiseTDSDeposit> PartyWiseTDSDepositList = new List<DSRPartyWiseTDSDeposit>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSRPartyWiseTDSDeposit objPartyWiseTDSDeposit = new DSRPartyWiseTDSDeposit();
                        objPartyWiseTDSDeposit.Id = Convert.ToInt32(dr["Id"]);
                        objPartyWiseTDSDeposit.ReceiptNo = Convert.ToString(dr["ReceiptNo"]);
                        objPartyWiseTDSDeposit.PartyName = Convert.ToString(dr["PartyName"]);
                        objPartyWiseTDSDeposit.CirtificateNo = Convert.ToString(dr["CirtificateNo"]);
                        objPartyWiseTDSDeposit.CirtificateDate = Convert.ToString(dr["CirtificateDate"]);
                        objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(dr["Amount"]);
                        objPartyWiseTDSDeposit.TDSBalance = 0; //Convert.ToDecimal(dr["TDSBalance"]);
                        objPartyWiseTDSDeposit.DepositDate = Convert.ToString(dr["ReceiptDate"]);
                        objPartyWiseTDSDeposit.IsCan = "";
                        objPartyWiseTDSDeposit.Remarks = Convert.ToString(dr["Remarks"]);
                        objPartyWiseTDSDeposit.FinancialYear = Convert.ToInt32(dr["FinYarFrom"]);
                        objPartyWiseTDSDeposit.FinancialYearNext = Convert.ToInt32(dr["FinYarTo"]);
                        objPartyWiseTDSDeposit.TdsQuarter = Convert.ToString(dr["TdsQuarter"]);
                        PartyWiseTDSDepositList.Add(objPartyWiseTDSDeposit);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = PartyWiseTDSDepositList;
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
            }
        }

        public void GetPartyWiseTDSDepositDetails(int PartyWiseTDSDepositId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyWiseTDSDepositId", MySqlDbType = MySqlDbType.Int32, Value = PartyWiseTDSDepositId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetPartyWiseTDSDepositDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSRPartyWiseTDSDeposit objPartyWiseTDSDeposit = new DSRPartyWiseTDSDeposit();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPartyWiseTDSDeposit.Id = Convert.ToInt32(Result["Id"]);
                    objPartyWiseTDSDeposit.PartyId = Convert.ToInt32(Result["PartyId"]);                   
                    objPartyWiseTDSDeposit.ReceiptNo = Convert.ToString(Result["ReceiptNo"]);
                    objPartyWiseTDSDeposit.PartyName = Convert.ToString(Result["PartyName"]);
                    objPartyWiseTDSDeposit.CirtificateNo = Convert.ToString(Result["CirtificateNo"]);
                    objPartyWiseTDSDeposit.CirtificateDate = Convert.ToString(Result["CirtificateDate"]);
                    objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(Result["Amount"]);
                    objPartyWiseTDSDeposit.TDSBalance = 0;//Convert.ToDecimal(Result["TDSBalance"]);
                    objPartyWiseTDSDeposit.Remarks = Convert.ToString(Result["Remarks"]);
                    objPartyWiseTDSDeposit.FinancialYear = Convert.ToInt32(Result["FinYarFrom"]);
                    objPartyWiseTDSDeposit.FinancialYearNext = Convert.ToInt32(Result["FinYarTo"]);
                    objPartyWiseTDSDeposit.TdsQuarter = Convert.ToString(Result["TdsQuarter"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Data = objPartyWiseTDSDeposit;
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
                Result.Dispose();
                Result.Close();
            }
        }

        public void DeletePartyWiseTDSDeposit(int PartyWiseTDSDepositId, string Amount, string ReceiptNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyWiseTDSDepositId", MySqlDbType = MySqlDbType.Int32, Value = PartyWiseTDSDepositId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeletePartyWiseTDSDeposit", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Party Wise TDS Deposit Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "On Account Balance will be negative for this Party. You cannot Delete this.";
                    _DBResponse.Status = 2;
                }

                //else if (result == 3)
                //{
                //    _DBResponse.Data = null;
                //    _DBResponse.Message = "You can not delete ! Its already deleted";
                //    _DBResponse.Status = 3;
                //}
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

        #endregion


        #region PartyInsurance
        public void AddEditPartyInsurance(DSRPartyInsurance ObjPartyInsurance)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyInsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPartyInsurance.PartyInsuranceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyInsurance.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyInsurance.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyInsurance.PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyInsurance.InsuranceFrom).ToString("yyyy/MM/dd")});
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsuranceTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyInsurance.InsuranceTo).ToString("yyyy/MM/dd") });
         
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyInsurance.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyInsurance.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstPartyInsurance", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjPartyInsurance.PartyInsuranceId == 0 ? "PartyInsurance Details Saved Successfully" : "PartyInsurance Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                //else if (Result == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Party Name Already Exists";
                //    _DBResponse.Data = null;
                //}
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "PartyInsurance Details Already Exists";
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
        public void DeletePartyInsurance(int PartyInsuranceId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyInsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyInsuranceId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstInsuredParty", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Party Details Deleted Successfully";
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
        public void GetAllPartyInsurance()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyInsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInsuredPartyList", CommandType.StoredProcedure, DParam);
            List<DSRPartyInsurance> LstPartyInsurance = new List<DSRPartyInsurance>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyInsurance.Add(new DSRPartyInsurance
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        PartyInsuranceId = Convert.ToInt32(Result["PartyInsuranceId"]),
                        InsuranceFrom = (Result["InsuredDateFrm"]).ToString(),
                        InsuranceTo =(Result["InsuredDateTo"]).ToString(),
                        
                       
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPartyInsurance;
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
        public void GetPartyInsurance(int PartyInsuranceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyInsuranceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyInsuranceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DSRPartyInsurance ObjPartyInsurance = new DSRPartyInsurance();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInsuredPartyList", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    ObjPartyInsurance.PartyId = Convert.ToInt32(Result["PartyId"]);
                    ObjPartyInsurance.PartyName = Result["PartyName"].ToString();
                    ObjPartyInsurance.PartyCode = Result["PartyCode"].ToString();
                    ObjPartyInsurance.PartyInsuranceId = Convert.ToInt32(Result["PartyInsuranceId"]);
                    ObjPartyInsurance.InsuranceFrom = (Result["InsuredDateFrm"]).ToString();
                    ObjPartyInsurance.InsuranceTo = (Result["InsuredDateTo"]).ToString();
                    //ObjPort.InsuranceFrom = Result["InsuranceFrom"].ToString();
                    //ObjPort.InsuranceTo = Result["InsuranceTo"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPartyInsurance;
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

        

        public void ListOfPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyNames", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<PartylistForPage> lstImporter = new List<PartylistForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new PartylistForPage
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstImporter, State };
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

        #region Party Wise Reservation

        public void SearchByPartyReservationCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyNames", CommandType.StoredProcedure, Dparam);
            IList<PartylistForPage> lstImporter = new List<PartylistForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new PartylistForPage
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImporter;
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
        public void AddEditPartyWiseReservation(DSRPartyWiseReservation ObjPartyReservation)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyReservationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPartyReservation.PartyReservationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyReservation.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReservationFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyReservation.ReservationFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReservationTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPartyReservation.ReservationTo).ToString("yyyy/MM/dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyReservation.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.GodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyReservation.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjPartyReservation.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GF", MySqlDbType = MySqlDbType.Decimal, Value = ObjPartyReservation.GF });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MF", MySqlDbType = MySqlDbType.Decimal, Value = ObjPartyReservation.MF });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpace", MySqlDbType = MySqlDbType.Decimal, Value = ObjPartyReservation.TotalSpace });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ResType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.ResType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPartyReservation.AreaType });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstPartyWiseReservation", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjPartyReservation.PartyReservationId == 0 ? "Party Wise Reservation Details Saved Successfully" : "Party Wise Reservation Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                //else if (Result == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Party Name Already Exists";
                //    _DBResponse.Data = null;
                //}
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Can't edit, invoice already generated";//"Party Wise Reservation Details Already Exists";
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
        public void GetAllPartyWiseReservation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyReservationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationPartyList", CommandType.StoredProcedure, DParam);
            List<DSRPartyWiseReservation> LstPartyReservation = new List<DSRPartyWiseReservation>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyReservation.Add(new DSRPartyWiseReservation
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        PartyReservationId = Convert.ToInt32(Result["PartyReservationId"]),
                        ReservationFrom = (Result["ReservationFrom"]).ToString(),
                        ReservationTo = (Result["ReservationTo"]).ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = (Result["GodownName"]).ToString(),
                        OperationType = (Result["OperationType"]).ToString(),
                        GF = Convert.ToDecimal(Result["GF"].ToString()),
                        MF = Convert.ToDecimal(Result["MF"].ToString()),
                        TotalSpace = Convert.ToDecimal(Result["TotalSpace"].ToString()),
                        ResType= (Result["ResType"]).ToString(),
                        AreaType = (Result["AreaType"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPartyReservation;
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
        public void ListOfPartyWiseReservationForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyNames", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<PartylistForPage> lstImporter = new List<PartylistForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new PartylistForPage
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstImporter, State };
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

        public void GetPartyReservation(int PartyReservationId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyReservationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyReservationId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DSRPartyWiseReservation ObjPartyReservation = new DSRPartyWiseReservation();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationPartyList", CommandType.StoredProcedure, DParam);
            try
            {
                while (Result.Read())
                {
                                      
                        Status = 1;
                        ObjPartyReservation.PartyId = Convert.ToInt32(Result["PartyId"].ToString());
                        ObjPartyReservation.PartyName = Result["PartyName"].ToString();
                        ObjPartyReservation.PartyCode = Result["PartyCode"].ToString();
                        ObjPartyReservation.PartyReservationId = Convert.ToInt32(Result["PartyReservationId"]);
                        ObjPartyReservation.ReservationFrom = (Result["ReservationFrom"]).ToString();
                        ObjPartyReservation.ReservationTo = (Result["ReservationTo"]).ToString();
                        ObjPartyReservation.GodownId = Convert.ToInt32(Result["GodownId"]);
                        ObjPartyReservation.GodownName = (Result["GodownName"]).ToString();
                        ObjPartyReservation.OperationType = (Result["OperationType"]).ToString();
                        ObjPartyReservation.GF = Convert.ToDecimal(Result["GF"]);
                        ObjPartyReservation.MF = Convert.ToDecimal(Result["MF"]);
                        ObjPartyReservation.TotalSpace = Convert.ToDecimal(Result["TotalSpace"]);
                        ObjPartyReservation.ResType = (Result["ResType"]).ToString();
                        ObjPartyReservation.AreaType = (Result["AreaType"]).ToString();
                  
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPartyReservation;
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

        public void DeletePartyReservation(int PartyReservationId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyReservationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyReservationId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteMstReservationParty", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Party Details Deleted Successfully";
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

        public void GodownList(string OperationType,int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();                      
            LstParam.Add(new MySqlParameter { ParameterName = "In_UserId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "In_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationGodownList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRReservationGodownList> lstGodownList = new List<DSRReservationGodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new DSRReservationGodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        //OperationTypeImport = Convert.ToBoolean(Result["OperationTypeImport"]),
                        OperationTypeExport = Convert.ToBoolean(Result["OperationTypeExport"]),
                        OperationTypeBond = Convert.ToBoolean(Result["OperationTypeBond"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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

        //public void OperationTypeList()
        //{
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("MiscChargesForInv", CommandType.StoredProcedure, DParam);
        //    List<SelectListItem> lstOperationType = new List<SelectListItem>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstOperationType.Add(new SelectListItem { Text = result["ChargesName"].ToString(), Value = result["ChargeId"].ToString() });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstOperationType;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Status = 1;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Status = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Status = 0;
        //    }
        //    finally
        //    {
        //        result.Dispose();
        //        result.Close();
        //    }
        //}

        #endregion

        #region ChargeName
        public void AddEditChargeName(DSRChargeName ObjChargeName)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChargeName.ChargeNameId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjChargeName.ChargeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MiscInv", MySqlDbType = MySqlDbType.Int32, Value = ObjChargeName.IsMisc });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjChargeName.ChargeCode });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChargeName.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditChargeName", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjChargeName.ChargeNameId == 0 ? "Charge Name Details Saved Successfully" : "Charge Name Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Charge Name Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Charge Alias Already Exists";
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

        public void GetAllChargeName()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChargeName", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRChargeName> LstChargeName = new List<DSRChargeName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstChargeName.Add(new DSRChargeName
                    {
                        IsMisc= Convert.ToBoolean(Result["IsMisc"]),
                        ChargeName = Result["ChargeName"].ToString(),
                        ChargeCode = (Result["ChargeCode"] == null ? "" : Result["ChargeCode"]).ToString(),
                        ChargeNameId = Convert.ToInt32(Result["ChargeNameId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChargeName;
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

        public void GetChargeName(int ChargeNameId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ChargeNameId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChargeName", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRChargeName ObjChargeName = new DSRChargeName();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjChargeName.ChargeName = Result["ChargeName"].ToString();
                    ObjChargeName.IsMisc =Convert.ToBoolean(Result["IsMisc"]);
                    ObjChargeName.ChargeNameId = Convert.ToInt32(Result["ChargeNameId"]);
                    ObjChargeName.ChargeCode = (Result["ChargeCode"] == null ? "" : Result["ChargeCode"]).ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjChargeName;
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

        public void DeleteChargeName(int ChargeNameId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ChargeNameId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteChargeName", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Charge Name Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Charge Name Details Does Not Exist";
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
        #endregion


    }
}