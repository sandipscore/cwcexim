using CwcExim.Areas.Master.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class Dnd_BidderRepository
    {

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        //update for password has been off
        public void AddEditEximTrader(PPGBidder ObjEximTrader)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEximTrader.BidderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderName", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.BidderName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderAlias", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.BidderAlias });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Password", MySqlDbType = MySqlDbType.VarChar, Value = ObjEximTrader.Password });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Importer", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHA", MySqlDbType = MySqlDbType.Int32, Value = 0});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Forwarder", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rent", MySqlDbType = MySqlDbType.Int32, Value = 0 });
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEximTrader.Amount) });        
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMstBidder", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjEximTrader.BidderId == 0 ? "Bidder Details Saved Successfully" : "Bidder Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Bidder Name Already Exist";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Bidder Alias Already Exist";
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

        public void GetAllBidderListPageWise(int Page)
        {
            int Status = 0;
            string[] EmailSplit = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllBidderListPageWise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGBidder> LstEximTrader = new List<PPGBidder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    EmailSplit = ((Result["Email"] == null ? "" : Result["Email"]).ToString()).Trim().Split(',');
                    // var Email = string.Join("\n ", EmailSplit);
                    LstEximTrader.Add(new PPGBidder
                    {
                        BidderId = Convert.ToInt32(Result["BidderId"]),
                        BidderName = Result["BidderName"].ToString(),
                        // Email=string.Join("\r\n ", EmailSplit),
                        Email = string.Join("\n", EmailSplit),
                        // Email = (Result["Email"] == null ? "" : Result["Email"]).ToString(),
                        ContactPerson = (Result["ContactPerson"] == null ? "" : Result["ContactPerson"]).ToString(),
                        Type = Result["Type"].ToString(),
                        GSTNo = Result["GSTNo"].ToString(),
                        BidderCode = Result["BidderCode"].ToString(),
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


        //public void DeleteEximTrader(int EximTraderId)
        //{
        //    string GeneratedClientId = "0";
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = EximTraderId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    _DBResponse = new DatabaseResponse();
        //    int Result = DataAccess.ExecuteNonQuery("DeleteMstEximTrader", CommandType.StoredProcedure, DParam, out GeneratedClientId);
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Exim Trader Details Deleted Successfully";
        //            _DBResponse.Data = GeneratedClientId;
        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Exim Trader Finance Control";
        //            _DBResponse.Data = null;
        //        }
        //        else if (Result == 3)
        //        {
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Pda Opening";
        //            _DBResponse.Data = null;
        //        }
        //        else if (Result == 4)
        //        {
        //            _DBResponse.Status = 4;
        //            _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Export-Carting Application";
        //            _DBResponse.Data = null;
        //        }
        //        else if (Result == 5)
        //        {
        //            _DBResponse.Status = 4;
        //            _DBResponse.Message = "Cannot Delete Exim Trader Details As It Exist In Another Page";
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
        public void GetBidder(int BidderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BidderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBidder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPGBidder ObjEximTrader = new PPGBidder();
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
                   // ObjEximTrader.ShippingLine = Convert.ToBoolean(Result["ShippingLine"] == DBNull.Value ? 0 : Result["ShippingLine"]);
                   // ObjEximTrader.CHA = Convert.ToBoolean(Result["CHA"] == DBNull.Value ? 0 : Result["CHA"]);
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


        public void GetBidderForAuction(int BidderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BidderId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBidderForAuctionEMD", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DNDBidder> ObjEximTrader = new List<DNDBidder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEximTrader.Add(new DNDBidder
                    {
                        BidderId = Convert.ToInt32(Result["BidderId"]),
                        BidderName = Result["BidderName"].ToString(),
                        BidderAlias = (Result["BidderAlias"] == null ? "" : Result["BidderAlias"]).ToString(),
                        //Amount = 0M;
                    });
                  
                   
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

    }
}