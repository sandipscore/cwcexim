using CwcExim.Areas.CashManagement.Models;
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
    public class Kol_CashManagementRepository
    {
        private DatabaseResponse _DBResponse;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public void AddNewPaymentVoucher(Kol_NewPaymentValucherModel m)
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
            int Result = DataAccess.ExecuteNonQuery("AddEditPaymentVoucher", CommandType.StoredProcedure, DParam, out GeneratedClientId,out VoucherNo);
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
                    var data = new { VoucherNo = VoucherNo,GeneratedClientId };
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
            Kol_NewPaymentValucherModel ObjPaymentVou = null;
            List<Kol_NewPaymentValucherModel> lstPaymentVou = new List<Kol_NewPaymentValucherModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPaymentVou = new Kol_NewPaymentValucherModel();
                    ObjPaymentVou.PVHeadId = Convert.ToInt32(Result["PVHeadId"] == DBNull.Value ? 0 : Result["PVHeadId"]);
                    ObjPaymentVou.VoucherNo = Convert.ToString(Result["VoucherNo"]);
                    ObjPaymentVou.PaymentDate = Convert.ToString(Result["VoucherDate"] == null ? "" : Result["VoucherDate"]);
                    ObjPaymentVou.TotalAmount = Convert.ToDecimal(Result["TotalAmount"] == DBNull.Value ? 0 : Result["TotalAmount"]);
                    ObjPaymentVou.Purpose = Convert.ToString(Result["Purpose"]);

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
            Kol_NewPaymentValucherModel LstSeal = new Kol_NewPaymentValucherModel();
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
                    LstSeal.Purpose = Convert.ToString(Result["Purpose"] == null ? "" : Result["Purpose"]);
                }
                 
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstSeal.expcharges.Add(new ExpensesDetails
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
                    model.Add(new PartyDetails { Id = Convert.ToInt32(Result["Id"]), Name = Result["Name"].ToString(), Address=Result["Address"].ToString(), Folio = Result["Folio"].ToString(), Balance = Convert.ToDecimal(Result["Balance"]) });
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
        public void GetAddmoneyToPDList(string SearchValue,int Pages=0)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
          
            IDataReader Result = DataAccess.ExecuteDataReader("GetAddMoneyToPDList", CommandType.StoredProcedure, DParam);
            List<Kdl_AddMoneyToPDList> lstAddMoneyToPDList = new List<Kdl_AddMoneyToPDList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstAddMoneyToPDList.Add(new Kdl_AddMoneyToPDList
                    {
                        PdaTransDate = Convert.ToString(Result["PdaTransDate"]),
                        PdaTranRecNo = Convert.ToString(Result["PdaTranRecNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        Amount = Convert.ToString(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        FolioNo = Convert.ToString(Result["FolioNo"]),
                        PdaChequeDate = Convert.ToString(Result["PdaChequeDate"]),
                        PdaDrawBank = Convert.ToString(Result["PdaDrawBank"]),
                        PdaInsNo = Convert.ToString(Result["PdaInsNo"]),
                        PdaPayType = Convert.ToString(Result["PdaPayType"]),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAddMoneyToPDList;
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

        public void GetAddmoneyToPDListByReceiptNo(string ReceiptNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
          //  lstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            IDataReader Result = DataAccess.ExecuteDataReader("GetAddMoneyToPDListByReceiptNo", CommandType.StoredProcedure, DParam);
            List<Kdl_AddMoneyToPDList> lstAddMoneyToPDList = new List<Kdl_AddMoneyToPDList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstAddMoneyToPDList.Add(new Kdl_AddMoneyToPDList
                    {
                        PdaTransDate = Convert.ToString(Result["PdaTransDate"]),
                        PdaTranRecNo = Convert.ToString(Result["PdaTranRecNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        Amount = Convert.ToString(Result["Amount"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        FolioNo = Convert.ToString(Result["FolioNo"]),
                        PdaChequeDate = Convert.ToString(Result["PdaChequeDate"]),
                        PdaDrawBank = Convert.ToString(Result["PdaDrawBank"]),
                        PdaInsNo = Convert.ToString(Result["PdaInsNo"]),
                        PdaPayType = Convert.ToString(Result["PdaPayType"]),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAddMoneyToPDList;
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
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId });
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
                    _DBResponse.Message = "Money Added To PD Successfully";                   
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
        // Get Invoice List
        public void GetInvoiceList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptInvoiceNo", CommandType.StoredProcedure, DParam);
            var model = new Kol_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.InvoiceDetail.Add(new Invoice { InvoiceId = Convert.ToInt32(Result["InvoiceId"]), InvoiceNo = Convert.ToString(Result["InvoiceNo"]) });
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

        // Get All Details Against Invoice No
        public void GetCashRcptDetails(int InvoiceId, string InvoiceNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDetailsAgnstInvoice", CommandType.StoredProcedure, DParam);
            var model = new Kol_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Result["PartyName"].ToString();
                    // model.PayBy= Result["PartyName"].ToString();
                    model.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                    model.InvoiceId = InvoiceId;
                    model.InvoiceNo = InvoiceNo;
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    model.TotalCwcCharges = Convert.ToDecimal(Result["TotalCwcCharges"]);
                    model.TotalHTCharges = Convert.ToDecimal(Result["TotalHTCharges"]);
                    model.TotalValue = Convert.ToDecimal(Result["TotCwcHtCharges"]);
                    model.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    model.InvoiceValue = Convert.ToDecimal(Result["InvoiceValue"]);
                    model.GSTNo = Result["GSTNo"].ToString();
                    model.PayByTraderId = Convert.ToInt32(Result["PayByTraderId"]);
                    model.PayBy = Convert.ToString(Result["PayByName"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
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
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.PdaAdjustdetail.Add(new PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CWCChargeType.Add(new CwcChargesType
                        {
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
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
                        model.HTChargeType.Add(new HTChargesType
                        {
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
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
        public void AddCashReceipt(Kol_CashReceiptModel ObjCashRcpt, string xml)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PayByPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjust", MySqlDbType = MySqlDbType.Int16, Value = ObjCashRcpt.PdaAdjust == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Adjusted) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaClosing", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Closing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TotalPaymentReceipt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TdsAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.InvoiceValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjCashRcpt.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size=500, Value = ObjCashRcpt.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_xml", MySqlDbType = MySqlDbType.VarChar, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddCashReceipt", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Add Cash Receipt Successfully.";
                    _DBResponse.Data = ReceiptNo;

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
                else if (Result == 3)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Already Cash Receipt generated Successfully.";
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
        public void GetCashRcptPrint(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptPrint", CommandType.StoredProcedure, DParam);
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
                    model.BOEDate= Result["BOEDate"].ToString();
                    model.InvoiceHtml = Result["InvoiceHtml"].ToString();
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
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                IDataReader Result = DataAccess.ExecuteDataReader("SELECT (MAX(ReceiptId) + 1) AS Id FROM receiptvoucher");
                while (Result.Read())
                {
                    id = Convert.ToInt32(Result["Id"]);
                }
            }
            catch (Exception ex)
            {
            }
            return "CWC/RV/" + id.ToString().PadLeft(6, '0') + "/" + DateTime.Today.Year.ToString();
        }

        public void AddNewReceiptVoucher(ReceiptVoucherModel m)
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
            ReceiptVoucherModel ObjRcptVou = null;
            List<ReceiptVoucherModel> lstRcptVou = new List<ReceiptVoucherModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjRcptVou = new ReceiptVoucherModel();
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1,Value=CRDR });
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
                    objInv.SupplyType = Result["SupplyType"].ToString();
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
        public void AddCreditNote(CreditNote objCR, string XML,string CRDR)
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
                    string html=GenerateHtmlForCRNote(Convert.ToInt32(Id), CRDR);
                    html=html.Replace("<br/>", "|_br_|");
                    string base64str=Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(html));
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
                        objCR.Remarks = Convert.ToString(Result["Remarks"]);
                        objCR.irn = Result["irn"].ToString();
                        objCR.SignedQRCode = Result["SignedQRCode"].ToString();
                        objCR.SupplyType = Result["SupplyType"].ToString();
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
        public string GenerateHtmlForCRNote(int CRNoteId,string CRDR)
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
                html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>"+ note +"</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceDate + "</span></td></tr><tr><td colspan='2'>";
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
            }
            catch(Exception ex)
            {

            }
            return html;
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
        #endregion

        #region Cash Collection Against Bounced Cheque
        public void AddEditCashCollectionChq(CashColAgnBncChq objCC)
        {
            string Status = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashCollectionId", MySqlDbType = MySqlDbType.Int32, Value = objCC.CashColAgnBncChqId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashCollectionDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCC.CashColAgnBncChqDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpChaId", MySqlDbType = MySqlDbType.Int32, Value = objCC.ImpExpChaId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpChaName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objCC.ImpExpChaName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeNo", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objCC.BncChequeNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.BncChequeDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BncChequeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCC.BncChequeAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BounceDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCC.BncChequeBounceDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChequeDDNo", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = objCC.ChequeDDNo });
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
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCashCollectionChq", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    _DBResponse.Data = Convert.ToInt32(Status);
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
                        ReceiptNo= Result["ReceiptNo"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PayByPdaId = Convert.ToInt32(Result["PayByPdaId"]),
                        PDAAdjusted= Convert.ToDecimal(Result["PdaAdjustedAmount"]),
                        TotalPaymentReceipt = Convert.ToDecimal(Result["TotalPaymentReceipt"]),
                        InvoiceId= Convert.ToInt32(Result["InvoiceId"])

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
                        EximTraderName= Result["EximTraderName"].ToString(),
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
                        PaymentMode= Result["PaymentMode"].ToString(),
                        DraweeBank= Result["DraweeBank"].ToString(),
                        InstrumentNo= Result["InstrumentNo"].ToString(),
                        Date= Result["Date"].ToString(),
                        Amount= Convert.ToDecimal(Result["Amount"] == DBNull.Value ? "" : Result["Amount"])
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



        public void RefundFromPDA(AddMoneyToPDModelRefund m, int Uid)
        {

            string BankBranch = m.Bank + '#' + m.Branch;
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_RefundAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.RefundAmount });
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
                    _DBResponse.Message = "Refund From PDA Successfully";
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
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetails");
            IList<RefundMoneyFromPD> model = new List<RefundMoneyFromPD>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new RefundMoneyFromPD
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Name = Result["Name"].ToString(),
                        Address = Result["Address"].ToString(),
                        Folio = Result["Folio"].ToString(),
                        Balance = Convert.ToDecimal(Result["Balance"])
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

        #region IRN for Debit & Credit Note
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
                            Unit =null, //Result["Unit"].ToString(),
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
        #endregion

        #region Misc. Invoice

        public void GetMiscInvoiceAmount(string purpose, string purposecode, string SAC, float GST, string InvoiceType,string SEZ, int PartyId, decimal Amount)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Charge", MySqlDbType = MySqlDbType.VarChar, Value = purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeCode", MySqlDbType = MySqlDbType.VarChar, Value = purposecode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = SAC });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GST", MySqlDbType = MySqlDbType.Float, Value = GST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getMiscpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Kol_MiscInv Obj = new Kol_MiscInv();
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

        public void GetMiscAmount(string purpose, string size)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_chargeName", MySqlDbType = MySqlDbType.VarChar, Value = purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = size });
            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("MiscChargesAmountForInv", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Kol_MiscInv Obj = new Kol_MiscInv();
            try
            {
                Obj.Amount = 0;
                while (Result.Read())
                {
                    Status = 1;
                    Obj.Amount = Convert.ToDecimal(Result["Amount"]);
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
                    _DBResponse.Data = Obj;
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
                    lstPurpose.Add(new SelectListItem { Text = result["ChargeName"].ToString(), Value = result["SacCode"].ToString() });
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


        public void SACForMiscInvc()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("MiscSacForInv", CommandType.StoredProcedure, DParam);
            List<SelectListItem> lstSac = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstSac.Add(new SelectListItem { Text = result["SacCode"].ToString(), Value = result["SacCode"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstSac;
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
        public void AddMiscInv(KolMiscPostModel ObjPostPaymentSheet, int BranchId, int Uid, string Module,string ExportUnder)
        {
            string GeneratedClientId = "0";
            string Deldt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            string Invdt = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Deldt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Invdt) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_State", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.State });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PurposeCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PurposeCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml});

            LstParam.Add(new MySqlParameter { ParameterName = "IN_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SACCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.IGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_ChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ChargeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

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

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice Date should be Greater than or equal to already generated Invoice Date";
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
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Amount Should Be Greater Than 0";
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
        public void GetPaymentPartyMisc(int ptype)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ptype", MySqlDbType = MySqlDbType.Int32, Value = ptype });
            IDataParameter[] DParam = LstParam.ToArray();

            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPaymentPartyForMisc", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<KolPaymentPartyName> objPaymentPartyName = new List<KolPaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new KolPaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        PartyAlias = Convert.ToString(Result["PartyAlias"]),

                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", " "),
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


        public void GetPaymentPayerMisc()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPaymentPayer", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<KolPaymentPartyName> objPaymentPartyName = new List<KolPaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new KolPaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", " "),
                        PartyAlias = Convert.ToString(Result["PartyAlias"]),

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

        public void ListOfMiscInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofMiscInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<KolListOfMiscInvoice> lstExpInvoice = new List<KolListOfMiscInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new KolListOfMiscInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExpInvoice;
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

        #region Misc. Invoice Multi Charges        
        public void AddMiscInvMultiChrg(KolMiscPostModel ObjPostPaymentSheet, string XML, int BranchId, int Uid, string Module, string ExportUnder)
        {
            string GeneratedClientId = "0";
            string Deldt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            string Invdt = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Deldt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Invdt) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_State", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.State });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Purpose });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PurposeCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PurposeCode });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Naration", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Naration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            //LstParam.Add(new MySqlParameter { ParameterName = "IN_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SACCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.IGSTPer });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CGSTPer });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SGSTPer });
            //LstParam.Add(new MySqlParameter { ParameterName = "IN_ChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ChargeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_MiscXML", MySqlDbType = MySqlDbType.Text, Value = XML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditMiscInvoiceMultiChrge", CommandType.StoredProcedure, DParam, out GeneratedClientId);
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

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice Date should be Greater than or equal to already generated Invoice Date";
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
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Amount Should Be Greater Than 0";
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
                                ArrivalTime = Convert.ToString(Result["ArrivalTime"]),
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
                    //objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").ToList().ForEach(item =>
                    //{
                    //    if (!objPostPaymentSheet.ActualApplicable.Any(i => i == item.Clause))
                    //        objPostPaymentSheet.ActualApplicable.Add(item.Clause);
                    //});

                    //HT Charges ---------------------------------------------------------------------------------------
                    DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var objGenericCharges = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                    
                    #region H&T Charges
                    try
                    {
                        var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                        var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                        var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                        var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause }); 

                        if (objPostPaymentSheet.Module == "IMPYard")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if(objPostPaymentSheet.Module == "IMPDest")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDSPS == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "IMPDeli")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDelivery == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "ECYard")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.EmptyContainerDelivery == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "ECGodn")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.EmptyContainerDelivery == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "EXP")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ExportPS == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "EXPLod")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.LoadedExport == 1).Select(o => new { Clause = o.Clause });
                        }
                        else if (objPostPaymentSheet.Module == "BTT")
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.BTT == 1).Select(o => new { Clause = o.Clause });
                        }
                        
                        else 
                        {
                            SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                        }

                        //SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportGodownDSPS == 1).Select(o => new { Clause = o.Clause });
                        //var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                        var ApplicableHT = objGenericCharges.HTChargeRent.ToList();
                        var HTCharges = 0M;
                        var TotalQuantity = 0M;
                        var RateForHT = 0M;
                        objPostPaymentSheet.lstCfsCodewiseRateHT.Clear();

                        foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                        {
                            foreach (var item1 in item.ToList())
                            {
                                if (item1.OperationCode == "5")
                                {
                                    objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                       .ToList().ForEach(c =>
                                       {
                                           objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                           {
                                               CFSCode = c.CFSCode,
                                               Clause = item1.OperationCode,
                                               CommodityType = c.CargoType.ToString(),
                                               Size = c.Size,
                                               Rate = item1.RateCWC
                                           });
                                       });
                                    HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                                }
                                else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                                {
                                    objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                       .ToList().ForEach(c =>
                                       {
                                           objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                           {
                                               CFSCode = c.CFSCode,
                                               Clause = item1.OperationCode,
                                               CommodityType = c.CargoType.ToString(),
                                               Size = c.Size,
                                               Rate = item1.RateCWC
                                           });
                                       });
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    if (item1.OperationType == 5)
                                    {
                                        objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                        .ToList().ForEach(c =>
                                        {
                                            objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                            {
                                                CFSCode = c.CFSCode,
                                                Clause = item1.OperationCode,
                                                CommodityType = c.CargoType.ToString(),
                                                Size = c.Size,
                                                Rate = item1.RateCWC
                                            });
                                        });
                                        HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                    }
                                    else if (item1.OperationType == 4)
                                    {
                                        objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                       .ToList().ForEach(c =>
                                       {
                                           objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                           {
                                               CFSCode = c.CFSCode,
                                               Clause = item1.OperationCode,
                                               CommodityType = c.CargoType.ToString(),
                                               Size = c.Size,
                                               Rate = item1.RateCWC
                                           });
                                       });
                                        HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                    }
                                    else if (item1.OperationType == 6)
                                    {
                                        objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                        .ToList().ForEach(c =>
                                        {
                                            objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                            {
                                                CFSCode = c.CFSCode,
                                                Clause = item1.OperationCode,
                                                CommodityType = c.CargoType.ToString(),
                                                Size = c.Size,
                                                Rate = item1.RateCWC
                                            });
                                        });
                                        HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                    }
                                    else if (item1.OperationType == 7)
                                    {
                                        objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                        .ToList().ForEach(c =>
                                        {
                                            objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                            {
                                                CFSCode = c.CFSCode,
                                                Clause = item1.OperationCode,
                                                CommodityType = c.CargoType.ToString(),
                                                Size = c.Size,
                                                Rate = item1.RateCWC
                                            });
                                        });
                                        HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                    }
                                    else
                                    {
                                        objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType)
                                        .ToList().ForEach(c =>
                                        {
                                            objPostPaymentSheet.lstCfsCodewiseRateHT.Add(new CfsCodewiseRateHT
                                            {
                                                CFSCode = c.CFSCode,
                                                Clause = item1.OperationCode,
                                                CommodityType = c.CargoType.ToString(),
                                                Size = c.Size,
                                                Rate = item1.RateCWC
                                            });
                                        });
                                        HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                    }
                                }
                            }
                            var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;

                            if (!objPostPaymentSheet.lstPostPaymentChrg.Any(i => i.ChargeType=="HT1" && i.Clause == item.Key))
                            {
                                objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                                {
                                    ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                                    Clause = item.Key,
                                    ChargeName = item.FirstOrDefault().OperationSDesc,
                                    ChargeType = "HT1",
                                    SACCode = item.FirstOrDefault().SacCode,
                                    Quantity = 0,
                                    Rate = 0M,// RateForHT,//
                                    Amount = HTCharges,
                                    Discount = 0,
                                    Taxable = HTCharges,
                                    CGSTPer = cgst,
                                    SGSTPer = sgst,
                                    IGSTPer = igst,
                                    CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                                    SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                                    IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                                    //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                                    Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                    (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                                    ClauseOrder = clzOrder
                                });
                            }

                            if (item.Key == "5")
                            {
                                if (!objPostPaymentSheet.lstPostPaymentChrg.Any(i => i.ChargeType == "HT1" && i.Clause == item.Key))
                                {
                                    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                                    {
                                        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                                        Clause = item.Key,
                                        ChargeName = item.FirstOrDefault().OperationSDesc,
                                        ChargeType = "HT1",
                                        SACCode = item.FirstOrDefault().SacCode,
                                        Quantity = 0,
                                        Rate = 0M,
                                        Amount = HTCharges,
                                        Discount = 0,
                                        Taxable = HTCharges,
                                        CGSTPer = cgst,
                                        SGSTPer = sgst,
                                        IGSTPer = igst,
                                        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                                        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                                        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                                        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                                        (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges
                                    });
                                }
                                
                            }
                            HTCharges = 0M;
                            TotalQuantity = 0M;
                            RateForHT = 0M;
                        }

                        //subir
                        HttpContext.Current.Session["lstCfsCodewiseRateHT"] = objPostPaymentSheet.lstCfsCodewiseRateHT;
                        //end
                        

                        if (objPostPaymentSheet.Module == "BNDUnld")
                        {
                            objPostPaymentSheet.ActualApplicable.Add("2");
                            objPostPaymentSheet.ActualApplicable.Add("6");
                            objPostPaymentSheet.ActualApplicable.Add("10A");
                            objPostPaymentSheet.ActualApplicable.Add("10B");
                        }
                        else if (objPostPaymentSheet.Module == "BND")
                        {
                            objPostPaymentSheet.ActualApplicable.Add("5");
                        }
                        else
                        {
                            var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                            actual.ForEach(item =>
                            {
                                if (objPostPaymentSheet.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                                {
                                    objPostPaymentSheet.ActualApplicable.Add(item.OperationCode);
                                }
                            });
                        }

                        //var sortedString = JsonConvert.SerializeObject(objPostPaymentSheet.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                        //objPostPaymentSheet.lstPostPaymentChrg.Clear();
                        //objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    //--------------------------------------------------------------------------------------------------
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


        public void GetInvoiceDetailsForEditBond(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForEditBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BondPostPaymentSheet objPostPaymentSheet = new BondPostPaymentSheet();
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
                    //objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                    objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                    objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                    objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                    objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                    objPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    objPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    objPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    //objPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                    //objPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                    objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                    objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                    objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                    //objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                    //objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                    //objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                    //objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                    //objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
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

        #endregion

        #region BankDeposit
        public void AddEditBankDeposit(KolBankDeposit Obj,string varXml,int Uid=0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Obj.Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.DepositDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cash", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Cash });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cheque", MySqlDbType = MySqlDbType.Decimal, Value = Obj.Cheque });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NEFT", MySqlDbType = MySqlDbType.Decimal, Value = Obj.NEFT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Xml", MySqlDbType = MySqlDbType.Text, Value = varXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBankDeposit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = "Bank Deposit Saved Successfully";
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Deposit Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = "Bank Deposit Updated Successfully";
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Bank Deposit Updated Successfully";
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

        public void DeleteBankDeposit(int Id, int Uid = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("DeleteBankDeposit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = "Bank Deposit Deleted Successfully";
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Deposit Deleted Successfully";
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

        public void GetBankDepositList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBankDepositList", CommandType.StoredProcedure, DParam);
            List<KolBankDeposit> lstBankDeposit = new List<KolBankDeposit>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBankDeposit.Add(new KolBankDeposit
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        DepositDate = Result["DepositDate"].ToString(),
                        Cash = Convert.ToDecimal(Result["Cash"]),
                        Cheque = Convert.ToDecimal(Result["Cheque"]),
                        NEFT = Convert.ToDecimal(Result["NEFT"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBankDeposit;
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

        public void GetBankDepositList(int id)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            DParam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBankDepositListById", CommandType.StoredProcedure, DParam);
            KolBankDeposit lstBankDeposit = new KolBankDeposit();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstBankDeposit.Id = Convert.ToInt32(Result["Id"]);
                    lstBankDeposit.DepositDate = Result["DepositDate"].ToString();
                    lstBankDeposit.Cash = Convert.ToDecimal(Result["Cash"]);
                    lstBankDeposit.Cheque = Convert.ToDecimal(Result["Cheque"]);
                    lstBankDeposit.NEFT = Convert.ToDecimal(Result["NEFT"]);

                   
                }
                if(Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstBankDeposit.ExpensesDetails.Add(new Expenses
                        {
                            VoucherNo = Convert.ToString(Result["VoucherNo"]),
                            ReceiptId = Convert.ToInt32(Result["ReceiptId"]),
                            RefundAmount = Convert.ToDecimal(Result["ReturnAmount"]),
                            id = Convert.ToInt32(Result["BankId"])

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBankDeposit;
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



        public void GetNEFTForBankDeposit(DateTime dt)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = dt });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetNEFTForBankDeposit", CommandType.StoredProcedure, DParam);
            //List<KolBankDeposit> lstBankDeposit = new List<KolBankDeposit>();
            KolBankDeposit obj = new KolBankDeposit();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj.NEFT = Convert.ToDecimal(Result["NEFT"]);
                    
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

        public void ExpenseBankDeposit()
        {
            //string GeneratedClientId = "0";
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<ExpensesDetails> LstExpense = new List<ExpensesDetails>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExpenseHeadWiseBalance", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstExpense.Add(new ExpensesDetails
                    {
                        ExpId = Convert.ToInt32(Result["HeadId"]),
                        ExpenseHead = Convert.ToString(Result["HeadName"]),
                        Amount = Convert.ToDecimal(Result["Amount"])

                    });



                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstExpense;
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

        public void ReceiptVoucherBalance(string HeadId, string DSNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_HeadId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HeadId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DSNo", MySqlDbType = MySqlDbType.VarChar, Value = DSNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmountReceiptVoucherWise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<KolVoucherHead> lstReceiptVoucher = new List<KolVoucherHead>();
            while (Result.Read())
            {

                Status = 1;
                lstReceiptVoucher.Add(new KolVoucherHead
                {
                    ReceiptId = Convert.ToInt32(Result["ReceiptId"]),
                    ExpenseId = Convert.ToInt32(Result["ExpenseId"]),
                    VoucherNo = Convert.ToString(Result["VoucherNo"]),
                    BalanceAmount = Convert.ToDecimal(Result["BalanceAmount"])
                });

            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceiptVoucher;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

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
        #region BulkDebit/CreaditNote
        public void PrintDetailsForBulkCRNote(string PeriodFrom, string PeriodTo, string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfBulkDRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();


            int Status = 0;
            _DBResponse = new DatabaseResponse();
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
                        Status = 1;
                        objCR.lstCrParty.Add(new PrintModelOfBulkCrParty
                        {
                            PartyName = Result["PartyName"].ToString(),
                            PartyAddress = Result["PartyAddress"].ToString(),
                            PartyCityName = Result["PartyCityName"].ToString(),
                            PartyStateName = Result["PartyStateName"].ToString(),
                            PartyStateCode = Result["PartyStateCode"].ToString(),
                            PartyGSTIN = Result["PartyGSTIN"].ToString(),
                            CRNoteNo = Result["CRNoteNo"].ToString(),
                            CRNoteDate = Result["CRNoteDate"].ToString(),
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            InvoiceDate = Result["InvoiceDate"].ToString(),
                            TotalAmt = Convert.ToDecimal(Result["TotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            GrandTotal = Convert.ToDecimal(Result["GrandTotal"]),
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                            Remarks = Result["Remarks"].ToString(),
                            irn = Result["irn"].ToString(),
                            SignedQRCode = Result["SignedQRCode"].ToString(),
                            SupplyType = Result["SupplyType"].ToString(),
                        });

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
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
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
        public void PrintDetailsForPortwise(string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PortWiseReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PortWiseReport SDResult = new PortWiseReport();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PortWiseReportList.Add(new PortWiseReport
                    {

                        SlaCode = Result["SLACode"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        RLoni = Convert.ToInt32(Result["RLoni"]),
                        ROth = Convert.ToInt32(Result["ROth"]),
                        RTkd = Convert.ToInt32(Result["RTkd"]),
                        TLoni = Convert.ToInt32(Result["TLoni"]),
                        TOth = Convert.ToInt32(Result["TOth"]),
                        TTkd = Convert.ToInt32(Result["TTkd"]),

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        if (Result["ContainerLoadType"].ToString() == "FCL")
                        {
                            SDResult.TotalFCL = Convert.ToInt32(Result["ContainerLoadTypeCount"]);
                        }
                        else if (Result["ContainerLoadType"].ToString() == "LCL")
                        {
                            SDResult.TotalLCL = Convert.ToInt32(Result["ContainerLoadTypeCount"]);
                        }



                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = SDResult;
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

        #region Direct Online Payment
        public void AddDirectPaymentVoucher(Kol_DirectOnlinePayment objDOP, int uid)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlinePayAckId", MySqlDbType = MySqlDbType.Int32, Value = objDOP.OnlinePayAckId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransId", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TransId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = objDOP.OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.PaymentAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlineFacilitationCharges", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.OnlineFacilitationCharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TotalPaymentAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Response", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Area });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOnlinePayACK", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "User not an SD Party!";
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
        public void GetOnlinePayACK(long OrderId, int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePayACK", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PaymentGatewayRequest objPayRequest = new PaymentGatewayRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objPayRequest.billing_name = Convert.ToString(Result["billing_name"]);
                    objPayRequest.billing_address = Convert.ToString(Result["billing_address"]);
                    objPayRequest.billing_city = Convert.ToString(Result["billing_city"]);
                    objPayRequest.billing_state = Convert.ToString(Result["billing_state"]);
                    objPayRequest.billing_zip = Convert.ToString(Result["billing_zip"]);
                    objPayRequest.billing_country = Convert.ToString(Result["billing_country"]);
                    objPayRequest.billing_tel = Convert.ToDecimal(Result["billing_tel"]);
                    objPayRequest.billing_email = Convert.ToString(Result["billing_email"]);
                    objPayRequest.tid = Convert.ToDecimal(Result["tid"]);
                    objPayRequest.order_id = Convert.ToInt64(Result["order_id"]);
                    objPayRequest.amount = Convert.ToDecimal(Result["amount"]);
                    objPayRequest.merchant_id = Convert.ToDecimal(Result["merchant_id"]);
                    objPayRequest.cancel_url = Convert.ToString(Result["ccavenueCancelURL"]);
                    objPayRequest.redirect_url = Convert.ToString(Result["ccavenueRedirectURL"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPayRequest;
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
        public void AddPaymentGatewayRequest(PaymentGatewayRequest objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tid", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.tid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_merchant_id", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.merchant_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.Int64, Value = objPGR.order_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.currency });
            LstParam.Add(new MySqlParameter { ParameterName = "in_redirect_url", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.redirect_url });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cancel_url", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.cancel_url });
            LstParam.Add(new MySqlParameter { ParameterName = "in_language", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.language });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.delivery_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_issuing_bank", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.issuing_bank });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mobile_number", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.mobile_number });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Request Saved Successfully-" + GeneratedClientId;
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

        public void AddPaymentGatewayResponse(PaymentGatewayResponse objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.tracking_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.Int64, Value = objPGR.order_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.bank_ref_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.failure_message });
            LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.payment_mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = objPGR.status_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.status_message });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.currency });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.billing_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.delivery_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.vault });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.offer_type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.offer_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.discount_value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.mer_amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.eci_value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.retry });
            LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.response_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.trans_date });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.bin_country });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponse", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
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

        public void GetOnlinePayAckList(int uid, long OrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePayAckList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_DirectOnlinePayment> objDOPList = new List<Kol_DirectOnlinePayment>();
            Kol_DirectOnlinePayment objDOP = new Kol_DirectOnlinePayment();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDOP = new Kol_DirectOnlinePayment();
                    objDOP.OrderId = Convert.ToInt64(Result["OrderId"]);
                    objDOP.PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]);
                    objDOP.OnlineFacilitationCharges = Convert.ToDecimal(Result["OnlineFacilitationCharges"]);
                    objDOP.TotalPaymentAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]);
                    objDOP.Response = Convert.ToString(Result["Response"]);
                    objDOP.CreatedOn = Convert.ToString(Result["TransDate"]);

                    objDOPList.Add(objDOP);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDOPList;
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


        public void GetOnlineConfirmPayment(decimal TotalAmt, long OrderId)
        {
            int Status = 0;
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ConfirmDirectOnlinePayment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            List<Kol_DirectOnlinePayment> objDOPList = new List<Kol_DirectOnlinePayment>();
            Kol_DirectOnlinePayment objDOP = new Kol_DirectOnlinePayment();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = GeneratedClientId;

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

        #region Online Payment Receipt
        public void OnlinePaymentReceiptDetails(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlinePaymentReceiptList(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

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
        #endregion

        #region Online Payment Against Invoice
        public void ListOfPendingInvoice(int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPendingInvoiceOnlinePtm", CommandType.StoredProcedure, DParam);
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
                        Amount = Convert.ToDecimal(Result["InvoiceAmt"]),
                        InvoiceDate = (Result["InvoiceDate"]).ToString(),
                        PayeeName = (Result["PayeeId"]).ToString(),
                        PartyName = (Result["PartyId"]).ToString()


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

        public void AddEditOnlinePaymentAgainstInvoice(Kol_OnlinePaymentAgainstInvoice objDOP, int uid, string XML)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlinePayAckId", MySqlDbType = MySqlDbType.Int32, Value = objDOP.OnlinePayAckId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransId", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TransId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = objDOP.OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlineFacilitationCharges", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.OnlineFacilitaionCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TotalPayAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Response", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceXML", MySqlDbType = MySqlDbType.LongText, Value = XML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOnlinePayInvoiceAgainst", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Invalid Amount..";
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

        public void GetOnlinePaymentAgainstInvoice(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentInvoiceAgainstList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_OnlinePaymentAgainstInvoice> lstCashReceipt = new List<Kol_OnlinePaymentAgainstInvoice>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Kol_OnlinePaymentAgainstInvoice
                    {
                        OrderId = Convert.ToInt64(Result["OrderId"]),
                        TotalPayAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]),
                        Response = Convert.ToString(Result["Response"]),
                        lstInvoiceDetails = Convert.ToString(Result["InvoiceNo"]),
                        OnlinePayAckId = Convert.ToInt32(Result["OnlinePayAckId"])

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
        #endregion

        #region Online Payment Receipt Against Invoice
        public void OnlinePaymentReceiptDetailsAgainstInvoice(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptDetailsAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlinePaymentReceiptListAgainstInvoice(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptListAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

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
        #endregion

        #region Online On Account Payment Receipt
        public void OnlineOAPaymentReceiptDetails(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlineOnAccountPaymentReceipt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlineOAPaymentReceiptList(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlineOAPaymentReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

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
        #endregion

        #region TransactionStatusEnquiry


        public void GetInvoiceNoForTransactionStatusEnquiry(string InvoiceNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForTransactionStatusEnquiry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<TransactionStatusEnquiry> lstInvoice = new List<TransactionStatusEnquiry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new TransactionStatusEnquiry()
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

        public void GetQRRequestDetails()
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetQRRequestDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
            }
        }

        public void AddQRTransactionAck(string JSON_XML, int InvoiceId)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_JSON_XML", MySqlDbType = MySqlDbType.LongText, Value = JSON_XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddQRTransactionAck", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
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



        public void GetOrderNoForTransactionStatusEnquiry(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOrderNoByInvoiceId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            string OiderNo = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    OiderNo = Convert.ToString(Result["OrderId"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OiderNo;
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


        public void AddCcAvnueTransactionUpdate(CcAvnResponseJsonModel vm)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_amt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.order_amt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_date_time", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(vm.order_date_time) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_name", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_bill_name) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_address", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_bill_address) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_zip", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_zip) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_tel", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_tel) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_email", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_email) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_ip", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ip) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_card_name", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_card_name) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_device_type", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_device_type) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_no", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_no) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_reference_no", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.reference_no) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_enquiry_by", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCCAvenueTransactionStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry ";
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


        public void CheckResponceOrderIdAndAmount(string OrderId, decimal Amount)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(OrderId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToString(Amount) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("CheckOrderIdAndAmount", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
                    _DBResponse.Data = Result;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = Result;
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


        #region Bulk Cash Receipt For Online

        public void GenericBulkInvoiceDetailsForPrint(string ReceiptNo)
        {
            
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ReceiptNo });
           
            try
            {
                DParam = LstParam.ToArray();
               
                    Result = DataAccess.ExecuteDataSet("GetCshDetailsAgnstInvoiceOnline", CommandType.StoredProcedure, DParam);
               
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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


        #region Acknowledgement View List
        public void AcknowledgementViewList(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = FromDate == null ? null : Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDate == null ? null : Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAcknowledgementViewList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kol_AcknowledgementViewList> lstReceipt = new List<Kol_AcknowledgementViewList>();


            try
            {
                while (Result.Read())
                {

                    Status = 1;

                    lstReceipt.Add(new Kol_AcknowledgementViewList
                    {
                        // PeriodFrom=Convert.ToString(Result["FromDate"])
                        TransId = Convert.ToString(Result["TransId"]),
                        OrderId = Convert.ToString(Result["OrderId"]),
                        OnlineFacilitationCharges = Convert.ToDecimal(Result["OnlineFacilitationCharges"]),
                        PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]),
                        TotalPaymentAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]),
                        Response = Convert.ToString(Result["Response"]),
                        TransactionDate = Convert.ToString(Result["TransactionDate"]),
                        BankRef = Convert.ToString(Result["reference_no"]),
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
                Result.Close();
                Result.Dispose();
            }

        }
        #endregion

        #region Pull
        public void BQRPaymentReceiptPullData()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBRQInvoiceStatusList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {


                    ReferenceNo = Convert.ToString(Result["InvoiceId"]),

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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }


        public void AddPaymentGatewayResponseBQR(CcAvnResponseJsonModel objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.reference_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_ref_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_option_type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_amt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_currncy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_capt_amt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.order_date_time });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponseTranBqr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
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

        #region Pull CCAvenue
        public void CCAvenuePaymentReceiptPullData()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCAvenueOrderStatusList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {


                    ReferenceNo = Convert.ToString(Result["OrderId"]),

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
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }


        public void AddPaymentGatewayResponseCCAvenue(CcAvnResponseJsonModel objPGR)
        {
            try
            {
                log.Error("Start Saving CCAvenue Pull  :");
                string GeneratedClientId = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.reference_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_ref_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
                LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
                LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_option_type });
                LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_response });
                LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_amt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_currncy });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_address });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_city });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_state });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_zip });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_tel });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_email });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_address });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_city });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_state });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_zip });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_country });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_tel });
                LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_capt_amt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
                LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.order_date_time });
                LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });


                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponse", CommandType.StoredProcedure, DParam, out GeneratedClientId);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Start Saving CCAvenue Error  :" + ex.StackTrace);
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            log.Error("Start Saving CCAvenue END  :");
        }
        #endregion
    }
}