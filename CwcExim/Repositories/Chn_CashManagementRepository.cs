using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
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
    public class Chn_CashManagementRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            var model = new Kol_PaymentVoucherCreateInfoModel();
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
                        model.Expenses.Add(new Expenses { HeadId = Convert.ToInt32(Result["HeadId"]), HeadCode = Result["HeadCode"].ToString(), HeadName = Result["HeadName"].ToString() });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.ExpHSN.Add(new ExpHSN { HSNCode = Result["HSNCode"].ToString(), ExpCode = Result["ExpCode"].ToString() });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.HSN.Add(new HSN { HSNId = Convert.ToInt32(Result["HSNId"]), HSNCode = Result["HSNCode"].ToString(), GST = Convert.ToDecimal(Result["GST"]) });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        model.Party.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Result["PartyName"].ToString() });
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

        public void AddNewPaymentVoucher(Chn_NewPaymentValucherModel m)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Pname", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = m.Party });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Adrs", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = m.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "In_State", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.State });
            LstParam.Add(new MySqlParameter { ParameterName = "In_StCode", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.StateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "In_City", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.City });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Pin", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = m.Pin });
            LstParam.Add(new MySqlParameter { ParameterName = "In_GST", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = m.GSTNo });
            //Added subir
            LstParam.Add(new MySqlParameter { ParameterName = "In_PanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = m.PanNo });
            //end
            LstParam.Add(new MySqlParameter { ParameterName = "In_Vno", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = m.VoucherNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_VDate", MySqlDbType = MySqlDbType.DateTime, Value = DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.TotalAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Nrtn", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = m.Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "In_IsURG", MySqlDbType = MySqlDbType.Int16, Value = m.IsUnregister });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ExpDtl", MySqlDbType = MySqlDbType.Text, Value = Utility.CreateXML(JsonConvert.DeserializeObject<IList<ExpensesDetails>>(m.ExpensesJson)) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddNewPaymentVoucher", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Voucher Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;//"CWC/PV/" + GeneratedClientId.PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
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
            Chn_NewPaymentValucherModel ObjPaymentVou = null;
            List<Chn_NewPaymentValucherModel> lstPaymentVou = new List<Chn_NewPaymentValucherModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPaymentVou = new Chn_NewPaymentValucherModel();
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
        #endregion

        #region-- ADD MONEY TO PD --

        public void GetPartyDetails()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetails");
            IList<PartyDetails> model = new List<PartyDetails>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new PartyDetails { Id = Convert.ToInt32(Result["Id"]), Name = Result["Name"].ToString(), Address = Result["Address"].ToString(), Folio = Result["Folio"].ToString(), Balance = Convert.ToDecimal(Result["Balance"]) });
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
        public void GetAddToMoneyCashReceiptList(int Page)
        {


            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAddToMoneyList", CommandType.StoredProcedure, DParam);
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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
        public void GetAllAddToMoneyReceiptNo(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAddToMoneySearchList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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

        #region Cash Receipt

        public void GetPartyList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyCashList", CommandType.StoredProcedure, DParam);
            var model = new Chn_CashReceiptModel();
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


        public void GetCashRcptDetails(int PartyId, string PartyName, string Type = "INVOICE")
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstCashParty", CommandType.StoredProcedure, DParam);

            var model = new Chn_CashReceiptModel();
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
        // List oF CashReceipt
        public void GetCashReceiptList(int Page)
        {
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
           IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptList", CommandType.StoredProcedure, DParam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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
        public void GetAllCashReceiptNo(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptSearchList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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

        // ADD Cash Receipt
        public void AddCashReceipt(Chn_CashReceiptModel ObjCashRcpt, string xml)
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

        public void AddNewReceiptVoucher(Chn_ReceiptVoucherModel m)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoucherNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.VoucherNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = m.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = m.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Narration", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = m.Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoucherDate", MySqlDbType = MySqlDbType.DateTime, Value = DateTime.ParseExact(m.PaymentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) });
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
            Chn_ReceiptVoucherModel ObjRcptVou = null;
            List<Chn_ReceiptVoucherModel> lstRcptVou = new List<Chn_ReceiptVoucherModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRcptVou = new Chn_ReceiptVoucherModel();
                    ObjRcptVou.ReceiptId = Convert.ToInt32(Result["ReceiptId"] == DBNull.Value ? 0 : Result["ReceiptId"]);
                    ObjRcptVou.VoucherNo = Convert.ToString(Result["VoucherNo"]);
                    ObjRcptVou.PaymentDate = Convert.ToString(Result["VoucherDate"] == null ? "" : Result["VoucherDate"]);
                    ObjRcptVou.Purpose = Convert.ToString(Result["Purpose"] == null ? "" : Result["Purpose"]);
                    ObjRcptVou.Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? 0 : Result["Amount"]);
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
            InvoiceDetails objInv = new InvoiceDetails();
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
        public void AddCreditNote(ChnCreditNote objCR, string XML, string CRDR)
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
            PpgInvoiceDetails objInv = new PpgInvoiceDetails();
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
            List<PpgChargeNameCrDb> objChrgs = new List<PpgChargeNameCrDb>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objChrgs.Add(new PpgChargeNameCrDb
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
        public void AddEditCashCollectionChq(Chn_CashColAgnBncChq objCC)
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
            //var model = new Chn_CashReceiptModel();
            Chn_EditReceiptPayment objEditReceiptPayment = new Chn_EditReceiptPayment();

            List<Chn_EditReceiptPayment> lstEditReceiptPayment = new List<Chn_EditReceiptPayment>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEditReceiptPayment.Add(new Chn_EditReceiptPayment
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
            //var model = new Chn_CashReceiptModel();
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
            //var model = new Chn_CashReceiptModel();
            List<Chn_CashReceiptEditDtls> lstCashReceiptEditDtls = new List<Chn_CashReceiptEditDtls>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceiptEditDtls.Add(new Chn_CashReceiptEditDtls
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


        public void SaveEditedCashRcpt(Chn_EditReceiptPayment objEditReceiptPayment, int uid)
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
            //var model = new Chn_CashReceiptModel();
            List<Chn_CashReceiptEditDtls> lstCashReceiptEditDtls = new List<Chn_CashReceiptEditDtls>();
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

        //#region pda refund



        //public void RefundFromPDA(Chn_AddMoneyToPDModelRefund m, int Uid)
        //{

        //    string BankBranch = m.Bank + '#' + m.Branch;
        //    string GeneratedClientId = "0";
        //    string RecNo = "0";
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_RefundAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.RefundAmount });
        //    // LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_BankBranch", MySqlDbType = MySqlDbType.Text, Value = BankBranch });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeDate", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DataAccess.ExecuteNonQuery("RefundFromPDA", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Refund From SD Successfully";
        //            _DBResponse.Data = RecNo;
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

        //public void GetPartyDetailsRefund()
        //{
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetailsForRefund");
        //    IList<PPGRefundMoneyFromPD> model = new List<PPGRefundMoneyFromPD>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            model.Add(new PPGRefundMoneyFromPD
        //            {
        //                Id = Convert.ToInt32(Result["Id"]),
        //                Name = Result["Name"].ToString(),
        //                Address = Result["Address"].ToString(),
        //                Folio = Result["Folio"].ToString(),
        //                Balance = (Convert.ToDecimal(Result["Balance"])),
        //                UnPaidAmount = (Convert.ToDecimal(Result["UnPaidAmount"])),
        //                PName = Result["PName"].ToString()

        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = model;
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
        //    finally
        //    {
        //        Result.Close();
        //        Result.Dispose();
        //    }
        //}


        //public void getCompanyDetails()
        //{

        //    int Status = 0;

        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("getCompanyDetails", CommandType.StoredProcedure, DParam);
        //    CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();

        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;

        //            objCompanyDetails.RoAddress = Convert.ToString(Result["ROAddress"]).Replace("<br/>", " ");
        //            objCompanyDetails.CompanyName = Convert.ToString(Result["CompanyName"]);
        //            objCompanyDetails.CompanyAddress = Convert.ToString(Result["CompanyAddress"]).Replace("<br/>", " ");



        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objCompanyDetails;
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
        //        Result.Close();
        //        Result.Dispose();

        //    }
        //}

        //#endregion


        #region Misc. Invoice

        public void GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId, decimal Amount,string SEZ)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeId", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToInt32(purpose.Split('-')[0]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Int32, Value = Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam  = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getMiscpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_MiscInv Obj = new CHN_MiscInv();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Obj.Amount = Convert.ToDecimal(Result["SUM"]);
                    Obj.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    Obj.Total = Convert.ToDecimal(Result["Total"]);
                    Obj.Round_up = Convert.ToDecimal(Result["Round_Up"]);
                    Obj.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    Obj.SACCode = Result["SACCode"].ToString();
                    Obj.IGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                    Obj.CGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                    Obj.SGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Obj;
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
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("MiscChargesForInv", CommandType.StoredProcedure, DParam);
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPurpose.Add(new SelectListItem { Text = result["ChargesName"].ToString(), Value = result["ChargeId"].ToString() });
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
        public void AddMiscInv(CHNMiscPostModel ObjPostPaymentSheet, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_State", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.State });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Taxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Taxable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Discount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Discount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DiscountPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.DiscountPer });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
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

            LstParam.Add(new MySqlParameter { ParameterName = "IN_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SACCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.IGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_ChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ChargeName });

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
        public void GetPaymentPartyMisc()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n"," "),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo= Convert.ToString(Result["GSTNo"])
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

        public void GetInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PostPaymentSheet objPostPaymentSheet = new PostPaymentSheet();
            try
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

                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
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
                            objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
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
                            objPostPaymentSheet.lstContWiseAmount.Add(new ContainerWiseAmount()
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
                            /*
                            PreInvoiceCargo objPre = new PreInvoiceCargo();
                            objPre.BOENo = Convert.ToString(Result["BOENo"]);
                            objPre.BOEDate = Convert.ToString(Result["BOEDate"]);
                            objPre.BOLNo = Convert.ToString(Result["BOLNo"]);
                            objPre.BOLDate = Convert.ToString(Result["BOLDate"]);
                            objPre.CargoDescription = Convert.ToString(Result["BOLDate"]);
                            objPre.GodownId = Convert.ToInt32(Result["GodownId"]);
                            objPre.GodownName = Convert.ToString(Result["GodownName"]);
                            objPre.GodownWiseLocationIds = Convert.ToString(Result["GdnWiseLctnIds"]);
                            objPre.GodownWiseLctnNames = Convert.ToString(Result["GdnWiseLctnNames"]);
                            objPre.CargoType = Convert.ToInt32(Result["CargoType"]);
                            objPre.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                            objPre.CartingDate = Convert.ToString(Result["CartingDate"]);
                            objPre.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                            objPre.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                            objPre.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                            objPre.WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]);
                            objPre.SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]);
                            objPre.SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]);
                            objPre.CIFValue = Convert.ToDecimal(Result["CIFValue"]);
                            objPre.Duty = Convert.ToDecimal(Result["Duty"]);
                            objPostPaymentSheet.lstPreInvoiceCargo.Add(objPre);
                            */

                            objPostPaymentSheet.lstPreInvoiceCargo.Add(new PreInvoiceCargo()
                            {
                                BOENo = Convert.ToString(Result["BOENo"]),
                                BOEDate = Convert.ToString(Result["BOEDate"]),
                                BOLNo = Convert.ToString(Result["BOLNo"]),
                                BOLDate = Convert.ToString(Result["BOLDate"]),
                                CargoDescription = Convert.ToString(Result["BOLDate"]),
                                GodownId = Convert.ToInt32(Result["GodownId"]),
                                GodownName = Convert.ToString(Result["GodownName"]),
                                GodownWiseLocationIds = Convert.ToString(Result["GdnWiseLctnIds"]),
                                GodownWiseLctnNames = Convert.ToString(Result["GdnWiseLctnNames"]),
                                CargoType = Convert.ToInt32(Result["CargoType"]),
                                DestuffingDate = Convert.ToString(Result["DestuffingDate"]),
                                CartingDate = Convert.ToString(Result["CartingDate"]),
                                StuffingDate = Convert.ToString(Result["StuffingDate"]),
                                NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                                GrossWeight = Convert.ToDecimal(Result["GrossWt"]),
                                WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                                SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                                SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                                CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                                Duty = Convert.ToDecimal(Result["Duty"])
                            });
                        }
                    }
                    objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").ToList().ForEach(item =>
                    {
                        if (!objPostPaymentSheet.ActualApplicable.Any(i => i == item.Clause))
                            objPostPaymentSheet.ActualApplicable.Add(item.Clause);
                    });
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

        public void GetBondInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BondPostPaymentSheet objBondPostPaymentSheet = new BondPostPaymentSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBondPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objBondPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                    // objBondPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                    objBondPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                    objBondPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                    objBondPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                    objBondPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                    objBondPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objBondPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                    objBondPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objBondPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                    objBondPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                    objBondPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                    objBondPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                    objBondPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                    objBondPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    objBondPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    objBondPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    objBondPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    objBondPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    objBondPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    objBondPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    objBondPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    objBondPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                    objBondPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                    objBondPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    objBondPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    objBondPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                    objBondPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    objBondPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    objBondPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    objBondPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    //  objBondPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                    objBondPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                    objBondPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                    objBondPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                    objBondPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                    objBondPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    objBondPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    objBondPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    //  objBondPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    objBondPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                    //  objBondPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    objBondPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                    objBondPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                    objBondPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                    objBondPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                    //objBondPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                    //objBondPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                    //objBondPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                    //objBondPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                    //objBondPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objBondPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                    objBondPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    //  objBondPostPaymentSheet.Module = Convert.ToString(Result["Module"]);

                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = objBondPostPaymentSheet.lstPostPaymentChrg.Count + 1,
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

                    objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").ToList().ForEach(item =>
                    {
                        if (!objBondPostPaymentSheet.ActualApplicable.Any(i => i == item.Clause))
                            objBondPostPaymentSheet.ActualApplicable.Add(item.Clause);
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBondPostPaymentSheet;
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
            Chn_FumigationInvoice ObjEntryThroughGate = new Chn_FumigationInvoice();
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
        public void AddEditFumigationInv(Chn_FumigationInvoice ObjPostPaymentSheet, String ChemicalData, int BranchId, int Uid, string Module)
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


        public void AddEditRentInv(PPG_RentInvoice ObjPostPaymentSheet, String RentData, int BranchId, int Uid, string month, int year, string Module)
        {
            string GeneratedClientId = "0";
            String mon = month.Substring(0, 3);
            int yr = year;
            // string dt = Convert.ToDateTime(ObjPostPaymentSheet.Date).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RentlXML", MySqlDbType = MySqlDbType.Text, Value = RentData });

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



        public void GetRentDet(string Mon, int Yr, int flg)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = Mon });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Yr });
            LstParam.Add(new MySqlParameter { ParameterName = "flag", MySqlDbType = MySqlDbType.Int32, Value = flg });

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


        public void SaveChn_CashDepositToBank(Chn_CashDepositToBank obj)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Chn_CashDepositToBank_Id", MySqlDbType = MySqlDbType.Int32, Value = obj.Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankId", MySqlDbType = MySqlDbType.Int32, Value = obj.BankId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BankAccountNo", MySqlDbType = MySqlDbType.VarChar, Value = obj.BankAccountNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(obj.DepositDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAmount", MySqlDbType = MySqlDbType.Decimal, Value = obj.DepositAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalanceAmount", MySqlDbType = MySqlDbType.Decimal, Value = obj.BalanceAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("SaveChn_CashDepositToBank", CommandType.StoredProcedure, DParam);
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
            Chn_DebitInvoice ObjEntryThroughGate = new Chn_DebitInvoice();
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

        public void AddEditDebitInvoice(Chn_DebitInvoice ObjPostPaymentSheet, String ChemicalData, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
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
            var model = new Chn_CashReceiptModel();
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

            var model = new Chn_CashReceiptModel();
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
        public void AddCashReceiptSD(Chn_CashReceiptModel ObjCashRcpt, string xml)
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
        public void GetCashReceiptSDList(int Page)
        {

           
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = lstParam.ToArray();
             DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptSDList", CommandType.StoredProcedure, DParam);
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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
        public void GetAllReceiptNo(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value =ReceiptNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptSDSearchList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Chn_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
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
        #region pda refund



        public void RefundFromPDA(ChnAddMoneyToPDModelRefund m, int Uid)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_NEFTNo", MySqlDbType = MySqlDbType.VarChar, Value = m.NEFT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NEFTDate", MySqlDbType = MySqlDbType.VarChar, Value = m.NEFTDate });
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
            IList<Chn_RefundMoneyFromPD> model = new List<Chn_RefundMoneyFromPD>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new Chn_RefundMoneyFromPD
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
            ChnViewSDRefund ObjSD = new ChnViewSDRefund();
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
                     ObjSD.NEFT= Convert.ToString(result["NEFTNo"]);
                    ObjSD.NEFTDate = Convert.ToString(result["NEFTDate"]);

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
            List<ChnSDRefundList> SdList = new List<ChnSDRefundList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    SdList.Add(new ChnSDRefundList
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

        #region Cancel Invoice
        public void GetHeaderIRNForInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

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

        public void ListOfCancleInvoice(string InvoiceNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCancleInvoiceNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CancelInvoice> lstInvoice = new List<CancelInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new CancelInvoice()
                    {

                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
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
        public void DetailsOfCancleInvoice(int invoiceid = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = invoiceid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsOfCancleInvoiceNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CancelInvoice objcancelinv = new CancelInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objcancelinv.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objcancelinv.InvoiceNo = (Result["InvoiceNo"]).ToString();
                    objcancelinv.InvoiceDate = (Result["InvoiceDate"]).ToString();
                    objcancelinv.PartyName = (Result["PartyName"]).ToString();
                    objcancelinv.Amount = Convert.ToDecimal(Result["Amount"]);
                    objcancelinv.Irn = Convert.ToString(Result["irn"]);
                    objcancelinv.SupplyType = Convert.ToString(Result["SupplyType"]);
                    objcancelinv.AckNo = Convert.ToString(Result["AckNo"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objcancelinv;
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

        public void ViewDetailsOfCancleInvoice(int invoiceid = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = invoiceid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewDetailsOfCancleInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CancelInvoice objcancelinv = new CancelInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objcancelinv.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objcancelinv.InvoiceNo = (Result["InvoiceNo"]).ToString();
                    objcancelinv.InvoiceDate = (Result["InvoiceDate"]).ToString();
                    objcancelinv.PartyName = (Result["PartyName"]).ToString();
                    objcancelinv.Amount = Convert.ToDecimal(Result["Amount"]);
                    objcancelinv.Irn = Convert.ToString(Result["irn"]);
                    objcancelinv.CancelRemarks = Convert.ToString(Result["CancelRemarks"]);
                    objcancelinv.CancelReason = Convert.ToString(Result["CancelReason"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objcancelinv;
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
        public void LstOfCancleInvoice(string InvoiceNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CancleInvoiceList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CancelInvoice> lstInvoice = new List<CancelInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new CancelInvoice()
                    {


                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString(),
                        InvoiceDate = (Result["InvoiceDate"]).ToString(),
                        PartyName = (Result["PartyName"]).ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CancelDate = (Result["CancelDate"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
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


        public void AddEditCancleInvoice(CancelInvoice ObjCancelInvoice, int uid)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjCancelInvoice.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelReason", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelReason });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelRemarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelRemarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SupplyType", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.SupplyType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCancleInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Invoice Cancelled Saved Successfully-" + GeneratedClientId;
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

        public void GetIRNForDebitCredit(String CRNoteNo, String TypeOfInv, String CRDR)
        {
            EinvoicePayload Obj = new EinvoicePayload();

            TranDtls tr = new TranDtls();
            DocDtls doc = new DocDtls();
            SellerDtls seller = new SellerDtls();
            BuyerDtls buyer = new BuyerDtls();
            DispDtls disp = new DispDtls();

            ShipDtls ship = new ShipDtls();
            BchDtls btc = new BchDtls();
            AttribDtls attr = new AttribDtls();
            ValDtls vald = new ValDtls();
            PayDtls payd = new PayDtls();
            RefDtls refd = new RefDtls();
            PrecDocDtls pred = new PrecDocDtls();
            ContrDtls Cont = new ContrDtls();
            AddlDocDtls addl = new AddlDocDtls();
            ExpDtls expd = new ExpDtls();
            EwbDtls ewb = new EwbDtls();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CRNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.Version = Result["Version"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        tr.TaxSch = Result["TaxSch"].ToString();
                        tr.SupTyp = Result["SupTyp"].ToString();
                        tr.RegRev = Result["RegRev"].ToString();
                        tr.EcmGstin = null;

                        tr.IgstOnIntra = Result["IgstOnIntra"].ToString();

                    }
                }




                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        doc.Typ = Result["Typ"].ToString();
                        doc.No = Result["No"].ToString();
                        doc.Dt = Result["Dt"].ToString();


                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        seller.Gstin = Result["Gstin"].ToString();
                        seller.LglNm = Result["LglNm"].ToString();
                        seller.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        seller.Addr1 = Result["Addr1"].ToString();
                        seller.Addr2 = null;
                        seller.Loc = Result["Loc"].ToString();
                        seller.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        seller.Stcd = Result["Stcd"].ToString();
                        seller.Ph = null;// Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        seller.Em = null;//Result["Em"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        buyer.Gstin = Result["Gstin"].ToString();
                        buyer.LglNm = Result["LglNm"].ToString();
                        buyer.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        buyer.Addr1 = Result["Addr1"].ToString();
                        buyer.Addr2 = null;
                        buyer.Pos = Convert.ToString(Result["Stcd"]);
                        buyer.Loc = Result["Loc"].ToString();
                        buyer.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        buyer.Stcd = Result["Stcd"].ToString();
                        buyer.Ph = null;//Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        buyer.Em = null; //Result["Em"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        disp.Nm = Result["Nm"].ToString();
                        disp.Addr1 = Result["Addr1"].ToString();
                        disp.Addr2 = null;
                        disp.Loc = Result["Loc"].ToString();
                        disp.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        disp.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ship.Gstin = Result["Gstin"].ToString();
                        ship.LglNm = Result["LglNm"].ToString();
                        ship.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        ship.Addr1 = Result["Addr1"].ToString();
                        ship.Addr2 = null;
                        ship.Loc = Result["Loc"].ToString();
                        ship.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        ship.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        btc.Nm = Result["Nm"].ToString();
                        btc.ExpDt = Result["ExpDt"].ToString();
                        btc.WrDt = Result["WrDt"].ToString();
                        // attr.Nm = Result["Nmm"].ToString();
                        // attr.Val= Result["Val"].ToString();
                        attr = null;
                        Obj.ItemList.Add(new ItemList
                        {
                            SlNo = Result["SlNo"].ToString(),
                            PrdDesc = null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit =null,// Result["Unit"].ToString(),
                            UnitPrice = Convert.ToDecimal(Result["UnitPrice"].ToString()),
                            TotAmt = Convert.ToDecimal(Result["TotAmt"].ToString()),
                            Discount = Convert.ToInt32(Result["Discount"].ToString()),
                            PreTaxVal = Convert.ToInt32(Result["PreTaxVal"].ToString()),
                            AssAmt = Convert.ToDecimal(Result["AssAmt"].ToString()),
                            GstRt = Convert.ToDecimal(Result["GstRt"].ToString()),
                            IgstAmt = Convert.ToDecimal(Result["IgstAmt"].ToString()),
                            CgstAmt = Convert.ToDecimal(Result["CgstAmt"].ToString()),
                            SgstAmt = Convert.ToDecimal(Result["SgstAmt"].ToString()),
                            CesRt = Convert.ToInt32(Result["CesRt"].ToString()),
                            CesAmt = Convert.ToDecimal(Result["CesAmt"].ToString()),
                            CesNonAdvlAmt = Convert.ToInt32(Result["CesNonAdvlAmt"].ToString()),
                            StateCesRt = Convert.ToInt32(Result["StateCesRt"].ToString()),
                            StateCesAmt = Convert.ToDecimal(Result["StateCesAmt"].ToString()),
                            StateCesNonAdvlAmt = Convert.ToInt32(Result["StateCesNonAdvlAmt"].ToString()),
                            OthChrg = Convert.ToInt32(Result["OthChrg"].ToString()),
                            TotItemVal = Convert.ToDecimal(Result["TotItemVal"].ToString()),
                            OrdLineRef = Convert.ToString(Result["OrdLineRef"].ToString()),
                            OrgCntry = Result["OrgCntry"].ToString(),
                            PrdSlNo = Convert.ToString(Result["PrdSlNo"].ToString()),
                            BchDtls = null,
                            AttribDtls = null,

                        });



                    }


                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        vald.AssVal = Convert.ToDecimal(Result["AssVal"].ToString());
                        vald.CgstVal = Convert.ToDecimal(Result["CgstVal"].ToString());
                        vald.SgstVal = Convert.ToDecimal(Result["SgstVal"].ToString());
                        vald.IgstVal = Convert.ToDecimal(Result["IgstVal"].ToString());
                        vald.CesVal = Convert.ToDecimal(Result["CesVal"].ToString());
                        vald.StCesVal = Convert.ToDecimal(Result["StCesVal"].ToString());
                        vald.Discount = Convert.ToDecimal(Result["Discount"].ToString());
                        vald.OthChrg = Convert.ToDecimal(Result["OthChrg"].ToString());
                        vald.RndOffAmt = Convert.ToDecimal(Result["RndOffAmt"].ToString());
                        vald.TotInvVal = Convert.ToDecimal(Result["TotInvVal"].ToString());
                        vald.TotInvValFc = Convert.ToDecimal(Result["TotInvValFc"].ToString());


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        payd.Nm = Result["Nm"].ToString();
                        payd.AccDet = Result["AccDet"].ToString();
                        payd.Mode = Result["Mode"].ToString();
                        payd.FinInsBr = Result["FinInsBr"].ToString();
                        payd.PayTerm = Result["PayTerm"].ToString();
                        payd.PayInstr = Result["PayInstr"].ToString();
                        payd.CrTrn = Result["CrTrn"].ToString();
                        payd.DirDr = Result["DirDr"].ToString();
                        payd.CrDay = Result["CrDay"].ToString();
                        payd.PaidAmt = Result["PaidAmt"].ToString();
                        payd.PaymtDue = Result["PaymtDue"].ToString();


                    }
                }


                if (Result.NextResult())
                {
                    DocPerdDtls docp = new DocPerdDtls();
                    while (Result.Read())
                    {
                        refd.InvRm = Result["InvRm"].ToString();
                        docp.InvStDt = Result["InvStDt"].ToString();
                        docp.InvEndDt = Result["InvEndDt"].ToString();
                        refd.DocPerdDtls = docp;


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        pred.InvNo = Result["InvNo"].ToString();
                        pred.InvDt = Result["InvDt"].ToString();
                        pred.OthRefNo = Result["OthRefNo"].ToString();


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Cont.RecAdvRefr = Result["RecAdvRefr"].ToString();
                        Cont.RecAdvDt = Result["RecAdvDt"].ToString();
                        Cont.TendRefr = Result["TendRefr"].ToString();
                        Cont.ContrRefr = Result["ContrRefr"].ToString();
                        Cont.ExtRefr = Result["ExtRefr"].ToString();
                        Cont.ProjRefr = Result["ProjRefr"].ToString();
                        Cont.PORefr = Result["PORefr"].ToString();
                        Cont.PORefDt = Result["PORefDt"].ToString();



                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        addl.Url = Result["Url"].ToString();
                        addl.Docs = Result["Docs"].ToString();
                        addl.Info = Result["Info"].ToString();




                    }
                }

                if (Result.NextResult())
                {
                    // while (Result.Read())
                    // {



                    // Obj.ExpDtls.Add(new ExpDtls
                    // {
                    // ShipBNo = Result["ShipBNo"].ToString(),
                    // ShipBDt = Result["ShipBDt"].ToString(),
                    // Port = Result["Port"].ToString(),
                    // RefClm = Result["RefClm"].ToString(),
                    // ForCur = Result["ForCur"].ToString(),
                    // CntCode = Result["CntCode"].ToString(),
                    // ExpDuty = Result["ExpDuty"].ToString(),


                    // });












                    // }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ewb.TransId = Result["TransId"].ToString();
                        ewb.TransName = Result["TransName"].ToString();
                        ewb.Distance = Result["Distance"].ToString();
                        ewb.TransDocNo = Result["TransDocNo"].ToString();
                        ewb.TransDocDt = Result["TransDocDt"].ToString();
                        ewb.VehNo = Result["VehNo"].ToString();
                        ewb.VehType = Result["VehType"].ToString();
                        ewb.TransMode = Result["TransMode"].ToString();




                    }
                }

                //if (Result.NextResult())
                //{
                // while (Result.Read())
                // {
                // hp.ClientID = Result["ClientID"].ToString();
                // hp.ClientSecret = Result["ClientSecret"].ToString();
                // hp.GSTIN = Result["GSTIN"].ToString();
                // hp.UserName = Result["UserName"].ToString();
                // hp.Password = Result["Password"].ToString();
                // hp.AppKey = "";



                // }
                //}
                Obj.TranDtls = tr;
                Obj.DocDtls = doc;
                Obj.SellerDtls = seller;
                Obj.BuyerDtls = buyer;
                Obj.DispDtls = disp;
                Obj.ShipDtls = ship;
                Obj.AttribDtls = attr;
                Obj.ValDtls = vald;
                Obj.PayDtls = null;
                Obj.RefDtls = null;
                Obj.PrecDocDtls = pred;
                Obj.ContrDtls = Cont;
                Obj.AddlDocDtls = null;
                Obj.ExpDtls = null;
                // Obj.ExpDtls = expd;
                Obj.EwbDtls = null;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

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
        public void GetIRNForB2CInvoiceForCreditDebitNote(String CrNoteNo, String TypeOfInv, String CRDR)
        {
            QrCodeData Obj = new QrCodeData();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.SellerGstin = Result["SellerGST"].ToString();
                    Obj.BuyerGstin = Result["BuyerGstin"].ToString();
                    Obj.DocNo = Result["DocNo"].ToString();
                    Obj.DocTyp = Result["DocTyp"].ToString();
                    Obj.DocDt = Result["DocDt"].ToString();
                    Obj.TotInvVal = (Int32)Convert.ToDecimal(Result["TotInvVal"].ToString());
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"].ToString());
                    Obj.MainHsnCode = Result["MainHsnCode"].ToString();
                    //  Obj.Irn = Result["Irn"].ToString();
                    //   Obj.IrnDt = Result["IrnDt"].ToString();
                    //  Obj.iss = Result["iss"].ToString();


                }
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

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

        public void GetIRNForB2CInvoiceForCreditDebitNoteDR(String CrNoteNo, String TypeOfInv, String CRDR)
        {
            CHN_IrnB2CDetails Obj = new CHN_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.SellerGstin = Result["SellerGST"].ToString();
                    Obj.BuyerGstin = Result["BuyerGstin"].ToString();
                    Obj.DocNo = Result["DocNo"].ToString();
                    Obj.DocTyp = Result["DocTyp"].ToString();
                    Obj.DocDt = Result["DocDt"].ToString();
                    Obj.TotInvVal = (Int32)Convert.ToDecimal(Result["TotInvVal"].ToString());
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"].ToString());
                    Obj.MainHsnCode = Result["MainHsnCode"].ToString();
                    //  Obj.Irn = Result["Irn"].ToString();
                    //   Obj.IrnDt = Result["IrnDt"].ToString();
                    //  Obj.iss = Result["iss"].ToString();
                    Obj.ver = Convert.ToString(Result["ver"]);

                    Obj.tier = Convert.ToString(Result["tier"]);
                    Obj.tid = Convert.ToString(Result["tid"]);
                    //Obj.sign = Convert.ToString(Result["sign"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    Obj.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    Obj.CESS = 0;

                    Obj.qrMedium = Convert.ToInt32(Result["qrMedium"]);
                    Obj.QRexpire = Convert.ToString(Result["QRexpireDays"]);

                    Obj.pinCode = Convert.ToInt32(Result["pinCode"]);
                    Obj.pa = Convert.ToString(Result["pa"]);
                    Obj.orgId = Convert.ToInt32(Result["orgId"]);
                    Obj.mtid = Convert.ToString(Result["mtid"]);
                    Obj.msid = Convert.ToString(Result["msid"]);
                    Obj.mode = Convert.ToInt32(Result["mode"]);
                    Obj.mc = Convert.ToString(Result["mc"]);
                    Obj.mam = Convert.ToString(Result["InvoiceAmt"]);
                    Obj.GSTPCT = Convert.ToInt32(Result["IGSTPer"]);
                    Obj.GSTIncentive = 0;
                    Obj.mid = Convert.ToString(Result["mid"]);
                    Obj.InvoiceName = Convert.ToString(Result["PartyName"]);
                    Obj.tr = Convert.ToString(Result["tr"]);
                    Obj.pn = Convert.ToString(Result["pn"]);
                    Obj.gstIn = Convert.ToString(Result["SellerGST"]);

                }
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

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
        public void AddEditIRNResponsecForCreditDebitNote(IrnResponse objOBL, string CrNoteNo, string CRDR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckNo", MySqlDbType = MySqlDbType.String, Value = objOBL.AckNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckDt", MySqlDbType = MySqlDbType.String, Value = objOBL.AckDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = objOBL.irn }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedInvoice", MySqlDbType = MySqlDbType.String, Value = objOBL.SignedInvoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCode", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.SignedQRCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = objOBL.QRCodeImageBase64 });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnStatus", MySqlDbType = MySqlDbType.String, Value = objOBL.IrnStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbNo", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.EwbNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbDt", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbValidTill", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbValidTill });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnResponsecol", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("Addeditirnresponcecrdr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Saved Successfully" : "IRN Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else
                {

                    log.Info("After Calling Stored Procedure Error" + " Credit Note No " + CrNoteNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);

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

        public void AddEditIRNB2CCreditDebitNote(String IRN, B2cQRCodeResponse QrCode, string CrNoteNo, String CRDR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = IRN });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodesigned", MySqlDbType = MySqlDbType.LongText, Value = QrCode.QrCodeJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnB2CresponceCreditDebit", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Saved Successfully" : "OBL Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Duplicate OBL No.!";
                    _DBResponse.Status = Result;
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

        public void GetHeaderIRNForCreditDebitNote()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }



                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

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




        #region Reservation Invoice Individual

        public void GetGodownListforReservation(String SpaceType)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = SpaceType });
            IDataParameter[] Dparam = lstParam.ToArray();

            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownFoReservationInv", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            List<CHN_Godown> LstStuffing = new List<CHN_Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new CHN_Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"].ToString()),
                        GodownName = Result["GodownName"].ToString(),
                        OperationType = Result["OperationType"].ToString(),


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



        //public void ListOfChargesNameForReservation()
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetCharges", CommandType.StoredProcedure);
        //    _DBResponse = new DatabaseResponse();
        //    List<CHN_charges> LstStuffing = new List<CHN_charges>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstStuffing.Add(new CHN_charges
        //            {
        //                ChargeId = Convert.ToInt32(Result["OperationId"].ToString()),
        //                ChargeName = Result["OperationSDesc"].ToString()


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

        public void GetAmountForChargesForReservation(int cid, String cname, String size, string Fromdt, string todt)
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
        public void GetInvoiceForReservation(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges)
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

        public void AddEditInvoiceReservationIndividual(CHN_ReservationIndividual obj, string dtlsXml, int BranchId, int userid, string module)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();


            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(obj.DeliveryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = obj.PartyId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = obj.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = obj.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = obj.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GstNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = obj.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.Int32, Value = obj.StateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundOff", MySqlDbType = MySqlDbType.Decimal, Value = obj.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = obj.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = obj.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpace", MySqlDbType = MySqlDbType.Decimal, Value = obj.SQM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = obj.GodownType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = obj.SpaceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From_Date", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = obj.ToDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Taxable", MySqlDbType = MySqlDbType.Decimal, Value = obj.Total });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = obj.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = obj.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = obj.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceXml", MySqlDbType = MySqlDbType.VarChar, Value = dtlsXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Userid", MySqlDbType = MySqlDbType.Int32, Value = userid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = obj.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LongTerm", MySqlDbType = MySqlDbType.Int32, Value = obj.Term });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = obj.SezValue });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceReservationIndividual", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
                    _DBResponse.Message = "Cannot Update Reservation Individual Invoice Details As It Already Exist In Another Page";

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


        public void GetPartyListforReservation(string PartyCode, int Gid, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Gid });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstPartyForReservation", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            WFLD_SearchEximTraderReservation LstEximTrader = new WFLD_SearchEximTraderReservation();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstExim.Add(new WfldPartyDet
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"]),
                        PartyName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        GstNo = Result["GSTNo"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StateName = Result["StateName"].ToString()
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

        public void GetPayeeListReservation(string PartyCode, int Page = 0)
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



        public void CreateReservationInvoicesIndividual(string DeliveryDate, string SpaceType, int Godown_Id, string Godown_Name, string Godown_Type, string From_Date, string To_Date, decimal SQM, int PartyId, string CargoType, string SEZ, int longTerm = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = SpaceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Godown_Id", MySqlDbType = MySqlDbType.Int32, Value = Godown_Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Godown_Name", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = Godown_Name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Godown_Type", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Godown_Type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(From_Date).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(To_Date).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SQM", MySqlDbType = MySqlDbType.Decimal, Value = SQM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(CargoType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LongTerm", MySqlDbType = MySqlDbType.Int32, Value = longTerm });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GenResIndividualInvoices", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<WFLD_ReservationDetails> objResInvoices = new List<WFLD_ReservationDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objResInvoices.Add(new WFLD_ReservationDetails()
                    {
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        TotalSpace = Convert.ToDecimal(Result["TotalSpace"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        GstNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        ComStateCode = Convert.ToString(Result["ComStateCode"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        RoundOff = Convert.ToDecimal(Result["RoundOff"]),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmount"]),
                        InvoiceNo = "",//Result["InvoiceNo"].ToString(),

                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),

                        SacCode = Convert.ToString(Result["SacCode"]),
                        Rate = Convert.ToDecimal(Result["Rate"]),

                        OperationType = Convert.ToString(Result["OperationType"]),
                        //IsCashReceipt = Convert.ToInt32(Result["IsCashReceipt"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objResInvoices;
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