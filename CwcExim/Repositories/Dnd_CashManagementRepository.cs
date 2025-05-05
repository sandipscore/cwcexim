using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using MySql.Data.MySqlClient;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Repositories
{
    public class Dnd_CashManagementRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region PAYMENT VOUCHER
        public void GetPaymentVoucherCreateInfo()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentVoucherCreateInfo");
            var model = new Ppg_PaymentVoucherInfo();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.UserGST = Result["GST"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.Expenses.Add(new Ppg_Expenses { HeadId = Convert.ToInt32(Result["HeadId"]), HeadCode = Result["HeadCode"].ToString(), HeadName = Result["HeadName"].ToString() });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.ExpHSN.Add(new Ppg_ExpHSN { HSNCode = Result["HSNCode"].ToString(), ExpCode = Result["ExpCode"].ToString() });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.HSN.Add(new Ppg_HSN { HSNId = Convert.ToInt32(Result["HSNId"]), HSNCode = Result["HSNCode"].ToString(), GST = Convert.ToDecimal(Result["GST"]) });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.Party.Add(new Ppg_Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Result["PartyName"].ToString() });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.VoucherId = "CWC/PV/" + Result["VID"].ToString().PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
                    }
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

        public void AddNewPaymentVoucher(Ppg_PaymentVoucher m)
        {
            string GeneratedClientId = "0";
            string VoucherNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Pname", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = m.Party });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Adrs", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = m.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "In_State", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.State });
            LstParam.Add(new MySqlParameter { ParameterName = "In_StCode", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.StateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "In_City", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.City });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Pin", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = m.Pin });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GST", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = m.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Vno", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = m.VoucherNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_VDate", MySqlDbType = MySqlDbType.DateTime, Value = DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.TotalAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Nrtn", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = m.Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "In_IsURG", MySqlDbType = MySqlDbType.Int16, Value = m.IsUnregister });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ExpDtl", MySqlDbType = MySqlDbType.Text, Value = Utility.CreateXML(JsonConvert.DeserializeObject<IList<ExpensesDetails>>(m.ExpensesJson)) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = m.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditPaymentVoucher", CommandType.StoredProcedure, DParam, out GeneratedClientId, out VoucherNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Voucher Saved Successfully";
                    // var data = new { VoucherNo = VoucherNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    // _DBResponse.Data =GeneratedClientId;
                    //  _DBResponse.Data = VoucherNo;
                    // _DBResponse.Dat = GeneratedClientId;
                    var data = new { VoucherNo = VoucherNo, GeneratedClientId };
                    _DBResponse.Data = data;
                    //"CWC/PV/" + GeneratedClientId.PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
                }
                else if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Voucher Number Already Exists";
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

        public void GetPaymentVoucherList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentVoucherList", CommandType.StoredProcedure, DParam);
            Ppg_PaymentVoucher ObjPaymentVou = null;
            List<Ppg_PaymentVoucher> lstPaymentVou = new List<Ppg_PaymentVoucher>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPaymentVou = new Ppg_PaymentVoucher();
                    ObjPaymentVou.PVHeadId = Convert.ToInt32(Result["PVHeadId"] == DBNull.Value ? 0 : Result["PVHeadId"]);
                    ObjPaymentVou.VoucherNo = Convert.ToString(Result["VoucherNo"]);
                    ObjPaymentVou.PaymentDate = Convert.ToString(Result["VoucherDate"] == null ? "" : Result["VoucherDate"]);
                    ObjPaymentVou.TotalAmount = Convert.ToDecimal(Result["TotalAmount"] == DBNull.Value ? 0 : Result["TotalAmount"]);
                    lstPaymentVou.Add(ObjPaymentVou);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPaymentVou;
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

        public void PaymentVoucherPrint(int PVId)
        {
            string GeneratedClientId = "0";
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PVId", MySqlDbType = MySqlDbType.Int32, Value = PVId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            //LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Ppg_PaymentVoucher LstSeal = new Ppg_PaymentVoucher();
            IDataReader Result = DataAccess.ExecuteDataReader("PaymentVoucherPrint", CommandType.StoredProcedure, DParam);
            // Kol_NewPaymentValucherModel ObjPayment = null;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSeal.Party = Convert.ToString(Result["PartyName"] == null ? "" : Result["PartyName"]);
                    LstSeal.CompanyName = Convert.ToString(Result["CompanyName"] == null ? "" : Result["CompanyName"]);
                    LstSeal.Address = Convert.ToString(Result["PartyName"] == null ? "" : Result["Address"]);
                    LstSeal.CompanyAddress = Convert.ToString(Result["CompanyAddress"] == null ? "" : Result["CompanyAddress"]);
                    LstSeal.City = Convert.ToString(Result["City"] == null ? "" : Result["City"]);
                    LstSeal.CompanyCity = Convert.ToString(Result["CompanyCity"] == null ? "" : Result["CompanyCity"]);
                    LstSeal.State = Convert.ToString(Result["State"] == null ? "" : Result["State"]);
                    LstSeal.CompanyState = Convert.ToString(Result["CompanyState"] == null ? "" : Result["CompanyState"]);
                    LstSeal.StateCode = Convert.ToString(Result["StateCode"] == null ? "" : Result["StateCode"]);
                    LstSeal.CompanyStateCode = Convert.ToInt32(Result["CompanyStateCode"] == null ? "" : Result["CompanyStateCode"]);
                    LstSeal.GSTNo = Convert.ToString(Result["GSTNo"] == null ? "" : Result["GSTNo"]);
                    LstSeal.CompanyGST = Convert.ToString(Result["CompanyGST"] == null ? "" : Result["CompanyGST"]);
                    LstSeal.PanNo = Convert.ToString(Result["GSTNo"] == null ? "" : Result["GSTNo"]);
                    LstSeal.CompanyPan = Convert.ToString(Result["CompanyPan"] == null ? "" : Result["CompanyPan"]);
                    LstSeal.VoucherNo = Convert.ToString(Result["VoucherNo"] == null ? "" : Result["VoucherNo"]);
                    LstSeal.PaymentDate = Convert.ToString(Result["VoucherDate"] == null ? "" : Result["VoucherDate"]);
                    LstSeal.TotalAmount = Convert.ToDecimal(Result["TotalAmount"] == null ? "" : Result["TotalAmount"]);
                    LstSeal.Narration = Convert.ToString(Result["Narration"] == null ? "" : Result["Narration"]);
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstSeal.expcharges.Add(new Ppg_ExpensesDetails
                        {
                            // ChargeName = Result["ChargeName"].ToString(),
                            //   Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            IGST = Convert.ToDecimal(Result["IGST"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            CGST = Convert.ToDecimal(Result["CGST"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            SGST = Convert.ToDecimal(Result["SGST"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            ExpenseHead = Convert.ToString(Result["ExpenseHead"]),
                            Expensecode = Convert.ToString(Result["Expensecode"]),
                            // Amount = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        // ChargeName = Result["ChargeName"].ToString(),
                        //   Taxable = Convert.ToDecimal(Result["Taxable"]),
                        LstSeal.TotalIGSTAmt = Convert.ToDecimal(Result["TotalIGSTAmt"]);
                        LstSeal.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        LstSeal.TotalCGSTAmt = Convert.ToDecimal(Result["TotalCGSTAmt"]);
                        LstSeal.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        LstSeal.TotalSGSTAmt = Convert.ToDecimal(Result["TotalSGSTAmt"]);
                        LstSeal.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        LstSeal.TotalAmounts = Convert.ToDecimal(Result["TotalAmount"]);
                        // Amount = Convert.ToDecimal(Result["Total"])

                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSeal;//"CWC/PV/" + GeneratedClientId.PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
                }
                else if (Status == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "No Data";
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

        #region-- ADD MONEY TO PD --

        public void GetPartyDetails(string PartyCode,int Page=0)
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
           
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetails", CommandType.StoredProcedure, DParam);
           
            IList<Dnd_PartyDetails> model = new List<Dnd_PartyDetails>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new Dnd_PartyDetails { Id = Convert.ToInt32(Result["Id"]), Name = Result["Name"].ToString(), Address = Result["Address"].ToString(), Folio = Result["Folio"].ToString(), Balance = Convert.ToDecimal(Result["Balance"]), PartyCode = Convert.ToString(Result["EximTraderAlias"]) });
                }
                if(Result.NextResult())
                {
                    while(Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstParty= model , State = State };
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

        public void AddMoneyToPD(int partyId, DateTime transDate, string xml)
        {
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransDate", MySqlDbType = MySqlDbType.DateTime, Value = transDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMoneyToPD", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Money Added To SD Successfully";
                    _DBResponse.Data = RecNo;
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

        #region Cash Receipt

        public void GetPartyList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
          
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
          
            DParam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyCashList", CommandType.StoredProcedure, DParam);
            var model = new Dnd_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.PartyDetail.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
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

        public void GetPartyListForCashReceipt(string PartyCode, int Page = 0)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            DParam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyCashList", CommandType.StoredProcedure, DParam);
            IList<ImpPartyForpage> lstParty = new List<ImpPartyForpage>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new ImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        PartyCode = Convert.ToString(Result["EximTraderAlias"])

                    });
                    
                }
                if(Result.NextResult())
                {
                    while(Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstParty, State }; ;
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
        public void GetCashRcptDetails(int PartyId, string PartyName, string Type = "INVOICE")
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstCashParty", CommandType.StoredProcedure, DParam);

            var model = new Dnd_CashReceiptModel();
           // var model = Dnd_CashReceiptModel();
            model.PartyId = PartyId;
            model.PartyName = PartyName;
            model.PayBy = PartyName;

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.PayByDetail.Add(new PayBy
                    {
                        PayByEximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        PayByName = Convert.ToString(Result["PayBy"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        CityName = Convert.ToString(Result["CityName"]),
                        CountryName = Convert.ToString(Result["CountryName"])
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.PdaAdjustdetail.Add(new PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = (Result["EximTraderId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptInvoiveMappingList.Add(new CashReceiptInvoiveMapping
                        {
                            InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                            InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                            AllTotalAmt = Convert.ToDecimal(Result["AllTotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            DueAmt = Convert.ToDecimal(Result["DueAmt"]),
                            AdjustmentAmt = Convert.ToDecimal(Result["AdjustmentAmt"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.TdsBalanceAmount = Convert.ToDecimal(Result["TdsBalanceAmount"]);
                    }
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

        // ADD Cash Receipt
        public void AddCashReceipt(Dnd_CashReceiptModel ObjCashRcpt, string xml)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PayByPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjust", MySqlDbType = MySqlDbType.Int16, Value = ObjCashRcpt.PdaAdjust == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Adjusted) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaClosing", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Closing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TotalPaymentReceipt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TdsAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.InvoiceValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjCashRcpt.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjCashRcpt.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_xml", MySqlDbType = MySqlDbType.VarChar, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.Type });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptinvdtlsxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.CashReceiptInvDtlsHtml });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddCashReceiptMultiInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Add Cash Receipt Successfully.";
                    _DBResponse.Data = data;

                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjCashRcpt.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Cash Receipt No.<span>", "Cash Receipt No. <span>" + ReceiptNo)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_RcptNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ReceiptNo });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateCashRcptHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = ReceiptNo;
                }
                else if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = ReceiptNo;
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

        // Get Cash receipt Print
        public void GetCashRcptPrint(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptMultiInvPrint", CommandType.StoredProcedure, DParam);
            var model = new PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceNo = Result["InvoiceNo"].ToString();
                    // model.InvoiceType = Result["InvoiceType"].ToString();
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    //  model.RequestId = Convert.ToInt32(Result["RequestId"]);
                    //  model.RequestNo = Result["RequestNo"].ToString();
                    //  model.RequestDate = Result["RequestDate"].ToString();
                    //  model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
                    //   model.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    //   model.PayeeName = Result["PayeeName"].ToString();
                    model.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    model.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    model.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    model.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    model.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    model.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    model.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    model.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    model.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    model.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    model.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    model.ShippingLineName = Result["ShippingLineName"].ToString();
                    model.CHAName = Result["CHAName"].ToString();
                    model.ImporterExporter = Result["ImporterExporter"].ToString();
                    model.BOENo = Result["BOENo"].ToString();
                    model.CFSCode = Result["CFSCode"].ToString();
                    // model.DestuffingDate = Result["DestuffingDate"].ToString();
                    model.StuffingDestuffingDate = Result["StuffingDestuffingDate"].ToString();
                    model.ArrivalDate = Result["ArrivalDate"].ToString();
                    model.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    model.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    model.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    model.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    model.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"].ToString();
                    model.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    model.PdaAdjustedAmount = Convert.ToDecimal(Result["PdaAdjustedAmount"]);
                    model.Remarks = Result["Remarks"].ToString();
                    // model.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                    // model.InvoiceHtml = Result["InvoiceHtml"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new PostPaymentContainer
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            CartingDate = Result["CartingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Result["StuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["StuffingDate"]),
                            DestuffingDate = Result["DestuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["DestuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentChrg.Add(new PostPaymentCharge
                        {
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CompGST = Result["CompGST"].ToString();
                        model.CompPAN = Result["CompPAN"].ToString();
                        model.CompStateCode = Result["CompStateCode"].ToString();
                        model.ROAddress = Result["ROAddress"].ToString();
                        model.CompanyAddress = Result["CompanyAddress"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptDetails.Add(new CashReceipt
                        {
                            PaymentMode = Convert.ToString(Result["PayMode"]),
                            DraweeBank = Convert.ToString(Result["DraweeBank"]),
                            InstrumentNo = Convert.ToString(Result["InstrumentNo"]),
                            Date = Convert.ToString(Result["PayDate"]),
                            Amount = Convert.ToDecimal(Result["Amount"])
                        });
                    }
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

        public void UpdatePrintHtml(int CashReceiptId, string htmlPrint)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Html", MySqlDbType = MySqlDbType.String, Value = htmlPrint });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCashRcptHtmlForPrint", CommandType.StoredProcedure, DParam);
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

        #endregion


        #region  edit receipt

        public void GetEditCashRcptPrint(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEditCashReceiptPrint", CommandType.StoredProcedure, DParam);
            var model = new PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceNo = Result["InvoiceNo"].ToString();
                    model.InvoiceType = Result["InvoiceType"].ToString();
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    model.RequestId = Convert.ToInt32(Result["RequestId"]);
                    model.RequestNo = Result["RequestNo"].ToString();
                    model.RequestDate = Result["RequestDate"].ToString();
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
                    model.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    model.PayeeName = Result["PayeeName"].ToString();
                    model.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    model.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    model.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    model.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    model.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    model.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    model.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    model.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    model.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    model.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    model.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    model.ShippingLineName = Result["ShippingLineName"].ToString();
                    model.CHAName = Result["CHAName"].ToString();
                    model.ImporterExporter = Result["ImporterExporter"].ToString();
                    model.BOENo = Result["BOENo"].ToString();
                    model.CFSCode = Result["CFSCode"].ToString();
                    // model.DestuffingDate = Result["DestuffingDate"].ToString();
                    model.StuffingDestuffingDate = Result["StuffingDestuffingDate"].ToString();
                    model.ArrivalDate = Result["ArrivalDate"].ToString();
                    model.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    model.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    model.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    model.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    model.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"].ToString();
                    model.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    model.Remarks = Result["Remarks"].ToString();
                    model.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                    model.InvoiceHtml = Result["InvoiceHtml"].ToString();
                    model.CashierRemarks = Result["CashierRemarks"].ToString();
                    model.PDAadjustedCashReceiptEdit = Result["PDAadjustedCashReceiptEdit"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new PostPaymentContainer
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            CartingDate = Result["CartingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Result["StuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["StuffingDate"]),
                            DestuffingDate = Result["DestuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["DestuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentChrg.Add(new PostPaymentCharge
                        {
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CompGST = Result["CompGST"].ToString();
                        model.CompPAN = Result["CompPAN"].ToString();
                        model.CompStateCode = Result["CompStateCode"].ToString();
                    }
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


        #endregion


        #region RECEIPT VOUCHER

        public string ReceiptVoucherNo()
        {
            var id = 1;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SELECT (MAX(ReceiptId) + 1) AS Id FROM receiptvoucher");
            try
            {


                while (Result.Read())
                {
                    id = Convert.ToInt32(Result["Id"]);
                }
                return "CWC/RV/" + id.ToString().PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
            //return "CWC/RV/" + id.ToString().PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
        }

        public void AddNewReceiptVoucher(Ppg_ReceiptVoucherModel m)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoucherNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.VoucherNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = m.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Narration", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = m.Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoucherDate", MySqlDbType = MySqlDbType.DateTime, Value = DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InstrumentNo", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = m.InstrumentNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InstrumentDate", MySqlDbType = MySqlDbType.VarChar, Value = m.InstrumentDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddReceiptVoucher", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Received Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Voucher Number Already Exists";
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

        public void GetReceiptVoucherList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReceiptVoucherList", CommandType.StoredProcedure, DParam);
            Ppg_ReceiptVoucherModel ObjRcptVou = null;
            List<Ppg_ReceiptVoucherModel> lstRcptVou = new List<Ppg_ReceiptVoucherModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRcptVou = new Ppg_ReceiptVoucherModel();
                    ObjRcptVou.ReceiptId = Convert.ToInt32(Result["ReceiptId"] == DBNull.Value ? 0 : Result["ReceiptId"]);
                    ObjRcptVou.VoucherNo = Convert.ToString(Result["VoucherNo"]);
                    ObjRcptVou.PaymentDate = Convert.ToString(Result["VoucherDate"] == null ? "" : Result["VoucherDate"]);
                    ObjRcptVou.Purpose = Convert.ToString(Result["Purpose"] == null ? "" : Result["Purpose"]);
                    ObjRcptVou.Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? 0 : Result["Amount"]);
                    ObjRcptVou.InstrumentNo = Convert.ToString(Result["InstrumentNo"]);
                    ObjRcptVou.InstrumentDate = Convert.ToString(Result["InstrumentDate"]);

                    ObjRcptVou.Narration = Result["Narration"].ToString();
                    lstRcptVou.Add(ObjRcptVou);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRcptVou;
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

        #endregion

        #region Credit Note
        public void GetInvoiceNoForCreaditNote(string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfInvcNofrCr", CommandType.StoredProcedure, DParam);
            List<ListOfInvoiceNo> lstInvcNo = new List<ListOfInvoiceNo>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvcNo.Add(new ListOfInvoiceNo { InvoiceId = Convert.ToInt32(Result["InvoiceId"]), InvoiceNo = Result["InvoiceNo"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvcNo;
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
        public void GetInvoiceDetailsForCreaditNote(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfInvcNofrCr", CommandType.StoredProcedure, DParam);
            Dnd_InvoiceDetails objInv = new Dnd_InvoiceDetails();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objInv.Module = Result["Module"].ToString();
                    objInv.InvoiceType = Result["InvoiceType"].ToString();
                    objInv.InvoiceDate = Result["InvoiceDate"].ToString();
                    objInv.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objInv.PartyName = Result["PartyName"].ToString();
                    objInv.PartyGSTNo = Result["PartyGSTNo"].ToString();
                    objInv.PartyAddress = Result["PartyAddress"].ToString();
                    objInv.PartyState = Result["PartyState"].ToString();
                    objInv.PartyStateCode = Result["PartyStateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInv.lstInvoiceCarges.Add(new InvoiceCarges
                        {
                            ChargesTypeId = Convert.ToInt32(Result["ChargesTypeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            RetValue = 0,
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = 0
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInv;
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
        public void AddCreditNote(Dnd_CreditNote objCR, string XML, string CRDR)
        {
            string ReturnObj = "";
            string Id = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCR.CRNoteDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCR.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.InvoiceType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCR.InvoiceDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCR.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = objCR.PayeeId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyGSTNo", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyGSTNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyAddress });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objCR.PartyState });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = objCR.PartyStateCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCR.TotalAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objCR.RoundUp });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GrandTotal", MySqlDbType = MySqlDbType.Decimal, Value = objCR.GrandTotal });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.Module });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddCRNote", CommandType.StoredProcedure, DParam, out Id, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    string html = GenerateHtmlForCRNote(Convert.ToInt32(Id), CRDR);
                    html = html.Replace("<br/>", "|_br_|");
                    string base64str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(html));
                    try
                    {
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(Id) });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_CRNoteHtml", MySqlDbType = MySqlDbType.Text, Value = base64str });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateCRNoteHtml", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Credit Note Saved Successfully";
                    _DBResponse.Data = ReturnObj;
                }
                else if (Result == 3)
                {
                    DBResponse.Status = 3;
                    _DBResponse.Message = "Party SD Balance is low";
                    _DBResponse.Data = ReturnObj;
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
        public void ListOfCRNote(string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCRNote", CommandType.StoredProcedure, DParam);
            List<ListOfCRNote> lstCR = new List<ListOfCRNote>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCR.Add(new ListOfCRNote
                    {
                        CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                        CRNoteNo = Result["CRNoteNo"].ToString(),
                        CRNoteDate = Result["CRNoteDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCR;
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
        public void SearchCreditDebitNote(string CRDR, string Search)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Search", MySqlDbType = MySqlDbType.VarChar, Value = Search });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchCreditDebitNote", CommandType.StoredProcedure, DParam);
            List<ListOfCRNote> lstCR = new List<ListOfCRNote>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCR.Add(new ListOfCRNote
                    {
                        CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                        CRNoteNo = Result["CRNoteNo"].ToString(),
                        CRNoteDate = Result["CRNoteDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCR;
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
        public void PrintDetailsForCRNote(int CRNoteId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfCRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfCr objCR = new PrintModelOfCr();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCR.CompanyName = Result["CompanyName"].ToString();
                    objCR.CompanyAddress = Result["CompanyAddress"].ToString();
                    objCR.CompStateName = Result["CompStateName"].ToString();
                    objCR.CompStateCode = Result["CompStateCode"].ToString();
                    objCR.CompCityName = Result["CompCityName"].ToString();
                    objCR.CompGstIn = Result["CompGstIn"].ToString();
                    objCR.CompPan = Result["CompPan"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.PartyName = Result["PartyName"].ToString();
                        objCR.PartyAddress = Result["PartyAddress"].ToString();
                        objCR.PartyCityName = Result["PartyCityName"].ToString();
                        objCR.PartyStateName = Result["PartyStateName"].ToString();
                        objCR.PartyStateCode = Result["PartyStateCode"].ToString();
                        objCR.PartyGSTIN = Result["PartyGSTIN"].ToString();
                        objCR.CRNoteNo = Result["CRNoteNo"].ToString();
                        objCR.CRNoteDate = Result["CRNoteDate"].ToString();
                        objCR.InvoiceNo = Result["InvoiceNo"].ToString();
                        objCR.InvoiceDate = Result["InvoiceDate"].ToString();
                        objCR.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objCR.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objCR.GrandTotal = Convert.ToDecimal(Result["GrandTotal"]);
                        objCR.Remarks = Result["Remarks"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModel
                        {
                            ChargeName = Result["ChargeName"].ToString(),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            SACCode = Result["SACCode"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCR;
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
        public string GenerateHtmlForCRNote(int CRNoteId, string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfCRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfCr objCR = new PrintModelOfCr();
            _DBResponse = new DatabaseResponse();
            string html = "";
            try
            {
                while (Result.Read())
                {
                    objCR.CompanyName = Result["CompanyName"].ToString();
                    objCR.CompanyAddress = Result["CompanyAddress"].ToString();
                    objCR.CompStateName = Result["CompStateName"].ToString();
                    objCR.CompStateCode = Result["CompStateCode"].ToString();
                    objCR.CompCityName = Result["CompCityName"].ToString();
                    objCR.CompGstIn = Result["CompGstIn"].ToString();
                    objCR.CompPan = Result["CompPan"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.PartyName = Result["PartyName"].ToString();
                        objCR.PartyAddress = Result["PartyAddress"].ToString();
                        objCR.PartyCityName = Result["PartyCityName"].ToString();
                        objCR.PartyStateName = Result["PartyStateName"].ToString();
                        objCR.PartyStateCode = Result["PartyStateCode"].ToString();
                        objCR.PartyGSTIN = Result["PartyGSTIN"].ToString();
                        objCR.CRNoteNo = Result["CRNoteNo"].ToString();
                        objCR.CRNoteDate = Result["CRNoteDate"].ToString();
                        objCR.InvoiceNo = Result["InvoiceNo"].ToString();
                        objCR.InvoiceDate = Result["InvoiceDate"].ToString();
                        objCR.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objCR.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objCR.GrandTotal = Convert.ToDecimal(Result["GrandTotal"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModel
                        {
                            ChargeName = Result["ChargeName"].ToString(),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            SACCode = Result["SACCode"].ToString()
                        });
                    }
                }
                string note = "";
                if (CRDR == "C")
                    note = "CREDIT NOTE";
                else
                    note = "DEBIT NOTE";
                string SACCode = "";
                objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
                {
                    if (SACCode == "")
                        SACCode = item.SACCode;
                    else
                        SACCode = SACCode + "," + item.SACCode;
                });
                html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceDate + "</span></td></tr><tr><td colspan='2'>";
                string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
                string tr = "";
                int Count = 1;
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                objCR.lstCharges.ToList().ForEach(item =>
                {
                    tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
                    IGSTAmt += item.IGSTAmt;
                    CGSTAmt += item.CGSTAmt;
                    SGSTAmt += item.SGSTAmt;
                    Count++;
                });
                string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
                string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
                html = html + htmltable + tr + tfoot;

                return html;
            }
            catch (Exception ex)
            {
                return html;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                //if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }



        //DEBIT NOTE
        public void GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfInvcNofrDebitNote", CommandType.StoredProcedure, DParam);
            Dnd_InvoiceDetails objInv = new Dnd_InvoiceDetails();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objInv.Module = Result["Module"].ToString();
                    objInv.InvoiceType = Result["InvoiceType"].ToString();
                    objInv.InvoiceDate = Result["InvoiceDate"].ToString();
                    objInv.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objInv.PartyName = Result["PartyName"].ToString();
                    objInv.PartyGSTNo = Result["PartyGSTNo"].ToString();
                    objInv.PartyAddress = Result["PartyAddress"].ToString();
                    objInv.PartyState = Result["PartyState"].ToString();
                    objInv.PartyStateCode = Result["PartyStateCode"].ToString();
                    objInv.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objInv.PayeeName = Result["PayeeName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInv.lstInvoiceCarges.Add(new InvoiceCarges
                        {
                            ChargesTypeId = Convert.ToInt32(Result["ChargesTypeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            RetValue = 0,
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = 0
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInv;
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

        public void GetChargesListForCrDb(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            /*lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });*/
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChargesForDebitNote", CommandType.StoredProcedure, DParam);
            List<Dnd_ChargeNameCrDb> objChrgs = new List<Dnd_ChargeNameCrDb>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objChrgs.Add(new Dnd_ChargeNameCrDb
                    {
                        Sr = Convert.ToInt32(Result["Sr"].ToString()),
                        Clause = Result["Clause"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IsLocalGST = Convert.ToInt32(Result["IsLocalGST"])
                    });
                }
                Status = 1;
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objChrgs;
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

        #endregion

        #region Cash Collection Against Bounced Cheque
        public void AddEditCashCollectionChq(PPG_CashColAgnBncChq objCC)
        {
            string Status = "0";

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashCollectionId", MySqlDbType = MySqlDbType.Int32, Value = objCC.CashColAgnBncChqId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashCollectionDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCC.CashColAgnBncChqDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpChaId", MySqlDbType = MySqlDbType.Int32, Value = objCC.ImpExpChaId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpChaName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objCC.ImpExpChaName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = objCC.BncChequeNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.BncChequeDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCC.BncChequeAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BounceDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.BncChequeBounceDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDDNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = objCC.ChequeDDNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.ChequeDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChequeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCC.ChequeAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.ChequeDepositDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OtherCharges", MySqlDbType = MySqlDbType.Decimal, Value = objCC.OtherCharges });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayMode", MySqlDbType = MySqlDbType.VarChar, Value = objCC.PayMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DraweeBank", MySqlDbType = MySqlDbType.VarChar, Value = objCC.DraweeBank });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InstrumentNo", MySqlDbType = MySqlDbType.VarChar, Value = objCC.InstrumentNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChqDate", MySqlDbType = MySqlDbType.VarChar, Value = objCC.ChqDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objCC.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //  lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCashCollectionChq", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            ppg_cashBncChqInvoice invobj = new ppg_cashBncChqInvoice();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    invobj.Status = Convert.ToInt32(Status);
                    //  _DBResponse.Data = Convert.ToInt32(Status);
                    _DBResponse.Data = Status;
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
        public void GetInvoiceAndCashReceipt(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objCashColAgnBncChqPrint = (CashColAgnBncChqPrint)DataAccess.ExecuteDynamicSet<CashColAgnBncChqPrint>("GetInvoiceAndCashReceipt", DParam);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objCashColAgnBncChqPrint;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetInvoiceCRForPrint(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objGenerateInvoiceCRPrint = (GenerateInvoiceCRPrint)DataAccess.ExecuteDynamicSet<GenerateInvoiceCRPrint>("GetInvoiceCRForPrint", DParam);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objGenerateInvoiceCRPrint;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void UpdateCCInvoiceAndCashReceipt(int InvoiceId, string InvoiceHtml, string ReceiptHtml)
        {
            InvoiceHtml = InvoiceHtml.Replace("<br/>", "|_br_|");
            ReceiptHtml = ReceiptHtml.Replace("<br/>", "|_br_|");
            string Invoicebase64str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(InvoiceHtml));
            string Receiptbase64str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ReceiptHtml));
            try
            {
                var LstParam1 = new List<MySqlParameter>();
                LstParam1.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
                LstParam1.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = Invoicebase64str });
                LstParam1.Add(new MySqlParameter { ParameterName = "in_ReceiptHtml", MySqlDbType = MySqlDbType.Text, Value = Receiptbase64str });
                LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam1 = LstParam1.ToArray();
                var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                DA1.ExecuteNonQuery("UpdateCCInvoiceReceipt", CommandType.StoredProcedure, DParam1);
            }
            catch { }
            _DBResponse.Status = 1;
            _DBResponse.Message = "Data Saved Successfully";
        }

        #endregion


        #region ListOfReceipt

        public void GetReceiptList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("EditCashReceiptPayModeHdr", CommandType.StoredProcedure, DParam);
            //var model = new Kol_CashReceiptModel();
            EditReceiptPayment objEditReceiptPayment = new EditReceiptPayment();

            List<EditReceiptPayment> lstEditReceiptPayment = new List<EditReceiptPayment>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEditReceiptPayment.Add(new EditReceiptPayment
                    {
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PayByPdaId = Convert.ToInt32(Result["PayByPdaId"]),
                        PDAAdjusted = Convert.ToDecimal(Result["PdaAdjustedAmount"]),
                        TotalPaymentReceipt = Convert.ToDecimal(Result["TotalPaymentReceipt"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])

                        //Result["InvoiceId"] == DBNull.Value ? 0 : Result["InvoiceId"]
                        //PartyId = Convert.ToString(Result["InvoiceNo"])
                    });
                }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                //{
                //    objEditReceiptPayment.lstPdaAdjustEdit.Add(new PdaAdjustEdit
                //    {
                //        PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                //        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                //        FolioNo = Result["FolioNo"].ToString(),
                //        Opening = Convert.ToDecimal(Result["Balance"])
                //    });
                //}
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEditReceiptPayment;
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



        public void GetReceiptPDAList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PDAEditList", CommandType.StoredProcedure, DParam);
            //var model = new Kol_CashReceiptModel();
            //EditReceiptPayment objEditReceiptPayment = new EditReceiptPayment();

            //List<PdaAdjustEdit> lstPdaAdjustEdit = new List<PdaAdjustEdit>();
            PDAListAndAddress lstPdaAdjustEdit = new PDAListAndAddress();

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    lstPdaAdjustEdit._PdaAdjustEdit.Add(new PdaAdjustEdit
                    {
                        PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        FolioNo = Result["FolioNo"].ToString(),
                        Opening = Convert.ToDecimal(Result["Balance"])
                    });
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstPdaAdjustEdit.PayByDetail.Add(new PayByEdit
                        {
                            PayByEximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                            PayByName = Convert.ToString(Result["PayBy"]),
                            Address = Convert.ToString(Result["Address"]),
                            StateName = Convert.ToString(Result["StateName"]),
                            CityName = Convert.ToString(Result["CityName"]),
                            CountryName = Convert.ToString(Result["CountryName"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPdaAdjustEdit;
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

        public void GetReceiptDtlsList(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCashReceiptDtls", CommandType.StoredProcedure, DParam);
            //var model = new Kol_CashReceiptModel();
            List<CashReceiptEditDtls> lstCashReceiptEditDtls = new List<CashReceiptEditDtls>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceiptEditDtls.Add(new CashReceiptEditDtls
                    {
                        CashReceiptDtlId = Convert.ToInt32(Result["CashReceiptDtlId"]),
                        PaymentMode = Result["PaymentMode"].ToString(),
                        DraweeBank = Result["DraweeBank"].ToString(),
                        InstrumentNo = Result["InstrumentNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? "" : Result["Amount"])
                        // CashReceiptHtml= Result["CashReceiptHtml"].ToString()

                    });
                }
                if (Status == 1)
                {
                    //lstCashReceiptEditDtls.ToList().ForEach(m => {

                    //    if(m.CashReceiptHtml !=null || m.CashReceiptHtml !="")
                    //    {
                    //        m.CashReceiptHtml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(m.CashReceiptHtml));
                    //    }
                    //});

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceiptEditDtls;
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


        public void SaveEditedCashRcpt(EditReceiptPayment objEditReceiptPayment, int uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = objEditReceiptPayment.CashReceiptId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = objEditReceiptPayment.TotalPaymentReceipt });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objEditReceiptPayment.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = objEditReceiptPayment.PayByPdaId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PDAAdjusted", MySqlDbType = MySqlDbType.Decimal, Value = objEditReceiptPayment.PDAAdjusted });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = objEditReceiptPayment.ReceiptNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = objEditReceiptPayment.InvoiceHtml });
            lstParam.Add(new MySqlParameter { ParameterName = "in_receiptTableJson", MySqlDbType = MySqlDbType.VarChar, Value = objEditReceiptPayment.receiptTableJson });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = objEditReceiptPayment. });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // int Result = DataAccess.ExecuteDataReader("EditCashReceipt", CommandType.StoredProcedure, DParam);
            int Result = DataAccess.ExecuteNonQuery("EditCashReceipt", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            //var model = new Kol_CashReceiptModel();
            List<CashReceiptEditDtls> lstCashReceiptEditDtls = new List<CashReceiptEditDtls>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                //while (Result.Read())
                //{
                //    Status = 1;

                //}

                if (Result == 1)
                {
                    //_DBResponse.Status = 1;
                    //_DBResponse.Message = "Success";
                    //_DBResponse.Data = lstCashReceiptEditDtls;

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cash Receipt Updated Successfully";
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

        #region pda refund



        public void RefundFromPDA(PPGAddMoneyToPDModelRefund m, int Uid)
        {

            string BankBranch = m.Bank + '#' + m.Branch;
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_RefundAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.RefundAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_OpeningAmt", MySqlDbType = MySqlDbType.Decimal, Value = m.OpBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ClosingAmt", MySqlDbType = MySqlDbType.Decimal, Value = m.closingBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = m.Remarks });

            // LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "In_BankBranch", MySqlDbType = MySqlDbType.Text, Value = BankBranch });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeDate", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeDate });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("RefundFromPDA", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Refund From SD Successfully";
                    _DBResponse.Data = RecNo;
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

        public void GetPartyDetailsRefund()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetailsForRefund");
            IList<PPGRefundMoneyFromPD> model = new List<PPGRefundMoneyFromPD>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new PPGRefundMoneyFromPD
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Name = Result["Name"].ToString(),
                        Address = Result["Address"].ToString(),
                        Folio = Result["Folio"].ToString(),
                        Balance = (Convert.ToDecimal(Result["Balance"])),
                        UnPaidAmount = (Convert.ToDecimal(Result["UnPaidAmount"])),
                        PName = Result["PName"].ToString(),
                        PartyAddress = Result["Address"].ToString()

                    });
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

        public void ViewSDRefund(int SDAcId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PdaId", MySqlDbType = MySqlDbType.Int32, Value = SDAcId });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ViewSDRefund", CommandType.StoredProcedure, Dparam);
            ViewSDRefund ObjSD = new ViewSDRefund();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    ObjSD.PDAACId = Convert.ToInt32(result["PdaAcId"]);
                    ObjSD.PartyName = result["Party"].ToString();
                    ObjSD.PartyAddress = result["PartyAddress"].ToString();
                    ObjSD.ClosureDate = result["ClosureDate"].ToString();
                    ObjSD.RecieptNo = result["ReceiptNo"].ToString();
                    ObjSD.OpeningAmt = Convert.ToDecimal(result["OpeningAmt"]);
                    //objCR.CartingAppId = Convert.ToInt32(result["CartingAppId"]);
                    ObjSD.RefundAmt = Convert.ToDecimal(result["RefundAmt"]);
                    ObjSD.ClosingAmt = Convert.ToDecimal(result["ClosingAmt"]);
                    ObjSD.BankName = result["Bank"].ToString();
                    ObjSD.Branch = result["Branch"].ToString();
                    ObjSD.ChqNo = (result["ChqNo"] == null ? "" : result["ChqNo"]).ToString();
                    ObjSD.ChqDate = (result["Chqdate"] == null ? "" : result["Chqdate"]).ToString();
                    ObjSD.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Data = ObjSD;
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
        public void GetSDRefundList()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SDRefundList");
            List<PPGSDRefundList> SdList = new List<PPGSDRefundList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    SdList.Add(new PPGSDRefundList
                    {
                        PDAACId = Convert.ToInt32(Result["PdaAcId"]),
                        ClosureDate = Result["ClosureDate"].ToString(),
                        PartyName = Result["Party"].ToString(),
                        RecieptNo = Result["ReceiptNo"].ToString(),
                        RefundAmt = (Convert.ToDecimal(Result["Amount"]))
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = SdList;
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

        public void getCompanyDetails()
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getCompanyDetails", CommandType.StoredProcedure, DParam);
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objCompanyDetails.RoAddress = Convert.ToString(Result["ROAddress"]).Replace("<br/>", " ");
                    objCompanyDetails.CompanyName = Convert.ToString(Result["CompanyName"]);
                    objCompanyDetails.CompanyAddress = Convert.ToString(Result["CompanyAddress"]).Replace("<br/>", " ");



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCompanyDetails;
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

        #endregion


        #region Misc. Invoice
        public void GetPaymentPartyForMiscInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImportInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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
        public void GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId, decimal Amount)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Int32, Value = Amount });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getMiscpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ppg_FumigationInvoice ObjEntryThroughGate = new ppg_FumigationInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.Amount = Convert.ToDecimal(Result["SUM"]);
                    ObjEntryThroughGate.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    ObjEntryThroughGate.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    ObjEntryThroughGate.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    ObjEntryThroughGate.Total = Convert.ToDecimal(Result["Total"]);
                    ObjEntryThroughGate.Round_up = Convert.ToDecimal(Result["Round_Up"]);



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
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
        public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<ImpPartyForpage> lstParty = new List<ImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new ImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
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
                    _DBResponse.Data = new { lstParty, State };
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
        public void PurposeListForMiscInvc()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("IncomecodeForMisleniousinvoice", CommandType.StoredProcedure, DParam);
            //IList<PurposeListForInvc> lstPurpose = new List<PurposeListForInvc>();
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPurpose.Add(new SelectListItem { Text = result["Exporter"].ToString(), Value = result["OperationSDesc"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPurpose;
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

        public void AddMiscInv(MiscInvModel ObjPostPaymentSheet, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Naration", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Naration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditMiscInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Misc. Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Misc. Invoice Updated Successfully";
                }

                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "SD Balance is not sufficient";
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        #endregion

        #region Invoice Edit
        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public void GetInvoiceForEdit(string Module)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Module });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<EditInvoiceList> objInvoiceList = new List<EditInvoiceList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objInvoiceList.Add(new EditInvoiceList()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInvoiceList;
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

        public void GetYardInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal hours = 0;
            PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                    objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                    objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                    objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                    objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                    objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                    objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                    objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                    objPostPaymentSheet.Pan = Result["Pan"].ToString();

                    objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                    objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                    objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (Result["ChargeType"].ToString() == "OTI")
                        {
                            if (Convert.ToDecimal(Result["Rate"]) > 0)
                            {
                                decimal hr = Convert.ToDecimal(Result["Amount"].ToString()) / Convert.ToDecimal(Result["Rate"]);
                                hours = hr;

                                objPostPaymentSheet.OTHours = hours;
                            }
                        }
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PpgPostPaymentChrg()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PpgPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt16(Result["IsReefer"]),
                            Insured = Convert.ToInt16(Result["Insured"]),
                            RMS = Convert.ToInt16(Result["RMS"]),
                            CargoType = Convert.ToInt16(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new PpgContainerWiseAmount()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            LineNo = HasColumn(Result, "LineNo") ? Result["LineNo"].ToString() : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
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
        #region Edit Delivery Payment Sheet

        public void GetDeliInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDelidInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            decimal hours = 0;
            //PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            PPGInvoiceGodown objPostPaymentSheet = new PPGInvoiceGodown();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                    objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                    objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                    objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                    objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                    objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                    objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                    objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                    objPostPaymentSheet.Pan = Result["Pan"].ToString();

                    objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                    objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                    objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (Result["ChargeType"].ToString() == "OTI")
                        {
                            if (Convert.ToDecimal(Result["Rate"]) > 0)
                            {
                                decimal hr = Convert.ToDecimal(Result["Amount"].ToString()) / Convert.ToDecimal(Result["Rate"]);
                                hours = hr;

                                objPostPaymentSheet.OTHours = hours;
                            }
                        }


                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PpgPostPaymentChrg()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PpgPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt16(Result["IsReefer"]),
                            Insured = Convert.ToInt16(Result["Insured"]),
                            RMS = Convert.ToInt16(Result["RMS"]),
                            CargoType = Convert.ToInt16(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new PpgContainerWiseAmount()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            LineNo = HasColumn(Result, "LineNo") ? Result["LineNo"].ToString() : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new PpgOperationCFSCodeWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstInvoiceCargo.Add(new PpgInvoiceCargo
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Result["CargoType"].ToString() == "" ? 0 : Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GdnWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GdnWiseLctnIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWt"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
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

        #region Cancel Receipt
        public void ListOfReceiptForCancel()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReceiptDetforCancel", CommandType.StoredProcedure, DParam);
            List<dynamic> lstReceipt = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstReceipt.Add(new { ReceiptId = Convert.ToInt32(Result["CashReceiptId"]), ReceiptNo = Result["ReceiptNo"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstReceipt;
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
        public void DetailsOfReceiptForCancel(int ReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = ReceiptId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReceiptDetforCancel", CommandType.StoredProcedure, DParam);
            dynamic objReceipt = new { ReceiptDate = "", PartyId = 0, PartyName = "", Total = "", Mode = "", No = "" };
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objReceipt = new
                    {
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        Total = Result["Total"].ToString(),
                        Mode = Result["Mode"].ToString(),
                        No = Result["No"].ToString()
                    };
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objReceipt;
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
        public void CancelReceiptList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCancelledReceipt", CommandType.StoredProcedure, DParam);
            List<CancelReceiptList> lstReceipt = new List<CancelReceiptList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstReceipt.Add(new CancelReceiptList
                    {
                        ReceiptId = Convert.ToInt32(Result["CashReceiptId"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        CancelledOn = Result["CancelledOn"].ToString(),
                        CancelledReason = Result["CancelledReason"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstReceipt;
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
        public void UpdateCancelReceipt(CancelReceipt objReceipt, int Uid)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = objReceipt.ReceiptId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsCancelled", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CancelledReason", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objReceipt.CancelledReason });
            lstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("CancelCashReceipt", CommandType.StoredProcedure, DParam);
            List<CancelReceiptList> lstReceipt = new List<CancelReceiptList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Receipt Cancelled Successfully";
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

        #region Fumigation Invoice
        public void GetChemical()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstChemical", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Chemical> objPaymentPartyName = new List<Chemical>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Chemical()
                    {
                        ChemicalId = Convert.ToInt32(Result["CommodityId"]),
                        ChemicalName = Convert.ToString(Result["CommodityName"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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

        public void GetFumigation(string FumigationChargeType, string InvoiceType, int PartyId, string size, string LineXML)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContCargoType", MySqlDbType = MySqlDbType.VarChar, Value = FumigationChargeType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont_size", MySqlDbType = MySqlDbType.VarChar, Value = size });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getFumigationpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ppg_FumigationInvoice ObjEntryThroughGate = new ppg_FumigationInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.Amount = Convert.ToDecimal(Result["SUM"]);
                    ObjEntryThroughGate.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    ObjEntryThroughGate.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    ObjEntryThroughGate.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    ObjEntryThroughGate.CGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                    ObjEntryThroughGate.SGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                    ObjEntryThroughGate.IGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                    ObjEntryThroughGate.Total = Convert.ToDecimal(Result["Total"]);
                    ObjEntryThroughGate.Round_up = Convert.ToDecimal(Result["Round_Up"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
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
        public void AddEditFumigationInv(ppg_FumigationInvoice ObjPostPaymentSheet, String ChemicalData, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_size", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGSTPer });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Naration", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Naration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalXML", MySqlDbType = MySqlDbType.Text, Value = ChemicalData });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditFumigationInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Fumigation Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Fumigation Invoice Updated Successfully";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Zero amount invoice can not be generated";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "SD Balance is not sufficient";
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        public void GetContainersForFumigation()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContListForFumigation", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<ContainersForFumigation> objConts = new List<ContainersForFumigation>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objConts.Add(new ContainersForFumigation()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objConts;
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

        #region Rent Invoice
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PPG_RentParty> objPaymentPartyName = new List<PPG_RentParty>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PPG_RentParty()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        logic1 = Convert.ToInt32(Result["logic1"]),
                        logic2 = Convert.ToInt32(Result["logic2"]),
                        logic3 = Convert.ToInt32(Result["logic3"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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


        public void AddEditRentInv(PPG_RentInvoice ObjPostPaymentSheet, String RentData, String ChargeData, int BranchId, int Uid, string month, int year, string Module)
        {
            string GeneratedClientId = "0";
            String mon = month.Substring(0, 3);
            int yr = year;
            // string dt = Convert.ToDateTime(ObjPostPaymentSheet.Date).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RentlXML", MySqlDbType = MySqlDbType.Text, Value = RentData });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = ChargeData });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = mon });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = yr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.addeditflg });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditRentInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Rent Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Rent Invoice Updated Successfully";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Rent Invoice Details As It Already Exist In Another Page";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "SD Balance is not sufficient";
                }

                else if (Result == 5)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Zero Value Invoice can not be generated";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Rent Invoice ";
                }

                else if (Result == -1)
                {
                    GeneratedClientId = " ";
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Rent Invoice Entry For this Month already exists. Populate data and Try to Update.";
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }



        public void GetRentDet(string Mon, int Yr, int flg, int PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = Mon });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Yr });
            LstParam.Add(new MySqlParameter { ParameterName = "flag", MySqlDbType = MySqlDbType.Int32, Value = flg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.Int32, Value = PartyCode });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            PPG_RentInvoice objInvoice = new PPG_RentInvoice();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentInvoices", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();


                while (Result.Read())
                {
                    objInvoice.lstPrePaymentCont.Add(new PpgRentDetails
                    {
                        Date = Result["InvoiceDate"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        amount = Convert.ToDecimal(Result["Amount"].ToString()),
                        cgst = Convert.ToDecimal(Result["CGST"]),
                        sgst = Convert.ToDecimal(Result["SGST"]),
                        igst = Convert.ToDecimal(Result["IGST"]),
                        round_up = Convert.ToDecimal(Result["Round_Up"]),
                        total = Convert.ToDecimal(Result["Total"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),

                        GSTNo = Result["GstNo"].ToString(),

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPpgRentInvoiceCharge.Add(new Ppg_RentInvoiceCharge
                        {
                            Date = Result["InvoiceDate"].ToString(),
                            amount = Result["Amount"].ToString(),
                            cgst = Result["CGSTAmt"].ToString(),
                            sgst = Result["SGSTAmt"].ToString(),
                            igst = Result["IGSTAmt"].ToString(),
                            round_up = Result["Round_Up"].ToString(),
                            total = Result["Total"].ToString(),
                            ChargeHead = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            GSTNo = Result["GstNo"].ToString(),

                        });
                    }
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        public void PurposeListForChargeInvc()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("RentChargeLst", CommandType.StoredProcedure, DParam);
            //IList<PurposeListForInvc> lstPurpose = new List<PurposeListForInvc>();
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPurpose.Add(new SelectListItem { Text = result["Exporter"].ToString(), Value = result["OperationSDesc"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPurpose;
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


        #region Reservation
        public void GetReservationParties()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<ReservationPartyDetails> objPaymentPartyName = new List<ReservationPartyDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new ReservationPartyDetails()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        GstNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        ComStateCode = Convert.ToString(Result["ComStateCode"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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

        public void AddEditInvoiceReservation(string dtlsXml, int mode, string type, int BranchId, int userid, string month, string year)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Mode", MySqlDbType = MySqlDbType.Int32, Value = mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceXml", MySqlDbType = MySqlDbType.VarChar, Value = dtlsXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Userid", MySqlDbType = MySqlDbType.Int32, Value = userid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.VarChar, Value = year });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });



            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceReservation", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Reservation Invoice Saved Successfully";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = ReturnObj;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "SD Balance is not sufficient";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Rent Invoice Details As It Already Exist In Another Page";

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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }


        }


        public void GetReservationInvoices(string month, int year, int mode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mode", MySqlDbType = MySqlDbType.Int32, Value = mode });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReservationInvoices", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ReservationPartyDetails> objPaymentPartyName = new List<ReservationPartyDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new ReservationPartyDetails()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        GstNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        RoundOff = Convert.ToDecimal(Result["RoundOff"]),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        GF = Convert.ToDecimal(Result["GF"]),
                        MF = Convert.ToDecimal(Result["MF"]),
                        TotalSpace = Convert.ToDecimal(Result["TotalSpace"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        ComStateCode = Convert.ToString(Result["ComStateCode"]),
                        IsCashReceipt = Convert.ToInt32(Result["IsCashReceipt"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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

        #region ChequeDeposit
        public void GetMstBank()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstBank", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgBank> objBankList = new List<PpgBank>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBankList.Add(new PpgBank()
                    {
                        BankId = Convert.ToInt32(Result["BankId"]),
                        BankName = Convert.ToString(Result["BankName"]),
                        AccountNo = Convert.ToString(Result["AccountNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        IFSC = Convert.ToString(Result["IFSC"]),
                        Branch = Convert.ToString(Result["Branch"]),
                        MobileNo = Convert.ToString(Result["MobileNo"]),
                        FaxNo = Convert.ToString(Result["FaxNo"]),
                        Email = Convert.ToString(Result["Email"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBankList;
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

        public void AddEditChequeDeposit(string dtlsXml, int mode, int userid, string EntryDate, int ChequeDepositId = 0)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Mode", MySqlDbType = MySqlDbType.Int32, Value = mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDepositId", MySqlDbType = MySqlDbType.Int32, Value = ChequeDepositId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDepositNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Xml", MySqlDbType = MySqlDbType.Text, Value = dtlsXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Userid", MySqlDbType = MySqlDbType.Int32, Value = userid });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });



            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditChequeDeposit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Deleted Successfully";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = ReturnObj;
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }


        }

        public void GetChequeDepositsAll()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDepositId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequeDeposits", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ChequeDepositList> objChqDepList = new List<ChequeDepositList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objChqDepList.Add(new ChequeDepositList()
                    {
                        ChequeDepositId = Convert.ToInt32(Result["ChequeDepositId"]),
                        ChequeDepositNo = Convert.ToString(Result["ChequeDepositNo"]),
                        EntryDate = Convert.ToString(Result["EntryDate"]),
                        ChequeNos = Convert.ToString(Result["ChequeNos"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objChqDepList;
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

        public void GetChequeDeposit(int Id)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDepositId", MySqlDbType = MySqlDbType.Int32, Value = Id });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequeDeposits", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //IList<ChequeDepositList> objChqDepList = new List<ChequeDepositList>();
            ChequeDeposit obj = new ChequeDeposit();

            int ChequeDepositId = 0;
            string ChequeDepositNo = "";
            string EntryDate = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ChequeDepositId = Convert.ToInt32(Result["ChequeDepositId"]);
                    ChequeDepositNo = Convert.ToString(Result["ChequeDepositNo"]);
                    EntryDate = Convert.ToString(Result["EntryDate"]);

                    obj.ChequeDetails.Add(new ChequeDepositDetail()
                    {
                        BankId = Convert.ToInt32(Result["BankId"]),
                        BankName = Convert.ToString(Result["BankName"]),
                        AccountNo = Convert.ToString(Result["AccountNo"]),
                        ChequeDate = Convert.ToString(Result["ChequeDate"]),
                        ChequeNo = Convert.ToString(Result["ChequeNo"]),
                        Mode = Convert.ToString(Result["Mode"]),
                        ModeText = "",
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        ChequeDepositDtlId = Convert.ToInt32(Result["ChequeDepositDtlId"]),
                    });

                    obj.ChequeDepositId = Id;
                    obj.ChequeDepositNo = ChequeDepositNo;
                    obj.EntryDate = EntryDate;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = obj;
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

        #region Cash Deposit To Bank

        public void GetCashDepositBalance(string TransactionDate)
        {
            int Status = 0;
            decimal Balance = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_TranDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(TransactionDate).ToString("yyyy-MM-dd HH:mm") + ":00" });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashDepositBalance", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Balance = Convert.ToDecimal(Result["CashDepositBalance"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Balance;
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

        public void GetPartyWiseSDBalance(int PartyId, string BalanceDate)
        {
            int Status = 0;
            decimal Balance = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(BalanceDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSDBalancePartyWise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Balance = Convert.ToDecimal(Result["SDBalance"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Balance;
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


        public void SaveCashDepositToBank(CashDepositToBank obj)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CashDepositToBank_Id", MySqlDbType = MySqlDbType.Int32, Value = obj.Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Value = obj.BankId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankAccountNo", MySqlDbType = MySqlDbType.VarChar, Value = obj.BankAccountNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(obj.DepositDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAmount", MySqlDbType = MySqlDbType.Decimal, Value = obj.DepositAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalanceAmount", MySqlDbType = MySqlDbType.Decimal, Value = obj.BalanceAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("SaveCashDepositToBank", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully";
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }


        }

        #endregion

        #region Cargo Shift Edit
        public void GetCargoShiftingNos()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoShiftingNos", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<dynamic> objList = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objList.Add(new
                    {
                        CargoShiftingId = Convert.ToInt32(Result["CargoShiftingId"]),
                        ShiftingNo = Convert.ToString(Result["ShiftingNo"]),
                        ShiftingDt = Convert.ToString(Result["ShiftingDt"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objList;
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
        public void GetCargoShiftingDetailsInv(int CargoShiftingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoShiftingId", MySqlDbType = MySqlDbType.Int32, Value = CargoShiftingId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoShiftingDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Areas.Export.Models.PpgInvoiceCargoShifting objPostPaymentSheet = new Areas.Export.Models.PpgInvoiceCargoShifting();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ShiftingNo = Result["ShiftingNo"].ToString();
                    objPostPaymentSheet.CargoShiftingId = Convert.ToInt32(Result["CargoShiftingId"]);
                    objPostPaymentSheet.ShiftingDt = Result["ShiftingDt"].ToString();
                    objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objPostPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                    objPostPaymentSheet.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    objPostPaymentSheet.FromGodownName = Result["FromGodownName"].ToString();
                    objPostPaymentSheet.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    objPostPaymentSheet.ToGodownName = Result["ToGodownName"].ToString();
                    objPostPaymentSheet.FromShippingId = Convert.ToInt32(Result["FromShippingId"]);
                    objPostPaymentSheet.FromShippingLineName = Result["FromShippingLineName"].ToString();
                    objPostPaymentSheet.ToShippingId = Convert.ToInt32(Result["ToShippingId"]);
                    objPostPaymentSheet.ToShippingLineName = Result["ToShippingLineName"].ToString();
                    objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objPostPaymentSheet.PayeeName = Result["PayeeName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstShippingDet.Add(new Areas.Export.Models.CargoShiftingShipBillDetails
                        {
                            CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                            CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                            RegisterDate = Convert.ToString(Result["RegisterDate"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Convert.ToString(Result["GodownName"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                            ActualQty = Convert.ToDecimal(Result["ActualQty"]),
                            ActualWeight = Convert.ToDecimal(Result["ActualWeight"]),
                            IsChecked = Convert.ToInt32(Result["IsChecked"]) == 0 ? false : true,
                            SQM = Convert.ToDecimal(Result["SQM"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                        objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                        objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                        objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                        objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                        objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                        objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                        objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                        objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                        objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                        objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                        objPostPaymentSheet.Pan = Result["Pan"].ToString();

                        objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                        objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                        objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Export.Models.PpgPostPaymentChrgShifting()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Export.Models.PpgOperationCFSCodeWiseAmountCS
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
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

        #region Container Debit Invoice

        public void GetContainerList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContForDebitInv", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<PPG_Container> LstStuffing = new List<PPG_Container>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new PPG_Container
                    {
                        id = Convert.ToInt32(Result["ContainerInfoId"].ToString()),
                        ContainerNo = Result["ContainerNo"].ToString() + "(" + Result["CFSCode"].ToString() + ")",
                        size = Result["Size"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        In_Date = Result["InDate"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        //public void ListOfGREParty()
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetListOfParty", CommandType.StoredProcedure);
        //    _DBResponse = new DatabaseResponse();
        //    List<PartyDet> LstStuffing = new List<PartyDet>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstStuffing.Add(new PartyDet
        //            {
        //                PartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
        //                PartyName = Result["EximTraderName"].ToString(),
        //                GstNo = Result["GSTNo"].ToString(),
        //                StateCode = Result["StateCode"].ToString()

        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = LstStuffing;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Dispose();
        //        Result.Close();
        //    }
        //}

        public void ListOfChargesName()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCharges", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<Charge> LstStuffing = new List<Charge>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Charge
                    {
                        ChargeId = Convert.ToInt32(Result["OperationId"].ToString()),
                        ChargeName = Result["OperationSDesc"].ToString()


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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

        public void GetAmountForCharges(int cid, String cname, String size, string Fromdt, string todt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_operationId", MySqlDbType = MySqlDbType.Int32, Value = cid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_charges_name", MySqlDbType = MySqlDbType.VarChar, Value = cname });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Fromdt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(todt) });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChargesAmount", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Decimal Amount = 0;
            try
            {
                if (Result.Read())
                {
                    Status = 1;

                    Amount = Convert.ToDecimal(Result["Amount"].ToString());



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Amount;
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
        public void GetDebitInvoice(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Int32, Value = TotalChrgAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Charges", MySqlDbType = MySqlDbType.VarChar, Value = Charges });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getcontainerdebitpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPG_DebitInvoice ObjEntryThroughGate = new PPG_DebitInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.TotalChrgAmount = Convert.ToDecimal(Result["SUM"]);
                    ObjEntryThroughGate.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    ObjEntryThroughGate.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    ObjEntryThroughGate.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    ObjEntryThroughGate.IGSTPER = Convert.ToDecimal(Result["IGSTPer"]);

                    ObjEntryThroughGate.CGSTPER = Convert.ToDecimal(Result["CGSTPER"]);
                    ObjEntryThroughGate.SGSTPER = Convert.ToDecimal(Result["SGSTPER"]);
                    ObjEntryThroughGate.Total = Convert.ToDecimal(Result["Total"]);
                    ObjEntryThroughGate.Round_up = Convert.ToDecimal(Result["Round_Up"]);



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
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

        public void AddEditDebitInvoice(PPG_DebitInvoice ObjPostPaymentSheet, String ChemicalData, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_size", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalChrgAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPER", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGSTPER });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPER", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGSTPER });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPER", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGSTPER });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = ChemicalData });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditDebitInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Debit Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Debit Invoice Updated Successfully";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Zero amount invoice can not be generated";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "SD Balance is not sufficient";
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        public void GetPartyList(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPartyForContainerDebit", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            SearchEximTraderContainerDebit LstEximTrader = new SearchEximTraderContainerDebit();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstExim.Add(new PartyDet
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"]),
                        PartyName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        GstNo = Result["GSTNo"].ToString(),
                        StateCode = Result["StateCode"].ToString()
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

        public void GetPayeeList(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPayeeForContainerDebit", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            SearchEximTraderContainerDebit LstEximTrader = new SearchEximTraderContainerDebit();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstExim.Add(new PartyDet
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"]),
                        PartyName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        GstNo = Result["GSTNo"].ToString(),
                        StateCode = Result["StateCode"].ToString()
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
        #endregion

        #region Container Movement Invoice Edit
        public void GetContainerMovementInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerMovementInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //PpgInvoiceYard objPostPaymentSheet = new PpgInvoiceYard();
            Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = new Areas.Export.Models.PPG_MovementInvoice();
            try
            {
                while (Result.Read())
                {
                    objPostPaymentSheet.ROAddress = Result["ROAddress"].ToString();
                    objPostPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPostPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPostPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPostPaymentSheet.PhoneNo = Result["PhoneNo"].ToString();
                    objPostPaymentSheet.FaxNumber = Result["FaxNumber"].ToString();
                    objPostPaymentSheet.EmailAddress = Result["EmailAddress"].ToString();
                    objPostPaymentSheet.StateId = Convert.ToInt32(Result["StateId"]);
                    objPostPaymentSheet.StateCode = Result["StateCode"].ToString();

                    objPostPaymentSheet.CityId = Convert.ToInt32(Result["CityId"]);
                    objPostPaymentSheet.GstIn = Result["GstIn"].ToString();
                    objPostPaymentSheet.Pan = Result["Pan"].ToString();

                    objPostPaymentSheet.CompGST = Result["GstIn"].ToString();
                    objPostPaymentSheet.CompPAN = Result["Pan"].ToString();
                    objPostPaymentSheet.CompStateCode = Result["StateCode"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                        objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                        objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                        objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                        objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                        objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                        objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                        objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                        objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                        objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                        objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                        objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                        objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                        objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                        objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                        objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                        objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                        objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                        objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                        objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                        objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                        objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                        objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                        objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                        objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                        objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                        objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                        objPostPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPostPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPostPaymentSheet.TotalWtPerUnit = Result["TotalWtPerUnit"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalWtPerUnit"]);
                        objPostPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPostPaymentSheet.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"] == DBNull.Value ? "" : Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                        objPostPaymentSheet.TotalValueOfCargo = Result["TotalValueOfCargo"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalValueOfCargo"]);
                        objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                        objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                        objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                        objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                        objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                        objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                        objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                        objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                        objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                        objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                        objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                        objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);
                        objPostPaymentSheet.LocationId = Convert.ToInt32(Result["LocationId"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new Areas.Export.Models.PpgPostPaymentChrg()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new Areas.Export.Models.PpgPostPaymentContainer
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt16(Result["IsReefer"]),
                            Insured = Convert.ToInt16(Result["Insured"]),
                            RMS = Convert.ToInt16(Result["RMS"]),
                            CargoType = Convert.ToInt16(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstContWiseAmount.Add(new Areas.Export.Models.PpgContainerWiseAmount
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            LineNo = HasColumn(Result, "LineNo") ? Result["LineNo"].ToString() : ""
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPostPaymentSheet.lstOperationCFSCodeWiseAmount.Add(new Areas.Export.Models.PpgOperationCFSCodeWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
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

        public void EditContainerMovementInvoice(Areas.Export.Models.PPG_MovementInvoice ObjPostPaymentSheet,
            string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("EditInvoiceContainerMovement", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
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
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion


        #region Cash Receipt SD

        public void GetPartyListSD()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartySDList", CommandType.StoredProcedure, DParam);
            var model = new PPG_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.PartyDetail.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
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


        public void GetCashRcptDetailsSD(int PartyId, string PartyName, string Type = "")
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstSDParty", CommandType.StoredProcedure, DParam);

            var model = new PPG_CashReceiptModel();
            model.PartyId = PartyId;
            model.PartyName = PartyName;
            model.PayBy = PartyName;

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.PayByDetail.Add(new PayBy
                    {
                        PayByEximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        PayByName = Convert.ToString(Result["PayBy"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        CityName = Convert.ToString(Result["CityName"]),
                        CountryName = Convert.ToString(Result["CountryName"])
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.PdaAdjustdetail.Add(new PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = (Result["EximTraderId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptInvoiveMappingList.Add(new CashReceiptInvoiveMapping
                        {
                            InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                            InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                            AllTotalAmt = Convert.ToDecimal(Result["AllTotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            DueAmt = Convert.ToDecimal(Result["DueAmt"]),
                            AdjustmentAmt = Convert.ToDecimal(Result["AdjustmentAmt"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.TdsBalanceAmount = Result["TdsBalanceAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TdsBalanceAmount"]);
                    }
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

        // ADD Cash Receipt
        public void AddCashReceiptSD(PPG_CashReceiptModel ObjCashRcpt, string xml)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PayByPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjust", MySqlDbType = MySqlDbType.Int16, Value = ObjCashRcpt.PdaAdjust == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Adjusted) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaClosing", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Closing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TotalPaymentReceipt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TdsAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.InvoiceValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjCashRcpt.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjCashRcpt.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_xml", MySqlDbType = MySqlDbType.VarChar, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.Type });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptinvdtlsxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.CashReceiptInvDtlsHtml });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("addcashreceiptSDmultiinvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Add Cash Receipt Successfully.";
                    _DBResponse.Data = data;

                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjCashRcpt.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Cash Receipt No.<span>", "Cash Receipt No. <span>" + ReceiptNo)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_RcptNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ReceiptNo });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateCashRcptHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = Convert.ToDecimal(ReceiptNo);
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

        // Get Cash receipt Print
        public void GetCashRcptPrintSD(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptMultiInvPrint", CommandType.StoredProcedure, DParam);
            var model = new PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceNo = Result["InvoiceNo"].ToString();
                    // model.InvoiceType = Result["InvoiceType"].ToString();
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    //  model.RequestId = Convert.ToInt32(Result["RequestId"]);
                    //  model.RequestNo = Result["RequestNo"].ToString();
                    //  model.RequestDate = Result["RequestDate"].ToString();
                    //  model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
                    //   model.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    //   model.PayeeName = Result["PayeeName"].ToString();
                    model.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    model.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    model.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    model.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    model.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    model.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    model.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    model.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    model.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    model.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    model.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    model.ShippingLineName = Result["ShippingLineName"].ToString();
                    model.CHAName = Result["CHAName"].ToString();
                    model.ImporterExporter = Result["ImporterExporter"].ToString();
                    model.BOENo = Result["BOENo"].ToString();
                    model.CFSCode = Result["CFSCode"].ToString();
                    // model.DestuffingDate = Result["DestuffingDate"].ToString();
                    model.StuffingDestuffingDate = Result["StuffingDestuffingDate"].ToString();
                    model.ArrivalDate = Result["ArrivalDate"].ToString();
                    model.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    model.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    model.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    model.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    model.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"].ToString();
                    model.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    model.PdaAdjustedAmount = Convert.ToDecimal(Result["PdaAdjustedAmount"]);
                    model.Remarks = Result["Remarks"].ToString();
                    // model.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                    // model.InvoiceHtml = Result["InvoiceHtml"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new PostPaymentContainer
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            CartingDate = Result["CartingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Result["StuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["StuffingDate"]),
                            DestuffingDate = Result["DestuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["DestuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentChrg.Add(new PostPaymentCharge
                        {
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CompGST = Result["CompGST"].ToString();
                        model.CompPAN = Result["CompPAN"].ToString();
                        model.CompStateCode = Result["CompStateCode"].ToString();
                        model.ROAddress = Result["ROAddress"].ToString();
                        model.CompanyAddress = Result["CompanyAddress"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptDetails.Add(new CashReceipt
                        {
                            PaymentMode = Convert.ToString(Result["PayMode"]),
                            DraweeBank = Convert.ToString(Result["DraweeBank"]),
                            InstrumentNo = Convert.ToString(Result["InstrumentNo"]),
                            Date = Convert.ToString(Result["PayDate"]),
                            Amount = Convert.ToDecimal(Result["Amount"])
                        });
                    }
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

        public void UpdatePrintHtmlSD(int CashReceiptId, string htmlPrint)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Html", MySqlDbType = MySqlDbType.String, Value = htmlPrint });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCashRcptHtmlForPrint", CommandType.StoredProcedure, DParam);
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

        #endregion

        #region-- Cheque Return --

        public void GetPartyDetail(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequePartyDetails", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            ChequeReturn LstEximTrader = new ChequeReturn();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstCheque.Add(new ChequeDetails
                    {
                        PartyId = Convert.ToInt32(Result["Id"]),
                        PartyName = Result["Name"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        //  ChequeName = Result["Name"].ToString(),
                        ChequeBalance = Convert.ToDecimal(Result["Balance"]),
                        ChequeSdNo = Result["SdNo"].ToString()
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
        public void GetChequeNo(int PartyId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequesForReturn", CommandType.StoredProcedure, DParam);
            IList<ChequeDetail> model = new List<ChequeDetail>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new ChequeDetail
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Cheque = Result["SdNo"].ToString(),

                    });

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
        public void GetChequeDetail(int partyid, string ChequeNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = partyid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pda", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCheques", CommandType.StoredProcedure, DParam);
            IList<ChequeDetail> model = new List<ChequeDetail>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new ChequeDetail
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Cheque = Result["SdNo"].ToString(),

                    });

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
        public void GetChequeDetails(string ChequeNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequedetailsForReturn", CommandType.StoredProcedure, DParam);
            IList<Cheques> model = new List<Cheques>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new Cheques
                    {
                        ChequeDate = Result["PdaChequeDate"].ToString(),
                        DraweeBank = Result["PdaDrawBank"].ToString(),
                        Amount = Result["Amount"].ToString()
                    });

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


        public void AddChequeReturn(int partyId, string ChequeReturnDate, string SdNo, string Balance, string ChequeNo, string DraweeBank, string Narration, string ChequeDate, decimal Amount, decimal AdjustedBalance)
        {
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeReturnDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ChequeReturnDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_SDNO", MySqlDbType = MySqlDbType.VarChar, Value = SdNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Balance", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDecimal(Balance) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_DraweeBank", MySqlDbType = MySqlDbType.VarChar, Value = DraweeBank });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Narration", MySqlDbType = MySqlDbType.VarChar, Value = Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ChequeDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_AdjustedBalance", MySqlDbType = MySqlDbType.Decimal, Value = AdjustedBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMoneyToDisHonourCheque", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "DisHonour Cheque Saved Successfully";
                    _DBResponse.Data = RecNo;
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
        public void GetAllChequeReturn()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllChequeReturn", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<ChequeReturn> PartyWiseTDSDepositList = new List<ChequeReturn>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        ChequeReturn objPartyWiseTDSDeposit = new ChequeReturn();
                        objPartyWiseTDSDeposit.DishonuredId = Convert.ToInt32(dr["DishonuredId"]);
                        objPartyWiseTDSDeposit.AutoDisHonourRcptNo = Convert.ToString(dr["ReceiptNo"]);
                        objPartyWiseTDSDeposit.ChequeReturnDate = Convert.ToString(dr["ChequeReturnDate"]);
                        objPartyWiseTDSDeposit.PartyName = Convert.ToString(dr["PartyName"]);
                        objPartyWiseTDSDeposit.SdNo = Convert.ToString(dr["SDNo"]);
                        objPartyWiseTDSDeposit.Balance = Convert.ToDecimal(dr["Balance"]);
                        objPartyWiseTDSDeposit.ChequeNo = Convert.ToString(dr["ChequeNo"]);
                        objPartyWiseTDSDeposit.DraweeBank = Convert.ToString(dr["DraweeBank"]);
                        objPartyWiseTDSDeposit.ChequeDate = Convert.ToString(dr["ChequeDate"]);
                        objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(dr["Amount"]);
                        objPartyWiseTDSDeposit.AdjustedBalance = Convert.ToDecimal(dr["AdjustedBalance"]);
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

        #endregion
        #region  Cargo/Container Hold
        public void GetCargoList(int CargoType,int Stats)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_statusValue", MySqlDbType = MySqlDbType.Int32, Value = Stats });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoContainerDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Dnd_HoldRelease> objListForInvoice = new List<Dnd_HoldRelease>();
            try
            {
                //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
                while (Result.Read())
                {
                    Status = 1; 

                    objListForInvoice.Add(new Dnd_HoldRelease()
                    {
                        Id=Convert.ToInt32(Result["Id"]),
                          No = Result["No"].ToString(),
                        Date = Result["Date"].ToString(),
                        CFSCode= Result["CFSCode"].ToString(),
                        StatusType= Result["StatusValue"].ToString(),
                    });
                     
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objListForInvoice;
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
        public void AddEditHoldRelease(Dnd_HoldRelease ObjHold)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjHold.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjHold.Date).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StatusType", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.StatusType});
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.No });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reason", MySqlDbType = MySqlDbType.VarChar, Value = ObjHold.Reason });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditHoldRelease", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Hold Release  Saved Successfully";
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
        public void GetAllHoldRelease()

        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllHoldRelease", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<Dnd_HoldRelease> HoldList = new List<Dnd_HoldRelease> ();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Dnd_HoldRelease ReleaseList = new Dnd_HoldRelease();
                        ReleaseList.Id= Convert.ToInt32(dr["HoldId"]);
                        ReleaseList.No = (dr["ContainerNo"]).ToString();
                        ReleaseList.DeliveryType = (dr["ContainerType"]).ToString();
                        ReleaseList.CargoType = (dr["CargoType"]).ToString();
                        ReleaseList.StatusType = (dr["StatusType"]).ToString();
                        ReleaseList.Date = (dr["Date"]).ToString();
                        ReleaseList.CFSCode = (dr["CFSCode"]).ToString();
                        //objPartyWiseTDSDeposit.DishonuredId = Convert.ToInt32(dr["DishonuredId"]);
                        //objPartyWiseTDSDeposit.AutoDisHonourRcptNo = Convert.ToString(dr["ReceiptNo"]);
                        //objPartyWiseTDSDeposit.ChequeReturnDate = Convert.ToString(dr["ChequeReturnDate"]);
                        //objPartyWiseTDSDeposit.PartyName = Convert.ToString(dr["PartyName"]);
                        //objPartyWiseTDSDeposit.SdNo = Convert.ToString(dr["SDNo"]);
                        //objPartyWiseTDSDeposit.Balance = Convert.ToDecimal(dr["Balance"]);
                        //objPartyWiseTDSDeposit.ChequeNo = Convert.ToString(dr["ChequeNo"]);
                        //objPartyWiseTDSDeposit.DraweeBank = Convert.ToString(dr["DraweeBank"]);
                        //objPartyWiseTDSDeposit.ChequeDate = Convert.ToString(dr["ChequeDate"]);
                        //objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(dr["Amount"]);
                        //objPartyWiseTDSDeposit.AdjustedBalance = Convert.ToDecimal(dr["AdjustedBalance"]);
                        HoldList.Add(ReleaseList);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = HoldList;
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

        #endregion

    }
}